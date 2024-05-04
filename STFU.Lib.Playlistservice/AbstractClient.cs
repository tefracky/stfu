using System;
using log4net;

namespace STFU.Lib.Playlistservice
{
	public abstract class AbstractClient
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(AbstractClient));

		protected Uri Host { get; set; }
		protected string Username { get; set; }
		protected string Password { get; set; }

		public AbstractClient(Uri host)
		{
			Logger.Info($"Creating new AbstractClient with host {host}");
			Host = host;
		}

		public AbstractClient(Uri host, string user, string pass)
			: this(host)
		{
			Logger.Info($"Creating new AbstractClient with user {user} and pass {pass}");
			Username = user;
			Password = pass;
		}
	}
}
