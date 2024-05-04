using Newtonsoft.Json;

namespace STFU.Lib.Youtube.Model.Serializable
{
	public class YoutubePlaylistItem
	{
		public YoutubePlaylistItem(string playlistId, string videoId)
		{
			Snippet = new PlaylistSnippet(playlistId, videoId);
		}

		public PlaylistSnippet Snippet { get; set; } 
	}

	public class PlaylistSnippet
	{
		public PlaylistSnippet(string playlistId, string videoId)
		{
			this.PlaylistId = playlistId;

			ResourceId = new VideoResource
            {
                VideoId = videoId
            };
        }

		[JsonProperty(PropertyName = "playlistId")]
		public string PlaylistId { get; set; }

		[JsonProperty(PropertyName = "resourceId")]
		public VideoResource ResourceId { get; set; }
	}

	public class VideoResource
	{
		[JsonProperty(PropertyName = "kind")]
		public string Kind { get; set; } = "youtube#video";

		[JsonProperty(PropertyName = "videoId")]
		public string VideoId { get; set; }
	}
}
