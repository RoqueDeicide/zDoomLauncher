using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Launcher.Annotations;
using Launcher.Logging;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for ExtraFilesSelectionBox.xaml
	/// </summary>
	public partial class ExtraFilesSelectionBox
	{
		private string gameFolder;
		private readonly List<LoadableFile> extraFiles;
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

					LoadableFile file = this.extraFiles.Find(x => x.FileName == currentSelectedFile);
					if (file == null)
					{
						this.selectedFiles.RemoveAt(i--);
					}
					else
					{
						this.AddFileToSelection(file.SelectButton, null);
					}
				}
			}
		}
		/// <summary>
		/// Creates a user control.
		/// </summary>
		public ExtraFilesSelectionBox()
		{
			this.extraFiles = new List<LoadableFile>(50);
			this.selectedFiles = null;

			this.InitializeComponent();
		}

		private void ClearSelection()
		{
			// Look through the selection list to find which files are selected.
			var selectedFiles = from selectionText in this.FilesSelectionGrid.Children.OfType<TextBlock>()
								let run = selectionText.Inlines.FirstInline as Run
								where run != null
								select run.Text;

			// Find the files and execute the event handlers on "deselect" buttons.
			var deselectionButtons =
				from selectedFile in selectedFiles
				select this.ExtraFilesGrid.Children
						   .OfType<TextBlock>()
						   .FirstOrDefault(x => x.Inlines.FirstInline is Run &&
												((Run)x.Inlines.FirstInline).Text == selectedFile)
				into mainListFile
				where mainListFile != null
				select Grid.GetRow(mainListFile)
				into index
				select
					this.ExtraFilesGrid.Children.OfType<Button>()
						.FirstOrDefault(x => Grid.GetRow(x) == index && x.Content.Equals(CrossSymbol));

			foreach (Button removeButton in deselectionButtons.ToList())
			{
				this.RemoveFileFromSelection(removeButton, null);
			}

			if (this.FilesSelectionGrid.RowDefinitions.Count == 1)
			{
				return;
			}

			// Clear off all stragglers.

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

			List<string> extraFiles = new List<string>(50);

			// Get the list of files from base directory.
			extraFiles.AddRange(GetLoadableFiles(this.gameFolder));

			// Get the files from DOOMWADDIR environment variable.
			string doomWadVar = Environment.GetEnvironmentVariable("DOOMWADDIR");
			if (!string.IsNullOrWhiteSpace(doomWadVar))
			{
				extraFiles.AddRange(GetLoadableFiles(doomWadVar));
			}

			// Add the file rows to the main list.
			for (int i = 0; i < extraFiles.Count; i++)
			{
				this.AddExtraFileRow(i, extraFiles[i]);
			}

			// Add the row after last one that fills the rest of the grid.
			this.ExtraFilesGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(1, GridUnitType.Star)
			});
		}
		private void AddExtraFileRow(int index, string filePath)
		{
			// Create an object that will hold references to all objects relevant to the file.
			LoadableFile file = new LoadableFile
			{
				FileName = filePath
			};
			file.SelectButton = this.CreateAddFileButton(index, file);
			file.DeselectButton = this.CreateRemoveFileButton(index, file);

			// The text.
			TextBlock block = new TextBlock(new Run(file.FileName));
			Grid.SetColumn(block, 2);
			Grid.SetRow(block, index);
			this.ExtraFilesGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(20, GridUnitType.Pixel)
			});
			file.MainListText = block;

			this.ExtraFilesGrid.Children.Add(file.SelectButton);
			this.ExtraFilesGrid.Children.Add(file.DeselectButton);
			this.ExtraFilesGrid.Children.Add(file.MainListText);

			this.extraFiles.Add(file);
		}
		private void AddFileSelectionRow(int index, bool addToMainList = true)
		{
			if (this.SelectedFiles == null)
			{
				return;
			}

			LoadableFile selectedFile = this.extraFiles[index];

			if (addToMainList)
			{
				this.SelectedFiles.Add(selectedFile.FileName);
			}

			int selectionIndex = this.FilesSelectionGrid.RowDefinitions.Count - 1;

			// Add a row.
			this.FilesSelectionGrid.RowDefinitions.Insert(selectionIndex, new RowDefinition
			{
				Height = new GridLength(20, GridUnitType.Pixel)
			});

			// Add buttons.
			selectedFile.MoveUpButton = this.CreateMoveUpButton(selectedFile, selectionIndex);
			selectedFile.MoveDownButton = this.CreateMoveDownButton(selectedFile, selectionIndex);

			// The context menu for the text.
			MenuItem contextMenuItem = new MenuItem
			{
				Header = "Deselect",
				Tag = selectedFile
			};
			contextMenuItem.Click += this.RemoveFileFromSelection;
			ContextMenu contextMenu = new ContextMenu();
			contextMenu.Items.Add(contextMenuItem);

			// The text.
			TextBlock block = new TextBlock(new Run(selectedFile.FileName))
			{
				ContextMenu = contextMenu
			};
			Grid.SetColumn(block, 2);
			Grid.SetRow(block, selectionIndex);
			selectedFile.SelectionListText = block;

			this.FilesSelectionGrid.Children.Add(selectedFile.MoveUpButton);
			this.FilesSelectionGrid.Children.Add(selectedFile.MoveDownButton);
			this.FilesSelectionGrid.Children.Add(selectedFile.SelectionListText);
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