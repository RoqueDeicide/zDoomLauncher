using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Launcher.Databases;
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
			var database = new Database("xlcf", "blcf");

			database.AddEnum(nameof(this.StartupAction), this.StartupAction);
			database.AddEnum(nameof(this.PixelMode),     this.PixelMode);
			database.AddEnum(nameof(this.DisableFlags),  this.DisableFlags);

			database.AddContent(nameof(this.Name),               this.Name);
			database.AddContent(nameof(this.IwadFile),           this.IwadFile.FileName);
			database.AddContent(nameof(this.ConfigFile),         this.ConfigFile);
			database.AddContent(nameof(this.SaveDirectory),      this.SaveDirectory);
			database.AddContent(nameof(this.SaveGamePath),       this.SaveGamePath);
			database.AddContent(nameof(this.DemoPath),           this.DemoPath);
			database.AddContent(nameof(this.EpisodeIndex),       this.EpisodeIndex);
			database.AddContent(nameof(this.MapIndex),           this.MapIndex);
			database.AddContent(nameof(this.MapName),            this.MapName);
			database.AddContent(nameof(this.ExtraOptions),       this.ExtraOptions);
			database.AddContent(nameof(this.SpecifyWidth),       this.SpecifyWidth);
			database.AddContent(nameof(this.Width),              this.Width);
			database.AddContent(nameof(this.SpecifyHeight),      this.SpecifyHeight);
			database.AddContent(nameof(this.Height),             this.Height);
			database.AddContent(nameof(this.FastMonsters),       this.FastMonsters);
			database.AddContent(nameof(this.NoMonsters),         this.NoMonsters);
			database.AddContent(nameof(this.RespawningMonsters), this.RespawningMonsters);
			database.AddContent(nameof(this.SpecifyTimeLimit),   this.SpecifyTimeLimit);
			database.AddContent(nameof(this.TimeLimit),          this.TimeLimit);
			database.AddContent(nameof(this.SpecifyTurboMode),   this.SpecifyTurboMode);
			database.AddContent(nameof(this.TurboMode),          this.TurboMode);
			database.AddContent(nameof(this.SpecifyDifficulty),  this.SpecifyDifficulty);
			database.AddContent(nameof(this.Difficulty),         this.Difficulty);

			this.SaveExtraFiles(database, gameFolder);

			database.Save(file);
		}

		private void SaveExtraFiles(Database database, string gameFolder)
		{
			string doomWadDir = ExtraFilesLookUp.DoomWadDirectory;

			var filesEntry = new DatabaseEntry("ExtraFiles", null);
			var folderUri  = new Uri(PathUtils.EndWithBackSlash(gameFolder), UriKind.Absolute);

			// Maximal number of digits that can used to designate an index of the entry.
			int digitCount = (int)Math.Floor(Math.Log10(this.ExtraFiles.Count) + 1);
			for (int i = 0; i < this.ExtraFiles.Count; i++)
			{
				string filePath = this.ExtraFiles[i];
				var content = new TextContent(Path.GetDirectoryName(filePath) == doomWadDir
												  ? Path.GetFileName(filePath)
												  : PathUtils.ToRelativePath(filePath, folderUri));
				var entry = new DatabaseEntry($"ExtraFile{i.ToString($"D{digitCount}")}",
											  content);

				filesEntry.SubEntries.Add(entry.Name, entry);
			}

			database.AddEntry(filesEntry);
		}

		/// <summary>
		/// Loads this configuration from the file.
		/// </summary>
		/// <param name="file">      Path to the file.</param>
		/// <param name="gameFolder">Path to the folder that contains the executables.</param>
		public void Load(string file, string gameFolder)
		{
			var database = new Database("xlcf", "blcf");

			database.Load(file);

			string f = database.GetText(nameof(this.IwadFile));
			if (f.IsNullOrWhiteSpace())
			{
				// Older format.
				f = database.GetText("IwadPath");
			}
			this.IwadFile = Iwads.SupportedIwads.Find(x => x.FileName == f);

			this.Name               = database.GetText(nameof(this.Name));
			this.ConfigFile         = database.GetText(nameof(this.ConfigFile));
			this.SaveDirectory      = database.GetText(nameof(this.SaveDirectory));
			this.SaveGamePath       = database.GetText(nameof(this.SaveGamePath));
			this.DemoPath           = database.GetText(nameof(this.DemoPath));
			this.MapName            = database.GetText(nameof(this.MapName));
			this.StartupAction      = database.GetEnum<StartupAction>(nameof(this.StartupAction));
			this.ExtraOptions       = database.GetText(nameof(this.ExtraOptions));
			this.PixelMode          = database.GetEnum<PixelMode>(nameof(this.PixelMode));
			this.SpecifyWidth       = database.GetBool(nameof(this.SpecifyWidth));
			this.SpecifyHeight      = database.GetBool(nameof(this.SpecifyHeight));
			this.DisableFlags       = database.GetEnum<DisableOptions>(nameof(this.DisableFlags));
			this.FastMonsters       = database.GetBool(nameof(this.FastMonsters));
			this.NoMonsters         = database.GetBool(nameof(this.NoMonsters));
			this.RespawningMonsters = database.GetBool(nameof(this.RespawningMonsters));
			this.SpecifyTimeLimit   = database.GetBool(nameof(this.SpecifyTimeLimit));
			this.SpecifyTurboMode   = database.GetBool(nameof(this.SpecifyTurboMode));
			this.SpecifyDifficulty  = database.GetBool(nameof(this.SpecifyDifficulty));

			database.GetInteger(nameof(this.EpisodeIndex), x => this.EpisodeIndex = x);
			database.GetInteger(nameof(this.MapIndex),     x => this.MapIndex     = x);
			database.GetInteger(nameof(this.Width),        x => this.Width        = x);
			database.GetInteger(nameof(this.Height),       x => this.Height       = x);
			database.GetInteger(nameof(this.TimeLimit),    x => this.TimeLimit    = x);
			database.GetInteger(nameof(this.TurboMode),    x => this.TurboMode    = (byte)x);
			database.GetInteger(nameof(this.Difficulty),   x => this.Difficulty   = x);

			this.LoadExtraFiles(database, gameFolder);
		}

		public void LoadExtraFiles(Database database, string gameFolder)
		{
			if (!database.Contains("ExtraFiles", false))
			{
				return;
			}

			string        doomWadDir    = ExtraFilesLookUp.DoomWadDirectory;
			DatabaseEntry filesEntry    = database["ExtraFiles"];
			const string  entryNamePart = "ExtraFile";

			// Restore full paths.
			var paths = from nameEntry in filesEntry.SubEntries
						where nameEntry.Key.Substring(0, entryNamePart.Length) == entryNamePart
						let content = nameEntry.Value.GetContent<TextContent>()?.Text
						where content != null
						select content;

			foreach (string filePath in paths)
			{
				string relativeFilePath = filePath;
				string path             = Path.Combine(doomWadDir ?? "", relativeFilePath);
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
			var dirs = new List<string>(10);
			foreach (string dirName in from extraFile in this.ExtraFiles
									   select Path.GetDirectoryName(extraFile)
									   into dirName
									   where !dirs.Contains(dirName)
									   select dirName)
			{
				dirs.Add(dirName);
			}

			foreach (string dir in dirs)
			{
				ExtraFilesLookUp.AddDirectory(dir);
			}
		}
	}

	internal static class DataBaseExtensions
	{
		internal static void AddContent(this Database database, string entryName, string textContent)
		{
			TextContent content = string.IsNullOrEmpty(textContent) ? null : new TextContent(textContent);
			var         entry   = new DatabaseEntry(entryName, content);

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
			var content = new IntegerContent(enumContent.ToInt64(CultureInfo.InvariantCulture));
			var entry   = new DatabaseEntry(entryName, content);

			database.AddEntry(entry);
		}

		internal static void AddContent(this Database database, string entryName, int number)
		{
			var content = new IntegerContent(number);
			var entry   = new DatabaseEntry(entryName, content);

			database.AddEntry(entry);
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
			if (!database.Contains(entryName, false)) return default;

			var value = database[entryName].GetContent<IntegerContent>()?.Value;

			if (value == null) return default;

			EnumType enumValue = Cast<EnumType>.From((int)value.Value);
			return enumValue;
		}

		internal static void GetInteger(this Database database, string entryName, Action<int> assigner)
		{
			if (database.Contains(entryName, false))
			{
				var content = database[entryName].GetContent<IntegerContent>();
				if (content != null)
				{
					assigner((int)database[entryName].GetContent<IntegerContent>().Value);
				}
			}
		}
	}
}