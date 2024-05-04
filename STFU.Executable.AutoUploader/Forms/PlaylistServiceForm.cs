using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using log4net;
using STFU.Lib.GUI.Forms;
using STFU.Lib.Playlistservice;
using STFU.Lib.Playlistservice.Model;
using STFU.Lib.Youtube.Interfaces.Enums;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Model.Helpers;
using STFU.Lib.Youtube.Services;

namespace STFU.Executable.AutoUploader.Forms
{
	public partial class PlaylistServiceForm : Form
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(PlaylistServiceForm));

		private IYoutubeClient Client { get; set; }

		private IPlaylistServiceConnectionContainer PlaylistServiceContainer { get; set; }

		private string Host { get; set; }
		private string Port { get; set; }
		private string Username { get; set; }
		private string Password { get; set; }
		private bool IsConnected { get; set; }

		private AccountClient AccountClient { get; set; }
		private TaskClient TaskClient { get; set; }

		private List<Account> Accounts { get; } = new List<Account>();
		private List<Task> FoundTasks { get; } = new List<Task>();

		public PlaylistServiceForm(IPlaylistServiceConnectionContainer container, IYoutubeClient client)
		{
			Logger.Info($"Initializing new instance of PlaylistServiceForm");

			Client = client;
			PlaylistServiceContainer = container;

			InitializeComponent();
		}

        private void ConnectServiceButton_Click(object sender, EventArgs e)
		{
			Logger.Debug($"User clicked the button to connect to the playlist service");

			ConnectToService();
		}

		private void ConnectToService()
		{
			if (!string.IsNullOrWhiteSpace(hostTextbox.Text) && !string.IsNullOrWhiteSpace(portTextbox.Text) && portTextbox.Text.All(c => "0123456789".Contains(c)))
			{
				var uri = new Uri($"http://{hostTextbox.Text}:{portTextbox.Text}");
				Logger.Info($"Trying to connect to '{uri}'");

				VersionClient client = new VersionClient(uri);
				if (!string.IsNullOrWhiteSpace(usernameTextbox.Text) && !string.IsNullOrWhiteSpace(passwordTextbox.Text))
				{
					Logger.Info($"Using username and password authentification");
					Logger.Debug($"username: '{usernameTextbox.Text}', password: '{passwordTextbox.Text}'");
					client = new VersionClient(new Uri($"http://{hostTextbox.Text}:{portTextbox.Text}"), usernameTextbox.Text, passwordTextbox.Text);
				}

				IsConnected = client.IsAvailable();
				if (IsConnected)
				{
					Logger.Info($"Connection could be established!");
					connectionStatusLabel.BackColor = Color.LightGreen;
					connectionStatusLabel.ForeColor = Color.DarkGreen;
					connectionStatusLabel.Text = "Erfolgreich verbunden";

					Host = hostTextbox.Text;
					Port = portTextbox.Text;
					Username = usernameTextbox.Text;
					Password = passwordTextbox.Text;

					Logger.Info($"Creating account client");

					AccountClient = new AccountClient(new Uri($"http://{Host}:{Port}"));
					if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
					{
						AccountClient = new AccountClient(new Uri($"http://{Host}:{Port}"), Username, Password);
					}

					Logger.Info($"Creating task client");

					TaskClient = new TaskClient(new Uri($"http://{Host}:{Port}"));
					if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
					{
						TaskClient = new TaskClient(new Uri($"http://{Host}:{Port}"), Username, Password);
					}

					ReloadAccounts();
				}
				else
				{
					Logger.Error($"Connection could not be established!");

					connectionStatusLabel.BackColor = Color.FromArgb(255, 192, 192);
					connectionStatusLabel.ForeColor = Color.DarkRed;
					connectionStatusLabel.Text = "Verbindung fehlgeschlagen";
				}

				mainSplitContainer.Enabled = IsConnected;
			}
			else
			{
				Logger.Warn($"User did not provide valid host and port => connection can't be established!");

				MessageBox.Show(this, "Bitte einen gültigen Host und Port angeben!");
			}
		}

		private void ReloadAccounts()
		{
			Logger.Info($"Reloading accounts");

			Account[] accounts = AccountClient.GetAllAccounts();

			Accounts.Clear();
			Accounts.AddRange(accounts);

			Logger.Info($"Added {Accounts.Count} accounts");

			clearAccountsButton.Enabled = Accounts.Count > 0;

			RefillAccountsListView();

			accountsListView.SelectedIndices.Clear();
			if (accountsListView.Items.Count > 0)
			{
				accountsListView.SelectedIndices.Add(0);
			}

			removeAccountButton.Enabled = accountsListView.SelectedIndices.Count > 0;
		}

		private void RefillAccountsListView()
		{
			Logger.Info($"Refilling accounts list view");

			accountsListView.Items.Clear();

			foreach (var account in Accounts)
			{
				Logger.Info($"Adding account '{account.Title}'");

				accountsListView.Items.Add(new ListViewItem(account.Title));
			}

			clearAccountsButton.Enabled = Accounts.Count > 0;
		}

        private void ConnectAccountButton_Click(object sender, EventArgs e)
		{
			Logger.Info($"User attempts to connect a new account to the service");

            AddYoutubeAccountForm form = new AddYoutubeAccountForm(false)
            {
                ExternalCodeUrl = new YoutubeAccountCommunicator().CreateAuthUri(Client, YoutubeRedirectUri.Localhost, GoogleScope.Manage).AbsoluteUri
            };

            if (form.ShowDialog(this) == DialogResult.OK)
			{
				Logger.Info($"Adding new account");

                var code = new AuthCode
                {
                    ClientId = Client.Id,
                    ClientSecret = Client.Secret,
                    RedirectUri = YoutubeRedirectUri.Localhost.GetAttribute<EnumMemberAttribute>().Value,
                    Code = form.AuthToken
                };
                Account account = AccountClient.AddAccount(code);

				Accounts.Add(account);
				accountsListView.Items.Add(new ListViewItem(account.Title));
				accountsListView.SelectedIndices.Clear();
				accountsListView.SelectedIndices.Add(Accounts.Count - 1);

				Logger.Info($"Added account '{account.Title}'");

				clearAccountsButton.Enabled = true;
			}
			else
			{
				Logger.Info($"User did not finish add youtube account form");
			}
		}

        private void AccountsListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			Logger.Debug($"User selected a new entry");

			bool selected = accountsListView.SelectedIndices.Count == 1;

			removeAccountButton.Enabled = accountDetailsTlp.Enabled = selected;

			if (selected)
			{
				Account account = Accounts[accountsListView.SelectedIndices[0]];

				Logger.Debug($"User selected account '{account.Title}'");

				accountIdLabel.Text = account.Id.ToString();
				channelTitleLabel.Text = account.Title;
				channelUrlLinkLabel.Text = $"https://youtube.com/channel/{account.ChannelId}";

				ResetTaskFilters();
			}
			else
			{
				Logger.Debug($"User did not select any account");
			}
		}

		private void ResetTaskFilters()
		{
			Logger.Debug($"Resetting task filters");

			filterIdTextbox.Text = "";

			filterTaskdateAfterDtp.Value = DateTime.Now.Date;
			filterTaskdateBeforeDtp.Value = DateTime.Now.AddMonths(1).Date;

			filterAttemptCountTextbox.Text = "";
			filterMinAttemptCountTextbox.Text = "";
			filterMaxAttemptCountTextbox.Text = "";

			filterPlaylistIdTextbox.Text = "";
			filterPlaylistTitleTextbox.Text = "";

			filterVideoIdTextbox.Text = "";
			filterVideoTitleTextbox.Text = "";

			sortByCombobox.SelectedIndex = 0;
			sortOrderCombobox.SelectedIndex = 0;

			showOpenTasksCheckbox.Checked = true;
			showDoneTasksCheckbox.Checked = false;
			showFailedTasksCheckbox.Checked = false;

			RefreshTasks();
		}

		private void RefreshTasks()
		{
			Logger.Info($"User attempts to load tasks from the service");

			FoundTasks.Clear();

			long[] ids = new string(filterIdTextbox.Text.Replace(',', ';').Where(c => ";0123456789".Contains(c)).ToArray())
				.Split(';')
				.Where(c => !string.IsNullOrEmpty(c))
				.Select(c => Convert.ToInt64(c))
				.ToArray();

			int? attemptCount = null;
			int? minAttemptCount = null;
			int? maxAttemptCount = null;

			List<TaskState> states = new List<TaskState>();
			if (showOpenTasksCheckbox.Checked) states.Add(TaskState.Open);
			if (showDoneTasksCheckbox.Checked) states.Add(TaskState.Done);
			if (showFailedTasksCheckbox.Checked) states.Add(TaskState.Failed);

			FoundTasks.AddRange(TaskClient.GetTasks(Accounts[accountsListView.SelectedIndices[0]].Id, ids, filterTaskdateAfterDtp.Value,
				filterTaskdateBeforeDtp.Value, attemptCount, minAttemptCount, maxAttemptCount, filterPlaylistIdTextbox.Text,
				filterPlaylistTitleTextbox.Text, filterVideoIdTextbox.Text, filterVideoTitleTextbox.Text, states.ToArray(), (TaskOrder)sortByCombobox.SelectedIndex,
				(TaskOrderDirection)sortOrderCombobox.SelectedIndex));

			Logger.Info($"Playlistservice returned {FoundTasks.Count} tasks");

			RefillTasksListView();
		}

		private void RefillTasksListView()
		{
			Logger.Debug($"Refilling task list view");

			tasksListView.Items.Clear();

			foreach (var task in FoundTasks)
			{
				string state = "Offen";
				if (task.State == TaskState.Done) state = "Erledigt";
				if (task.State == TaskState.Failed) state = "Gescheitert";

				ListViewItem item = new ListViewItem(task.Id.ToString());
				item.SubItems.Add(task.PlaylistTitle);
				item.SubItems.Add(task.VideoTitle);
				item.SubItems.Add(task.AddAt.ToString("yyyy-MM-dd HH\\:mm"));
				item.SubItems.Add(state);
				item.SubItems.Add(task.AttemptCount.ToString());

				tasksListView.Items.Add(item);

				Logger.Debug($"Added task with values: {task}");
			}

			clearTasksButton.Enabled = FoundTasks.Count > 0;
		}

        private void RemoveAccountButton_Click(object sender, EventArgs e)
		{
			Account account = Accounts[accountsListView.SelectedIndices[0]];
			Logger.Info($"Removing account: '{account}'");

			AccountClient.DeleteAccount(account);

			ReloadAccounts();
		}

        private void ClearAccountsButton_Click(object sender, EventArgs e)
		{
			Logger.Info($"Deleting all registered accounts");

			AccountClient.DeleteAllAccounts();

			ReloadAccounts();
		}

        private void SearchButton_Click(object sender, EventArgs e)
		{
			Logger.Debug($"User wants to refresh search");

			RefreshTasks();
		}

        private void ChannelUrlLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Logger.Info($"Opening link '{channelUrlLinkLabel.Text}'");

			Process.Start(channelUrlLinkLabel.Text);
		}

        private void AddTaskButton_Click(object sender, EventArgs e)
		{
			Logger.Info($"User wants to add a new task");

			AddOrUpdateTaskForm form = new AddOrUpdateTaskForm(null);
			if (form.ShowDialog(this) == DialogResult.OK)
			{
				Task created = TaskClient.CreateTask(Accounts[accountsListView.SelectedIndices[0]].Id, form.Task);

				RefreshTasks();

				if (FoundTasks.Any(t => t.Id == created.Id))
				{
					Logger.Info($"Selecting the newly created task");

					tasksListView.SelectedIndices.Clear();
					tasksListView.SelectedIndices.Add(FoundTasks.IndexOf(FoundTasks.First(t => t.Id == created.Id)));
				}
				else
				{
					MessageBox.Show(this, "Task angelegt", "Der Task wurde erfolgreich angelegt.");
				}
			}
		}

        private void TasksListView_DoubleClick(object sender, EventArgs e)
		{
			Logger.Debug($"User wants to edit a task");

			if (tasksListView.SelectedIndices.Count == 1)
			{
				Logger.Info($"Editing task '{FoundTasks[tasksListView.SelectedIndices[0]]}'");

				AddOrUpdateTaskForm form = new AddOrUpdateTaskForm(FoundTasks[tasksListView.SelectedIndices[0]]);
				if (form.ShowDialog(this) == DialogResult.OK)
				{
					Logger.Info($"Updating task");

					Task updated = TaskClient.UpdateTask(Accounts[accountsListView.SelectedIndices[0]].Id, form.Task);

					Logger.Info($"Updated task, new value '{updated}'");

					RefreshTasks();

					if (FoundTasks.Any(t => t.Id == updated.Id))
					{
						Logger.Info($"Selecting the updated task");

						tasksListView.SelectedIndices.Clear();
						tasksListView.SelectedIndices.Add(FoundTasks.IndexOf(FoundTasks.First(t => t.Id == updated.Id)));
					}
					else
					{
						MessageBox.Show(this, "Task aktualisiert", "Der Task wurde erfolgreich aktualisiert.");
					}
				}
				else
				{
					Logger.Info($"Edit dialog was canceled by the user");
				}
			}
		}

        private void RemoveTaskButton_Click(object sender, EventArgs e)
		{
			Logger.Debug($"User attempts to remove a task");

			if (tasksListView.SelectedIndices.Count == 1)
			{
				var index = tasksListView.SelectedIndices[0];
				var selectedTask = FoundTasks[index];

				Logger.Info($"Removing task '{selectedTask}'");

				if (TaskClient.DeleteTask(Accounts[accountsListView.SelectedIndices[0]].Id, selectedTask.Id))
				{
					Logger.Info($"Task was successfully removed");

					FoundTasks.RemoveAt(index);
					tasksListView.Items.RemoveAt(index);
				}
				else
				{
					Logger.Error($"Task could not be removed");
				}
			}
		}

        private void ClearTasksButton_Click(object sender, EventArgs e)
		{
			Logger.Info($"User clears all tasks");

			for (int index = 0; index < FoundTasks.Count; index++)
			{
				var selectedTask = FoundTasks[index];

				if (TaskClient.DeleteTask(Accounts[accountsListView.SelectedIndices[0]].Id, selectedTask.Id))
				{
					Logger.Info($"Removed task '{selectedTask}'");

					FoundTasks.RemoveAt(index);
					tasksListView.Items.RemoveAt(index);
					index--;
				}
			}
		}

        private void TasksListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			removeTaskButton.Enabled = tasksListView.SelectedIndices.Count == 1;
		}

		private void PlaylistServiceForm_Load(object sender, EventArgs e)
		{
			Logger.Info($"Loading playlist service form");

			if (PlaylistServiceContainer != null && PlaylistServiceContainer.Connection != null)
			{
				hostTextbox.Text = PlaylistServiceContainer.Connection.Host;
				portTextbox.Text = PlaylistServiceContainer.Connection.Port;
				usernameTextbox.Text = PlaylistServiceContainer.Connection.Username;
				passwordTextbox.Text = PlaylistServiceContainer.Connection.Password;

				Logger.Info($"Found stored connection, attempting to connect to service directly");

				ConnectToService();
			}
		}

		private void PlaylistServiceForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Logger.Info($"Closing the form");

			if (IsConnected)
			{
				Logger.Info($"Saving connection");

				PlaylistServiceContainer.Connection = new PlaylistServiceConnection
                {
                    Host = Host,
                    Port = Port,
                    Username = Username,
                    Password = Password,
                    Accounts = Accounts.ToArray()
                };
            }
			else
			{
				Logger.Info($"Removing connection");

				PlaylistServiceContainer.Connection = null;
			}
		}
	}
}
