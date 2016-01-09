using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using Launcher.Configs;
using Launcher.Logging;
using Ookii.Dialogs.Wpf;
using PathIO = System.IO.Path;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private AboutWindow aboutWindow;
		private HelpWindow helpWindow;
		/// <exception cref="FileNotFoundException">
		/// The file cannot be found, such as when mode is FileMode.Truncate or FileMode.Open, and the file
		/// specified by path does not exist. The file must already exist in these modes.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error, such as specifying FileMode.CreateNew when the file specified by path already
		/// exists, occurred. -or-The system is running Windows 98 or Windows 98 Second Edition and share
		/// is set to FileShare.Delete.-or-The stream has been closed.
		/// </exception>
		/// <exception cref="SecurityException">
		/// The caller does not have the required permission.
		/// </exception>
		/// <exception cref="DirectoryNotFoundException">
		/// The specified path is invalid, such as being on an unmapped drive.
		/// </exception>
		/// <exception cref="UnauthorizedAccessException">
		/// The access requested is not permitted by the operating system for the specified path, such as
		/// when access is Write or ReadWrite and the file or directory is set for read-only access.
		/// </exception>
		/// <exception cref="SerializationException">
		/// The serializationStream supports seeking, but its length is 0. -or-The target type is a
		/// <see cref="T:System.Decimal"/>, but the value is out of range of the
		/// <see cref="T:System.Decimal"/> type.
		/// </exception>
		public MainWindow()
		{
			this.LoadAppConfiguration();

			this.SelectZDoomInstallationFolder(this, null);

			if (this.zDoomFolder == null)
			{
				return;
			}

			try
			{
				Log.Message("Getting command line arguments.");

				string[] args = Environment.GetCommandLineArgs();

				Log.Message("Checking arguments.");

				if (args.Length > 1 && File.Exists(args[1]))
				{
					Log.Message("File [{0}] from first slot has been selected.", args[1]);

					this.file = args[1];
				}
				else if (args.Length > 2 && args[1] == "-file" && File.Exists(args[2]))
				{
					Log.Message("File [{0}] from second slot has been selected.", args[2]);

					this.file = args[2];
				}

				Log.Message("Done with arguments.");
			}
			catch (NotSupportedException)
			{
			}

			if (this.file != null && File.Exists(this.file))
			{
				this.config = LaunchConfiguration.Load(this.file);
			}
			else
			{
				this.config = new LaunchConfiguration();
				this.file = "DefaultConfigFile.lcf";
			}

			this.InitializeComponent();

			this.RefreshExeFiles(this, null);
			this.RefreshIwads();
			this.InitializeDialogs();
			this.InitializeContextMenus();
			this.InitializeSomeEventHandlers();
			this.SetupInterface();
		}
		#region Setting Up
		private void SetupInterface()
		{
			if (this.config == null)
			{
				this.config = new LaunchConfiguration();
			}
			// Set the title of the window.
			this.Title = string.Format("ZDoom Launcher - {0}", this.config.Name);
			// Set the name of the configuration in the text box.
			this.ConfigurationNameTextBox.Text = this.config.Name;
			// Set up the list of IWADs.
			this.RefreshIwads();
			// Ignore block map?
			this.IgnoreBlockMapIndicator.IsChecked = this.config.IgnoreBlockMap;
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
			string appFile = PathIO.Combine(this.zDoomFolder, appFileName);
			if (File.Exists(appFile))
			{
				Process.Start(appFile, this.config.CommandLine);
			}
			else
			{
				string errorText =
					string.Format("Unable to find {0} in {1}", appFileName, this.zDoomFolder);
				Log.Error(errorText);
				MessageBox.Show(errorText, "Cannot launch the game", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
			}
		}
		private void CreateNewConfiguration(object sender, RoutedEventArgs e)
		{
			if (this.saveConfigurationDialog.ShowDialog(this) == true)
			{
				this.config = null;
				this.file = this.saveConfigurationDialog.FileName;
				this.SetupInterface();
			}
		}
		private void SaveConfiguration(object sender, RoutedEventArgs e)
		{
			this.config.Save(this.file);
		}
		private void OpenConfiguration(object sender, RoutedEventArgs e)
		{
			if (this.openConfigurationDialog.ShowDialog(this) == true)
			{
				this.config = LaunchConfiguration.Load(this.openConfigurationDialog.FileName);
				this.file = this.openConfigurationDialog.FileName;
				this.SetupInterface();
			}
		}
		private void CloseWindow(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void ShowCommandLine(object sender, RoutedEventArgs e)
		{
			new CommandLineWindow(this.config.CommandLine).Show();
		}

		private void SelectZDoomInstallationFolder(object sender, RoutedEventArgs e)
		{
			if (this.zDoomFolder == null)
			{
				VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog
				{
					Description = @"Select folder with zdoom.exe",
					UseDescriptionForTitle = true,
					ShowNewFolderButton = false
				};

				while (true)
				{
					if (dialog.ShowDialog() == true && Directory.EnumerateFiles(dialog.SelectedPath, "*.exe").Any())
					{
						break;
					}

					if (MessageBox.Show("Valid folder needs to contain at least one .exe file.",
									   "No folder or invalid one was chosen",
									   MessageBoxButton.YesNo,
									   MessageBoxImage.Question) != MessageBoxResult.Yes)
					{
						Log.Warning("No folder was selected. Closing.");

						this.Close();
						return;
					}
				}

				this.zDoomFolder = dialog.SelectedPath;
			}

			this.RefreshExeFiles(this, null);
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

			string currentExeFileFullName = PathIO.Combine(this.zDoomFolder, this.currentExeFile ?? "");

			if (this.currentExeFile == null || !File.Exists(currentExeFileFullName))
			{
				// Clear the selected item on ExeFileNameComboBox.
				this.ExeFileNameComboBox.SelectedItem = null;
				this.currentExeFile = null;
			}
			if (this.currentExeFile != null && File.Exists(currentExeFileFullName))
			{
				// Select the file in the combo box.
				this.ExeFileNameComboBox.SelectedValue = this.currentExeFile;
			}
		}

		private void ExeFileSelected(object sender, SelectionChangedEventArgs e)
		{
			// Update the field.
			ComboBoxItem selectedItem = this.ExeFileNameComboBox.SelectedItem as ComboBoxItem;
			if (selectedItem != null)
			{
				this.currentExeFile = selectedItem.Content as string;
			}
		}

		private void OpenAboutWindow(object sender, RoutedEventArgs e)
		{
			if (this.aboutWindow == null)
			{
				this.aboutWindow = new AboutWindow();
			}

			this.aboutWindow.Show();
		}

		private void OpenHelpWindow(object sender, RoutedEventArgs e)
		{
			if (this.helpWindow == null)
			{
				this.helpWindow = new HelpWindow();
			}

			this.helpWindow.Show();
		}

		private void RefreshExtraFiles(object sender, RoutedEventArgs e)
		{
			if (this.config == null || this.config.ExtraFiles == null ||
				this.ExtraFilesBox == null || this.zDoomFolder == null)
			{
				return;
			}

			// This just triggers a refresh of extra files.
			this.ExtraFilesBox.GameFolder = this.zDoomFolder;
			this.ExtraFilesBox.SelectedFiles = this.config.ExtraFiles;
		}

		private void RefreshIwadFiles(object sender, RoutedEventArgs e)
		{
			this.RefreshIwads();
		}
		private void RefreshEverything(object sender, RoutedEventArgs e)
		{
			this.RefreshExeFiles(this.ExeFilesRefreshButton, e);
			this.RefreshExtraFiles(this.ExtraFilesRefreshButton, e);
			this.RefreshIwadFiles(this.IwadRefreshButton, e);
		}
	}
}