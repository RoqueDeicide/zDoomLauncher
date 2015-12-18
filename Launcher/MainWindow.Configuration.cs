// App configuration.

using System;
using System.IO;
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

			var content = ToEntryContent(!string.IsNullOrWhiteSpace(this.file) && File.Exists(this.file),
										 this.file);
			var entry = new DatabaseEntry(LastConfigurationFileEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			content = ToEntryContent(!string.IsNullOrWhiteSpace(this.zDoomFolder), this.zDoomFolder);
			entry = new DatabaseEntry(GameFolderEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			content = ToEntryContent(!string.IsNullOrWhiteSpace(this.currentExeFile), this.currentExeFile);
			entry = new DatabaseEntry(LastExeFileEntryName, content);
			appConfigurationDatabase.AddEntry(entry);

			appConfigurationDatabase.Save(AppConfigurationFileName);
		}

		private void LoadAppConfiguration()
		{
			try
			{
				if (File.Exists(AppConfigurationFileName))
				{
					Log.Message("Loading configuration file.");
					Database appConfigurationDatabase = new Database(AppConfigurationXmlExtension,
																	 AppConfigurationBinaryExtension);
					appConfigurationDatabase.Load(AppConfigurationFileName);
					if (appConfigurationDatabase.Contains(LastConfigurationFileEntryName, false))
					{
						string entryText =
							appConfigurationDatabase[LastConfigurationFileEntryName]
								.GetContent<TextContent>()
								.Text;
						this.file =
							entryText == "Nothing" && File.Exists(entryText)
								? null
								: entryText;
					}
					if (appConfigurationDatabase.Contains(GameFolderEntryName, false))
					{
						string entryText =
							appConfigurationDatabase[GameFolderEntryName]
								.GetContent<TextContent>()
								.Text;
						this.zDoomFolder =
							entryText == "Nothing"
								? null
								: entryText;
					}
					if (appConfigurationDatabase.Contains(LastExeFileEntryName, false))
					{
						string entryText =
							appConfigurationDatabase[LastExeFileEntryName]
								.GetContent<TextContent>()
								.Text;
						this.currentExeFile =
							entryText == "Nothing"
								? null
								: entryText;
					}
				}
				else
				{
					Log.Message("No application configuration file was found.");
				}
			}
			catch (XmlException)
			{
				Log.Warning("Unable to load application configuration file. Ignoring.");
			}
		}
	}
}