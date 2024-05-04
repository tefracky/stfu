using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Windows.Forms;
using log4net;
using STFU.Executable.AutoUploader.Forms;

namespace STFU.Executable.AutoUploader
{
	public static class Program
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(Program));

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main(string[] args)
		{
			Logger.Info("Application was started");
			AppDomain.CurrentDomain.FirstChanceException += LogException;

			ClearOldExceptionFiles();

			ServicePointManager.DefaultConnectionLimit = int.MaxValue;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm(args.Any(arg => arg.ToLower() == "showreleasenotes")));

			Logger.Info("Application stopped");
		}

		private static void ClearOldExceptionFiles()
		{
			if (Directory.Exists("errors"))
			{
				Logger.Info($"Clearing old exception logs");
				var subdirectories = Directory.EnumerateDirectories("errors");
				foreach (var subdir in subdirectories)
				{
					Logger.Debug($"Clearing old exception logs in folder '{subdir}'");
					var maxTime = new TimeSpan(14, 0, 0, 0);
					foreach (var file in Directory.EnumerateFiles(subdir))
					{
						if (DateTime.Now - new FileInfo(file).CreationTime > maxTime)
						{
							Logger.Debug($"Deleting file '{file}'");
							File.Delete(file);
						}
					}
				}

				if (new DirectoryInfo("errors").EnumerateFiles("*", SearchOption.AllDirectories).ToArray().Length == 0)
				{
					Logger.Info($"Deleting old errors folder once and for all");

					try
					{
						Directory.Delete("errors", true);
					}
					catch (Exception ex)
					{
						Logger.Error($"Errors folder could not be deleted because of an exception occured", ex);
					}
				}
			}
		}

		private static void LogException(object sender, FirstChanceExceptionEventArgs e)
		{
			if (!IsIncompleteResume(e)
				&& !IsCoreLibException(e)
				&& !IsCancelException(e))
			{
				Logger.Error("An unexpected Exception occured.", e.Exception);
			}
		}

		private static bool IsCancelException(FirstChanceExceptionEventArgs e)
		{
			return (e.Exception is IOException exception && exception.HResult == -2146232800)
				|| (e.Exception is WebException webException && webException.Status == WebExceptionStatus.RequestCanceled);
		}

		private static bool IsCoreLibException(FirstChanceExceptionEventArgs e)
		{
			return e.Exception.Source == "mscorlib";
		}

		private static bool IsIncompleteResume(FirstChanceExceptionEventArgs e)
		{
			return e.Exception is WebException exception
					&& exception.Status == WebExceptionStatus.ProtocolError
					&& (int)(exception.Response as HttpWebResponse).StatusCode == 308;
		}
	}
}
