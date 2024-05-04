using System;
using System.Windows.Forms;
using log4net;

namespace STFU.Executable.AutoUploader.Forms
{
	public partial class AddPlannedVideoForm : Form
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(AddPlannedVideoForm));

		public string Filename { get; private set; }

		public AddPlannedVideoForm()
		{
			Logger.Info($"Initializing new instance of AddPlannedVideoForm");

			InitializeComponent();
			DialogResult = DialogResult.Cancel;
		}

		private void SaveButtonClick(object sender, EventArgs e)
		{
			Filename = filenameBox.Text;
			DialogResult = DialogResult.OK;

			Logger.Info($"User chose to add the following planned video: '{Filename}'");

			Close();
		}

		private void CancelButtonClick(object sender, EventArgs e)
		{
			Logger.Info($"User chose to cancel AddPlannedVideoForm");

			Close();
		}
	}
}
