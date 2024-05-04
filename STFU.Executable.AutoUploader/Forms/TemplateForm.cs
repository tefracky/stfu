using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using log4net;
using STFU.Lib.Playlistservice;
using STFU.Lib.Youtube.Automation.Interfaces;
using STFU.Lib.Youtube.Automation.Interfaces.Model;
using STFU.Lib.Youtube.Automation.Programming;
using STFU.Lib.Youtube.Automation.Templates;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model.Enums;
using STFU.Lib.Youtube.Model;
using STFU.Lib.Youtube.Persistor;

namespace STFU.Executable.AutoUploader.Forms
{
	public partial class TemplateForm : Form
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(TemplateForm));

		private readonly ITemplateContainer templateContainer;
		private readonly IYoutubeCategoryContainer categoryContainer;
		private readonly IYoutubeLanguageContainer languageContainer;
		private readonly IYoutubePlaylistContainer playlistContainer;
		private readonly IPlaylistServiceConnectionContainer playlistServiceConnectionContainer;
		private readonly TemplatePersistor templatePersistor;
		private ITemplate current;

		private bool reordering = false;
		private bool isDirty = false;
		private bool skipDirtyManipulation = false;

		private bool IsDirty
		{
			get
			{
				return isDirty;
			}
			set
			{
				if (!skipDirtyManipulation)
				{
					isDirty = value;
					saveTemplateButton.Enabled = isDirty;
					resetTemplateButton.Enabled = isDirty;
				}
			}
		}

		public TemplateForm(TemplatePersistor persistor,
			IYoutubeCategoryContainer categoryContainer,
			IYoutubeLanguageContainer languageContainer,
			IYoutubePlaylistContainer playlistContainer,
			IPlaylistServiceConnectionContainer playlistServiceConnectionContainer,
			bool accountHasMailEnabled)
		{
			Logger.Info("Initializing templtes form");

			InitializeComponent();

			addWeekdayCombobox.SelectedIndex = 0;

			templatePersistor = persistor;
			templateContainer = persistor.Container;
			this.categoryContainer = categoryContainer;
			this.languageContainer = languageContainer;
			this.playlistContainer = playlistContainer;
			this.playlistServiceConnectionContainer = playlistServiceConnectionContainer;

			if (templateValuesTabControl.TabPages.Contains(cSharpTabPage))
			{
				templateValuesTabControl.TabPages.Remove(cSharpTabPage);
			}

			if (accountHasMailEnabled)
			{
				Logger.Info("Account has mail functionality enabled");

				connectMailNotificationLabel.Visible = false;

				newVideoMNCheckbox.Enabled = true;
				uploadStartedMNCheckbox.Enabled = true;
				uploadFinishedMNCheckbox.Enabled = true;
				uploadFailedMNCheckbox.Enabled = true;

				mailRecipientTextbox.Enabled = true;
			}
			else
			{
				Logger.Info("Account doesn't have mail functionality enabled");

				connectMailNotificationLabel.Visible = true;

				newVideoMNCheckbox.Enabled = false;
				uploadStartedMNCheckbox.Enabled = false;
				uploadFinishedMNCheckbox.Enabled = false;
				uploadFailedMNCheckbox.Enabled = false;

				mailRecipientTextbox.Enabled = false;
			}

			if (playlistServiceConnectionContainer != null && playlistServiceConnectionContainer.Connection != null)
			{
				Logger.Info("Found an active playlist service connection");

				addPlaylistViaServiceGroupbox.Enabled = true;
				foreach (var account in playlistServiceConnectionContainer.Connection.Accounts)
				{
					chooseAccountCombobox.Items.Add($"{account.Id}: {account.Title}, {account.ChannelId}");
				}
			}

			Logger.Info($"Adding {playlistContainer.RegisteredPlaylists.Count} playlists to playlist combobox");

			foreach (var playlist in playlistContainer.RegisteredPlaylists)
			{
				playlistCombobox.Items.Add(playlist.Title);
				choosePlaylistCombobox.Items.Add(playlist.Title);
			}
		}

		private void AddTemplateButtonClick(object sender, EventArgs e)
		{
			Logger.Info($"User added a new template");

			ITemplate templ = new Template();
			templateContainer.RegisterTemplate(templ);

			RefillListView();
		}

		private void RefillListView()
		{
			Logger.Info($"Refilling template list view");

			templateListView.Items.Clear();

			foreach (var template in templateContainer.RegisteredTemplates)
			{
				var name = template.Name;
				if (string.IsNullOrWhiteSpace(name))
				{
					name = "<Template ohne Namen>";
				}

				Logger.Debug($"Adding template '{name}'");

				templateListView.Items.Add(name);
			}
		}

		private void SplitContainerPaint(object sender, PaintEventArgs e)
		{
			var control = sender as SplitContainer;
			//paint the three dots'
			Point[] points = new Point[3];
			var w = control.Width;
			var h = control.Height;
			var d = control.SplitterDistance;
			var sW = control.SplitterWidth;

			//calculate the position of the points'
			if (control.Orientation == Orientation.Horizontal)
			{
				points[0] = new Point((w / 2), d + (sW / 2));
				points[1] = new Point(points[0].X - 10, points[0].Y);
				points[2] = new Point(points[0].X + 10, points[0].Y);
			}
			else
			{
				points[0] = new Point(d + (sW / 2), (h / 2));
				points[1] = new Point(points[0].X, points[0].Y - 10);
				points[2] = new Point(points[0].X, points[0].Y + 10);
			}

			foreach (Point p in points)
			{
				p.Offset(-2, -2);
				e.Graphics.FillEllipse(SystemBrushes.ControlDark,
					new Rectangle(p, new Size(7, 7)));
			}
		}

		private void TemplateFormLoad(object sender, EventArgs e)
		{
			Logger.Info($"Loading template form");

			RefillListView();

			cSharpSystemFunctionsFctb.Text = StandardFunctions.GlobalFunctions.Aggregate((a, b) => $"{a}{Environment.NewLine}{Environment.NewLine}{b}");
			cSharpScriptingTabControl.SelectedIndex = 2;
		}

		private void DeleteTemplateButtonClick(object sender, EventArgs e)
		{
			if (templateListView.SelectedItems.Count == 0 || templateContainer.RegisteredTemplates.ElementAt(templateListView.SelectedIndices[0]).Id == 0)
			{
				return;
			}

			var confirmation = MessageBox.Show(this, "Möchtest du das ausgewählte Template wirklich löschen? Alle Pfade, die es verwenden, werden auf das Standard-Template umgestellt.", "Wirklich löschen?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (confirmation == DialogResult.Yes)
			{
				Logger.Info($"User asks to delete template '{templateContainer.RegisteredTemplates.ElementAt(templateListView.SelectedIndices[0])}'");

				templateContainer.UnregisterTemplateAt(templateListView.SelectedIndices[0]);

				templateListView.SelectedIndices.Clear();

				RefillListView();
			}
		}

		private void ClearTemplatesButtonClick(object sender, EventArgs e)
		{
			var confirmation = MessageBox.Show(this, "Möchtest du wirklich alle Templates löschen? Das Standard-Template kann nicht entfernt werden. Alle Pfade werden auf das Standard-Template umgestellt.", "Wirklich löschen?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (confirmation == DialogResult.Yes)
			{
				Logger.Info($"User asks to delete all templates");

				templateContainer.UnregisterAllTemplates();

				templateListView.SelectedIndices.Clear();

				RefillListView();
			}
		}

		private void TemplateListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			if (!reordering)
			{
				AskForSaveIfIsDirty();

				skipDirtyManipulation = true;

				editTemplateTableLayoutPanel.Enabled = templateListView.SelectedIndices.Count > 0;
				deleteTemplateButton.Enabled = templateListView.SelectedIndices.Count > 0 && templateContainer.RegisteredTemplates.ElementAt(templateListView.SelectedIndices[0]).Id != 0;

				if (templateListView.SelectedIndices.Count == 0)
				{
					Logger.Info($"User unselected templates");

					current = new Template();
					ClearEditView();
				}
				else
				{
					Logger.Info($"User selected template at index {templateListView.SelectedIndices[0]}");

					var save = templateContainer.RegisteredTemplates.ElementAt(templateListView.SelectedIndices[0]);
					current = Template.Duplicate(save);

					FillTemplateIntoEditView(current);
				}

				skipDirtyManipulation = false;
			}
		}

		private void ClearEditView()
		{
			Logger.Debug($"Clearing edit view");

			templateNameTextbox.Text = string.Empty;
			templateTitleTextbox.Text = string.Empty;
			templateDescriptionTextbox.Text = string.Empty;
			templateTagsTextbox.Text = string.Empty;

			templateValuesTabControl.SelectedIndex = 0;

			if (templateValuesTabControl.TabPages.Contains(cSharpTabPage))
			{
				templateValuesTabControl.TabPages.Remove(cSharpTabPage);
			}

			RefillPlannedVideosListView();
		}

		private void FillTemplateIntoEditView(ITemplate template)
		{
			Logger.Info($"Filling template '{template.Name}' into edit views");

			skipDirtyManipulation = true;

			templateNameTextbox.Text = template.Name;

			templateTitleTextbox.Text = template.Title;
			templateDescriptionTextbox.Text = template.Description;
			templateTagsTextbox.Text = template.Tags;

			privacyComboBox.SelectedIndex = (int)template.Privacy;
			publishAtCheckbox.Checked = template.ShouldPublishAt;

			defaultLanguageCombobox.Items.Clear();
			defaultLanguageCombobox.Items.AddRange(languageContainer.RegisteredLanguages.Select(lang => lang.Name).ToArray());
			defaultLanguageCombobox.SelectedIndex = languageContainer.RegisteredLanguages.ToList().IndexOf(languageContainer.RegisteredLanguages.FirstOrDefault(lang => lang.Id == template.DefaultLanguage?.Id));

			categoryCombobox.Items.Clear();
			categoryCombobox.Items.AddRange(categoryContainer.RegisteredCategories.Select(cat => cat.Title).ToArray());
			categoryCombobox.SelectedIndex = categoryContainer.RegisteredCategories.ToList().IndexOf(categoryContainer.RegisteredCategories.FirstOrDefault(c => c.Id == template.Category?.Id));

			licenseCombobox.SelectedIndex = (int)template.License;

			isEmbeddableCheckbox.Checked = template.IsEmbeddable;
			publicStatsViewableCheckbox.Checked = template.PublicStatsViewable;
			notifySubscribersCheckbox.Checked = template.NotifySubscribers;
			autoLevelsCheckbox.Checked = template.AutoLevels;
			stabilizeCheckbox.Checked = template.Stabilize;

			thumbnailTextbox.Text = template.ThumbnailPath;

			useExpertmodeCheckbox.Checked = template.EnableExpertMode;
			if (template.EnableExpertMode && !templateValuesTabControl.TabPages.Contains(cSharpTabPage))
			{
				templateValuesTabControl.TabPages.Add(cSharpTabPage);
			}
			else if (!template.EnableExpertMode && templateValuesTabControl.TabPages.Contains(cSharpTabPage))
			{
				templateValuesTabControl.TabPages.Remove(cSharpTabPage);
			}

			cSharpPrepareFctb.Text = template.CSharpPreparationScript;
			cSharpCleanupFctb.Text = template.CSharpCleanUpScript;
			assemblyReferencesFctb.Text = template.ReferencedAssembliesText;

			RefillTimesListView();

			if (template.PublishTimes.Count > 0)
			{
				// Ersten markieren.
				timesListView.SelectedIndices.Clear();
				timesListView.SelectedIndices.Add(0);
			}

			nextPublishTimeDtp.Value = template.NextUploadSuggestion;

			mailRecipientTextbox.Text = template.MailTo;

			newVideoDNCheckbox.Checked = template.NewVideoDesktopNotification;
			newVideoMNCheckbox.Checked = template.NewVideoMailNotification;
			uploadStartedDNCheckbox.Checked = template.UploadStartedDesktopNotification;
			uploadStartedMNCheckbox.Checked = template.UploadStartedMailNotification;
			uploadFinishedDNCheckbox.Checked = template.UploadFinishedDesktopNotification;
			uploadFinishedMNCheckbox.Checked = template.UploadFinishedMailNotification;
			uploadFailedDNCheckbox.Checked = template.UploadFailedDesktopNotification;
			uploadFailedMNCheckbox.Checked = template.UploadFailedMailNotification;

			addToPlaylistCheckbox.Checked = playlistCombobox.Enabled = template.AddToPlaylist;
			var pl = playlistContainer.RegisteredPlaylists.FirstOrDefault(p => p.Id == template.PlaylistId);
			if (pl != null)
			{
				playlistCombobox.SelectedIndex = playlistContainer.RegisteredPlaylists.ToList().IndexOf(pl);
			}
			else if (playlistCombobox.Items.Count > 0)
			{
				playlistCombobox.SelectedIndex = 0;
			}

			sendToPlaylistserviceCheckbox.Checked = template.SendToPlaylistService;

			if (playlistServiceConnectionContainer != null && playlistServiceConnectionContainer.Connection != null && (playlistServiceConnectionContainer.Connection.Accounts?.Any(a => a.Id == template.AccountId) ?? false))
			{
				chooseAccountCombobox.SelectedIndex = playlistServiceConnectionContainer.Connection.Accounts.ToList()
					.IndexOf(playlistServiceConnectionContainer.Connection.Accounts.First(a => a.Id == template.AccountId));
			}
			else if (chooseAccountCombobox.Items.Count > 0)
			{
				chooseAccountCombobox.SelectedIndex = 0;
			}

			var playlist = playlistContainer.RegisteredPlaylists.FirstOrDefault(p => p.Id == template.PlaylistIdForService && p.Title == template.PlaylistTitleForService);
			if (playlist != null)
			{
				usePlaylistFromAccountRadiobutton.Checked = true;
				choosePlaylistCombobox.SelectedIndex = playlistContainer.RegisteredPlaylists.ToList().IndexOf(playlist);
			}
			else
			{
				enterPlaylistIdManuallyRadiobutton.Checked = true;
				if (choosePlaylistCombobox.Items.Count > 0)
				{
					choosePlaylistCombobox.SelectedIndex = 0;
				}
			}

			chooseAccountCombobox.Enabled
				= enterPlaylistIdManuallyRadiobutton.Enabled
				= useCustomPlaylistIdTextbox.Enabled
				= useCustomPlaylistTitleTextbox.Enabled
				= usePlaylistFromAccountRadiobutton.Enabled
				= choosePlaylistCombobox.Enabled
				= sendToPlaylistserviceCheckbox.Checked;

			useCustomPlaylistIdTextbox.Enabled
				&= enterPlaylistIdManuallyRadiobutton.Checked;

			useCustomPlaylistTitleTextbox.Enabled
				&= enterPlaylistIdManuallyRadiobutton.Checked;

			choosePlaylistCombobox.Enabled &= usePlaylistFromAccountRadiobutton.Checked;

			useCustomPlaylistIdTextbox.Text = template.PlaylistIdForService;
			useCustomPlaylistTitleTextbox.Text = template.PlaylistTitleForService;

			skipDirtyManipulation = false;
		}

		private void PublishAtCheckboxCheckedChanged(object sender, EventArgs e)
		{
			if (!reordering)
			{
				Logger.Debug($"User clicked publish at checkbox. New value: {publishAtCheckbox.Checked}");

				current.ShouldPublishAt = publishAtCheckbox.Checked;
				publishGroupbox.Enabled = publishAtCheckbox.Checked;
				IsDirty = true;
			}
		}

		private void PrivacyComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			if (!reordering)
			{
				Logger.Debug($"User changed privacy combobox selected index. New value: {privacyComboBox.SelectedIndex}");

				publishAtCheckbox.Enabled = privacyComboBox.SelectedIndex == 2;

				if (privacyComboBox.SelectedIndex != 2)
				{
					publishAtCheckbox.Checked = false;
				}

				switch (privacyComboBox.SelectedIndex)
				{
					case 0:
						current.Privacy = PrivacyStatus.Public;
						break;
					case 1:
						current.Privacy = PrivacyStatus.Unlisted;
						break;
					default:
						current.Privacy = PrivacyStatus.Private;
						break;
				}

				IsDirty = true;
			}
		}

		private void AskForSaveIfIsDirty()
		{
			if (IsDirty)
			{
				Logger.Info($"Current template was edited. Asking user if they want to save it.");

				var result = MessageBox.Show(this, "Das Template wurde bearbeitet. Speichern?", "Das Template wurde bearbeitet, aber nicht abgespeichert. Soll es jetzt gespeichert werden?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result == DialogResult.Yes)
				{
					Logger.Info($"User wants to save. Saving template...");

					templateContainer.UpdateTemplate(current);
					templatePersistor.Save();

					for (int i = 0; i < templateListView.Items.Count; i++)
					{
						var template = templateContainer.RegisteredTemplates.ElementAt(i);
						templateListView.Items[i].Text = !string.IsNullOrWhiteSpace(template.Name) ? template.Name : "<Template ohne Namen>";
					}
				}

				IsDirty = false;
			}
		}

		private void ResetTemplateButtonClick(object sender, EventArgs e)
		{
			if (resetTemplateButton.Enabled)
			{
				Logger.Info($"User clicked reset template button - discarding the changes they made to the template");

				IsDirty = false;
				TemplateListViewSelectedIndexChanged(sender, e);
				RefillPlannedVideosListView();
				RefillFillFieldsListView();
			}
		}

		private void AddTimeButtonClick(object sender, EventArgs e)
		{
			if (addWeekdayCombobox.SelectedIndex == 0)
			{
				var time = addTimeTimePicker.Value.TimeOfDay;

				Logger.Debug($"User clicked add time button - adding a daily time for {time}");

				// täglich
				for (int i = 0; i < 7; i++)
				{
					var weekDay = (DayOfWeek)((i + 1) % 7);

					PublishTime publishTime = new PublishTime
                    {
                        DayOfWeek = weekDay,
                        Time = time
                    };
                    current.PublishTimes.Add(publishTime);
				}
			}
			else
			{
				var weekDay = (DayOfWeek)(addWeekdayCombobox.SelectedIndex % 7);
				var time = addTimeTimePicker.Value.TimeOfDay;

				Logger.Debug($"User clicked add time button - adding a time for {weekDay} at time {time}");

				PublishTime publishTime = new PublishTime
                {
                    DayOfWeek = weekDay,
                    Time = time
                };
                current.PublishTimes.Add(publishTime);
			}

			RefillTimesListView();
			IsDirty = true;
		}

		private void RefillTimesListView()
		{
			Logger.Debug($"Refilling times list view");

			// Selektion merken
			int[] selectedIndices = new int[timesListView.SelectedIndices.Count];
			timesListView.SelectedIndices.CopyTo(selectedIndices, 0);

			timesListView.Items.Clear();
			foreach (var publishTime in current.PublishTimes)
			{
				timesListView.Items.Add(new ListViewItem(new[]
				{
					GetDayString(publishTime.DayOfWeek),
					$"{publishTime.Time:hh\\:mm} Uhr",
					$"{publishTime.SkipDays} Tage"
				}));
			}

			foreach (var index in selectedIndices)
			{
				if (timesListView.Items.Count > index)
				{
					timesListView.SelectedIndices.Add(index);
				}
			}
		}

		private string GetDayString(DayOfWeek day)
		{
			string result = string.Empty;

			switch (day)
			{
				case DayOfWeek.Sunday:
					result = "Sonntag";
					break;
				case DayOfWeek.Monday:
					result = "Montag";
					break;
				case DayOfWeek.Tuesday:
					result = "Dienstag";
					break;
				case DayOfWeek.Wednesday:
					result = "Mittwoch";
					break;
				case DayOfWeek.Thursday:
					result = "Donnerstag";
					break;
				case DayOfWeek.Friday:
					result = "Freitag";
					break;
				case DayOfWeek.Saturday:
					result = "Samstag";
					break;
			}

			return result;
		}

		private void SaveTemplateButtonClick(object sender, EventArgs e)
		{
			if (saveTemplateButton.Enabled && templateListView.SelectedIndices.Count == 1)
			{
				if (ScriptsAreValid())
				{
					var name = !string.IsNullOrWhiteSpace(current.Name) ? current.Name : "<Template ohne Namen>";
					Logger.Info($"Saving template '{name}'");

					reordering = true;
					templateContainer.UpdateTemplate(current);
					templatePersistor.Save();

					IsDirty = false;

					templateListView.Items[templateListView.SelectedIndices[0]].Text
						= !string.IsNullOrWhiteSpace(current.Name) ? current.Name : "<Template ohne Namen>";
					reordering = false;

					TemplateListViewSelectedIndexChanged(sender, e);
				}
				else
				{
					Logger.Info($"Saving template is not possible since there are script errors");
					MessageBox.Show(this, $"Änderungen konnten nicht gespeichert werden.{Environment.NewLine}Bitte überprüfe folgende Felder: {invalidFields.Aggregate((a, b) => $"{a}, {b}")}.{Environment.NewLine}{Environment.NewLine}Darin ist ein Script nicht mit > oder >>> korrekt geschlossen, sodass ein Feld fehlerhaft ist.", "Scripts nicht korrekt abgeschlossen", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private readonly IList<string> invalidFields = new List<string>();

		private bool ScriptsAreValid()
		{
			var titleValid = ExpressionEvaluator.IsValid(current.Title);
			var descriptionValid = ExpressionEvaluator.IsValid(current.Description);
			var tagsValid = ExpressionEvaluator.IsValid(current.Tags);
			var thumbnailPathValid = ExpressionEvaluator.IsValid(current.ThumbnailPath);

			invalidFields.Clear();

			if (!titleValid)
			{
				Logger.Warn($"Found script error in title field");
				invalidFields.Add("Titel");
			}

			if (!descriptionValid)
			{
				Logger.Warn($"Found script error in description field");
				invalidFields.Add("Beschreibung");
			}

			if (!tagsValid)
			{
				Logger.Warn($"Found script error in tags field");
				invalidFields.Add("Tags");
			}

			if (!thumbnailPathValid)
			{
				Logger.Warn($"Found script error in thumbnail field");
				invalidFields.Add("Thumbnailpfad");
			}

			return titleValid && descriptionValid && tagsValid && thumbnailPathValid;
		}

		private void TemplateNameTextboxTextChanged(object sender, EventArgs e)
		{
			if (!reordering)
			{
				Logger.Debug($"User decided to change template name - new name: {templateNameTextbox.Text}");

				current.Name = templateNameTextbox.Text;
				IsDirty = true;
			}
		}

		private void TemplateTitleTextboxTextChanged(object sender, EventArgs e)
		{
			maxTitleLengthLabel.Text = $"Länge Titel: {templateTitleTextbox.Text.Length} / {YoutubeVideo.MaxTitleLength} Zeichen";
			if (!reordering && current != null)
			{
				Logger.Debug($"User decided to change template title");

				current.Title = templateTitleTextbox.Text;
				IsDirty = true;
			}
		}

		private void TemplateDescriptionTextboxTextChanged(object sender, EventArgs e)
		{
			maxDescriptionLengthLabel.Text = $"Länge Beschreibung: {templateDescriptionTextbox.Text.Length} / {YoutubeVideo.MaxDescriptionLength} Zeichen";
			if (!reordering && current != null)
			{
				Logger.Debug($"User decided to change template description");

				current.Description = templateDescriptionTextbox.Text;
				IsDirty = true;
			}
		}

		private void TemplateTagsTextboxTextChanged(object sender, EventArgs e)
		{
			maxTagsLengthLabel.Text = $"Länge Tags: {templateTagsTextbox.Text.Length} / {YoutubeVideo.MaxTagsLength} Zeichen";
			if (!reordering && current != null)
			{
				Logger.Debug($"User decided to change template tags");

				current.Tags = templateTagsTextbox.Text;
				IsDirty = true;
			}
		}

		private void MoveTemplateUpButtonClick(object sender, EventArgs e)
		{
			if (templateListView.SelectedIndices.Count == 1)
			{
				var index = templateListView.SelectedIndices[0];

				Logger.Debug($"User decided to move template at index {index} up");

				reordering = true;
				templateContainer.ShiftTemplatePositionsAt(index, index - 1);

				RefillListView();

				templateListView.SelectedIndices.Add((index - 1 >= 0) ? index - 1 : 0);
				templateListView.Select();
				reordering = false;
			}
		}

		private void MoveTemplateDownButtonClick(object sender, EventArgs e)
		{
			if (templateListView.SelectedIndices.Count == 1)
			{
				var index = templateListView.SelectedIndices[0];

				Logger.Debug($"User decided to move template at index {index} down");

				reordering = true;
				templateContainer.ShiftTemplatePositionsAt(index, index + 1);

				RefillListView();

				templateListView.SelectedIndices.Add((index + 1 < templateContainer.RegisteredTemplates.Count) ? index + 1 : templateContainer.RegisteredTemplates.Count - 1);
				templateListView.Select();
				reordering = false;
			}
		}

		private void DeleteTimeButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"User decided to delete selected publish times");

			if (timesListView.SelectedIndices.Count == 0)
			{
				return;
			}

			var timesToRemove = new List<IPublishTime>();
			foreach (int index in timesListView.SelectedIndices)
			{
				timesToRemove.Add(current.PublishTimes[index]);
			}

			foreach (var time in timesToRemove)
			{
				current.PublishTimes.Remove(time);
			}

			RefillTimesListView();
			IsDirty = true;
		}

		private void ClearTimesButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"User decided to clear all publish times");

			current.PublishTimes.Clear();
			RefillTimesListView();
			IsDirty = true;
		}

        private void AddOneDayButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"User decided add a day to selected publish times");

			foreach (int index in timesListView.SelectedIndices)
			{
				current.PublishTimes[index].SkipDays++;
			}

			RefillTimesListView();
			IsDirty = true;
			timesListView.Select();
		}

        private void SubstractOneDayButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"User decided to substract a day from selected publish times");

			foreach (int index in timesListView.SelectedIndices)
			{
				if (current.PublishTimes[index].SkipDays > 0)
				{
					current.PublishTimes[index].SkipDays--;
				}
			}

			RefillTimesListView();
			IsDirty = true;
			timesListView.Select();
		}

		private void AddOneWeekButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"User decided to add a week to selected publish times");

			foreach (int index in timesListView.SelectedIndices)
			{
				current.PublishTimes[index].SkipDays += 7;
			}

			RefillTimesListView();
			IsDirty = true;
			timesListView.Select();
		}

		private void SubstractOneWeekButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"User decided to substract a week from selected publish times");

			foreach (int index in timesListView.SelectedIndices)
			{
				current.PublishTimes[index].SkipDays -= 7;
				if (current.PublishTimes[index].SkipDays < 0)
				{
					current.PublishTimes[index].SkipDays = 0;
				}
			}

			RefillTimesListView();
			IsDirty = true;
			timesListView.Select();
		}

		private void MoveTimeUpButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"User decided to move selected publish times up");

			timesListView.BeginUpdate();

			bool atLeastOneUpdate = false;
			for (int i = 0; i < current.PublishTimes.Count; i++)
			{
				if (timesListView.SelectedIndices.Contains(i) && i > 0 && !timesListView.SelectedIndices.Contains(i - 1))
				{
					atLeastOneUpdate = true;

					(current.PublishTimes[i], current.PublishTimes[i - 1]) = (current.PublishTimes[i - 1], current.PublishTimes[i]);

                    timesListView.SelectedIndices.Remove(i);
					timesListView.SelectedIndices.Add(i - 1);
				}
			}

			timesListView.EndUpdate();

			if (atLeastOneUpdate)
			{
				RefillTimesListView();
				timesListView.Select();
				IsDirty = true;
			}
		}

		private void MoveTimeDownButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"User decided to move selected publish times down");

			timesListView.BeginUpdate();

			bool atLeastOneUpdate = false;
			for (int i = current.PublishTimes.Count - 1; i >= 0; i--)
			{
				if (timesListView.SelectedIndices.Contains(i) && i < current.PublishTimes.Count - 1 && !timesListView.SelectedIndices.Contains(i + 1))
				{
					atLeastOneUpdate = true;

					(current.PublishTimes[i], current.PublishTimes[i + 1]) = (current.PublishTimes[i + 1], current.PublishTimes[i]);

                    timesListView.SelectedIndices.Remove(i);
					timesListView.SelectedIndices.Add(i + 1);
				}
			}

			timesListView.EndUpdate();

			if (atLeastOneUpdate)
			{
				RefillTimesListView();
				timesListView.Select();
				IsDirty = true;
			}
		}

		private void CategoryComboboxSelectedIndexChanged(object sender, EventArgs e)
		{
			Logger.Debug($"Category was changed to {categoryCombobox.Text}");

			current.Category = categoryContainer.RegisteredCategories.FirstOrDefault(c => c.Title == categoryCombobox.Text);
			IsDirty = true;
		}

		private void DefaultLanguageComboboxSelectedIndexChanged(object sender, EventArgs e)
		{
			Logger.Debug($"Default language was changed to {defaultLanguageCombobox.Text}");

			current.DefaultLanguage = languageContainer.RegisteredLanguages.FirstOrDefault(lang => lang.Name == defaultLanguageCombobox.Text);
			IsDirty = true;
		}

		private void LicenseComboboxSelectedIndexChanged(object sender, EventArgs e)
		{
			Logger.Debug($"License was changed to {licenseCombobox.Text}");

			current.License = (License)licenseCombobox.SelectedIndex;
			IsDirty = true;
		}

		private void IsEmbeddableCheckboxCheckedChanged(object sender, EventArgs e)
		{
			Logger.Debug($"Is embeddable was changed to {isEmbeddableCheckbox.Checked}");

			current.IsEmbeddable = isEmbeddableCheckbox.Checked;
			IsDirty = true;
		}

		private void PublicStatsViewableCheckboxCheckedChanged(object sender, EventArgs e)
		{
			Logger.Debug($"Public stats viewable was changed to {publicStatsViewableCheckbox.Checked}");

			current.PublicStatsViewable = publicStatsViewableCheckbox.Checked;
			IsDirty = true;
		}

		private void NotifySubscribersCheckboxCheckedChanged(object sender, EventArgs e)
		{
			Logger.Debug($"Notify subscribers was changed to {notifySubscribersCheckbox.Checked}");

			current.NotifySubscribers = notifySubscribersCheckbox.Checked;
			IsDirty = true;
		}

		private void AutoLevelsCheckboxCheckedChanged(object sender, EventArgs e)
		{
			Logger.Debug($"Auto levels was changed to {autoLevelsCheckbox.Checked}");

			current.AutoLevels = autoLevelsCheckbox.Checked;
			IsDirty = true;
		}

		private void StabilizeCheckboxCheckedChanged(object sender, EventArgs e)
		{
			Logger.Debug($"Sabilize was changed to {stabilizeCheckbox.Checked}");

			current.Stabilize = stabilizeCheckbox.Checked;
			IsDirty = true;
		}

		private void DuplicateTemplateButtonClick(object sender, EventArgs e)
		{
			if (templateListView.SelectedItems.Count == 0)
			{
				return;
			}

			var index = templateListView.SelectedIndices[0];
			var template = Template.Duplicate(templateContainer.RegisteredTemplates.ElementAt(index));

			template.Name += " (Kopie)";

			Logger.Info($"Duplicated template - name of the new one: '{template.Name}'");

			templateContainer.RegisterTemplate(template);
			RefillListView();

			templateListView.SelectedIndices.Add(index);
			templateListView.Focus();
		}

		private void ThumbnailTextboxTextChanged(object sender, EventArgs e)
		{
			if (current != null)
			{
				Logger.Debug($"Thumbnail path was changed to '{thumbnailTextbox.Text}'");

				current.ThumbnailPath = thumbnailTextbox.Text;
				IsDirty = true;
			}
		}

		private void ChooseThumbnailPathButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"User opened dialog to chose a thumbnail location");

			var result = openThumbnailDialog.ShowDialog(this);
			if (result == DialogResult.OK)
			{
				thumbnailTextbox.Text = openThumbnailDialog.FileName;
			}
		}

		private void CSharpPrepareFctbTextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
		{
			if (current != null)
			{
				Logger.Debug($"csharp preparation script was updated");

				current.CSharpPreparationScript = cSharpPrepareFctb.Text;
				IsDirty = true;
			}
		}

		private void CSharpCleanupFctbTextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
		{
			if (current != null)
			{
				Logger.Debug($"csharp cleanup script was updated");

				current.CSharpCleanUpScript = cSharpCleanupFctb.Text;
				IsDirty = true;
			}
		}

		private void PlanVideosTabpageEntered(object sender, EventArgs e)
		{
			Logger.Debug($"Entering planned videos tab page -> refreshing which fields should be available");

			RefreshFieldNames();
			RefillPlannedVideosListView();
		}

		private void RefreshFieldNames()
		{
			// Refresh der Feldnamen
			var fieldNames = ExpressionEvaluator.GetFieldNames(current);

			foreach (var name in fieldNames)
			{
				Logger.Debug($"Found field name '{name}'");
			}

			foreach (var plannedVid in current.PlannedVideos)
			{
				var fieldsDict = fieldNames.ToDictionary(name => name, name => string.Empty);

				foreach (var name in fieldNames)
				{
					if (plannedVid.Fields.ContainsKey(name))
					{
						fieldsDict[name] = plannedVid.Fields[name];
					}
				}

				plannedVid.Fields = fieldsDict;
			}
		}

		private void RefillPlannedVideosListView()
		{
			Logger.Debug($"Refilling planned videos list view");

			filenamesListView.SelectedIndices.Clear();
			filenamesListView.Items.Clear();

			foreach (var plannedVid in current.PlannedVideos)
			{
				Logger.Debug($"Adding planned video '{plannedVid.Name}'");

				ListViewItem item = new ListViewItem(plannedVid.Name);
				item.SubItems.Add(plannedVid.Fields.All(field => !string.IsNullOrEmpty(field.Value)) ? "Ja" : "Nein");

				filenamesListView.Items.Add(item);
			}
		}

		private void RefillFillFieldsListView()
		{
			Logger.Debug($"Refilling fields list view");

			fillFieldsListView.Items.Clear();
			fieldNameTxbx.Text = string.Empty;
			fieldValueTxbx.Text = string.Empty;
			fieldValueTxbx.Enabled = false;

			if (filenamesListView.SelectedIndices.Count == 1)
			{
				foreach (var field in current.PlannedVideos[filenamesListView.SelectedIndices[0]].Fields)
				{
					ListViewItem item = new ListViewItem(field.Key);
					item.SubItems.Add(field.Value);

					fillFieldsListView.Items.Add(item);
				}
			}
		}

		private void FilenamesListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			Logger.Debug($"User made a new selection in filenames list view");

			fillFieldsGroupbox.Enabled = removeFilenameButton.Enabled = filenamesListView.SelectedIndices.Count == 1;

			RefillFillFieldsListView();
		}

		private void AddFilenameButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"User wants to add a new file name");

			AddPlannedVideoForm addForm = new AddPlannedVideoForm();
			var result = addForm.ShowDialog();

			if (result == DialogResult.OK
				&& current.PlannedVideos.All(v => v.Name.ToLower() != addForm.Filename.ToLower())
				&& !string.IsNullOrWhiteSpace(addForm.Filename))
			{
				Logger.Info($"Adding file name '{addForm.Filename.ToLower()}'");

                IPlannedVideo video = new PlannedVideo
                {
                    Name = addForm.Filename.ToLower()
                };
                current.PlannedVideos.Add(video);

				RefreshFieldNames();

				RefillPlannedVideosListView();

				filenamesListView.SelectedIndices.Clear();
				filenamesListView.SelectedIndices.Add(filenamesListView.Items.Count - 1);
				IsDirty = true;
			}
		}

		private void FillFieldsListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			fieldValueTxbx.Enabled = fillFieldsListView.SelectedIndices.Count == 1;

			if (fillFieldsListView.SelectedIndices.Count == 1)
			{
				fieldNameTxbx.Text = fillFieldsListView.SelectedItems[0].Text;

				Logger.Debug($"Editing field '{fieldNameTxbx.Text}'");

				var multiline = ExpressionEvaluator.IsFieldOnlyInDescription(fieldNameTxbx.Text, current);
				fieldValueTxbx.Multiline = multiline;

				if (multiline)
				{
					Logger.Debug($"Field '{fieldNameTxbx.Text}' is multiline");

					fieldValueTxbx.Dock = DockStyle.Fill;
				}
				else
				{
					Logger.Debug($"Field '{fieldNameTxbx.Text}' is single line");

					fieldValueTxbx.Dock = DockStyle.None;
					fieldValueTxbx.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
				}

				skipDirtyManipulation = true;

				fieldValueTxbx.Text = current
					.PlannedVideos[filenamesListView.SelectedIndices[0]]
					.Fields[fillFieldsListView.SelectedItems[0].Text];

				skipDirtyManipulation = false;
			}
			else
			{
				Logger.Debug($"No field was selected");

				fieldNameTxbx.Text = string.Empty;
				fieldValueTxbx.Text = string.Empty;
			}
		}

		private void RefreshFilenamesAllFilled()
		{
			filenamesListView.SelectedItems[0].SubItems[1].Text
				= current.PlannedVideos[filenamesListView.SelectedIndices[0]]
				.Fields
				.All(field => !string.IsNullOrEmpty(field.Value)) ? "Ja" : "Nein";
		}

		private void TemplateFormFormClosing(object sender, FormClosingEventArgs e)
		{
			Logger.Debug($"Attempting to close template form");

			AskForSaveIfIsDirty();
		}

		private void FieldValueTxbxTextChanged(object sender, EventArgs e)
		{
			if (fillFieldsListView.SelectedIndices.Count == 1)
			{
				Logger.Debug($"Value of Field '{fillFieldsListView.SelectedItems[0].Text}' is updated to '{fieldValueTxbx.Text}'");

				current
					.PlannedVideos[filenamesListView.SelectedIndices[0]]
					.Fields[fillFieldsListView.SelectedItems[0].Text]
					= fieldValueTxbx.Text;
				
				fillFieldsListView.SelectedItems[0].SubItems[1].Text = fieldValueTxbx.Text;

				RefreshFilenamesAllFilled();
				IsDirty = true;
			}
		}

		private void RemoveFilenameButtonClick(object sender, EventArgs e)
		{
			if (filenamesListView.SelectedIndices.Count == 1)
			{
				Logger.Debug($"Removing planned video '{current.PlannedVideos.ElementAt(filenamesListView.SelectedIndices[0])}'");

				current.PlannedVideos.RemoveAt(filenamesListView.SelectedIndices[0]);
				IsDirty = true;
				RefillPlannedVideosListView();
				RefillFillFieldsListView();
			}
		}

		private void ClearFilenamesButtonClick(object sender, EventArgs e)
		{
			Logger.Debug($"Attempting to clear all planned videos");

			if (DialogResult.Yes == MessageBox.Show(this, "Willst du wirklich alle geplanten Videos löschen? Dieser Schritt kann nach dem Speichern nicht mehr rückgängig gemacht werden!", "Bitte bestätigen", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
			{
				Logger.Debug($"Clearing all planned videos");

				current.PlannedVideos.Clear();
				IsDirty = true;
				RefillPlannedVideosListView();
				RefillFillFieldsListView();
			}
		}

		private void UseExpertmodeCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (current != null)
			{
				Logger.Debug($"Setting enable expert mode to {useExpertmodeCheckbox.Checked}");

				current.EnableExpertMode = useExpertmodeCheckbox.Checked;
				if (current.EnableExpertMode && !templateValuesTabControl.TabPages.Contains(cSharpTabPage))
				{
					templateValuesTabControl.TabPages.Add(cSharpTabPage);
				}
				else if (!current.EnableExpertMode && templateValuesTabControl.TabPages.Contains(cSharpTabPage))
				{
					templateValuesTabControl.TabPages.Remove(cSharpTabPage);
				}

				IsDirty = true;
			}
		}

        private void AssemblyReferencesFctb_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
		{
			if (current != null)
			{
				Logger.Debug($"Assembly references textbox text changed");

				current.ReferencedAssembliesText = assemblyReferencesFctb.Text;
				IsDirty = true;
			}
		}

		private void MailRecipientTextbox_TextChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Setting mail recipient: '{mailRecipientTextbox.Text}'");

				current.MailTo = mailRecipientTextbox.Text;
				IsDirty = true;
			}
		}

        private void NewVideoDNCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Setting new video desktop notification to: {newVideoDNCheckbox.Checked}");

				current.NewVideoDesktopNotification = newVideoDNCheckbox.Checked;
				IsDirty = true;
			}
		}

		private void NewVideoMNCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Setting new video mail notification to: {newVideoMNCheckbox.Checked}");

				current.NewVideoMailNotification = newVideoMNCheckbox.Checked;
				IsDirty = true;
			}
		}

        private void UploadStartedDNCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Setting upload started desktop notification to: {uploadStartedDNCheckbox.Checked}");

				current.UploadStartedDesktopNotification = uploadStartedDNCheckbox.Checked;
				IsDirty = true;
			}
		}

		private void UploadStartedMNCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Setting upload started mail notification to: {uploadStartedMNCheckbox.Checked}");

				current.UploadStartedMailNotification = uploadStartedMNCheckbox.Checked;
				IsDirty = true;
			}
		}

		private void UploadFinishedDNCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Setting upload finished desktop notification to: {uploadFinishedDNCheckbox.Checked}");

				current.UploadFinishedDesktopNotification = uploadFinishedDNCheckbox.Checked;
				IsDirty = true;
			}
		}

		private void UploadFinishedMNCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Setting upload finished mail notification to: {uploadFinishedMNCheckbox.Checked}");

				current.UploadFinishedMailNotification = uploadFinishedMNCheckbox.Checked;
				IsDirty = true;
			}
		}

		private void UploadFailedDNCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Setting upload failed desktop notification to: {uploadFailedDNCheckbox.Checked}");

				current.UploadFailedDesktopNotification = uploadFailedDNCheckbox.Checked;
				IsDirty = true;
			}
		}

        private void UploadFailedMNCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Setting upload failed mail notification to: {uploadFailedMNCheckbox.Checked}");

				current.UploadFailedMailNotification = uploadFailedMNCheckbox.Checked;
				IsDirty = true;
			}
		}

		private void NextPublishTimeDtp_ValueChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Next published time dtp value changed to: {nextPublishTimeDtp.Value}");

				current.NextUploadSuggestion = nextPublishTimeDtp.Value;
				IsDirty = true;
			}
		}

		private void AddToPlaylistCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Add to playlist checkbox checked changed to {addToPlaylistCheckbox.Checked}");

				current.AddToPlaylist = playlistCombobox.Enabled = addToPlaylistCheckbox.Checked;
				IsDirty = true;
			}
		}

        private void PlaylistCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Playlist combobox selected index changed to {playlistCombobox.SelectedIndex}");

				current.PlaylistId = playlistContainer.RegisteredPlaylists.ElementAt(playlistCombobox.SelectedIndex).Id;
				IsDirty = true;
			}
		}

        private void SendToPlaylistserviceCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Send to playlist service checked changed to {sendToPlaylistserviceCheckbox.Checked}");

				current.SendToPlaylistService
					= chooseAccountCombobox.Enabled
					= enterPlaylistIdManuallyRadiobutton.Enabled
					= useCustomPlaylistIdTextbox.Enabled
					= useCustomPlaylistTitleTextbox.Enabled
					= usePlaylistFromAccountRadiobutton.Enabled
					= choosePlaylistCombobox.Enabled
					= sendToPlaylistserviceCheckbox.Checked;

				useCustomPlaylistIdTextbox.Enabled
					&= enterPlaylistIdManuallyRadiobutton.Checked;

				useCustomPlaylistTitleTextbox.Enabled
					&= enterPlaylistIdManuallyRadiobutton.Checked;

				choosePlaylistCombobox.Enabled &= usePlaylistFromAccountRadiobutton.Checked;

				IsDirty = true;
			}
		}

        private void ChooseAccountCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Choose account combobox index changed to {chooseAccountCombobox.SelectedIndex}");

				current.AccountId = playlistServiceConnectionContainer.Connection.Accounts[chooseAccountCombobox.SelectedIndex].Id;
				IsDirty = true;
			}
		}

        private void EnterPlaylistIdManuallyRadiobutton_CheckedChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Enter playlist id manually radio button checked changed to: {enterPlaylistIdManuallyRadiobutton.Checked}");

				useCustomPlaylistIdTextbox.Enabled = useCustomPlaylistTitleTextbox.Enabled = enterPlaylistIdManuallyRadiobutton.Checked;

				if (enterPlaylistIdManuallyRadiobutton.Checked)
				{
					current.PlaylistIdForService = useCustomPlaylistIdTextbox.Text;
					current.PlaylistTitleForService = useCustomPlaylistTitleTextbox.Text;
				}

				IsDirty = true;
			}
		}

        private void UseCustomPlaylistIdTextbox_TextChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Use custom playlist id textbox text changed to '{useCustomPlaylistIdTextbox.Text}'");

				current.PlaylistIdForService = useCustomPlaylistIdTextbox.Text;
				IsDirty = true;
			}
		}

        private void UseCustomPlaylistTitleTextbox_TextChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Use custom playlist title textbox text changed to '{useCustomPlaylistTitleTextbox.Text}'");

				current.PlaylistTitleForService = useCustomPlaylistTitleTextbox.Text;
				IsDirty = true;
			}
		}

        private void UsePlaylistFromAccountRadiobutton_CheckedChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Use playlist from account radio button checked changed to '{usePlaylistFromAccountRadiobutton.Checked}'");

				choosePlaylistCombobox.Enabled = usePlaylistFromAccountRadiobutton.Checked;

				if (usePlaylistFromAccountRadiobutton.Checked)
				{
					current.PlaylistIdForService = playlistContainer.RegisteredPlaylists.ElementAt(choosePlaylistCombobox.SelectedIndex).Id;
					current.PlaylistTitleForService = playlistContainer.RegisteredPlaylists.ElementAt(choosePlaylistCombobox.SelectedIndex).Title;
				}

				IsDirty = true;
			}
		}

        private void ChoosePlaylistCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (current != null && !skipDirtyManipulation)
			{
				Logger.Debug($"Choose playlist combobox selected index changed to {choosePlaylistCombobox.SelectedIndex}");

				current.PlaylistIdForService = playlistContainer.RegisteredPlaylists.ElementAt(choosePlaylistCombobox.SelectedIndex).Id;
				current.PlaylistTitleForService = playlistContainer.RegisteredPlaylists.ElementAt(choosePlaylistCombobox.SelectedIndex).Title;

				IsDirty = true;
			}
		}
	}
}
