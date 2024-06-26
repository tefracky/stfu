﻿using System;
using System.IO;
using System.Linq;
using log4net;
using Newtonsoft.Json;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Model;

namespace STFU.Lib.Youtube.Persistor
{
	public class CategoryPersistor
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(CategoryPersistor));

		public string Path { get; private set; } = null;
		public IYoutubeCategoryContainer Container { get; private set; } = null;
		public IYoutubeCategoryContainer Saved { get; private set; } = null;

		public CategoryPersistor(IYoutubeCategoryContainer container, string path)
		{
			Logger.Debug($"Creating category persistor for path '{path}'");

			Path = path;
			Container = container;
		}

		public bool Load()
		{
			Logger.Info($"Loading categories from path '{Path}'");
			Container.UnregisterAllCategories();

			bool worked = true;

			try
			{
				if (File.Exists(Path))
				{
					using (StreamReader reader = new StreamReader(Path))
					{
						var json = reader.ReadToEnd();
						Logger.Debug($"Json from loaded path: '{json}'");

						var categories = JsonConvert.DeserializeObject<YoutubeCategory[]>(json);
						Logger.Info($"Loaded {categories.Length} categories");

						foreach (var loaded in categories)
						{
							Logger.Info($"Adding category '{loaded.Title}'");
							Container.RegisterCategory(loaded);
						}
					}
				}

				RecreateSaved();
			}
			catch (Exception e)
			when (e is UnauthorizedAccessException
			|| e is ArgumentException
			|| e is ArgumentNullException
			|| e is DirectoryNotFoundException
			|| e is PathTooLongException
			|| e is IOException)
			{
				Logger.Error($"Could not load categories, exception occured!", e);
				worked = false;
			}

			return worked;
		}

		public bool Save()
		{
			ICategory[] categories = Container.RegisteredCategories.ToArray();
			Logger.Info($"Saving {categories.Length} categories to file '{Path}'");

			var json = JsonConvert.SerializeObject(categories);

			var worked = true;
			try
			{
				using (StreamWriter writer = new StreamWriter(Path, false))
				{
					writer.Write(json);
				}
				Logger.Info($"Categories saved");

				RecreateSaved();
			}
			catch (Exception e)
			when (e is UnauthorizedAccessException
			|| e is ArgumentException
			|| e is ArgumentNullException
			|| e is DirectoryNotFoundException
			|| e is PathTooLongException
			|| e is IOException)
			{
				Logger.Error($"Could not save categories, exception occured!", e);
				worked = false;
			}

			return worked;
		}

		private void RecreateSaved()
		{
			Logger.Debug($"Recreating cache of saved categories");
			Saved = new YoutubeCategoryContainer();
			foreach (var category in Container.RegisteredCategories)
			{
				Logger.Debug($"Recreating cache for path '{category.Title}'");
				var newCategory = new YoutubeCategory(category.Id, category.Title);

				Saved.RegisterCategory(newCategory);
			}
		}
	}
}
