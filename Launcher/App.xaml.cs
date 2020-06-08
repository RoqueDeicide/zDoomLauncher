using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Launcher.Logging;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private void StartUp(object sender, StartupEventArgs e)
		{
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledExceptionInCurrentDomain;

			// Make sure that the working directory is not screwed up when launching the app from the link.
			var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (exePath == null)
			{
				return;
			}

			Directory.SetCurrentDirectory(exePath);
		}

		private static void OnUnhandledExceptionInCurrentDomain(object o, UnhandledExceptionEventArgs args)
		{
			Log.Error("An error has occurred:");

			if (!(args.ExceptionObject is Exception ex))
			{
				Log.Error("Unknown format of the error: additional information cannot be provided.");
			}
			else
			{
				Log.Error(ex.Message);
				Log.Error(ex.StackTrace);

				var inner = ex.InnerException;
				while (inner != null)
				{
					Log.Error("Caused by:");
					Log.Error(inner.Message);
					Log.Error(inner.StackTrace);

					inner = inner.InnerException;
				}
			}
		}
	}
}