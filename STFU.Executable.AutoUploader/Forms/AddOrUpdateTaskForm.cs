using System;
using System.Linq;
using System.Windows.Forms;
using log4net;
using STFU.Lib.Playlistservice.Model;

namespace STFU.Executable.AutoUploader.Forms
{
	public partial class AddOrUpdateTaskForm : Form
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(AddOrUpdateTaskForm));

		public Task Task { get; private set; }

		public AddOrUpdateTaskForm(Task task)
		{
			Logger.Info($"Initializing new instance of AddOrUpdateTaskForm");

			InitializeComponent();

			Task = task;
			Logger.Debug($"Task to handle: {Task}");
		}

		private void AddOrUpdateTaskForm_Load(object sender, EventArgs e)
		{
			Logger.Info($"Loading form");

			if (Task != null)
			{
				Logger.Info($"Form was created to edit a task. Task to edit: '{Task}'");

				idLabel.Text = Task.Id.ToString();

				playlistIdTextbox.Text = Task.PlaylistId;
				videoIdTextbox.Text = Task.VideoId;
				addDtp.Value = Task.AddAt;

				playlistTitleTextbox.Text = Task.PlaylistTitle;
				videoTitleTextbox.Text = Task.VideoTitle;

				reopenWarningLabel.Visible = Task.State == TaskState.Done;
			}
			else
			{
				Logger.Info($"Form was created to add a new task");

				addDtp.Value = DateTime.Now.Date.AddHours(DateTime.Now.Hour + 1);
				Task = new Task();
			}
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Logger.Info($"User canceled the form");

			Close();
		}

		private void SaveButton_Click(object sender, EventArgs e)
		{
			Logger.Info($"User finished the form");

			DialogResult = DialogResult.OK;

			Task.PlaylistId = GetId(playlistIdTextbox.Text, "list");
			Task.VideoId = GetId(videoIdTextbox.Text, "v");
			Task.AddAt = addDtp.Value;

			Task.PlaylistTitle = playlistTitleTextbox.Text;
			Task.VideoTitle = videoTitleTextbox.Text;

			Logger.Info($"Resulting task: '{Task}'");

			Close();
		}

		private string GetId(string text, string queryParameter)
		{
            if ((text.ToLower().Contains("youtube") || text.ToLower().Contains("youtu.be")) && Uri.TryCreate(text, UriKind.RelativeOrAbsolute, out var uri))
			{
				string queryString = uri.Query;
				var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
				if (queryDictionary.AllKeys.Any(k => k == queryParameter))
				{
					text = queryDictionary[queryParameter];
				}
			}

			return text;
		}
	}
}
