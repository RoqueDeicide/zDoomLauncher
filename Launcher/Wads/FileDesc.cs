﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Launcher
{
	/// <summary>
	/// Represents the description of the file.
	/// </summary>
	public class FileDesc : DependencyObject, IComparable<FileDesc>, IComparable
	{
		#region Fields

		/// <summary>
		/// A dependency property that represents the value that indicates whether this file is selected.
		/// </summary>
		public static readonly DependencyProperty SelectedProperty =
			DependencyProperty.Register(nameof(Selected), typeof(bool), typeof(FileDesc), new PropertyMetadata(default(bool)));

		/// <summary>
		/// A dependency property that represents the value that indicates whether this file is at the top of the selection list.
		/// </summary>
		public static readonly DependencyProperty AtTopProperty =
			DependencyProperty.Register(nameof(AtTop), typeof(bool), typeof(FileDesc), new PropertyMetadata(default(bool)));

		/// <summary>
		/// A dependency property that represents the value that indicates whether this file is at the bottom of the selection list.
		/// </summary>
		public static readonly DependencyProperty AtBottomProperty =
			DependencyProperty.Register(nameof(AtBottom), typeof(bool), typeof(FileDesc), new PropertyMetadata(default(bool)));

		/// <summary>
		/// Dependency property that represents the value that indicates the visibility of an indicator that shows whether a target of drag'n'drop operation is
		/// over the top half of the selected file list box item.
		/// </summary>
		public static readonly DependencyProperty DragOverTopProperty =
			DependencyProperty.Register(nameof(DragOverTop), typeof(Visibility), typeof(FileDesc), new PropertyMetadata(Visibility.Hidden));

		/// <summary>
		/// Dependency property that represents the value that indicates the visibility of an indicator that shows whether a target of drag'n'drop operation is
		/// over the bottom half of the selected file list box item.
		/// </summary>
		public static readonly DependencyProperty DragOverBottomProperty =
			DependencyProperty.Register(nameof(DragOverBottom), typeof(Visibility), typeof(FileDesc), new PropertyMetadata(Visibility.Hidden));

		#endregion

		#region Properties

		/// <summary>
		/// Gets the full path to the file.
		/// </summary>
		public string FullPath { get; }

		/// <summary>
		/// Gets the name of the directory where the file is contained.
		/// </summary>
		public string Directory { get; }

		/// <summary>
		/// Gets the name of the file with extension.
		/// </summary>
		public string FileName { get; }

		/// <summary>
		/// Gets or sets the value that indicates whether this file is selected.
		/// </summary>
		public bool Selected
		{
			get => (bool)this.GetValue(SelectedProperty);
			set => this.SetValue(SelectedProperty, value);
		}

		/// <summary>
		/// Gets or sets the value that indicates whether this file is at the top of the selection list.
		/// </summary>
		public bool AtTop
		{
			get => (bool)this.GetValue(AtTopProperty);
			set => this.SetValue(AtTopProperty, value);
		}

		/// <summary>
		/// Gets or sets the value that indicates whether this file is at the bottom of the selection list.
		/// </summary>
		public bool AtBottom
		{
			get => (bool)this.GetValue(AtBottomProperty);
			set => this.SetValue(AtBottomProperty, value);
		}

		/// <summary>
		/// Gets or sets the value that indicates the visibility of an indicator that shows whether a target of drag'n'drop operation is over the top half of
		/// the selected file list box item.
		/// </summary>
		public Visibility DragOverTop
		{
			get => (Visibility)this.GetValue(DragOverTopProperty);
			set => this.SetValue(DragOverTopProperty, value);
		}

		/// <summary>
		/// Gets or sets the value that indicates the visibility of an indicator that shows whether a target of drag'n'drop operation is over the bottom half of
		/// the selected file list box item.
		/// </summary>
		public Visibility DragOverBottom
		{
			get => (Visibility)this.GetValue(DragOverBottomProperty);
			set => this.SetValue(DragOverBottomProperty, value);
		}

		#endregion

		#region Construction

		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		/// <param name="fullPath">Full path to the file.</param>
		public FileDesc(string fullPath)
		{
			this.FullPath  = fullPath;
			this.FileName  = Path.GetFileName(fullPath);
			this.Directory = Path.GetDirectoryName(fullPath);
			this.Selected  = false;
		}

		#endregion

		#region Comparison

		/// <summary>
		/// Determines position of this object relative to another in a sequence that is sorted in ascending order.
		/// </summary>
		/// <param name="other">Another object to compare this one against.</param>
		/// <returns>
		/// An integer that, if negative, indicates that this object must precede another, if positive, indicates that this object must succeed another,
		/// otherwise, that objects are equal.
		/// </returns>
		public int CompareTo(FileDesc other)
		{
			if (ReferenceEquals(this, other)) return 0;
			return other is null ? 1 : string.Compare(this.FullPath, other.FullPath, StringComparison.InvariantCulture);
		}

		/// <summary>
		/// Determines position of this object relative to another in a sequence that is sorted in ascending order.
		/// </summary>
		/// <param name="obj">Another object to compare this one against.</param>
		/// <returns>
		/// An integer that, if negative, indicates that this object must precede another, if positive, indicates that this object must succeed another,
		/// otherwise, that objects are equal.
		/// </returns>
		/// <exception cref="ArgumentException">Object must be of type <see cref="FileDesc"/>.</exception>
		public int CompareTo(object obj)
		{
			if (obj is null) return 1;
			if (ReferenceEquals(this, obj)) return 0;
			return obj is FileDesc other ? this.CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(FileDesc)}");
		}

		/// <summary>
		/// Indicates whether left object of type <see cref="FileDesc"/> must precede the one on the right.
		/// </summary>
		/// <param name="left"> Left object</param>
		/// <param name="right">Right object.</param>
		/// <returns><c>true</c>, if left object of type <see cref="FileDesc"/> must precede the one on the right, otherwise <c>false</c>.</returns>
		public static bool operator <(FileDesc left, FileDesc right)
		{
			return Comparer<FileDesc>.Default.Compare(left, right) < 0;
		}

		/// <summary>
		/// Indicates whether left object of type <see cref="FileDesc"/> must succeed the one on the right.
		/// </summary>
		/// <param name="left"> Left object</param>
		/// <param name="right">Right object.</param>
		/// <returns><c>true</c>, if left object of type <see cref="FileDesc"/> must succeed the one on the right, otherwise <c>false</c>.</returns>
		public static bool operator >(FileDesc left, FileDesc right)
		{
			return Comparer<FileDesc>.Default.Compare(left, right) > 0;
		}

		/// <summary>
		/// Indicates whether left object of type <see cref="FileDesc"/> must not succeed the one on the right.
		/// </summary>
		/// <param name="left"> Left object</param>
		/// <param name="right">Right object.</param>
		/// <returns><c>true</c>, if left object of type <see cref="FileDesc"/> must not succeed the one on the right, otherwise <c>false</c>.</returns>
		public static bool operator <=(FileDesc left, FileDesc right)
		{
			return Comparer<FileDesc>.Default.Compare(left, right) <= 0;
		}

		/// <summary>
		/// Indicates whether left object of type <see cref="FileDesc"/> must not precede the one on the right.
		/// </summary>
		/// <param name="left"> Left object</param>
		/// <param name="right">Right object.</param>
		/// <returns><c>true</c>, if left object of type <see cref="FileDesc"/> must not precede the one on the right, otherwise <c>false</c>.</returns>
		public static bool operator >=(FileDesc left, FileDesc right)
		{
			return Comparer<FileDesc>.Default.Compare(left, right) >= 0;
		}

		#endregion
	}
}