namespace Launcher
{
	public partial class MainWindow
	{
		// Name of the file that contains the application configuration.
		private const string AppConfigurationFileName = "Zdl.config";

		// Extension to use for application configuration file that contains the data in Xml format.
		private const string AppConfigurationXmlExtension = "config";

		// Extension to use for application configuration file that contains the data in binary format.
		private const string AppConfigurationBinaryExtension = "binaryConfig";

		// Name of the application configuration entry that contains the path to the folder were Doom source port files are
		// located.
		private const string GameFolderEntryName = "zDoomInstallationFolder";

		// Name of the application configuration entry that contains the full name of the file that contains last used launch
		// configuration.
		private const string LastConfigurationFileEntryName = "LastLaunchConfigurationFile";

		// Name of the application configuration entry that contains the name of the last used executable file.
		private const string LastExeFileEntryName = "LastExeFile";

		// Name of the entry that contains sub-entries that are paths to directories to for loadable files in.
		private const string LoadableFilesDirectoriesEntryName = "LoadableFilesDirectories";

		// Name of the entry that contains a path to the directory to for loadable files in.
		private const string LoadableFilesDirectoryEntryName = "LoadableFilesDirectory";

		// Name of the entry that contains custom position of the main window.
		private const string MainWindowPositionEntryName = "MainWindowPosition";

		// Name of the entry that contains custom dimensions of the main window.
		private const string MainWindowSizeEntryName = "MainWindowSize";

		// Name of the entry that indicates that the main window is maximized.
		private const string MainWindowMaximizedEntryName = "MainWindowMaximized";

		// Name of the entry that indicates preferred theme.
		private const string PreferredThemeEntryName = "PreferredTheme";

		// Name of the entry that indicates preferred accent color.
		private const string PreferredAccentColorEntryName = "PreferredAccentColor";
	}
}