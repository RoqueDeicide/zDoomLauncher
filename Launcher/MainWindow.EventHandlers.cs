using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Launcher.Configs;
using ModernWpf.Controls.Primitives;

namespace Launcher
{
	public partial class MainWindow
	{
		private void ToggleOption(object sender, RoutedEventArgs e)
		{
			var item = sender as CheckBox;

			if (!(item?.Tag is IConvertible convertibleFlag))
			{
				return;
			}

			int flag = convertibleFlag.ToInt32(CultureInfo.InvariantCulture);
			if (item.IsChecked == true)
			{
				// Set the flag.
				this.config.DisableFlags |= (DisableOptions)flag;
			}
			else
			{
				// Remove the flag.
				this.config.DisableFlags &= (DisableOptions)~flag;
			}
		}

		private void SelectSaveGameFile(object sender, RoutedEventArgs e)
		{
			if (!this.config.SaveGamePath.IsNullOrWhiteSpace())
			{
				this.openSaveGameFileDialog.FileName = this.config.SaveGamePath;
			}

			if (this.openSaveGameFileDialog.ShowDialog() == true)
			{
				this.config.SaveGamePath = this.openSaveGameFileDialog.FileName;
			}
		}

		private void SelectDemoFile(object sender, RoutedEventArgs e)
		{
			if (!this.config.DemoPath.IsNullOrWhiteSpace())
			{
				this.openDemoFileDialog.FileName = this.config.DemoPath;
			}

			if (this.openDemoFileDialog.ShowDialog() == true)
			{
				this.config.DemoPath = this.openDemoFileDialog.FileName;
			}
		}

		private void SelectSaveDirectory(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(this.config.SaveDirectory))
			{
				this.openSaveFolderDialog.SelectedPath = this.config.SaveDirectory;
			}

			if (this.openSaveFolderDialog.ShowDialog() == true)
			{
				string selectedPath = this.openSaveFolderDialog.SelectedPath;
				string gamePath     = this.zDoomFolder;

				this.config.SaveDirectory = PathUtils.ToRelativePath(selectedPath, gamePath);
			}
		}

		private void SelectConfigPath(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(this.config.ConfigFile))
			{
				this.openConfigFileDialog.FileName = this.config.ConfigFile;
			}

			if (this.openConfigFileDialog.ShowDialog() == true)
			{
				this.config.ConfigFile = PathUtils.ToRelativePath(this.openConfigFileDialog.FileName,
																  this.zDoomFolder);
			}
		}

		private void ShowAttachedFlyout(object sender, RoutedEventArgs e)
		{
			if (sender is FrameworkElement element)
			{
				FlyoutBase.ShowAttachedFlyout(element);
			}
		}

		private void HideAttachedFlyout(object sender, RoutedEventArgs e)
		{
			if (sender is FrameworkElement element)
			{
				var flyout = FlyoutBase.GetAttachedFlyout(element);

				flyout.Hide();
			}
		}
	}
}