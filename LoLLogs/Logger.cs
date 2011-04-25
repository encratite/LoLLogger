using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LoLLogs
{
	public class Logger
	{
		Configuration LoggerConfiguration;
		Nil.Serialiser<Configuration> ConfigurationSerialiser;

		LogHistory History;
		Nil.Serialiser<LogHistory> HistorySerialiser;

		static string ConfigurationPath = "Configuration.xml";
		static string HistoryPath = "History.xml";

		public LogForm MainForm;
		ConfigurationForm LoggerConfigurationForm;

		const int WM_VSCROLL = 0x115;
		const int SB_BOTTOM = 7;

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr window, int message, int wparam, int lparam);

		public Logger()
		{
			MainForm = new LogForm(this);
			LoggerConfigurationForm = new ConfigurationForm(this);
			ConfigurationSerialiser = new Nil.Serialiser<Configuration>(ConfigurationPath);
			HistorySerialiser = new Nil.Serialiser<LogHistory>(HistoryPath);
		}

		public void Run()
		{
			Application.Run(MainForm);
		}

		void Print(string text)
		{
			MainForm.logTextBox.Invoke
			(
				(MethodInvoker)delegate
				{
					MainForm.logTextBox.AppendText(text);
					SendMessage(MainForm.logTextBox.Handle, WM_VSCROLL, SB_BOTTOM, 0);
				}
			);
		}

		void PrintLine(string text)
		{
			Print(text + "\n");
		}

		public void OnLoad()
		{
			try
			{
				History = HistorySerialiser.Load();
			}
			catch (IOException)
			{
				// there is no history file yet - start with a blank history
				PrintLine("No history file was found - starting with a blank one");
			}
			try
			{
				LoggerConfiguration = ConfigurationSerialiser.Load();
			}
			catch (IOException)
			{
				// there is no configuration file yet
				PrintLine("Unable to find configuration file " + ConfigurationPath + " - opening the configuration dialogue");
				ShowConfigurationDialogue();
			}
		}

		public void OnClosing()
		{
			SerialiseConfiguration();
			SerialiseHistory();
		}

		public void ShowConfigurationDialogue()
		{
			LoggerConfigurationForm.ShowDialog();
		}

		public void SaveConfiguration(string directory, string server, int port)
		{
			LoggerConfiguration = new Configuration();
			LoggerConfiguration.LoLDirectory = directory;
			LoggerConfiguration.LogServer = server;
			LoggerConfiguration.LogServerPort = port;
			SerialiseConfiguration();
		}

		void SerialiseConfiguration()
		{
			ConfigurationSerialiser.Store(LoggerConfiguration);
		}

		void SerialiseHistory()
		{
			HistorySerialiser.Store(History);
		}
	}
}
