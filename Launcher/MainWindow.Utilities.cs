using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace Launcher
{
	public partial class MainWindow
	{
		private bool EpisodicIwadIsSelected()
		{
			ComboBoxItem selectedIwad = this.IwadComboBox.SelectedItem as ComboBoxItem;
			return selectedIwad != null && Iwads.EpisodicIwads.Contains((string)selectedIwad.Tag);
		}
		private static IEnumerable<string> GetLoadableFiles(string folder)
		{
			return
				Directory.EnumerateFiles
				(
					folder,
					"*.*",
					SearchOption.TopDirectoryOnly
				)
				.Where
				(
					x =>
						x.EndsWith("wad", StringComparison.InvariantCultureIgnoreCase) ||
						x.EndsWith("pk3", StringComparison.InvariantCultureIgnoreCase)
				)
				.Select(Path.GetFileName);
		}
	}
}