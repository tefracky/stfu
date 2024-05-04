using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using log4net;
using Newtonsoft.Json;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Model;

namespace STFU.Lib.Youtube.Services
{
	public static class YoutubeAccountService
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(YoutubeAccountService));

		public static string GetAccessToken(IYoutubeAccount account)
		{
			return GetAccessToken(account.Access);
		}

		public static string GetAccessToken(IList<IYoutubeAccountAccess> access, Func<IYoutubeAccountAccess, bool> condition)
		{
			string token = null;

			if (access.Any(ac => !ac.Client?.LimitReached ?? false))
			{
				Logger.Info($"Searching for a valid access");

				var firstUsefullAccess = access.FirstOrDefault(ac => !ac.Client.LimitReached && !ac.IsExpired && condition(ac));

				while (firstUsefullAccess == null && RefreshAccess(access))
				{
					Logger.Info($"Did not find a valid access on the first time, trying to find it after refreshing access");

					firstUsefullAccess = access.FirstOrDefault(ac => !ac.Client.LimitReached && !ac.IsExpired && condition(ac));
				}

				Logger.Info($"Did we find a token? {firstUsefullAccess != null}");

				token = firstUsefullAccess?.AccessToken;
			}

			return token;
		}


		public static string GetAccessToken(IList<IYoutubeAccountAccess> access)
		{
			return GetAccessToken(access, ac => true);
		}

		private static bool RefreshAccess(IList<IYoutubeAccountAccess> access)
		{
			var firstOutdatedAccess = access.FirstOrDefault(ac => !ac.Client.LimitReached && ac.IsExpired && ac.RefreshAllowed);

			bool result = false;
			if (firstOutdatedAccess != null)
			{
				Logger.Info($"Found an outdated access, refreshing it");

				// Content zusammenbauen
				string content = $"client_id={YoutubeClientData.Client.Id}&client_secret={YoutubeClientData.Client.Secret}&refresh_token={firstOutdatedAccess.RefreshToken}&grant_type=refresh_token";
				var bytes = Encoding.UTF8.GetBytes(content);

				// Request erstellen
				WebRequest request = WebRequest.Create($"https://www.googleapis.com/oauth2/v4/token");
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";

				var response = WebService.Communicate(request, bytes);

				if (response != null && !response.Contains("revoked"))
				{
					Logger.Info($"Refresh was successful");

					result = true;

					// Account 
					var authResponse = JsonConvert.DeserializeObject<YoutubeAuthResponse>(response);

					if (!string.IsNullOrWhiteSpace(authResponse.AccessToken))
					{
						var newAccess = new YoutubeAccountAccess
                        {
                            Client = firstOutdatedAccess.Client,
                            AccessToken = authResponse.AccessToken,
                            TokenType = authResponse.TokenType,
                            ExpirationDate = DateTime.Now.AddSeconds(authResponse.ExpiresIn),
                            RefreshToken = firstOutdatedAccess.RefreshToken,
                            ClientId = firstOutdatedAccess.ClientId,
                            HasSendMailPrivilegue = firstOutdatedAccess.HasSendMailPrivilegue
                        };

                        access.Remove(firstOutdatedAccess);
						access.Add(newAccess);
					}
					else
					{
						firstOutdatedAccess.NextRefreshAllowed = DateTime.Now.Add(new TimeSpan(1, 0, 0));
					}
				}
				else
				{
					Logger.Info($"Access has been revoked and cannot be refreshed anymore");
				}
			}

			return result;
		}

		internal static void RevokeAccessOfAccount(IYoutubeAccount account)
		{
			Logger.Info($"Revoking all {account.Access.Count} accesses of account with id: {account.Id} and title: '{account.Title}'");

			while (account.Access.Count > 0)
			{
				RevokeSingleAccess(account, account.Access[0]);
			}

			Logger.Info($"All accesses revoked");
		}

		internal static void RevokeSingleAccess(IYoutubeAccount account, IYoutubeAccountAccess access)
		{
			Logger.Info($"Revoking single access of account with id: {account.Id} and title: '{account.Title}'");

			string address = $"https://accounts.google.com/o/oauth2/revoke?token={access.RefreshToken}";

			WebRequest request = WebRequest.Create(address);
			request.ContentType = "application/x-www-form-urlencoded";

			WebService.Communicate(request);

			account.Access.Remove(access);
		}

		private class YoutubeAuthResponse
		{
			public string AccessToken { get; set; }
			public string TokenType { get; set; }
			public int ExpiresIn { get; set; }
			public string RefreshToken { get; set; }
		}
	}
}
