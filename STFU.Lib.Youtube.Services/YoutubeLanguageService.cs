﻿using System.Collections.Generic;
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
	public static class YoutubeLanguageService
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(YoutubeLanguageService));

		private static bool _loaded = false;
		private static List<ILanguage> _languages = new List<ILanguage>();

		public static IReadOnlyList<ILanguage> LoadLanguages(IYoutubeAccountContainer container)
		{
			if (!_loaded)
			{
				if (container.RegisteredAccounts.Count > 0)
				{
					var account = container.RegisteredAccounts.First();

					Logger.Info($"Loading languages for account with id: '{account.Id}', title: '{account.Title}'");

					_languages = GetLanguages().ToList();
				}
				else
				{
					Logger.Info($"No accounts registered => using fallback languages");

					// Fallback
					foreach (var lang in StandardLanguages.Languages)
					{
						Logger.Info($"Adding language with id: {lang.Id}, hl: {lang.Hl} and title: '{lang.Name}'");

						_languages.Add(lang);
					}
				}

				_loaded = true;
			}
			else
			{
				Logger.Info($"Languages were already loaded");
			}

			return _languages.AsReadOnly();
		}

		public static ILanguage[] GetLanguages()
        {
			Logger.Info($"Loading languages from youtube");

			var pageToken = string.Empty;
			CultureInfo ci = CultureInfo.CurrentUICulture;
			string url = string.Format("https://www.googleapis.com/youtube/v3/i18nLanguages?part=snippet&hl={1}&key={0}", YoutubeClientData.YoutubeApiKey, ci.Name);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Proxy = null;
			request.Method = "GET";
			request.Credentials = CredentialCache.DefaultCredentials;
			request.ProtocolVersion = HttpVersion.Version11;

			var result = WebService.Communicate(request);
			QuotaProblemHandler.ThrowOnQuotaLimitReached(result);

			Response response = JsonConvert.DeserializeObject<Response>(result);

			if (response.Items == null)
			{
				response.Items = new Item[0];
				Logger.Error($"Could not load languages from youtube!");
			}

			var languages = response.Items.Select(i =>
            {
                var language = new YoutubeLanguage
                {
                    Id = i.Id,
                    Hl = i.Snippet.Hl,
                    Name = i.Snippet.Name
                };
                return language;
            }).OrderBy(lang => lang.Name).ToArray();

			foreach (var lang in languages)
			{
				Logger.Info($"Adding language with id: {lang.Id}, hl: {lang.Hl} and title: '{lang.Name}'");
			}

			return languages;
		}

		private static class StandardLanguages
		{
			public static readonly ILanguage[] Languages = new YoutubeLanguage[] {
				new YoutubeLanguage()
				{
					Id= "af",
					Hl= "af",
					Name= "Afrikaans"
				},
				new YoutubeLanguage()
				{
					Id= "sq",
					Hl= "sq",
					Name= "Albanisch"
				},
				new YoutubeLanguage()
				{
					Id= "am",
					Hl= "am",
					Name= "Amharisch"
				},
				new YoutubeLanguage()
				{
					Id= "ar",
					Hl= "ar",
					Name= "Arabisch"
				},
				new YoutubeLanguage()
				{
					Id= "hy",
					Hl= "hy",
					Name= "Armenisch"
				},
				new YoutubeLanguage()
				{
					Id= "az",
					Hl= "az",
					Name= "Aserbaidschanisch"
				},
				new YoutubeLanguage()
				{
					Id= "eu",
					Hl= "eu",
					Name= "Baskisch"
				},
				new YoutubeLanguage()
				{
					Id= "bn",
					Hl= "bn",
					Name= "Bengalisch"
				},
				new YoutubeLanguage()
				{
					Id= "my",
					Hl= "my",
					Name= "Birmanisch"
				},
				new YoutubeLanguage()
				{
					Id= "bs",
					Hl= "bs",
					Name= "Bosnisch"
				},
				new YoutubeLanguage()
				{
					Id= "bg",
					Hl= "bg",
					Name= "Bulgarisch"
				},
				new YoutubeLanguage()
				{
					Id= "zh-CN",
					Hl= "zh-CN",
					Name= "Chinesisch"
				},
				new YoutubeLanguage()
				{
					Id= "zh-HK",
					Hl= "zh-HK",
					Name= "Chinesisch (Hongkong)"
				},
				new YoutubeLanguage()
				{
					Id= "zh-TW",
					Hl= "zh-TW",
					Name= "Chinesisch (Taiwan)"
				},
				new YoutubeLanguage()
				{
					Id= "da",
					Hl= "da",
					Name= "Dänisch"
				},
				new YoutubeLanguage()
				{
					Id= "de",
					Hl= "de",
					Name= "Deutsch"
				},
				new YoutubeLanguage()
				{
					Id= "en",
					Hl= "en",
					Name= "Englisch"
				},
				new YoutubeLanguage()
				{
					Id= "en-GB",
					Hl= "en-GB",
					Name= "Englisch (Vereinigtes Königreich)"
				},
				new YoutubeLanguage()
				{
					Id= "et",
					Hl= "et",
					Name= "Estnisch"
				},
				new YoutubeLanguage()
				{
					Id= "fil",
					Hl= "fil",
					Name= "Filipino"
				},
				new YoutubeLanguage()
				{
					Id= "fi",
					Hl= "fi",
					Name= "Finnisch"
				},
				new YoutubeLanguage()
				{
					Id= "fr",
					Hl= "fr",
					Name= "Französisch"
				},
				new YoutubeLanguage()
				{
					Id= "fr-CA",
					Hl= "fr-CA",
					Name= "Französisch (Kanada)"
				},
				new YoutubeLanguage()
				{
					Id= "gl",
					Hl= "gl",
					Name= "Galicisch"
				},
				new YoutubeLanguage()
				{
					Id= "ka",
					Hl= "ka",
					Name= "Georgisch"
				},
				new YoutubeLanguage()
				{
					Id= "el",
					Hl= "el",
					Name= "Griechisch"
				},
				new YoutubeLanguage()
				{
					Id= "gu",
					Hl= "gu",
					Name= "Gujarati"
				},
				new YoutubeLanguage()
				{
					Id= "iw",
					Hl= "iw",
					Name= "Hebräisch"
				},
				new YoutubeLanguage()
				{
					Id= "hi",
					Hl= "hi",
					Name= "Hindi"
				},
				new YoutubeLanguage()
				{
					Id= "id",
					Hl= "id",
					Name= "Indonesisch"
				},
				new YoutubeLanguage()
				{
					Id= "is",
					Hl= "is",
					Name= "Isländisch"
				},
				new YoutubeLanguage()
				{
					Id= "it",
					Hl= "it",
					Name= "Italienisch"
				},
				new YoutubeLanguage()
				{
					Id= "ja",
					Hl= "ja",
					Name= "Japanisch"
				},
				new YoutubeLanguage()
				{
					Id= "kn",
					Hl= "kn",
					Name= "Kannada"
				},
				new YoutubeLanguage()
				{
					Id= "kk",
					Hl= "kk",
					Name= "Kasachisch"
				},
				new YoutubeLanguage()
				{
					Id= "ca",
					Hl= "ca",
					Name= "Katalanisch"
				},
				new YoutubeLanguage()
				{
					Id= "km",
					Hl= "km",
					Name= "Khmer"
				},
				new YoutubeLanguage()
				{
					Id= "ky",
					Hl= "ky",
					Name= "Kirgisisch"
				},
				new YoutubeLanguage()
				{
					Id= "ko",
					Hl= "ko",
					Name= "Koreanisch"
				},
				new YoutubeLanguage()
				{
					Id= "hr",
					Hl= "hr",
					Name= "Kroatisch"
				},
				new YoutubeLanguage()
				{
					Id= "lo",
					Hl= "lo",
					Name= "Laotisch"
				},
				new YoutubeLanguage()
				{
					Id= "lv",
					Hl= "lv",
					Name= "Lettisch"
				},
				new YoutubeLanguage()
				{
					Id= "lt",
					Hl= "lt",
					Name= "Litauisch"
				},
				new YoutubeLanguage()
				{
					Id= "ms",
					Hl= "ms",
					Name= "Malaiisch"
				},
				new YoutubeLanguage()
				{
					Id= "ml",
					Hl= "ml",
					Name= "Malayalam"
				},
				new YoutubeLanguage()
				{
					Id= "mr",
					Hl= "mr",
					Name= "Marathi"
				},
				new YoutubeLanguage()
				{
					Id= "mk",
					Hl= "mk",
					Name= "Mazedonisch"
				},
				new YoutubeLanguage()
				{
					Id= "mn",
					Hl= "mn",
					Name= "Mongolisch"
				},
				new YoutubeLanguage()
				{
					Id= "ne",
					Hl= "ne",
					Name= "Nepalesisch"
				},
				new YoutubeLanguage()
				{
					Id= "nl",
					Hl= "nl",
					Name= "Niederländisch"
				},
				new YoutubeLanguage()
				{
					Id= "no",
					Hl= "no",
					Name= "Norwegisch"
				},
				new YoutubeLanguage()
				{
					Id= "fa",
					Hl= "fa",
					Name= "Persisch"
				},
				new YoutubeLanguage()
				{
					Id= "pl",
					Hl= "pl",
					Name= "Polnisch"
				},
				new YoutubeLanguage()
				{
					Id= "pt",
					Hl= "pt",
					Name= "Portugiesisch (Brasilien)"
				},
				new YoutubeLanguage()
				{
					Id= "pt-PT",
					Hl= "pt-PT",
					Name= "Portugiesisch (Portugal)"
				},
				new YoutubeLanguage()
				{
					Id= "pa",
					Hl= "pa",
					Name= "Punjabi"
				},
				new YoutubeLanguage()
				{
					Id= "ro",
					Hl= "ro",
					Name= "Rumänisch"
				},
				new YoutubeLanguage()
				{
					Id= "ru",
					Hl= "ru",
					Name= "Russisch"
				},
				new YoutubeLanguage()
				{
					Id= "sv",
					Hl= "sv",
					Name= "Schwedisch"
				},
				new YoutubeLanguage()
				{
					Id= "sr",
					Hl= "sr",
					Name= "Serbisch"
				},
				new YoutubeLanguage()
				{
					Id= "sr-Latn",
					Hl= "sr-Latn",
					Name= "Serbisch (Lateinisch)"
				},
				new YoutubeLanguage()
				{
					Id= "si",
					Hl= "si",
					Name= "Singhalesisch"
				},
				new YoutubeLanguage()
				{
					Id= "sk",
					Hl= "sk",
					Name= "Slowakisch"
				},
				new YoutubeLanguage()
				{
					Id= "sl",
					Hl= "sl",
					Name= "Slowenisch"
				},
				new YoutubeLanguage()
				{
					Id= "es-419",
					Hl= "es-419",
					Name= "Spanisch (Lateinamerika)"
				},
				new YoutubeLanguage()
				{
					Id= "es",
					Hl= "es",
					Name= "Spanisch (Spanien)"
				},
				new YoutubeLanguage()
				{
					Id= "es-US",
					Hl= "es-US",
					Name= "Spanisch (Vereinigte Staaten)"
				},
				new YoutubeLanguage()
				{
					Id= "sw",
					Hl= "sw",
					Name= "Suaheli"
				},
				new YoutubeLanguage()
				{
					Id= "ta",
					Hl= "ta",
					Name= "Tamil"
				},
				new YoutubeLanguage()
				{
					Id= "te",
					Hl= "te",
					Name= "Telugu"
				},
				new YoutubeLanguage()
				{
					Id= "th",
					Hl= "th",
					Name= "Thailändisch"
				},
				new YoutubeLanguage()
				{
					Id= "cs",
					Hl= "cs",
					Name= "Tschechisch"
				},
				new YoutubeLanguage()
				{
					Id= "tr",
					Hl= "tr",
					Name= "Türkisch"
				},
				new YoutubeLanguage()
				{
					Id= "uk",
					Hl= "uk",
					Name= "Ukrainisch"
				},
				new YoutubeLanguage()
				{
					Id= "hu",
					Hl= "hu",
					Name= "Ungarisch"
				},
				new YoutubeLanguage()
				{
					Id= "ur",
					Hl= "ur",
					Name= "Urdu"
				},
				new YoutubeLanguage()
				{
					Id= "uz",
					Hl= "uz",
					Name= "Usbekisch"
				},
				new YoutubeLanguage()
				{
					Id= "vi",
					Hl= "vi",
					Name= "Vietnamesisch"
				},
				new YoutubeLanguage()
				{
					Id= "be",
					Hl= "be",
					Name= "Weißrussisch"
				},
				new YoutubeLanguage()
				{
					Id= "zu",
					Hl= "zu",
					Name= "Zulu"
				}
			};
		}
	}
}
