using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Launcher.Configs;
using ModernWpf.Controls;

namespace Launcher
{
	public partial class MainWindow
	{
		#region Resolution

		// Nullify resolution, if it has been checked, otherwise give it existing value.
		private void EnableWidthField(object sender, RoutedEventArgs e)
		{
			this.config.Width = Convert.ToInt32(this.WidthValueField.Value);
		}

		private void EnableHeightField(object sender, RoutedEventArgs e)
		{
			this.config.Height = Convert.ToInt32(this.HeightValueField.Value);
		}

		private void DisableHeightField(object sender, RoutedEventArgs e)
		{
			this.config.Height = null;
		}

		private void DisableWidthField(object sender, RoutedEventArgs e)
		{
			this.config.Width = null;
		}

		private void UpdateWidthValue(NumberBox numberBox, NumberBoxValueChangedEventArgs args)
		{
			if (this.WidthCheckBox == null || args.NewValue.IsNaN() || this.config == null)
			{
				return;
			}

			this.config.Width = this.WidthCheckBox.IsChecked == true
									? Convert.ToInt32(args.NewValue)
									: (int?)null;
		}

		private void UpdateHeightValue(NumberBox numberBox, NumberBoxValueChangedEventArgs args)
		{
			if (this.HeightCheckBox == null || args.NewValue.IsNaN() || this.config == null)
			{
				return;
			}

			this.config.Height = this.HeightCheckBox.IsChecked == true
									 ? Convert.ToInt32(args.NewValue)
									 : (int?)null;
		}

		#endregion

		#region Disables

		private void EnableOption(object sender, RoutedEventArgs e)
		{
			var item = sender as CheckBox;

			if (!(item?.Tag is IConvertible convertibleFlag))
			{
				return;
			}

			int flag = convertibleFlag.ToInt32(CultureInfo.InvariantCulture);
			// Set the flag.
			this.config.DisableFlags |= (DisableOptions)flag;
		}

		private void DisableOption(object sender, RoutedEventArgs e)
		{
			var item = sender as CheckBox;

			if (!(item?.Tag is IConvertible convertibleFlag))
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
			this.config.TurboMode = Convert.ToByte(this.TurboValueField.Value);
		}

		private void DisableTurbo(object sender, RoutedEventArgs e)
		{
			this.config.TurboMode = null;
		}

		private void UpdateTurboField(NumberBox numberBox, NumberBoxValueChangedEventArgs args)
		{
			if (this.TurboIndicator == null || args.NewValue.IsNaN() || this.config == null)
			{
				return;
			}

			this.config.TurboMode = this.TurboIndicator.IsChecked == true
										? Convert.ToByte(args.NewValue)
										: (byte?)null;
		}

		private void EnableTimeLimit(object sender, RoutedEventArgs e)
		{
			this.config.TimeLimit = Convert.ToInt32(this.TimeLimitValueField.Value);
		}

		private void DisableTimeLimit(object sender, RoutedEventArgs e)
		{
			this.config.TimeLimit = null;
		}

		private void UpdateTimeLimitField(NumberBox numberBox, NumberBoxValueChangedEventArgs args)
		{
			if (this.TimeLimitIndicator == null || args.NewValue.IsNaN() || this.config == null)
			{
				return;
			}

			this.config.TimeLimit = this.TimeLimitIndicator.IsChecked == true
										? Convert.ToByte(args.NewValue)
										: (byte?)null;
		}

		private void EnableCustomDifficulty(object sender, RoutedEventArgs e)
		{
			this.config.Difficulty = Convert.ToInt32(this.DifficultyValueField.Value);
		}

		private void DisableCustomDifficulty(object sender, RoutedEventArgs e)
		{
			this.config.Difficulty = null;
		}

		private void UpdateCustomDifficultyField(NumberBox numberBox, NumberBoxValueChangedEventArgs args)
		{
			if (this.DifficultyIndicator == null || args.NewValue.IsNaN() || this.config == null)
			{
				return;
			}

			this.config.Difficulty = this.DifficultyIndicator.IsChecked == true
										 ? Convert.ToByte(args.NewValue)
										 : (int?)null;
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
			if (this.LoadGameTextBox == null || this.settingUpStartUp)
			{
				return;
			}

			this.config.StartUpFileKind = StartupFile.SaveGame;
			this.config.AutoStartFile   = this.LoadGameTextBox.Text;
		}

		private void UpdateSaveGameFile(object sender, TextChangedEventArgs e)
		{
			if (this.LoadSaveIndicator != null && this.LoadSaveIndicator.IsChecked == true &&
				!this.settingUpStartUp)
			{
				this.config.AutoStartFile = this.LoadGameTextBox.Text;
			}
		}

		private void SwitchToDemo(object sender, RoutedEventArgs e)
		{
			if (this.settingUpStartUp)
			{
				return;
			}

			this.config.StartUpFileKind = StartupFile.Demo;
			this.config.AutoStartFile   = this.PlayDemoTextBox.Text;
		}

		private void UpdateDemoFile(object sender, TextChangedEventArgs e)
		{
			if (this.LoadDemoIndicator != null && this.LoadDemoIndicator.IsChecked == true &&
				!this.settingUpStartUp)
			{
				this.config.AutoStartFile = this.PlayDemoTextBox.Text;
			}
		}

		private void SwitchToMap(object sender, RoutedEventArgs e)
		{
			if (this.MapValueField == null || this.EpisodeValueField == null || this.settingUpStartUp)
			{
				return;
			}

			this.config.StartUpFileKind = StartupFile.Map;
			this.config.AutoStartFile = this.EpisodicIwadIsSelected()
											? $"{this.EpisodeValueField.Value} {this.MapValueField.Value}"
											: $"{this.MapValueField.Value}";
		}

		private void UpdateEpisodeIndex(NumberBox numberBox, NumberBoxValueChangedEventArgs args)
		{
			double episodeValue = args.NewValue;

			if (!episodeValue.IsNaN() && this.LoadMapIndicator != null && this.LoadMapIndicator.IsChecked == true &&
				this.EpisodicIwadIsSelected() && !this.settingUpStartUp)
			{
				this.config.AutoStartFile = this.config.AutoStartFile.ChangeNumber(0, (int)episodeValue);
			}
		}

		private void UpdateMapIndex(NumberBox numberBox, NumberBoxValueChangedEventArgs args)
		{
			double mapValue = args.NewValue;

			if (!mapValue.IsNaN() && this.LoadMapIndicator != null &&
				this.LoadMapIndicator.IsChecked            == true && !this.settingUpStartUp)
			{
				this.config.AutoStartFile =
					this.config.AutoStartFile.ChangeNumber(this.EpisodicIwadIsSelected() ? 1 : 0,
														   (int)mapValue);
			}
		}

		private void UpdateMapName(object sender, TextChangedEventArgs e)
		{
			if (this.LoadNamedMapIndicator?.IsChecked == true && !this.settingUpStartUp)
			{
				this.config.AutoStartFile = this.LoadNamedMapTextBox.Text;
			}
		}

		private void SwitchToNothing(object sender, RoutedEventArgs e)
		{
			// This check is needed for the case of the window getting closed before constructor is complete.
			if (this.config == null)
			{
				return;
			}

			this.config.StartUpFileKind = StartupFile.None;
		}

		private void SwitchToNamedMap(object sender, RoutedEventArgs e)
		{
			if (this.settingUpStartUp)
			{
				return;
			}

			this.config.StartUpFileKind = StartupFile.NamedMap;
			this.config.AutoStartFile   = this.LoadNamedMapTextBox.Text;
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
			if (this.config == null)
			{
				return;
			}

			this.config.Name = this.ConfigurationNameTextBox.Text;
			this.UpdateWindowTitle();
		}

		private void SelectIwad(object sender, EventArgs e)
		{
			if (!(sender is IwadComboBox comboBox))
			{
				return;
			}

			this.config.IwadPath = !(comboBox.SelectedItem is IwadFile selectedFile)
									   ? ""
									   : selectedFile.FileName;

			if (this.LoadMapIndicator.IsChecked == true)
			{
				// Update the value in case selection of IWAD moves from episodic to non-episodic one or vice-versa.
				this.config.AutoStartFile = this.EpisodicIwadIsSelected()
												? $"{this.EpisodeValueField.Value} {this.MapValueField.Value}"
												: $"{this.MapValueField.Value}";
			}
		}

		private void SelectSaveGameFile(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(this.LoadGameTextBox.Text))
			{
				this.openSaveGameFileDialog.FileName = this.LoadGameTextBox.Text;
			}

			if (this.openSaveGameFileDialog.ShowDialog() == true)
			{
				this.LoadGameTextBox.Text = this.openSaveGameFileDialog.FileName;
			}
		}

		private void SelectDemoFile(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(this.PlayDemoTextBox.Text))
			{
				this.openDemoFileDialog.FileName = this.PlayDemoTextBox.Text;
			}

			if (this.openDemoFileDialog.ShowDialog() == true)
			{
				this.PlayDemoTextBox.Text = this.openDemoFileDialog.FileName;
			}
		}

		private void SelectSaveDirectory(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(this.SaveDirectoryTextBox.Text))
			{
				this.openSaveFolderDialog.SelectedPath = this.SaveDirectoryTextBox.Text;
			}

			if (this.openSaveFolderDialog.ShowDialog() == true)
			{
				string selectedPath = this.openSaveFolderDialog.SelectedPath;
				string gamePath     = this.zDoomFolder;

				this.SaveDirectoryTextBox.Text = PathUtils.ToRelativePath(selectedPath, gamePath);
			}
		}

		private void SelectConfigPath(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(this.ConfigFileTextBox.Text))
			{
				this.openConfigFileDialog.FileName = this.ConfigFileTextBox.Text;
			}

			if (this.openConfigFileDialog.ShowDialog() == true)
			{
				this.ConfigFileTextBox.Text =
					PathUtils.ToRelativePath(this.openConfigFileDialog.FileName, this.zDoomFolder);
			}
		}
	}
}