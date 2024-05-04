using System;
using Newtonsoft.Json;

namespace STFU.Lib.Playlistservice.Model
{
	public class Task
	{
		public long Id { get; set; }
		public string VideoId { get; set; }
		public string PlaylistId { get; set; }
		public string VideoTitle { get; set; }
		public string PlaylistTitle { get; set; }
		public DateTime AddAt { get; set; }
		public TaskState State { get; set; }
		public int AttemptCount { get; set; }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
