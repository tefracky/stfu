using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using log4net;
using Newtonsoft.Json;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Interfaces.Model.Args;
using STFU.Lib.Youtube.Interfaces.Model.Handler;
using STFU.Lib.Youtube.Model;
using STFU.Lib.Youtube.Upload.Steps;

namespace STFU.Lib.Youtube.Upload
{
	public class YoutubeJob : IYoutubeJob
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(YoutubeJob));
		public static bool SimplifyLogging { get; set; } = false;

		public IYoutubeVideo Video { get; set; }

		public IYoutubeAccount Account { get; set; }

		private JobState state = JobState.NotStarted;
		public JobState State
		{
			get
			{
				return state;
			}
			set
			{
				if (state != value)
				{
					state = value;

					if (!SimplifyLogging)
					{
						Logger.Info($"State of Job '{Video.Title}' changed to '{state}'");
					}

					OnPropertyChanged();
				}
			}
		}

		private bool shouldBeSkipped = false;
		public bool ShouldBeSkipped
		{
			get
			{
				return shouldBeSkipped;
			}
			set
			{
				if (shouldBeSkipped != value)
				{
					shouldBeSkipped = value;
					Logger.Info($"Should be skipped value of Job '{Video.Title}' changed to '{shouldBeSkipped}'");
					OnPropertyChanged();
				}
			}
		}

		private IYoutubeError error;

		public IYoutubeError Error
		{
			get
			{
				return error;
			}
			set
			{
				if (error != value)
				{
					error = value;
					Logger.Info($"Error of Job '{Video.Title}' changed to '{error}'");
					OnPropertyChanged();
				}
			}
		}

		// Das gehört eigentlich zum Job
		[JsonConverter(typeof(ConcreteTypeConverter<NotificationSettings>))]
		public INotificationSettings NotificationSettings { get; set; } = new NotificationSettings() { };

		private UploadStatus uploadStatus = new UploadStatus();
		public UploadStatus UploadStatus
		{
			get
			{
				if (uploadStatus == null)
				{
					uploadStatus = new UploadStatus();
				}
				return uploadStatus;
			}
			set
			{
				if (value != null && uploadStatus != value)
				{
					uploadStatus = value;

					if (!SimplifyLogging)
					{
						Logger.Info($"Uploadstatus of Job '{Video.Title}' changed to '{uploadStatus}'");
					}
				}
			}
		}

		private JobUploader JobUploader { get; }

		public YoutubeJob(IYoutubeVideo video, IYoutubeAccount account, UploadStatus uploadStatus)
		{
			Logger.Info($"Creating new job for video '{video?.Title ?? "null"}' and account '{account?.Title ?? "null"}'");

			Video = video;
			Account = account;
			UploadStatus = uploadStatus;

			JobUploader = new JobUploader(this);
			JobUploader.StateChanged += JobUploaderStateChanged;
		}

		[JsonConstructor]
		public YoutubeJob(YoutubeVideo video, YoutubeAccount account, YoutubeError error, UploadStatus uploadStatus)
			: this(video, account, uploadStatus)
		{
			if (!SimplifyLogging)
			{
				Logger.Info($"Creating new job with error '{error}'");
			}

			Error = error;
		}

		private void JobUploaderStateChanged(object sender, UploaderStateChangedEventArgs e)
		{
			State = e.NewState;

			if (State == JobState.Successful)
			{
				UploadCompletedAction?.Invoke(new JobFinishedEventArgs(this));
			}
		}

		public void Run()
		{
			Logger.Info($"Starting upload, video: '{Video.Title}'");

			JobUploader.Run();
		}

		public void Pause()
		{
			Logger.Info($"Pausing upload, video: '{Video.Title}'");

			if (UploadStatus?.CurrentStep != null)
			{
				UploadStatus.CurrentStep.Cancel();
			}

			State = JobState.Paused;
		}

		public void Resume()
		{
			Logger.Info($"Resuming upload, video: '{Video.Title}'");

			if (UploadStatus.CurrentStep != null)
			{
				UploadStatus.CurrentStep.RunAsync();
			}
			else
			{
				JobUploader.Run();
			}

			State = JobState.Running;
		}

		public void Cancel()
		{
			Logger.Info($"Canceling upload, video: '{Video.Title}'");

			JobUploader.Reset();
			UploadStatus = new UploadStatus();
			State = JobState.Canceled;
		}

		public void Reset(bool resetStatus = false)
		{
			Logger.Info($"Reseting upload, video: '{Video.Title}'");

			JobUploader.Reset();

			if (resetStatus)
			{
				UploadStatus.Reset();
			}
		}

		public void Delete()
		{
			Logger.Info($"Deleting upload, video: '{Video.Title}'");

			JobUploader.Reset();

			TriggerDeletion?.Invoke(this, new EventArgs());
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public event JobFinishedEventHandler UploadCompletedAction;
		public event TriggerDeletionEventHandler TriggerDeletion;

		private void OnPropertyChanged([CallerMemberName]string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		public void RefreshDurationAndSpeed()
		{
			UploadStatus.CurrentStep?.RefreshDurationAndSpeed();
		}
	}
}
