using System;
using System.IO;
using log4net;
using Newtonsoft.Json;
using STFU.Lib.Youtube.Persistor.Model;

namespace STFU.Lib.Youtube.Persistor
{
	public class AutoUploaderSettingsPersistor
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(AutoUploaderSettingsPersistor));

		public string Path { get; private set; } = null;
		public AutoUploaderSettings Settings { get; private set; } = null;
		public AutoUploaderSettings Saved { get; private set; } = null;

		public AutoUploaderSettingsPersistor(AutoUploaderSettings settings, string path)
		{
			Logger.Debug($"Creating autouploader settings persistor for path '{path}'");

			Path = path;
			Settings = settings;
		}

		public bool Load()
		{
			Logger.Info($"Loading autouploader settings from path '{Path}'");
			Settings.Reset();

			bool worked = true;

			try
			{
				if (File.Exists(Path))
				{
					using (StreamReader reader = new StreamReader(Path))
					{
						var json = reader.ReadToEnd();
						Logger.Debug($"Json from loaded path: '{json}'");

						var settings = JsonConvert.DeserializeObject<AutoUploaderSettings>(json);
						Settings.ShowReleaseNotes = settings.ShowReleaseNotes;

						Logger.Info($"Loaded autouploader settings");
					}
				}

				RecreateSaved();
			}
			catch (Exception e)
			when (e is UnauthorizedAccessException
			|| e is ArgumentException
			|| e is ArgumentNullException
			|| e is DirectoryNotFoundException
			|| e is PathTooLongException
			|| e is IOException)
			{
				Logger.Error($"Could not load autouploader settings, exception occured!", e);
				worked = false;
			}

			return worked;
		}

		public bool Save()
		{
			Logger.Info($"Saving autouploader settings to file '{Path}'");
			var json = JsonConvert.SerializeObject(Settings);

			var worked = true;
			try
			{
				using (StreamWriter writer = new StreamWriter(Path, false))
				{
					writer.Write(json);
				}
				Logger.Info($"Autouploader settings saved");

				RecreateSaved();
			}
			catch (Exception e)
			when (e is UnauthorizedAccessException
			|| e is ArgumentException
			|| e is ArgumentNullException
			|| e is DirectoryNotFoundException
			|| e is PathTooLongException
			|| e is IOException)
			{
				Logger.Error($"Could not save autouploader settings, exception occured!", e);
				worked = false;
			}

			return worked;
		}

		private void RecreateSaved()
		{
			Logger.Debug($"Recreating cache of saved autouploader settings");
			Saved = new AutoUploaderSettings
            {
                ShowReleaseNotes = Settings.ShowReleaseNotes
            };
        }
	}
}
