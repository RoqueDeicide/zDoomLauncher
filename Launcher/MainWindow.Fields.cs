using System;
using System.Windows.Controls;
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
		// File that stores the launch configuration.
		private string file;
		// Name of the file to execute when launching.
		private string currentExeFile;
		// Open and save dialogs.
		private VistaOpenFileDialog openConfigurationDialog;
		private VistaSaveFileDialog saveConfigurationDialog;
		// Context menu for selection of either demos or saves.
		private ContextMenu demoSaveSelectionMenu;
		// Time stamps for context menu opening.
		private int lastRightClickTime;
	}
}