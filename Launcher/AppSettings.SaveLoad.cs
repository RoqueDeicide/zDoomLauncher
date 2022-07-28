using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Xml;
using Launcher.Databases;
using Launcher.Utilities;
using ModernWpf;

namespace Launcher
{
	public partial class AppSettings
	{
		// Name of the file that contains the application configuration.
		private const string AppConfigurationFileName = "Zdl.config";

		// Extension to use for application configuration file that contains the data in Xml format.
		private const string AppConfigurationXmlExtension = "config";

		// Extension to use for application configuration file that contains the data in binary format.
		private const string AppConfigurationBinaryExtension = "binaryConfig";

		// Name of the application configuration entry that contains the path to the folder were Doom source port files are located.
		private const string GameFolderEntryName = "zDoomInstallationFolder";

		// Name of the application configuration entry that contains the full name of the file that contains last used launch configuration.
		private const string LastConfigurationFileEntryName = "LastLaunchConfigurationFile";

		// Name of the application configuration entry that contains the name of the last used executable file.
		private const string LastExeFileEntryName = "LastExeFile";

		// Name of the entry that contains sub-entries that are paths to directories to for loadable files in.
		private const string LoadableFilesDirectoriesEntryName = "LoadableFilesDirectories";

		// Name of the entry that contains a path to the directory to for loadable files in.
		private const string LoadableFilesDirectoryEntryName = "LoadableFilesDirectory";

		// Name of the entry that contains custom position of the main window.
		private const string MainWindowPositionEntryName = "MainWindowPosition";

		// Name of the entry that contains custom dimensions of the main window.
		private const string MainWindowSizeEntryName = "MainWindowSize";

		// Name of the entry that indicates that the main window is maximized.
		private const string MainWindowMaximizedEntryName = "MainWindowMaximized";

		// Name of the entry that indicates preferred theme.
		private const string PreferredThemeEntryName = "PreferredTheme";

		// Name of the entry that indicates preferred accent color.
		private const string PreferredAccentColorEntryName = "PreferredAccentColor";

		/// <summary>
		/// Saves this application's current settings to the file.
		/// </summary>
		public static void Save()
		{
			var appConfigurationDatabase = new Database(AppConfigurationXmlExtension,
														AppConfigurationBinaryExtension);

			DatabaseEntryContent content = ToEntryContent(!currentConfigFile.IsNullOrWhiteSpace() &&
														  File.Exists(currentConfigFile),
														  currentConfigFile);
			var entry = new DatabaseEntry(LastConfigurationFileEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			content = ToEntryContent(!zDoomDirectory.IsNullOrWhiteSpace(), zDoomDirectory);
			entry   = new DatabaseEntry(GameFolderEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			content = ToEntryContent(!currentExeFile.IsNullOrWhiteSpace(), currentExeFile);
			entry   = new DatabaseEntry(LastExeFileEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			content = new VectorContent<double>(3)
					  {
						  X = startingX,
						  Y = startingY,
						  Z = startAtPosition ? 1 : double.NaN
					  };
			entry = new DatabaseEntry(MainWindowPositionEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			content = new VectorContent<double>(3)
					  {
						  X = startingWidth,
						  Y = startingHeight,
						  Z = startWithSize ? 1 : double.NaN
					  };
			entry = new DatabaseEntry(MainWindowSizeEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			if (startMaximized)
			{
				appConfigurationDatabase.AddEntry(new DatabaseEntry(MainWindowMaximizedEntryName, null));
			}

			// Save preferred theme and accent color.
			if (ThemeManager.Current.ApplicationTheme != null)
			{
				content = new IntegerContent((long)ThemeManager.Current.ApplicationTheme.Value);
				entry   = new DatabaseEntry(PreferredThemeEntryName, content);
				appConfigurationDatabase.AddEntry(entry);
			}

			if (ThemeManager.Current.AccentColor != null)
			{
				Color c = ThemeManager.Current.AccentColor.Value;
				content = new VectorContent<byte>(4)
						  {
							  R = c.R,
							  G = c.G,
							  B = c.B,
							  A = c.A
						  };
				entry = new DatabaseEntry(PreferredAccentColorEntryName, content);
				appConfigurationDatabase.AddEntry(entry);
			}

			// Save the directories.
			var loadableFilesDirectoriesEntry = new DatabaseEntry(LoadableFilesDirectoriesEntryName, null);

			string baseDir = AppDomain.CurrentDomain.BaseDirectory;
			var dirs = new List<string>(from directory in ExtraFilesLookUp.Directories
										select PathUtils.ToRelativePath(directory, baseDir));
			dirs.Sort();

			int counter = 0;
			foreach (string dir in dirs)
			{
				content = new TextContent(dir);
				entry   = new DatabaseEntry(LoadableFilesDirectoryEntryName, content);

				loadableFilesDirectoriesEntry.SubEntries.Add($"{LoadableFilesDirectoryEntryName}{counter++}", entry);
			}

			appConfigurationDatabase.AddEntry(loadableFilesDirectoriesEntry);

			// Save the config file.
			appConfigurationDatabase.Save(AppConfigurationFileName);
		}

		private static TextContent ToEntryContent(bool condition, string text)
		{
			return new TextContent(condition ? text : "Nothing");
		}

		/// <summary>
		/// Loads this application's current settings from the file.
		/// </summary>
		public static void Load()
		{
			try
			{
				if (!File.Exists(AppConfigurationFileName))
				{
					Log.Message("No application configuration file was found.");
					return;
				}

				Log.Message("Loading configuration file.");

				var configurationDatabase = new Database(AppConfigurationXmlExtension, AppConfigurationBinaryExtension);
				configurationDatabase.Load(AppConfigurationFileName);

				CurrentConfigFile = TryGetEntryText(configurationDatabase, LastConfigurationFileEntryName);
				ZDoomDirectory    = TryGetEntryText(configurationDatabase, GameFolderEntryName);
				CurrentExeFile    = TryGetEntryText(configurationDatabase, LastExeFileEntryName);

				// Load custom size and position of the main window, if available.
				if (configurationDatabase.Contains(MainWindowPositionEntryName))
				{
					var doubles = configurationDatabase[MainWindowPositionEntryName].GetContent<VectorContent<double>>();

					if (doubles.Count > 2)
					{
						StartAtPosition = !double.IsNaN(doubles.Z);
					}

					StartingX = Convert.ToInt32(doubles.X);
					StartingY = Convert.ToInt32(doubles.Y);
				}

				if (configurationDatabase.Contains(MainWindowSizeEntryName))
				{
					var doubles = configurationDatabase[MainWindowSizeEntryName].GetContent<VectorContent<double>>();

					if (doubles.Count > 2)
					{
						StartWithSize = !double.IsNaN(doubles.Z);
					}

					StartingWidth  = Convert.ToInt32(doubles.X);
					StartingHeight = Convert.ToInt32(doubles.Y);
				}

				StartMaximized = configurationDatabase.Contains(MainWindowMaximizedEntryName);

				// Try getting information about the theme and accent colors.
				if (configurationDatabase.Contains(PreferredThemeEntryName))
				{
					ThemeManager.Current.ApplicationTheme = (ApplicationTheme)configurationDatabase[PreferredThemeEntryName].GetContent<IntegerContent>().Value;
				}

				if (configurationDatabase.Contains(PreferredAccentColorEntryName))
				{
					var bytes = configurationDatabase[PreferredAccentColorEntryName].GetContent<VectorContent<byte>>();

					ThemeManager.Current.AccentColor = new Color { R = bytes.R, G = bytes.G, B = bytes.B, A = bytes.A };
				}

				if (configurationDatabase.Contains(LoadableFilesDirectoriesEntryName, false))
				{
					DatabaseEntry loadableFilesDirectoriesEntry = configurationDatabase[LoadableFilesDirectoriesEntryName];
					string        baseDir                       = AppDomain.CurrentDomain.BaseDirectory;

					foreach (string dir in from subEntry in loadableFilesDirectoriesEntry.SubEntries
										   let content = subEntry.Value.GetContent<TextContent>()
										   where content != null
										   select PathUtils.GetLocalPath(Path.Combine(baseDir, content.Text)))
					{
						ExtraFilesLookUp.AddDirectory(dir);
					}
				}
			}
			catch (XmlException)
			{
				Log.Warning("Unable to load application configuration file. Ignoring.");
			}
		}

		private static string TryGetEntryText(Database appConfigurationDatabase, string entryName)
		{
			if (appConfigurationDatabase.Contains(entryName, false))
			{
				string entryText = appConfigurationDatabase[entryName].GetContent<TextContent>().Text;
				return entryText == "Nothing" ? null : entryText;
			}

			return null;
		}
	}
}