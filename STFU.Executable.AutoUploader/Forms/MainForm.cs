using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using log4net;
using Microsoft.WindowsAPICodePack.Taskbar;
using STFU.Lib.GUI.Forms;
using STFU.Lib.Playlistservice;
using STFU.Lib.Playlistservice.Model;
using STFU.Lib.Youtube;
using STFU.Lib.Youtube.Automation;
using STFU.Lib.Youtube.Automation.Interfaces;
using STFU.Lib.Youtube.Automation.Interfaces.Model;
using STFU.Lib.Youtube.Automation.Interfaces.Model.Args;
using STFU.Lib.Youtube.Automation.Templates;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Enums;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Interfaces.Model.Enums;
using STFU.Lib.Youtube.Model;
using STFU.Lib.Youtube.Persistor;
using STFU.Lib.Youtube.Persistor.Model;
using STFU.Lib.Youtube.Services;
using STFU.Lib.Youtube.Upload;

namespace STFU.Executable.AutoUploader.Forms
{
	public partial class MainForm : Form
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(MainForm));

        readonly IPathContainer pathContainer = new PathContainer();
        readonly ITemplateContainer templateContainer = new TemplateContainer();
        readonly IYoutubeClientContainer clientContainer = new YoutubeClientContainer();
        readonly IYoutubeAccountContainer accountContainer = new YoutubeAccountContainer();
        readonly IYoutubeCategoryContainer categoryContainer = new YoutubeCategoryContainer();
        readonly IYoutubeLanguageContainer languageContainer = new YoutubeLanguageContainer();
        readonly IYoutubeJobContainer queueContainer = new YoutubeJobContainer();
        readonly IYoutubeJobContainer archiveContainer = new YoutubeJobContainer();

        readonly IYoutubePlaylistContainer playlistContainer = new YoutubePlaylistContainer();
        readonly IPlaylistServiceConnectionContainer playlistServiceConnectionContainer = new PlaylistServiceConnectionContainer();
        readonly IYoutubeAccountCommunicator accountCommunicator = new YoutubeAccountCommunicator();

		IAutomationUploader autoUploader;

        readonly ProcessList processes = new ProcessList();

        readonly AutoUploaderSettings autoUploaderSettings = new AutoUploaderSettings();

		PathPersistor pathPersistor = null;
		TemplatePersistor templatePersistor = null;
		AccountPersistor accountPersistor = null;
		CategoryPersistor categoryPersistor = null;
		LanguagePersistor languagePersistor = null;
		AutoUploaderSettingsPersistor settingsPersistor = null;
		JobPersistor queuePersistor = null;
		JobPersistor archivePersistor = null;

		PlaylistPersistor playlistPersistor = null;
		PlaylistServiceConnectionPersistor playlistServiceConnectionPersistor = null;

		private readonly bool showReleaseNotes = false;
		bool ended = false;
		bool canceled = false;

		int progress = 0;

		public MainForm(bool showReleaseNotes)
		{
			Logger.Info("Initializing main form");

			InitializeComponent();

			this.showReleaseNotes = showReleaseNotes;

			Text = $"Strohis Toolset Für Uploads - AutoUploader v{ProductVersion} [BETA]";
		}

		private void RefillArchiveView()
		{
			Logger.Info("Refilling archive listview");

			archiveListView.Items.Clear();

			foreach (var job in archiveContainer.RegisteredJobs)
			{
				ListViewItem item = new ListViewItem(job.Video.Title);
				item.SubItems.Add(job.Video.Path);
				archiveListView.Items.Add(item);

				Logger.Debug($"Added entry for job for video '{job.Video.Title}'");
			}
		}

		private void UploaderNewUploadStarted(UploadStartedEventArgs args)
		{
			Logger.Info($"Received event that a new upload was started - Video: '{args.Job.Video.Title}'");

			args.Job.PropertyChanged += Job_PropertyChanged;

			if (args.Job.NotificationSettings.NotifyOnVideoUploadStartedDesktop)
			{
				Logger.Info($"Informing via balloon tip");

				notifyIcon.ShowBalloonTip(
					10000,
					"Upload wurde gestartet",
					$"Das Video '{args.Job.Video.Title}' wird nun hochgeladen.",
					ToolTipIcon.Info
				);
			}
		}

		private void Job_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Logger.Debug($"Received a job property changed event - Property: '{e.PropertyName}'");
			Logger.Debug($"Sender was job with video: '{((IYoutubeJob)sender).Video.Title}'");

			if (e.PropertyName == nameof(IYoutubeJob.State))
			{
				var job = (IYoutubeJob)sender;

				Logger.Info($"Received new State '{job.State}' for job with video: '{job.Video.Title}'");

				if (job.State == JobState.Successful)
				{
					Logger.Info($"Informing via balloon tip");

					if (job.NotificationSettings.NotifyOnVideoUploadFinishedDesktop)
					{
						notifyIcon.ShowBalloonTip(
							10000,
							"Upload abgeschlossen!",
							$"Das Video '{job.Video.Title}' wurde erfolgreich hochgeladen.",
							ToolTipIcon.Info
						);
					}
				}
				else if (job.State == JobState.Error)
				{
					if (job.NotificationSettings.NotifyOnVideoUploadFailedDesktop)
					{
						Logger.Info($"Informing via balloon tip");

						notifyIcon.ShowBalloonTip(
							10000,
							"Upload fehlgeschlagen.",
							$"Das Video '{job.Video.Title}' konnte aufgrund eines Fehlers nicht hochgeladen werden.",
							ToolTipIcon.Info
						);
					}
				}

				if (job.State == JobState.NotStarted
					|| job.State == JobState.Canceled
					|| job.State == JobState.Error
					|| job.State == JobState.Successful)
				{
					Logger.Debug($"Removing listener for job with video: '{job.Video.Title}'");
					job.PropertyChanged -= Job_PropertyChanged;
				}
			}
		}

		private void RefillSelectedPathsListView()
		{
			Logger.Info("Refilling selected paths listview");

			lvSelectedPaths.Items.Clear();

			foreach (var entry in pathContainer.RegisteredPaths)
			{
				var newItem = lvSelectedPaths.Items.Add(entry.Fullname);
				newItem.SubItems.Add(entry.Filter);

				string templateName = templateContainer.RegisteredTemplates.FirstOrDefault(t => t.Id == entry.SelectedTemplateId)?.Name;
				if (string.IsNullOrWhiteSpace(templateName))
				{
					templateName = templateContainer.RegisteredTemplates.FirstOrDefault(t => t.Id == 0).Name;
				}
				newItem.SubItems.Add(templateName);

				newItem.SubItems.Add(entry.SearchRecursively ? "Ja" : "Nein");
				newItem.SubItems.Add(entry.SearchHidden ? "Ja" : "Nein");
				newItem.SubItems.Add(entry.Inactive ? "Ja" : "Nein");
				newItem.SubItems.Add(entry.MoveAfterUpload ? "Ja" : "Nein");

				Logger.Debug($"Added entry for path '{entry.Fullname}'");
			}
		}

		private void AutoUploader_FileToUploadOccured(object sender, JobEventArgs e)
		{
			Logger.Info($"Received information about a newly found video '{e.Job.Video.Title}' from autouploader");

			if (e.Job.NotificationSettings.NotifyOnVideoFoundDesktop)
			{
				Logger.Info($"Informing via balloon tip");

				notifyIcon.ShowBalloonTip(
					10000,
					"Neues Video in der Warteschlange",
					$"Das Video '{e.Job.Video.Title}' wurde in die Warteschlange aufgenommen.",
					ToolTipIcon.Info
				);
			}

			// Aktualisiertes Hochladedatum im Template speichern
			templatePersistor.Save();
		}

		private void ConnectToYoutube()
		{
			Logger.Debug($"Youtube account connection method was called");

			tlpSettings.Enabled = false;
			  
			var client = clientContainer.RegisteredClients.FirstOrDefault();

            var addForm = new AddYoutubeAccountForm
            {
                ExternalCodeUrl = accountCommunicator.CreateAuthUri(client, YoutubeRedirectUri.Localhost, GoogleScope.Manage).AbsoluteUri,
                SendMailAuthUrl = accountCommunicator.CreateAuthUri(client, YoutubeRedirectUri.Localhost, GoogleScope.Manage | GoogleScope.SendMail).AbsoluteUri
            };

            var result = addForm.ShowDialog(this);
            try
            {
                if (result == DialogResult.OK)
                {
                    Logger.Info($"Trying to connect a new youtube account");

                    IYoutubeAccount account;
                    if ((account = accountCommunicator.ConnectToAccount(addForm.AuthToken, addForm.MailsRequested, client, YoutubeRedirectUri.Localhost)) != null)
                    {
                        Logger.Info($"Could connect to account");

                        accountContainer.RegisterAccount(account);

                        Logger.Info($"Reloading categories to match up with that account");

                        var loader = new LanguageCategoryLoader(accountContainer);
                        var categories = loader.Categories;

                        categoryContainer.UnregisterAllCategories();
                        foreach (var category in categories)
                        {
                            Logger.Info($"Adding category '{category.Title}'");
                            categoryContainer.RegisterCategory(category);
                        }

                        Logger.Info($"Reloading languages");

                        var languages = loader.Languages;

                        languageContainer.UnregisterAllLanguages();
                        foreach (var language in languages)
                        {
                            Logger.Info($"Adding language '{language.Name}'");
                            languageContainer.RegisterLanguage(language);
                        }

                        // Account speichern! Und so!
                        accountPersistor.Save();
                        categoryPersistor.Save();
                        languagePersistor.Save();

                        Logger.Info($"Youtube account was added successfully");

                        MessageBox.Show(this, "Der Uploader wurde erfolgreich mit dem Account verbunden!", "Account verbunden!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ActivateAccountLink();
                    }
                }
            }
            catch (QuotaErrorException ex)
            {
                Logger.Error($"Could not connect to account because max quota was reached", ex);

                MessageBox.Show(this, $"Die Verbindung mit dem Account konnte nicht hergestellt werden. Das liegt daran, dass Youtube die Anzahl der Aufrufe, die Programme machen dürfen, beschränkt. Für dieses Programm wurden heute alle Aufrufe ausgeschöpft, daher geht es heute nicht mehr.{Environment.NewLine}{Environment.NewLine}Bitte versuche es morgen wieder.", "Account kann heute nicht verbunden werden!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            tlpSettings.Enabled = true;
		}

		private void ActivateAccountLink()
		{
			Logger.Info("Activating account link and start buttons");

			lnklblCurrentLoggedIn.Visible = lblCurrentLoggedIn.Visible = addVideosToQueueButton.Enabled = clearVideosButton.Enabled = accountContainer.RegisteredAccounts.Count > 0;
			RefreshToolstripButtonsEnabled();
			lnklblCurrentLoggedIn.Text = accountContainer.RegisteredAccounts.SingleOrDefault()?.Title ?? "Kanaltitel unbekannt";
			btnStart.Enabled = true;
			queueStatusButton.Enabled = true;
		}

		private void BtnStartClick(object sender, EventArgs e)
		{
			Logger.Debug($"Start autouploader button was clicked");

			if (autoUploader.State == RunningState.NotRunning && ConditionsForStartAreFullfilled())
			{
				Logger.Info($"Starting autouploader");

				var publishSettings = pathContainer.ActivePaths
					.Select(path => new ObservationConfiguration(path, templateContainer.RegisteredTemplates.FirstOrDefault(t => t.Id == path.SelectedTemplateId)))
					.ToArray();

				SetUpAutoUploaderAndQueue(publishSettings);

				autoUploader.StartAsync();
			}
			else
			{
				Logger.Info($"Cancelling autouploader");

				canceled = true;
				autoUploader.Cancel(true);
				autoUploader.Uploader.CancelAll();
			}
		}

		private void SetTimeAndStartUploaderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Logger.Info($"Start autouploader with extended settings button was clicked");

			if (autoUploader.State == RunningState.NotRunning && ConditionsForStartAreFullfilled())
			{
				Logger.Info($"Starting autouploader with extended settings");

				ChooseStartTimesForm cstForm = new ChooseStartTimesForm(pathContainer, templateContainer);
				var shouldStartUpload = cstForm.ShowDialog(this);

				if (shouldStartUpload == DialogResult.OK)
				{
					Logger.Info($"Starting autouploader (via extended settings button)");

					SetUpAutoUploaderAndQueue(cstForm.GetPublishSettingsArray());
					autoUploader.StartWithExtraConfigAsync();
				}
			}
			else
			{
				Logger.Info($"Cancelling autouploader (via extended settings button)");

				canceled = true;
				autoUploader.Cancel(true);
				autoUploader.Uploader.CancelAll();
			}
		}

		private void SetUpAutoUploaderAndQueue(IObservationConfiguration[] publishSettings)
		{
			Logger.Info($"Setting up autouploader and youtube queue");

			autoUploader.Account = accountContainer.RegisteredAccounts.First();

			autoUploader.Configuration.Clear();
			foreach (var setting in publishSettings)
			{
				Logger.Info($"Using settings for path '{setting.PathInfo.Fullname}'");
				Logger.Info($"Template to use: {setting.Template.Id} '{setting.Template.Name}'");
				Logger.Info($"Startdate for video publishes: '{setting.StartDate}'");
				Logger.Info($"Should upload videos private '{setting.UploadPrivate}'");

				autoUploader.Configuration.Add(setting);
			}

			jobQueue.Fill(categoryContainer, languageContainer, playlistContainer, playlistServiceConnectionContainer);

			jobQueue.ShowActionsButtons = true;
			jobQueue.Uploader = autoUploader.Uploader;
		}

		private bool ConditionsForStartAreFullfilled()
		{
			Logger.Debug($"Checking if conditions for running the autouploader are fulfilled");

			if (accountContainer.RegisteredAccounts.Count == 0)
			{
				Logger.Error($"Autouploader can't be started without a registered youtube account!");

				MessageBox.Show(this, "Es wurde keine Verbindung zu einem Account hergestellt. Bitte zuerst bei einem Youtube-Konto anmelden!", "Kein Account verbunden!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			if (pathContainer.ActivePaths.Count == 0)
			{
				Logger.Error($"Autouploader can't be started without a path to watch!");

				MessageBox.Show(this, "Es wurden keine Pfade hinzugefügt, die der Uploader überwachen soll und die auf aktiv gesetzt sind. Er würde deshalb nichts hochladen. Bitte zuerst Pfade hinzufügen.", "Keine Pfade vorhanden!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			return true;
		}

		delegate void Action();
		private void UploaderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Logger.Debug($"Received uploader property changed event from youtube uploader. Property: '{e.PropertyName}'");

			if (e.PropertyName == nameof(IYoutubeUploader.State))
			{
				Logger.Info($"Youtube uploader state has changed to: '{autoUploader.Uploader.State}'");

				if (autoUploader.Uploader.State == UploaderState.Waiting)
				{
					Logger.Debug($"Refreshing progress bar");

					Invoke(new Action(() => TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal, Handle)));
					Invoke(new Action(() => TaskbarManager.Instance.SetProgressValue(10000, 10000, Handle)));
				}

				if (autoUploader.State == RunningState.NotRunning && autoUploader.Uploader.State == UploaderState.Waiting
					&& autoUploader.Uploader.Queue.All(j => j.State == JobState.Canceled || j.State == JobState.Error || j.State == JobState.Successful))
				{
					Logger.Info($"Should end now");

					ended = true;
				}

				if (autoUploader.Uploader.State == UploaderState.NotRunning)
				{
					Logger.Info($"Uploader was stopped => should end now");

					ended = true;
					Invoke(new Action(() => queueStatusLabel.Text = "Die Warteschlange ist gestoppt"));
				}
				else
				{
					Logger.Info($"Uploader was started");

					Invoke(new Action(() => queueStatusLabel.Text = "Die Warteschlange wird abgearbeitet"));
				}

				RenameStartButton();
			}
			else if (e.PropertyName == nameof(IYoutubeUploader.Progress))
			{
				progress = autoUploader.Uploader.Progress;

				Logger.Debug($"Refreshing progress in taskbar to: '{progress}'");

				try
				{
					Invoke(new Action(() => TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal, Handle)));
					Invoke(new Action(() => TaskbarManager.Instance.SetProgressValue(progress, 10000, Handle)));
				}
				catch (InvalidOperationException)
				{ }
			}
		}

		private void RenameStartButton()
		{
			if (autoUploader.State == RunningState.NotRunning)
			{
				Logger.Debug($"Renaming autouploader start button text to 'Sofort starten!'");

				Invoke(new Action(() => btnStart.Text = "Sofort starten!"));
				Invoke(new Action(() => zeitenFestlegenUndAutouploaderStartenToolStripMenuItem.Enabled = true));
			}
			else
			{
				Logger.Debug($"Renaming autouploader start button text to 'Abbrechen!'");

				Invoke(new Action(() => btnStart.Text = "Abbrechen!"));
				Invoke(new Action(() => zeitenFestlegenUndAutouploaderStartenToolStripMenuItem.Enabled = false));
			}

			if (autoUploader.Uploader.State == UploaderState.NotRunning)
			{
				Logger.Debug($"Renaming youtube uploader start button text to 'Start!'");

				Invoke(new Action(() => queueStatusButton.Text = "Start!"));
			}
			else
			{
				Logger.Debug($"Renaming youtube uploader start button text to 'Abbrechen!'");

				Invoke(new Action(() => queueStatusButton.Text = "Abbrechen!"));
			}
		}

		private void AutoUploaderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Logger.Debug($"Received autouploader property changed event. Property: '{e.PropertyName}'");

			if (e.PropertyName == nameof(autoUploader.State))
			{
				if (autoUploader.State == RunningState.NotRunning)
				{
					Logger.Info($"Autouploader was stopped => should end now");

					ended = true;
					Invoke(new Action(() => autoUploaderStateLabel.Text = "Der AutoUploader ist gestoppt"));
				}
				else
				{
					Logger.Info($"Autouploader was started");

					Invoke(new Action(() => autoUploaderStateLabel.Text = "Der AutoUploader läuft und fügt gefundene Videos automatisch hinzu"));
				}

				RenameStartButton();
			}
		}

		private void MainFormLoad(object sender, EventArgs e)
		{
			Logger.Debug($"Mainform is loading => running background worker");

			bgwCreateUploader.RunWorkerAsync();
		}

		private void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			Logger.Info($"Attempting to stop the program");

			if (autoUploader.Uploader.State == UploaderState.Uploading)
			{
				Logger.Warn($"Uploader is running => asking if the program really should exit");

				var result = MessageBox.Show(this, $"Aktuell werden Videos hochgeladen! Das Hochladen wird abgebrochen und kann beim nächsten Start des Programms fortgesetzt werden.{Environment.NewLine}{Environment.NewLine}Möchtest du das Programm wirklich schließen?", "Schließen bestätigen", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

				if (result == DialogResult.No)
				{
					Logger.Info($"User decided to not exit the program");

					e.Cancel = true;
					return;
				}
			}

			autoUploader.PropertyChanged -= AutoUploaderPropertyChanged;
			autoUploader.Uploader.PropertyChanged -= UploaderPropertyChanged;

			Logger.Info($"Cancelling autouploader");

			autoUploader?.Cancel(false);
			pathPersistor.Save();
			templatePersistor.Save();

			for (int i = 0; i < queueContainer.RegisteredJobs.Count; i++)
			{
				var job = queueContainer.RegisteredJobs.ElementAt(i);
				if (job.State == JobState.Successful)
				{
					Logger.Info($"Job for video '{job.Video.Title}' was successful => moving to archive");

					queueContainer.UnregisterJobAt(i);
					archiveContainer.RegisterJob(job);
					i--;
				}
				else if (job.State == JobState.Running)
				{
					Logger.Info($"Job for video '{job.Video.Title}' was still running => resetting job to run again on next program execution");

					job.Reset();
				}
			}

			Logger.Info($"Saving queue and archive");

			queuePersistor.Save();

			YoutubeJob.SimplifyLogging = true;
			archivePersistor.Save();
			YoutubeJob.SimplifyLogging = false;

			Logger.Info($"Stopping program");
		}

		private void RevokeAccess()
		{
			Logger.Info($"Revoke access to youtube account");

			tlpSettings.Enabled = false;
			accountCommunicator.RevokeAccount(accountContainer, accountContainer.RegisteredAccounts.Single());
			accountPersistor.Save();

			Logger.Info($"Revokation was successful");

			// MessageBox.Show(this, "Die Verbindung zum Youtube-Account wurde erfolgreich getrennt.", "Verbindung getrennt!", MessageBoxButtons.OK, MessageBoxIcon.Information);

			btnStart.Enabled = false;
			queueStatusButton.Enabled = false;

			lnklblCurrentLoggedIn.Visible = lblCurrentLoggedIn.Visible = accountContainer.RegisteredAccounts.Count > 0;
			RefreshToolstripButtonsEnabled();
			tlpSettings.Enabled = true;
		}

		private void RefreshToolstripButtonsEnabled()
		{
			Logger.Info("Refreshing tool strip buttons enabled state");

			verbindenToolStripMenuItem.Enabled = accountContainer.RegisteredAccounts.Count == 0;
			verbindungLösenToolStripMenuItem.Enabled = templatesToolStripMenuItem1.Enabled = pfadeToolStripMenuItem1.Enabled = playlistsToolStripMenuItem.Enabled = accountContainer.RegisteredAccounts.Count > 0;
		}

		private void BgwCreateUploaderDoWork(object sender, DoWorkEventArgs e)
		{
			Logger.Info("Loading application settings...");

			clientContainer.RegisterClient(YoutubeClientData.Client);

			if (!Directory.Exists("./settings"))
			{
				Logger.Info("Creating settings directory");
				Directory.CreateDirectory("./settings");
            }

            accountPersistor = new AccountPersistor(accountContainer, "./settings/accounts.json", clientContainer);
            accountPersistor.Load();

            IReadOnlyCollection<IYoutubeAccount> registeredAccounts = accountContainer.RegisteredAccounts;

            if (registeredAccounts.Count > 0 && registeredAccounts.First().Access.GetActiveToken() == null)
            {
                File.Delete("./settings/accounts.json");

				BgwCreateUploaderDoWork(sender, e);

				return;
            }

            pathPersistor = new PathPersistor(pathContainer, "./settings/paths.json");
			pathPersistor.Load();

			templatePersistor = new TemplatePersistor(templateContainer, "./settings/templates.json");
			templatePersistor.Load();

            categoryPersistor = new CategoryPersistor(categoryContainer, "./settings/categories.json");
			categoryPersistor.Load();

			languagePersistor = new LanguagePersistor(languageContainer, "./settings/languages.json");
			languagePersistor.Load();

			settingsPersistor = new AutoUploaderSettingsPersistor(autoUploaderSettings, "./settings/autouploader.json");
			settingsPersistor.Load();

			queuePersistor = new JobPersistor(queueContainer, "./settings/queue.json");
			queuePersistor.Load();

			YoutubeJob.SimplifyLogging = true;
			archivePersistor = new JobPersistor(archiveContainer, "./settings/archive.json");
			archivePersistor.Load();
			YoutubeJob.SimplifyLogging = false;

            RefreshPlaylists();

            playlistServiceConnectionPersistor = new PlaylistServiceConnectionPersistor(playlistServiceConnectionContainer, "./settings/playlistservice.json");
			playlistServiceConnectionPersistor.Load();

			if (playlistServiceConnectionContainer.Connection != null && playlistServiceConnectionContainer.Connection.Accounts.Length > 0)
			{
				bool somethingChanged = false;

				foreach (var template in templateContainer.RegisteredTemplates)
				{
					var firstId = playlistServiceConnectionContainer.Connection.Accounts.FirstOrDefault(a => a.Id >= 0)?.Id ?? -1;

					if (template.AccountId == -1 && firstId > -1)
					{
						Logger.Info($"Fix: setting account id for playlist service connection of template '{template.Title}' from -1 to {firstId}");
						template.AccountId = firstId;
						somethingChanged = true;
					}
				}

				if (somethingChanged)
				{
					templatePersistor.Save();
				}
			}

			foreach (var item in queueContainer.RegisteredJobs)
			{
				item.Account = accountContainer.RegisteredAccounts.FirstOrDefault(a => a.Id == item.Account.Id);

				if (item.Account == null)
				{
					var account = accountContainer.RegisteredAccounts.FirstOrDefault();
					Logger.Info($"Fix: saved account for job with video '{item.Video.Title}' could not be found. Using account '{account.Title}' instead.");
					item.Account = account;
				}
			}

			Logger.Info("Creating youtube uploader...");
			var uploader = new YoutubeUploader(queueContainer)
            {
                StopAfterCompleting = false,
                RemoveCompletedJobs = false
            };

            Logger.Info("Creating automation uploader...");
            autoUploader = new AutomationUploader(uploader, archiveContainer, playlistServiceConnectionContainer)
            {
                WatchedProcesses = processes
            };

            autoUploader.PropertyChanged += AutoUploaderPropertyChanged;
			autoUploader.Uploader.PropertyChanged += UploaderPropertyChanged;
			autoUploader.Uploader.NewUploadStarted += UploaderNewUploadStarted;
			autoUploader.FileToUploadOccured += AutoUploader_FileToUploadOccured;

			Logger.Info("Filling job queue...");
			jobQueue.Fill(categoryContainer, languageContainer, playlistContainer, playlistServiceConnectionContainer);
			jobQueue.Uploader = autoUploader.Uploader;

			Logger.Info("Finished loading application settings...");
		}

		private void RefreshPlaylists()
		{
			playlistPersistor = new PlaylistPersistor(playlistContainer, "./settings/playlists.json");
			playlistPersistor.Load();

			if (accountContainer.RegisteredAccounts.Count > 0)
			{
				Logger.Info($"Refreshing playlists of the youtube account");

				playlistContainer.UnregisterAllPlaylists();

                var playlists = new YoutubePlaylistCommunicator().LoadPlaylists(accountContainer.RegisteredAccounts.First());
				foreach (var playlist in playlists)
				{
					Logger.Info($"Found playlist '{playlist.Title}'");
					playlistContainer.RegisterPlaylist(playlist);
				}

				playlistPersistor.Save();

				Logger.Info($"Playlists refreshed");
			}
		}

		private void BgwCreateUploaderRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			cmbbxFinishAction.SelectedIndex = 0;

			limitUploadSpeedCombobox.SelectedIndex = 1;

			RefillSelectedPathsListView();
			RefillArchiveView();
			ActivateAccountLink();

			if (File.Exists("stfu-updater.exe"))
			{
				try
				{
					Logger.Info("Found updater executable from last update, will attempt to remove it");

					File.Delete("stfu-updater.exe");
				}
				catch (Exception ex)
				{
					Logger.Info("Updater executable could not be deleted", ex);
				}
			}

			if (showReleaseNotes || autoUploaderSettings.ShowReleaseNotes)
			{
				Logger.Info("Showing release notes");

				var releaseNotesForm = new ReleaseNotesForm(autoUploaderSettings);
				releaseNotesForm.ShowDialog(this);

				settingsPersistor.Save();
			}

			var updateForm = new UpdateForm();
			if (updateForm.ShowDialog(this) == DialogResult.Yes)
			{
				Logger.Info("Update triggered, closing application so that it can be updated");

				Close();
				return;
			}

			lnklblCurrentLoggedIn.Visible = lblCurrentLoggedIn.Visible = accountContainer.RegisteredAccounts.Count > 0;
			RefreshToolstripButtonsEnabled();

			if (accountContainer.RegisteredAccounts.Count > 0)
			{
				Logger.Info($"Currently logged in: {accountContainer.RegisteredAccounts.SingleOrDefault()?.Title}");
				lnklblCurrentLoggedIn.Text = accountContainer.RegisteredAccounts.SingleOrDefault()?.Title;
			}

			tlpSettings.Enabled = true;

			btnStart.Enabled = queueStatusButton.Enabled = accountContainer.RegisteredAccounts.Count > 0;

			Logger.Info("Application started successfully");
		}

		private void LnklblCurrentLoggedInLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (accountContainer.RegisteredAccounts.Count == 0)
			{
				Logger.Error("Link to currently logged in account was clicked but there is NONE");
				return;
			}

            Process p = new Process
            {
                StartInfo = new ProcessStartInfo(accountContainer.RegisteredAccounts.Single().Uri.AbsoluteUri)
            };
            p.Start();

			Logger.Info($"Link to currently logged in account was clicked. Opening URL: '{accountContainer.RegisteredAccounts.Single().Uri.AbsoluteUri}'");
		}

		private void BeendenToolStripMenuItemClick(object sender, EventArgs e)
		{
			Logger.Info($"Closing application via main menu strip");
			Close();
		}

		private void ChbChoseProcessesCheckedChanged(object sender, EventArgs e)
		{
			btnChoseProcs.Enabled = chbChoseProcesses.Checked;

			if (chbChoseProcesses.Checked)
			{
				Logger.Info($"Selected the option to wait for processes to finish");
				ChoseProcesses();
			}
			else
			{
				Logger.Info($"Unselected the option to wait for processes to finish");
				processes.Clear();
			}
		}

		private void ChoseProcesses()
		{
			Logger.Debug($"Choosing processes to wait for before finishing");

			ProcessForm processChoser = new ProcessForm(processes.Where(p => !p.HasExited).ToList());
			processChoser.ShowDialog(this);

			if (processChoser.DialogResult == DialogResult.OK
				&& processChoser.Selected.Count > 0)
			{
				var procs = processChoser.Selected;
				processes.Clear();
				processes.AddRange(procs);

				Logger.Info($"Chose {procs.Count} processes to wait for before finishing");

				foreach (var proc in procs)
				{
					try
					{
						Logger.Debug($"Added process: '{proc.ProcessName}'");
					}
					catch (Exception)
					{
						// I do not care
					}
				}
			}
			else
			{
				Logger.Info($"No processes to watch were chosen -> unselecting watch for processes checkbox");

				chbChoseProcesses.Checked = false;
			}
		}

		private void CmbbxFinishActionSelectedIndexChanged(object sender, EventArgs e)
		{
			autoUploader.EndAfterUpload = chbChoseProcesses.Enabled = cmbbxFinishAction.SelectedIndex > 0;

			Logger.Info($"Action after uploading combobox index was set to: {cmbbxFinishAction.SelectedIndex}");

			if (cmbbxFinishAction.SelectedIndex == 0)
			{
				Logger.Info($"Action was set to nothing -> clearing processes if available");
				processes.Clear();
				chbChoseProcesses.Checked = false;
			}
		}

		private void BtnChoseProcsClick(object sender, EventArgs e)
		{
			Logger.Debug($"Button to choose processes was clicked");
			ChoseProcesses();
		}

		private void TemplatesToolStripMenuItem1Click(object sender, EventArgs e)
		{
			Logger.Debug($"Button to manage templates was clicked");

			TemplateForm tf = new TemplateForm(templatePersistor,
				categoryContainer,
				languageContainer,
				playlistContainer,
				playlistServiceConnectionContainer,
				accountContainer.RegisteredAccounts.FirstOrDefault()?.Access.FirstOrDefault()?.HasSendMailPrivilegue ?? false);
			tf.ShowDialog(this);

			templatePersistor.Save();

			RefillSelectedPathsListView();
		}

		private void PathsToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Button to manage paths was clicked");

			PathForm pf = new PathForm(pathContainer, templateContainer, queueContainer, archiveContainer, accountContainer);
			pf.ShowDialog(this);

			pathPersistor.Save();
			YoutubeJob.SimplifyLogging = true;
			archivePersistor.Save();
			YoutubeJob.SimplifyLogging = false;

			RefillSelectedPathsListView();
			RefillArchiveView();
		}

		private void ConnectToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Button to connect a youtube account was clicked");

			ConnectToYoutube();
			RefreshToolstripButtonsEnabled();
		}

		private void DeleteConnectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Button to revoke the youtube account was clicked");

			RevokeAccess();
			RefreshToolstripButtonsEnabled();
		}

		private void ThreadImLPFToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Button to open the lpf support link was clicked");

			Process.Start("https://letsplayforum.de/thread/175111-beta-strohis-toolset-f%C3%BCr-uploads-automatisch-videos-hochladen/");
		}

		private void DownloadPageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Button to open the download page link was clicked");

			Process.Start("https://drive.google.com/drive/folders/1ClbLVtOf6uOEEkkujvmUmb9Ya5l7T_Yx");
		}

		private void ThreadImYTFToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Button to open the ytf support link was clicked");

			Process.Start("https://ytforum.de/index.php/Thread/19543-BETA-Strohis-Toolset-Für-Uploads-v0-1-1-Videos-automatisch-hochladen/");
		}

		private void NewFeaturesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Button to show the release notes was clicked");

			var releaseNotesForm = new ReleaseNotesForm(autoUploaderSettings);
			releaseNotesForm.ShowDialog(this);

			settingsPersistor.Save();
		}

		private void OpenLogsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Directory.Exists("logs"))
			{
				Logger.Info($"Creating logs directory before showing it");

				Directory.CreateDirectory("logs");
			}

			Process.Start("explorer.exe", "logs");
		}

		private void WatchingTimer_Tick(object sender, EventArgs e)
		{
			if (ended)
			{
				Logger.Debug($"Timer for watching if the program should end: ended was true");

				if (!canceled)
				{
					Logger.Info($"Uploads ended and were not canceled");

					// Upload wurde regulär beendet.
					switch (cmbbxFinishAction.SelectedIndex)
					{
						case 1:
							Logger.Info($"Closing the program");
							Close();
							return;
						case 2:
							Logger.Info($"Shutting down the computer");

							Process.Start("shutdown.exe", "-s -t 60");
							Close();
							return;
						default:
							Logger.Info($"No special action was requested after ending");
							break;
					}
				}

				Logger.Debug($"resetting ended and canceled flags to false");

				ended = false;
				canceled = false;
			}
		}

		private void QueueStatusButton_Click(object sender, EventArgs e)
		{
			Logger.Info($"Button to start or stop youtube uploader was clicked");

			var uploader = autoUploader.Uploader;

			if (uploader.State == UploaderState.NotRunning)
			{
				if (accountContainer.RegisteredAccounts.Count == 0)
				{
					Logger.Error($"Could not start youtube uploader - there's no account to upload the videos to");

					MessageBox.Show(this, "Es wurde keine Verbindung zu einem Account hergestellt. Bitte zuerst bei einem Youtube-Konto anmelden!", "Kein Account verbunden!", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				Logger.Info($"Starting youtube uploader");

				jobQueue.Fill(categoryContainer, languageContainer, playlistContainer, playlistServiceConnectionContainer);

				jobQueue.ShowActionsButtons = true;
				jobQueue.Uploader = autoUploader.Uploader;

				uploader.StartUploader();
			}
			else
			{
				Logger.Info($"Stopping youtube uploader");

				canceled = true;
				autoUploader.Uploader.CancelAll();
			}
		}

		private void ArchiveRemoveJobButton_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Button to remove jobs from archive was clicked");

			RemoveSelectedArchiveJobs();
		}

		private void RemoveSelectedArchiveJobs()
		{
			for (int i = archiveListView.Items.Count - 1; i >= 0; i--)
			{
				bool isSelected = archiveListView.SelectedIndices.Contains(i);
				if (isSelected)
				{
					Logger.Debug($"Unregistering job for video '{archiveContainer.RegisteredJobs.ElementAt(i).Video.Title}'");

					archiveContainer.UnregisterJobAt(i);
					archiveListView.Items.RemoveAt(i);
				}
			}

			YoutubeJob.SimplifyLogging = true;
			archivePersistor.Save();
			YoutubeJob.SimplifyLogging = false;
		}

		private void VideoTutorialPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Button to open tutorial youtube playlist was clicked");

			Process.Start("https://www.youtube.com/playlist?list=PLm5B9FzOsaWfrn-MeuU_zf7pwooPdPCts");
		}

		private void ArchiveAddButton_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Button to videos to archive was clicked");

			var result = openFileDialog.ShowDialog(this);
			if (result == DialogResult.OK)
			{
				var files = openFileDialog.FileNames;
				Logger.Info($"Adding {files.Length} files to archive");

				foreach (var file in files)
				{
					Logger.Debug($"Adding video '{file}' to archive");
                    var video = new YoutubeVideo(file)
                    {
                        Title = file
                    };
                    archiveContainer.RegisterJob(new YoutubeJob(video, accountContainer.RegisteredAccounts.FirstOrDefault(), new UploadStatus()));
					archiveListView.Items.Add(file);
				}
			}

			YoutubeJob.SimplifyLogging = true;
			archivePersistor.Save();
			YoutubeJob.SimplifyLogging = false;
		}

		private void ArchiveListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			moveBackToQueueButton.Enabled = archiveRemoveJobButton.Enabled = archiveListView.SelectedIndices.Count > 0;
		}

		private void MoveBackToQueueButton_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < archiveListView.SelectedIndices.Count; i++)
			{
				var job = archiveContainer.RegisteredJobs.ElementAt(archiveListView.SelectedIndices[i]);

				Logger.Info($"Moving '{job.Video.Title}' from archive back to queue");

				job.Reset(true);
				job.Account = accountContainer.RegisteredAccounts.First();
				autoUploader.Uploader.QueueUpload(job);
			}

			RemoveSelectedArchiveJobs();
		}

		private void LimitUploadSpeedCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			autoUploader.Uploader.LimitUploadSpeed = limitUploadSpeedCheckbox.Checked;
			Logger.Info($"Checkbox to limit upload speed was ticked and is now: {limitUploadSpeedCheckbox.Checked}");
		}

		private void LimitUploadSpeedNud_ValueChanged(object sender, EventArgs e)
		{
			Logger.Info($"New upload speed limit: {limitUploadSpeedNud.Value} {limitUploadSpeedCombobox.Text}");
			SetNewUploadSpeedLimit();
		}

		private void LimitUploadSpeedCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			Logger.Info($"New upload speed limit: {limitUploadSpeedNud.Value} {limitUploadSpeedCombobox.Text}");
			SetNewUploadSpeedLimit();
		}

		private void SetNewUploadSpeedLimit()
		{
			long value = (long)limitUploadSpeedNud.Value;
			long factor = (long)Math.Pow(1000, limitUploadSpeedCombobox.SelectedIndex + 1);

			Logger.Info($"Setting new upload speed limit: {value * factor} kByte/s");

			autoUploader.Uploader.UploadLimitKByte = value * factor;
		}

		private void AddVideosToQueueButton_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Button to add jobs to the queue manually was clicked");
			AddVideosForm form = new AddVideosForm(templateContainer.RegisteredTemplates.ToArray(), pathContainer.RegisteredPaths.ToArray(), categoryContainer, languageContainer, playlistContainer, playlistServiceConnectionContainer, accountContainer.RegisteredAccounts.First());

			if (form.ShowDialog(this) == DialogResult.OK)
			{
				Logger.Info($"Adding {form.Videos.Count} jobs to uploader queue manually");
				templatePersistor.Save();

				foreach (var videoAndEvaluator in form.Videos)
				{
					Logger.Info($"Adding job for video '{videoAndEvaluator.Video.Title}' to uploader queue manually");

					var video = videoAndEvaluator.Video;
					var evaluator = videoAndEvaluator.Evaluator;
					var notificationSettings = videoAndEvaluator.NotificationSettings;

					var job = autoUploader.Uploader.QueueUpload(video, accountContainer.RegisteredAccounts.First(), notificationSettings);
					var path = form.TemplateVideoCreator.FindNearestPath(video.File.FullName);

					job.UploadCompletedAction += (args) => evaluator.CleanUp().Wait();

					if (path != null && path.MoveAfterUpload)
					{
						Logger.Info($"Adding move after upload action to job");

						job.UploadCompletedAction += (args) => autoUploader.MoveVideo(args.Job, path.MoveDirectoryPath);
					}
				}
			}
		}

		private void ClearVideosButton_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Button clear queue was clicked");
			var result = MessageBox.Show(this, $"Hiermit wird die Warteschlange vollständig geleert, alle laufenden Uploads werden abgebrochen.{Environment.NewLine}{Environment.NewLine}Möchtest du das wirklich tun?", "Warteschlange wirklich leeren?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

			if (result == DialogResult.Yes)
			{
				Logger.Info($"Clearing job queue");

				autoUploader.Uploader.CancelAll();

				while (autoUploader.Uploader.Queue.Count > 0)
				{
					Logger.Info($"Removing job for video '{autoUploader.Uploader.Queue.ElementAt(0).Video.Title}' from queue");

					autoUploader.Uploader.RemoveFromQueue(autoUploader.Uploader.Queue.ElementAt(0));
				}
			}
		}

		private void PlaylistsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Opening playlists form");
			new RefreshPlaylistsForm(playlistPersistor, accountContainer.RegisteredAccounts.First()).Show(this);
		}

		private void PlaylistserviceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Logger.Debug($"Opening playlist service form");
			PlaylistServiceForm form = new PlaylistServiceForm(playlistServiceConnectionContainer, clientContainer.RegisteredClients.FirstOrDefault());
			form.ShowDialog(this);

			playlistServiceConnectionPersistor.Save();
		}

        private void ThreadAufGitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Debug($"Button to open the GitHub support link was clicked");

            Process.Start("https://github.com/tefracky/stfu");
        }
    }
}
