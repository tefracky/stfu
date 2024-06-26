﻿using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Services;

namespace STFU.Lib.Youtube.Upload.Steps
{
	public class ThumbnailUploadStep : ThrottledUploadStep
	{
		public override double Progress
		{
			get
			{
				if (FileStream != null)
				{
					return ((double)FileStream.Position) * 100 / FileStream.Length;
				}

				return 0;
			}
		}

		public ThumbnailUploadStep(IYoutubeJob job)
			: base(job) { }

		internal override void Run()
		{
			if (File.Exists(Video.ThumbnailPath))
			{
				Logger.Info($"Uploading thumbnail of path '{Video.ThumbnailPath}'");

				HttpWebRequest request = CreateThumbnailUploadRequest();

				Logger.Info($"Uploading thumbnail to '{request.Method} {request.RequestUri}'");

				Upload(Video.ThumbnailPath, request);

				if (FinishedSuccessful)
				{
					Logger.Info($"Finishing thumbnail upload request");

					request.Headers.Set("Authorization", $"Bearer {Account.GetActiveToken()}");
					var thumbnailResource = WebService.Communicate(request);

					Logger.Info($"Thumbnail upload of file '{Video.ThumbnailPath}' finished successfully");

					Status.QuotaReached = QuotaProblemHandler.IsQuotaLimitReached(thumbnailResource);
				}
			}
			else
			{
				Logger.Warn($"Skipping thumbnail upload since a thumbnail did not exist or maybe should not be uploaded. Thumbnail path: '{Video.ThumbnailPath}'");

				// Keine Datei -> Upload war erfolgreich
				FinishedSuccessful = true;
			}

			OnStepFinished();
		}

		private HttpWebRequest CreateThumbnailUploadRequest()
		{
			var accessToken = Account.GetActiveToken();
			var secret = Account.Access.First(a => a.AccessToken == accessToken).Client.Secret;

			var request = HttpWebRequestCreator.CreateWithAuthHeader(
				$"https://www.googleapis.com/upload/youtube/v3/thumbnails/set?videoId={Video.Id}&key={secret}",
				"POST", accessToken);

			FileInfo file = new FileInfo(Video.ThumbnailPath);
			request.ContentLength = file.Length;
			request.ContentType = MimeMapping.GetMimeMapping(Video.ThumbnailPath);
			return request;
		}

		public override void Cancel()
		{
			if (RunningTask != null && RunningTask.Status == TaskStatus.Running)
			{
				Logger.Info($"Cancelling thumbnail upload");
				CancellationTokenSource.Cancel();
			}
		}
	}
}
