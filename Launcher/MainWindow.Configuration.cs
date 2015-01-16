
// App configuration.

using System;
using System.IO;
using Launcher.Databases;

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
					"Last Launch Configuration File",
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

			appConfigurationDatabase.Save("Zdl.config");
		}

		private void LoadAppConfiguration()
		{
			if (File.Exists("Zdl.config"))
			{
				Database appConfigurationDatabase = new Database("config", "binaryConfig");
				if (appConfigurationDatabase.Contains("Last Launch Configuration File", false))
				{
					string entryText =
						appConfigurationDatabase["Last Launch Configuration File"]
							.GetContent<TextContent>()
							.Text;
					this.file =
						entryText == "Nothing" && File.Exists(entryText)
							? null
							: entryText;
				}
			}
		}
	}
}