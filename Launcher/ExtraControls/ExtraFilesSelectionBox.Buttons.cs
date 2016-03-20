using System;
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
		private void ToggleFileSelection(object sender, RoutedEventArgs routedEventArgs)
		{
			FrameworkElement element = sender as FrameworkElement;
			if (element == null)
			{
				return;
			}

			FileDesc fileDesc = element.DataContext as FileDesc;
			if (fileDesc == null)
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
			SpinnerButtons buttons = sender as SpinnerButtons;

			FileDesc fileDesc = buttons?.DataContext as FileDesc;
			if (fileDesc == null)
			{
				return;
			}

			int index = this.FileSelection.IndexOf(fileDesc);
			if (index == 0)
			{
				return;
			}

			var temp = this.FileSelection[index - 1];
			this.FileSelection[index - 1] = this.FileSelection[index];
			this.FileSelection[index] = temp;

			this.UpdateTopBottomSpinners();
		}
		private void MoveSelectedFileDown(object sender, RoutedEventArgs routedEventArgs)
		{
			SpinnerButtons buttons = sender as SpinnerButtons;

			FileDesc fileDesc = buttons?.DataContext as FileDesc;
			if (fileDesc == null)
			{
				return;
			}

			int index = this.FileSelection.IndexOf(fileDesc);
			if (index == this.FileSelection.Count - 1)
			{
				return;
			}

			var temp = this.FileSelection[index + 1];
			this.FileSelection[index + 1] = this.FileSelection[index];
			this.FileSelection[index] = temp;

			this.UpdateTopBottomSpinners();
		}

		private void UpdateTopBottomSpinners()
		{
			for (int i = 0; i < this.FileSelection.Count; i++)
			{
				FileDesc fileDesc = this.FileSelection[i] as FileDesc;
				if (fileDesc == null)
				{
					continue;
				}

				fileDesc.AtTop = false;
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