using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Launcher.Configs;
using Launcher.Utilities;
using ModernWpf.Controls;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		public new static App                 Current;
		public            LaunchConfiguration Config { get; } = new LaunchConfiguration();

		private void StartUp(object sender, StartupEventArgs e)
		{
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledExceptionInCurrentDomain;

			// Make sure that the working directory is not screwed up when launching the app from the link.
			string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (exePath == null)
			{
				return;
			}

			Directory.SetCurrentDirectory(exePath);

			Current = this;

			AppSettings.Load();

			try
			{
				Log.Message("Getting command line arguments.");

				var args = Environment.GetCommandLineArgs();

				Log.Message("Checking arguments.");

				if (args.Length > 1 && File.Exists(args[1]))
				{
					Log.Message("File [{0}] from first slot has been selected.", args[1]);

					AppSettings.CurrentConfigFile = args[1];
				}
				else if (args.Length > 2 && args[1] == "-file" && File.Exists(args[2]))
				{
					Log.Message("File [{0}] from second slot has been selected.", args[2]);

					AppSettings.CurrentConfigFile = args[2];
				}

				Log.Message("Done with arguments.");
			}
			catch (NotSupportedException)
			{
			}

			if (AppSettings.CurrentConfigFile != null && File.Exists(AppSettings.CurrentConfigFile))
			{
				this.LoadConfiguration(AppSettings.CurrentConfigFile);
			}
			else
			{
				AppSettings.CurrentConfigFile = "DefaultConfigFile.xlcf";
			}
		}

		public void LoadConfiguration(string configFile)
		{
			this.Config.Load(configFile, AppSettings.ZDoomDirectory);

			string doomWadDir = ExtraFilesLookUp.DoomWadDirectory;

			if (!string.IsNullOrWhiteSpace(doomWadDir))
			{
				ExtraFilesLookUp.AddDirectory(doomWadDir);
			}

			ExtraFilesLookUp.AddDirectory(AppSettings.ZDoomDirectory);

			AppSettings.CurrentConfigFile = Path.ChangeExtension(configFile, ".xlcf");
		}

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);

			AppSettings.Save();
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

				Exception inner = ex.InnerException;
				while (inner != null)
				{
					Log.Error("Caused by:");
					Log.Error(inner.Message);
					Log.Error(inner.StackTrace);

					inner = inner.InnerException;
				}
			}
		}

		private void ReplaceIconWithContent(object sender, RoutedEventArgs e)
		{
			if (sender is AppBarButton abb && abb.Template.FindName("Content", abb) is ContentPresenter cp)
			{
				var binding = new Binding("Content") {Source = abb};
				cp.SetBinding(ContentPresenter.ContentProperty, binding);
			}
		}
	}
}