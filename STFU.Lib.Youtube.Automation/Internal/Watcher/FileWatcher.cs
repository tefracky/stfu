﻿using System.Collections.Generic;
using System.IO;
using SPath = System.IO.Path;
using System.Linq;

namespace STFU.Lib.Youtube.Automation.Internal.Watcher
{
	internal delegate void FileAdded(FileSystemEventArgs e);

	internal class FileWatcher
	{
		internal event FileAdded FileAdded;

		private IList<FileSystemWatcher> Watchers { get; } = new List<FileSystemWatcher>();

		internal FileWatcher() { }

		internal void AddWatcher(string path, string filter, bool searchRecursively)
		{
			if (!Watchers.Any(w => SPath.GetFullPath(w.Path).ToLower() != SPath.GetFullPath(path).ToLower()))
			{
				var watcher = new FileSystemWatcher(path, filter);
				watcher.NotifyFilter = NotifyFilters.Attributes
					| NotifyFilters.CreationTime
					| NotifyFilters.DirectoryName
					| NotifyFilters.FileName
					| NotifyFilters.LastAccess
					| NotifyFilters.LastWrite
					| NotifyFilters.Security
					| NotifyFilters.Size;
				watcher.IncludeSubdirectories = searchRecursively;
				watcher.Created += ReactOnFileChanges;
				watcher.Changed += ReactOnFileChanges;
				watcher.Renamed += ReactOnFileChanges;
				watcher.EnableRaisingEvents = true;

				Watchers.Add(watcher);
			}
		}

		internal void Stop()
		{
			while (Watchers.Count > 0)
			{
				Watchers.First().Created -= ReactOnFileChanges;
				Watchers.First().Changed -= ReactOnFileChanges;
				Watchers.First().Renamed -= ReactOnFileChanges;

				Watchers.First().Dispose();
				Watchers.RemoveAt(0);
			}
		}

		private void ReactOnFileChanges(object sender, FileSystemEventArgs e)
		{
			FileAdded?.Invoke(e);
		}
	}
}
