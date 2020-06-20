// App configuration.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using Launcher.Databases;
using Launcher.Logging;
using ModernWpf;

namespace Launcher
{
	public partial class MainWindow
	{
		private void SaveAppConfiguration()
		{
			var appConfigurationDatabase = new Database(AppConfigurationXmlExtension,
														AppConfigurationBinaryExtension);

			DatabaseEntryContent content = ToEntryContent(!string.IsNullOrWhiteSpace(this.CurrentConfigFile) &&
														  File.Exists(this.CurrentConfigFile),
														  this.CurrentConfigFile);
			var entry = new DatabaseEntry(LastConfigurationFileEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			content = ToEntryContent(!string.IsNullOrWhiteSpace(this.zDoomFolder), this.zDoomFolder);
			entry   = new DatabaseEntry(GameFolderEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			content = ToEntryContent(!string.IsNullOrWhiteSpace(this.currentExeFile), this.currentExeFile);
			entry   = new DatabaseEntry(LastExeFileEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			// Save window position and size if redefined.
			if (this.windowPosition != null)
			{
				content = new VectorContent(2)
						  {
							  X = Convert.ToDecimal(this.windowPosition.Value.X),
							  Y = Convert.ToDecimal(this.windowPosition.Value.Y)
						  };
				entry = new DatabaseEntry(MainWindowPositionEntryName, content);
				appConfigurationDatabase.AddEntry(entry);
			}

			if (this.windowSize != null)
			{
				content = new VectorContent(2)
						  {
							  X = Convert.ToDecimal(this.windowSize.Value.Width),
							  Y = Convert.ToDecimal(this.windowSize.Value.Height)
						  };
				entry = new DatabaseEntry(MainWindowSizeEntryName, content);
				appConfigurationDatabase.AddEntry(entry);
			}

			if (this.maximized)
			{
				appConfigurationDatabase.AddEntry(new DatabaseEntry(MainWindowMaximizedEntryName, null));
			}

			// Save preferred theme and accent color.
			if (ThemeManager.Current.ApplicationTheme != null)
			{
				content = new IntegerContent((long) ThemeManager.Current.ApplicationTheme.Value);
				entry   = new DatabaseEntry(PreferredThemeEntryName, content);
				appConfigurationDatabase.AddEntry(entry);
			}

			if (ThemeManager.Current.AccentColor != null)
			{
				Color c = ThemeManager.Current.AccentColor.Value;
				content = new VectorContent(4)
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

				loadableFilesDirectoriesEntry.SubEntries.Add($"{LoadableFilesDirectoryEntryName}{counter++}",
															 entry);
			}

			appConfigurationDatabase.AddEntry(loadableFilesDirectoriesEntry);

			// Save the config file.
			appConfigurationDatabase.Save(AppConfigurationFileName);
		}

		private void LoadAppConfiguration()
		{
			try
			{
				if (!File.Exists(AppConfigurationFileName))
				{
					Log.Message("No application configuration file was found.");
					return;
				}

				Log.Message("Loading configuration file.");

				var appConfigurationDatabase = new Database(AppConfigurationXmlExtension,
															AppConfigurationBinaryExtension);
				appConfigurationDatabase.Load(AppConfigurationFileName);

				this.CurrentConfigFile = TryGetEntryText(appConfigurationDatabase, LastConfigurationFileEntryName);
				this.zDoomFolder       = TryGetEntryText(appConfigurationDatabase, GameFolderEntryName);
				this.currentExeFile    = TryGetEntryText(appConfigurationDatabase, LastExeFileEntryName);

				// Load custom size and position of the main window, if available.
				VectorContent numbers = TryGetVector(appConfigurationDatabase, MainWindowPositionEntryName);
				if (numbers != null)
				{
					this.windowPosition = new Point(Convert.ToDouble(numbers.X), Convert.ToDouble(numbers.Y));
				}

				numbers = TryGetVector(appConfigurationDatabase, MainWindowSizeEntryName);
				if (numbers != null)
				{
					this.windowSize = new Size(Convert.ToDouble(numbers.X), Convert.ToDouble(numbers.Y));
				}

				this.maximized = appConfigurationDatabase.Contains(MainWindowMaximizedEntryName);

				// Try getting information about the theme and accent colors.
				if (appConfigurationDatabase.Contains(PreferredThemeEntryName))
				{
					ThemeManager.Current.ApplicationTheme =
						(ApplicationTheme) appConfigurationDatabase[PreferredThemeEntryName]
										  .GetContent<IntegerContent>()
										  .Value;
				}

				VectorContent accentColors = TryGetVector(appConfigurationDatabase, PreferredAccentColorEntryName);
				if (accentColors != null)
				{
					ThemeManager.Current.AccentColor = new Color
													   {
														   R = Convert.ToByte(accentColors.R),
														   G = Convert.ToByte(accentColors.G),
														   B = Convert.ToByte(accentColors.B),
														   A = Convert.ToByte(accentColors.A)
													   };
				}

				if (appConfigurationDatabase.Contains(LoadableFilesDirectoriesEntryName, false))
				{
					DatabaseEntry loadableFilesDirectoriesEntry =
						appConfigurationDatabase[LoadableFilesDirectoriesEntryName];
					string baseDir = AppDomain.CurrentDomain.BaseDirectory;

					foreach (string dir in from subEntry in loadableFilesDirectoriesEntry.SubEntries
										   let content = subEntry.Value.GetContent<TextContent>()
										   where content != null &&
												 !ExtraFilesLookUp.Directories.Contains(content.Text)
										   select PathUtils.GetLocalPath(Path.Combine(baseDir, content.Text)))
					{
						ExtraFilesLookUp.Directories.Add(dir);
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

		private static VectorContent TryGetVector(Database appConfigurationDatabase, string entryName)
		{
			return appConfigurationDatabase.Contains(entryName, false)
					   ? appConfigurationDatabase[entryName].GetContent<VectorContent>()
					   : null;
		}
	}
}