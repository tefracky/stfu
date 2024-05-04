using System;
using log4net;
using Newtonsoft.Json;

namespace STFU.Lib.Youtube.Services
{
	public class QuotaProblemHandler
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(QuotaProblemHandler));

		public static bool IsQuotaLimitReached(string response)
		{
			if (string.IsNullOrWhiteSpace(response))
			{
				return false;
			}
			if (!response.Trim().StartsWith("{"))
			{
				return false;
			}

			try
			{
				var castedError = JsonConvert.DeserializeObject<QuotaErrorResponse>(response);

				var isQuotaReached = castedError?.Error?.Code == 403
					&& castedError?.Error?.Errors?[0] != null
					&& castedError?.Error?.Errors?[0].Domain == "youtube.quota"
					&& castedError?.Error?.Errors?[0].Reason == "quotaExceeded";

				if (isQuotaReached)
				{
					Logger.Error($"YOUTUBE QUOTA FOR THIS APPLICATION WAS REACHED! RESPONSE: '{response}'");
				}

				return isQuotaReached;
			}
			catch (Exception)
			{
				// Lies sich nicht in einen Quota-Fehler parsen => Ist keiner
				return false;
			}
		}

		public static void ThrowOnQuotaLimitReached(string response)
		{
			if (IsQuotaLimitReached(response))
			{
				Logger.Error($"THROWING AN EXCEPTION!");
				throw new QuotaErrorException();
			}
		}
	}
}
