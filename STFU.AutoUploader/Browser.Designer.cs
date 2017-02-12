﻿namespace STFU.AutoUploader
{
	partial class Browser
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
			this.WebBrowser = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// WebBrowser
			// 
			this.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WebBrowser.IsWebBrowserContextMenuEnabled = false;
			this.WebBrowser.Location = new System.Drawing.Point(0, 0);
			this.WebBrowser.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.WebBrowser.MinimumSize = new System.Drawing.Size(15, 16);
			this.WebBrowser.Name = "WebBrowser";
			this.WebBrowser.ScriptErrorsSuppressed = true;
			this.WebBrowser.Size = new System.Drawing.Size(649, 623);
			this.WebBrowser.TabIndex = 0;
			this.WebBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.WebBrowserNavigated);
			// 
			// Browser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(649, 623);
			this.Controls.Add(this.WebBrowser);
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "Browser";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Browser";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BrowserFormClosing);
			this.Load += new System.EventHandler(this.BrowserLoad);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser WebBrowser;
	}
}