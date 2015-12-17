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
		private static IEnumerable<string> GetLoadableFiles(string folder)
		{
			Log.Message("Looking for loadable files in {0}", folder);
			return
				from file in Directory.EnumerateFiles(folder, "*.*", SearchOption.TopDirectoryOnly)
				select Path.GetFileName(file) into fileName
				where fileName != null
				let extension = Path.GetExtension(fileName).ToLowerInvariant()
				where extension == ".wad" || extension == ".pk3"
				where !Iwads.SupportedIwads.Keys.Contains(fileName.ToLowerInvariant())
				select fileName;
		}
		private static TextContent ToEntryContent(bool condition, string text)
		{
			return new TextContent(condition ? text : "Nothing");
		}
	}
}