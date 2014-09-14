using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using Launcher.Configs;
using Ookii.Dialogs.Wpf;

namespace Launcher
{
	public partial class MainWindow
	{
		private void SetupIwads()
		{
			ComboBoxItem iwadItem =
				this.IwadComboBox.Items
				.OfType<ComboBoxItem>()
				.FirstOrDefault(x => (string)x.Content == Path.GetFileName(this.config.IwadPath));
			if (iwadItem != null)
			{
				iwadItem.IsSelected = true;
			}
		}
		private void SetupExtraFiles()
		{
			// Clear the list before adding files from the configuration.
			this.ExtraFilesListView.Items.Clear();
			// Fill the list.
			for (int i = 0; i < this.config.ExtraFiles.Count; i++)
			{
				// Add the item to the list.
				this.ExtraFilesListView.Items.Add
				(
					this.CreateExtraFileListItem(this.config.ExtraFiles[i], i)
				);
			}
			// Add "Add" button to the end of extras.
			Button addExtraButton = new Button
			{
				Content = "Add"
			};
			addExtraButton.Click += (sender, args) =>
			{
				if (this.selectExtraFilesDialog.ShowDialog(this) == true)
				{
					// Add files that are not IWADs to the list of extras.
					string[] files = this.selectExtraFilesDialog.FileNames;
					for (int i = 0; i < files.Length; i++)
					{
						// Add the file to the list.
						this.config.ExtraFiles.Add(files[i]);
						this.ExtraFilesListView.Items.Insert
						(
							// Put the item before the add button.
							this.ExtraFilesListView.Items.Count - 2,
							this.CreateExtraFileListItem
							(
								files[i],
								this.config.ExtraFiles.Count
							)
						);
					}
				}
			};
			this.ExtraFilesListView.Items.Add
			(
				new
				{
					FileName = addExtraButton,
					Type = ""
				}
			);
		}
		private object CreateExtraFileListItem(string extraFile, int i)
		{
			// Type column.
			string type;
			switch (Path.GetExtension(extraFile))
			{
				case ".bex":
				case ".deh":
					type = "Patch";
					break;
				case ".wad":
					type = "PWAD";
					break;
				default:
					type = "Unknown";
					break;
			}
			// File name column.
			StackPanel fileNamePanel = new StackPanel();
			Button removeExtraButton = new Button
			{
				Content = 'x'
			};
			int i1 = i;
			removeExtraButton.Click += (sender, args) =>
			{
				this.ExtraFilesListView.Items.RemoveAt(i1);
				this.config.ExtraFiles.RemoveAt(i1);
			};
			fileNamePanel.Children.Add(removeExtraButton);
			fileNamePanel.Children.Add(new TextBlock(new Run(Path.GetFileName(extraFile))));
			// Add the item to the list.
			return new
			{
				FileName = fileNamePanel,
				Type = type
			};
		}
		private void SetupPixelMode()
		{
			switch (this.config.PixelMode)
			{
				case PixelMode.NoChange:
					this.SinglePixelComboBoxItem.IsSelected = true;
					break;
				case PixelMode.Double:
					this.DoublePixelComboBoxItem.IsSelected = true;
					break;
				case PixelMode.Quad:
					this.QuadPixelComboBoxItem.IsSelected = true;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			this.PixelModeComboBox.SelectionChanged +=
				(sender, args) =>
				{
					if (this.PixelModeComboBox.SelectedItem != null)
					{
						// Update configuration with a new selection.
						ComboBoxItem item = (ComboBoxItem)this.PixelModeComboBox.SelectedItem;
						this.config.PixelMode = (PixelMode)(int)item.Tag;
					}
				};
		}
		private void SetupResolution()
		{
			if (this.config.Width.HasValue)
			{
				WidthCheckBox.IsChecked = true;
				WidthValueField.Value = this.config.Width.Value;
			}
			if (this.config.Height.HasValue)
			{
				HeightCheckBox.IsChecked = true;
				HeightValueField.Value = this.config.Height.Value;
			}
			// Update resolution when value is changed.
			this.WidthValueField.ValueChanged += (sender, args) =>
			{
				this.config.Width =
					(this.WidthCheckBox.IsChecked == true) ? this.WidthValueField.Value : null;
			};
			this.HeightValueField.ValueChanged += (sender, args) =>
			{
				this.config.Height =
					(this.HeightCheckBox.IsChecked == true) ? this.HeightValueField.Value : null;
			};
			// Nullify resolution, if it has been checked, otherwise give it existing value.
			this.WidthCheckBox.Checked +=
				(sender, args) => this.config.Width = this.WidthValueField.Value;
			this.WidthCheckBox.Unchecked +=
				(sender, args) => this.config.Width = null;
			this.HeightCheckBox.Checked +=
				(sender, args) => this.config.Height = this.HeightValueField.Value;
			this.HeightCheckBox.Unchecked +=
				(sender, args) => this.config.Height = null;
		}
		private void SetupDisableOptions()
		{
			if (this.config.DisableFlags != DisableOptions.EnableAll)
			{
				this.AutoLoadItem.IsSelected =
					this.config.DisableFlags.HasFlag(DisableOptions.AutoLoad);
				this.CDAudioItem.IsSelected =
					this.config.DisableFlags.HasFlag(DisableOptions.CompactDiskAudio);
				this.IdlingItem.IsSelected =
					this.config.DisableFlags.HasFlag(DisableOptions.Idling);
				this.JoyStickItem.IsSelected =
					this.config.DisableFlags.HasFlag(DisableOptions.JoyStick);
				this.MusicItem.IsSelected =
					this.config.DisableFlags.HasFlag(DisableOptions.Music);
				this.SfxItem.IsSelected =
					this.config.DisableFlags.HasFlag(DisableOptions.SoundEffects);
				this.SpriteRenamingItem.IsSelected =
					this.config.DisableFlags.HasFlag(DisableOptions.SpriteRenaming);
				this.StartupScreensItem.IsSelected =
					this.config.DisableFlags.HasFlag(DisableOptions.StartupScreens);
			}
			this.DisableFlagsList.ItemSelectionChanged += (sender, args) =>
			{
				ListBoxItem selectedItem = args.Item as ListBoxItem;
				if (selectedItem != null)
				{
					if (args.IsSelected)
					{
						// Set the flag.
						this.config.DisableFlags |= (DisableOptions)(int)selectedItem.Tag;
					}
					else
					{
						// Remove the flag.
						this.config.DisableFlags &= (DisableOptions)~((int)selectedItem.Tag);
					}
				}
			};
		}
		private void SetupStartUp()
		{
			switch (this.config.StartUpFileKind)
			{
				case StartupFile.None:
					LoadNothingIndicator.IsChecked = true;
					break;
				case StartupFile.SaveGame:
					LoadSaveIndicator.IsChecked = true;
					break;
				case StartupFile.Demo:
					LoadDemoIndicator.IsChecked = true;
					break;
				case StartupFile.Map:
					LoadMapIndicator.IsChecked = true;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			this.LoadNothingIndicator.Checked +=
				(sender, args) => this.config.StartUpFileKind = StartupFile.None;
			this.LoadSaveIndicator.Checked +=
				(sender, args) => this.config.StartUpFileKind = StartupFile.SaveGame;
			this.LoadDemoIndicator.Checked +=
				(sender, args) => this.config.StartUpFileKind = StartupFile.Demo;
			this.LoadMapIndicator.Checked +=
				(sender, args) => this.config.StartUpFileKind = StartupFile.Map;
			this.LoadGameTextBox.TextChanged += (sender, args) =>
			{
				if (this.LoadSaveIndicator.IsChecked == true)
				{
					this.config.AutoStartFile = this.LoadGameTextBox.Text;
				}
			};
			this.PlayDemoTextBox.TextChanged += (sender, args) =>
			{
				if (this.LoadDemoIndicator.IsChecked == true)
				{
					this.config.AutoStartFile = this.PlayDemoTextBox.Text;
				}
			};
			this.StartMapTextBox.TextChanged += (sender, args) =>
			{
				if (this.LoadMapIndicator.IsChecked == true)
				{
					this.config.AutoStartFile = this.StartMapTextBox.Text;
				}
			};
			// Add context menus for save-game and demo selection.
			this.demoSaveSelectionMenu = new ContextMenu();
			MenuItem selectFileMenuItem = new MenuItem { Header = "Select The File" };
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
			this.demoSaveSelectionMenu.Items.Add(selectFileMenuItem);
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
		}
	}
}