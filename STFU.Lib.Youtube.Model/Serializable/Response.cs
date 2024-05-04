using System;

namespace STFU.Lib.Youtube.Model.Serializable
{
	public class Response
	{
		public string Kind { get; set; }
		public string Etag { get; set; }
		public string PrevPageToken { get; set; }
		public string NextPageToken { get; set; }
		public PageInfo PageInfo { get; set; }
		public Item[] Items { get; set; }
	}

	public class PageInfo
	{
		public int TotalResults { get; set; }
		public int ResultsPerPage { get; set; }
	}

	public class Item
	{
		public string Kind { get; set; }
		public string Etag { get; set; }
		public string Id { get; set; }
		public ContentDetails ContentDetails { get; set; }
		public Snippet Snippet { get; set; }
	}

	public class ContentDetails
	{
		public string VideoId { get; set; }
		public string VideoPublishedAt { get; set; }
	}

	public class Snippet
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string CustomUrl { get; set; }
		public DateTime PublishedAt { get; set; }
		public string Country { get; set; }
		public Thumbnails Thumbnails { get; set; }
		public Localization Localized { get; set; }
		public string ChannelId { get; set; }
		public bool Assignable { get; set; }
		public string Hl { get; set; }
		public string Name { get; set; }
	}

	public class Thumbnails
	{
		public Url Default { get; set; }
		public Url Medium { get; set; }
		public Url High { get; set; }
	}

	public class Url
	{
		public string UrlString { get; set; }
	}

	public class Localization
	{
		public string Title { get; set; }
		public string Description { get; set; }
	}
}
