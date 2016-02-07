using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Launcher.Annotations;
using Launcher.Logging;

namespace Launcher
{
	public partial class ExtraFilesSelectionBox
	{
		private const string FontAddRemoveButtons = "Wingdings";
		private const string SelectSymbol = "\u006F";
		private const string DeselectSymbol = "\u00FE";
		// Creates a Button object that toggles file selection status.
		//
		//index -> Zero-based index of the extra file in the main list.
		//file  -> An object that consolidates all relevant data pertaining to the file.
		[NotNull]
		private Button CreateAddRemoveFileButton(int index, LoadableFile file)
		{
			TextBoxButton button = new TextBoxButton
			{
				FontFamily = new FontFamily(FontAddRemoveButtons),
				FontSize = 14,
				Content = SelectSymbol,
				Margin = new Thickness(0, 1, 0, 0),
				Tag = file
			};

			// Assign the button to the left-most column.
			Grid.SetColumn(button, 0);
			// Assign the button to the appropriate row.
			Grid.SetRow(button, index);

			button.Click += this.ToggleFileSelection;

			return button;
		}
		private void ToggleFileSelection(object sender, RoutedEventArgs routedEventArgs)
		{
			FrameworkElement button = sender as FrameworkElement;
			if (button == null)
			{
				return;
			}

			int index = Grid.GetRow(button);

			LoadableFile file = button.Tag as LoadableFile;
			if (file != null)
			{
				if (file.Selected)
				{
					// Deselect the file.

					// Find and remove the row.

					// Remove the elements.
					if (file.MoveUpButton == null)
					{
						// This is another case of the button getting pressed with the file not being selected.
						return;
					}

					int selectionRowIndex = Grid.GetRow(file.MoveUpButton);

					this.FilesSelectionGrid.Children.Remove(file.MoveUpButton);
					this.FilesSelectionGrid.Children.Remove(file.MoveDownButton);
					this.FilesSelectionGrid.Children.Remove(file.SelectionListText);

					var files = this.SelectedFiles;

					// routedEventArgs is only null, if this method was called not via Click event, but when
					// clearing the selection when selection list is already clear.
					if (files != null && routedEventArgs != null)
					{
						files.RemoveAt(selectionRowIndex);
					}

					// Move elements below the row one row up.
					for (int i = 0; i < this.FilesSelectionGrid.Children.Count; i++)
					{
						UIElement gridElement = this.FilesSelectionGrid.Children[i];
						int currentRowIndex = Grid.GetRow(gridElement);
						if (currentRowIndex > selectionRowIndex)
						{
							// Move elements up one row.
							Grid.SetRow(gridElement, currentRowIndex - 1);
						}
					}

					// Remove penultimate row.
					this.FilesSelectionGrid.RowDefinitions.RemoveAt(this.FilesSelectionGrid.RowDefinitions.Count - 2);

					Log.Message("Current row definitions count in the selection grid = {0}",
								this.FilesSelectionGrid.RowDefinitions.Count);

					file.SelectDeselectButton.Content = SelectSymbol;
				}
				else
				{
					// Select the file.
					this.AddFileSelectionRow(index, routedEventArgs != null);

					file.SelectDeselectButton.Content = DeselectSymbol;
				}

				file.Selected = !file.Selected;
			}
		}
		// Creates a Button object that, when pressed, moves selected file up.
		//
		//file -> An object that consolidates all relevant data pertaining to the file.
		//selectionIndex -> Zero-based index of the file in the selection list.
		[NotNull]
		private Button CreateMoveUpButton(LoadableFile file, int selectionIndex)
		{
			Button button = new Button
			{
				FontFamily = new FontFamily("Arial"),
				FontSize = 12,
				Content = "\u25b2",
				Margin = new Thickness(0),
				Tag = file
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

			var files = this.SelectedFiles;
			if (files == null) return;

			string temp = files[thisRowIndex];
			files[thisRowIndex] = files[upperRowIndex];
			files[upperRowIndex] = temp;
		}
		// Creates a Button object that, when pressed, moves selected file up.
		//
		//file -> An object that consolidates all relevant data pertaining to the file.
		//selectionIndex -> Zero-based index of the file in the selection list.
		[NotNull]
		private Button CreateMoveDownButton(LoadableFile file, int selectionIndex)
		{
			Button button = new Button
			{
				FontFamily = new FontFamily("Arial"),
				FontSize = 12,
				Content = "\u25bc",
				Margin = new Thickness(0),
				Tag = file
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

			var files = this.SelectedFiles;
			if (files == null) return;

			string temp = files[thisRowIndex];
			files[thisRowIndex] = files[lowerRowIndex];
			files[lowerRowIndex] = temp;
		}
	}
}