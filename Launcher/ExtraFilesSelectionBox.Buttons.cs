using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Launcher.Logging;

namespace Launcher
{
	public partial class ExtraFilesSelectionBox
	{
		private const string FontAddRemoveButtons = "Webdings";
		// Creates a Button object that, when pressed, add an extra file to the selection.
		//
		// index -> Zero-based index of the extra file in the main list.
		private Button CreateAddFileButton(int index)
		{
			Button button = new Button
			{
				FontFamily = new FontFamily(FontAddRemoveButtons),
				FontSize = 12,
				Content = "\u0061",
				Margin = new Thickness(0)
			};

			// Assign the button to the left-most column.
			Grid.SetColumn(button, 0);
			// Assign the button to the appropriate row.
			Grid.SetRow(button, index);

			button.Click += this.AddFileToSelection;

			return button;
		}
		private void AddFileToSelection(object sender, RoutedEventArgs routedEventArgs)
		{
			Button button = sender as Button;
			if (button == null)
			{
				return;
			}

			int index = Grid.GetRow(button);

			// Add the file to the selection list.
			this.AddFileSelectionRow(index, routedEventArgs != null);

			Button removeButton = button.Tag as Button;
			if (removeButton != null)
			{
				// Reveal the button.
				removeButton.Visibility = System.Windows.Visibility.Visible;
			}

			// Hide the "add file" button.
			button.Visibility = System.Windows.Visibility.Hidden;
		}
		// Creates a Button object that, when pressed, removes an extra file from the selection.
		//
		// index -> Zero-based index of the extra file in the main list.
		private Button CreateRemoveFileButton(int index)
		{
			Button button = new Button
			{
				FontFamily = new FontFamily(FontAddRemoveButtons),
				FontSize = 12,
				Content = "\u0072",
				Margin = new Thickness(0),
				Visibility = System.Windows.Visibility.Hidden
			};

			// Assign the button to the second column from the left.
			Grid.SetColumn(button, 1);
			// Assign the button to the appropriate row.
			Grid.SetRow(button, index);

			button.Click += this.RemoveFileFromSelection;

			return button;
		}
		private void RemoveFileFromSelection(object sender, RoutedEventArgs routedEventArgs)
		{
			Button button = sender as Button;
			if (button == null)
			{
				return;
			}

			if (button.Visibility == System.Windows.Visibility.Hidden)
			{
				// Just in case.
				return;
			}

			int index = Grid.GetRow(button);

			// Hide the button.
			button.Visibility = System.Windows.Visibility.Hidden;

			// Unhide the "add" button.
			Button addButton = button.Tag as Button;
			if (addButton != null)
			{
				addButton.Visibility = System.Windows.Visibility.Visible;
			}

			// Find and remove the row.

			// Remove the elements.
			var buttonsInTheRow = from UIElement child in this.FilesSelectionGrid.Children
								  where child is Button && ((Button)child).Tag.Equals(index)
								  select child as Button;

			Button moveButton = buttonsInTheRow.FirstOrDefault();

			if (moveButton == null)
			{
				// This is another case of the button getting pressed with the file not being selected.
				return;
			}

			int selectionRowIndex = Grid.GetRow(moveButton);

			this.SelectedFiles.RemoveAt(selectionRowIndex);

			// Remove elements from the row and move everything below the row up.
			for (int i = 0; i < this.FilesSelectionGrid.Children.Count; i++)
			{
				UIElement element = this.FilesSelectionGrid.Children[i];
				int currentRowIndex = Grid.GetRow(element);
				if (currentRowIndex == selectionRowIndex)
				{
					// Remove the element on the row.
					this.FilesSelectionGrid.Children.RemoveAt(i--);
				}
				else if (currentRowIndex > selectionRowIndex)
				{
					// Move elements up one row.
					Grid.SetRow(element, currentRowIndex - 1);
				}
			}

			// Remove penultimate row.
			this.FilesSelectionGrid.RowDefinitions.RemoveAt(this.FilesSelectionGrid.RowDefinitions.Count - 2);

			Log.Message("Current row definitions count in the selection grid = {0}",
						this.FilesSelectionGrid.RowDefinitions.Count);
		}
		// Creates a Button object that, when pressed, moves selected file up.
		//
		// index          -> Zero-based index of the file in the main list, used in removal of the selection.
		// selectionIndex -> Zero-based index of the file in the selection list.
		private Button CreateMoveUpButton(int index, int selectionIndex)
		{
			Button button = new Button
			{
				FontFamily = new FontFamily("Arial"),
				FontSize = 12,
				Content = "\u25b2",
				Margin = new Thickness(0),
				Tag = index
			};

			Grid.SetRow(button, selectionIndex);
			Grid.SetColumn(button, 0);

			button.Click += this.MoveSelectedFileUp;

			return button;
		}
		private void MoveSelectedFileUp(object sender, RoutedEventArgs routedEventArgs)
		{
			Button button = sender as Button;
			if (button == null)
			{
				return;
			}

			int thisRowIndex = Grid.GetRow(button);
			int upperRowIndex = thisRowIndex - 1;

			if (upperRowIndex < 0)
			{
				// Can't move up.
				return;
			}

			var thisRowEnum = from UIElement child in this.FilesSelectionGrid.Children
							  where Grid.GetRow(child) == thisRowIndex
							  select child;
			UIElement[] thisRowElements = thisRowEnum.ToArray();

			var upperRowEnum = from UIElement child in this.FilesSelectionGrid.Children
							  where Grid.GetRow(child) == upperRowIndex
							  select child;
			UIElement[] upperRowElements = upperRowEnum.ToArray();

			foreach (UIElement element in upperRowElements)
			{
				Grid.SetRow(element, thisRowIndex);
			}
			foreach (UIElement element in thisRowElements)
			{
				Grid.SetRow(element, upperRowIndex);
			}

			string temp = this.SelectedFiles[thisRowIndex];
			this.SelectedFiles[thisRowIndex] = this.SelectedFiles[upperRowIndex];
			this.SelectedFiles[upperRowIndex] = temp;
		}
		// Creates a Button object that, when pressed, moves selected file up.
		//
		// index          -> Zero-based index of the file in the main list, used in removal of the selection.
		// selectionIndex -> Zero-based index of the file in the selection list.
		private Button CreateMoveDownButton(int index, int selectionIndex)
		{
			Button button = new Button
			{
				FontFamily = new FontFamily("Arial"),
				FontSize = 12,
				Content = "\u25bc",
				Margin = new Thickness(0),
				Tag = index
			};

			Grid.SetRow(button, selectionIndex);
			Grid.SetColumn(button, 1);

			button.Click += this.MoveSelectedFileDown;

			return button;
		}
		private void MoveSelectedFileDown(object sender, RoutedEventArgs routedEventArgs)
		{
			Button button = sender as Button;
			if (button == null)
			{
				return;
			}

			int thisRowIndex = Grid.GetRow(button);
			int lowerRowIndex = thisRowIndex + 1;

			if (lowerRowIndex == this.FilesSelectionGrid.RowDefinitions.Count - 1)
			{
				// Can't move down.
				return;
			}

			var thisRowEnum = from UIElement child in this.FilesSelectionGrid.Children
							  where Grid.GetRow(child) == thisRowIndex
							  select child;
			UIElement[] thisRowElements = thisRowEnum.ToArray();

			var lowerRowEnum = from UIElement child in this.FilesSelectionGrid.Children
							   where Grid.GetRow(child) == lowerRowIndex
							   select child;
			UIElement[] lowerRowElements = lowerRowEnum.ToArray();

			foreach (UIElement element in lowerRowElements)
			{
				Grid.SetRow(element, thisRowIndex);
			}
			foreach (UIElement element in thisRowElements)
			{
				Grid.SetRow(element, lowerRowIndex);
			}

			string temp = this.SelectedFiles[thisRowIndex];
			this.SelectedFiles[thisRowIndex] = this.SelectedFiles[lowerRowIndex];
			this.SelectedFiles[lowerRowIndex] = temp;
		}
	}
}