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
			this.Title = string.Format("ZDoom Launcher - {0}", this.config.Name);
		}
	}
}