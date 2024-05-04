using System.Collections.Generic;
using log4net;
using STFU.Lib.Youtube.Interfaces.Model;

namespace STFU.Lib.Youtube.Services
{
	public static class YoutubeAccountAccessExtension
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(YoutubeAccountAccessExtension));

		public static string GetActiveToken(this IList<IYoutubeAccountAccess> access)
		{
			Logger.Debug($"Returning active access token for one of {access.Count} accesses");
			return YoutubeAccountService.GetAccessToken(access);
		}

		public static string GetActiveToken(this IYoutubeAccount account)
		{
			Logger.Debug($"Returning active access token for account with id: {account.Id} and title: '{account.Title}'");
			return YoutubeAccountService.GetAccessToken(account);
		}
	}
}
