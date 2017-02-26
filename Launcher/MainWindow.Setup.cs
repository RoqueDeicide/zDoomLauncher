using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Launcher.Configs;
using Launcher.Logging;

namespace Launcher
{
	public partial class MainWindow
	{
		private bool settingUpStartUp;

		private void SetupExtraFiles()
		{
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
				this.IgnoreBlockMapItem.IsChecked = this.config.IgnoreBlockMap;
			}
		}
		private void SetupStartUp()
		{
			this.settingUpStartUp = true;

			switch (this.config.StartUpFileKind)
			{
				case StartupFile.None:
					this.LoadNothingIndicator.IsChecked = true;
					break;
				case StartupFile.SaveGame:
					this.LoadSaveIndicator.IsChecked = true;
					this.LoadGameTextBox.Text = this.config.AutoStartFile;
					break;
				case StartupFile.Demo:
					this.LoadDemoIndicator.IsChecked = true;
					this.PlayDemoTextBox.Text = this.config.AutoStartFile;
					break;
				case StartupFile.Map:
					Regex regex = new Regex("\\d+");
					var indexes = from Match match in regex.Matches(this.config.AutoStartFile)
								  select match.Value;
					string[] numbers = indexes.ToArray();
					switch (numbers.Length)
					{
						case 1:
							this.EpisodeValueField.Value = 0;
							this.MapValueField.Value = int.Parse(numbers[0]);
							break;
						case 2:
							this.EpisodeValueField.Value = int.Parse(numbers[0]);
							this.MapValueField.Value = int.Parse(numbers[1]);
							break;
						default:
							this.EpisodeValueField.Value = 0;
							this.MapValueField.Value = 0;
							break;
					}
					this.LoadMapIndicator.IsChecked = true;
					break;
				case StartupFile.NamedMap:
					this.LoadNamedMapIndicator.IsChecked = true;
					this.LoadNamedMapTextBox.Text = this.config.AutoStartFile;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			this.settingUpStartUp = false;
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
				this.TimeLimitValueField.Value = this.config.TimeLimit.Value;
			}
		}
	}
}