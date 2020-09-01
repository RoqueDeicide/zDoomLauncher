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
		private LaunchConfiguration config;

		// Name of the file to execute when launching.
		private string currentExeFile;

		// Open and save dialogs.
		private VistaOpenFileDialog openConfigurationDialog;

		private VistaSaveFileDialog saveConfigurationDialog;

		private VistaOpenFileDialog      openSaveGameFileDialog;
		private VistaOpenFileDialog      openDemoFileDialog;
		private VistaOpenFileDialog      openConfigFileDialog;
		private VistaFolderBrowserDialog openSaveFolderDialog;

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