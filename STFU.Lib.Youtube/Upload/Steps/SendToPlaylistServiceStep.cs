using System;
using STFU.Lib.Playlistservice;
using STFU.Lib.Playlistservice.Model;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Interfaces.Model.Enums;

namespace STFU.Lib.Youtube.Upload.Steps
{
	public class SendToPlaylistServiceStep : AbstractUploadStep
	{
		private double progress = 0.0;

		public override double Progress => progress;

		public SendToPlaylistServiceStep(IYoutubeJob job)
			: base(job) { }

		internal override void Run()
		{
			progress = 0;

			if (Video.PlaylistServiceSettings.ShouldSend
				&& !string.IsNullOrWhiteSpace(Video.PlaylistServiceSettings.Host)
				 && !string.IsNullOrWhiteSpace(Video.PlaylistServiceSettings.Port))
			{
				var taskClient = new TaskClient(new Uri($"http://{Video.PlaylistServiceSettings.Host}:{Video.PlaylistServiceSettings.Port}"),
					Video.PlaylistServiceSettings.Username, Video.PlaylistServiceSettings.Password);

				if (Video.PlaylistServiceSettings.TaskId == null || !Video.PlaylistServiceSettings.TaskId.HasValue)
				{
					Task task = taskClient.CreateTask(Video.PlaylistServiceSettings.AccountId, new Task()
					{
						AddAt = (Video.Privacy == PrivacyStatus.Private && Video.PublishAt.HasValue) ? Video.PublishAt.Value : DateTime.Now.AddMinutes(5),
						PlaylistId = Video.PlaylistServiceSettings.PlaylistId,
						PlaylistTitle = Video.PlaylistServiceSettings.PlaylistTitle,
						VideoId = Video.Id,
						VideoTitle = Video.Title
					});

					Video.PlaylistServiceSettings.TaskId = task.Id;
				}
				else
				{
					Task task = taskClient.UpdateTask(Video.PlaylistServiceSettings.AccountId, new Task()
					{
						Id = Video.PlaylistServiceSettings.TaskId.Value,
						AddAt = (Video.Privacy == PrivacyStatus.Private && Video.PublishAt.HasValue) ? Video.PublishAt.Value : DateTime.Now.AddMinutes(5),
						PlaylistId = Video.PlaylistServiceSettings.PlaylistId,
						PlaylistTitle = Video.PlaylistServiceSettings.PlaylistTitle,
						VideoId = Video.Id,
						VideoTitle = Video.Title
					});

					Video.PlaylistServiceSettings.TaskId = task.Id;
				}
			}

			FinishedSuccessful = true;
			progress = 100;

			OnStepFinished();
		}

		public override void RefreshDurationAndSpeed()
		{
			Status.CurrentSpeed = 0;
			Status.UploadedDuration = new TimeSpan(0, 0, 0);
			Status.RemainingDuration = new TimeSpan(0, 0, 0);
		}

		public override void Cancel()
		{
			// Höhö, das kann man nicht abbrechen lol
			Logger.Warn($"Called cancel but it is not possible on this type of step");
		}
	}
}
