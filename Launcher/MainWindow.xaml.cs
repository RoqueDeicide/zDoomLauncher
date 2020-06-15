using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Launcher.Annotations;
using Launcher.Configs;
using Launcher.Logging;
using ModernWpf.Controls;
using Ookii.Dialogs.Wpf;
using PathIO = System.IO.Path;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	[UsedImplicitly]
	public partial class MainWindow
	{
		private const int CommandLineMaxLength = 2080;

		private AboutWindow aboutWindow;
		private HelpWindow  helpWindow;

		public MainWindow()
		{
			this.settingUpStartUp = false;

			this.InitializeComponent();

			this.SetupThemeMenuItems();

			this.LoadAppConfiguration();

			this.SelectZDoomInstallationFolder(this, null);

			if (!this.IsCurrentZDoomFolderValid())
			{
				return;
			}

			try
			{
				Log.Message("Getting command line arguments.");

				var args = Environment.GetCommandLineArgs();

				Log.Message("Checking arguments.");

				if (args.Length > 1 && File.Exists(args[1]))
				{
					Log.Message("File [{0}] from first slot has been selected.", args[1]);

					this.CurrentConfigFile = args[1];
				}
				else if (args.Length > 2 && args[1] == "-file" && File.Exists(args[2]))
				{
					Log.Message("File [{0}] from second slot has been selected.", args[2]);

					this.CurrentConfigFile = args[2];
				}

				Log.Message("Done with arguments.");
			}
			catch (NotSupportedException)
			{
			}

			if (this.CurrentConfigFile != null && File.Exists(this.CurrentConfigFile))
			{
				this.config = this.LoadConfiguration(this.CurrentConfigFile);
			}
			else
			{
				this.config            = new LaunchConfiguration();
				this.CurrentConfigFile = "DefaultConfigFile.xlcf";
			}

			if (this.zDoomFolder != null)
			{
				Iwads.IwadFolder = this.zDoomFolder;
			}
			this.RefreshExeFiles(this, null);
			this.InitializeDialogs();
			this.InitializeSomeEventHandlers();
			this.SetupInterface();
		}

		#region Setting Up

		private void SetupInterface()
		{
			this.config ??= new LaunchConfiguration();
			// Set the title of the window.
			this.UpdateWindowTitle();
			// Set the name of the configuration in the text box.
			this.ConfigurationNameTextBox.Text = this.config.Name;
			// Set up a list of extra files.
			this.SetupExtraFiles();
			// Save directory.
			this.SaveDirectoryTextBox.Text = this.config.SaveDirectory;
			// Pixel mode.
			this.SetupPixelMode();
			// Custom resolution.
			this.SetupResolution();
			// What to disable?
			this.SetupDisableOptions();
			// What to do when starting the game?
			this.SetupStartUp();
			// Some extras.
			this.ExtraOptionsTextBox.Text = this.config.ExtraOptions;
			// Gameplay.
			this.SetupGamePlay();
		}

		#endregion

		private void LaunchTheGame(object sender, RoutedEventArgs e)
		{
			this.Launch(this.currentExeFile);
		}

		private void Launch(string appFileName)
		{
			var appFile     = PathIO.Combine(this.zDoomFolder, appFileName);
			var commandLine = this.config.GetCommandLine(this.zDoomFolder);

			var appExists       = File.Exists(appFile);
			var commandLineFits = commandLine.Length + appFile.Length <= CommandLineMaxLength;

			if (appExists && commandLineFits)
			{
				Process.Start(appFile, commandLine);
			}
			else
			{
				var multipleErrors = !appExists && !commandLineFits;

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
					error.Append(this.zDoomFolder);
					error.AppendLine(".");
				}

				if (!commandLineFits)
				{
					error.AppendLine("    - Length of the command line exceeds allowed number of characters,");
					error.AppendLine("      you might have too many extra parameters or too many files that");
					error.AppendLine("      are not in the same directory as the application.");
				}

				var errorText = error.ToString();
				Log.Error(errorText);
				MessageBox.Show(errorText, "Cannot launch the game", MessageBoxButton.OK,
								MessageBoxImage.Error, MessageBoxResult.OK);
			}
		}

		private void CreateNewConfiguration(object sender, RoutedEventArgs e)
		{
			this.saveConfigurationDialog.FileName = "";

			if (this.saveConfigurationDialog.ShowDialog(this) == true)
			{
				this.config            = null;
				this.CurrentConfigFile = this.saveConfigurationDialog.FileName;
				this.SetupInterface();
			}
		}

		private void SaveConfiguration(object sender, RoutedEventArgs e)
		{
			this.config.Save(this.CurrentConfigFile, this.zDoomFolder);
		}

		private void SaveConfigurationAs(object sender, RoutedEventArgs e)
		{
			this.saveConfigurationDialog.FileName = this.CurrentConfigFile;

			if (this.saveConfigurationDialog.ShowDialog(this) == true)
			{
				this.CurrentConfigFile = this.saveConfigurationDialog.FileName;
				this.config.Save(this.CurrentConfigFile, this.zDoomFolder);
			}
		}

		private void OpenConfiguration(object sender, RoutedEventArgs e)
		{
			if (this.openConfigurationDialog.ShowDialog(this) == true)
			{
				this.config = this.LoadConfiguration(this.openConfigurationDialog.FileName);
				this.SetupInterface();
			}
		}

		private void CloseWindow(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void ShowCommandLine(object sender, RoutedEventArgs e)
		{
			var appPath = Path.Combine(this.zDoomFolder, this.currentExeFile);

			var commandLineArgs = this.config.GetCommandLine(this.zDoomFolder);

			new CommandLineWindow($"{appPath} {commandLineArgs}").ShowDialog();
		}

		private bool IsCurrentZDoomFolderValid()
		{
			return this.zDoomFolder != null && Directory.Exists(this.zDoomFolder);
		}

		private async void SelectZDoomInstallationFolder(object sender, RoutedEventArgs e)
		{
			if (!this.IsCurrentZDoomFolderValid() || sender is MenuItem)
			{
				var dialog = new VistaFolderBrowserDialog
							 {
								 Description            = @"Select folder with zdoom.exe",
								 UseDescriptionForTitle = true,
								 ShowNewFolderButton    = false
							 };

				while (true)
				{
					if (dialog.ShowDialog() == true && Directory.EnumerateFiles(dialog.SelectedPath, "*.exe").Any())
					{
						break;
					}

					var messageBox = new ContentDialog
									 {
										 Title             = "No folder or invalid one was chosen",
										 Content           = "Valid folder needs to contain at least one .exe file.",
										 CloseButtonText   = "Cancel",
										 PrimaryButtonText = "Choose another"
									 };

					var result = await messageBox.ShowAsync();

					if (result == ContentDialogResult.None)
					{
						if (sender is MenuItem)
						{
							return;
						}

						Log.Warning("No folder was selected. Closing.");

						this.Close();
						return;
					}
				}

				this.zDoomFolder = dialog.SelectedPath;
			}

			if (this.zDoomFolder != null)
			{
				Iwads.IwadFolder = this.zDoomFolder;
			}

			this.RefreshExeFiles(this, null);
		}

		private void OpenDirectoriesWindow(object sender, RoutedEventArgs e)
		{
			var dirs = new Directories();

			dirs.ShowDialog();
		}

		private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.SaveAppConfiguration();
		}

		private void RefreshExeFiles(object sender, RoutedEventArgs e)
		{
			if (this.zDoomFolder == null || this.ExeFileNameComboBox == null)
			{
				return;
			}

			this.ExeFileNameComboBox.Items.Clear();

			var exeFiles = from exeFile in Directory.EnumerateFiles(this.zDoomFolder, "*.exe")
						   select new ComboBoxItem
								  {
									  Content = PathIO.GetFileName(exeFile)
								  };

			foreach (var comboBoxItem in exeFiles)
			{
				this.ExeFileNameComboBox.Items.Add(comboBoxItem);
			}

			var currentExeFileFullName = PathIO.Combine(this.zDoomFolder, this.currentExeFile ?? "");

			if (this.currentExeFile == null || !File.Exists(currentExeFileFullName))
			{
				// Clear the selected item on ExeFileNameComboBox.
				this.ExeFileNameComboBox.SelectedItem = null;
				this.currentExeFile                   = null;
			}

			if (this.currentExeFile != null && File.Exists(currentExeFileFullName))
			{
				// Select the file in the combo box.
				this.ExeFileNameComboBox.SelectedValue = this.currentExeFile;
			}

			if (ExtraFilesLookUp.Directories.Count == 0)
			{
				ExtraFilesLookUp.Directories.Add(this.zDoomFolder);
			}
			else
			{
				ExtraFilesLookUp.Directories[0] = this.zDoomFolder;
			}
		}

		private void ExeFileSelected(object sender, SelectionChangedEventArgs e)
		{
			// Update the field.
			if (this.ExeFileNameComboBox.SelectedItem is ComboBoxItem selectedItem)
			{
				this.currentExeFile = selectedItem.Content as string;
			}
		}

		private void OpenAboutWindow(object sender, RoutedEventArgs e)
		{
			this.aboutWindow ??= new AboutWindow();

			this.aboutWindow.Show();
		}

		private void OpenHelpWindow(object sender, RoutedEventArgs e)
		{
			this.helpWindow ??= new HelpWindow();

			this.helpWindow.Show();
		}

		private void RefreshExtraFiles(object sender, RoutedEventArgs e)
		{
			if (this.config?.ExtraFiles == null || this.ExtraFilesBox == null || this.zDoomFolder == null)
			{
				return;
			}

			// This just triggers a refresh of extra files.
			ExtraFilesLookUp.Refresh();
		}

		private void RefreshEverything(object sender, RoutedEventArgs e)
		{
			this.RefreshExeFiles(this.ExeFilesRefreshButton, e);
			this.RefreshExtraFiles(this.ExtraFilesRefreshButton, e);
		}

		private LaunchConfiguration LoadConfiguration(string configFile)
		{
			var loadedConfig = LaunchConfiguration.Load(configFile, this.zDoomFolder);

			var doomWadDir = ExtraFilesLookUp.DoomWadDirectory;

			if (!string.IsNullOrWhiteSpace(doomWadDir) && !ExtraFilesLookUp.Directories.Contains(doomWadDir))
			{
				ExtraFilesLookUp.Directories.Add(doomWadDir);
			}

			if (!ExtraFilesLookUp.Directories.Contains(this.zDoomFolder))
			{
				ExtraFilesLookUp.Directories.Add(this.zDoomFolder);
			}

			this.IwadComboBox.Select(loadedConfig.IwadPath);

			this.CurrentConfigFile = PathIO.ChangeExtension(configFile, ".xlcf");

			return loadedConfig;
		}
	}
}