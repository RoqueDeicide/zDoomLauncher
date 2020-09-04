using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Launcher.Utilities;
using ModernWpf.Controls;
using Ookii.Dialogs.Wpf;
using PathIO = System.IO.Path;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private const int CommandLineMaxLength = 2080;

		private AccentColorPicker accentColorPicker;

		private (string path, string args) CommandLine
		{
			get
			{
				string appPath              = PathIO.Combine(this.zDoomFolder, this.currentExeFile);
				string commandLineArguments = this.Config.GetCommandLine(this.zDoomFolder);

				return (appPath, commandLineArguments);
			}
		}

		public MainWindow()
		{
			this.DataContext = this.Config;

			this.InitializeComponent();

			this.SetupThemeMenuItems();

			this.LoadAppConfiguration();

			this.SelectZDoomInstallationFolder();

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
				this.LoadConfiguration(this.CurrentConfigFile);
			}
			else
			{
				this.CurrentConfigFile = "DefaultConfigFile.xlcf";
			}

			if (this.zDoomFolder != null)
			{
				Iwads.IwadFolder = this.zDoomFolder;
			}

			this.RefreshExeFiles();

			this.ExtraFilesBox.SelectedFiles = this.Config.ExtraFiles;
		}

		private void Launch(string appFileName)
		{
			(string path, string args) = this.CommandLine;

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
					error.Append(this.zDoomFolder);
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

		private bool IsCurrentZDoomFolderValid()
		{
			return this.zDoomFolder != null && Directory.Exists(this.zDoomFolder);
		}

		/// <summary>
		/// Prompts the user to select the directory where zDoom is installed.
		/// </summary>
		/// <param name="reselecting">
		/// A <see cref="bool"/> value that indicates, whether a valid directory was chosen before, and this call
		/// shouldn't result in closing of the window, if no valid directory is chosen right now.
		/// </param>
		public async void SelectZDoomInstallationFolder(bool reselecting = false)
		{
			if (!this.IsCurrentZDoomFolderValid() || reselecting)
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
										 CloseButtonText   = reselecting ? "Nevermind" : "Cancel",
										 PrimaryButtonText = "Choose another"
									 };

					ContentDialogResult result = await messageBox.ShowAsync();

					if (result == ContentDialogResult.None)
					{
						if (reselecting)
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

			this.RefreshExeFiles();
		}

		private void RefreshExeFiles()
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

			foreach (ComboBoxItem comboBoxItem in exeFiles)
			{
				this.ExeFileNameComboBox.Items.Add(comboBoxItem);
			}

			string currentExeFileFullName = PathIO.Combine(this.zDoomFolder, this.currentExeFile ?? "");

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

		private void RefreshExtraFiles()
		{
			if (this.Config?.ExtraFiles == null || this.ExtraFilesBox == null || this.zDoomFolder == null)
			{
				return;
			}

			// This just triggers a refresh of extra files.
			ExtraFilesLookUp.Refresh();
		}

		private void LoadConfiguration(string configFile)
		{
			this.Config.Load(configFile, this.zDoomFolder);

			string doomWadDir = ExtraFilesLookUp.DoomWadDirectory;

			if (!string.IsNullOrWhiteSpace(doomWadDir) && !ExtraFilesLookUp.Directories.Contains(doomWadDir))
			{
				ExtraFilesLookUp.Directories.Add(doomWadDir);
			}

			if (!ExtraFilesLookUp.Directories.Contains(this.zDoomFolder))
			{
				ExtraFilesLookUp.Directories.Add(this.zDoomFolder);
			}

			if (this.Config.IwadFile != null)
			{
				this.IwadComboBox.Select(this.Config.IwadFile.FileName);
			}

			this.CurrentConfigFile = PathIO.ChangeExtension(configFile, ".xlcf");
		}
	}
}