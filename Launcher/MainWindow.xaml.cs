using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Launcher.Configs;
using Launcher.Logging;
using Ookii.Dialogs.Wpf;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;
using PathIO = System.IO.Path;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			this.resetting = true;

			this.LoadAppConfiguration();

			this.SelectZDoomInstallationFolder(this, null);

			if (this.file != null && File.Exists(this.file))
			{
				this.config = LaunchConfiguration.Load(this.file);
			}
			else
			{
				this.config = new LaunchConfiguration();
				this.file = "DefaultConfigFile.lcf";
			}

			InitializeComponent();

			this.RefreshExeFiles(this, null);
			this.InitializeDialogs();
			this.InitializeLoadableFiles();
			this.InitializeContextMenus();
			this.InitializeSomeEventHandlers();
			this.SetupInterface();
		}
		#region Setting Up
		private void SetupInterface()
		{
			this.resetting = true;
			if (this.config == null)
			{
				this.config = new LaunchConfiguration();
			}
			// Set the title of the window.
			this.Title = String.Format("ZDoom Launcher - {0}", this.config.Name);
			// Set the name of the configuration in the text box.
			this.ConfigurationNameTextBox.Text = this.config.Name;
			// Set up the list of IWADs.
			this.SetupIwads();
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
			this.resetting = false;
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
					String.Format("Unable to find {0} in {1}", appFileName, this.zDoomFolder);
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

				while
				(
					!
					(
						dialog.ShowDialog() == true
						&&
						Directory.EnumerateFiles(dialog.SelectedPath, "*.exe").Any()
					)
					&&
					MessageBox.Show
					(
						"Valid folder needs to contain at least one .exe file.",
						"No folder or invalid one was chosen",
						MessageBoxButton.YesNo,
						MessageBoxImage.Question
					) == MessageBoxResult.Yes)
				{
				}
				this.zDoomFolder = dialog.SelectedPath;
			}

			this.RefreshExeFiles(this, null);
		}

		private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.SaveAppConfiguration();
		}

		private void IwadSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBoxItem item = this.IwadComboBox.SelectedItem as ComboBoxItem;
			if (item == null)
			{
				return;
			}

			string selectedIwad = (string)item.Content;
			if (selectedIwad != PathIO.GetFileName(this.config.IwadPath))
			{
				Log.Message("Selected {0}", selectedIwad);
				this.config.IwadPath = PathIO.Combine(this.zDoomFolder, selectedIwad);
			}
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
	}
}