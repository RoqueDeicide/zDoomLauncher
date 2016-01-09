using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Launcher.Databases;
using Launcher.Logging;

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
	}
}