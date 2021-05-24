using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Launcher.Configs;
using Launcher.Utilities;
using ModernWpf.Controls;
using Ookii.Dialogs.Wpf;
using UiColor = Windows.UI.Color;
using UiColors = Windows.UI.Colors;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for MainPage.xaml
	/// </summary>
	public partial class MainPage
	{
		private const int CommandLineMaxLength = 2080;

		private static (string path, string args) CommandLine
		{
			get
			{
				string appPath   = Path.Combine(AppSettings.ZDoomDirectory, AppSettings.CurrentExeFile);
				string arguments = App.Current.Config.GetCommandLine(AppSettings.ZDoomDirectory);

				return (appPath, arguments);
			}
		}

		// Open and save dialogs.
		private readonly VistaOpenFileDialog openConfigurationDialog
			= new VistaOpenFileDialog
			  {
				  Multiselect      = true,
				  CheckFileExists  = true,
				  InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				  Title            = @"Select launch configuration to load",
				  ValidateNames    = true,
				  Filter           = @"Launch configuration files (*.xlcf)|*.xlcf|All files (*.*)|*.*"
			  };

		private readonly VistaSaveFileDialog saveConfigurationDialog =
			new VistaSaveFileDialog
			{
				DefaultExt       = ".xlcf",
				InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				Filter           = @"Launch configuration files (*.xlcf)|*.xlcf|All files (*.*)|*.*",
				Title            = @"Select where to save the configuration",
				ValidateNames    = true
			};

		private readonly VistaOpenFileDialog openSaveGameFileDialog =
			new VistaOpenFileDialog
			{
				DefaultExt       = ".zds",
				InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				Filter =
					@"ZDoom-compatible save game files (*.zds)|*.zds|All files (*.*)|*.*",
				Title           = @"Select the save game file to load with the game",
				ValidateNames   = true,
				CheckFileExists = true
			};

		private readonly VistaOpenFileDialog openDemoFileDialog =
			new VistaOpenFileDialog
			{
				DefaultExt       = ".lmp",
				InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				Filter           = @"ZDoom-compatible demo files (*.lmp)|*.lmp|All files (*.*)|*.*",
				Title            = @"Select the demo file to play in the game",
				ValidateNames    = true,
				CheckFileExists  = true
			};

		private readonly VistaOpenFileDialog openConfigFileDialog =
			new VistaOpenFileDialog
			{
				DefaultExt       = ".ini",
				InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				Filter           = @"Configuration files (*.ini)|*.ini|All files (*.*)|*.*",
				Title            = @"Select the configuration file to load with the game",
				CheckFileExists  = false
			};

		private readonly VistaFolderBrowserDialog openSaveFolderDialog =
			new VistaFolderBrowserDialog
			{
				Description = @"Select the folder where to store the save game files"
			};

		public MainPage()
		{
			this.DataContext = App.Current.Config;

			this.InitializeComponent();

			this.ExtraFilesBox.SelectedFiles = App.Current.Config.ExtraFiles;

			AppSettings.StaticPropertyChanged += (sender, args) =>
												 {
													 if (args.PropertyName == nameof(AppSettings.CurrentExeFile))
													 {
														 this.ChangeIcon(AppSettings.CurrentExeFile);
													 }
												 };

			this.ChangeIcon(AppSettings.CurrentExeFile);
		}

		/// <summary>
		/// Changes the window icon to the one derived from the exe file.
		/// </summary>
		/// <param name="exeFile">Path to the exe file which icon to use.</param>
		public void ChangeIcon(string exeFile = null)
		{
			Icon icon;
			if (exeFile is null)
			{
				icon = null;
			}
			else
			{
				string fullPath = Path.Combine(AppSettings.ZDoomDirectory, exeFile);
				icon = Icon.ExtractAssociatedIcon(fullPath);
			}

			if (icon == null)
			{
				var uri = new Uri("pack://application:,,,/Launcher;component/Resources/LauncherIcon.ico",
								  UriKind.Absolute);
				this.PlayIconImage.Source = new BitmapImage(uri);
			}
			else
			{
				byte[]    bytes  = new byte[2048];
				using var stream = new MemoryStream(bytes, true);

				icon.Save(stream);

				stream.Seek(0, SeekOrigin.Begin);

				var image = new BitmapImage();

				image.BeginInit();

				image.CacheOption  = BitmapCacheOption.OnLoad;
				image.StreamSource = stream;

				image.EndInit();

				this.PlayIconImage.Source = image;
			}
		}

		private void LaunchGameButtonClick(object sender, RoutedEventArgs e)
		{
			Launch(AppSettings.CurrentExeFile);
		}

		private void SelectSaveGameFile(object sender, RoutedEventArgs e)
		{
			if (!App.Current.Config.SaveGamePath.IsNullOrWhiteSpace())
			{
				this.openSaveGameFileDialog.FileName = App.Current.Config.SaveGamePath;
			}

			if (this.openSaveGameFileDialog.ShowDialog() == true)
			{
				App.Current.Config.SaveGamePath = this.openSaveGameFileDialog.FileName;
			}
		}

		private void SelectDemoFile(object sender, RoutedEventArgs e)
		{
			if (!App.Current.Config.DemoPath.IsNullOrWhiteSpace())
			{
				this.openDemoFileDialog.FileName = App.Current.Config.DemoPath;
			}

			if (this.openDemoFileDialog.ShowDialog() == true)
			{
				App.Current.Config.DemoPath = this.openDemoFileDialog.FileName;
			}
		}

		private void SelectSaveDirectory(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(App.Current.Config.SaveDirectory))
			{
				this.openSaveFolderDialog.SelectedPath = App.Current.Config.SaveDirectory;
			}

			if (this.openSaveFolderDialog.ShowDialog() == true)
			{
				string selectedPath = this.openSaveFolderDialog.SelectedPath;
				string gamePath     = AppSettings.ZDoomDirectory;

				App.Current.Config.SaveDirectory = PathUtils.ToRelativePath(selectedPath, gamePath);
			}
		}

		private void SelectConfigPath(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(App.Current.Config.ConfigFile))
			{
				this.openConfigFileDialog.FileName = App.Current.Config.ConfigFile;
			}

			if (this.openConfigFileDialog.ShowDialog() == true)
			{
				App.Current.Config.ConfigFile = PathUtils.ToRelativePath(this.openConfigFileDialog.FileName,
																		 AppSettings.ZDoomDirectory);
			}
		}

		private void RefreshButtonClick(object sender, RoutedEventArgs e)
		{
			ExeManager.RefreshExeFiles();
			this.RefreshExtraFiles();
		}

		private async void CreateNewConfigurationClick(object sender, RoutedEventArgs e)
		{
			ContentDialogResult choice = await new ContentDialog
											   {
												   Title = "How to Create a New Configuration",
												   Content = "New configuration can be created by clearing the " +
															 "current one or by making a copy.",
												   PrimaryButtonText   = "Clear",
												   SecondaryButtonText = "Copy",
												   CloseButtonText     = "Cancel"
											   }.ShowAsync();

			if (choice != ContentDialogResult.None)
			{
				if (choice == ContentDialogResult.Primary)
				{
					this.saveConfigurationDialog.FileName = "";

					if (this.saveConfigurationDialog.ShowDialog() == true)
					{
						LaunchConfiguration config = App.Current.Config;

						string extraOptions = config.ExtraOptions;
						int    width        = config.Width;
						int    height       = config.Height;

						config.Reset();
						AppSettings.CurrentConfigFile = this.saveConfigurationDialog.FileName;

						config.ExtraOptions = extraOptions;
						config.Width        = width;
						config.Height       = height;
					}
				}
				else
				{
					this.saveConfigurationDialog.FileName = AppSettings.CurrentConfigFile;

					if (this.saveConfigurationDialog.ShowDialog() == true)
					{
						AppSettings.CurrentConfigFile = this.saveConfigurationDialog.FileName;
						App.Current.Config.Save(AppSettings.CurrentConfigFile, AppSettings.ZDoomDirectory);
					}
				}
			}
		}

		private void SaveConfigurationClick(object sender, RoutedEventArgs e)
		{
			App.Current.Config.Save(AppSettings.CurrentConfigFile, AppSettings.ZDoomDirectory);
		}

		private void OpenConfigurationClick(object sender, RoutedEventArgs e)
		{
			if (this.openConfigurationDialog.ShowDialog() == true)
			{
				App.Current.LoadConfiguration(this.openConfigurationDialog.FileName);
			}
		}

		private void UpdateTestCommandLineFlyoutOpening(object sender, object e)
		{
			(string path, string args) = CommandLine;

			string commandLine = $"{path} {args}";

			AppBarButton button = this.CommandLineAppButton;
			button.Resources["CommandLineText"] = commandLine;

			if (commandLine.Length > CommandLineMaxLength)
			{
				button.Resources["FlyoutColor"]  = new SolidColorBrush(Colors.Red);
				button.Resources["FlyoutSymbol"] = Symbol.Cancel;
			}
			else
			{
				button.Resources["FlyoutColor"]  = new SolidColorBrush(Colors.Green);
				button.Resources["FlyoutSymbol"] = Symbol.Accept;
			}
		}

		private static void Launch(string appFileName)
		{
			(string path, string args) = CommandLine;

			bool appExists       = File.Exists(path);
			bool commandLineFits = path.Length + args.Length + 1 <= CommandLineMaxLength;

			if (appExists && commandLineFits)
			{
				Process.Start(path, args);
			}
			else
			{
				bool multipleErrors = !appExists && !commandLineFits;

				var error = new StringBuilder(100);

				error.AppendLine(multipleErrors
									 ? "Unable to launch: multiple errors have been found:"
									 : "Unable to launch: an error has been found:");
				error.AppendLine();

				if (!appExists)
				{
					error.Append("    - Unable to find ");
					error.Append(appFileName);
					error.Append(" in ");
					error.Append(AppSettings.ZDoomDirectory);
					error.AppendLine(".");
				}

				if (!commandLineFits)
				{
					error.AppendLine("    - Length of the command line exceeds allowed number of characters,");
					error.AppendLine("      you might have too many extra parameters or too many files that");
					error.AppendLine("      are not in the same directory as the application.");
				}

				string errorText = error.ToString();
				Log.Error(errorText);
				MessageBox.Show(errorText, "Cannot launch the game", MessageBoxButton.OK,
								MessageBoxImage.Error, MessageBoxResult.OK);
			}
		}

		private void RefreshExtraFiles()
		{
			if (this.ExtraFilesBox == null || AppSettings.ZDoomDirectory.IsNullOrWhiteSpace())
			{
				return;
			}

			// This just triggers a refresh of extra files.
			ExtraFilesLookUp.Refresh();
		}
	}
}