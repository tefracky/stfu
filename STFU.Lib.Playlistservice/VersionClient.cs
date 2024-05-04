using System;
using System.Net;
using log4net;
using STFU.Lib.Youtube.Services;

namespace STFU.Lib.Playlistservice
{
	public class VersionClient : AbstractClient
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(VersionClient));

		public VersionClient(Uri host) : base(host) { }
		public VersionClient(Uri host, string user, string pass) : base(host, user, pass) { }

		public bool IsAvailable()
		{
			Logger.Info($"Checking if the service is available");

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(Host, "/version"));
			request.Method = "GET";
			request.Accept = "application/json";

			if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
			{
				Utils.AddBasicAuth(request, Username, Password);
			}

            string answer = WebService.Communicate(request, out var ex);
			bool available = ex == null && !string.IsNullOrWhiteSpace(answer);

			Logger.Info($"Service is available: {available}");

			return available;
		}
	}
}
