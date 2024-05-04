using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using log4net;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model;

namespace STFU.Lib.Youtube
{
	public class YoutubeLanguageContainer : IYoutubeLanguageContainer
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(YoutubeLanguageContainer));

		private IList<ILanguage> Languages { get; } = new List<ILanguage>();

		public IReadOnlyCollection<ILanguage> RegisteredLanguages => new ReadOnlyCollection<ILanguage>(Languages);

		public void RegisterLanguage(ILanguage language)
		{
			if (!RegisteredLanguages.Any(c => c.Id == language.Id))
			{
				Logger.Debug($"Adding a new language, name: '{language.Name}'");
				Languages.Add(language);
			}
		}

		public void UnregisterAllLanguages()
		{
			Logger.Debug($"Removing all languages");
			Languages.Clear();
		}

		public void UnregisterLanguage(ILanguage language)
		{
			if (Languages.Contains(language))
			{
				Logger.Debug($"Removing language, name: '{language.Name}'");
				Languages.Remove(language);
			}
		}

		public void UnregisterLanguageAt(int index)
		{
			if (Languages.Count > index)
			{
				Logger.Debug($"Removing language at index {index}, name: '{Languages[index].Name}'");
				Languages.RemoveAt(index);
			}
		}
	}
}
