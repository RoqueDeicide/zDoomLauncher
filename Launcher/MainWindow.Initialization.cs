using System;
using System.Windows.Media;
using Windows.UI.ViewManagement;
using Launcher.Utilities;
using ModernWpf;
using UiColor = Windows.UI.Color;
using UiColors = Windows.UI.Colors;

namespace Launcher
{

	public partial class MainWindow
	{
		private void SetupThemeMenuItems()
		{
			static void SetTheme(ApplicationTheme? theme)
			{
				ThemeManager.Current.ApplicationTheme = theme;
			}

			this.LightThemeMenuItem.Click += (sender, args) => SetTheme(ApplicationTheme.Light);
			this.DarkThemeMenuItem.Click  += (sender, args) => SetTheme(ApplicationTheme.Dark);

			if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 10)
			{
				// OS doesn't support theme enforcement.

				this.OSThemeMenuItem.IsEnabled = false;
			}
			else
			{
				void UpdateOSThemeMenuItem()
				{
					UiColor color = UiSettings.Current.GetColorValue(UIColorType.Background);

					ApplicationTheme osTheme = color == UiColors.Black ? ApplicationTheme.Dark : ApplicationTheme.Light;

					string osThemeName = osTheme.GetName();
					this.OSThemeMenuItem.Header  = $"OS-Set Theme ({osThemeName} theme)";
					this.OSThemeMenuItem.ToolTip = $"Let the operating system set the theme ({osThemeName} theme).";
					bool isLight = osTheme == ApplicationTheme.Light;
					this.OSThemeRectangle.Stroke = isLight ? Brushes.Black : Brushes.White;
					this.OSThemeRectangle.Fill = isLight ? Brushes.White: Brushes.Black;
				}

				UpdateOSThemeMenuItem();

				UiSettings.Current.ColorValuesChanged += (sender, args) =>
														 {
															 this
																.Dispatcher.BeginInvoke((Action)UpdateOSThemeMenuItem);
														 };

				ThemeManager.AddActualThemeChangedHandler(this, (sender, args) => UpdateOSThemeMenuItem());
				this.OSThemeMenuItem.Click += (sender, args) => SetTheme(null);
			}
		}
	}
}