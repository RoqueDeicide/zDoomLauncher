using System.Collections.Generic;
using System.IO;
using System.Windows;
using Ookii.Dialogs.Wpf;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for Directories.xaml
	/// </summary>
	public partial class Directories
	{
		private readonly VistaFolderBrowserDialog dialog;

		public Directories()
		{
			this.dialog = new VistaFolderBrowserDialog
						  {
							  Description            = @"Folder Where to Look for Loadable Files in.",
							  ShowNewFolderButton    = true,
							  UseDescriptionForTitle = true
						  };

			this.InitializeComponent();
		}

		private void AddDirectoryNoSubDirectories(object sender, RoutedEventArgs e)
		{
			this.AddDirectory(false);
		}

		private void AddDirectoryWithSubDirectories(object sender, RoutedEventArgs e)
		{
			this.AddDirectory(true);
		}

		private void AddDirectory(bool withSubFolders)
		{
			if (this.dialog.ShowDialog() == true && Directory.Exists(this.dialog.SelectedPath))
			{
				string path = this.dialog.SelectedPath;

				AddDirectory(path);

				if (withSubFolders)
				{
					foreach (string directory in Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories))
					{
						AddDirectory(directory);
					}
				}
			}
		}

		private static void AddDirectory(string directory)
		{
			int index = ExtraFilesLookUp.Directories.BinarySearch(directory);

			if (index >= 0)
			{
				// We already have it.
				return;
			}

			// Insert where necessary to maintain a sorted sequence.
			ExtraFilesLookUp.Directories.Insert(~index, directory);
		}

		private void RemoveDirectory(object sender, RoutedEventArgs e)
		{
			var element = sender as FrameworkElement;

			if (!(element?.DataContext is string name))
			{
				return;
			}

			// Remove the row.
			int index = ExtraFilesLookUp.Directories.BinarySearch(name);
			if (index < 0)
			{
				return;
			}

			ExtraFilesLookUp.Directories.RemoveAt(index);
		}
	}
}