using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;

namespace STFU.Executable.AutoUploader.Forms
{
	public partial class ProcessForm : Form
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(ProcessForm));

		public IReadOnlyCollection<Process> Selected { get { return selectedProcesses; } }

		private readonly List<Process> selectedProcesses = new List<Process>();
		private Process[] AllProcesses { get; set; }
		private bool reactToCheckedEvents = true;


		public ProcessForm(IReadOnlyCollection<Process> selected)
		{
			Logger.Info($"Initializing new instance of ProcessForm with {selected.Count} already selected procs");

			InitializeComponent();

			selectedProcesses = selected.ToList();
		}

		private void ProcessFormLoad(object sender, EventArgs e)
		{
			Logger.Info($"Loading process form");

			RefreshAllProcsAsync();
		}

        readonly List<string> titles = new List<string>();

		private async void RefreshAllProcsAsync()
		{
			reactToCheckedEvents = false;
			lvProcs.BeginUpdate();

			lvProcs.Items.Clear();

			List<ListViewItem> items = new List<ListViewItem>();

			await Task.Run(() =>
			{
				var currentSessionId = Process.GetCurrentProcess().SessionId;

				Logger.Debug($"Current session id: {currentSessionId}");

				AllProcesses = Process.GetProcesses()
					.OrderBy(item => item.ProcessName)
					.Where(p => HasAccess(p) && p.SessionId == currentSessionId && p.Id != Process.GetCurrentProcess().Id)
					.ToArray();

				Logger.Info($"Found {AllProcesses.Length} processes");

				foreach (var item in AllProcesses)
				{
					Logger.Info($"Adding process '{item.ProcessName}' to the list");

					ListViewItem newItem = new ListViewItem(string.Empty);
					newItem.SubItems.Add(item.ProcessName);

					if (selectedProcesses.Any(proc => item.Id == proc.Id))
					{
						Logger.Info($"Process was already selected => marking checkbox");
						newItem.Checked = true;
					}

					try
					{
						newItem.SubItems.Add(item.MainModule.FileVersionInfo.FileDescription);
					}
					catch (Exception ex)
					{
						Logger.Debug($"Couldn't add process file description to list view item", ex);
					}

					items.Add(newItem);
				}
			});

			lvProcs.Items.AddRange(items.ToArray());

			lvProcs.Columns[0].Width = -1;
			lvProcs.Columns[1].Width = -1;
			lvProcs.Columns[2].Width = -2;

			lvProcs.EndUpdate();
			reactToCheckedEvents = true;
		}

		private bool HasAccess(Process p)
		{
			var result = false;

			try
			{
				Logger.Debug($"Checking access status for process '{p.ProcessName}'");

				// let it go true only if the process is accessable
				result = p.HasExited || true;

				Logger.Debug($"Does the uploader has access to the status: {result}");
			}
			catch (Exception ex)
			{
				Logger.Debug($"Couldn't access the processes has exitec status", ex);
			}

			return result;
		}

		private void BtnRefreshClick(object sender, EventArgs e)
		{
			Logger.Debug($"User wants to refresh the process list");
			RefreshAllProcsAsync();
		}

		private void LvProcsItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (!reactToCheckedEvents)
			{
				return;
			}

			Process item = AllProcesses[e.Item.Index];

			if (selectedProcesses.Any(proc => proc.Id == item.Id))
			{
				Logger.Info($"User removed process '{item.ProcessName}' from the process list");
				selectedProcesses.RemoveAll(proc => proc.Id == item.Id);
			}
			else
			{
				Logger.Info($"User added process '{item.ProcessName}' to the process list");
				selectedProcesses.Add(item);
			}
		}

		private void BtnSubmitClick(object sender, EventArgs e)
		{
			Logger.Info($"User accepted the dialog, {Selected.Count} processes should be watched");
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
