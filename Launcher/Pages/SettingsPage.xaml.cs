using System;
using System.IO;
using System.Linq;
using System.Windows;
using Windows.UI.ViewManagement;
using Launcher.Utilities;
using ModernWpf;
using ModernWpf.Controls.Primitives;
using Ookii.Dialogs.Wpf;
using Color = Windows.UI.Color;
using Colors = Windows.UI.Colors;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for SettingsPage.xaml
	/// </summary>
	public partial class SettingsPage
	{
		public SettingsPage()
		{
			this.InitializeComponent();

			this.SetupThemeMenuItems();
		}

		private void SetupThemeMenuItems()
		{
			static void SetTheme(ApplicationTheme? theme)
			{
				ThemeManager.Current.ApplicationTheme = theme;
			}

			this.LightThemeRadio.Click += (sender, args) => SetTheme(ApplicationTheme.Light);
			this.DarkThemeRadio.Click  += (sender, args) => SetTheme(ApplicationTheme.Dark);

			if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 10)
			{
				// OS doesn't support theme enforcement.

				this.SystemSettingThemeRadio.IsEnabled = false;
			}
			else
			{
				void UpdateOSThemeMenuItem()
				{
					Color color = UiSettings.Current.GetColorValue(UIColorType.Background);

					ApplicationTheme osTheme = color == Colors.Black ? ApplicationTheme.Dark : ApplicationTheme.Light;

					string osThemeName = osTheme.GetName();
					this.SystemSettingThemeRadio.Content = $"System Set ({osThemeName} theme)";
				}

				UpdateOSThemeMenuItem();

				UiSettings.Current.ColorValuesChanged +=
					(sender, args) => { this.Dispatcher.BeginInvoke((Action)UpdateOSThemeMenuItem); };

				ThemeManager.AddActualThemeChangedHandler(this, (sender, args) => UpdateOSThemeMenuItem());
				this.SystemSettingThemeRadio.Click += (sender, args) => SetTheme(null);
			}
		}

		private void ZDoomInstallDirectoryClick(object sender, RoutedEventArgs e)
		{
			var dialog = new VistaFolderBrowserDialog
						 {
							 Description            = @"Select folder with zdoom.exe",
							 UseDescriptionForTitle = true,
							 ShowNewFolderButton    = false
						 };

			if (dialog.ShowDialog() == true)
			{
				string selectedFolder = dialog.SelectedPath;
				if (Directory.EnumerateFiles(selectedFolder, "*.exe").Any())
				{
					AppSettings.ZDoomDirectory = selectedFolder;
				}
				else
				{
					FlyoutBase.ShowAttachedFlyout(this.ZDoomDirBox);
				}
			}
		}

		private void SavePositionClick(object sender, RoutedEventArgs e)
		{
			AppSettings.StartAtPosition = true;
			AppSettings.StartingX       = Convert.ToInt32(MainWindow.Current.Left);
			AppSettings.StartingY       = Convert.ToInt32(MainWindow.Current.Top);
		}

		private void ResetPositionClick(object sender, RoutedEventArgs e)
		{
			AppSettings.StartAtPosition = false;

			MainWindow.Current.ResetPosition();
		}

		private void SaveSizeClick(object sender, RoutedEventArgs e)
		{
			AppSettings.StartWithSize  = true;
			AppSettings.StartingWidth  = Convert.ToInt32(MainWindow.Current.Width);
			AppSettings.StartingHeight = Convert.ToInt32(MainWindow.Current.Height);
		}

		private void ResetSizeClick(object sender, RoutedEventArgs e)
		{
			AppSettings.StartWithSize = false;

			MainWindow.Current.ResetSize();
		}
	}
}