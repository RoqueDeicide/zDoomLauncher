using System;
using System.IO;
using ModernWpf.Controls;
using static System.Drawing.Icon;

namespace Launcher
{
	public partial class MainWindow
	{
		private void UpdateLaunchIcon()
		{
			this.ClearIconCache();

			string exePath = Path.Combine(this.zDoomFolder, this.currentExeFile);

			var icon = ExtractAssociatedIcon(exePath);
			if (icon == null)
			{
				this.LaunchAppButton.Icon = new SymbolIcon(Symbol.Play);
			}
			else
			{
				// Cache the icon.
				string cacheFilePath = null;
				for (int i = 0; i < 100; i++)
				{
					string cacheFileName = $"cache{i}.ico";
					cacheFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cacheFileName);

					try
					{
						using var stream =
							new FileStream(cacheFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
						icon.Save(stream);
						break;
					}
					catch (IOException)
					{
						// Skip this file.
					}
				}

				if (cacheFilePath == null)
				{
					throw new Exception("Unable to cache the icon.");
				}

				this.LaunchAppButton.Icon = new BitmapIcon
											{
												UriSource        = new Uri(cacheFilePath, UriKind.Absolute),
												ShowAsMonochrome = false
											};
			}
		}

		private void ClearIconCache()
		{
			for (int i = 0; i < 100; i++)
			{
				string cacheFileName = $"cache{i}.ico";
				string cacheFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cacheFileName);

				try
				{
					File.Delete(cacheFilePath);
				}
				catch (IOException)
				{
					// Skip this file.
				}
			}
		}
	}
}