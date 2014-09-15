using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Launcher.Configs;

using Ookii.Dialogs.Wpf;
using Xceed.Wpf.Toolkit;
using PathIO = System.IO.Path;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private LaunchConfiguration config;
		private string file;
		private VistaOpenFileDialog openConfigurationDialog;
		private VistaSaveFileDialog saveConfigurationDialog;
		
		private ContextMenu demoSaveSelectionMenu;
		// Time stamps for context menu opening.
		private int lastRightClickTime;
		public MainWindow()
		{
			this.resetting = true;

			this.config = new LaunchConfiguration();
			this.file = "DefaultConfigFile.lcf";
			
			InitializeComponent();

			this.InitializeDialogs();
			this.InitializeLoadableFiles();
			this.InitializeContextMenus();
			this.InitializeSomeEventHandlers();
			this.SetupInterface();
		}
		#region Setting Up
		private void SetupInterface()
		{
			this.resetting = true;
			if (this.config == null)
			{
				this.config = new LaunchConfiguration();
			}
			// Set the title of the window.
			this.Title = this.file ?? "zDoom Launcher";
			// Set up the list of IWADs.
			this.SetupIwads();
			// Ignore block map?
			this.IgnoreBlockMapIndicator.IsChecked = this.config.IgnoreBlockMap;
			// Set up a list of extra files.
			this.SetupExtraFiles();
			// Save directory.
			this.SaveDirectoryTextBox.Text = this.config.SaveDirectory;
			// Pixel mode.
			this.SetupPixelMode();
			// Custom resolution.
			this.SetupResolution();
			// What to disable?
			this.SetupDisableOptions();
			// What to do when starting the game?
			this.SetupStartUp();
			// Some extras.
			this.ExtraOptionsTextBox.Text = this.config.ExtraOptions;
			// Gameplay.
			this.SetupGamePlay();
			this.resetting = false;
		}
		#endregion
		private void LaunchTheGame(object sender, RoutedEventArgs e)
		{
			Process.Start("zdoom.exe", this.config.CommandLine);
		}
		private void CreateNewConfiguration(object sender, RoutedEventArgs e)
		{
			if (this.saveConfigurationDialog.ShowDialog(this) == true)
			{
				this.config = null;
				this.file = this.saveConfigurationDialog.FileName;
				this.SetupInterface();
			}
		}
		private void SaveConfiguration(object sender, RoutedEventArgs e)
		{
			this.config.Save(this.file);
		}
		private void OpenConfiguration(object sender, RoutedEventArgs e)
		{
			if (this.openConfigurationDialog.ShowDialog(this) == true)
			{
				this.config = LaunchConfiguration.Load(this.openConfigurationDialog.FileName);
				this.file = this.openConfigurationDialog.FileName;
				this.SetupInterface();
			}
		}
		private void CloseWindow(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		private void DisableOptionHandler(object sender, RoutedEventArgs e)
		{
			// I'll keep this here for now, in case ItemSelectionChanged handler doesn't work.
		}

		private void ShowCommandLine(object sender, RoutedEventArgs e)
		{
			new CommandLineWindow(this.config.CommandLine).Show();
		}
	}
}