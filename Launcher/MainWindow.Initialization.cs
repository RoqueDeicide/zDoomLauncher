using System;
using System.IO;
using System.Windows.Controls;
using Ookii.Dialogs.Wpf;

namespace Launcher
{
	public partial class MainWindow
	{
		private void InitializeDialogs()
		{
			// Initialize dialogs.
			this.selectExtraFilesDialog = new VistaOpenFileDialog
			{
				CheckFileExists = true,
				Multiselect = true,
				InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				Title = @"Select extra files to load",
				ValidateNames = true,
				Filter = @"PWAD files, Deh/Bex Patches (*.wad, *.deh, *.bex)|*.wad;*.deh;*.bex|All files (*.*)|*.*"
			};
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
		private void InitializeIwads()
		{
			foreach (string iwad in FindSupportedIwads())
			{
				ComboBoxItem item = new ComboBoxItem
				{
					Content = supportedIwads[iwad]
				};
				string iwadLocal = iwad;
				item.Selected += (sender, args) =>
				{
					this.config.IwadPath =
						Path.Combine(AppDomain.CurrentDomain.BaseDirectory, iwadLocal);
				};
				this.IwadComboBox.Items.Add(item);
			}
		}
	}
}