using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Model.Serializable;
using STFU.Lib.Youtube.Services;

namespace STFU.Lib.Youtube.Upload.Steps
{
	public class VideoUploadStep : ThrottledUploadStep
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

		public VideoUploadStep(IYoutubeJob job)
			: base(job) { }

		internal override void Run()
		{
			Logger.Info($"Uploading video file '{Video.Path}' to youtube");

			// Initialisieren
			GenerateInitUri();
			var request = CreateRequest();

			if (request == null && !Status.QuotaReached)
			{
				Logger.Warn($"Upload could not be continued or old upload address was not valid anymore - restarting upload");
				Logger.Debug($"Old upload address: '{Status.UploadAddress}'");

				// evtl. vorhandene UploadUri hat nicht geklappt => Versuch, den Upload von vorne zu beginnen.
				Status.UploadAddress = null;
				GenerateInitUri();
				request = CreateRequest();
			}

			// Hochladen
			if (!Status.QuotaReached && request != null && File.Exists(Video.Path))
			{
				Logger.Info($"Uploading video file to '{request.Method} {request.RequestUri}'");

				Upload(Video.Path, request);

				if (FinishedSuccessful)
				{
					request.Headers.Set("Authorization", $"Bearer {Account.GetActiveToken()}");
					string result = WebService.Communicate(request);

					Status.QuotaReached = QuotaProblemHandler.IsQuotaLimitReached(result);

					if (!Status.QuotaReached)
					{
						Video.Id = JsonConvert.DeserializeObject<SerializableYoutubeVideo>(result).Id;

						Logger.Info($"Upload finished successful - video is now on youtube with id {Video.Id} - url: https://youtube.com/watch?v={Video.Id}");

						// Status entfernen, damit nicht erneut an die selbe Adresse hochgeladen wird.
						Status.UploadAddress = null;
					}
				}
				else
				{
					Logger.Error($"Upload did not finish successful");
				}
			}

			OnStepFinished();
		}

		private void GenerateInitUri()
		{
			if (Status.UploadAddress == null && !Status.QuotaReached)
			{
				Logger.Info($"Creating upload uri");
				string result = InitializeUploadOnYoutube();

				Status.QuotaReached = QuotaProblemHandler.IsQuotaLimitReached(result);

                if (!Status.QuotaReached && Uri.TryCreate(result, UriKind.Absolute, out var uri))
				{
					Logger.Info($"Upload uri was created: '{uri}'");
					Status.UploadAddress = uri;
				}
			}
		}

		private string InitializeUploadOnYoutube()
		{
			var ytVideo = SerializableYoutubeVideo.Create(Video);
			string content = JsonConvert.SerializeObject(ytVideo);
			var bytes = Encoding.UTF8.GetBytes(content);

			var request = HttpWebRequestCreator.CreateWithAuthHeader(
				$"https://www.googleapis.com/upload/youtube/v3/videos?uploadType=resumable"
				+ $"&autoLevels={Video.AutoLevels}&notifySubscribers={Video.NotifySubscribers}"
				+ $"&stabilize={Video.Stabilize}&part=snippet,status,contentDetails",
				"POST",
				Account.GetActiveToken());

			request.Headers.Add("Slug", Path.GetFileName(new string(Video.File.Name.Where(c => Convert.ToInt32(c) < 128).ToArray())));
			request.Headers.Add($"x-upload-content-length: {Video.File.Length}");
			request.Headers.Add($"x-upload-content-type: {MimeMapping.GetMimeMapping(Video.File.Name)}");

			request.ContentLength = bytes.Length;
			request.ContentType = "application/json; charset=utf-8";

			return WebService.Communicate(request, bytes, "Location");
		}

		private HttpWebRequest CreateRequest()
		{
			if (Status.QuotaReached)
			{
				return null;
			}

			// Default: Upload klappt nicht
			HttpWebRequest request = null;

			Status.LastByte = CheckUploadStatus();

			if (Status.LastByte == -1)
			{
				// Komplette Datei hochladen
				request = HttpWebRequestCreator.CreateForNewUpload(Status.UploadAddress, Video, Account);
			}
			else if (Status.LastByte >= 0)
			{
				// Nur einen Teil der Datei hochladen
				request = HttpWebRequestCreator.CreateForResumeUpload(Status.UploadAddress, Video, Account, Status.LastByte);
			}

			return request;
		}

		private long CheckUploadStatus()
		{
			Logger.Info($"Checking upload status");

			var request = HttpWebRequestCreator.CreateWithAuthHeader(Status.UploadAddress.AbsoluteUri, "PUT", Account.GetActiveToken());
			request.ContentLength = 0;
			request.Headers.Add($"content-range: bytes */{Video.File.Length}");

            var answer = WebService.Communicate(request, out var ex);

			if (answer == null)
			{
				if (ex?.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
				{
					// Upload kann nicht fortgesetzt werden, da Verarbeitung mittlerweile abgebrochen wurde.
					// Workaround: Upload neu starten
					Logger.Error($"Youtube upload cannot be continued because it was paused for too long", ex);
					return -2;
				}
				else if (ex?.Response != null && (int)((HttpWebResponse)ex.Response).StatusCode != 308)
				{
					// Es gab einen anderen unerwarteten Fehler.
					// Auch hier ist der Workaround ein Neustart.
					Logger.Error($"Youtube upload cannot be continued because an exceptioin occured", ex);
					return -3;
				}
				else
				{
					Logger.Info($"Youtube upload should be started from the beginning");
					return -1;
				}
			}

			Status.QuotaReached = QuotaProblemHandler.IsQuotaLimitReached(answer);

			if (Status.QuotaReached)
			{
				return -4;
			}

			answer = answer.Substring("bytes=".Length);

			long lastbyte;
			try
			{
				lastbyte = Convert.ToInt64(answer.Split('-')[1]);
				Logger.Info($"Upload can be continued from byte {lastbyte} onwards");
			}
			catch (Exception exc)
			{
				Logger.Error($"lastbyte could not be parsed - youtube upload should be started from the beginning", exc);
				return -1;
			}

			return lastbyte;
		}

		public override void Cancel()
		{
			Logger.Info($"Canceling video upload");
			CancellationTokenSource.Cancel();
		}
	}
}
