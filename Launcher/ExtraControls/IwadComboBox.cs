using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Launcher
{
	/// <summary>
	/// Represents a combo-box that is used to select an iwad file to load.
	/// </summary>
	public class IwadComboBox : ComboBox
	{
		private IwadFile cachedSelectedFile;

		/// <summary>
		/// Gets the collection of available IWAD files.
		/// </summary>
		public ObservableCollection<IwadFile> Files { get; }

		static IwadComboBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(IwadComboBox), new FrameworkPropertyMetadata(typeof(IwadComboBox)));
		}

		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		public IwadComboBox()
		{
			this.Files = new ObservableCollection<IwadFile>();

			Iwads.Updating += this.DispatchOnUpdating;
			Iwads.Updated  += this.DispatchOnUpdated;

			this.Dispatcher.ShutdownStarted += this.Release;

			void BindItemSourceWithFiles(object sender, RoutedEventArgs args)
			{
				this.ItemsSource = this.Files;

				this.Loaded -= BindItemSourceWithFiles;
			}

			this.Loaded += BindItemSourceWithFiles;

			this.IwadsOnUpdated();
		}

		/// <summary>
		/// Attempts to select the IWAD.
		/// </summary>
		/// <param name="fileName">Name of the file that represents the IWAD.</param>
		public void Select(string fileName)
		{
			int index = this.Files.IndexOf(x => x.FileName == fileName);

			if (index != -1)
			{
				this.SelectedIndex = index;
			}
			else
			{
				this.SelectedItem = null;
			}
		}

		private void IwadsOnUpdating()
		{
			// Memorize which file was selected.
			this.cachedSelectedFile = this.SelectedItem as IwadFile;
			this.SelectedItem       = null;

			// Clear current items.
			this.Files.Clear();
		}

		private void IwadsOnUpdated()
		{
			// Rebuild the item collection.
			Iwads.SupportedIwads.Where(x => x.Available).Foreach(x => this.Files.Add(x));

			// Reselect the item.
			if (this.cachedSelectedFile != null && this.cachedSelectedFile.Available)
			{
				this.SelectedItem = this.cachedSelectedFile;
			}
		}

		private void DispatchOnUpdating(object sender, EventArgs args)
		{
			this.Dispatcher.Invoke(this.IwadsOnUpdating);
		}

		private void DispatchOnUpdated(object sender, EventArgs args)
		{
			this.Dispatcher.Invoke(this.IwadsOnUpdated);
		}

		private void Release(object sender, EventArgs eventArgs)
		{
			Iwads.Updating                  -= this.DispatchOnUpdating;
			Iwads.Updated                   -= this.DispatchOnUpdated;
			this.Dispatcher.ShutdownStarted -= this.Release;
		}
	}
}