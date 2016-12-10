using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace Launcher
{
	/// <summary>
	/// Represents the description of the file.
	/// </summary>
	public class FileDesc : DependencyObject
	{
		#region Fields
		/// <summary>
		/// A dependency property that represents the value that indicates whether this file is
		/// selected.
		/// </summary>
		public static readonly DependencyProperty SelectedProperty =
			DependencyProperty.Register("Selected", typeof(bool), typeof(FileDesc),
										new PropertyMetadata(default(bool)));
		/// <summary>
		/// A dependency property that represents the value that indicates whether this file is at
		/// the top of the selection list.
		/// </summary>
		public static readonly DependencyProperty AtTopProperty =
			DependencyProperty.Register("AtTop", typeof(bool), typeof(FileDesc),
										new PropertyMetadata(default(bool)));
		/// <summary>
		/// A dependency property that represents the value that indicates whether this file is at
		/// the bottom of the selection list.
		/// </summary>
		public static readonly DependencyProperty AtBottomProperty =
			DependencyProperty.Register("AtBottom", typeof(bool), typeof(FileDesc),
										new PropertyMetadata(default(bool)));
		#endregion
		#region Properties
		/// <summary>
		/// Gets the full path to the file.
		/// </summary>
		public string FullPath { get; private set; }
		/// <summary>
		/// Gets the name of the directory where the file is contained.
		/// </summary>
		public string Directory { get; private set; }
		/// <summary>
		/// Gets the name of the file with extension.
		/// </summary>
		public string FileName { get; private set; }
		/// <summary>
		/// Gets or sets the value that indicates whether this file is selected.
		/// </summary>
		public bool Selected
		{
			get { return (bool)this.GetValue(SelectedProperty); }
			set { this.SetValue(SelectedProperty, value); }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this file is at the top of the selection
		/// list.
		/// </summary>
		public bool AtTop
		{
			get { return (bool)this.GetValue(AtTopProperty); }
			set { this.SetValue(AtTopProperty, value); }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this file is at the bottom of the selection
		/// list.
		/// </summary>
		public bool AtBottom
		{
			get { return (bool)this.GetValue(AtBottomProperty); }
			set { this.SetValue(AtBottomProperty, value); }
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		/// <param name="fullPath">Full path to the file.</param>
		public FileDesc(string fullPath)
		{
			this.FullPath = fullPath;
			this.FileName = Path.GetFileName(fullPath);
			this.Directory = Path.GetDirectoryName(fullPath);
			this.Selected = false;
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities
		#endregion
	}
}