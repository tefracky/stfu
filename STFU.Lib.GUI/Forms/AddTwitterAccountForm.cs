using System;
using System.Diagnostics;
using System.Windows.Forms;
using STFU.Lib.Twitter;

namespace STFU.Lib.GUI.Forms
{
	public partial class AddTwitterAccountForm : Form
	{
		public string AuthPin { get; private set; }

		public TwitterAccountConnector Communicator { get; set; }

		private string BrowseUrl { get; set; }

		public AddTwitterAccountForm()
		{
			InitializeComponent();
		}

		private void AddAccountFormLoad(object sender, EventArgs e)
		{
			BrowseUrl = Communicator.CreateBrowseableUrl();
			useExternalLinkTextbox.Text = BrowseUrl;
		}

		private void UseExternalLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(BrowseUrl);
		}

		private void SignInButtonClick(object sender, EventArgs e)
		{
			AuthPin = useExternalCodeTextbox.Text;
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
