using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Launcher.Configs;

namespace Launcher
{
	public partial class MainWindow
	{
		#region Resolution
		// Nullify resolution, if it has been checked, otherwise give it existing value.
		private void EnableWidthField(object sender, RoutedEventArgs e)
		{
			this.config.Width = this.WidthValueField.Value;
		}

		private void EnableHeightField(object sender, RoutedEventArgs e)
		{
			this.config.Height = this.HeightValueField.Value;
		}

		private void DisableHeightField(object sender, RoutedEventArgs e)
		{
			this.config.Height = null;
		}

		private void DisableWidthField(object sender, RoutedEventArgs e)
		{
			this.config.Width = null;
		}

		private void UpdateWidthValue(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (this.WidthCheckBox == null)
			{
				return;
			}
			this.config.Width = (this.WidthCheckBox.IsChecked == true)
				? this.WidthValueField.Value
				: null;
		}

		private void UpdateHeightValue(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (this.HeightCheckBox == null)
			{
				return;
			}
			this.config.Height = (this.HeightCheckBox.IsChecked == true)
				? this.HeightValueField.Value
				: null;
		}
		#endregion
		#region Pixel Mode
		#endregion
		#region Disables
		private void EnableOption(object sender, RoutedEventArgs e)
		{
			CheckBox item = sender as CheckBox;
			if (item == null)
			{
				return;
			}

			IConvertible convertibleFlag = item.Tag as IConvertible;
			if (convertibleFlag == null)
			{
				return;
			}
			int flag = convertibleFlag.ToInt32(CultureInfo.InvariantCulture);
			// Set the flag.
			this.config.DisableFlags |= (DisableOptions)flag;
		}
		private void DisableOption(object sender, RoutedEventArgs e)
		{
			CheckBox item = sender as CheckBox;
			if (item == null)
			{
				return;
			}

			IConvertible convertibleFlag = item.Tag as IConvertible;
			if (convertibleFlag == null)
			{
				return;
			}
			int flag = convertibleFlag.ToInt32(CultureInfo.InvariantCulture);
			// Remove the flag.
			this.config.DisableFlags &= (DisableOptions)~flag;
		}
		#endregion
		#region GamePlay
		private void DisableMonsters(object sender, RoutedEventArgs e)
		{
			this.config.NoMonsters = true;
		}
		private void EnableMonsters(object sender, RoutedEventArgs e)
		{
			this.config.NoMonsters = false;
		}

		private void EnableFastMonsters(object sender, RoutedEventArgs e)
		{
			this.config.FastMonsters = true;
		}
		private void DisableFastMonsters(object sender, RoutedEventArgs e)
		{
			this.config.FastMonsters = true;
		}

		private void EnableRespawn(object sender, RoutedEventArgs e)
		{
			this.config.RespawningMonsters = true;
		}
		private void DisableRespawn(object sender, RoutedEventArgs e)
		{
			this.config.RespawningMonsters = true;
		}

		private void EnableTurbo(object sender, RoutedEventArgs e)
		{
			this.config.TurboMode = this.TurboValueField.Value;
		}
		private void DisableTurbo(object sender, RoutedEventArgs e)
		{
			this.config.TurboMode = null;
		}
		private void UpdateTurboField(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (this.TurboIndicator != null && this.TurboIndicator.IsChecked == true)
			{
				this.config.TurboMode = this.TurboValueField.Value;
			}
		}

		private void EnableTimeLimit(object sender, RoutedEventArgs e)
		{
			this.config.TimeLimit = this.TimeLimitValueField.Value;
		}
		private void DisableTimeLimit(object sender, RoutedEventArgs e)
		{
			this.config.TimeLimit = null;
		}
		private void UpdateTimeLimitField(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (this.TimeLimitIndicator != null && this.TimeLimitIndicator.IsChecked == true)
			{
				this.config.TimeLimit = this.TimeLimitValueField.Value;
			}
		}

		private void EnableCustomDifficulty(object sender, RoutedEventArgs e)
		{
			this.config.Difficulty = this.DifficultyValueField.Value;
		}
		private void DisableCustomDifficulty(object sender, RoutedEventArgs e)
		{
			this.config.Difficulty = null;
		}
		private void UpdateCustomDifficultyField(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (this.DifficultyIndicator != null && this.DifficultyIndicator.IsChecked == true)
			{
				this.config.Difficulty = this.DifficultyValueField.Value;
			}
		}
		#endregion
		#region Physics
		private void EnableIgnoreBlockMap(object sender, RoutedEventArgs e)
		{
			this.config.IgnoreBlockMap = true;
		}
		private void DisableIgnoreBlockMap(object sender, RoutedEventArgs e)
		{
			this.config.IgnoreBlockMap = false;
		}
		#endregion
		#region Start Up
		private void SwitchToSave(object sender, RoutedEventArgs e)
		{
			if (this.LoadGameTextBox == null)
			{
				return;
			}
			this.config.StartUpFileKind = StartupFile.SaveGame;
			this.config.AutoStartFile = this.LoadGameTextBox.Text;
		}
		private void UpdateSaveGameFile(object sender, TextChangedEventArgs e)
		{
			if (this.LoadSaveIndicator != null && this.LoadSaveIndicator.IsChecked == true)
			{
				this.config.AutoStartFile = this.LoadGameTextBox.Text;
			}
		}

		private void SwitchToDemo(object sender, RoutedEventArgs e)
		{
			this.config.StartUpFileKind = StartupFile.Demo;
			this.config.AutoStartFile = this.PlayDemoTextBox.Text;
		}
		private void UpdateDemoFile(object sender, TextChangedEventArgs e)
		{
			if (this.LoadDemoIndicator != null && this.LoadDemoIndicator.IsChecked == true)
			{
				this.config.AutoStartFile = this.PlayDemoTextBox.Text;
			}
		}

		private void SwitchToMap(object sender, RoutedEventArgs e)
		{
			if (this.MapValueField == null || this.EpisodeValueField == null)
			{
				return;
			}
			this.config.StartUpFileKind = StartupFile.Map;
			if (this.EpisodicIwadIsSelected())
			{
				this.config.AutoStartFile = string.Format("{0} {1}",
														  this.EpisodeValueField.Value,
														  this.MapValueField.Value);
			}
			else
			{
				this.config.AutoStartFile = string.Format("{0}", this.MapValueField.Value);
			}
		}
		private void UpdateEpisodeIndex(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			var episodeValue = this.EpisodeValueField.Value;

			if (this.LoadMapIndicator != null &&
				this.LoadMapIndicator.IsChecked == true &&
				episodeValue != null &&
				this.EpisodicIwadIsSelected())
			{
				this.config.AutoStartFile = this.config.AutoStartFile.ChangeNumber(0, episodeValue.Value);
			}
		}
		private void UpdateMapIndex(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			var mapValue = this.MapValueField.Value;

			if (this.LoadMapIndicator != null &&
				this.LoadMapIndicator.IsChecked == true &&
				mapValue != null)
			{
				this.config.AutoStartFile =
					this.config.AutoStartFile.ChangeNumber((this.EpisodicIwadIsSelected()) ? 1 : 0,
														   mapValue.Value);
			}
		}

		private void SwitchToNothing(object sender, RoutedEventArgs e)
		{
			this.config.StartUpFileKind = StartupFile.None;
		}
		#endregion
		private void UpdateSaveDirectory(object sender, TextChangedEventArgs e)
		{
			this.config.SaveDirectory = this.SaveDirectoryTextBox.Text;
		}

		private void UpdateConfigFile(object sender, TextChangedEventArgs e)
		{
			this.config.ConfigFile = this.ConfigFileTextBox.Text;
		}

		private void UpdateExtraOptions(object sender, TextChangedEventArgs e)
		{
			this.config.ExtraOptions = this.ExtraOptionsTextBox.Text;
		}

		private void ConfigurationNameChanged(object sender, TextChangedEventArgs e)
		{
			this.config.Name = this.ConfigurationNameTextBox.Text;
		}
	}
}