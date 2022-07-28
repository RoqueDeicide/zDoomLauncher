using System.Windows;
using Launcher.Utilities;

namespace Launcher
{
	public partial class ExtraFilesSelectionBox
	{
		private void ToggleFileSelection(object sender, RoutedEventArgs routedEventArgs)
		{
			if (!(sender is FrameworkElement element))
			{
				return;
			}

			if (!(element.DataContext is FileDesc fileDesc))
			{
				Log.Warning("Unknown data context within the sender.");
				return;
			}

			if (fileDesc.Selected)
			{
				// Deselect.
				this.FileSelection.Remove(fileDesc);
			}
			else
			{
				// Select.
				this.FileSelection.Add(fileDesc);
			}

			// Invert selection status.
			fileDesc.Selected = !fileDesc.Selected;

			this.UpdateTopBottomSpinners();
		}

		private void MoveSelectedFileUp(object sender, RoutedEventArgs routedEventArgs)
		{
			var buttons = sender as SpinnerButtons;

			if (!(buttons?.DataContext is FileDesc fileDesc))
			{
				return;
			}

			int index = this.FileSelection.IndexOf(fileDesc);
			if (index == 0)
			{
				return;
			}

			(this.FileSelection[index - 1], this.FileSelection[index]) = (this.FileSelection[index], this.FileSelection[index - 1]);

			this.UpdateTopBottomSpinners();
		}

		private void MoveSelectedFileDown(object sender, RoutedEventArgs routedEventArgs)
		{
			var buttons = sender as SpinnerButtons;

			if (!(buttons?.DataContext is FileDesc fileDesc))
			{
				return;
			}

			int index = this.FileSelection.IndexOf(fileDesc);
			if (index == this.FileSelection.Count - 1)
			{
				return;
			}

			(this.FileSelection[index + 1], this.FileSelection[index]) = (this.FileSelection[index], this.FileSelection[index + 1]);

			this.UpdateTopBottomSpinners();
		}

		private void UpdateTopBottomSpinners()
		{
			for (int i = 0; i < this.FileSelection.Count; i++)
			{
				if (!(this.FileSelection[i] is FileDesc fileDesc))
				{
					continue;
				}

				fileDesc.AtTop    = false;
				fileDesc.AtBottom = false;

				if (i == 0)
				{
					fileDesc.AtTop = true;
				}

				if (i == this.FileSelection.Count - 1)
				{
					fileDesc.AtBottom = true;
				}
			}
		}
	}
}