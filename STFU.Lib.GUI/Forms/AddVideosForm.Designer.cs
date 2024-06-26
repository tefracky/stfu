﻿namespace STFU.Lib.GUI.Forms
{
	partial class AddVideosForm
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
			this.mainTlp = new System.Windows.Forms.TableLayoutPanel();
			this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.videosListView = new System.Windows.Forms.ListView();
			this.dateiNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.addVideosButton = new System.Windows.Forms.Button();
			this.moveVideosUpButton = new System.Windows.Forms.Button();
			this.moveVideosDownButton = new System.Windows.Forms.Button();
			this.removeVideoButton = new System.Windows.Forms.Button();
			this.clearVideosButton = new System.Windows.Forms.Button();
			this.insertTemplatesButton = new System.Windows.Forms.Button();
			this.editVideoInformationGrid = new STFU.Lib.GUI.Controls.EditVideoGrid();
			this.acceptButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.loadWorker = new System.ComponentModel.BackgroundWorker();
			this.addVideosWorker = new System.ComponentModel.BackgroundWorker();
			this.addVideosDialog = new System.Windows.Forms.OpenFileDialog();
			this.mainTlp.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
			this.mainSplitContainer.Panel1.SuspendLayout();
			this.mainSplitContainer.Panel2.SuspendLayout();
			this.mainSplitContainer.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTlp
			// 
			this.mainTlp.ColumnCount = 7;
			this.mainTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.mainTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.mainTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mainTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.mainTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mainTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.mainTlp.Controls.Add(this.mainSplitContainer, 1, 1);
			this.mainTlp.Controls.Add(this.acceptButton, 3, 3);
			this.mainTlp.Controls.Add(this.cancelButton, 5, 3);
			this.mainTlp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTlp.Enabled = false;
			this.mainTlp.Location = new System.Drawing.Point(0, 0);
			this.mainTlp.Margin = new System.Windows.Forms.Padding(0);
			this.mainTlp.Name = "mainTlp";
			this.mainTlp.RowCount = 5;
			this.mainTlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.mainTlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.mainTlp.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.mainTlp.Size = new System.Drawing.Size(1073, 786);
			this.mainTlp.TabIndex = 0;
			// 
			// mainSplitContainer
			// 
			this.mainTlp.SetColumnSpan(this.mainSplitContainer, 5);
			this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainSplitContainer.Location = new System.Drawing.Point(10, 10);
			this.mainSplitContainer.Margin = new System.Windows.Forms.Padding(0);
			this.mainSplitContainer.Name = "mainSplitContainer";
			// 
			// mainSplitContainer.Panel1
			// 
			this.mainSplitContainer.Panel1.Controls.Add(this.tableLayoutPanel2);
			// 
			// mainSplitContainer.Panel2
			// 
			this.mainSplitContainer.Panel2.Controls.Add(this.editVideoInformationGrid);
			this.mainSplitContainer.Size = new System.Drawing.Size(1053, 727);
			this.mainSplitContainer.SplitterDistance = 246;
			this.mainSplitContainer.SplitterWidth = 10;
			this.mainSplitContainer.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.videosListView, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.addVideosButton, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.moveVideosUpButton, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.moveVideosDownButton, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.removeVideoButton, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.clearVideosButton, 0, 8);
			this.tableLayoutPanel2.Controls.Add(this.insertTemplatesButton, 0, 10);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 12;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(246, 727);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// videosListView
			// 
			this.videosListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.dateiNameColumnHeader});
			this.videosListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.videosListView.FullRowSelect = true;
			this.videosListView.HideSelection = false;
			this.videosListView.Location = new System.Drawing.Point(62, 0);
			this.videosListView.Margin = new System.Windows.Forms.Padding(0);
			this.videosListView.Name = "videosListView";
			this.tableLayoutPanel2.SetRowSpan(this.videosListView, 12);
			this.videosListView.Size = new System.Drawing.Size(184, 727);
			this.videosListView.TabIndex = 0;
			this.videosListView.UseCompatibleStateImageBehavior = false;
			this.videosListView.View = System.Windows.Forms.View.Details;
			this.videosListView.SelectedIndexChanged += new System.EventHandler(this.VideosListView_SelectedIndexChanged);
			this.videosListView.Resize += new System.EventHandler(this.VideosListView_Resize);
			// 
			// dateiNameColumnHeader
			// 
			this.dateiNameColumnHeader.Text = "Dateiname";
			this.dateiNameColumnHeader.Width = 165;
			// 
			// addVideosButton
			// 
			this.addVideosButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.addVideosButton.AutoSize = true;
			this.addVideosButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.addVideosButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.addVideosButton.ForeColor = System.Drawing.Color.ForestGreen;
			this.addVideosButton.Location = new System.Drawing.Point(0, 0);
			this.addVideosButton.Margin = new System.Windows.Forms.Padding(0);
			this.addVideosButton.Name = "addVideosButton";
			this.addVideosButton.Size = new System.Drawing.Size(52, 41);
			this.addVideosButton.TabIndex = 3;
			this.addVideosButton.Text = "+";
			this.addVideosButton.UseVisualStyleBackColor = true;
			this.addVideosButton.Click += new System.EventHandler(this.AddVideosButton_Click);
			// 
			// moveVideosUpButton
			// 
			this.moveVideosUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.moveVideosUpButton.AutoSize = true;
			this.moveVideosUpButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.moveVideosUpButton.Enabled = false;
			this.moveVideosUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.moveVideosUpButton.Location = new System.Drawing.Point(0, 51);
			this.moveVideosUpButton.Margin = new System.Windows.Forms.Padding(0);
			this.moveVideosUpButton.Name = "moveVideosUpButton";
			this.moveVideosUpButton.Size = new System.Drawing.Size(52, 41);
			this.moveVideosUpButton.TabIndex = 4;
			this.moveVideosUpButton.Text = "↑";
			this.moveVideosUpButton.UseVisualStyleBackColor = true;
			this.moveVideosUpButton.Click += new System.EventHandler(this.MoveVideosUpButton_Click);
			// 
			// moveVideosDownButton
			// 
			this.moveVideosDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.moveVideosDownButton.AutoSize = true;
			this.moveVideosDownButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.moveVideosDownButton.Enabled = false;
			this.moveVideosDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.moveVideosDownButton.Location = new System.Drawing.Point(0, 102);
			this.moveVideosDownButton.Margin = new System.Windows.Forms.Padding(0);
			this.moveVideosDownButton.Name = "moveVideosDownButton";
			this.moveVideosDownButton.Size = new System.Drawing.Size(52, 41);
			this.moveVideosDownButton.TabIndex = 5;
			this.moveVideosDownButton.Text = "↓";
			this.moveVideosDownButton.UseVisualStyleBackColor = true;
			this.moveVideosDownButton.Click += new System.EventHandler(this.MoveVideosDownButton_Click);
			// 
			// removeVideoButton
			// 
			this.removeVideoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.removeVideoButton.AutoSize = true;
			this.removeVideoButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.removeVideoButton.Enabled = false;
			this.removeVideoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.removeVideoButton.ForeColor = System.Drawing.Color.Red;
			this.removeVideoButton.Location = new System.Drawing.Point(0, 153);
			this.removeVideoButton.Margin = new System.Windows.Forms.Padding(0);
			this.removeVideoButton.Name = "removeVideoButton";
			this.removeVideoButton.Size = new System.Drawing.Size(52, 41);
			this.removeVideoButton.TabIndex = 6;
			this.removeVideoButton.Text = "-";
			this.removeVideoButton.UseVisualStyleBackColor = true;
			this.removeVideoButton.Click += new System.EventHandler(this.RemoveVideoButton_Click);
			// 
			// clearVideosButton
			// 
			this.clearVideosButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.clearVideosButton.AutoSize = true;
			this.clearVideosButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.clearVideosButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.clearVideosButton.ForeColor = System.Drawing.Color.Red;
			this.clearVideosButton.Location = new System.Drawing.Point(0, 204);
			this.clearVideosButton.Margin = new System.Windows.Forms.Padding(0);
			this.clearVideosButton.Name = "clearVideosButton";
			this.clearVideosButton.Size = new System.Drawing.Size(52, 41);
			this.clearVideosButton.TabIndex = 7;
			this.clearVideosButton.Text = "x";
			this.clearVideosButton.UseVisualStyleBackColor = true;
			this.clearVideosButton.Click += new System.EventHandler(this.ClearVideosButton_Click);
			// 
			// insertTemplatesButton
			// 
			this.insertTemplatesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.insertTemplatesButton.AutoSize = true;
			this.insertTemplatesButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.insertTemplatesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.insertTemplatesButton.ForeColor = System.Drawing.Color.Blue;
			this.insertTemplatesButton.Location = new System.Drawing.Point(0, 255);
			this.insertTemplatesButton.Margin = new System.Windows.Forms.Padding(0);
			this.insertTemplatesButton.Name = "insertTemplatesButton";
			this.insertTemplatesButton.Size = new System.Drawing.Size(52, 41);
			this.insertTemplatesButton.TabIndex = 3;
			this.insertTemplatesButton.Text = "T";
			this.insertTemplatesButton.UseVisualStyleBackColor = true;
			this.insertTemplatesButton.Click += new System.EventHandler(this.InsertTemplatesButton_Click);
			// 
			// editVideoInformationGrid
			// 
			this.editVideoInformationGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editVideoInformationGrid.Enabled = false;
			this.editVideoInformationGrid.IsNewUpload = false;
			this.editVideoInformationGrid.Location = new System.Drawing.Point(0, 0);
			this.editVideoInformationGrid.Name = "editVideoInformationGrid";
			this.editVideoInformationGrid.PscContainer = null;
			this.editVideoInformationGrid.Size = new System.Drawing.Size(797, 727);
			this.editVideoInformationGrid.TabIndex = 0;
			// 
			// acceptButton
			// 
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.acceptButton.AutoSize = true;
			this.acceptButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.acceptButton.Location = new System.Drawing.Point(853, 747);
			this.acceptButton.Margin = new System.Windows.Forms.Padding(0);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Padding = new System.Windows.Forms.Padding(15, 3, 15, 3);
			this.acceptButton.Size = new System.Drawing.Size(101, 29);
			this.acceptButton.TabIndex = 1;
			this.acceptButton.Text = "Hinzufügen";
			this.acceptButton.UseVisualStyleBackColor = true;
			this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.AutoSize = true;
			this.cancelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.cancelButton.Location = new System.Drawing.Point(964, 747);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(0);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Padding = new System.Windows.Forms.Padding(15, 3, 15, 3);
			this.cancelButton.Size = new System.Drawing.Size(99, 29);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Abbrechen";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// loadWorker
			// 
			this.loadWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LoadWorker_DoWork);
			this.loadWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Worker_RunWorkerCompleted);
			// 
			// addVideosWorker
			// 
			this.addVideosWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AddVideosWorker_DoWork);
			this.addVideosWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Worker_RunWorkerCompleted);
			// 
			// addVideosDialog
			// 
			this.addVideosDialog.Filter = "Alle Dateien|*.*";
			this.addVideosDialog.Multiselect = true;
			this.addVideosDialog.Title = "Bitte die Videos zum Hinzufügen auswählen.";
			// 
			// AddVideosForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1073, 786);
			this.Controls.Add(this.mainTlp);
			this.Name = "AddVideosForm";
			this.ShowInTaskbar = false;
			this.Text = "Videos in die Warteschlange einreihen";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.AddVideosForm_Load);
			this.mainTlp.ResumeLayout(false);
			this.mainTlp.PerformLayout();
			this.mainSplitContainer.Panel1.ResumeLayout(false);
			this.mainSplitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
			this.mainSplitContainer.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel mainTlp;
		private System.Windows.Forms.SplitContainer mainSplitContainer;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.ListView videosListView;
		private Controls.EditVideoGrid editVideoInformationGrid;
		private System.Windows.Forms.ColumnHeader dateiNameColumnHeader;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Button cancelButton;
		private System.ComponentModel.BackgroundWorker loadWorker;
		private System.ComponentModel.BackgroundWorker addVideosWorker;
		private System.Windows.Forms.OpenFileDialog addVideosDialog;
		private System.Windows.Forms.Button addVideosButton;
		private System.Windows.Forms.Button moveVideosUpButton;
		private System.Windows.Forms.Button moveVideosDownButton;
		private System.Windows.Forms.Button removeVideoButton;
		private System.Windows.Forms.Button clearVideosButton;
		private System.Windows.Forms.Button insertTemplatesButton;
	}
}