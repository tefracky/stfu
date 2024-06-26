﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using log4net;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model;

namespace STFU.Lib.Youtube
{
	public class YoutubeAccountContainer : IYoutubeAccountContainer
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(YoutubeAccountContainer));

		IList<IYoutubeAccount> Accounts { get; } = new List<IYoutubeAccount>();

		public IReadOnlyCollection<IYoutubeAccount> RegisteredAccounts => new ReadOnlyCollection<IYoutubeAccount>(Accounts);

		public void RegisterAccount(IYoutubeAccount account)
		{
			if (!RegisteredAccounts.Any(r => r.Id == account.Id))
			{
				Logger.Debug($"Adding a new account, title: '{account.Title}'");
				Accounts.Add(account);
			}
		}

		public void UnregisterAccount(IYoutubeAccount account)
		{
			if (Accounts.Contains(account))
			{
				Logger.Debug($"Removing account, title: '{account.Title}'");
				Accounts.Remove(account);
			}
		}

		public void UnregisterAccountAt(int index)
		{
			if (Accounts.Count > index)
			{
				Logger.Debug($"Removing account at index {index}, title: '{Accounts[index].Title}'");
				Accounts.RemoveAt(index);
			}
		}

		public void UnregisterAllAccounts()
		{
			Logger.Debug($"Removing all accounts");
			Accounts.Clear();
		}
	}
}
