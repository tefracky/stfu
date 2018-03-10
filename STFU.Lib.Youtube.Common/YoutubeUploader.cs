﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using STFU.Lib.Youtube.Common.InternalInterfaces;
using STFU.Lib.Youtube.Common.Model;
using STFU.Lib.Youtube.Common.Upload;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Interfaces.Model.Enums;

namespace STFU.Lib.Youtube.Common
{
	public class YoutubeUploader : IYoutubeUploader
	{
		private int maxSimultaneousUploads = 1;
		private IList<IYoutubeJob> jobQueue = new List<IYoutubeJob>();
		private UploaderState state = UploaderState.NotRunning;
		private bool stopAfterCompleting = true;

		private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		private IList<IYoutubeJobUploader> runningJobUploaders = new List<IYoutubeJobUploader>();

		/// <see cref="IYoutubeUploader.MaxSimultaneousUploads"/>
		public int MaxSimultaneousUploads
		{
			get
			{
				return maxSimultaneousUploads;
			}

			set
			{
				if (maxSimultaneousUploads != value && value > 0)
				{
					maxSimultaneousUploads = value;
					OnPropertyChanged();
				}
			}
		}

		private IList<IYoutubeJob> JobQueue
		{
			get
			{
				return jobQueue;
			}

			set
			{
				if (jobQueue != value)
				{
					jobQueue = value;
					OnPropertyChanged();
				}
			}
		}

		/// <see cref="IYoutubeUploader.Queue"/>
		public IReadOnlyCollection<IYoutubeJob> Queue => new ReadOnlyCollection<IYoutubeJob>(JobQueue);

		/// <see cref="IYoutubeUploader.State"/>
		public UploaderState State
		{
			get
			{
				return state;
			}

			internal set
			{
				if (state != value)
				{
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
				return stopAfterCompleting;
			}

			set
			{
				if (stopAfterCompleting != value)
				{
					stopAfterCompleting = value;
					OnPropertyChanged();
				}
			}
		}

		/// <see cref="IYoutubeUploader.QueueUpload(IYoutubeJob)"/>
		public IYoutubeJob QueueUpload(IYoutubeVideo video, IYoutubeAccount account)
		{
			if (Queue.Any(job => job.Video == video && job.Account == account))
			{
				throw new ArgumentException("Der aktuelle Job ist bereits in der Warteschlange vorhanden.");
			}

			var newJob = new InternalYoutubeJob(video, account);
			JobQueue.Add(newJob);

			return newJob;
		}

		/// <see cref="IYoutubeUploader.CancelUploader"/>
		public void CancelUploader()
		{
			throw new NotImplementedException();
		}

		/// <see cref="IYoutubeUploader.ChangePositionInQueue(IYoutubeJob, IYoutubeJob)"/>
		public void ChangePositionInQueue(IYoutubeJob first, IYoutubeJob second)
		{
			if (!Queue.Contains(first))
			{
				throw new ArgumentException("Der erste angegebene Job ist nicht in der Warteschlange vorhanden.");
			}
			if (!Queue.Contains(second))
			{
				throw new ArgumentException("Der zweite angegebene Job ist nicht in der Warteschlange vorhanden.");
			}

			int firstPos = JobQueue.IndexOf(first);
			int secondPos = JobQueue.IndexOf(second);

			JobQueue[firstPos] = second;
			JobQueue[secondPos] = first;
		}

		/// <see cref="IYoutubeUploader.RemoveFromQueue(IYoutubeJob)"/>
		public void RemoveFromQueue(IYoutubeJob job)
		{
			if (!Queue.Contains(job))
			{
				throw new ArgumentException("Der Job ist nicht in der Warteschlange vorhanden.");
			}

			JobQueue.Remove(job);
		}

		/// <see cref="IYoutubeUploader.StartUploader"/>
		public void StartUploader()
		{
			if (State == UploaderState.NotRunning)
			{
				State = UploaderState.Waiting;
				StartJobUploaders();
			}
		}

		private void StartJobUploaders()
		{
			while (runningJobUploaders.Count < MaxSimultaneousUploads
				&& Queue.Any(job => job.State == UploadState.NotStarted))
			{
				var nextJob = Queue.First(job => job.State == UploadState.NotStarted);
				nextJob.PropertyChanged += RunningJobPropertyChanged;

				var jobUploader = new YoutubeJobUploader(nextJob as InternalYoutubeJob);
				var task = jobUploader.UploadAsync(cancellationTokenSource.Token);

				runningJobUploaders.Add(jobUploader);
			}

			if (runningJobUploaders.Count == 0)
			{
				State = UploaderState.Waiting;
			}
			else
			{
				State = UploaderState.Uploading;
			}
		}

		private void RunningJobPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var job = sender as IYoutubeJob;
			if (e.PropertyName == nameof(IYoutubeJob.State) && job.State != UploadState.Running && job.State != UploadState.ThumbnailUploading)
			{
				var jobUploader = runningJobUploaders.Single(upl => upl.Job == job);
				runningJobUploaders.Remove(jobUploader);

				StartJobUploaders();
			}
		}

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName]string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion INofityPropertyChanged
	}
}
