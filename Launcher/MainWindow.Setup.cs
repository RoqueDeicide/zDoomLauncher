using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Launcher.Configs;
using Launcher.Logging;

namespace Launcher
{
	public partial class MainWindow
	{
		private void RefreshIwads()
		{
			if (this.IwadComboBox == null)
			{
				return;
			}

			// Clear the combo box.
			this.IwadComboBox.Items.Clear();

			// Fill the combo box with found IWADs.
			if (this.zDoomFolder == null)
			{
				return;
			}

			this.IwadComboBox.SelectedItem = null;

			string iwadFile = Path.GetFileName(this.config.IwadPath);

			foreach (string iwad in Iwads.FindSupportedIwads(this.zDoomFolder))
			{
				ComboBoxItem iwadItem = new ComboBoxItem
				{
					Content = Iwads.SupportedIwads[iwad],
					Tag = iwad
				};
				iwadItem.Selected += this.SelectTheIwad;

				int currentItemIndex = this.IwadComboBox.Items.Count;
				this.IwadComboBox.Items.Add(iwadItem);

				// Select the item, if it is supposed to be selected in the configuration.
				if (iwad.Equals(iwadFile, StringComparison.InvariantCultureIgnoreCase))
				{
					this.IwadComboBox.SelectedIndex = currentItemIndex;
				}
			}
		}
		private void SelectTheIwad(object sender, RoutedEventArgs routedEventArgs)
		{
			ComboBoxItem item = sender as ComboBoxItem;
			if (item == null)
			{
				return;
			}

			string selectedIwad = item.Tag as string;
			Debug.Assert(selectedIwad != null, "Path to the IWAD was not assigned to respective combo box item.");

			this.config.IwadPath = selectedIwad;
		}
		private void SetupExtraFiles()
		{
			this.ExtraFilesBox.GameFolder = this.zDoomFolder;
			this.ExtraFilesBox.SelectedFiles = this.config.ExtraFiles;
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
		}
		private void SetupResolution()
		{
			if (this.config.Width.HasValue)
			{
				this.WidthCheckBox.IsChecked = true;
				this.WidthValueField.Value = this.config.Width.Value;
			}
			if (this.config.Height.HasValue)
			{
				this.HeightCheckBox.IsChecked = true;
				this.HeightValueField.Value = this.config.Height.Value;
			}
		}
		private void SetupDisableOptions()
		{
			Log.Message("Setting up a list of options that can be disabled.");
			if (this.config.DisableFlags != DisableOptions.EnableAll)
			{
				this.AutoLoadItem.IsChecked =
					this.config.DisableFlags.HasFlag(DisableOptions.AutoLoad);
				this.CDAudioItem.IsChecked =
					this.config.DisableFlags.HasFlag(DisableOptions.CompactDiskAudio);
				this.IdlingItem.IsChecked =
					this.config.DisableFlags.HasFlag(DisableOptions.Idling);
				this.JoyStickItem.IsChecked =
					this.config.DisableFlags.HasFlag(DisableOptions.JoyStick);
				this.MusicItem.IsChecked =
					this.config.DisableFlags.HasFlag(DisableOptions.Music);
				this.SfxItem.IsChecked =
					this.config.DisableFlags.HasFlag(DisableOptions.SoundEffects);
				this.SpriteRenamingItem.IsChecked =
					this.config.DisableFlags.HasFlag(DisableOptions.SpriteRenaming);
				this.StartupScreensItem.IsChecked =
					this.config.DisableFlags.HasFlag(DisableOptions.StartupScreens);
			}
		}
		private void SetupStartUp()
		{
			switch (this.config.StartUpFileKind)
			{
				case StartupFile.None:
					this.LoadNothingIndicator.IsChecked = true;
					break;
				case StartupFile.SaveGame:
					this.LoadSaveIndicator.IsChecked = true;
					break;
				case StartupFile.Demo:
					this.LoadDemoIndicator.IsChecked = true;
					break;
				case StartupFile.Map:
					this.LoadMapIndicator.IsChecked = true;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		private void SetupGamePlay()
		{
			this.NoMonstersIndicator.IsChecked = this.config.NoMonsters;
			this.FastMonstersIndicator.IsChecked = this.config.FastMonsters;
			this.RespawningMonstersIndicator.IsChecked = this.config.RespawningMonsters;
			this.TurboIndicator.IsChecked = this.config.TurboMode.HasValue;
			if (this.config.TurboMode.HasValue)
			{
				this.TurboValueField.Value = this.config.TurboMode.Value;
			}
			this.TimeLimitIndicator.IsChecked = this.config.TimeLimit.HasValue;
			if (this.config.TimeLimit.HasValue)
			{
				this.TimeLimitValueField.Value = (byte?)this.config.TimeLimit.Value;
			}
		}
	}
}