using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

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
		Object loggerLock;
		AutoResetEvent terminateThreadEvent;

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
			loggerLock = new Object();
			terminateThreadEvent = new AutoResetEvent(false);
		}

		public void Run()
		{
			Application.Run(mainForm);
		}

		void Print(string text)
		{
			mainForm.logTextBox.Invoke
			(
				(MethodInvoker)delegate
				{
					mainForm.logTextBox.AppendText(text);
					SendMessage(mainForm.logTextBox.Handle, WM_VSCROLL, SB_BOTTOM, 0);
				}
			);
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
			SerialiseConfiguration();
			SerialiseHistory();
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
			bool terminateThread = false;
			lock (loggerLock)
			{
				if (running)
				{
					running = false;
					terminateThread = true;
				}
			}
			if (terminateThread)
				loggingThread.Join();
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
					lock (loggerLock)
					{
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
					if(doProcessLog)
						ProcessLog(path, status, size);
				}
				catch (IOException exception)
				{
					PrintLine("An error occurred during the processing of \"" + path + "\": " + exception.Message);
				}
			}
		}

		void ProcessLog(string path, LogStatus status, long fileSize)
		{
			StreamReader reader = new StreamReader(path);
			reader.BaseStream.Seek(status.offset, SeekOrigin.Begin);
			int bufferSize = (int)(fileSize - status.offset);
			char[] buffer = new char[bufferSize];
			reader.Read(buffer, 0, bufferSize);
			reader.Close();
			string contents = new string(buffer);
			ProcessLogContents(path, contents);
		}

		void ProcessLogContents(string path, string contents)
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
			}
		}

		void RunLogger()
		{
			while (true)
			{
				lock(loggerLock)
				{
					if (!running)
						break;
					ProcessLogs();
				}
				lock (loggerLock)
					SerialiseHistory();
				terminateThreadEvent.WaitOne(pollingDelay);
			}
		}
	}
}
