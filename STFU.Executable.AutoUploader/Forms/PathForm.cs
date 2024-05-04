using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using log4net;
using STFU.Lib.Youtube.Automation.Interfaces;
using STFU.Lib.Youtube.Automation.Interfaces.Model;
using STFU.Lib.Youtube.Interfaces;

namespace STFU.Executable.AutoUploader.Forms
{
	public partial class PathForm : Form
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(PathForm));

        readonly IPathContainer pathContainer = null;
        readonly ITemplateContainer templateContainer = null;
        readonly IYoutubeJobContainer queueContainer = null;
        readonly IYoutubeJobContainer archiveContainer = null;
        readonly IYoutubeAccountContainer accountContainer = null;

		public PathForm(IPathContainer pathContainer, ITemplateContainer templateContainer, IYoutubeJobContainer queueContainer, IYoutubeJobContainer archiveContainer, IYoutubeAccountContainer accountContainer)
		{
			Logger.Info($"Initializing new instance of PathForm");

			InitializeComponent();

			this.pathContainer = pathContainer;
			this.templateContainer = templateContainer;

			this.queueContainer = queueContainer;
			this.archiveContainer = archiveContainer;
			this.accountContainer = accountContainer;
		}

		private void LvSelectedPathsKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Delete && ItemSelected())
			{
				Logger.Info($"Deleting path on position {lvPaths.SelectedIndices[0]} via delete key");
				Logger.Info($"Path to delete: '{pathContainer.RegisteredPaths.ElementAt(lvPaths.SelectedIndices[0])}'");

				pathContainer.UnregisterPathAt(lvPaths.SelectedIndices[0]);
				RefillListView();
				ClearEditBox();
			}
		}

		private void RefillListView()
		{
			Logger.Info($"Refilling list view");

			lvPaths.Items.Clear();

			foreach (var entry in pathContainer.RegisteredPaths)
			{
				var newItem = lvPaths.Items.Add(entry.Fullname);
				newItem.SubItems.Add(entry.Filter);

				Logger.Debug($"Adding entry for path: '{entry}'");

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
			}

			lvPaths.SelectedIndices.Clear();
		}

		private void RefillEditBox()
		{
			Logger.Debug($"Refilling edit box");

			if (NoItemSelected())
			{
				Logger.Debug($"No item selected => will only clear the edit box");

				ClearEditBox();
				return;
			}

			tlpEditPaths.Enabled = true;
			int index = lvPaths.SelectedIndices[0];

			var selectedItem = pathContainer.RegisteredPaths.ElementAt(index);

			Logger.Debug($"Path to fill into edit box: {selectedItem}");

			txtbxAddPath.Text = selectedItem.Fullname;
			txtbxAddFilter.Text = selectedItem.Filter;
			chbHidden.Checked = selectedItem.SearchHidden;
			chbRecursive.Checked = selectedItem.SearchRecursively;
			chbHidden.Enabled = chbRecursive.Checked;
			deactivateCheckbox.Checked = selectedItem.Inactive;
			moveAfterUploadCheckbox.Checked = selectedItem.MoveAfterUpload;
			moveAfterUploadTextbox.Text = selectedItem.MoveDirectoryPath;

			searchOrderCombobox.SelectedIndex = (int)selectedItem.SearchOrder;

			if (templateContainer.RegisteredTemplates.Any(t => t.Id == selectedItem.SelectedTemplateId))
			{
				cobSelectedTemplate.SelectedIndex = templateContainer.RegisteredTemplates.ToList().IndexOf(templateContainer.RegisteredTemplates.First(t => t.Id == selectedItem.SelectedTemplateId));
			}
			else
			{
				cobSelectedTemplate.SelectedIndex = templateContainer.RegisteredTemplates.ToList().IndexOf(templateContainer.RegisteredTemplates.First(t => t.Id == 0));
			}
		}

		private void ClearEditBox()
		{
			Logger.Debug($"Clearing edit box");

			tlpEditPaths.Enabled = false;
			txtbxAddPath.Text = string.Empty;
			txtbxAddFilter.Text = string.Empty;
			chbRecursive.Checked = false;
			chbHidden.Checked = false;
			chbHidden.Enabled = false;
			deactivateCheckbox.Checked = false;
			moveAfterUploadCheckbox.Checked = false;
			moveAfterUploadTextbox.Text = "";
			searchOrderCombobox.SelectedIndex = 0;
		}

		private void PathFormLoad(object sender, EventArgs e)
		{
			Logger.Info($"Loading path form");

			foreach (var template in templateContainer.RegisteredTemplates)
			{
				string templateName = string.IsNullOrWhiteSpace(template.Name) ? "<namenloses Template>" : template.Name;
				Logger.Info($"Adding template into template combobox: '{templateName}'");
				cobSelectedTemplate.Items.Add(templateName);
			}

			RefillListView();
		}

		private void AddPathButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"User pressed button to add a new path");

			var result = folderBrowserDialog.ShowDialog(this);
			if (result == DialogResult.OK)
			{
				Logger.Info($"Trying to add a new path");

				if (Directory.Exists(folderBrowserDialog.SelectedPath) && !pathContainer.RegisteredPaths.Any(path => path.Fullname == folderBrowserDialog.SelectedPath))
				{
					var newPath = new Lib.Youtube.Automation.Paths.Path
                    {
                        Fullname = folderBrowserDialog.SelectedPath,
                        Filter = "*.mp4;*.mkv",
                        SelectedTemplateId = 0,
                        SearchRecursively = false,
                        Inactive = false,
                        SearchHidden = false
                    };

                    Logger.Info($"Adding newly created path: '{newPath}'");

					pathContainer.RegisterPath(newPath);
					RefillListView();
					lvPaths.SelectedIndices.Add(lvPaths.Items.Count - 1);
				}
				else
				{
					Logger.Error($"Could not add path '{folderBrowserDialog.SelectedPath}': either it doesn't exist or it's already part of the path array.");
				}
			}
		}

		private void DeletePathButtonClick(object sender, EventArgs e)
		{
			if (NoItemSelected())
			{
				Logger.Error($"No path was selected => can't delete any path");

				return;
			}

			Logger.Info($"Deleting path on position {lvPaths.SelectedIndices[0]} via delete button");
			Logger.Info($"Path to delete: '{pathContainer.RegisteredPaths.ElementAt(lvPaths.SelectedIndices[0])}'");

			pathContainer.UnregisterPathAt(lvPaths.SelectedIndices[0]);
			RefillListView();
			ClearEditBox();
		}

		private void LvSelectedPathsSelectedIndexChanged(object sender, EventArgs e)
		{
			RefillEditBox();
		}

		private void BtnSaveClick(object sender, EventArgs e)
		{
			if (NoItemSelected())
			{
				Logger.Error($"No path was selected => can't save any path");

				return;
			}

			int index = lvPaths.SelectedIndices[0];

			var selectedItem = pathContainer.RegisteredPaths.ElementAt(index);

			selectedItem.Fullname = txtbxAddPath.Text;
			selectedItem.Filter = txtbxAddFilter.Text;
			selectedItem.SearchHidden = chbHidden.Checked;
			selectedItem.SearchRecursively = chbRecursive.Checked;
			selectedItem.SelectedTemplateId = templateContainer.RegisteredTemplates.ElementAt(cobSelectedTemplate.SelectedIndex)?.Id ?? 0;
			selectedItem.Inactive = deactivateCheckbox.Checked;
			selectedItem.MoveAfterUpload = moveAfterUploadCheckbox.Checked;
			selectedItem.MoveDirectoryPath = moveAfterUploadTextbox.Text;
			selectedItem.SearchOrder = (FoundFilesOrderByFilter)searchOrderCombobox.SelectedIndex;

			Logger.Info($"Saving edited path: {selectedItem}");

			ClearEditBox();
			RefillListView();
		}

		private bool NoItemSelected()
		{
			return lvPaths.SelectedItems.Count == 0;
		}

		private bool ItemSelected()
		{
			return lvPaths.SelectedItems.Count > 0;
		}

		private void BtnSelectPathClick(object sender, EventArgs e)
		{
			Logger.Debug($"User wants to change directory, which is currently: '{txtbxAddPath.Text}'");

			var result = folderBrowserDialog.ShowDialog(this);
			if (result == DialogResult.OK)
			{
				if (Directory.Exists(folderBrowserDialog.SelectedPath)
					&& (!pathContainer.RegisteredPaths.Any(path => path.Fullname == folderBrowserDialog.SelectedPath)
					|| folderBrowserDialog.SelectedPath == pathContainer.RegisteredPaths.ElementAt(lvPaths.SelectedIndices[0]).Fullname))
				{
					Logger.Info($"Changing directory of the path from: '{txtbxAddPath.Text}' to: '{folderBrowserDialog.SelectedPath}'");

					txtbxAddPath.Text = folderBrowserDialog.SelectedPath;
				}
			}
		}

		private void MovePathUpButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"User wants move a path upwards");

			if (NoItemSelected())
			{
				Logger.Error($"There was no path selected. Can't move one upwards!");

				return;
			}

			var index = lvPaths.SelectedIndices[0];

			pathContainer.ShiftPathPositionsAt(index, index - 1);

			RefillListView();
			lvPaths.SelectedIndices.Clear();
			lvPaths.SelectedIndices.Add(index - 1);
		}

		private void MovePathDownButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"User wants move a path downwards");

			if (NoItemSelected())
			{
				Logger.Error($"There was no path selected. Can't move one downwards!");

				return;
			}

			var index = lvPaths.SelectedIndices[0];

			pathContainer.ShiftPathPositionsAt(index, index + 1);

			RefillListView();
			lvPaths.SelectedIndices.Clear();
			lvPaths.SelectedIndices.Add(index + 1);
		}

		private void ClearButtonClick(object sender, EventArgs e)
		{
			Logger.Info($"User wants to clear all paths");

			pathContainer.UnregisterAllPaths();
			RefillListView();
			ClearEditBox();
		}

		private void BtnCancelClick(object sender, EventArgs e)
		{
			Logger.Info($"User wants to discard his changes of the current path");

			ClearEditBox();
			lvPaths.SelectedIndices.Clear();
		}

		private void ChbRecursiveCheckedChanged(object sender, EventArgs e)
		{
			chbHidden.Enabled = chbRecursive.Checked;

			Logger.Info($"User wants to mark all videos in path '{pathContainer.RegisteredPaths.ElementAt(lvPaths.SelectedIndices[0]).Fullname}' as read.");

			if (!chbRecursive.Checked)
			{
				chbHidden.Checked = false;
			}
		}

		private void BtnMarkAsReadClick(object sender, EventArgs e)
		{
			Logger.Info($"User wants to mark all videos in path '{pathContainer.RegisteredPaths.ElementAt(lvPaths.SelectedIndices[0]).Fullname}' as read.");

			chosePathTlp.Enabled = false;
			pathContainer.MarkAllFilesAsRead(pathContainer.RegisteredPaths.ElementAt(lvPaths.SelectedIndices[0]), queueContainer, archiveContainer, accountContainer);
			MessageBox.Show(this, "Die Videos, die durch diesen Pfad gefunden werden können und nicht schon in der Warteschlange sind, wurden erfolgreich als bereits hochgeladen markiert. Dazu wurden sie ins Archiv aufgenommen. Der Autouploader wird sie nun nicht mehr finden. Um das zu ändern, einfach die Videodatei wieder aus dem Archiv löschen.", "Videos erfolgreich als hochgeladen markiert", MessageBoxButtons.OK, MessageBoxIcon.Information);
			chosePathTlp.Enabled = true;
		}

		private void MoveAfterUploadCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Logger.Info($"User wants to change move videos after upload value in path: '{pathContainer.RegisteredPaths.ElementAt(lvPaths.SelectedIndices[0]).Fullname}'. New value: {moveAfterUploadCheckbox.Checked}");

			moveAfterUploadTextbox.Enabled = moveAfterUploadButton.Enabled = moveAfterUploadCheckbox.Checked;
		}

		private void MoveAfterUploadButton_Click(object sender, EventArgs e)
		{
			var path = txtbxAddPath.Text;
			if (Directory.Exists(path))
			{
				selectMovePathDialog.SelectedPath = path;
			}

			var result = selectMovePathDialog.ShowDialog(this);
			if (result == DialogResult.OK)
			{
				moveAfterUploadTextbox.Text = selectMovePathDialog.SelectedPath;
			}
		}
	}
}
