using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using STFU.Lib.Common;
using STFU.Lib.Youtube.Automation.Interfaces.Model;
using STFU.Lib.Youtube.Interfaces.Model.Enums;

namespace STFU.Lib.Youtube.Automation.Internal
{
	internal delegate void FileFound(FileSystemEventArgs e);

	internal class FileSearcher : INotifyPropertyChanged
	{
		private static ILog Logger { get; set; } = LogManager.GetLogger(nameof(FileSearcher));

		private int runningCount = 0;

		internal event FileFound FileFound;

		private RunningState state = RunningState.NotRunning;
		internal RunningState State
		{
			get
			{
				return state;
			}
			set
			{
				if (value != state)
				{
					Logger.Info($"State of file searcher changed to {value}");

					state = value;
					OnPropertyChaged();
				}
			}
		}

		internal void Cancel()
		{
			if (State != RunningState.NotRunning)
			{
				Logger.Info($"Canceling file searcher");

				State = RunningState.CancelPending;
			}
		}

		internal async void SearchFilesAsync(string path, string filters, bool searchRecursively, bool searchHidden, FoundFilesOrderByFilter order)
		{
			Logger.Info($"Searching files async");

			await Task.Run(() => SearchFiles(path, filters, searchRecursively, searchHidden, order));
		}

		private void SearchFiles(string path, string filters, bool searchRecursively, bool searchHidden, FoundFilesOrderByFilter order)
		{
			try
			{
				Logger.Info($"Searching for files");
				Logger.Info($"Parameters => path: '{path}', filters: '{filters}', recursive: {searchRecursively}, hidden: {searchHidden}, order by: {order}");

				runningCount++;
				State = RunningState.Running;

				if (State == RunningState.CancelPending)
				{
					Logger.Info($"Canceling search");

					return;
				}

				if (!searchHidden && new DirectoryInfo(path).Attributes.HasFlag(FileAttributes.Hidden))
				{
					Logger.Info($"Path '{path}' is hidden and hidden search is disabled => skipping directory");
					return;
				}

				string[] singleFilters = filters.Split(';');

				foreach (var filter in singleFilters)
				{
					var files = Directory.GetFiles(path, filter).Select(p => new FileInfo(p)).ToArray();

					Logger.Info($"Found {files.Length} files for filter '{filter}' in path '{path}'");

					switch (order)
					{
						case FoundFilesOrderByFilter.NameAsc:
							files = files.OrderBy((file) => file.Name).ToArray();
							break;
						case FoundFilesOrderByFilter.NameDsc:
							files = files.OrderByDescending((file) => file.Name).ToArray();
							break;
						case FoundFilesOrderByFilter.CreationDateAsc:
							files = files.OrderBy((file) => file.CreationTimeUtc).ToArray();
							break;
						case FoundFilesOrderByFilter.CreationDateDsc:
							files = files.OrderByDescending((file) => file.CreationTimeUtc).ToArray();
							break;
						case FoundFilesOrderByFilter.ChangedDateAsc:
							files = files.OrderBy((file) => file.LastWriteTimeUtc).ToArray();
							break;
						case FoundFilesOrderByFilter.ChangedDateDsc:
							files = files.OrderByDescending((file) => file.LastWriteTimeUtc).ToArray();
							break;
						case FoundFilesOrderByFilter.SizeAsc:
							files = files.OrderBy((file) => file.Length).ToArray();
							break;
						case FoundFilesOrderByFilter.SizeDsc:
							files = files.OrderByDescending((file) => file.Length).ToArray();
							break;
						default:
							files = files.OrderBy((file) => file.Name).ToArray();
							break;
					}

					foreach (var file in files.Where(f => !f.Name.StartsWith("_") && IsVideoAnalyzer.IsVideo(f.Name)))
					{
						Logger.Info($"Found video file '{file.FullName}'");

						FileFound?.Invoke(new FileSystemEventArgs(WatcherChangeTypes.All, file.DirectoryName, file.Name));
					}
				}

				if (searchRecursively)
				{
					Logger.Info($"Recursive mode is enabled => searching sub directories");

					Directory.GetDirectories(path)
						.ToList()
						.ForEach(s => SearchFiles(s, filters, true, searchHidden, order));
				}
			}
			catch (UnauthorizedAccessException ex)
			{
				Logger.Error($"Access to directory was denied", ex);
			}
			catch (ThreadAbortException ex)
			{
				Logger.Error($"Search thread was aborted", ex);
			}
			finally
			{
				Logger.Info($"Search for files in directory '{path}' has been finished");

				runningCount--;

				if (runningCount == 0)
				{
					Logger.Info($"Searcher finished search");

					State = RunningState.NotRunning;
				}
			}
		}

		#region PropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChaged([CallerMemberName] string caller = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
		}
		#endregion PropertyChanged
	}
}
