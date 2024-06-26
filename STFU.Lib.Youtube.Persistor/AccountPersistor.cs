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
	public class AccountPersistor
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(AccountPersistor));

		public string Path { get; private set; } = null;
		public IYoutubeAccountContainer Container { get; private set; } = null;
		public IYoutubeAccountContainer Saved { get; private set; } = null;

		private IYoutubeClientContainer Clients { get; set; }

		public AccountPersistor(IYoutubeAccountContainer container, string path, IYoutubeClientContainer clients)
		{
			Logger.Debug($"Creating account persistor for path '{path}'");

			Path = path;
			Container = container;
			Clients = clients;
		}

		public bool Load()
		{
			Logger.Info($"Loading accounts from path '{Path}'");
			Container.UnregisterAllAccounts();

			bool worked = true;

			try
			{
				if (File.Exists(Path))
				{
					using (StreamReader reader = new StreamReader(Path))
					{
						var json = reader.ReadToEnd();
						Logger.Debug($"Json from loaded path: '{json}'");

						var accounts = JsonConvert.DeserializeObject<YoutubeAccount[]>(json);
						Logger.Info($"Loaded {accounts.Length} accounts");

						foreach (var loaded in accounts)
						{
							Logger.Info($"Adding account '{loaded.Title}'");

							foreach (var ac in loaded.Access)
							{
								Logger.Debug($"Setting client to access '{ac.AccessToken}'");
								ac.Client = Clients.RegisteredClients.SingleOrDefault(rc => rc.Id == ac.ClientId);
							}

							for (int i = 0; i < loaded.Access.Count; i++)
							{
								if (loaded.Access[i].Client == null)
								{
									Logger.Debug($"Removing access without client '{loaded.Access[i].AccessToken}'");
									loaded.Access.RemoveAt(i);
									i--;
								}
							}

							Container.RegisterAccount(loaded);
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
				Logger.Error($"Could not load accounts, exception occured!", e);
				worked = false;
			}

			return worked;
		}

		public bool Save()
		{
			IYoutubeAccount[] accounts = Container.RegisteredAccounts.ToArray();
			Logger.Info($"Saving {accounts.Length} accounts to file '{Path}'");

			var json = JsonConvert.SerializeObject(accounts);

			var worked = true;
			try
			{
				using (StreamWriter writer = new StreamWriter(Path, false))
				{
					writer.Write(json);
				}
				Logger.Info($"Accounts saved");

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
				Logger.Error($"Could not save accounts, exception occured!", e);
				worked = false;
			}

			return worked;
		}

		private void RecreateSaved()
		{
			Logger.Debug($"Recreating cache of saved accounts");
			Saved = new YoutubeAccountContainer();
			foreach (var account in Container.RegisteredAccounts)
			{
				Logger.Debug($"Recreating cache for account '{account.Title}'");
				var newAccount = YoutubeAccount.Create(account.Id, account.Title, account.Region);

				Saved.RegisterAccount(newAccount);
			}
		}
	}
}
