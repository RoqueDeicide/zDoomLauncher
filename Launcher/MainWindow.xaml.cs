﻿using System;
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
	public partial class MainWindow : Window
	{
		private static readonly SortedList<string, string> supportedIwads = new SortedList<string, string>
		{
			{"doom1.wad", "Doom Shareware version"},
			{"doom.wad", "Doom Full version"},
			{"doomu.wad", "Ultimate Doom"},
			{"bfgdoom.wad", "Doom BFG Edition"},
			{"freedoom1.wad", "Freedoom: Phase 1"},
			{"doom2.wad", "Doom 2"},
			{"bfgdoom2.wad", "Doom 2 BFG Edition"},
			{"tnt.wad", "Final Doom — TNT: Evilution"},
			{"plutonia.wad", "Final Doom — The Plutonia Experiment"},
			{"freedm.wad", "FreeDM"},
			{"freedoom2.wad", "Freedoom: Phase 2"},
			{"doom2f.wad", "French Doom II"},
			{"heretic1.wad", "Heretic (Shareware)"},
			{"heretic.wad", "Heretic (Registered or Commercial)"},
			{"blasphemer.wad", "Blasphemer"},
			{"hexendemo.wad", "Hexen (Demo)"},
			{"hexen.wad", "Hexen (Full)"},
			{"hexdd.wad", "Hexen: Deathkings of the Dark Citadel"},
			{"strife0.wad", "Strife (Teaser)"},
			{"strife.wad", "Strife (Full)"},
			{"chex.wad", "Chex Quest"},
			{"chex3.wad", "Chex Quest 3"},
			{"action2.wad", "Action Doom 2: Urban Brawl"},
			{"harm1.wad", "Harmony v1.1"},
			{"hacx.wad", "Hacx v1.2"},
			{"hacx2.wad", "Hacx v2.0"}
		};
		private LaunchConfiguration config;
		private string file;
		private VistaOpenFileDialog selectExtraFilesDialog;
		private VistaOpenFileDialog openConfigurationDialog;
		private VistaSaveFileDialog saveConfigurationDialog;
		private ContextMenu demoSaveSelectionMenu;
		// Time stamps for context menu opening.
		private int lastRightClickTime;
		public MainWindow()
		{
			InitializeComponent();

			this.config = new LaunchConfiguration();
			this.file = null;
			this.InitializeDialogs();
			this.InitializeIwads();
			this.SetupInterface();
		}
		#region Setting Up
		private void SetupInterface()
		{
			if (this.config == null)
			{
				this.config = new LaunchConfiguration();
			}
			// Set the title of the window.
			this.Title = this.file != null ? "zDoom Launcher" : this.file;
			// Set up the list of IWADs.
			this.SetupIwads();
			// Ignore block map?
			this.IgnoreBlockMapIndicator.IsChecked = this.config.IgnoreBlockMap;
			this.IgnoreBlockMapIndicator.Checked +=
				(sender, args) => this.config.IgnoreBlockMap = true;
			this.IgnoreBlockMapIndicator.Unchecked +=
				(sender, args) => this.config.IgnoreBlockMap = false;
			// Set up a list of extra files.
			this.SetupExtraFiles();
			// Save directory.
			this.SaveDirectoryTextBox.Text = this.config.SaveDirectory;
			this.SaveDirectoryTextBox.TextChanged += (sender, args) =>
				this.config.SaveDirectory = this.SaveDirectoryTextBox.Text;
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
			this.ExtraOptionsTextBox.TextChanged += (sender, args) =>
				this.config.ExtraOptions = this.ExtraOptionsTextBox.Text;
		}
		private static IEnumerable<string> FindSupportedIwads()
		{
			// Scan for IWADs in the app folder.
			string currentFolder = AppDomain.CurrentDomain.BaseDirectory;
			return supportedIwads.Keys.Where(x => File.Exists(PathIO.Combine(currentFolder, x)));
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
	}
}