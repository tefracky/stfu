namespace STFU.Lib.Playlistservice.Model
{
	public class AuthCode
	{
		public string Code { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string RedirectUri { get; set; }
	}
}
