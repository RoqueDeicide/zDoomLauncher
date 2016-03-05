using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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
		private readonly ObservableCollection<object> fileSelection;
		private List<string> selectedFiles;
			/// <summary>
		/// Gets the list of files that are currently selected.
		/// </summary>
		[NotNull]
		public List<string> SelectedFiles
		{
			get { return this.selectedFiles; }
			set
			{
				this.ClearSelection();
				this.selectedFiles = null;

				var selectedFiles = value;

				var files = ExtraFilesLookUp.LoadableFiles;

				// Add files that are available to the list.
				foreach (FileDesc file in from filePath in selectedFiles
										  let fileDesc = files.FirstOrDefault(x => x.FullPath == filePath)
										  where fileDesc != null
										  select fileDesc)
				{
					this.FileSelection.Add(file);
					file.Selected = true;
				}

				this.selectedFiles = selectedFiles;
			}
		}
		/// <summary>
		/// Gets the observable collection that contains information about selected files.
		/// </summary>
		public ObservableCollection<object> FileSelection
		{
			get { return this.fileSelection; }
		}
		/// <summary>
		/// Creates a user control.
		/// </summary>
		public ExtraFilesSelectionBox()
		{
			this.fileSelection = new ObservableCollection<object>();

			this.InitializeComponent();

			ExtraFilesLookUp.LoadableFiles.CollectionChanged += this.RemoveUnavailableSelection;
			this.FileSelection.CollectionChanged += this.UpdatedSelectedFiles;
		}
		private void UpdatedSelectedFiles(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.selectedFiles == null)
			{
				return;
			}

			this.selectedFiles.Clear();

			this.selectedFiles.AddRange(from fileDesc in this.FileSelection.OfType<FileDesc>()
										select fileDesc.FullPath);
		}

		private void ClearSelection()
		{
			foreach (FileDesc file in this.fileSelection.OfType<FileDesc>())
			{
				file.Selected = false;
			}
			this.fileSelection.Clear();
		}
		private void RemoveUnavailableSelection(object sender, NotifyCollectionChangedEventArgs args)
		{
			switch (args.Action)
			{
				case NotifyCollectionChangedAction.Remove:
				case NotifyCollectionChangedAction.Replace:
				case NotifyCollectionChangedAction.Reset:

					for (int i = 0; i < this.fileSelection.Count; i++)
					{
						FileDesc file = this.fileSelection[i] as FileDesc;

						Debug.Assert(file != null, "file != null");

						if (!ExtraFilesLookUp.LoadableFiles.Contains(file))
						{
							file.Selected = false;
							this.fileSelection.RemoveAt(i--);
						}
					}

					break;
				case NotifyCollectionChangedAction.Add:
					break;
				case NotifyCollectionChangedAction.Move:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}