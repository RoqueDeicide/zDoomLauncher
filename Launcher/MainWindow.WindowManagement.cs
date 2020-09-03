using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ModernWpf.Controls;
using Point = System.Windows.Point;
using Size = System.Windows.Size;
using SdIcon = System.Drawing.Icon;

namespace Launcher
{
	public partial class MainWindow
	{
		#region Fields

		private Point? windowPosition;
		private Size?  windowSize;

		private Point defaultWindowPosition;
		private Size  defaultWindowSize;

		private bool maximized;

		#endregion

		#region Utilities

		private void DealWithWindowPositionSize(object sender, RoutedEventArgs e)
		{
			// Store design-time dimensions and position as defaults.
			this.defaultWindowPosition = new Point(this.Left, this.Top);
			this.defaultWindowSize     = new Size(this.Width, this.Height);

			// Assign new dimensions and position if they were specified in the app configuration.
			if (this.windowPosition != null)
			{
				this.Left = this.windowPosition.Value.X;
				this.Top  = this.windowPosition.Value.Y;
			}

			if (this.windowSize != null)
			{
				this.Width  = this.windowSize.Value.Width;
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
			this.maximized      = this.WindowState == WindowState.Maximized;
		}

		private void SaveWindowDimensions(object sender, RoutedEventArgs e)
		{
			this.windowSize = new Size(this.Width, this.Height);
			this.maximized  = this.WindowState == WindowState.Maximized;
		}

		private void ResetWindowPosition(object sender, RoutedEventArgs e)
		{
			this.windowPosition = null;

			this.Left = this.defaultWindowPosition.X;
			this.Top  = this.defaultWindowPosition.Y;

			this.maximized   = false;
			this.WindowState = WindowState.Normal;
		}

		private void ResetWindowDimensions(object sender, RoutedEventArgs e)
		{
			this.windowSize = null;

			this.Width  = this.defaultWindowSize.Width;
			this.Height = this.defaultWindowSize.Height;

			this.maximized   = false;
			this.WindowState = WindowState.Normal;
		}

		private async void ShowAccentColorPickerDialog(object sender, RoutedEventArgs e)
		{
			this.accentColorPicker ??= new AccentColorPicker();
			await this.accentColorPicker.ShowAsync();
		}

		#endregion

		private void UpdateLaunchIcon()
		{
			// Detach old image.
			if (this.LaunchAppButton.Content is Image)
			{
				this.PlayIconImage.Source = null;
			}

			string exePath = Path.Combine(this.zDoomFolder, this.currentExeFile);

			var icon = SdIcon.ExtractAssociatedIcon(exePath);
			if (icon == null)
			{
				this.LaunchAppButton.Content = new SymbolIcon(Symbol.Play);
			}
			else
			{
				// Cache the icon.
				const string cacheFileName = "cache.ico";
				string       cacheFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cacheFileName);

				using (var stream = new FileStream(cacheFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					icon.Save(stream);
				}

				var bitmap = new BitmapImage();
				bitmap.BeginInit();
				bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
				bitmap.CacheOption   = BitmapCacheOption.OnLoad;
				bitmap.UriSource     = new Uri(cacheFilePath);
				bitmap.EndInit();

				this.PlayIconImage.Source    = bitmap;
				this.LaunchAppButton.Content = this.PlayIconImage;
			}
		}
	}
}