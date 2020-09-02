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
			if (sender is CheckBox checkBox && checkBox.DataContext is DisableOptionsUi option)
			{

				if (checkBox.IsChecked == true)
				{
					// Set the flag.
					this.Config.DisableFlags |= option.DisableOptions;
				}
				else
				{
					// Remove the flag.
					this.Config.DisableFlags &= ~option.DisableOptions;
				}
			}
		}

		private void SelectSaveGameFile(object sender, RoutedEventArgs e)
		{
			if (!this.Config.SaveGamePath.IsNullOrWhiteSpace())
			{
				this.openSaveGameFileDialog.FileName = this.Config.SaveGamePath;
			}

			if (this.openSaveGameFileDialog.ShowDialog() == true)
			{
				this.Config.SaveGamePath = this.openSaveGameFileDialog.FileName;
			}
		}

		private void SelectDemoFile(object sender, RoutedEventArgs e)
		{
			if (!this.Config.DemoPath.IsNullOrWhiteSpace())
			{
				this.openDemoFileDialog.FileName = this.Config.DemoPath;
			}

			if (this.openDemoFileDialog.ShowDialog() == true)
			{
				this.Config.DemoPath = this.openDemoFileDialog.FileName;
			}
		}

		private void SelectSaveDirectory(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(this.Config.SaveDirectory))
			{
				this.openSaveFolderDialog.SelectedPath = this.Config.SaveDirectory;
			}

			if (this.openSaveFolderDialog.ShowDialog() == true)
			{
				string selectedPath = this.openSaveFolderDialog.SelectedPath;
				string gamePath     = this.zDoomFolder;

				this.Config.SaveDirectory = PathUtils.ToRelativePath(selectedPath, gamePath);
			}
		}

		private void SelectConfigPath(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(this.Config.ConfigFile))
			{
				this.openConfigFileDialog.FileName = this.Config.ConfigFile;
			}

			if (this.openConfigFileDialog.ShowDialog() == true)
			{
				this.Config.ConfigFile = PathUtils.ToRelativePath(this.openConfigFileDialog.FileName,
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