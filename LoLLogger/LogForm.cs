using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoLLogs
{
	public partial class LogForm : Form
	{
		Logger ParentLogger;

		public LogForm(Logger logger)
		{
			ParentLogger = logger;
			InitializeComponent();
		}

		private void LogForm_Load(object sender, EventArgs e)
		{
			ParentLogger.OnLoad();
		}

		private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			ParentLogger.OnClosing();
		}

		private void configurationButton_Click(object sender, EventArgs e)
		{
			ParentLogger.ShowConfigurationDialogue();
		}
	}
}
