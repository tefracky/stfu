using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using log4net;
using STFU.Lib.MailSender.Generator;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Interfaces.Model.Args;
using STFU.Lib.Youtube.Interfaces.Model.Enums;
using STFU.Lib.Youtube.Interfaces.Model.Handler;
using STFU.Lib.Youtube.Upload;
using STFU.Lib.Youtube.Upload.Steps;

namespace STFU.Lib.Youtube
{
	public class YoutubeUploader : IYoutubeUploader
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(YoutubeUploader));

		private int maxSimultaneousUploads = 1;
		private readonly IList<IYoutubeJob> jobQueue = new List<IYoutubeJob>();
		private UploaderState state = UploaderState.NotRunning;
		private bool stopAfterCompleting = true;

		public bool RemoveCompletedJobs { get; set; }

		/// <see cref="IYoutubeUploader.MaxSimultaneousUploads"/>
		public int MaxSimultaneousUploads
		{
			get
			{
				Logger.Debug($"Returning maxSimultaneousUploads with value {maxSimultaneousUploads}");
				return maxSimultaneousUploads;
			}

			set
			{
				if (maxSimultaneousUploads != value && value > 0)
				{
					Logger.Debug($"Setting maxSimultaneousUploads to new value {value}");
					maxSimultaneousUploads = value;
					OnPropertyChanged();
				}
			}
		}

		private IYoutubeJobContainer JobQueue { get; set; } = new YoutubeJobContainer();

		/// <see cref="IYoutubeUploader.Queue"/>
		public IReadOnlyCollection<IYoutubeJob> Queue => new ReadOnlyCollection<IYoutubeJob>(JobQueue.RegisteredJobs.ToList());

		/// <see cref="IYoutubeUploader.State"/>
		public UploaderState State
		{
			get
			{
				Logger.Debug($"Returning state with value {state}");
				return state;
			}

			internal set
			{
				if (state != value)
				{
					Logger.Debug($"Setting state to new value {value}");
					state = value;
					OnPropertyChanged();
				}
			}
		}

		/// <see cref="IYoutubeUploader.StopAfterCompleting"/>
		public bool StopAfterCompleting
		{
			get
			{
				Logger.Debug($"Returning stopAfterCompleting with value {stopAfterCompleting}");
				return stopAfterCompleting;
			}

			set
			{
				if (stopAfterCompleting != value)
				{
					Logger.Debug($"Setting stopAfterCompleting to new value {value}");
					stopAfterCompleting = value;
					OnPropertyChanged();
				}
			}
		}

		private int progress = 0;
		/// <see cref="IYoutubeUploader.Progress"/>
		public int Progress
		{
			get
			{
				Logger.Debug($"Returning progress with value {progress}");
				return progress;
			}

			set
			{
				if (progress != value && value > 0)
				{
					Logger.Debug($"Setting progress to new value {value}");
					progress = value;
					OnPropertyChanged();
				}
			}
		}

		public bool LimitUploadSpeed { get => ThrottledReadStream.ShouldThrottle; set => ThrottledReadStream.ShouldThrottle = value; }

		public long UploadLimitKByte { get => ThrottledReadStream.GlobalLimit; set => ThrottledReadStream.GlobalLimit = value; }

		public YoutubeUploader()
		{
			Logger.Debug($"Creating a new instance of youtube uploader");
			ServicePointManager.DefaultConnectionLimit = 100;
		}

		public YoutubeUploader(IYoutubeJobContainer queue)
			: this()
		{
			Logger.Debug($"Creating a new instance of youtube uploader with {queue.RegisteredJobs.Count} jobs");

			JobQueue = queue;

			if (JobQueue == null)
			{
				Logger.Warn("Given queue was NULL!!!");
				JobQueue = new YoutubeJobContainer();
			}

			for (int i = 0; i < JobQueue.RegisteredJobs.Count; i++)
			{
				YoutubeJob job = JobQueue.RegisteredJobs.ElementAt(i) as YoutubeJob;

				Logger.Info($"Registering job for video {job.Video.Title}");

				job.TriggerDeletion += Job_TriggerDeletion;
				job.PropertyChanged += RunningJobPropertyChanged;
				job.UploadStatus.PropertyChanged += UploadStatusPropertyChanged;

				OnJobQueued(job, i);
			}
		}

		public event UploadStarted NewUploadStarted;

		/// <see cref="IYoutubeUploader.QueueUpload(IYoutubeVideo, IYoutubeAccount)"/>
		public IYoutubeJob QueueUpload(IYoutubeVideo video, IYoutubeAccount account, INotificationSettings notificationSettings)
		{
			Logger.Info($"Queueing Video '{video.Title}' for account '{account.Title}' was called");

			if (Queue.Any(existing => existing.Video == video && existing.Account == account))
			{
				var foundJob = Queue.Single(existing => existing.Video == video && existing.Account == account);
				Logger.Info($"Requested video was already in queue at position {Queue.ToList().IndexOf(foundJob)}, it won't be added again.");
				return foundJob;
			}

			var job = new YoutubeJob(video, account, new UploadStatus())
            {
                NotificationSettings = notificationSettings
            };

            RegisterJob(job);

			return job;
		}

		/// <see cref="IYoutubeUploader.QueueUpload(IYoutubeJob)"/>
		public IYoutubeJob QueueUpload(IYoutubeJob job)
		{
			Logger.Info($"Queueing job with video '{job.Video.Title}' for account '{job.Account.Title}' was called");

			if (Queue.Any(existing => existing == job))
			{
				Logger.Info($"Requested job was already in queue at position {Queue.ToList().IndexOf(job)}, it won't be added again.");
				return Queue.Single(existing => existing == job);
			}

			RegisterJob(job);

			return job;
		}

		private void RegisterJob(IYoutubeJob job)
		{
			Logger.Debug("Registering job");

			job.TriggerDeletion += Job_TriggerDeletion;
			job.PropertyChanged += RunningJobPropertyChanged;
			job.UploadStatus.PropertyChanged += UploadStatusPropertyChanged;

			if (job.NotificationSettings.NotifyOnVideoFoundMail)
			{
				MailSender.MailSender.Send(
					job.Account,
					job.NotificationSettings.MailReceiver,
					$"Wartet: '{job.Video.Title}' ist in der Warteschlange!",
					new NewVideoFoundMailGenerator().Generate(job));
			}

			JobQueue.RegisterJob(job);

			OnJobQueued(job, JobQueue.RegisteredJobs.ToList().IndexOf(job));

			if (State == UploaderState.Waiting || State == UploaderState.Uploading)
			{
				Logger.Info("Starting uploader now");
				StartJobs();
			}
		}

		private void Job_TriggerDeletion(object sender, System.EventArgs args)
		{
			Logger.Info($"Removing job for video '{((YoutubeJob)sender).Video.Title}' from queue");
			RemoveFromQueue((YoutubeJob)sender);
		}

		/// <see cref="IYoutubeUploader.CancelAll"/>
		public void CancelAll()
		{
			Logger.Info("Cancelling all jobs");

			var runningJobs = JobQueue.RegisteredJobs.Where(j => j.State.IsStarted()).ToArray();
			if (runningJobs.Length > 0
				&& (State == UploaderState.Uploading || State == UploaderState.Waiting))
			{
				Logger.Info($"Found {runningJobs.Length} running jobs to cancel");
				State = UploaderState.CancelPending;

				foreach (var runningJob in runningJobs)
				{
					Logger.Info($"Cancelling job for video {runningJob.Video.Title}");
					runningJob.Cancel();
				}
			}
			else
			{
				State = UploaderState.NotRunning;
			}

			Logger.Info("Cancelling completed");
		}

		/// <see cref="IYoutubeUploader.ChangePosition(IYoutubeJob job, int newPosition)"/>
		public void ChangePosition(IYoutubeJob job, int newPosition)
		{
			Logger.Debug($"Switching position of job with video {job.Video.Title} to new positon {newPosition}");

			if (Queue.Contains(job))
			{
				int oldPosition = JobQueue.RegisteredJobs.ToList().IndexOf(job);
				Logger.Info($"Switching position of job with video {job.Video.Title} from old position {oldPosition} to new positon {newPosition}");

				JobQueue.UnregisterJobAt(oldPosition);

				if (JobQueue.RegisteredJobs.Count < newPosition)
				{
					Logger.Debug($"Setting newPosition to {JobQueue.RegisteredJobs.Count} since there are not enough videos in queue");
					newPosition = JobQueue.RegisteredJobs.Count;
				}
				else if (newPosition < 0)
				{
					Logger.Debug($"Setting newPosition to 0 since it was < 0");
					newPosition = 0;
				}

				JobQueue.RegisterJob(newPosition, job);
				OnJobPositionChanged(job, oldPosition, newPosition);
			}
		}

		/// <see cref="IYoutubeUploader.RemoveFromQueue(IYoutubeJob)"/>
		public void RemoveFromQueue(IYoutubeJob job)
		{
			Logger.Debug($"Removing job with video {job.Video.Title} from queue");

			if (Queue.Contains(job))
			{
				int position = JobQueue.RegisteredJobs.ToList().IndexOf(job);

				Logger.Info($"Removing job with video {job.Video.Title} from position {position} in queue");

				job.TriggerDeletion -= Job_TriggerDeletion;
				job.PropertyChanged -= RunningJobPropertyChanged;
				job.UploadStatus.PropertyChanged -= UploadStatusPropertyChanged;

				JobQueue.UnregisterJob(job);

				OnJobDequeued(job, position);
			}
		}

		private void UploadStatusPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Logger.Info($"Handling upload status property changed event for property {e.PropertyName}");

			if (e.PropertyName == nameof(UploadStatus.Progress))
			{
				RecalculateProgress();
			}
		}

		/// <see cref="IYoutubeUploader.StartUploader"/>
		public void StartUploader()
		{
			Logger.Debug($"Starting uploader");

			if (State == UploaderState.NotRunning)
			{
				Logger.Info($"Uploader was not running. Starting it now.");
				State = UploaderState.Waiting;
				StartJobs();
			}
		}

		private void StartJobs()
		{
			Logger.Info($"Starting jobs");
			HashSet<IYoutubeJob> startedJobs = new HashSet<IYoutubeJob>();

			if (JobQueue.RegisteredJobs == null)
			{
				Logger.Error("JobQueue.RegisteredJobs was NULL!!");
			}

			while (State != UploaderState.CancelPending
				&& JobQueue.RegisteredJobs.Where(j => j != null && j.State.IsStarted() && !startedJobs.Contains(j)).Count() + startedJobs.Count < MaxSimultaneousUploads
				&& Queue.Any(job => job.State == JobState.NotStarted && job.Video.File.Exists && !job.ShouldBeSkipped))
			{
				var nextJob = Queue.FirstOrDefault(job => job != null && job.State == JobState.NotStarted && job.Video.File.Exists && !job.ShouldBeSkipped);

				if (nextJob != null)
				{
					Logger.Info($"Preparing waiting job for video '{nextJob.Video.Title}'");

					bool start = false;
					State = UploaderState.Uploading;
					while (!start && nextJob.Video.File.Exists)
					{
						try
						{
							Logger.Debug($"Trying to open the video for write access to see if it's ready");
							using (StreamWriter writer = new StreamWriter(nextJob.Video.File.FullName, true))
							{
								Logger.Debug($"Video can be accessed");
								start = true;
							}
						}
						catch (System.Exception)
						{
							Logger.Debug($"Video file was in write access by another program. Waiting until it's being released");
						}
					}

					if (!startedJobs.Contains(nextJob) && nextJob.Video.File.Exists)
					{
						Logger.Info($"Starting waiting job for video '{nextJob.Video.Title}'");

						NewUploadStarted?.Invoke(new UploadStartedEventArgs(nextJob));

						nextJob.Run();

						if (nextJob.NotificationSettings.NotifyOnVideoUploadStartedMail)
						{
							MailSender.MailSender.Send(
								nextJob.Account,
								nextJob.NotificationSettings.MailReceiver,
										$"Start: '{nextJob.Video.Title}' wird jetzt hochgeladen!",
										new UploadStartedMailGenerator().Generate(nextJob));
						}

						startedJobs.Add(nextJob);
					}
				}
			}
		}

		private void RefreshUploaderState()
		{
			Logger.Debug($"Refreshing uploader state");

			if (State != UploaderState.CancelPending)
			{
				if (JobQueue.RegisteredJobs.ToList().Where(j => j.State.IsStarted()).Count() == 0)
				{
					if (StopAfterCompleting || State == UploaderState.NotRunning)
					{
						Logger.Info($"Setting uploader state to not running");
						State = UploaderState.NotRunning;
					}
					else
					{
						Logger.Info($"Setting uploader state to waiting");
						State = UploaderState.Waiting;
					}
				}
				else
				{
					Logger.Info($"Setting uploader state to uploading");
					State = UploaderState.Uploading;
				}
			}
			else
			{
				Logger.Info($"Setting uploader state to not running");
				State = UploaderState.NotRunning;
			}
		}

		private void RunningJobPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var job = sender as IYoutubeJob;

			Logger.Debug($"Received property change of property '{e.PropertyName}' from job for video '{job.Video.Title}'");

			if (e.PropertyName == nameof(IYoutubeJob.State))
			{
				Logger.Info($"Reachting to property change of property '{e.PropertyName}' from job for video '{job.Video.Title}'");

				if (job.State.IsFailed() && job.Error?.FailReason == FailureReason.UserUploadLimitExceeded)
				{
					Logger.Info($"Setting uploader state to cancel pending");
					State = UploaderState.CancelPending;
				}

				if (job.State == JobState.Successful && job.NotificationSettings.NotifyOnVideoUploadFinishedMail)
				{
					MailSender.MailSender.Send(
						job.Account,
						job.NotificationSettings.MailReceiver,
						$"Erfolg: '{job.Video.Title}' wurde erfolgreich hochgeladen!",
						new UploadFinishedMailGenerator().Generate(job));
				}
				else if (job.State.IsFailed() && job.NotificationSettings.NotifyOnVideoUploadFailedMail)
				{
					MailSender.MailSender.Send(
						job.Account,
						job.NotificationSettings.MailReceiver,
						$"Fehler: '{job.Video.Title}' konnte nicht hochgeladen werden",
						new UploadFailedMailGenerator().Generate(job));
				}

				RefreshUploaderState();

				if (State != UploaderState.CancelPending && State != UploaderState.NotRunning
					&& (job.State == JobState.Successful
					|| job.State.IsFailed()
					|| job.State.IsCanceled()))
				{
					if (!job.State.IsCanceled() && !job.State.IsFailed() && RemoveCompletedJobs)
					{
						Logger.Info($"Job didn't fail - removing it from queue");
						RemoveFromQueue(job);
					}

					if (State == UploaderState.Uploading
						|| State == UploaderState.Waiting)
					{
						Logger.Debug($"Calling start method to maybe start next job");
						StartJobs();
					}
				}
			}
		}

		private void RecalculateProgress()
		{
			var runningJobs = JobQueue.RegisteredJobs.ToList().Where(j => j.State.IsStarted()).ToArray();

			if (runningJobs.Length > 0)
			{
				Progress = runningJobs.Sum(j => (int)(j.UploadStatus.Progress * 100)) / runningJobs.Length;
			}
			else
			{
				Progress = 0;
			}

			Logger.Info($"Recalculated progress to: {Progress / 100.0} %");
		}

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName]string caller = "")
		{
			Logger.Debug($"Property {caller} changed, invoking handler");
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
		}

		public event JobQueuedEventHandler JobQueued;
		private void OnJobQueued(IYoutubeJob job, int position)
		{
			Logger.Debug($"Job for video '{job.Video.Title}' was added on position {position}, invoking handler");
			JobQueued?.Invoke(this, new JobQueuedEventArgs(job, position));
		}

		public event JobDequeuedEventHandler JobDequeued;
		private void OnJobDequeued(IYoutubeJob job, int position)
		{
			Logger.Debug($"Job for video '{job.Video.Title}' on position {position} was dequeued, invoking handler");
			JobDequeued?.Invoke(this, new JobDequeuedEventArgs(job, position));
		}

		public event JobPositionChangedEventHandler JobPositionChanged;
		private void OnJobPositionChanged(IYoutubeJob job, int oldPosition, int newPosition)
		{
			Logger.Debug($"Position of job for video '{job.Video.Title}' changed to {newPosition}, invoking handler");
			JobPositionChanged?.Invoke(this, new JobPositionChangedEventArgs(job, oldPosition, newPosition));
		}

		#endregion Events
	}
}
