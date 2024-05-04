using System.Linq;
using log4net;

namespace STFU.Lib.Common
{
	public static class IsVideoAnalyzer
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(IsVideoAnalyzer));

		static readonly string[] _allowedVideoTypes = new[]
		{
			".mkv",
			".mov",
			".mp4",
			".avi",
			".wmv",
			".mpegps",
			".flv",
			".3gp",
			".webm",
			".ogv",
			".mpg",
			".mpeg",
			".m2v",
			".3g2",
			".webm",
			".ogg"
		};

		public static bool IsVideo(string filename)
		{
			bool isVideo = _allowedVideoTypes.Any(extension => filename.ToLower().EndsWith(extension));
			Logger.Info($"Is '{filename}' a video: {isVideo}");
			return isVideo;
		}
	}
}
