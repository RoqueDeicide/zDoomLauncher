using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Launcher.Annotations;

namespace Launcher
{
	/// <summary>
	/// Represents an observable string.
	/// </summary>
	public class DirectoryName : INotifyPropertyChanged
	{
		private string path;
		private bool   exists;

		/// <summary>
		/// Gets or sets the path to the directory.
		/// </summary>
		public string Path
		{
			get => this.path;
			set
			{
				if (this.path != value)
				{
					this.path   = value;
					this.Exists = Directory.Exists(value);
					this.OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets the value that indicates whether this directory exists.
		/// </summary>
		public bool Exists
		{
			get => this.exists;
			set
			{
				if (this.exists != value)
				{
					this.exists = value;
					this.OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the zero-based index of this directory in the <see cref="ExtraFilesLookUp.Directories"/>.
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// Occurs when one of the properties changes its value.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = this.PropertyChanged;
			handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}