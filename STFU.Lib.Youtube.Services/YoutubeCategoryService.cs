using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using log4net;
using Newtonsoft.Json;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Model;
using STFU.Lib.Youtube.Model.Serializable;

namespace STFU.Lib.Youtube.Services
{
	public static class YoutubeCategoryService
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(YoutubeCategoryService));

		private static bool _loaded = false;
		private static List<ICategory> _categories = new List<ICategory>();

		public static IReadOnlyList<ICategory> LoadCategories(IYoutubeAccountContainer container)
		{
			if (!_loaded)
			{
                if (container.RegisteredAccounts.Count > 0)
				{
					var account = container.RegisteredAccounts.First();

					var region = account.Region;
					if (account.Region == null)
					{
						region = "de";
					}

					Logger.Info($"Loading categores for account with id: '{account.Id}', title: '{account.Title}' and region: {region}");

					_categories = GetVideoCategories(region, account.GetActiveToken()).ToList();
				}
				else
				{
					Logger.Info($"No accounts registered => using fallback categories");

					// Fallback
					foreach (var cat in StandardCategories.Categories)
					{
						Logger.Info($"Adding category with id: {cat.Id} and title: '{cat.Title}'");

						_categories.Add(cat);
					}
				}

				_loaded = true;
			}
			else
			{
				Logger.Info($"Categories were already loaded");
			}

			return _categories.AsReadOnly();
		}

		public static ICategory[] GetVideoCategories(string regionCode, string accessToken)
		{
			Logger.Info($"Loading video categories from youtube");

			var pageToken = string.Empty;
			CultureInfo ci = CultureInfo.CurrentUICulture;
			string url = string.Format("https://www.googleapis.com/youtube/v3/videoCategories?part=snippet&hl={2}&regionCode={1}&key={0}", YoutubeClientData.YoutubeApiKey, regionCode, ci.Name);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Proxy = null;
			request.Method = "GET";
			request.Credentials = CredentialCache.DefaultCredentials;
			request.ProtocolVersion = HttpVersion.Version11;

			// Header schreiben
			request.Headers.Add(string.Format("Authorization: Bearer {0}", accessToken));

			var result = WebService.Communicate(request);
			QuotaProblemHandler.ThrowOnQuotaLimitReached(result);

			Response response = JsonConvert.DeserializeObject<Response>(result);

			if (response.Items == null)
			{
				response.Items = new Item[0];
				Logger.Error($"Could not load video categories from youtube!");
			}

			var categories = response.Items.Where(i => i.Snippet.Assignable).Select(i => new YoutubeCategory(int.Parse(i.Id), i.Snippet.Title)).ToArray();

			foreach (var cat in categories)
			{
				Logger.Info($"Adding category with id: {cat.Id} and title: {cat.Title}");
			}

			return categories;
		}

		private static class StandardCategories
		{
			public static readonly ICategory[] Categories = new YoutubeCategory[] {
				new YoutubeCategory(1, "Film & Animation"),
				new YoutubeCategory(2, "Autos & Fahrzeuge"),
				new YoutubeCategory(10, "Musik"),
				new YoutubeCategory(15, "Tiere"),
				new YoutubeCategory(17, "Sport"),
				new YoutubeCategory(19, "Reisen & Events"),
				new YoutubeCategory(20, "Gaming"),
				new YoutubeCategory(22, "Menschen & Blogs"),
				new YoutubeCategory(23, "Komödie"),
				new YoutubeCategory(24, "Unterhaltung"),
				new YoutubeCategory(25, "Nachrichten & Politik"),
				new YoutubeCategory(26, "Praktische Tipps & Styling"),
				new YoutubeCategory(27, "Bildung"),
				new YoutubeCategory(28, "Wissenschaft & Technik")
			};
		}
	}
}
