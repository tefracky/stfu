﻿using System;
using System.IO;
using System.Net;
using System.Text;
using log4net;

namespace STFU.Lib.Youtube.Services
{
	public static class WebService
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(WebService));

		public static string Communicate(WebRequest request, out WebException exception, byte[] bytes = null, string headerName = null)
		{
			string result = null;
			exception = null;

			Logger.Info($"Sending a request to '{request.Method} {request.RequestUri}'");

			if (bytes != null && bytes.Length != 0)
			{
				Logger.Info($"Sending bytes: '{Encoding.UTF8.GetString(bytes)}'");

				// Senden
				var requestStream = request.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
			}

			try
			{
				// Verbinden
				var response = request.GetResponse();
				var responseStream = response.GetResponseStream();

				if (string.IsNullOrWhiteSpace(headerName))
				{
					Logger.Info($"Reading response...");

					// Lesen
					StreamReader responseReader = new StreamReader(responseStream);
					result = responseReader.ReadToEnd();
				}
				else
				{
					Logger.Info($"Reading header '{headerName}'...");

					// Wert eines Headers (bisher nur Url zum Upload) erhalten
					result = response.Headers.Get(headerName);
				}
			}
			catch (WebException ex)
			{
				exception = ex;

				if (ex.Status == WebExceptionStatus.ProtocolError)
				{
					var response = ex.Response as HttpWebResponse;
					if ((int)response.StatusCode == 308)
					{
						var range = response.Headers.Get("range");
						Logger.Info($"Returning bytes to continue upload: '{range}'");
						result = range;
					}
				}
				else
				{
					Logger.Error("A web exception occured and we did not expect it. D:", ex);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("An exception occured and we did not expect it. D:", ex);
			}

			Logger.Info($"Response: '{result}'");
			return result;
		}

		public static string Communicate(WebRequest request, byte[] bytes = null, string headerName = null)
		{
            string result = Communicate(request, out var ex, bytes, headerName);

			if (ex != null)
			{
				if (ex.Response != null)
				{
					using (var stream = ex.Response.GetResponseStream())
					{
						using (var reader = new StreamReader(stream))
						{
							result = reader.ReadToEnd();
							Logger.Warn($"Returning Exception response: '{result}'");
						}
					}
				}
				else
				{
					Logger.Warn($"Exception had no response, returning null!");
					result = null;
				}
			}

			return result;
		}
	}
}
