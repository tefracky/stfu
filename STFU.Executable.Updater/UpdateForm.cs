using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using log4net;

namespace STFU.Executable.Updater
{
	public partial class UpdateForm : Form
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(UpdateForm));

		private readonly string zipFile;
		private readonly string executableFile;

		private readonly Updater updater = new Updater();

		public UpdateForm(string zipFile, string executableFile)
		{
			Logger.Info("Initializing update form");

			InitializeComponent();

			this.zipFile = zipFile;
			this.executableFile = executableFile;

			installUpdateBgw.RunWorkerAsync();
		}

		private void RefreshStatusTextTimerTick(object sender, EventArgs e)
		{
			statusLabel.Text = updater.Message;
		}

		private void InstallUpdateBgwDoWork(object sender, DoWorkEventArgs e)
		{
			var procs = Process.GetProcesses().Where(p => ProcessBlocksExe(p)).ToArray();

			Logger.Info("Waiting for blocking processes to exit");

			while (procs.Any(p => !p.HasExited))
			{
				Thread.Sleep(100);
			}

			Logger.Info("All processes exited -> extracting zip");

			updater.ExtractUpdate(zipFile);
		}

		private bool ProcessBlocksExe(Process p)
		{
			var result = false;

			try
			{
				string fullPath = p.MainModule.FileName;
				if (Path.GetFullPath(fullPath).ToLower() == Path.GetFullPath(executableFile).ToLower())
				{
					Logger.Info($"Found blocking process '{p.ProcessName}'");

					result = true;
				}
			}
			catch (Exception)
			{}

			return result;
		}

		private void InstallUpdateBgwRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (updater.Successfull)
			{
				refreshStatusTextTimer.Enabled = false;

				Logger.Info("Updater finished sucessfully");

				if (File.Exists(executableFile))
				{
					Logger.Info($"Starting executable '{executableFile}'");

					Process.Start(executableFile, "showReleaseNotes");
				}

				Logger.Info("Closing the form");
				Close();
			}
			else
			{
				Logger.Info("Updater did not finish sucessfully!");
			}
		}
	}
}
