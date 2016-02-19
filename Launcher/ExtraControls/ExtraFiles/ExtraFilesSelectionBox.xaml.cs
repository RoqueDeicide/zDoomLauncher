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
						this.ToggleFileSelection(file.SelectDeselectButton, null);
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
			// Look through the list of extra files to see which ones are selected.
			var selectedFiles = from loadableFile in this.extraFiles
								where loadableFile.Selected
								select loadableFile;

			foreach (LoadableFile selectedFile in selectedFiles)
			{
				// Invoke the "Click" event handler. That should cause the file to be deselected.
				this.ToggleFileSelection(selectedFile.SelectDeselectButton, null);
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
			extraFiles.AddRange(ExtraFilesLookUp.GetLoadableFiles(this.gameFolder));

			// Get the files from DOOMWADDIR environment variable.
			string doomWadVar = Environment.GetEnvironmentVariable("DOOMWADDIR");
			if (!string.IsNullOrWhiteSpace(doomWadVar))
			{
				extraFiles.AddRange(ExtraFilesLookUp.GetLoadableFiles(doomWadVar));
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
			file.SelectDeselectButton = this.CreateAddRemoveFileButton(index, file);

			// The text.
			TextBlock block = new TextBlock(new Run(file.FileName));
			Grid.SetColumn(block, 1);
			Grid.SetRow(block, index);
			this.ExtraFilesGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(20, GridUnitType.Pixel)
			});
			file.MainListText = block;

			this.ExtraFilesGrid.Children.Add(file.SelectDeselectButton);
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
			selectedFile.MoveButtons = this.CreateMoveButtons(selectedFile, selectionIndex);

			// The context menu for the text.
			MenuItem contextMenuItem = new MenuItem
			{
				Header = "Deselect",
				Tag = selectedFile
			};
			contextMenuItem.Click += this.ToggleFileSelection;
			ContextMenu contextMenu = new ContextMenu();
			contextMenu.Items.Add(contextMenuItem);

			// The text.
			TextBlock block = new TextBlock(new Run(selectedFile.FileName))
			{
				ContextMenu = contextMenu
			};
			Grid.SetColumn(block, 1);
			Grid.SetRow(block, selectionIndex);
			selectedFile.SelectionListText = block;

			this.FilesSelectionGrid.Children.Add(selectedFile.MoveButtons);
			this.FilesSelectionGrid.Children.Add(selectedFile.SelectionListText);
		}
	}
}