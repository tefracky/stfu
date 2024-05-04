using System;
using System.Linq;
using STFU.Lib.Youtube.Interfaces.Model;

namespace STFU.Lib.Youtube.Model.Serializable
{
	public class SerializableYoutubeVideo
	{
		public YoutubeSnippet Snippet { get; set; }
		public YoutubeStatus Status { get; set; }

		public string Id { get; set; }

		public static SerializableYoutubeVideo Create(IYoutubeVideo video)
		{
			var svideo = new SerializableYoutubeVideo
            {
                Id = video.Id,
                Snippet = new YoutubeSnippet
                {
                    CategoryId = video.Category?.Id ?? 20,
                    Title = video.Title,
                    DefaultLanguage = video.DefaultLanguage?.Hl ?? "de-de",
                    Description = video.Description,
                    Tags = video.Tags.ToArray()
                },
                Status = new YoutubeStatus
                {
                    IsEmbeddable = video.IsEmbeddable,
                    Privacy = video.Privacy,
                    License = video.License,
                    PublishAt = (video.PublishAt ?? default).ToString("yyyy-MM-ddTHH:mm:ss.ffffzzz"),
                    ShouldPublishAt = video.PublishAt != null,
                    PublicStatsViewable = video.PublicStatsViewable
                }
            };

            return svideo;
		}
	}
}

