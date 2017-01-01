using System;
using System.Windows;

namespace Launcher
{
	public partial class MainWindow
	{
		#region Fields
		private Point? windowPosition;
		private Size? windowSize;

		private Point defaultWindowPosition;
		private Size defaultWindowSize;

		private bool maximized;
		#endregion
		#region Utilities
		private void DealWithWindowPositionSize(object sender, RoutedEventArgs e)
		{
			// Store design-time dimensions and position as defaults.
			this.defaultWindowPosition = new Point(this.Left, this.Top);
			this.defaultWindowSize = new Size(this.Width, this.Height);

			// Assign new dimensions and position if they were specified in the app configuration.
			if (this.windowPosition != null)
			{
				this.Left = this.windowPosition.Value.X;
				this.Top = this.windowPosition.Value.Y;
			}
			if (this.windowSize != null)
			{
				this.Width = this.windowSize.Value.Width;
				this.Height = this.windowSize.Value.Height;
			}

			// Maximize the window, if necessary.
			if (this.maximized)
			{
				this.WindowState = WindowState.Maximized;
			}
		}

		private void SaveWindowPosition(object sender, RoutedEventArgs e)
		{
			this.windowPosition = new Point(this.Left, this.Top);
			this.maximized = this.WindowState == WindowState.Maximized;
		}
		private void SaveWindowDimensions(object sender, RoutedEventArgs e)
		{
			this.windowSize = new Size(this.Width, this.Height);
			this.maximized = this.WindowState == WindowState.Maximized;
		}
		private void ResetWindowPosition(object sender, RoutedEventArgs e)
		{
			this.windowPosition = null;

			this.Left = this.defaultWindowPosition.X;
			this.Top = this.defaultWindowPosition.Y;

			this.maximized = false;
			this.WindowState = WindowState.Normal;
		}
		private void ResetWindowDimensions(object sender, RoutedEventArgs e)
		{
			this.windowSize = null;

			this.Width = this.defaultWindowSize.Width;
			this.Height = this.defaultWindowSize.Height;

			this.maximized = false;
			this.WindowState = WindowState.Normal;
		}
		#endregion
	}
}