using System;
using System.Windows;
using Launcher.Configs;
using Ookii.Dialogs.Wpf;

namespace Launcher
{
	public partial class MainWindow
	{
		// zDoom installation directory.
		private string zDoomFolder;

		// ZDoom launch configuration.
		public LaunchConfiguration Config { get; } = new LaunchConfiguration();

		// Name of the file to execute when launching.
		private string currentExeFile;

		// Open and save dialogs.
		private readonly VistaOpenFileDialog openConfigurationDialog
			= new VistaOpenFileDialog
			  {
				  Multiselect      = true,
				  CheckFileExists  = true,
				  InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				  Title            = @"Select launch configuration to load",
				  ValidateNames    = true,
				  Filter           = @"Launch configuration files (*.xlcf)|*.xlcf|All files (*.*)|*.*"
			  };

		private readonly VistaSaveFileDialog saveConfigurationDialog =
			new VistaSaveFileDialog
			{
				DefaultExt       = ".xlcf",
				InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				Filter           = @"Launch configuration files (*.xlcf)|*.xlcf|All files (*.*)|*.*",
				Title            = @"Select where to save the configuration",
				ValidateNames    = true
			};

		private readonly VistaOpenFileDialog openSaveGameFileDialog =
			new VistaOpenFileDialog
			{
				DefaultExt       = ".zds",
				InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				Filter =
					@"ZDoom-compatible save game files (*.zds)|*.zds|All files (*.*)|*.*",
				Title           = @"Select the save game file to load with the game",
				ValidateNames   = true,
				CheckFileExists = true
			};

		private readonly VistaOpenFileDialog openDemoFileDialog =
			new VistaOpenFileDialog
			{
				DefaultExt       = ".lmp",
				InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				Filter           = @"ZDoom-compatible demo files (*.lmp)|*.lmp|All files (*.*)|*.*",
				Title            = @"Select the demo file to play in the game",
				ValidateNames    = true,
				CheckFileExists  = true
			};

		private readonly VistaOpenFileDialog openConfigFileDialog =
			new VistaOpenFileDialog
			{
				DefaultExt       = ".ini",
				InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				Filter           = @"Configuration files (*.ini)|*.ini|All files (*.*)|*.*",
				Title            = @"Select the configuration file to load with the game",
				CheckFileExists  = false
			};

		private readonly VistaFolderBrowserDialog openSaveFolderDialog =
			new VistaFolderBrowserDialog
			{
				Description = @"Select the folder where to store the save game files"
			};

		public static readonly DependencyProperty CurrentConfigFileProperty =
			DependencyProperty.Register("CurrentConfigFile", typeof(string), typeof(MainWindow),
										new PropertyMetadata(default(string)));

		/// <summary>
		/// Gets or sets the current file that is associated with the launch configuration.
		/// </summary>
		public string CurrentConfigFile
		{
			get => (string)this.GetValue(CurrentConfigFileProperty);
			set => this.SetValue(CurrentConfigFileProperty, value);
		}
	}
}