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
	public partial class ConfigurationForm : Form
	{
		Logger ParentLogger;

		public ConfigurationForm(Logger logger)
		{
			ParentLogger = logger;
			InitializeComponent();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void saveConfigurationButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void loggingServerPortTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			// make sure that only valid ports are entered
			if (e.KeyChar == '\b')
				return;
			int number = 0;
			string input = loggingServerPortTextBox.Text + e.KeyChar.ToString();
			bool successfulConversion = int.TryParse(input, out number);
			bool isGoodNumber;
			if (successfulConversion)
				isGoodNumber = number >= 1 && number <= 65535;
			else
				isGoodNumber = false;
			e.Handled = !isGoodNumber;
		}
	}
}
