﻿using STFU.Lib.GUI.Controls;

namespace STFU.Executable.AutoUploader.Forms
{
	partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tlpSettings = new System.Windows.Forms.TableLayoutPanel();
            this.lblCurrentLoggedIn = new System.Windows.Forms.Label();
            this.lnklblCurrentLoggedIn = new System.Windows.Forms.LinkLabel();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verwaltenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.youtubeAccountToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.verbindenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verbindungLösenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.templatesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pfadeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistserviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hilfeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.neueFunktionenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.videotutorialPlaylistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.threadImLPFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.threadImYTFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.threadAufGitHubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.downloadSeiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.logverzeichnisÖffnenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.uploaderTabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.queueStatusLabel = new System.Windows.Forms.Label();
            this.queueStatusButton = new System.Windows.Forms.Button();
            this.lblFinishAction = new System.Windows.Forms.Label();
            this.chbChoseProcesses = new System.Windows.Forms.CheckBox();
            this.btnChoseProcs = new System.Windows.Forms.Button();
            this.jobQueue = new STFU.Lib.GUI.Controls.Queue.JobQueue();
            this.cmbbxFinishAction = new System.Windows.Forms.ComboBox();
            this.limitUploadSpeedNud = new System.Windows.Forms.NumericUpDown();
            this.limitUploadSpeedCheckbox = new System.Windows.Forms.CheckBox();
            this.limitUploadSpeedCombobox = new System.Windows.Forms.ComboBox();
            this.addVideosToQueueButton = new System.Windows.Forms.Button();
            this.clearVideosButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.autoUploaderStateLabel = new System.Windows.Forms.Label();
            this.btnStart = new STFU.Lib.GUI.Controls.MenuButton();
            this.startExtendedOptionsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.zeitenFestlegenUndAutouploaderStartenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathsTabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lvSelectedPaths = new System.Windows.Forms.ListView();
            this.chPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFilter = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTemplate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRecursive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chHidden = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbInactive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbMoveAfterUpload = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.archiveTabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.archiveLabel = new System.Windows.Forms.Label();
            this.archiveListView = new System.Windows.Forms.ListView();
            this.archiveVideoName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.archiveVideoPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.archiveRemoveJobButton = new System.Windows.Forms.Button();
            this.archiveAddButton = new System.Windows.Forms.Button();
            this.moveBackToQueueButton = new System.Windows.Forms.Button();
            this.bgwCreateUploader = new System.ComponentModel.BackgroundWorker();
            this.watchingTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tlpSettings.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.uploaderTabPage.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limitUploadSpeedNud)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.startExtendedOptionsContextMenu.SuspendLayout();
            this.pathsTabPage.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.archiveTabPage.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSettings
            // 
            this.tlpSettings.AutoSize = true;
            this.tlpSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpSettings.ColumnCount = 10;
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpSettings.Controls.Add(this.lblCurrentLoggedIn, 1, 4);
            this.tlpSettings.Controls.Add(this.lnklblCurrentLoggedIn, 3, 4);
            this.tlpSettings.Controls.Add(this.mainMenu, 0, 0);
            this.tlpSettings.Controls.Add(this.mainTabControl, 1, 2);
            this.tlpSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSettings.Enabled = false;
            this.tlpSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpSettings.Margin = new System.Windows.Forms.Padding(2);
            this.tlpSettings.Name = "tlpSettings";
            this.tlpSettings.RowCount = 6;
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpSettings.Size = new System.Drawing.Size(1286, 645);
            this.tlpSettings.TabIndex = 0;
            // 
            // lblCurrentLoggedIn
            // 
            this.lblCurrentLoggedIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentLoggedIn.AutoSize = true;
            this.lblCurrentLoggedIn.Location = new System.Drawing.Point(10, 622);
            this.lblCurrentLoggedIn.Margin = new System.Windows.Forms.Padding(0);
            this.lblCurrentLoggedIn.Name = "lblCurrentLoggedIn";
            this.lblCurrentLoggedIn.Size = new System.Drawing.Size(50, 13);
            this.lblCurrentLoggedIn.TabIndex = 10;
            this.lblCurrentLoggedIn.Text = "Youtube:";
            this.lblCurrentLoggedIn.Visible = false;
            // 
            // lnklblCurrentLoggedIn
            // 
            this.lnklblCurrentLoggedIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lnklblCurrentLoggedIn.AutoSize = true;
            this.lnklblCurrentLoggedIn.Location = new System.Drawing.Point(70, 622);
            this.lnklblCurrentLoggedIn.Margin = new System.Windows.Forms.Padding(0);
            this.lnklblCurrentLoggedIn.Name = "lnklblCurrentLoggedIn";
            this.lnklblCurrentLoggedIn.Size = new System.Drawing.Size(23, 13);
            this.lnklblCurrentLoggedIn.TabIndex = 11;
            this.lnklblCurrentLoggedIn.TabStop = true;
            this.lnklblCurrentLoggedIn.Text = "link";
            this.lnklblCurrentLoggedIn.Visible = false;
            this.lnklblCurrentLoggedIn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnklblCurrentLoggedInLinkClicked);
            // 
            // mainMenu
            // 
            this.mainMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpSettings.SetColumnSpan(this.mainMenu, 10);
            this.mainMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.verwaltenToolStripMenuItem,
            this.hilfeToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.mainMenu.Size = new System.Drawing.Size(1286, 24);
            this.mainMenu.TabIndex = 13;
            this.mainMenu.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beendenToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.dateiToolStripMenuItem.Text = "Datei";
            // 
            // beendenToolStripMenuItem
            // 
            this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            this.beendenToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.beendenToolStripMenuItem.Text = "Beenden";
            this.beendenToolStripMenuItem.Click += new System.EventHandler(this.BeendenToolStripMenuItemClick);
            // 
            // verwaltenToolStripMenuItem
            // 
            this.verwaltenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.youtubeAccountToolStripMenuItem1,
            this.templatesToolStripMenuItem1,
            this.pfadeToolStripMenuItem1,
            this.playlistsToolStripMenuItem,
            this.playlistserviceToolStripMenuItem});
            this.verwaltenToolStripMenuItem.Name = "verwaltenToolStripMenuItem";
            this.verwaltenToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.verwaltenToolStripMenuItem.Text = "Verwalten";
            // 
            // youtubeAccountToolStripMenuItem1
            // 
            this.youtubeAccountToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verbindenToolStripMenuItem,
            this.verbindungLösenToolStripMenuItem});
            this.youtubeAccountToolStripMenuItem1.Name = "youtubeAccountToolStripMenuItem1";
            this.youtubeAccountToolStripMenuItem1.Size = new System.Drawing.Size(168, 22);
            this.youtubeAccountToolStripMenuItem1.Text = "Youtube-Account";
            // 
            // verbindenToolStripMenuItem
            // 
            this.verbindenToolStripMenuItem.Name = "verbindenToolStripMenuItem";
            this.verbindenToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.verbindenToolStripMenuItem.Text = "Anmelden mit Google";
            this.verbindenToolStripMenuItem.Click += new System.EventHandler(this.ConnectToolStripMenuItem1_Click);
            // 
            // verbindungLösenToolStripMenuItem
            // 
            this.verbindungLösenToolStripMenuItem.Name = "verbindungLösenToolStripMenuItem";
            this.verbindungLösenToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.verbindungLösenToolStripMenuItem.Text = "Verbindung lösen";
            this.verbindungLösenToolStripMenuItem.Click += new System.EventHandler(this.DeleteConnectionToolStripMenuItem_Click);
            // 
            // templatesToolStripMenuItem1
            // 
            this.templatesToolStripMenuItem1.Name = "templatesToolStripMenuItem1";
            this.templatesToolStripMenuItem1.Size = new System.Drawing.Size(168, 22);
            this.templatesToolStripMenuItem1.Text = "Templates";
            this.templatesToolStripMenuItem1.Click += new System.EventHandler(this.TemplatesToolStripMenuItem1Click);
            // 
            // pfadeToolStripMenuItem1
            // 
            this.pfadeToolStripMenuItem1.Name = "pfadeToolStripMenuItem1";
            this.pfadeToolStripMenuItem1.Size = new System.Drawing.Size(168, 22);
            this.pfadeToolStripMenuItem1.Text = "Pfade";
            this.pfadeToolStripMenuItem1.Click += new System.EventHandler(this.PathsToolStripMenuItem1_Click);
            // 
            // playlistsToolStripMenuItem
            // 
            this.playlistsToolStripMenuItem.Name = "playlistsToolStripMenuItem";
            this.playlistsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.playlistsToolStripMenuItem.Text = "Playlists";
            this.playlistsToolStripMenuItem.Click += new System.EventHandler(this.PlaylistsToolStripMenuItem_Click);
            // 
            // playlistserviceToolStripMenuItem
            // 
            this.playlistserviceToolStripMenuItem.Name = "playlistserviceToolStripMenuItem";
            this.playlistserviceToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.playlistserviceToolStripMenuItem.Text = "Playlistservice";
            this.playlistserviceToolStripMenuItem.Click += new System.EventHandler(this.PlaylistserviceToolStripMenuItem_Click);
            // 
            // hilfeToolStripMenuItem
            // 
            this.hilfeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.neueFunktionenToolStripMenuItem,
            this.toolStripSeparator2,
            this.videotutorialPlaylistToolStripMenuItem,
            this.threadImLPFToolStripMenuItem,
            this.threadImYTFToolStripMenuItem,
            this.threadAufGitHubToolStripMenuItem,
            this.toolStripSeparator1,
            this.downloadSeiteToolStripMenuItem,
            this.toolStripSeparator3,
            this.logverzeichnisÖffnenToolStripMenuItem});
            this.hilfeToolStripMenuItem.Name = "hilfeToolStripMenuItem";
            this.hilfeToolStripMenuItem.Size = new System.Drawing.Size(97, 20);
            this.hilfeToolStripMenuItem.Text = "Hilfe / Support";
            // 
            // neueFunktionenToolStripMenuItem
            // 
            this.neueFunktionenToolStripMenuItem.Name = "neueFunktionenToolStripMenuItem";
            this.neueFunktionenToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.neueFunktionenToolStripMenuItem.Text = "Neue Funktionen";
            this.neueFunktionenToolStripMenuItem.Click += new System.EventHandler(this.NewFeaturesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(187, 6);
            // 
            // videotutorialPlaylistToolStripMenuItem
            // 
            this.videotutorialPlaylistToolStripMenuItem.Name = "videotutorialPlaylistToolStripMenuItem";
            this.videotutorialPlaylistToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.videotutorialPlaylistToolStripMenuItem.Text = "Videotutorial-Playlist";
            this.videotutorialPlaylistToolStripMenuItem.Click += new System.EventHandler(this.VideoTutorialPlaylistToolStripMenuItem_Click);
            // 
            // threadImLPFToolStripMenuItem
            // 
            this.threadImLPFToolStripMenuItem.Name = "threadImLPFToolStripMenuItem";
            this.threadImLPFToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.threadImLPFToolStripMenuItem.Text = "Thread im LPF";
            this.threadImLPFToolStripMenuItem.Click += new System.EventHandler(this.ThreadImLPFToolStripMenuItem_Click);
            // 
            // threadImYTFToolStripMenuItem
            // 
            this.threadImYTFToolStripMenuItem.Name = "threadImYTFToolStripMenuItem";
            this.threadImYTFToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.threadImYTFToolStripMenuItem.Text = "Thread im YTF";
            this.threadImYTFToolStripMenuItem.Click += new System.EventHandler(this.ThreadImYTFToolStripMenuItem_Click);
            // 
            // threadAufGitHubToolStripMenuItem
            // 
            this.threadAufGitHubToolStripMenuItem.Name = "threadAufGitHubToolStripMenuItem";
            this.threadAufGitHubToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.threadAufGitHubToolStripMenuItem.Text = "Thread auf GitHub";
            this.threadAufGitHubToolStripMenuItem.Click += new System.EventHandler(this.ThreadAufGitHubToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(187, 6);
            // 
            // downloadSeiteToolStripMenuItem
            // 
            this.downloadSeiteToolStripMenuItem.Name = "downloadSeiteToolStripMenuItem";
            this.downloadSeiteToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.downloadSeiteToolStripMenuItem.Text = "Download-Seite";
            this.downloadSeiteToolStripMenuItem.Click += new System.EventHandler(this.DownloadPageToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(187, 6);
            // 
            // logverzeichnisÖffnenToolStripMenuItem
            // 
            this.logverzeichnisÖffnenToolStripMenuItem.Name = "logverzeichnisÖffnenToolStripMenuItem";
            this.logverzeichnisÖffnenToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.logverzeichnisÖffnenToolStripMenuItem.Text = "Logverzeichnis öffnen";
            this.logverzeichnisÖffnenToolStripMenuItem.Click += new System.EventHandler(this.OpenLogsToolStripMenuItem_Click);
            // 
            // mainTabControl
            // 
            this.tlpSettings.SetColumnSpan(this.mainTabControl, 8);
            this.mainTabControl.Controls.Add(this.uploaderTabPage);
            this.mainTabControl.Controls.Add(this.pathsTabPage);
            this.mainTabControl.Controls.Add(this.archiveTabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(10, 34);
            this.mainTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1266, 578);
            this.mainTabControl.TabIndex = 18;
            // 
            // uploaderTabPage
            // 
            this.uploaderTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.uploaderTabPage.Controls.Add(this.tableLayoutPanel3);
            this.uploaderTabPage.Location = new System.Drawing.Point(4, 22);
            this.uploaderTabPage.Name = "uploaderTabPage";
            this.uploaderTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.uploaderTabPage.Size = new System.Drawing.Size(1258, 552);
            this.uploaderTabPage.TabIndex = 1;
            this.uploaderTabPage.Text = "Warteschlange";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.groupBox2, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1252, 546);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(13, 91);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1226, 442);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Warteschlange";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 21;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel2.Controls.Add(this.queueStatusLabel, 17, 8);
            this.tableLayoutPanel2.Controls.Add(this.queueStatusButton, 19, 8);
            this.tableLayoutPanel2.Controls.Add(this.lblFinishAction, 3, 8);
            this.tableLayoutPanel2.Controls.Add(this.chbChoseProcesses, 13, 8);
            this.tableLayoutPanel2.Controls.Add(this.btnChoseProcs, 15, 8);
            this.tableLayoutPanel2.Controls.Add(this.jobQueue, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmbbxFinishAction, 5, 8);
            this.tableLayoutPanel2.Controls.Add(this.limitUploadSpeedNud, 7, 6);
            this.tableLayoutPanel2.Controls.Add(this.limitUploadSpeedCheckbox, 3, 6);
            this.tableLayoutPanel2.Controls.Add(this.limitUploadSpeedCombobox, 9, 6);
            this.tableLayoutPanel2.Controls.Add(this.addVideosToQueueButton, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.clearVideosButton, 1, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 10;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1220, 423);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // queueStatusLabel
            // 
            this.queueStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queueStatusLabel.AutoSize = true;
            this.queueStatusLabel.Location = new System.Drawing.Point(986, 396);
            this.queueStatusLabel.Margin = new System.Windows.Forms.Padding(0);
            this.queueStatusLabel.Name = "queueStatusLabel";
            this.queueStatusLabel.Size = new System.Drawing.Size(155, 13);
            this.queueStatusLabel.TabIndex = 19;
            this.queueStatusLabel.Text = "Die Warteschlange ist gestoppt";
            // 
            // queueStatusButton
            // 
            this.queueStatusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queueStatusButton.AutoSize = true;
            this.queueStatusButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.queueStatusButton.Enabled = false;
            this.queueStatusButton.Location = new System.Drawing.Point(1151, 389);
            this.queueStatusButton.Margin = new System.Windows.Forms.Padding(0);
            this.queueStatusButton.Name = "queueStatusButton";
            this.queueStatusButton.Padding = new System.Windows.Forms.Padding(11, 2, 11, 2);
            this.queueStatusButton.Size = new System.Drawing.Size(64, 27);
            this.queueStatusButton.TabIndex = 7;
            this.queueStatusButton.Text = "Start!";
            this.toolTip.SetToolTip(this.queueStatusButton, "Den Uploader starten oder stoppen - hiermit werden bereits in die Warteschlange h" +
        "inzugefügte Videos hochgeladen.");
            this.queueStatusButton.UseVisualStyleBackColor = true;
            this.queueStatusButton.Click += new System.EventHandler(this.QueueStatusButton_Click);
            // 
            // lblFinishAction
            // 
            this.lblFinishAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFinishAction.Location = new System.Drawing.Point(66, 396);
            this.lblFinishAction.Margin = new System.Windows.Forms.Padding(0);
            this.lblFinishAction.Name = "lblFinishAction";
            this.lblFinishAction.Size = new System.Drawing.Size(53, 13);
            this.lblFinishAction.TabIndex = 14;
            this.lblFinishAction.Text = "Am Ende:";
            // 
            // chbChoseProcesses
            // 
            this.chbChoseProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.chbChoseProcesses.Enabled = false;
            this.chbChoseProcesses.Location = new System.Drawing.Point(780, 394);
            this.chbChoseProcesses.Margin = new System.Windows.Forms.Padding(0);
            this.chbChoseProcesses.Name = "chbChoseProcesses";
            this.chbChoseProcesses.Size = new System.Drawing.Size(150, 17);
            this.chbChoseProcesses.TabIndex = 16;
            this.chbChoseProcesses.Text = "Programmenden abwarten";
            this.chbChoseProcesses.UseVisualStyleBackColor = true;
            this.chbChoseProcesses.CheckedChanged += new System.EventHandler(this.ChbChoseProcessesCheckedChanged);
            // 
            // btnChoseProcs
            // 
            this.btnChoseProcs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChoseProcs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnChoseProcs.Enabled = false;
            this.btnChoseProcs.Location = new System.Drawing.Point(940, 389);
            this.btnChoseProcs.Margin = new System.Windows.Forms.Padding(0);
            this.btnChoseProcs.Name = "btnChoseProcs";
            this.btnChoseProcs.Padding = new System.Windows.Forms.Padding(2);
            this.btnChoseProcs.Size = new System.Drawing.Size(36, 27);
            this.btnChoseProcs.TabIndex = 17;
            this.btnChoseProcs.Text = "[...]";
            this.btnChoseProcs.UseVisualStyleBackColor = true;
            this.btnChoseProcs.Click += new System.EventHandler(this.BtnChoseProcsClick);
            // 
            // jobQueue
            // 
            this.jobQueue.AutoScroll = true;
            this.jobQueue.AutoSize = true;
            this.jobQueue.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.SetColumnSpan(this.jobQueue, 17);
            this.jobQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jobQueue.Location = new System.Drawing.Point(66, 7);
            this.jobQueue.Margin = new System.Windows.Forms.Padding(0);
            this.jobQueue.Name = "jobQueue";
            this.tableLayoutPanel2.SetRowSpan(this.jobQueue, 4);
            this.jobQueue.ShowActionsButtons = true;
            this.jobQueue.Size = new System.Drawing.Size(1149, 341);
            this.jobQueue.TabIndex = 18;
            this.jobQueue.Uploader = null;
            // 
            // cmbbxFinishAction
            // 
            this.cmbbxFinishAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.cmbbxFinishAction, 7);
            this.cmbbxFinishAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbbxFinishAction.FormattingEnabled = true;
            this.cmbbxFinishAction.Items.AddRange(new object[] {
            "Nichts tun",
            "Programm schließen",
            "Herunterfahren"});
            this.cmbbxFinishAction.Location = new System.Drawing.Point(129, 392);
            this.cmbbxFinishAction.Margin = new System.Windows.Forms.Padding(0);
            this.cmbbxFinishAction.Name = "cmbbxFinishAction";
            this.cmbbxFinishAction.Size = new System.Drawing.Size(641, 21);
            this.cmbbxFinishAction.TabIndex = 15;
            this.cmbbxFinishAction.SelectedIndexChanged += new System.EventHandler(this.CmbbxFinishActionSelectedIndexChanged);
            // 
            // limitUploadSpeedNud
            // 
            this.limitUploadSpeedNud.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.limitUploadSpeedNud.Location = new System.Drawing.Point(314, 358);
            this.limitUploadSpeedNud.Margin = new System.Windows.Forms.Padding(0);
            this.limitUploadSpeedNud.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.limitUploadSpeedNud.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.limitUploadSpeedNud.Name = "limitUploadSpeedNud";
            this.limitUploadSpeedNud.Size = new System.Drawing.Size(58, 20);
            this.limitUploadSpeedNud.TabIndex = 20;
            this.limitUploadSpeedNud.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.limitUploadSpeedNud.ValueChanged += new System.EventHandler(this.LimitUploadSpeedNud_ValueChanged);
            // 
            // limitUploadSpeedCheckbox
            // 
            this.limitUploadSpeedCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.limitUploadSpeedCheckbox.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.limitUploadSpeedCheckbox, 3);
            this.limitUploadSpeedCheckbox.Location = new System.Drawing.Point(66, 360);
            this.limitUploadSpeedCheckbox.Margin = new System.Windows.Forms.Padding(0);
            this.limitUploadSpeedCheckbox.Name = "limitUploadSpeedCheckbox";
            this.limitUploadSpeedCheckbox.Size = new System.Drawing.Size(238, 17);
            this.limitUploadSpeedCheckbox.TabIndex = 16;
            this.limitUploadSpeedCheckbox.Text = "Uploadgeschwindigkeit pro Job limitieren auf:";
            this.limitUploadSpeedCheckbox.UseVisualStyleBackColor = true;
            this.limitUploadSpeedCheckbox.CheckedChanged += new System.EventHandler(this.LimitUploadSpeedCheckbox_CheckedChanged);
            // 
            // limitUploadSpeedCombobox
            // 
            this.limitUploadSpeedCombobox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.limitUploadSpeedCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.limitUploadSpeedCombobox.FormattingEnabled = true;
            this.limitUploadSpeedCombobox.Items.AddRange(new object[] {
            "kByte/s",
            "MByte/s",
            "GByte/s",
            "TByte/s"});
            this.limitUploadSpeedCombobox.Location = new System.Drawing.Point(377, 358);
            this.limitUploadSpeedCombobox.Margin = new System.Windows.Forms.Padding(0);
            this.limitUploadSpeedCombobox.Name = "limitUploadSpeedCombobox";
            this.limitUploadSpeedCombobox.Size = new System.Drawing.Size(69, 21);
            this.limitUploadSpeedCombobox.TabIndex = 21;
            this.limitUploadSpeedCombobox.SelectedIndexChanged += new System.EventHandler(this.LimitUploadSpeedCombobox_SelectedIndexChanged);
            // 
            // addVideosToQueueButton
            // 
            this.addVideosToQueueButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.addVideosToQueueButton.AutoSize = true;
            this.addVideosToQueueButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addVideosToQueueButton.Enabled = false;
            this.addVideosToQueueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addVideosToQueueButton.ForeColor = System.Drawing.Color.ForestGreen;
            this.addVideosToQueueButton.Location = new System.Drawing.Point(5, 7);
            this.addVideosToQueueButton.Margin = new System.Windows.Forms.Padding(0);
            this.addVideosToQueueButton.Name = "addVideosToQueueButton";
            this.addVideosToQueueButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.addVideosToQueueButton.Size = new System.Drawing.Size(51, 41);
            this.addVideosToQueueButton.TabIndex = 22;
            this.addVideosToQueueButton.Text = "+";
            this.toolTip.SetToolTip(this.addVideosToQueueButton, "Neue Videos manuell hinzufügen");
            this.addVideosToQueueButton.UseVisualStyleBackColor = true;
            this.addVideosToQueueButton.Click += new System.EventHandler(this.AddVideosToQueueButton_Click);
            // 
            // clearVideosButton
            // 
            this.clearVideosButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.clearVideosButton.AutoSize = true;
            this.clearVideosButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.clearVideosButton.Enabled = false;
            this.clearVideosButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearVideosButton.ForeColor = System.Drawing.Color.Red;
            this.clearVideosButton.Location = new System.Drawing.Point(5, 58);
            this.clearVideosButton.Margin = new System.Windows.Forms.Padding(0);
            this.clearVideosButton.Name = "clearVideosButton";
            this.clearVideosButton.Size = new System.Drawing.Size(51, 41);
            this.clearVideosButton.TabIndex = 23;
            this.clearVideosButton.Text = "x";
            this.toolTip.SetToolTip(this.clearVideosButton, "Alle Videos aus der Warteschlange entfernen");
            this.clearVideosButton.UseVisualStyleBackColor = true;
            this.clearVideosButton.Click += new System.EventHandler(this.ClearVideosButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.tableLayoutPanel4);
            this.groupBox2.Location = new System.Drawing.Point(13, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1226, 62);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Automatischer Videoupload";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel4.Controls.Add(this.autoUploaderStateLabel, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.btnStart, 3, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1220, 43);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // autoUploaderStateLabel
            // 
            this.autoUploaderStateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.autoUploaderStateLabel.AutoSize = true;
            this.autoUploaderStateLabel.Location = new System.Drawing.Point(5, 15);
            this.autoUploaderStateLabel.Margin = new System.Windows.Forms.Padding(0);
            this.autoUploaderStateLabel.Name = "autoUploaderStateLabel";
            this.autoUploaderStateLabel.Size = new System.Drawing.Size(1077, 13);
            this.autoUploaderStateLabel.TabIndex = 19;
            this.autoUploaderStateLabel.Text = "Der AutoUploader ist gestoppt";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.AutoSize = true;
            this.btnStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(1092, 7);
            this.btnStart.Margin = new System.Windows.Forms.Padding(0);
            this.btnStart.Menu = this.startExtendedOptionsContextMenu;
            this.btnStart.Name = "btnStart";
            this.btnStart.Padding = new System.Windows.Forms.Padding(15, 3, 25, 3);
            this.btnStart.Size = new System.Drawing.Size(123, 29);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Sofort starten!";
            this.toolTip.SetToolTip(this.btnStart, "Den Autouploader starten oder stoppen - hiermit werden Videos automatisch in die " +
        "Warteschlange aufgenommen.");
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStartClick);
            // 
            // startExtendedOptionsContextMenu
            // 
            this.startExtendedOptionsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zeitenFestlegenUndAutouploaderStartenToolStripMenuItem});
            this.startExtendedOptionsContextMenu.Name = "startExtendedOptionsContextMenu";
            this.startExtendedOptionsContextMenu.Size = new System.Drawing.Size(298, 26);
            // 
            // zeitenFestlegenUndAutouploaderStartenToolStripMenuItem
            // 
            this.zeitenFestlegenUndAutouploaderStartenToolStripMenuItem.Name = "zeitenFestlegenUndAutouploaderStartenToolStripMenuItem";
            this.zeitenFestlegenUndAutouploaderStartenToolStripMenuItem.Size = new System.Drawing.Size(297, 22);
            this.zeitenFestlegenUndAutouploaderStartenToolStripMenuItem.Text = "Zeiten festlegen und Autouploader starten";
            this.zeitenFestlegenUndAutouploaderStartenToolStripMenuItem.Click += new System.EventHandler(this.SetTimeAndStartUploaderToolStripMenuItem_Click);
            // 
            // pathsTabPage
            // 
            this.pathsTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.pathsTabPage.Controls.Add(this.tableLayoutPanel1);
            this.pathsTabPage.Location = new System.Drawing.Point(4, 22);
            this.pathsTabPage.Name = "pathsTabPage";
            this.pathsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.pathsTabPage.Size = new System.Drawing.Size(1258, 552);
            this.pathsTabPage.TabIndex = 0;
            this.pathsTabPage.Text = "Zu überwachende Pfade";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Controls.Add(this.lvSelectedPaths, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1252, 546);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lvSelectedPaths
            // 
            this.lvSelectedPaths.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPath,
            this.chFilter,
            this.chTemplate,
            this.chRecursive,
            this.chHidden,
            this.cbInactive,
            this.cbMoveAfterUpload});
            this.lvSelectedPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSelectedPaths.FullRowSelect = true;
            this.lvSelectedPaths.GridLines = true;
            this.lvSelectedPaths.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvSelectedPaths.HideSelection = false;
            this.lvSelectedPaths.Location = new System.Drawing.Point(10, 10);
            this.lvSelectedPaths.Margin = new System.Windows.Forms.Padding(0);
            this.lvSelectedPaths.Name = "lvSelectedPaths";
            this.lvSelectedPaths.ShowGroups = false;
            this.lvSelectedPaths.Size = new System.Drawing.Size(1232, 526);
            this.lvSelectedPaths.TabIndex = 9;
            this.lvSelectedPaths.UseCompatibleStateImageBehavior = false;
            this.lvSelectedPaths.View = System.Windows.Forms.View.Details;
            // 
            // chPath
            // 
            this.chPath.Text = "Pfad";
            this.chPath.Width = 350;
            // 
            // chFilter
            // 
            this.chFilter.Text = "Filter";
            this.chFilter.Width = 150;
            // 
            // chTemplate
            // 
            this.chTemplate.Text = "Template";
            this.chTemplate.Width = 150;
            // 
            // chRecursive
            // 
            this.chRecursive.Text = "Unterverzeichnisse";
            this.chRecursive.Width = 150;
            // 
            // chHidden
            // 
            this.chHidden.Text = "Versteckte";
            this.chHidden.Width = 80;
            // 
            // cbInactive
            // 
            this.cbInactive.Text = "Inaktiv";
            // 
            // cbMoveAfterUpload
            // 
            this.cbMoveAfterUpload.Text = "Verschieben";
            this.cbMoveAfterUpload.Width = 80;
            // 
            // archiveTabPage
            // 
            this.archiveTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.archiveTabPage.Controls.Add(this.tableLayoutPanel5);
            this.archiveTabPage.Location = new System.Drawing.Point(4, 22);
            this.archiveTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.archiveTabPage.Name = "archiveTabPage";
            this.archiveTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.archiveTabPage.Size = new System.Drawing.Size(1258, 552);
            this.archiveTabPage.TabIndex = 2;
            this.archiveTabPage.Text = "Videos, die nicht hochgeladen werden sollen";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 9;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel5.Controls.Add(this.archiveLabel, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.archiveListView, 1, 3);
            this.tableLayoutPanel5.Controls.Add(this.archiveRemoveJobButton, 7, 5);
            this.tableLayoutPanel5.Controls.Add(this.archiveAddButton, 3, 5);
            this.tableLayoutPanel5.Controls.Add(this.moveBackToQueueButton, 5, 5);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 7;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1252, 546);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // archiveLabel
            // 
            this.archiveLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.archiveLabel.AutoSize = true;
            this.tableLayoutPanel5.SetColumnSpan(this.archiveLabel, 7);
            this.archiveLabel.Location = new System.Drawing.Point(10, 10);
            this.archiveLabel.Margin = new System.Windows.Forms.Padding(0);
            this.archiveLabel.Name = "archiveLabel";
            this.archiveLabel.Size = new System.Drawing.Size(1232, 65);
            this.archiveLabel.TabIndex = 0;
            this.archiveLabel.Text = resources.GetString("archiveLabel.Text");
            // 
            // archiveListView
            // 
            this.archiveListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.archiveVideoName,
            this.archiveVideoPath});
            this.tableLayoutPanel5.SetColumnSpan(this.archiveListView, 7);
            this.archiveListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.archiveListView.FullRowSelect = true;
            this.archiveListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.archiveListView.HideSelection = false;
            this.archiveListView.LabelWrap = false;
            this.archiveListView.Location = new System.Drawing.Point(10, 85);
            this.archiveListView.Margin = new System.Windows.Forms.Padding(0);
            this.archiveListView.Name = "archiveListView";
            this.archiveListView.Size = new System.Drawing.Size(1232, 412);
            this.archiveListView.TabIndex = 1;
            this.archiveListView.UseCompatibleStateImageBehavior = false;
            this.archiveListView.View = System.Windows.Forms.View.Details;
            this.archiveListView.SelectedIndexChanged += new System.EventHandler(this.ArchiveListView_SelectedIndexChanged);
            // 
            // archiveVideoName
            // 
            this.archiveVideoName.Text = "Video";
            this.archiveVideoName.Width = 1000;
            // 
            // archiveVideoPath
            // 
            this.archiveVideoPath.Text = "Pfad";
            this.archiveVideoPath.Width = 500;
            // 
            // archiveRemoveJobButton
            // 
            this.archiveRemoveJobButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.archiveRemoveJobButton.AutoSize = true;
            this.archiveRemoveJobButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.archiveRemoveJobButton.Enabled = false;
            this.archiveRemoveJobButton.Location = new System.Drawing.Point(1104, 507);
            this.archiveRemoveJobButton.Margin = new System.Windows.Forms.Padding(0);
            this.archiveRemoveJobButton.Name = "archiveRemoveJobButton";
            this.archiveRemoveJobButton.Padding = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.archiveRemoveJobButton.Size = new System.Drawing.Size(138, 29);
            this.archiveRemoveJobButton.TabIndex = 2;
            this.archiveRemoveJobButton.Text = "Aus Archiv löschen";
            this.archiveRemoveJobButton.UseVisualStyleBackColor = true;
            this.archiveRemoveJobButton.Click += new System.EventHandler(this.ArchiveRemoveJobButton_Click);
            // 
            // archiveAddButton
            // 
            this.archiveAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.archiveAddButton.AutoSize = true;
            this.archiveAddButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.archiveAddButton.Location = new System.Drawing.Point(763, 507);
            this.archiveAddButton.Margin = new System.Windows.Forms.Padding(0);
            this.archiveAddButton.Name = "archiveAddButton";
            this.archiveAddButton.Padding = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.archiveAddButton.Size = new System.Drawing.Size(129, 29);
            this.archiveAddButton.TabIndex = 2;
            this.archiveAddButton.Text = "Video hinzufügen";
            this.archiveAddButton.UseVisualStyleBackColor = true;
            this.archiveAddButton.Click += new System.EventHandler(this.ArchiveAddButton_Click);
            // 
            // moveBackToQueueButton
            // 
            this.moveBackToQueueButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.moveBackToQueueButton.AutoSize = true;
            this.moveBackToQueueButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.moveBackToQueueButton.Enabled = false;
            this.moveBackToQueueButton.Location = new System.Drawing.Point(902, 507);
            this.moveBackToQueueButton.Margin = new System.Windows.Forms.Padding(0);
            this.moveBackToQueueButton.Name = "moveBackToQueueButton";
            this.moveBackToQueueButton.Padding = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.moveBackToQueueButton.Size = new System.Drawing.Size(192, 29);
            this.moveBackToQueueButton.TabIndex = 2;
            this.moveBackToQueueButton.Text = "In Warteschlange verschieben";
            this.moveBackToQueueButton.UseVisualStyleBackColor = true;
            this.moveBackToQueueButton.Click += new System.EventHandler(this.MoveBackToQueueButton_Click);
            // 
            // bgwCreateUploader
            // 
            this.bgwCreateUploader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BgwCreateUploaderDoWork);
            this.bgwCreateUploader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgwCreateUploaderRunWorkerCompleted);
            // 
            // watchingTimer
            // 
            this.watchingTimer.Enabled = true;
            this.watchingTimer.Interval = 3000;
            this.watchingTimer.Tick += new System.EventHandler(this.WatchingTimer_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Strohis Toolset Für Uploads";
            this.notifyIcon.Visible = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Alle Dateien|*.*";
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.Title = "Videos zum ignorieren hinzufügen";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1286, 645);
            this.Controls.Add(this.tlpSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Strohis Toolset Für Uploads - AutoUploader";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.tlpSettings.ResumeLayout(false);
            this.tlpSettings.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.uploaderTabPage.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limitUploadSpeedNud)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.startExtendedOptionsContextMenu.ResumeLayout(false);
            this.pathsTabPage.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.archiveTabPage.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpSettings;
		private MenuButton btnStart;
		private System.Windows.Forms.ListView lvSelectedPaths;
		private System.Windows.Forms.ColumnHeader chPath;
		private System.Windows.Forms.ColumnHeader chFilter;
		private System.ComponentModel.BackgroundWorker bgwCreateUploader;
		private System.Windows.Forms.Label lblCurrentLoggedIn;
		private System.Windows.Forms.LinkLabel lnklblCurrentLoggedIn;
		private System.Windows.Forms.ColumnHeader chRecursive;
		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
		private System.Windows.Forms.Label lblFinishAction;
		private System.Windows.Forms.ComboBox cmbbxFinishAction;
		private System.Windows.Forms.CheckBox chbChoseProcesses;
		private System.Windows.Forms.Button btnChoseProcs;
		private System.Windows.Forms.ColumnHeader chTemplate;
		private System.Windows.Forms.ToolStripMenuItem verwaltenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem templatesToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem pfadeToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem youtubeAccountToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem verbindenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem verbindungLösenToolStripMenuItem;
		private System.Windows.Forms.ColumnHeader chHidden;
		private System.Windows.Forms.ColumnHeader cbInactive;
		private System.Windows.Forms.ToolStripMenuItem hilfeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem threadImLPFToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem threadImYTFToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem downloadSeiteToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem neueFunktionenToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem logverzeichnisÖffnenToolStripMenuItem;
		private System.Windows.Forms.TabControl mainTabControl;
		private System.Windows.Forms.TabPage pathsTabPage;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TabPage uploaderTabPage;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private Lib.GUI.Controls.Queue.JobQueue jobQueue;
		private System.Windows.Forms.Timer watchingTimer;
		private System.Windows.Forms.Label autoUploaderStateLabel;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.Button queueStatusButton;
		private System.Windows.Forms.Label queueStatusLabel;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.ContextMenuStrip startExtendedOptionsContextMenu;
		private System.Windows.Forms.ToolStripMenuItem zeitenFestlegenUndAutouploaderStartenToolStripMenuItem;
		private System.Windows.Forms.TabPage archiveTabPage;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label archiveLabel;
		private System.Windows.Forms.ListView archiveListView;
		private System.Windows.Forms.ColumnHeader archiveVideoName;
		private System.Windows.Forms.Button archiveRemoveJobButton;
		private System.Windows.Forms.ToolStripMenuItem videotutorialPlaylistToolStripMenuItem;
		private System.Windows.Forms.Button archiveAddButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.ColumnHeader archiveVideoPath;
		private System.Windows.Forms.Button moveBackToQueueButton;
		private System.Windows.Forms.ColumnHeader cbMoveAfterUpload;
		private System.Windows.Forms.NumericUpDown limitUploadSpeedNud;
		private System.Windows.Forms.CheckBox limitUploadSpeedCheckbox;
		private System.Windows.Forms.ComboBox limitUploadSpeedCombobox;
		private System.Windows.Forms.Button addVideosToQueueButton;
		private System.Windows.Forms.Button clearVideosButton;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ToolStripMenuItem playlistsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem playlistserviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem threadAufGitHubToolStripMenuItem;
    }
}

