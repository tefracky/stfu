using System;
using System.IO;
using System.Linq;
using log4net;
using Newtonsoft.Json;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Upload;

namespace STFU.Lib.Youtube.Persistor
{
	public class JobPersistor
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(JobPersistor));

		public string Path { get; private set; } = null;
		public IYoutubeJobContainer Container { get; private set; } = null;
		public IYoutubeJobContainer Saved { get; private set; } = null;

		public JobPersistor(IYoutubeJobContainer container, string path)
		{
			Logger.Debug($"Creating job persistor for path '{path}'");

			Path = path;
			Container = container;
		}

		public bool Load()
		{
			Logger.Info($"Loading jobs from path '{Path}'");
			Container.UnregisterAllJobs();

			bool worked = true;

			try
			{
				if (File.Exists(Path))
				{
					using (StreamReader reader = new StreamReader(Path))
					{
						var json = reader.ReadToEnd();
						Logger.Debug($"Json from loaded path: '{json}'");

						var jobs = JsonConvert.DeserializeObject<YoutubeJob[]>(json);
						Logger.Info($"Loaded {jobs.Length} jobs");

						foreach (var loaded in jobs)
						{
							Logger.Info($"Adding job for video '{loaded.Video.Title}'");
							Container.RegisterJob(loaded);
						}
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
				Logger.Error($"Could not load paths, exception occured!", e);
				worked = false;
			}

			return worked;
		}

		public bool Save()
		{
			IYoutubeJob[] jobs = Container.RegisteredJobs.ToArray();
			Logger.Info($"Saving {jobs.Length} jobs to file '{Path}'");

			var json = JsonConvert.SerializeObject(jobs);

			var worked = true;
			try
			{
				using (StreamWriter writer = new StreamWriter(Path, false))
				{
					writer.Write(json);
				}
				Logger.Info($"Jobs saved");

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
				Logger.Error($"Could not save jobs, exception occured!", e);
				worked = false;
			}

			return worked;
		}

		private void RecreateSaved()
		{
			Logger.Debug($"Recreating cache of saved jobs");
			Saved = new YoutubeJobContainer();
			foreach (var job in Container.RegisteredJobs)
			{
				Logger.Debug($"Recreating cache for job with video '{job.Video.Title}'");
				var newJob = new YoutubeJob(job.Video.CreateCopy(), job.Account, job.UploadStatus)
                {
                    State = job.State
                };

                Saved.RegisterJob(newJob);
			}
		}
	}
}
