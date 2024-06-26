﻿using System.Collections.Generic;
using log4net;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Services;

namespace STFU.Lib.Youtube
{
	public class LanguageCategoryLoader : ILanguageCategoryLoader
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(LanguageCategoryLoader));

		private IYoutubeAccountContainer Container { get; }

		public LanguageCategoryLoader(IYoutubeAccountContainer container)
		{
			Container = container;
		}

		private IReadOnlyList<ICategory> categories = new List<ICategory>().AsReadOnly();

		public IReadOnlyList<ICategory> Categories
		{
			get
			{
				if (categories.Count == 0)
				{
					Logger.Info($"No categories found, loading...");
					categories = YoutubeCategoryService.LoadCategories(Container);
					Logger.Info($"Loaded {categories.Count} categories");
				}

				return categories;
			}
		}

		private IReadOnlyList<ILanguage> languages = new List<ILanguage>().AsReadOnly();

		public IReadOnlyList<ILanguage> Languages
		{
			get
			{
				if (languages.Count == 0)
				{
					// TODO: Auf Festplatte speichern und auch schon vorher laden als beim Template öffnen.
					Logger.Info($"No languages found, loading...");
					languages = YoutubeLanguageService.LoadLanguages(Container);
					Logger.Info($"Loaded {languages.Count} languages");
				}

				return languages;
			}
		}
	}
}
