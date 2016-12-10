using System;
using System.Globalization;
using System.IO;
using System.Linq;
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

		private void UpdateWidthValue(object sender, int? oldValue, int? newValue)
		{
			if (this.WidthCheckBox == null)
			{
				return;
			}
			this.config.Width = (this.WidthCheckBox.IsChecked == true)
				? newValue
				: null;
		}

		private void UpdateHeightValue(object sender, int? oldValue, int? newValue)
		{
			if (this.HeightCheckBox == null)
			{
				return;
			}
			this.config.Height = (this.HeightCheckBox.IsChecked == true)
				? newValue
				: null;
		}
		#endregion
		#region Pixel Mode
		#endregion
		#region Disables
		private void EnableOption(object sender, RoutedEventArgs e)
		{
			CheckBox item = sender as CheckBox;

			IConvertible convertibleFlag = item?.Tag as IConvertible;
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

			IConvertible convertibleFlag = item?.Tag as IConvertible;
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
			this.config.TurboMode = (byte?)this.TurboValueField.Value;
		}
		private void DisableTurbo(object sender, RoutedEventArgs e)
		{
			this.config.TurboMode = null;
		}
		private void UpdateTurboField(object sender, int? oldValue, int? newValue)
		{
			if (this.TurboIndicator != null && this.TurboIndicator.IsChecked == true)
			{
				this.config.TurboMode = (byte?)newValue;
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
		private void UpdateTimeLimitField(object sender, int? oldValue, int? newValue)
		{
			if (this.TimeLimitIndicator != null && this.TimeLimitIndicator.IsChecked == true)
			{
				this.config.TimeLimit = newValue;
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
		private void UpdateCustomDifficultyField(object sender, int? oldValue, int? newValue)
		{
			if (this.DifficultyIndicator != null && this.DifficultyIndicator.IsChecked == true)
			{
				this.config.Difficulty = newValue;
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
			this.config.AutoStartFile = this.EpisodicIwadIsSelected()
				? $"{this.EpisodeValueField.Value} {this.MapValueField.Value}"
				: $"{this.MapValueField.Value}";
		}
		private void UpdateEpisodeIndex(object sender, int? oldValue, int? newValue)
		{
			var episodeValue = newValue;

			if (this.LoadMapIndicator != null && this.LoadMapIndicator.IsChecked == true &&
				this.EpisodicIwadIsSelected() && episodeValue != null)
			{
				this.config.AutoStartFile = this.config.AutoStartFile.ChangeNumber(0, (int)episodeValue);
			}
		}
		private void UpdateMapIndex(object sender, int? oldValue, int? newValue)
		{
			var mapValue = newValue;

			if (this.LoadMapIndicator != null && this.LoadMapIndicator.IsChecked == true &&
				mapValue != null)
			{
				this.config.AutoStartFile =
					this.config.AutoStartFile.ChangeNumber((this.EpisodicIwadIsSelected()) ? 1 : 0,
														   (int)mapValue);
			}
		}

		private void UpdateMapName(object sender, TextChangedEventArgs e)
		{
			if (this.LoadNamedMapIndicator?.IsChecked == true)
			{
				this.config.AutoStartFile = this.LoadNamedMapTextBox.Text;
			}
		}

		private void SwitchToNothing(object sender, RoutedEventArgs e)
		{
			// This check is needed for the case of the window getting closed before constructor is
			// complete.
			if (this.config == null)
			{
				return;
			}

			this.config.StartUpFileKind = StartupFile.None;
		}

		private void SwitchToNamedMap(object sender, RoutedEventArgs e)
		{
			this.config.StartUpFileKind = StartupFile.NamedMap;
			this.config.AutoStartFile = this.LoadNamedMapTextBox.Text;
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
			this.UpdateWindowTitle();
		}

		private void SelectIwad(object sender, EventArgs e)
		{
			IwadComboBox comboBox = sender as IwadComboBox;
			if (comboBox == null)
			{
				return;
			}

			IwadFile selectedFile = comboBox.SelectedItem as IwadFile;
			this.config.IwadPath = selectedFile == null
				? ""
				: selectedFile.FileName;

			if (this.LoadMapIndicator.IsChecked == true)
			{
				// Update the value in case selection of IWAD moves from episodic to non-episodic one or
				// vice-versa.
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
				string gamePath = this.zDoomFolder;

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
					PathUtils.ToRelativePath(this.openConfigFileDialog.FileName,this.zDoomFolder);
			}
		}
	}
}