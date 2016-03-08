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
			ComboBoxItem selectedIwad = this.IwadComboBox.SelectedItem as ComboBoxItem;
			return
				selectedIwad != null
				&&
				Iwads.EpisodicIwads.Any(iwadName =>
											iwadName.Equals((string)selectedIwad.Tag,
															StringComparison.InvariantCultureIgnoreCase));
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