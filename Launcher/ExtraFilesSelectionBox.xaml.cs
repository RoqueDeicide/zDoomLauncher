using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Launcher.Annotations;
using Launcher.Logging;
using Path = System.IO.Path;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for ExtraFilesSelectionBox.xaml
	/// </summary>
	public partial class ExtraFilesSelectionBox
	{
		private string gameFolder;
		private readonly List<string> extraFiles;
		private readonly List<Button> extraFilesAddButtons;
		private List<string> selectedFiles;
		/// <summary>
		/// Gets or sets the folder where to look for the files.
		/// </summary>
		public string GameFolder
		{
			get { return this.gameFolder; }
			set
			{
				if (this.gameFolder == value)
				{
					return;
				}

				this.gameFolder = value;

				this.ClearFiles();

				if (!Directory.Exists(this.gameFolder))
				{
					return;
				}

				this.RefreshMainList();
			}
		}
		/// <summary>
		/// Gets the list of files that are currently selected.
		/// </summary>
		[CanBeNull]
		public List<string> SelectedFiles
		{
			get { return this.selectedFiles; }
			set
			{
				this.selectedFiles = value;

				this.ClearSelection();

				if (this.selectedFiles == null || this.selectedFiles.Count == 0)
				{
					return;
				}

				// Add all files to the selection. Remove files that are not in the main list.
				if (this.extraFiles.Count == 0)
				{
					this.selectedFiles.Clear();
					return;
				}

				for (int i = 0; i < this.selectedFiles.Count; i++)
				{
					string currentSelectedFile = this.selectedFiles[i];

					int fileIndex = this.extraFiles.IndexOf(currentSelectedFile);
					if (fileIndex < 0)
					{
						this.selectedFiles.RemoveAt(i--);
					}
					else
					{
						this.AddFileToSelection(this.extraFilesAddButtons[fileIndex], null);
					}
				}
			}
		}
		/// <summary>
		/// Creates a user control.
		/// </summary>
		public ExtraFilesSelectionBox()
		{
			this.extraFiles = new List<string>(50);
			this.extraFilesAddButtons = new List<Button>(50);
			this.selectedFiles = null;

			this.InitializeComponent();
		}

		private void ClearSelection()
		{
			this.FilesSelectionGrid.Children.Clear();
			this.FilesSelectionGrid.RowDefinitions.Clear();
			// Add the ending filler row to the selection grid.
			this.FilesSelectionGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(1, GridUnitType.Star)
			});
		}
		private void ClearFiles()
		{
			this.ExtraFilesGrid.Children.Clear();
			this.ExtraFilesGrid.RowDefinitions.Clear();

			this.ClearSelection();

			this.extraFiles.Clear();
		}
		private void RefreshMainList()
		{
			if (!Directory.Exists(this.gameFolder))
			{
				Log.Error("Attempted to refresh the extra files box without valid game folder.");
				return;
			}

			// Get the list of files from base directory.
			this.extraFiles.AddRange(GetLoadableFiles(this.gameFolder));
			
			// Get the files from DOOMWADDIR environment variable.
			string doomWadVar = Environment.GetEnvironmentVariable("DOOMWADDIR");
			if (!string.IsNullOrWhiteSpace(doomWadVar))
			{
				this.extraFiles.AddRange(GetLoadableFiles(doomWadVar));
			}

			// Add the file rows to the main list.
			for (int i = 0; i < this.extraFiles.Count; i++)
			{
				this.AddExtraFileRow(i);
			}

			// Add the row after last one that fills the rest of the grid.
			this.ExtraFilesGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(1, GridUnitType.Star)
			});

		}
		private void AddExtraFileRow(int index)
		{
			Button addFile = this.CreateAddFileButton(index);
			Button removeFile = this.CreateRemoveFileButton(index);
			addFile.Tag = removeFile;
			removeFile.Tag = addFile;
			this.ExtraFilesGrid.Children.Add(addFile);
			this.ExtraFilesGrid.Children.Add(removeFile);
			this.extraFilesAddButtons.Add(addFile);

			// The text.
			TextBlock block = new TextBlock(new Run(this.extraFiles[index]));
			Grid.SetColumn(block, 2);
			Grid.SetRow(block, index);
			this.ExtraFilesGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(20, GridUnitType.Pixel)
			});

			this.ExtraFilesGrid.Children.Add(block);
		}
		private void AddFileSelectionRow(int index, bool addToMainList = true)
		{
			if (this.SelectedFiles == null)
			{
				return;
			}

			string selectedFile = this.extraFiles[index];

			if (addToMainList)
			{
				this.SelectedFiles.Add(selectedFile);
			}

			int selectionIndex = this.FilesSelectionGrid.RowDefinitions.Count - 1;

			// Add a row.
			this.FilesSelectionGrid.RowDefinitions.Insert(selectionIndex, new RowDefinition
			{
				Height = new GridLength(20, GridUnitType.Pixel)
			});

			// Add buttons.
			this.FilesSelectionGrid.Children.Add(this.CreateMoveUpButton(index, selectionIndex));
			this.FilesSelectionGrid.Children.Add(this.CreateMoveDownButton(index, selectionIndex));

			// The text.
			TextBlock block = new TextBlock(new Run(selectedFile));
			Grid.SetColumn(block, 2);
			Grid.SetRow(block, selectionIndex);

			this.FilesSelectionGrid.Children.Add(block);
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
	}
}
