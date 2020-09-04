using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;

namespace Launcher
{
	public partial class MainWindow
	{

		private void LaunchGameButtonClick(object sender, RoutedEventArgs e)
		{
			this.Launch(this.currentExeFile);
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
				FlyoutBase flyout = FlyoutBase.GetAttachedFlyout(element);

				flyout.Hide();
			}
		}

		private void OpenAboutWindowClick(object sender, RoutedEventArgs e)
		{
			new AboutWindow().Show();
		}

		private void OpenHelpWindowClick(object sender, RoutedEventArgs e)
		{
			new HelpWindow
			{
				WindowStartupLocation = WindowStartupLocation.CenterScreen
			}.Show();
		}

		private void OpenDirectoriesWindowClick(object sender, RoutedEventArgs e)
		{
			new Directories().ShowDialog();
		}

		private void RefreshButtonClick(object sender, RoutedEventArgs e)
		{
			this.RefreshExeFiles();
			this.RefreshExtraFiles();
		}

		private void MainWindowClosing(object sender, CancelEventArgs e)
		{
			this.SaveAppConfiguration();
		}

		private void GamePathButtonClick(object sender, RoutedEventArgs e)
		{
			this.SelectZDoomInstallationFolder(true);
		}

		private void CreateNewConfigurationClick(object sender, RoutedEventArgs e)
		{
			this.saveConfigurationDialog.FileName = "";

			if (this.saveConfigurationDialog.ShowDialog(this) == true)
			{
				string extraOptions = this.Config.ExtraOptions;
				int    width        = this.Config.Width;
				int    height       = this.Config.Height;

				this.Config.Reset();
				this.CurrentConfigFile = this.saveConfigurationDialog.FileName;

				this.Config.ExtraOptions = extraOptions;
				this.Config.Width        = width;
				this.Config.Height       = height;
			}
		}

		private void SaveConfigurationClick(object sender, RoutedEventArgs e)
		{
			this.Config.Save(this.CurrentConfigFile, this.zDoomFolder);
		}

		private void SaveConfigurationAsClick(object sender, RoutedEventArgs e)
		{
			this.saveConfigurationDialog.FileName = this.CurrentConfigFile;

			if (this.saveConfigurationDialog.ShowDialog(this) == true)
			{
				this.CurrentConfigFile = this.saveConfigurationDialog.FileName;
				this.Config.Save(this.CurrentConfigFile, this.zDoomFolder);
			}
		}

		private void OpenConfigurationClick(object sender, RoutedEventArgs e)
		{
			if (this.openConfigurationDialog.ShowDialog(this) == true)
			{
				this.LoadConfiguration(this.openConfigurationDialog.FileName);
			}
		}

		private void UpdateTestCommandLineFlyoutOpening(object sender, object e)
		{
			(string path, string args) = this.CommandLine;

			string commandLine = $"{path} {args}";

			AppBarButton button = this.CommandLineAppButton;
			button.Resources["CommandLineText"] = commandLine;

			if (commandLine.Length > CommandLineMaxLength)
			{
				button.Resources["FlyoutColor"]  = new SolidColorBrush(Colors.Red);
				button.Resources["FlyoutSymbol"] = Symbol.Cancel;
			}
			else
			{
				button.Resources["FlyoutColor"]  = new SolidColorBrush(Colors.Green);
				button.Resources["FlyoutSymbol"] = Symbol.Accept;
			}
		}

		private void ExeFileSelected(object sender, SelectionChangedEventArgs e)
		{
			// Update the field.
			if (this.ExeFileNameComboBox.SelectedItem is ComboBoxItem selectedItem)
			{
				this.currentExeFile = selectedItem.Content as string ?? "";

				this.UpdateLaunchIcon();
			}
		}
	}
}