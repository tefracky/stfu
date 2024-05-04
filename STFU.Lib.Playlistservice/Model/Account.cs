using Newtonsoft.Json;

namespace STFU.Lib.Playlistservice.Model
{
	public class Account
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string ChannelId { get; set; }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
