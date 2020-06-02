using System;
using System.Globalization;
using System.Windows.Controls;
using Launcher.Configs;
using Ookii.Dialogs.Wpf;

namespace Launcher
{
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
	}
}