
using System.Windows.Controls;
using Launcher.Configs;
using Ookii.Dialogs.Wpf;

namespace Launcher
{
	public partial class MainWindow
	{
		// ZDoom launch configuration.
		private LaunchConfiguration config;
		// File that stores the launch configuration configuration.
		private string file;
		// Open and save dialogs.
		private VistaOpenFileDialog openConfigurationDialog;
		private VistaSaveFileDialog saveConfigurationDialog;
		// Context menu for selection of either demos or saves.
		private ContextMenu demoSaveSelectionMenu;
		// Time stamps for context menu opening.
		private int lastRightClickTime;
	}
}