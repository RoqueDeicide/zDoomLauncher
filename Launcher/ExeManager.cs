using System.Collections.ObjectModel;
using System.IO;

namespace Launcher
{
	/// <summary>
	/// Manages the list of available executable files.
	/// </summary>
	public static class ExeManager
	{
		/// <summary>
		/// An observable collection of executable files available to the user.
		/// </summary>
		public static readonly ObservableCollection<string> AvailableExeFiles;

		static ExeManager()
		{
			AvailableExeFiles = new ObservableCollection<string>();

			if (!AppSettings.ZDoomDirectory.IsNullOrWhiteSpace() && Directory.Exists(AppSettings.ZDoomDirectory))
			{
				RefreshExeFiles();
			}

			AppSettings.StaticPropertyChanged += (sender, args) =>
												 {
													 if (args.PropertyName == nameof(AppSettings.ZDoomDirectory))
													 {
														 RefreshExeFiles();
													 }
												 };
		}

		public static void RefreshExeFiles()
		{
			AvailableExeFiles.Clear();

			foreach (string file in Directory.EnumerateFiles(AppSettings.ZDoomDirectory, "*.exe", SearchOption.TopDirectoryOnly))
			{
				AvailableExeFiles.Add(Path.GetFileName(file));
			}
		}
	}
}