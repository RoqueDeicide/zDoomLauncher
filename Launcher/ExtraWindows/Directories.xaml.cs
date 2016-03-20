using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ookii.Dialogs.Wpf;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for Directories.xaml
	/// </summary>
	public partial class Directories : Window
	{
		private readonly VistaFolderBrowserDialog dialog;

		public Directories()
		{
			this.dialog = new VistaFolderBrowserDialog
			{
				Description = @"Folder Where to Look for Loadable Files in.",
				ShowNewFolderButton = true,
				UseDescriptionForTitle = true
			};

			this.InitializeComponent();

			for (int i = 0; i < ExtraFilesLookUp.Directories.Count; i++)
			{
				string directory = ExtraFilesLookUp.Directories[i];
				
				this.DirectoriesListBox.Items.Add(this.CreateDirectoryName(directory, i));
			}

			// Add empty row, if there are no directories in the list.
			if (ExtraFilesLookUp.Directories.Count == 0)
			{
				this.DirectoriesListBox.Items.Add(this.CreateDirectoryName("", -1));
			}
		}
		private DirectoryName CreateDirectoryName(string path, int index)
		{
			var name = new DirectoryName
			{
				Path = path,
				Index = index
			};
			name.PropertyChanged += this.DirectoryNameChanged;
			return name;
		}

		private void DirectoryNameChanged(object sender, PropertyChangedEventArgs  e)
		{
			DirectoryName directory = sender as DirectoryName;
			if (directory == null)
			{
				return;
			}

			if (e.PropertyName != "Path")
			{
				return;
			}

			// Remove the directory from the observable collection if new path doesn't exist.
			if (!directory.Exists && directory.Index > -1)
			{
				this.RemovePath(directory.Index);

				return;
			}

			// Add the directory to the collection, if there is no such directory in it.
			if (directory.Exists && directory.Index == -1)
			{
				if (ExtraFilesLookUp.Directories.Any(x=>x == directory.Path))
				{
					return;
				}

				int senderIndex = this.DirectoriesListBox.Items.IndexOf(directory);
				if (senderIndex == 0)
				{
					// Insert at the start of global collection.
					this.InsertPath(0, directory);
				}
				else if (senderIndex == this.DirectoriesListBox.Items.Count - 1)
				{
					directory.Index = ExtraFilesLookUp.Directories.Count;
					// Just add to the end.
					ExtraFilesLookUp.Directories.Add(directory.Path);
				}
				else
				{
					// Insert somewhere between directories this one is in between.
					var next = this.DirectoriesListBox.Items[senderIndex + 1] as DirectoryName;

					Debug.Assert(next != null);

					this.InsertPath(next.Index, directory);
				}

				return;
			}

			if (directory.Exists && directory.Index > -1)
			{
				// Replace the path.
				ExtraFilesLookUp.Directories[directory.Index] = directory.Path;
			}
		}
		private void OpenFolderDialog(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;

			DirectoryName name = button?.DataContext as DirectoryName;
			if (name == null)
			{
				return;
			}

			this.dialog.SelectedPath = name.Exists ? name.Path : "";

			if (this.dialog.ShowDialog(this) == true && Directory.Exists(this.dialog.SelectedPath))
			{
				name.Path = this.dialog.SelectedPath;
			}
		}
		private void InsertDirectoryAbove(object sender, RoutedEventArgs e)
		{
			this.InsertRow(sender, 0);
		}
		private void InsertDirectoryBelow(object sender, RoutedEventArgs e)
		{
			this.InsertRow(sender, 1);
		}
		private void InsertRow(object sender, int offset)
		{
			FrameworkElement element = sender as FrameworkElement;

			DirectoryName name = element?.DataContext as DirectoryName;
			if (name == null)
			{
				return;
			}

			int senderIndex = this.DirectoriesListBox.Items.IndexOf(name);
			this.DirectoriesListBox.Items.Insert(senderIndex + offset, this.CreateDirectoryName("", -1));
		}
		private void RemoveDirectory(object sender, RoutedEventArgs e)
		{
			FrameworkElement element = sender as FrameworkElement;

			DirectoryName name = element?.DataContext as DirectoryName;
			if (name == null)
			{
				return;
			}

			// Clear the text box, if this is only one row.
			if (this.DirectoriesListBox.Items.Count == 1)
			{
				name.Path = "";
				return;
			}

			if (name.Index > -1)
			{
				this.RemovePath(name.Index);
			}

			// Remove the row.
			this.DirectoriesListBox.Items.Remove(name);
		}

		private void InsertPath(int index, DirectoryName name)
		{
			ExtraFilesLookUp.Directories.Insert(index, name.Path);

			// Adjust indexes.
			for (int i = 0; i < this.DirectoriesListBox.Items.Count; i++)
			{
				DirectoryName directory = this.DirectoriesListBox.Items[i] as DirectoryName;
				
				Debug.Assert(directory != null);

				if (directory.Index >= name.Index)
				{
					directory.Index++;
				}
			}

			name.Index = index;
		}
		private void RemovePath(int index)
		{
			ExtraFilesLookUp.Directories.RemoveAt(index);

			// Adjust indexes.
			for (int i = 0; i < this.DirectoriesListBox.Items.Count; i++)
			{
				DirectoryName directory = this.DirectoriesListBox.Items[i] as DirectoryName;

				if (directory?.Index > index)
				{
					directory.Index--;
				}
			}
		}
	}
}
