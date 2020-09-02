namespace Launcher
{
	public partial class MainWindow
	{
		private void SetupExtraFiles()
		{
			this.ExtraFilesBox.SelectedFiles = this.Config.ExtraFiles;
		}
	}
}