﻿using STFU.Lib.Youtube.Automation.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Interfaces.Model.Enums;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace STFU.Executable.AutoUploader.Forms
{
	public partial class UploadForm : Form
	{
		IAutomationUploader autoUploader = null;

		string fileText = string.Empty;
		string statusText = "Warte auf Dateien für den Upload...";
		int progress = 0;

		bool aborted = false;
		bool ended = false;
		bool allowChosingProcs = false;

		public int UploadEndedActionIndex { get; set; }

		public UploadForm(IAutomationUploader upl, int uploadEndedIndex)
		{
			InitializeComponent();
			autoUploader = upl;

			autoUploader.PropertyChanged += AutoUploaderPropertyChanged;
			autoUploader.Uploader.PropertyChanged += UploaderPropertyChanged;
			autoUploader.Uploader.NewUploadStarted += OnNewUploadStarted;

			cmbbxFinishAction.SelectedIndex = UploadEndedActionIndex = uploadEndedIndex;
			chbChoseProcesses.Checked = autoUploader.ProcessContainer.ProcessesToWatch.Count > 0;
			btnChoseProcs.Enabled = chbChoseProcesses.Enabled;

			allowChosingProcs = true;

			DialogResult = DialogResult.Cancel;
		}

		private void OnNewUploadStarted(UploadStartedEventArgs args)
		{
			args.Job.PropertyChanged += CurrentUploadPropertyChanged;
		}

		private void CurrentUploadPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var job = (IYoutubeJob)sender;
			var oldtitle = fileText;
			fileText = job.Video.Title;

			if (e.PropertyName == nameof(job.Progress)
				&& (job.State == UploadState.Running || job.State == UploadState.ThumbnailUploading))
			{
				if (job.State == UploadState.ThumbnailUploading)
				{
					statusText = $"Lade Thumbnail hoch: {job.Progress:0.00} %";
				}
				else
				{
					statusText = $"Lade Video hoch: {job.Progress:0.00} %";
				}
				progress = (int)(job.Progress * 100);
			}
			else if (e.PropertyName == nameof(job.State))
			{
				switch (job.State)
				{
					case UploadState.NotStarted:
					case UploadState.Running:
						fileText = job.Video.Title;
						statusText = $"Video-Upload wird gestartet...";
						break;
					case UploadState.ThumbnailUploading:
						statusText = $"Thumbnail-Upload wird gestartet...";
						break;
					case UploadState.CancelPending:
						statusText = $"Upload wird abgebrochen...";
						break;
					case UploadState.Error:
						statusText = $"Es gab einen Fehler beim Upload.";
						break;
					case UploadState.Canceled:
						statusText = $"Upload wurde abgebrochen.";
						break;
					case UploadState.Successful:
						fileText = oldtitle;
						break;
					default:
						throw new ArgumentException("Dieser Status wird nicht unterstützt.");
				}
			}
		}

		private void UploaderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(autoUploader.Uploader.State) && autoUploader.Uploader.State == UploaderState.Waiting)
			{
				statusText = $"Upload abgeschlossen.{Environment.NewLine}Warte auf Dateien für den Upload...";
			}
		}

		private void AutoUploaderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(autoUploader.State)
				&& autoUploader.State == RunningState.NotRunning)
			{
				if (!aborted)
				{
					DialogResult = DialogResult.OK;
				}

				ResetStatusAndForm();
			}
		}

		private void refreshTimerTick(object sender, EventArgs e)
		{
			fileLabel.Text = fileText;
			statusLabel.Text = statusText;
			prgbarProgress.Value = progress;

			if (ended)
			{
				refreshTimer.Enabled = false;
				Close();
			}
		}

		private void ResetStatusAndForm()
		{
			autoUploader.PropertyChanged -= AutoUploaderPropertyChanged;
			autoUploader.Uploader.PropertyChanged -= UploaderPropertyChanged;
			autoUploader.Uploader.NewUploadStarted -= OnNewUploadStarted;
			autoUploader.PropertyChanged -= AutoUploaderPropertyChanged;

			ended = true;
		}

		private void btnStopClick(object sender, EventArgs e)
		{
			aborted = true;
			autoUploader.Cancel();
		}

		private void UploadFormLoad(object sender, EventArgs e)
		{
			Left = Screen.PrimaryScreen.WorkingArea.Width - 30 - Width;
			Top = Screen.PrimaryScreen.WorkingArea.Height - 30 - Height;

			autoUploader.StartAsync();
		}

		private void cmbbxFinishActionSelectedIndexChanged(object sender, EventArgs e)
		{
			UploadEndedActionIndex = cmbbxFinishAction.SelectedIndex;
			chbChoseProcesses.Enabled = cmbbxFinishAction.SelectedIndex != 0;

			autoUploader.ProcessContainer.Stop();

			if (autoUploader != null)
			{
				autoUploader.EndAfterUpload = cmbbxFinishAction.SelectedIndex != 0;
			}

			if (cmbbxFinishAction.SelectedIndex == 0)
			{
				autoUploader?.ProcessContainer.RemoveAllProcesses();
				chbChoseProcesses.Checked = false;
			}
			else
			{
				autoUploader.ProcessContainer.Start();
			}
		}

		private void chbChoseProcessesCheckedChanged(object sender, EventArgs e)
		{
			if (allowChosingProcs)
			{
				btnChoseProcs.Enabled = chbChoseProcesses.Checked;

				if (chbChoseProcesses.Checked)
				{
					ChoseProcesses();
				}
				else
				{
					autoUploader.EndAfterUpload = false;
					autoUploader.ProcessContainer.RemoveAllProcesses();
				}
			}
		}

		private void ChoseProcesses()
		{
			autoUploader.ProcessContainer.Stop();

			ProcessForm processChoser = new ProcessForm(autoUploader.ProcessContainer.ProcessesToWatch);
			processChoser.ShowDialog(this);
			if (processChoser.DialogResult == DialogResult.OK
				&& processChoser.Selected.Count > 0)
			{
				var procs = processChoser.Selected;
				autoUploader.ProcessContainer.RemoveAllProcesses();
				autoUploader.ProcessContainer.AddProcesses(procs);
				autoUploader.EndAfterUpload = true;
			}
			else
			{
				chbChoseProcesses.Checked = false;
			}

			autoUploader.ProcessContainer.Start();
		}

		private void btnChoseProcsClick(object sender, EventArgs e)
		{
			ChoseProcesses();
		}

		private void UploadFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (!ended)
			{
				e.Cancel = true;
				btnStopClick(this, e);
			}
		}
	}
}