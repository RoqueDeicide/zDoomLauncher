using System;
using System.Linq;
using System.Windows.Controls;
using Launcher.Databases;

namespace Launcher
{
	public partial class MainWindow
	{
		private bool EpisodicIwadIsSelected()
		{
			IwadFile selectedIwad = this.IwadComboBox.SelectedItem as IwadFile;
			
			return selectedIwad != null && selectedIwad.Episodic;
		}
		private static TextContent ToEntryContent(bool condition, string text)
		{
			return new TextContent(condition ? text : "Nothing");
		}
		private void UpdateWindowTitle()
		{
			bool hasName = this.config != null && !string.IsNullOrWhiteSpace(this.config.Name);
			bool hasFile = !string.IsNullOrWhiteSpace(this.CurrentConfigFile);

			this.Title = string.Format("ZDoom Launcher{0}{1}{2}{3}",
									   hasName ? " - " : "",
									   hasName ? this.config.Name : "",
									   hasFile ? " - " : "",
									   hasFile ? this.CurrentConfigFile : "");
		}
	}
}