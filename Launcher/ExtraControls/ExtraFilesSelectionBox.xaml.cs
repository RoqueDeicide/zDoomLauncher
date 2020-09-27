using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Launcher.Annotations;

namespace Launcher
{
	/// <summary>
	/// Represents an object that is used to select the name of the directory out of its full path.
	/// </summary>
	public class DirectoryNameSelector : IValueConverter
	{
		/// <summary>
		/// Selects the name of the directory out of its full name.
		/// </summary>
		/// <param name="value">     Object that is supposed to represent the full path to the directory.</param>
		/// <param name="targetType">The object that is supposed to represent <see cref="string"/> type.</param>
		/// <param name="parameter"> Ignored.</param>
		/// <param name="culture">   Ignored.</param>
		/// <returns>
		/// If operation was a success, then the name of the directory is returned, otherwise <c>null</c> is returned.
		/// </returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}

			if (targetType != typeof(string))
			{
				return null;
			}

			try
			{
				return Path.GetFileName((value as CollectionViewGroup)?.Name as string);
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="value">     Ignored.</param>
		/// <param name="targetType">Ignored.</param>
		/// <param name="parameter"> Ignored.</param>
		/// <param name="culture">   Ignored.</param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

	/// <summary>
	/// Interaction logic for ExtraFilesSelectionBox.xaml
	/// </summary>
	public partial class ExtraFilesSelectionBox
	{
		private ObservableCollection<string> selectedFileNames;

		/// <summary>
		/// Gets the list of files that are currently selected.
		/// </summary>
		[NotNull]
		public ObservableCollection<string> SelectedFiles
		{
			get => this.selectedFileNames;
			set
			{
				if (this.selectedFileNames.Equals(value))
				{
					return;
				}

				this.selectedFileNames.CollectionChanged -= this.UpdateSelectionFromOutside;
				this.selectedFileNames                   =  value;
				this.selectedFileNames.CollectionChanged += this.UpdateSelectionFromOutside;
				this.UpdateSelectionFromOutside(this, new NotifyCollectionChangedEventArgs
													(NotifyCollectionChangedAction.Reset));
			}
		}

		private void UpdateSelectionFromOutside(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.FileSelection is null) // this.FileSelection is null if update comes from this object.
			{
				// Prevent infinite recursion.
				return;
			}

			var selectedNames = this.selectedFileNames;
			this.selectedFileNames = null;

			this.ClearSelection();

			var files = ExtraFilesLookUp.LoadableFiles;

			// Add files that are available to the list.
			foreach (FileDesc file in from filePath in selectedNames
									  let fileDesc = files.FirstOrDefault(x => x.FullPath == filePath)
									  where fileDesc != null
									  select fileDesc)
			{
				this.FileSelection.Add(file);
				file.Selected = true;
			}

			this.UpdateTopBottomSpinners();

			this.selectedFileNames = selectedNames;
		}

		/// <summary>
		/// Gets the observable collection that contains information about selected files.
		/// </summary>
		public ObservableCollection<object> FileSelection { get; private set; }

		/// <summary>
		/// Creates a user control.
		/// </summary>
		public ExtraFilesSelectionBox()
		{
			this.selectedFileNames                   =  new ObservableCollection<string>();
			this.selectedFileNames.CollectionChanged += this.UpdateSelectionFromOutside;
			this.FileSelection                       =  new ObservableCollection<object>();

			this.InitializeComponent();

			ICollectionView allFilesCollView = CollectionViewSource.GetDefaultView(this.AllFilesListBox.ItemsSource);
			if (allFilesCollView.GroupDescriptions.OfType<PropertyGroupDescription>().All(x=>x.PropertyName != nameof(FileDesc.Directory)))
			{
				allFilesCollView.GroupDescriptions.Add(new PropertyGroupDescription("Directory"));
			}

			ExtraFilesLookUp.LoadableFiles.CollectionChanged += this.RemoveUnavailableSelection;
			this.FileSelection.CollectionChanged             += this.UpdatedSelectionFromInside;
		}

		private void UpdatedSelectionFromInside(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.selectedFileNames == null) // this.selectedFileNames is null if update comes from outside.
			{
				// Prevent infinite recursion.
				return;
			}

			var selectedDescriptions = this.FileSelection;
			this.FileSelection = null;

			this.selectedFileNames.Clear();

			this.selectedFileNames.AddRange(from fileDesc in selectedDescriptions.OfType<FileDesc>()
											select fileDesc.FullPath);

			this.FileSelection = selectedDescriptions;
		}

		private void ClearSelection()
		{
			foreach (FileDesc file in this.FileSelection.OfType<FileDesc>())
			{
				file.Selected = false;
			}

			this.FileSelection.Clear();
		}

		private void RemoveUnavailableSelection(object sender, NotifyCollectionChangedEventArgs args)
		{
			switch (args.Action)
			{
				case NotifyCollectionChangedAction.Remove:
				case NotifyCollectionChangedAction.Replace:
				case NotifyCollectionChangedAction.Reset:

					for (int i = 0; i < this.FileSelection.Count; i++)
					{
						var file = this.FileSelection[i] as FileDesc;

						Debug.Assert(file != null, "file != null");

						if (!ExtraFilesLookUp.LoadableFiles.Contains(file))
						{
							file.Selected = false;
							this.FileSelection.RemoveAt(i--);
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

		private void ScrollTheViewer(object sender, MouseWheelEventArgs e)
		{
			if (!(sender is ScrollViewer sv))
			{
				return;
			}

			if (e.Delta < 0)
			{
				if (sv.VerticalOffset - e.Delta <= sv.ExtentHeight - sv.ViewportHeight)
				{
					sv.ScrollToVerticalOffset(sv.VerticalOffset - e.Delta);
				}
				else
				{
					sv.ScrollToBottom();
				}
			}
			else
			{
				if (sv.VerticalOffset + e.Delta > 0)
				{
					sv.ScrollToVerticalOffset(sv.VerticalOffset - e.Delta);
				}
				else
				{
					sv.ScrollToTop();
				}
			}

			e.Handled = true;
		}
	}
}