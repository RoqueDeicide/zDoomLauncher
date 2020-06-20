using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using Windows.UI;
using Windows.UI.ViewManagement;
using Launcher.Configs;
using Launcher.Utilities;
using ModernWpf;
using Ookii.Dialogs.Wpf;

namespace Launcher
{
	/// <summary>
	/// Determines whether a theme menu button has to be checked.
	/// </summary>
	public class ThemeMenuButtonChecker : IValueConverter
	{
		/// <summary>
		/// Creates a value that indicates whether current app theme matches the one specified by the <paramref
		/// name="parameter"/>.
		/// </summary>
		/// <param name="value">     
		/// An instance of type <see cref="Nullable{ApplicationTheme}"/> that indicates a current application theme.
		/// </param>
		/// <param name="targetType">A type of <see cref="bool"/>.</param>
		/// <param name="parameter"> 
		/// A <see cref="string"/> value that indicates what menu button the value is created for.
		/// </param>
		/// <param name="culture">   Not used.</param>
		/// <returns>
		/// A value that indicates whether current app theme matches the one specified by the <paramref
		/// name="parameter"/>.
		/// </returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return parameter switch
				   {
					   "Light" => value is ApplicationTheme theme && theme == ApplicationTheme.Light,
					   "Dark"  => value is ApplicationTheme theme && theme == ApplicationTheme.Dark,
					   "null"  => value == null,
					   _       => false
				   };
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}

	public partial class MainWindow
	{
		private void InitializeDialogs()
		{
			// Initialize dialogs.
			this.saveConfigurationDialog = new VistaSaveFileDialog
										   {
											   DefaultExt       = ".xlcf",
											   InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
											   Filter =
												   @"Launch configuration files (*.xlcf)|*.xlcf|All files (*.*)|*.*",
											   Title         = @"Select where to save the configuration",
											   ValidateNames = true
										   };
			this.openConfigurationDialog = new VistaOpenFileDialog
										   {
											   Multiselect      = true,
											   CheckFileExists  = true,
											   InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
											   Title            = @"Select launch configuration to load",
											   ValidateNames    = true,
											   Filter =
												   @"Launch configuration files (*.xlcf)|*.xlcf|All files (*.*)|*.*"
										   };
			this.openSaveGameFileDialog = new VistaOpenFileDialog
										  {
											  DefaultExt       = ".zds",
											  InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
											  Filter =
												  @"ZDoom-compatible save game files (*.zds)|*.zds|All files (*.*)|*.*",
											  Title           = @"Select the save game file to load with the game",
											  ValidateNames   = true,
											  CheckFileExists = true
										  };
			this.openDemoFileDialog = new VistaOpenFileDialog
									  {
										  DefaultExt = ".lmp",
										  InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
										  Filter = @"ZDoom-compatible demo files (*.lmp)|*.lmp|All files (*.*)|*.*",
										  Title = @"Select the demo file to play in the game",
										  ValidateNames = true,
										  CheckFileExists = true
									  };
			this.openConfigFileDialog = new VistaOpenFileDialog
										{
											DefaultExt       = ".ini",
											InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
											Filter           = @"Configuration files (*.ini)|*.ini|All files (*.*)|*.*",
											Title            = @"Select the configuration file to load with the game",
											CheckFileExists  = false
										};
			this.openSaveFolderDialog = new VistaFolderBrowserDialog
										{
											Description = @"Select the folder where to store the save game files"
										};
		}

		private void InitializeSomeEventHandlers()
		{
			this.PixelModeComboBox.SelectionChanged +=
				(sender, args) =>
				{
					var selectedItem = this.PixelModeComboBox.SelectedItem as ComboBoxItem;

					// Update configuration with a new selection.
					if (selectedItem?.Tag is IConvertible mode)
					{
						this.config.PixelMode =
							(PixelMode) mode.ToInt32(CultureInfo.InvariantCulture);
					}
				};
		}

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
					Color color = UiSettings.Current.GetColorValue(UIColorType.Background);

					ApplicationTheme osTheme = color == Colors.Black ? ApplicationTheme.Dark : ApplicationTheme.Light;

					string osThemeName = osTheme.GetName();
					this.OSThemeMenuItem.Header  = $"OS-Set Theme ({osThemeName} theme)";
					this.OSThemeMenuItem.ToolTip = $"Let the operating system set the theme ({osThemeName} theme).";
				}

				UpdateOSThemeMenuItem();

				UiSettings.Current.ColorValuesChanged += (sender, args) =>
														 {
															 this
																.Dispatcher.BeginInvoke((Action) UpdateOSThemeMenuItem);
														 };

				ThemeManager.AddActualThemeChangedHandler(this, (sender, args) => UpdateOSThemeMenuItem());
				this.OSThemeMenuItem.Click += (sender, args) => SetTheme(null);
			}
		}
	}
}