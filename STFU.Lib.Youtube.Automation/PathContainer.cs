﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using log4net;
using STFU.Lib.Youtube.Automation.Interfaces;
using STFU.Lib.Youtube.Automation.Interfaces.Model;
using STFU.Lib.Youtube.Automation.Internal;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Interfaces.Model.Enums;
using STFU.Lib.Youtube.Model;
using STFU.Lib.Youtube.Upload;

namespace STFU.Lib.Youtube.Automation
{
	public class PathContainer : IPathContainer
	{
		private static ILog Logger { get; set; } = LogManager.GetLogger(nameof(PathContainer));

		public PathContainer() { }

		private IList<IPath> Paths { get; } = new List<IPath>();

		public IReadOnlyCollection<IPath> RegisteredPaths => new ReadOnlyCollection<IPath>(Paths);
		public IReadOnlyCollection<IPath> ActivePaths => new ReadOnlyCollection<IPath>(Paths.Where(p => !p.Inactive).ToList());

		private bool PathIsAlreadyRegistered(IPath path)
		{
			var alreadyRegistered = RegisteredPaths.Any(p => SamePathUsed(path, p));

			Logger.Info($"Is path '{path.Fullname}' already registrered => {alreadyRegistered}");

			return alreadyRegistered;
		}

		private static bool SamePathUsed(IPath path, IPath p)
		{
			var samePathUsed = Path.GetFullPath(path.Fullname).ToLower() == Path.GetFullPath(p.Fullname).ToLower();

			Logger.Info($"Are paths '{path.Fullname}' and '{p.Fullname}' the same => {samePathUsed}");

			return samePathUsed;
		}

		public void RegisterPath(IPath path)
		{
			if (!PathIsAlreadyRegistered(path))
			{
				Logger.Info($"Adding path '{path.Fullname}'");

				Paths.Add(path);
			}
		}

		public void UnregisterPath(IPath path)
		{
			if (RegisteredPaths.Contains(path))
			{
				Logger.Info($"Removing path '{path.Fullname}'");

				Paths.Remove(path);
			}
		}
		public void UnregisterPathAt(int index)
		{
			if (RegisteredPaths.Count > index)
			{
				Logger.Info($"Removing path '{RegisteredPaths.ElementAt(index).Fullname}' at index {index}");

				Paths.RemoveAt(index);
			}
		}

		public void UnregisterAllPaths()
		{
			Logger.Info($"Removing all paths");

			Paths.Clear();
		}

		public void ShiftPathPositions(IPath first, IPath second)
		{
			IPath firstToChange = null;
			IPath secondToChange = null;
			if (first != null
				&& second != null
				&& (firstToChange = Paths.FirstOrDefault(p => p == first)) != null
				&& (secondToChange = Paths.FirstOrDefault(p => p == second)) != null)
			{
				Logger.Info($"Switching positions of paths '{first.Fullname}' and '{second.Fullname}'");

				ShiftPathPositionsAt(Paths.IndexOf(firstToChange), Paths.IndexOf(secondToChange));
			}
		}

		public void ShiftPathPositionsAt(int firstIndex, int secondIndex)
		{
			if (firstIndex >= 0 && secondIndex >= 0 && firstIndex < Paths.Count && secondIndex < Paths.Count)
			{
				Logger.Info($"Switching positions of paths at position '{firstIndex}' and '{secondIndex}'");

				(Paths[firstIndex], Paths[secondIndex]) = (Paths[secondIndex], Paths[firstIndex]);
            }
		}

		private IYoutubeJobContainer queueContainer = null;
		private IYoutubeJobContainer archiveContainer = null;
		private IYoutubeAccountContainer accountContainer = null;

		public void MarkAllFilesAsRead(IPath path, IYoutubeJobContainer queueContainer, IYoutubeJobContainer archiveContainer, IYoutubeAccountContainer accountContainer)
		{
			Logger.Info($"Marking all files from path '{path.Fullname}' as read");

			this.archiveContainer = archiveContainer;
			this.accountContainer = accountContainer;
			this.queueContainer = queueContainer;

			FileSearcher searcher = new FileSearcher();
			searcher.FileFound += SearcherFileFound;

			searcher.SearchFilesAsync(path.Fullname, path.Filter, path.SearchRecursively, path.SearchHidden, path.SearchOrder);

			while (searcher.State != RunningState.NotRunning)
			{
				Thread.Sleep(5);
			}

			Logger.Info($"Finished marking all files from path '{path.Fullname}' as read");
		}

		private void SearcherFileFound(FileSystemEventArgs e)
		{
			if (queueContainer.RegisteredJobs.All(job => Path.GetFullPath(job.Video.Path).ToLower() != Path.GetFullPath(e.FullPath).ToLower()))
			{
				Logger.Info($"Adding file '{e.FullPath}' to archive container");

                var video = new YoutubeVideo(e.FullPath)
                {
                    Title = e.Name
                };
                archiveContainer.RegisterJob(
					new YoutubeJob(video, accountContainer.RegisteredAccounts.FirstOrDefault(), new UploadStatus())
				);
			}
		}
	}
}
