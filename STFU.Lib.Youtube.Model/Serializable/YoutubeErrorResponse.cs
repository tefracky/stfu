namespace STFU.Lib.Youtube.Model.Serializable
{
	public class YoutubeErrorResponse
	{
		public YoutubeErrorArray Error { get; set; }

		public int Code { get; set; }

		public string Message { get; set; }
	}

	public class YoutubeErrorArray
	{
		public YoutubeErrorDetail[] Errors { get; set; }
	}

	public class YoutubeErrorDetail
	{
		public string Domain { get; set; }
		public string Reason { get; set; }
		public string Message { get; set; }
	}
}