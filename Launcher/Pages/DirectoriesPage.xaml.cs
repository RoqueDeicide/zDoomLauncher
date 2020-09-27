using System.IO;
using System.Windows;
using Ookii.Dialogs.Wpf;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for DirectoriesPage.xaml
	/// </summary>
	public partial class DirectoriesPage
	{
		private readonly VistaFolderBrowserDialog dialog;

		public DirectoriesPage()
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

				ExtraFilesLookUp.AddDirectory(path);

				if (withSubFolders)
				{
					foreach (string directory in Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories))
					{
						ExtraFilesLookUp.AddDirectory(directory);
					}
				}
			}
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