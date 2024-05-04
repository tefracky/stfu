using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using log4net;
using STFU.Lib.Playlistservice;
using STFU.Lib.Youtube.Automation.Interfaces;
using STFU.Lib.Youtube.Automation.Interfaces.Model;
using STFU.Lib.Youtube.Automation.Interfaces.Model.Args;
using STFU.Lib.Youtube.Automation.Interfaces.Model.Events;
using STFU.Lib.Youtube.Automation.Internal;
using STFU.Lib.Youtube.Automation.Internal.Templates;
using STFU.Lib.Youtube.Automation.Internal.Watcher;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Interfaces.Model.Enums;

namespace STFU.Lib.Youtube.Automation
{
	public class AutomationUploader : IAutomationUploader
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(AutomationUploader));

		private IYoutubeUploader uploader = null;
		public IYoutubeUploader Uploader
		{
			get
			{
				Logger.Debug($"Returning uploader with value {uploader}");
				return uploader;
			}
			set
			{
				if (State == RunningState.NotRunning && uploader != value)
				{
					Logger.Debug($"Setting uploader to new value {value}");
					uploader = value;
					OnPropertyChaged();
				}
			}
		}

		private IYoutubeAccount account = null;
		public IYoutubeAccount Account
		{
			get
			{
				Logger.Debug($"Returning account with value {account}");
				return account;
			}
			set
			{
				if (account != value)
				{
					Logger.Debug($"Setting account to new value {value}");
					account = value;
					OnPropertyChaged();
				}
			}
		}
		public IList<IObservationConfiguration> Configuration { get; } = new List<IObservationConfiguration>();

		private RunningState state = RunningState.NotRunning;
		public RunningState State
		{
			get
			{
				Logger.Debug($"Returning state with value {state}");
				return state;
			}
			private set
			{
				if (state != value)
				{
					Logger.Debug($"Setting state to new value {value}");
					state = value;
					OnPropertyChaged();
				}
			}
		}

		private IProcessList watchedProcesses = new ProcessList();
		public IProcessList WatchedProcesses
		{
			get
			{
				Logger.Debug($"Returning watchedProcesses with value {watchedProcesses}");
				return watchedProcesses;
			}
			set
			{
				if (watchedProcesses != value && value != null)
				{
					Logger.Debug($"Setting watchedProcesses to new value {value}");
					watchedProcesses = value;
					OnPropertyChaged();
				}
			}
		}

		public bool EndAfterUpload { get; set; }

		private DirectoryWatcher DirectoryWatcher { get; set; } = new DirectoryWatcher();
		private FileSearcher Searcher { get; set; } = new FileSearcher();

		private TemplateVideoCreator VideoCreator { get; set; }

		private readonly IYoutubeJobContainer archive;

		private IPlaylistServiceConnectionContainer pscContainer = null;
		public IPlaylistServiceConnectionContainer PscContainer
		{
			get
			{
				Logger.Debug($"Returning pscContainer with value {pscContainer}");
				return pscContainer;
			}
			set
			{
				if (pscContainer != value)
				{
					Logger.Debug($"Setting pscContainer to new value {value}");
					pscContainer = value;
					OnPropertyChaged();
				}
			}
		}

		public AutomationUploader(IYoutubeUploader uploader, IYoutubeJobContainer archiveContainer, IPlaylistServiceConnectionContainer pscContainer)
		{
			Logger.Debug($"Creating new instance of AutomationUploader");

			Uploader = uploader;
			archive = archiveContainer;
			PscContainer = pscContainer;
		}

		public AutomationUploader(IYoutubeUploader uploader, IYoutubeJobContainer archiveContainer, IPlaylistServiceConnectionContainer pscContainer, IYoutubeAccount account, IEnumerable<IObservationConfiguration> configurationsToAdd)
			: this(uploader, archiveContainer, pscContainer)
		{
			Logger.Debug($"Creating new instance of AutomationUploader with account with title {account?.Title ?? null}");
			Account = account;

			foreach (var config in configurationsToAdd)
			{
				Logger.Debug($"Adding configuration for template '{config.Template.Name}' and path '{config.PathInfo.Fullname}'");
				Configuration.Add(config);
			}
		}

		public event FileToUploadPlannedEventHandler FileToUploadOccured;

		public void Cancel(bool cancelYoutubeUploader)
		{
			Logger.Debug($"Received cancel request");

			if (State == RunningState.Running)
			{
				Logger.Info($"Cancelling auto uploader");

				State = RunningState.CancelPending;
				Uploader.StopAfterCompleting = true;
				Searcher.Cancel();
				DirectoryWatcher.Cancel();

				if (cancelYoutubeUploader)
				{
					Logger.Info($"Cancelling youtube uploader");
					Uploader.CancelAll();
				}

				RefreshState();
			}
		}

		public async void StartAsync()
		{
			Logger.Debug($"Received start async request");

			if (State == RunningState.NotRunning)
			{
				Logger.Info($"Starting auto uploader asynchronously");

				await Task.Run(() => Start(Configuration
					.Where(c => !c.IgnorePath)
					.Select(pto => new PublishTimeCalculator(pto.PathInfo, pto.Template))
					.ToList()));
			}
		}

		public async void StartWithExtraConfigAsync()
		{
			Logger.Debug($"Received start with extra config async request");

			if (State == RunningState.NotRunning)
			{
				Logger.Info($"Starting auto uploader asynchronously");
				await Task.Run(() => Start(Configuration
					.Where(c => !c.IgnorePath)
					.Select(pto =>
                    {
                        var calculator = new PublishTimeCalculator(
                        pto.PathInfo,
                        pto.StartDate,
                        pto.Template,
                        pto.HasCustomStartDayIndex ? pto.CustomStartDayIndex : null)
                        {
                            UploadPrivate = pto.UploadPrivate
                        };
                        return calculator;
                    })
				.ToList()));
			}
		}

		private void Start(IList<PublishTimeCalculator> infos)
		{
			Logger.Info($"Starting auto uploader");
			if (Account == null || Uploader == null)
			{
				Logger.Error($"Either the account or the uploader were not set - can't start auto uploader!");
				return;
			}

			State = RunningState.Running;

			WatchedProcesses.ProcessesCompleted += OnProcessesCompleted;

			VideoCreator = new TemplateVideoCreator(infos, PscContainer);

			Uploader.PropertyChanged += UploaderPropertyChanged;
			Searcher.PropertyChanged += SearcherPropertyChanged;
			Uploader.StopAfterCompleting = false;

			Searcher.FileFound += OnFileToUploadOccured;
			DirectoryWatcher.FileAdded += OnFileToUploadOccured;

			foreach (var config in Configuration.Where(c => !c.IgnorePath && Directory.Exists(c.PathInfo.Fullname)))
			{
				Logger.Info($"Starting file searcher and directory watcher for configuration for template '{config.Template.Name}' and path '{config.PathInfo.Fullname}'");

				var pathInfo = config.PathInfo;
				Searcher.SearchFilesAsync(pathInfo.Fullname, pathInfo.Filter, pathInfo.SearchRecursively, pathInfo.SearchHidden, pathInfo.SearchOrder);
				DirectoryWatcher.AddWatcher(pathInfo.Fullname, pathInfo.Filter, pathInfo.SearchRecursively);
			}

			Logger.Info($"Starting youtube uploader");
			Uploader.StartUploader();

			Logger.Info($"Auto uploader was started successfully");
		}

		private void OnProcessesCompleted(object sender, System.EventArgs e)
		{
			Logger.Debug($"Received a process completed event");

			if (EndAfterUpload && uploader.State != UploaderState.Uploading && uploader.State != UploaderState.CancelPending && Searcher.State == RunningState.NotRunning)
			{
				Logger.Info($"Stopping auto uploader");

				if (uploader.State == UploaderState.Waiting)
				{
					Logger.Info($"Stopping youtube uploader");
					uploader.CancelAll();
				}

				if (DirectoryWatcher.State == RunningState.Running)
				{
					Logger.Info($"Stopping directory watcher");
					DirectoryWatcher.Cancel();
				}

				RefreshState();
			}
		}

		private void SearcherPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Logger.Debug($"Received a searcher completed event");

			if (Searcher.State == RunningState.NotRunning)
			{
				RefreshState();
			}
		}

		private void UploaderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Logger.Debug($"Received a uploader property changed event for property {e.PropertyName}");

			if (e.PropertyName == nameof(Uploader.State))
			{
				if (Uploader.State == UploaderState.Waiting)
				{
					Logger.Info($"Uploader state changed to waiting -> end if should end afterwards");
					WatchedProcesses.ProcessesCompleted += ProcessesCompleted;
					EndIfShouldEnd();
				}
				else if (Uploader.State == UploaderState.Uploading)
				{
					Logger.Info($"Uploader state changed to uploading -> can't end until it's finished or cancelled");
					WatchedProcesses.ProcessesCompleted -= ProcessesCompleted;
				}
				else if (Uploader.State == UploaderState.NotRunning)
				{
					Logger.Info($"Uploader state changed to not running -> end if should end");
					DirectoryWatcher.Cancel();
					EndIfShouldEnd();
				}
			}
		}

		private void EndIfShouldEnd()
		{
			if (EndAfterUpload
				&& WatchedProcesses.AllProcessesCompleted
				&& Uploader.State != UploaderState.Uploading
				&& (Uploader.State != UploaderState.Waiting || !Uploader.Queue.Any(u => u.State == JobState.NotStarted && !u.ShouldBeSkipped))
				&& Searcher.State != RunningState.Running)
			{
				Logger.Info($"Auto uploader should end now");

				Uploader.CancelAll();
				DirectoryWatcher.Cancel();
				RefreshState();
			}
		}

		private void ProcessesCompleted(object sender, System.EventArgs e)
		{
			Logger.Debug($"Received a process completed event");
			EndIfShouldEnd();
		}

		private void RefreshState()
		{
			Logger.Debug($"Refreshing state");

			if (Searcher.State == RunningState.NotRunning
				&& DirectoryWatcher.State == RunningState.NotRunning
				&& Uploader.State == UploaderState.NotRunning)
			{
				Logger.Info($"Auto uploader state is now 'not running'");
				State = RunningState.NotRunning;
			}
		}

		private void OnFileToUploadOccured(FileSystemEventArgs e)
		{
			Logger.Debug($"Received a file found event for file '{e.FullPath}'");

			if (!e.Name.StartsWith("_")
				&& Uploader.Queue.All(job => job.Video.File.FullName.ToLower() != e.FullPath.ToLower())
				&& archive.RegisteredJobs.All(job => job.Video.File.FullName.ToLower() != e.FullPath.ToLower()))
			{
				Logger.Info($"Found file to upload: '{e.FullPath}', adding it to uploader");

				var videoAndEvaluator = VideoCreator.CreateVideo(e.FullPath);
				var video = videoAndEvaluator.Video;
				var evaluator = videoAndEvaluator.Evaluator;
				var notificationSettings = videoAndEvaluator.NotificationSettings;

				Logger.Info($"Video title: '{video.Title}', queueing it");

				var job = Uploader.QueueUpload(video, Account, notificationSettings);
				var path = VideoCreator.FindNearestPath(e.FullPath);

				FileToUploadOccured?.Invoke(this, new JobEventArgs(job));

				job.UploadCompletedAction += (args) => evaluator.CleanUp().Wait();

				if (path.MoveAfterUpload)
				{
					Logger.Info($"Moving finished video file from: '{video.Path}' to: '{path.MoveDirectoryPath}'");
					job.UploadCompletedAction += (args) => MoveVideo(args.Job, path.MoveDirectoryPath);
				}

				Uploader.StartUploader();
			}
		}

		public void MoveVideo(IYoutubeJob job, string moveDirectory)
		{
			Logger.Debug($"Moving file: '{job.Video.Path}' to directory: '{moveDirectory}'");

			if (File.Exists(job.Video.Path))
			{
				var canMove = true;
				if (!Directory.Exists(moveDirectory))
				{
					Logger.Info($"Creating directory: '{moveDirectory}'");

					try
					{
						Directory.CreateDirectory(moveDirectory);
					}
					catch (Exception ex)
					{
						Logger.Info($"Could not create directory, exception occured!", ex);
						canMove = false;
					}
				}

				if (canMove)
				{
					Logger.Info($"Trying to move video file");
					try
					{
						string filename = Path.GetFileNameWithoutExtension(job.Video.Path);
						string extension = Path.GetExtension(job.Video.Path);

						string fullFileName = filename + extension;
						var movedFullName = Path.Combine(moveDirectory, fullFileName);

						int i = 2;
						while (File.Exists(movedFullName))
						{
							fullFileName = $"{filename} ({i}){extension}";

							Logger.Info($"File '{movedFullName}' exists, trying new file name '{fullFileName}'");

							i++;
							movedFullName = Path.Combine(moveDirectory, fullFileName);
						}

						Logger.Info($"Moving video file to path: '{movedFullName}'");
						File.Move(job.Video.Path, movedFullName);
						job.Video.Path = movedFullName;

						Logger.Info($"Video file was moved successfully");
					}
					catch (Exception ex)
					{
						Logger.Info($"Could not move video file, exception occured!", ex);
					}
				}
			}
		}

		#region PropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChaged([CallerMemberName] string caller = "")
		{
			Logger.Debug($"Property {caller} changed, invoking handler");
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
		}
		#endregion PropertyChanged
	}
}
