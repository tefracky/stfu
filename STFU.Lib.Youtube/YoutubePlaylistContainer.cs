using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using log4net;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model;

namespace STFU.Lib.Youtube
{
	public class YoutubePlaylistContainer : IYoutubePlaylistContainer
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(YoutubePlaylistContainer));

		private IList<IYoutubePlaylist> Playlists { get; } = new List<IYoutubePlaylist>();

		public IReadOnlyCollection<IYoutubePlaylist> RegisteredPlaylists => new ReadOnlyCollection<IYoutubePlaylist>(Playlists);

		public void RegisterPlaylist(IYoutubePlaylist playlist)
		{
			if (!RegisteredPlaylists.Any(p => p == playlist))
			{
				Logger.Debug($"Adding a new playlist, title: '{playlist.Title}'");
				Playlists.Add(playlist);
			}
		}

		public void RegisterPlaylist(int newPosition, IYoutubePlaylist playlist)
		{
			if (!RegisteredPlaylists.Any(p => p == playlist))
			{
				Logger.Debug($"Adding a new playlist, title: '{playlist.Title}' on position {newPosition}");
				Playlists.Insert(newPosition, playlist);
			}
		}

		public void UnregisterAllPlaylists()
		{
			Logger.Debug($"Removing all playlists");
			Playlists.Clear();
		}

		public void UnregisterPlaylist(IYoutubePlaylist playlist)
		{
			if (Playlists.Contains(playlist))
			{
				Logger.Debug($"Removing playlist, title: '{playlist.Title}'");
				Playlists.Remove(playlist);
			}
		}

		public void UnregisterPlaylistAt(int index)
		{
			if (Playlists.Count > index)
			{
				Logger.Debug($"Removing playlist at index {index}, title: '{Playlists[index].Title}'");
				Playlists.RemoveAt(index);
			}
		}
	}
}
