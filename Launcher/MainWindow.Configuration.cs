
// App configuration.

using System;
using System.IO;
using Launcher.Databases;
using Launcher.Logging;

namespace Launcher
{
	public partial class MainWindow
	{
		private void SaveAppConfiguration()
		{
			Database appConfigurationDatabase = new Database("config", "binaryConfig");

			appConfigurationDatabase.AddEntry
			(
				new DatabaseEntry
				(
					"LastLaunchConfigurationFile",
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
					"zDoomInstallationFolder",
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

			appConfigurationDatabase.Save("Zdl.config");
		}

		private void LoadAppConfiguration()
		{
			if (File.Exists("Zdl.config"))
			{
				Log.Message("Loading configuration file.");
				Database appConfigurationDatabase = new Database("config", "binaryConfig");
				appConfigurationDatabase.Load("Zdl.config");
				if (appConfigurationDatabase.Contains("LastLaunchConfigurationFile", false))
				{
					string entryText =
						appConfigurationDatabase["LastLaunchConfigurationFile"]
							.GetContent<TextContent>()
							.Text;
					this.file =
						entryText == "Nothing" && File.Exists(entryText)
							? null
							: entryText;
				}
				if (appConfigurationDatabase.Contains("zDoomInstallationFolder", false))
				{
					string entryText =
						appConfigurationDatabase["zDoomInstallationFolder"]
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
	}
}