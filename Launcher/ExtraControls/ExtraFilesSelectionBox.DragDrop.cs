using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Launcher
{
	public partial class ExtraFilesSelectionBox
	{
		// A point at which the mouse left button is clicked. Used to prevent the drag'n'drop
		// operation from happening when the user simply clicks on the list box for selected files.
		private Point dragStartPoint;
		private void SaveCurrentMousePosition(object sender, MouseButtonEventArgs e)
		{
			this.dragStartPoint = e.GetPosition(null);
		}
		private void AttemptInitiateDragDrop(object sender, MouseEventArgs e)
		{
			// Check the new mouse position against the saved one.
			Point currentPosition = e.GetPosition(null);
			Vector delta = currentPosition - this.dragStartPoint;

			// See, if the mouse went sufficiently far away from the point of the click.
			if (e.LeftButton == MouseButtonState.Released ||
				Math.Abs(delta.X) < SystemParameters.MinimumHorizontalDragDistance &&
				Math.Abs(delta.Y) < SystemParameters.MinimumVerticalDragDistance)
			{
				return;
			}

			// Initiate the drag'n'drop.
			var listBox = sender as ListBox;
			Debug.Assert(listBox != null);

			var item = (e.OriginalSource as DependencyObject).FindVisualParent<ListBoxItem>();
			Debug.Assert(item != null, "item != null");

			DragDrop.DoDragDrop(listBox, item.DataContext, DragDropEffects.Move);
		}
		private void MoveDraggedItem(object sender, DragEventArgs e)
		{
			ListBoxItem listBoxItem = sender as ListBoxItem;

			if (listBoxItem == null)
			{
				return;
			}

			var source = e.Data.GetData(typeof(FileDesc));
			var target = listBoxItem.DataContext;

			this.MoveSelectedItem(this.FileSelection.IndexOf(source), this.FileSelection.IndexOf(target));

			{
			}


		}

		private void MoveSelectedItem(int sourceIndex, int destinationIndex)
		{
			if (sourceIndex == destinationIndex)
			{
				return;
			}

			var source = this.FileSelection[sourceIndex];

			if (sourceIndex < destinationIndex)
			{
				// Removal of the item from the list causes the destination index to point at the
				// item 1 further away from the start.
				this.FileSelection.RemoveAt(sourceIndex);
				this.FileSelection.Insert(destinationIndex - 1, source);
			}
			else
			{
				this.FileSelection.RemoveAt(sourceIndex);
				this.FileSelection.Insert(destinationIndex, source);
			}

			this.UpdateTopBottomSpinners();
		}
	}
}