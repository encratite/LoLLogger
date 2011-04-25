namespace LoLLogs
{
	partial class LogForm
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
			this.logTextBox = new System.Windows.Forms.RichTextBox();
			this.configurationButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// logTextBox
			// 
			this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.logTextBox.BackColor = System.Drawing.Color.White;
			this.logTextBox.Location = new System.Drawing.Point(0, 31);
			this.logTextBox.Name = "logTextBox";
			this.logTextBox.ReadOnly = true;
			this.logTextBox.Size = new System.Drawing.Size(521, 242);
			this.logTextBox.TabIndex = 0;
			this.logTextBox.Text = "";
			// 
			// configurationButton
			// 
			this.configurationButton.Location = new System.Drawing.Point(5, 4);
			this.configurationButton.Name = "configurationButton";
			this.configurationButton.Size = new System.Drawing.Size(85, 23);
			this.configurationButton.TabIndex = 1;
			this.configurationButton.Text = "Configure";
			this.configurationButton.UseVisualStyleBackColor = true;
			this.configurationButton.Click += new System.EventHandler(this.configurationButton_Click);
			// 
			// logForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(521, 273);
			this.Controls.Add(this.configurationButton);
			this.Controls.Add(this.logTextBox);
			this.Name = "logForm";
			this.Text = "League of Legends Log Uploader";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogForm_FormClosing);
			this.Load += new System.EventHandler(this.LogForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.RichTextBox logTextBox;
		private System.Windows.Forms.Button configurationButton;
	}
}

