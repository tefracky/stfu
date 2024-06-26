﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using log4net;
using STFU.Lib.Youtube.Interfaces;
using STFU.Lib.Youtube.Interfaces.Model;

namespace STFU.Lib.Youtube
{
	public class YoutubeClientContainer : IYoutubeClientContainer
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(YoutubeClientContainer));

		private IList<IYoutubeClient> Clients { get; } = new List<IYoutubeClient>();

		public IReadOnlyCollection<IYoutubeClient> RegisteredClients => new ReadOnlyCollection<IYoutubeClient>(Clients);

		public void RegisterClient(IYoutubeClient client)
		{
			if (!RegisteredClients.Any(c => c.Id == client.Id))
			{
				Logger.Info($"Adding youtube client credentials with name '{client.Name}'");
				Clients.Add(client);
			}
		}

		public void UnregisterAllClients()
		{
			Logger.Info("Removing all youtube client credentials");
			Clients.Clear();
		}

		public void UnregisterClient(IYoutubeClient client)
		{
			if (Clients.Contains(client))
			{
				Logger.Info($"Removing youtube client credential with name '{client.Name}'");
				Clients.Remove(client);
			}
		}

		public void UnregisterClientAt(int index)
		{
			if (Clients.Count > index)
			{
				Logger.Info($"Removing youtube client credential with name '{Clients[index].Name}' at index {index}");
				Clients.RemoveAt(index);
			}
		}
	}
}
