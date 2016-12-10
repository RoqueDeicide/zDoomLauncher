// App configuration.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Launcher.Databases;
using Launcher.Logging;

namespace Launcher
{
	public partial class MainWindow
	{
		private void SaveAppConfiguration()
		{
			Database appConfigurationDatabase = new Database(AppConfigurationXmlExtension,
															 AppConfigurationBinaryExtension);

			var content = ToEntryContent(!string.IsNullOrWhiteSpace(this.CurrentConfigFile) &&
										 File.Exists(this.CurrentConfigFile),
										 this.CurrentConfigFile);
			var entry = new DatabaseEntry(LastConfigurationFileEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			content = ToEntryContent(!string.IsNullOrWhiteSpace(this.zDoomFolder), this.zDoomFolder);
			entry = new DatabaseEntry(GameFolderEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			content = ToEntryContent(!string.IsNullOrWhiteSpace(this.currentExeFile), this.currentExeFile);
			entry = new DatabaseEntry(LastExeFileEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			// Save the directories.
			var loadableFilesDirectoriesEntry = new DatabaseEntry(LoadableFilesDirectoriesEntryName, null);

			string baseDir = AppDomain.CurrentDomain.BaseDirectory;
			List<string> dirs = new List<string>(from directory in ExtraFilesLookUp.Directories
												 select PathUtils.ToRelativePath(directory, baseDir));
			dirs.Sort();

			int counter = 0;
			foreach (string dir in dirs)
			{
				content = new TextContent(dir);
				entry = new DatabaseEntry(LoadableFilesDirectoryEntryName, content);

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

				Database appConfigurationDatabase = new Database(AppConfigurationXmlExtension,
																 AppConfigurationBinaryExtension);
				appConfigurationDatabase.Load(AppConfigurationFileName);

				this.CurrentConfigFile = TryGetEntryText(appConfigurationDatabase, LastConfigurationFileEntryName);
				this.zDoomFolder = TryGetEntryText(appConfigurationDatabase, GameFolderEntryName);
				this.currentExeFile = TryGetEntryText(appConfigurationDatabase, LastExeFileEntryName);

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
	}
}