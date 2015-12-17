using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Launcher.Configs;
using Launcher.Logging;

namespace Launcher
{
	public partial class MainWindow
	{
		private void SetupIwads()
		{
			var foundIwads = (from ComboBoxItem comboItem in this.IwadComboBox.Items
							  select (string)comboItem.Content).ToList();
			string iwadFile = Path.GetFileName(this.config.IwadPath);
			int foundIwadIndex =
				foundIwads.FindIndex(iwad => iwad.Equals(iwadFile,
														 StringComparison.InvariantCultureIgnoreCase));
			if (foundIwadIndex != -1)
			{
				this.IwadComboBox.SelectedValue = foundIwads[foundIwadIndex];
			}
			else
			{
				this.IwadComboBox.SelectedItem = null;
			}
		}
		private void SetupExtraFiles()
		{
			foreach (CheckBox item in this.ExtraFilesListBox.Items)
			{
				item.IsChecked = this.config.ExtraFiles.Contains((string)item.Content);
			}
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