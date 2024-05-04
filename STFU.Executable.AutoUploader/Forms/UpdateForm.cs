using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using STFU.Lib.Updater;

namespace STFU.Executable.AutoUploader.Forms
{
	public partial class UpdateForm : Form
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(UpdateForm));

		private readonly IUpdater updater;
		private bool updateAvailable;

		public FileInfo Executable { get; private set; }

		private string status = $"Es wird nach Updates gesucht.{Environment.NewLine}Bitte warten...";

		public UpdateForm()
		{
			Logger.Info("Initializing update form");

			InitializeComponent();
			DialogResult = DialogResult.No;

			updater = new Updater(ProductVersion);
			searchUpdateBgw.RunWorkerAsync();
		}

		private void RefreshStatusTextTimerTick(object sender, EventArgs e)
		{
			statusLabel.Text = status;
		}

		private void SearchUpdateBgwDoWork(object sender, DoWorkEventArgs e)
		{
			Logger.Info("Checking for updates");
			Logger.Info($"Current version: v{ProductVersion}");

			updateAvailable = updater.UpdateAvailable;
		}

		private void SearchUpdateBgwRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Logger.Info($"Finished search");

			if (updateAvailable)
			{
				Logger.Info($"Update available - asking user if they want to update to version 'v{updater.NewVersion}'");

				var installUpdate = MessageBox.Show(this, $"Ein Update auf Version {updater.NewVersion} ist nun verfügbar.{Environment.NewLine}{Environment.NewLine}Soll es heruntergeladen und installiert werden?", "Update verfügbar!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (installUpdate == DialogResult.Yes)
				{
					Logger.Info($"Starting update");

					downloadUpdateBgw.RunWorkerAsync();
				}
				else
				{
					Logger.Info($"Closing update form - user does not want to update");

					Close();
				}
			}
			else
			{
				Logger.Info($"Closing update form - no update available");

				Close();
			}
		}

		private void DownloadUpdateBgwDoWork(object sender, DoWorkEventArgs e)
		{
			Logger.Info($"Downloading update zip");

			status = $"Update wird heruntergeladen.{Environment.NewLine}Bitte warten...";
			updater.DownloadUpdate();

			Logger.Info($"Extracting updater from zip file");

			status = $"Updater wird extrahiert. Programm startet anschließend neu.{Environment.NewLine}Bitte warten...";
			Executable = updater.ExtractUpdateExe();
		}

		private void DownloadUpdateBgwRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			refreshStatusTextTimer.Enabled = false;

			if (Executable != null && Executable.Exists)
			{
				DialogResult = DialogResult.Yes;

				var app = Path.GetFullPath(Assembly.GetEntryAssembly().Location);

				string arguments = $"\"{updater.UpdateFile.FullName}\" \"{app}\"";
				Logger.Info($"Starting updater with arguments: '{Executable.FullName} {arguments}'");

				ProcessStartInfo info = new ProcessStartInfo(Executable.FullName, arguments);
				Process.Start(info);
			}
			else
			{
				Logger.Error($"There was an unkown error and the updater could either not be downloaded or not be extracted. Cancelling update");

				MessageBox.Show(this, $"Es gab leider einen Fehler beim Herunterladen des Updates, daher kann es nicht installiert werden.", "Fehler beim Download!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			Close();
		}
	}
}
