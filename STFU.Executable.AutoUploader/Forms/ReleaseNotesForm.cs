using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using log4net;
using STFU.Lib.Youtube.Persistor.Model;

namespace STFU.Executable.AutoUploader.Forms
{
	public partial class ReleaseNotesForm : Form
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(ReleaseNotesForm));

		private readonly string filename = "ReleaseNotes.rtf";
		private readonly AutoUploaderSettings settings = null;

		public ReleaseNotesForm(AutoUploaderSettings settings)
		{
			Logger.Info("Initializing release notes form");

			InitializeComponent();
			this.settings = settings;

			disableNotesCheckbox.Checked = settings.ShowReleaseNotes;
		}

		private void ReleaseNotesFormLoad(object sender, EventArgs e)
		{
			if (File.Exists(filename))
			{
				Logger.Info("Loading and showing release notes");

				releaseNotesBox.LoadFile(filename);
			}
			else
			{
				Logger.Warn("No release notes found -> close the window immediately");

				Close();
			}
		}

		private void CloseButtonClick(object sender, EventArgs e)
		{
			Logger.Info("Closing release notes");
			Logger.Info($"Should the release notes be showed again next program start: {settings.ShowReleaseNotes}");

			Close();
		}

		private void DisableNotesCheckboxCheckedChanged(object sender, EventArgs e)
		{
			settings.ShowReleaseNotes = disableNotesCheckbox.Checked;
			Logger.Debug($"Changed show release notes setting to: {settings.ShowReleaseNotes}");
		}

		private void ReleaseNotesBoxLinkClicked(object sender, LinkClickedEventArgs e)
		{
			Logger.Debug($"Link to: '{settings.ShowReleaseNotes}' was clicked");
			Process.Start(e.LinkText);
		}
	}
}
