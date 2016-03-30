using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Launcher.Databases;
using Launcher.Extensions;
using Launcher.Logging;
using Launcher.Utilities;

namespace Launcher.Configs
{
	public partial class LaunchConfiguration
	{
		/// <summary>
		/// Saves this configuration to the file.
		/// </summary>
		/// <param name="file">      Path to the file.</param>
		/// <param name="gameFolder">Path to the folder that contains the executables.</param>
		public void Save(string file, string gameFolder)
		{
			Database database = new Database("xlcf", "blcf");

			// Name.
			database.AddContent(nameof(this.Name), this.Name);

			// Iwad Path.
			database.AddContent(nameof(this.IwadPath), this.IwadPath);

			// Config File.
			database.AddContent(nameof(this.ConfigFile), this.ConfigFile);

			// Ignore block map.
			database.AddContent(nameof(this.IgnoreBlockMap), this.IgnoreBlockMap);

			// Save directory.
			database.AddContent(nameof(this.SaveDirectory), this.SaveDirectory);

			// Auto start file.
			database.AddContent(nameof(this.AutoStartFile), this.AutoStartFile);

			// Start-up file kind.
			database.AddEnum(nameof(this.StartUpFileKind), this.StartUpFileKind);

			// Extra options.
			database.AddContent(nameof(this.ExtraOptions), this.ExtraOptions);

			// Pixel mode.
			database.AddEnum(nameof(this.PixelMode), this.PixelMode);

			// Width.
			database.AddContent(nameof(this.Width), this.Width);

			// Height.
			database.AddContent(nameof(this.Height), this.Height);

			// Disable flags.
			database.AddEnum(nameof(this.DisableFlags), this.DisableFlags);

			// Fast monsters.
			database.AddContent(nameof(this.FastMonsters), this.FastMonsters);

			// No monsters.
			database.AddContent(nameof(this.NoMonsters), this.NoMonsters);

			// Respawning monsters.
			database.AddContent(nameof(this.RespawningMonsters), this.RespawningMonsters);

			// Time limit.
			database.AddContent(nameof(this.TimeLimit), this.TimeLimit);

			// Turbo mode.
			database.AddContent(nameof(this.TurboMode), this.TurboMode);

			// Difficulty.
			database.AddContent(nameof(this.Difficulty), this.Difficulty);

			this.SaveExtraFiles(database, gameFolder);

			database.Save(file);
		}
		private void SaveExtraFiles(Database database, string gameFolder)
		{
			string doomWadDir = ExtraFilesLookUp.DoomWadDirectory;

			DatabaseEntry filesEntry = new DatabaseEntry("ExtraFiles", null);
			Uri folderUri = new Uri(PathUtils.EndWithBackSlash(gameFolder), UriKind.Absolute);
			for (int i = 0; i < this.ExtraFiles.Count; i++)
			{
				string filePath = this.ExtraFiles[i];
				TextContent content = new TextContent(Path.GetDirectoryName(filePath) == doomWadDir
					? Path.GetFileName(filePath)
					: PathUtils.ToRelativePath(filePath, folderUri));
				DatabaseEntry entry = new DatabaseEntry($"ExtraFile{i}", content);

				filesEntry.SubEntries.Add(entry.Name, entry);
			}

			database.AddEntry(filesEntry);
		}
		/// <summary>
		/// Loads this configuration from the file.
		/// </summary>
		/// <param name="file">      Path to the file.</param>
		/// <param name="gameFolder">Path to the folder that contains the executables.</param>
		public static LaunchConfiguration Load(string file, string gameFolder)
		{
			if (Path.GetExtension(file) == ".lcf")
			{
				return LoadLegacy(file, gameFolder);
			}

			Database database = new Database("xlcf", "blcf");

			database.Load(file);

			LaunchConfiguration config = new LaunchConfiguration();

			// Name.
			config.Name = database.GetText(nameof(config.Name));

			// Iwad Path.
			config.IwadPath = database.GetText(nameof(config.IwadPath));

			// Config File.
			config.ConfigFile = database.GetText(nameof(config.ConfigFile));

			// Ignore block map.
			config.IgnoreBlockMap = database.GetBool(nameof(config.IgnoreBlockMap));

			// Save directory.
			config.SaveDirectory = database.GetText(nameof(config.SaveDirectory));

			// Auto start file.
			config.AutoStartFile = database.GetText(nameof(config.AutoStartFile));

			// Start-up file kind.
			config.StartUpFileKind = database.GetEnum<StartupFile>(nameof(config.StartUpFileKind));

			// Extra options.
			config.ExtraOptions = database.GetText(nameof(config.ExtraOptions));

			// Pixel mode.
			config.PixelMode = database.GetEnum<PixelMode>(nameof(config.PixelMode));

			// Width.
			config.Width = database.GetInteger(nameof(config.Width));

			// Height.
			config.Height = database.GetInteger(nameof(config.Height));

			// Disable flags.
			config.DisableFlags = database.GetEnum<DisableOptions>(nameof(config.DisableFlags));

			// Fast monsters.
			config.FastMonsters = database.GetBool(nameof(config.FastMonsters));

			// No monsters.
			config.NoMonsters = database.GetBool(nameof(config.NoMonsters));

			// Respawning monsters.
			config.RespawningMonsters = database.GetBool(nameof(config.RespawningMonsters));

			// Time limit.
			config.TimeLimit = database.GetInteger(nameof(config.TimeLimit));

			// Turbo mode.
			config.TurboMode = (byte?)database.GetInteger(nameof(config.TurboMode));

			// Difficulty.
			config.Difficulty = database.GetInteger(nameof(config.Difficulty));

			config.LoadExtraFiles(database, gameFolder);

			return config;
		}
		public void LoadExtraFiles(Database database, string gameFolder)
		{
			if (!database.Contains("ExtraFiles", false))
			{
				return;
			}

			string doomWadDir = ExtraFilesLookUp.DoomWadDirectory;
			DatabaseEntry filesEntry = database["ExtraFiles"];
			string entryNamePart = "ExtraFile";

			// Restore full paths.
			var paths = from nameEntry in filesEntry.SubEntries
						where nameEntry.Key.Substring(0, entryNamePart.Length) == entryNamePart
						let content = nameEntry.Value.GetContent<TextContent>()?.Text
						where content != null
						select content;

			foreach (string filePath in paths)
			{
				string relativeFilePath = filePath;
				string path = Path.Combine(doomWadDir, relativeFilePath);
				if (File.Exists(path))
				{
					this.ExtraFiles.Add(PathUtils.GetLocalPath(path));
				}
				else
				{
					path = Path.Combine(gameFolder, relativeFilePath);
					if (File.Exists(path))
					{
						this.ExtraFiles.Add(PathUtils.GetLocalPath(path));
					}
				}
			}

			// Update directories.
			List<string> dirs = new List<string>(10);
			foreach (string dirName in from extraFile in this.ExtraFiles
									   select Path.GetDirectoryName(extraFile) into dirName
									   where !dirs.Contains(dirName)
									   select dirName)
			{
				dirs.Add(dirName);
			}

			foreach (string dir in dirs.Where(dir => !ExtraFilesLookUp.Directories.Contains(dir)))
			{
				ExtraFilesLookUp.Directories.Add(dir);
			}
		}
		/// <summary>
		/// Loads this configuration from the file.
		/// </summary>
		/// <param name="file">      Path to the file.</param>
		/// <param name="gameFolder">Path to the folder that contains the executables.</param>
		public static LaunchConfiguration LoadLegacy(string file, string gameFolder)
		{
			try
			{
				LaunchConfiguration config;
				using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					BinaryFormatter formatter = new BinaryFormatter();
					config = (LaunchConfiguration)formatter.Deserialize(fs);
				}

				string doomWadDir = ExtraFilesLookUp.DoomWadDirectory;

				// Restore full paths.
				for (int i = 0; i < config.ExtraFiles.Count; i++)
				{
					string relativeFilePath = config.ExtraFiles[i];
					string path = Path.Combine(doomWadDir, relativeFilePath);
					if (File.Exists(path))
					{
						config.ExtraFiles[i] = PathUtils.GetLocalPath(path);
					}
					else
					{
						path = Path.Combine(gameFolder, relativeFilePath);
						if (File.Exists(path))
						{
							config.ExtraFiles[i] = PathUtils.GetLocalPath(path);
						}
						else
						{
							// File cannot be found so remove it.
							config.ExtraFiles.RemoveAt(i--);
						}
					}
				}

				// Update directories.
				List<string> dirs = new List<string>(10);
				foreach (string dirName in from extraFile in config.ExtraFiles
										   select Path.GetDirectoryName(extraFile) into dirName
										   where !dirs.Contains(dirName)
										   select dirName)
				{
					dirs.Add(dirName);
				}

				foreach (string dir in dirs.Where(dir => !ExtraFilesLookUp.Directories.Contains(dir)))
				{
					ExtraFilesLookUp.Directories.Add(dir);
				}

				return config;
			}
			catch (Exception ex)
			{
				Log.Error("{0}: {1}", ex.GetType().FullName, ex.Message);
				return null;
			}
		}
	}
	internal static class DataBaseExtensions
	{
		internal static void AddContent(this Database database, string entryName, string textContent)
		{
			var content = string.IsNullOrEmpty(textContent) ? null : new TextContent(textContent);
			DatabaseEntry entry = new DatabaseEntry(entryName, content);

			database.AddEntry(entry);
		}
		internal static void AddContent(this Database database, string entryName, bool boolContent)
		{
			if (boolContent)
			{
				database.AddEntry(new DatabaseEntry(entryName, null));
			}
		}
		internal static void AddEnum<EnumType>(this Database database, string entryName, EnumType enumContent)
			where EnumType : struct, IComparable, IConvertible, IFormattable
		{
			IntegerContent content = new IntegerContent(enumContent.ToInt64(CultureInfo.InvariantCulture));
			DatabaseEntry entry = new DatabaseEntry(entryName, content);

			database.AddEntry(entry);
		}
		internal static void AddContent(this Database database, string entryName, int? nullableContent)
		{
			if (nullableContent != null)
			{
				IntegerContent content = new IntegerContent(nullableContent.Value);
				DatabaseEntry entry = new DatabaseEntry(entryName, content);

				database.AddEntry(entry);
			}
		}
		internal static string GetText(this Database database, string entryName)
		{
			return database.Contains(entryName, false)
				? database[entryName].GetContent<TextContent>()?.Text
				: "";
		}
		internal static bool GetBool(this Database database, string entryName)
		{
			return database.Contains(entryName, false);
		}
		internal static EnumType GetEnum<EnumType>(this Database database, string entryName)
			where EnumType : struct, IComparable, IConvertible, IFormattable
		{
			if (database.Contains(entryName, false))
			{
				long? value = database[entryName].GetContent<IntegerContent>()?.Value;
				if (value != null)
				{
					EnumType enumValue = Cast<EnumType>.From((int)value.Value);
					return enumValue;
				}
			}
			return default(EnumType);
		}
		internal static int? GetInteger(this Database database, string entryName)
		{
			if (database.Contains(entryName, false))
			{
				return (int?)database[entryName].GetContent<IntegerContent>()?.Value;
			}
			return null;
		}
	}
}