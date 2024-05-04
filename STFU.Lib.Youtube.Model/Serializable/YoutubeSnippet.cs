using Newtonsoft.Json;

namespace STFU.Lib.Youtube.Model.Serializable
{
	public class YoutubeSnippet
	{
		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "tags")]
		public string[] Tags { get; set; }

		[JsonProperty(PropertyName = "categoryId")]
		public int CategoryId { get; set; }

		[JsonProperty(PropertyName = "defaultLanguage")]
		public string DefaultLanguage { get; set; }

		[JsonProperty(PropertyName = "defaultAudioLanguage")]
		public string DefaultAudioLanguage => DefaultLanguage;

		public bool ShouldSerializedefaultLanguage { get { return DefaultLanguage != null; } }

		public bool ShouldSerializedefaultAudioLanguage { get { return DefaultLanguage != null; } }
	}
}
