namespace LoLLogs
{
	partial class ConfigurationForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.LoLFolderLabel = new System.Windows.Forms.Label();
			this.lolFolderTextBox = new System.Windows.Forms.TextBox();
			this.loggingServerAddressLabel = new System.Windows.Forms.Label();
			this.loggingServerAddressTextBox = new System.Windows.Forms.TextBox();
			this.loggingServerPortLabel = new System.Windows.Forms.Label();
			this.loggingServerPortTextBox = new System.Windows.Forms.TextBox();
			this.chooseLoLFolderButton = new System.Windows.Forms.Button();
			this.saveConfigurationButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.lolDirectoryBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.SuspendLayout();
			// 
			// LoLFolderLabel
			// 
			this.LoLFolderLabel.AutoSize = true;
			this.LoLFolderLabel.Location = new System.Drawing.Point(13, 9);
			this.LoLFolderLabel.Name = "LoLFolderLabel";
			this.LoLFolderLabel.Size = new System.Drawing.Size(131, 13);
			this.LoLFolderLabel.TabIndex = 0;
			this.LoLFolderLabel.Text = "League of Legends folder:";
			// 
			// lolFolderTextBox
			// 
			this.lolFolderTextBox.Location = new System.Drawing.Point(187, 6);
			this.lolFolderTextBox.Name = "lolFolderTextBox";
			this.lolFolderTextBox.Size = new System.Drawing.Size(355, 20);
			this.lolFolderTextBox.TabIndex = 1;
			// 
			// loggingServerAddressLabel
			// 
			this.loggingServerAddressLabel.AutoSize = true;
			this.loggingServerAddressLabel.Location = new System.Drawing.Point(13, 37);
			this.loggingServerAddressLabel.Name = "loggingServerAddressLabel";
			this.loggingServerAddressLabel.Size = new System.Drawing.Size(147, 13);
			this.loggingServerAddressLabel.TabIndex = 2;
			this.loggingServerAddressLabel.Text = "Address of the logging server:";
			// 
			// loggingServerAddressTextBox
			// 
			this.loggingServerAddressTextBox.Location = new System.Drawing.Point(187, 34);
			this.loggingServerAddressTextBox.Name = "loggingServerAddressTextBox";
			this.loggingServerAddressTextBox.Size = new System.Drawing.Size(200, 20);
			this.loggingServerAddressTextBox.TabIndex = 3;
			// 
			// loggingServerPortLabel
			// 
			this.loggingServerPortLabel.AutoSize = true;
			this.loggingServerPortLabel.Location = new System.Drawing.Point(403, 37);
			this.loggingServerPortLabel.Name = "loggingServerPortLabel";
			this.loggingServerPortLabel.Size = new System.Drawing.Size(29, 13);
			this.loggingServerPortLabel.TabIndex = 4;
			this.loggingServerPortLabel.Text = "Port:";
			// 
			// loggingServerPortTextBox
			// 
			this.loggingServerPortTextBox.Location = new System.Drawing.Point(449, 34);
			this.loggingServerPortTextBox.Name = "loggingServerPortTextBox";
			this.loggingServerPortTextBox.Size = new System.Drawing.Size(93, 20);
			this.loggingServerPortTextBox.TabIndex = 5;
			this.loggingServerPortTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.loggingServerPortTextBox_KeyPress);
			// 
			// chooseLoLFolderButton
			// 
			this.chooseLoLFolderButton.Location = new System.Drawing.Point(561, 6);
			this.chooseLoLFolderButton.Name = "chooseLoLFolderButton";
			this.chooseLoLFolderButton.Size = new System.Drawing.Size(83, 20);
			this.chooseLoLFolderButton.TabIndex = 6;
			this.chooseLoLFolderButton.Text = "Browse";
			this.chooseLoLFolderButton.UseVisualStyleBackColor = true;
			this.chooseLoLFolderButton.Click += new System.EventHandler(this.chooseLoLFolderButton_Click);
			// 
			// saveConfigurationButton
			// 
			this.saveConfigurationButton.Location = new System.Drawing.Point(461, 76);
			this.saveConfigurationButton.Name = "saveConfigurationButton";
			this.saveConfigurationButton.Size = new System.Drawing.Size(81, 22);
			this.saveConfigurationButton.TabIndex = 7;
			this.saveConfigurationButton.Text = "Ok";
			this.saveConfigurationButton.UseVisualStyleBackColor = true;
			this.saveConfigurationButton.Click += new System.EventHandler(this.saveConfigurationButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(561, 76);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(81, 22);
			this.cancelButton.TabIndex = 8;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// lolDirectoryBrowser
			// 
			this.lolDirectoryBrowser.Description = "Select the League of Legends directory";
			this.lolDirectoryBrowser.RootFolder = System.Environment.SpecialFolder.MyComputer;
			this.lolDirectoryBrowser.ShowNewFolderButton = false;
			// 
			// ConfigurationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(649, 101);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.saveConfigurationButton);
			this.Controls.Add(this.chooseLoLFolderButton);
			this.Controls.Add(this.loggingServerPortTextBox);
			this.Controls.Add(this.loggingServerPortLabel);
			this.Controls.Add(this.loggingServerAddressTextBox);
			this.Controls.Add(this.loggingServerAddressLabel);
			this.Controls.Add(this.lolFolderTextBox);
			this.Controls.Add(this.LoLFolderLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "ConfigurationForm";
			this.Text = "Configure logger";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label LoLFolderLabel;
		private System.Windows.Forms.TextBox lolFolderTextBox;
		private System.Windows.Forms.Label loggingServerAddressLabel;
		private System.Windows.Forms.TextBox loggingServerAddressTextBox;
		private System.Windows.Forms.Label loggingServerPortLabel;
		private System.Windows.Forms.TextBox loggingServerPortTextBox;
		private System.Windows.Forms.Button chooseLoLFolderButton;
		private System.Windows.Forms.Button saveConfigurationButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.FolderBrowserDialog lolDirectoryBrowser;
	}
}