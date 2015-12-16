
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

			appConfigurationDatabase.AddEntry
			(
				new DatabaseEntry
				(
					LastConfigurationFileEntryName,
					new TextContent
					(
						String.IsNullOrWhiteSpace(this.file)
						&&
						File.Exists(this.file)
							? "Nothing"
							: this.file
					)
				)
			);

			appConfigurationDatabase.AddEntry
			(
				new DatabaseEntry
				(
					GameFolderEntryName,
					new TextContent
					(
						String.IsNullOrWhiteSpace(this.zDoomFolder)
						&&
						File.Exists(Path.Combine(this.zDoomFolder, "zdoom.exe"))
							? "Nothing"
							: this.zDoomFolder
					)
				)
			);

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
							&&
							File.Exists(Path.Combine(this.zDoomFolder, "zdoom.exe"))
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