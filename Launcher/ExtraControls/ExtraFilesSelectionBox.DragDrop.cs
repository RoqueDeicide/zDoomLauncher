using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Launcher
{
	public partial class ExtraFilesSelectionBox
	{
		// A point at which the mouse left button is clicked. Used to prevent the drag'n'drop operation from happening
		// when the user simply clicks on the list box for selected files.
		private Point dragStartPoint;

		private void SaveCurrentMousePosition(object sender, MouseButtonEventArgs e)
		{
			this.dragStartPoint = e.GetPosition(null);
		}

		private void AttemptInitiateDragDrop(object sender, MouseEventArgs e)
		{
			// Check the new mouse position against the saved one.
			Point  currentPosition = e.GetPosition(null);
			Vector delta           = currentPosition - this.dragStartPoint;

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
			if (!(sender is ListBoxItem listBoxItem))
			{
				return;
			}

			object source = e.Data.GetData(typeof(FileDesc));
			object target = listBoxItem.DataContext;

			int sourceIndex = this.FileSelection.IndexOf(source);
			int targetIndex = this.FileSelection.IndexOf(target);

			var targetFile = (FileDesc)target;
			if (targetFile.DragOverBottom == Visibility.Visible)
			{
				// Insert below the targeted item, rather then above.
				targetIndex++;
			}

			this.MoveSelectedItem(sourceIndex, targetIndex);

			targetFile.DragOverTop    = Visibility.Hidden;
			targetFile.DragOverBottom = Visibility.Hidden;
		}

		private void MoveSelectedItem(int sourceIndex, int destinationIndex)
		{
			if (sourceIndex == destinationIndex)
			{
				return;
			}

			object source = this.FileSelection[sourceIndex];

			if (sourceIndex < destinationIndex)
			{
				// Removal of the item from the list causes the destination index to point at the item 1 further away
				// from the start.
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

		private void UpdateDropLocationIndication(object sender, DragEventArgs e)
		{
			var item = sender as ListBoxItem;

			if (!(item?.DataContext is FileDesc file))
			{
				return;
			}

			var cp = item.FindVisualChild<ContentPresenter>();
			Debug.Assert(cp != null, "cp != null");

			// Check the coordinates of the mouse relative to the ContentPresenter of the list box item.
			Point position   = e.GetPosition(cp);
			int   halfHeight = (int)item.ActualHeight / 2; // 0-9: upper half, 10-19: lower half.

			if (position.Y < halfHeight)
			{
				file.DragOverTop    = Visibility.Visible;
				file.DragOverBottom = Visibility.Hidden;
			}
			else
			{
				file.DragOverBottom = Visibility.Visible;
				file.DragOverTop    = Visibility.Hidden;
			}
		}

		private void TurnOffDropLocationIndication(object sender, DragEventArgs e)
		{
			var item = sender as ListBoxItem;

			if (!(item?.DataContext is FileDesc file))
			{
				return;
			}

			file.DragOverTop    = Visibility.Hidden;
			file.DragOverBottom = Visibility.Hidden;
		}
	}
}