﻿using System;
using System.Linq;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using log4net;
using STFU.Lib.Youtube.Model;

namespace STFU.Lib.Updater
{
	public class VersionChecker
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(VersionChecker));

		public UpdateInformation CheckStfuVersion(string currentVersion)
		{
			Logger.Info($"Checking if there is a newer version available than '{currentVersion}'");

			UpdateInformation infos = new UpdateInformation(false, null, null);

			var file = GetLatestStfuZipInfo();
			if (file != null && NewVersionAvailable(file, currentVersion))
			{
				Logger.Info($"Newer version available, creating new update information");
				infos = new UpdateInformation(true, file.Name.Split('-')[1], file.Id);
			}

			return infos;
		}

		private bool NewVersionAvailable(File file, string currentVersion)
		{
			var newestVersionArray = file.Name.Split('-')[1].Replace("v", string.Empty).Split('.');
			var currentVersionArray = currentVersion.Replace("v", string.Empty).Split('.');

			bool isAvailable = false;
			for (int i = 0; i < newestVersionArray.Length; i++)
			{
				int newestVersionPart = GetVersionNumber(newestVersionArray, i);
				int currentVersionPart = GetVersionNumber(currentVersionArray, i);

				if (newestVersionPart > currentVersionPart)
				{
					// newer version available
					isAvailable = true;
					break;
				}
				else if (newestVersionPart < currentVersionPart)
				{
					// currentVersion is greater (???) than the newest version. Shouldn't happen but we still ne to stop immediately..
					break;
				}
			}

			return isAvailable;
		}

		private int GetVersionNumber(string[] versionsArray, int position)
		{
			if (position >= versionsArray.Length)
			{
				return 0;
			}

			return int.Parse(versionsArray[position]);
		}

		private File GetLatestStfuZipInfo()
		{
			Logger.Info($"Loading latest stfu version from google drive");

            var initializer = new BaseClientService.Initializer
            {
                ApiKey = YoutubeClientData.UpdaterApiKey
            };
            var service = new DriveService(initializer);

			var request = service.Files.List();
			request.Q = "'1kCRPLg-95PjbQKjEpj-HW7tjvzmZ87RI' in parents";

			FileList result = null;
			try
			{
				result = request.Execute();
			}
			catch (Exception)
			{ }

			var file = result?.Files?.FirstOrDefault(f => f.MimeType == "application/x-zip-compressed");

			Logger.Info($"Found newest version file '{file.Name}'");

			return file;
		}
	}
}
