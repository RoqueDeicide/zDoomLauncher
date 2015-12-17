using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Launcher.Configs;
using Ookii.Dialogs.Wpf;

namespace Launcher
{
	public partial class MainWindow
	{
		private bool resetting;
		private void InitializeDialogs()
		{
			// Initialize dialogs.
			this.saveConfigurationDialog = new VistaSaveFileDialog
			{
				DefaultExt = ".lcf",
				InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				Filter = @"Launch configuration files (*.lcf)|*.lcf|All files (*.*)|*.*",
				Title = @"Select where to save the configuration",
				ValidateNames = true
			};
			this.openConfigurationDialog = new VistaOpenFileDialog
			{
				CheckFileExists = true,
				Multiselect = true,
				InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				Title = @"Select launch configuration to load",
				ValidateNames = true,
				Filter = @"Launch configuration files (*.lcf)|*.lcf|All files (*.*)|*.*"
			};
		}
		private void InitializeLoadableFiles()
		{
			// Initialize IWAD combo-box.
			foreach
				(
				var iwadItem in from iwad in Iwads.FindSupportedIwads(this.zDoomFolder)
								select new ComboBoxItem {Content = iwad}
				)
			{
				this.IwadComboBox.Items.Add(iwadItem);
			}
			List<string> extras = new List<string>(50);
			// Get the list of files from base directory.
			extras.AddRange(GetLoadableFiles(this.zDoomFolder));
			// Get the files from DOOMWADDIR environment variable.
			string doomWadVar = Environment.GetEnvironmentVariable("DOOMWADDIR");
			if (!string.IsNullOrWhiteSpace(doomWadVar))
			{
				extras.AddRange(GetLoadableFiles(doomWadVar));
			}
			// Put iwads into separate list.
			List<string> iwads = extras.Where(Iwads.SupportedIwads.ContainsKey).ToList();
			extras.RemoveAll(Iwads.SupportedIwads.ContainsKey);
			// Put iwads into combo box.
			foreach (string iwad in iwads)
			{
				ComboBoxItem item = new ComboBoxItem
				{
					Content = Iwads.SupportedIwads[iwad],
					Tag = iwad
				};
				string iwadLocal = iwad;
				item.Selected += (sender, args) =>
				{
					this.config.IwadPath =
						Path.Combine(AppDomain.CurrentDomain.BaseDirectory, iwadLocal);
				};
				this.IwadComboBox.Items.Add(item);
			}
			// Put the rest into extras.
			foreach (string extra in extras)
			{
				CheckBox extraBox = new CheckBox
				{
					Content = extra
				};
				string extra1 = extra;
				extraBox.Checked += (sender, args) =>
				{
					if (this.resetting)
					{
						return;
					}
					this.config.ExtraFiles.Add(extra1);
				};
				extraBox.Unchecked += (sender, args) =>
				{
					if (this.resetting)
					{
						return;
					}
					this.config.ExtraFiles.Remove(extra1);
				};
				this.ExtraFilesListBox.Items.Add(extraBox);
			}
		}
		private void InitializeContextMenus()
		{
			// Context menu for selecting save-game or demo file.
			this.demoSaveSelectionMenu = new ContextMenu();
			MenuItem selectFileMenuItem = new MenuItem {Header = "Select The File"};
			selectFileMenuItem.Click += (sender, args) =>
			{
				TextBox textBox = this.demoSaveSelectionMenu.PlacementTarget as TextBox;
				if (textBox != null)
				{
					VistaOpenFileDialog dialog = new VistaOpenFileDialog
					{
						AddExtension = true,
						CheckFileExists = true,
						FilterIndex = 0,
						InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
					};
					if (textBox.Name == "PlayDemoTextBox")
					{
						// Select the demo.
						dialog.Title = @"Select the demo file to play";
						dialog.DefaultExt = ".lmp";
						dialog.Filter = @"Demo files (*.lmp)|*.lmp|All files (*.*)|*.*";
					}
					if (textBox.Name == "LoadGameTextBox")
					{
						// Select the save game.
						dialog.Title = @"Select the save game file to load";
						dialog.DefaultExt = ".zds";
						dialog.Filter = @"zDoom save game files (*.zds)|*.zds|All files (*.*)|*.*";
					}
					if (dialog.ShowDialog(this) == true)
					{
						if (dialog.DefaultExt == ".lmp")
						{
							this.PlayDemoTextBox.Text = dialog.FileName;
						}
						else
						{
							this.LoadGameTextBox.Text = dialog.FileName;
						}
					}
				}
			};
		}
		private void InitializeSomeEventHandlers()
		{
			// Add context menus for save-game and demo selection.
			this.PlayDemoTextBox.MouseRightButtonDown += (sender, args) =>
			{
				// Record when the click started.
				this.lastRightClickTime = args.Timestamp;
			};
			this.PlayDemoTextBox.MouseRightButtonUp += (sender, args) =>
			{
				// Ignore the click if it was too long.
				if (args.Timestamp - this.lastRightClickTime < 1000)
				{
					// Open the context menu.
					this.demoSaveSelectionMenu.PlacementTarget = this.PlayDemoTextBox;
					this.demoSaveSelectionMenu.IsOpen = true;
				}
			};
			this.LoadGameTextBox.MouseRightButtonDown += (sender, args) =>
			{
				// Record when the click started.
				this.lastRightClickTime = args.Timestamp;
			};
			this.LoadGameTextBox.MouseRightButtonUp += (sender, args) =>
			{
				// Ignore the click if it was too long.
				if (args.Timestamp - this.lastRightClickTime < 1000)
				{
					// Open the context menu.
					this.demoSaveSelectionMenu.PlacementTarget = this.LoadGameTextBox;
					this.demoSaveSelectionMenu.IsOpen = true;
				}
			};
			this.PixelModeComboBox.SelectionChanged +=
				(sender, args) =>
				{
					ComboBoxItem selectedItem = this.PixelModeComboBox.SelectedItem as ComboBoxItem;
					if (selectedItem != null)
					{
						// Update configuration with a new selection.
						IConvertible mode = selectedItem.Tag as IConvertible;
						if (mode != null)
						{
							this.config.PixelMode =
								(PixelMode)mode.ToInt32(CultureInfo.InvariantCulture);
						}
					}
				};
		}
	}
}