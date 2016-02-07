using System;
using System.Linq;
using System.Windows.Controls;
using Launcher.Annotations;

namespace Launcher
{
	/// <summary>
	/// Represents the loadable file in the selection control.
	/// </summary>
	public class LoadableFile
	{
		#region Fields
		private string fileName;
		private Button selectDeselectButton;
		private bool selected;
		private TextBlock mainListText;
		private Button moveUpButton;
		private Button moveDownButton;
		private TextBlock selectionListText;
		#endregion
		#region Properties
		/// <summary>
		/// Path to the file.
		/// </summary>
		[NotNull]
		public string FileName
		{
			get { return this.fileName; }
			set { this.fileName = value; }
		}
		/// <summary>
		/// A button that, when clicked on when file is not selected, adds this file to the selection,
		/// otherwise when clicked when file is selected, removes it from selection.
		/// </summary>
		[NotNull]
		public Button SelectDeselectButton
		{
			get { return this.selectDeselectButton; }
			set { this.selectDeselectButton = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this file is selected.
		/// </summary>
		public bool Selected
		{
			get { return this.selected; }
			set { this.selected = value; }
		}
		/// <summary>
		/// A block of text in the main list of selectable files.
		/// </summary>
		[NotNull]
		public TextBlock MainListText
		{
			get { return this.mainListText; }
			set { this.mainListText = value; }
		}
		/// <summary>
		/// A button that, when clicked on, moves this file one row up within the selection.
		/// </summary>
		[CanBeNull]
		public Button MoveUpButton
		{
			get { return this.moveUpButton; }
			set { this.moveUpButton = value; }
		}
		/// <summary>
		/// A button that, when clicked on, moves this file one row down within the selection.
		/// </summary>
		[CanBeNull]
		public Button MoveDownButton
		{
			get { return this.moveDownButton; }
			set { this.moveDownButton = value; }
		}
		/// <summary>
		/// A block of text in the list of selected files.
		/// </summary>
		[CanBeNull]
		public TextBlock SelectionListText
		{
			get { return this.selectionListText; }
			set { this.selectionListText = value; }
		}
		#endregion
	}
}