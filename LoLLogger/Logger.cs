using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;

namespace LoLLogs
{
	public class Logger
	{
		Configuration loggerConfiguration;
		Nil.Serialiser<Configuration> configurationSerialiser;

		LogHistory history;
		Nil.Serialiser<LogHistory> historySerialiser;

		const string configurationPath = "Configuration.xml";
		const string historyPath = "History.xml";

		public LogForm mainForm;
		ConfigurationForm loggerConfigurationForm;

		Thread loggingThread;
		bool running;
		AutoResetEvent terminateThreadEvent;

		TcpClient networkClient;

		const int WM_VSCROLL = 0x115;
		const int SB_BOTTOM = 7;

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr window, int message, int wparam, int lparam);

		const int pollingDelay = 5000;

		public Logger()
		{
			mainForm = new LogForm(this);
			loggerConfigurationForm = new ConfigurationForm(this);
			configurationSerialiser = new Nil.Serialiser<Configuration>(configurationPath);
			historySerialiser = new Nil.Serialiser<LogHistory>(historyPath);
			running = false;
			terminateThreadEvent = new AutoResetEvent(false);
		}

		public void Run()
		{
			Application.Run(mainForm);
		}

		void Print(string text)
		{
			try
			{
				mainForm.logTextBox.BeginInvoke
				(
					(MethodInvoker)delegate
					{
						mainForm.logTextBox.AppendText(text);
						SendMessage(mainForm.logTextBox.Handle, WM_VSCROLL, SB_BOTTOM, 0);
					}
				);
			}
			catch (InvalidOperationException)
			{
				// this is thrown when the application is currently closing
				// with Invoke it just deadlocks then instead
				// I am not happy with this at all
			}
		}

		void PrintLine(string text)
		{
			Print(Nil.Time.Timestamp() + " " + text + "\n");
		}

		public void OnLoad()
		{
			try
			{
				history = historySerialiser.Load();
			}
			catch (IOException)
			{
				// there is no history file yet - start with a blank history
				PrintLine("No history file was found - starting with a blank one");
				history = new LogHistory();
			}
			try
			{
				loggerConfiguration = configurationSerialiser.Load();
			}
			catch (IOException)
			{
				// there is no configuration file yet
				PrintLine("Unable to find configuration file " + configurationPath + " - opening the configuration dialogue");
				ShowConfigurationDialogue();
			}
			StartLogging();
		}

		public void OnClosing()
		{
			if(networkClient != null)
				networkClient.Close();
			StopLogging();
		}

		public void ShowConfigurationDialogue()
		{
			loggerConfigurationForm.ShowDialog();
		}

		public void SaveConfiguration(string directory, string server, int port)
		{
			loggerConfiguration = new Configuration();
			loggerConfiguration.lolDirectory = directory;
			loggerConfiguration.logServer = server;
			loggerConfiguration.logServerPort = port;
			PrintLine("Configuration has been saved");
			SerialiseConfiguration();
		}

		public void InitialiseConfigurationForm()
		{
			if (loggerConfiguration != null)
				loggerConfigurationForm.SetFields(loggerConfiguration.lolDirectory, loggerConfiguration.logServer, loggerConfiguration.logServerPort);
		}

		void SerialiseConfiguration()
		{
			configurationSerialiser.Store(loggerConfiguration);
		}

		void SerialiseHistory()
		{
			historySerialiser.Store(history);
		}

		void StopLogging()
		{
			if (running)
			{
				lock(history)
					running = false;
				terminateThreadEvent.Set();
				loggingThread.Join();
				terminateThreadEvent.Reset();
			}
		}

		void StartLogging()
		{
			StopLogging();
			loggingThread = new Thread(() =>
				{
					RunLogger();
				}
			);
			running = true;
			loggingThread.Start();
		}

		string GetLogDirectory()
		{
			return Path.Combine(loggerConfiguration.lolDirectory, "air", "logs");
		}

		void ProcessLogs()
		{
			string[] logs;
			string directory = GetLogDirectory();
			try
			{
				logs = Directory.GetFiles(directory);
			}
			catch (IOException)
			{
				PrintLine("Unable to read directory \"" + directory + "\". Please check your League of Legends directory setting in the configuration dialogue.");
				return;
			}

			foreach (string path in logs)
			{
				try
				{
					FileInfo information = new FileInfo(path);
					long size = information.Length;
					LogStatus status;
					bool doProcessLog = false;
					lock (history)
					{
						if (!running)
							break;
						if (history.logMap.ContainsKey(path))
						{
							status = history.logMap[path];
							if (status.LogHasChanged(size))
							{
								// there is new data to be parsed
								doProcessLog = true;
							}
						}
						else
						{
							// it's a new file which is not part of the history yet
							status = new LogStatus();
							history.logMap.Add(path, status);
							doProcessLog = true;
						}
					}
					if (doProcessLog)
					{
						bool noNetworkErrorOccurred = ProcessLog(path, status, size);
						if (!noNetworkErrorOccurred)
							break;
					}
				}
				catch (IOException exception)
				{
					PrintLine("An error occurred during the processing of \"" + path + "\": " + exception.Message);
				}

				lock (history)
				{
					if (!running)
						break;
					SerialiseHistory();
				}
			}
		}

		bool ProcessLog(string path, LogStatus status, long fileSize)
		{
			StreamReader reader = new StreamReader(path);
			reader.BaseStream.Seek(status.offset, SeekOrigin.Begin);
			int bufferSize = (int)(fileSize - status.offset);
			char[] buffer = new char[bufferSize];
			reader.Read(buffer, 0, bufferSize);
			reader.Close();
			string contents = new string(buffer);
			if (!running)
				return true;
			bool noNetworkErrorOccurred = ProcessLogContents(path, status, contents);
			return noNetworkErrorOccurred;
		}

		// returns false when a network error occurred
		bool ProcessLogContents(string path, LogStatus status, string contents)
		{
			string beginningMarker = "  body = (com.riotgames.platform.gameclient.domain::EndOfGameStats)#1";
			string endMarker = "timeToLive = 0";
			for (int offset = 0; offset < contents.Length; )
			{
				int beginningOffset = contents.IndexOf(beginningMarker, offset);
				if (beginningOffset == -1)
					break;
				int endOffset = contents.IndexOf(endMarker, beginningOffset);
				if (endOffset == -1)
				{
					PrintLine("Encountered an incomplete end of game stats section in \"" + path + "\"");
					break;
				}
				string endOfGameStats = contents.Substring(beginningOffset, endOffset - beginningOffset);
				offset = endOffset + endMarker.Length;
				PrintLine("Discovered stats for a game of size " + endOfGameStats.Length.ToString() + " in file \"" + path + "\"");
				if (!TransmitContents(contents))
					return false;
			}
			lock (history)
			{
				if (!running)
					return true;
				status.offset = contents.Length;
			}
			return true;
		}

		void RunLogger()
		{
			while (true)
			{
				if (!running)
					break;
				ProcessLogs();
				SerialiseHistory();
				terminateThreadEvent.WaitOne(pollingDelay);
			}
		}

		bool TransmitContents(string contents)
		{
			string packet = contents.Length.ToString() + ":" + contents;
			if (networkClient == null)
			{
				PrintLine("Connecting to " + loggerConfiguration.logServer + ":" + loggerConfiguration.logServerPort.ToString() + " to upload the game stats");
				try
				{
					networkClient = new TcpClient(loggerConfiguration.logServer, loggerConfiguration.logServerPort);
				}
				catch (SocketException exception)
				{
					PrintLine("Unable to connect to the server: " + exception.Message);
					return false;
				}
			}
			try
			{
				NetworkStream stream = networkClient.GetStream();
				StreamWriter writer = new StreamWriter(stream);
				writer.Write(packet);
				writer.Flush();
			}
			catch (SocketException exception)
			{
				PrintLine("Unable to upload the stats to the server: " + exception.Message);
				networkClient = null;
				return false;
			}
			return true;
		}
	}
}
