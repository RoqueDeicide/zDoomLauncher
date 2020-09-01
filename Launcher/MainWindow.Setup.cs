using System;
using Launcher.Configs;
using Launcher.Utilities;

namespace Launcher
{
	public partial class MainWindow
	{
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

		private void SetupDisableOptions()
		{
			Log.Message("Setting up a list of options that can be disabled.");
			if (this.config.DisableFlags != DisableOptions.EnableAll)
			{
				this.AutoLoadItem.IsChecked =
					this.config.DisableFlags.HasFlag(DisableOptions.AutoLoad);
				this.CdAudioItem.IsChecked =
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
	}
}