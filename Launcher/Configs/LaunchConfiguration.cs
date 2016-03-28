using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Text;
using Launcher.Extensions;
using Launcher.Logging;

namespace Launcher.Configs
{
	/// <summary>
	/// Represents a launch configuration.
	/// </summary>
	[Serializable]
	public class LaunchConfiguration : ILaunchConfiguration
	{
		/// <summary>
		/// Gets or sets the name of this configuration.
		/// </summary>
		public string Name { get; set; }
		#region Files
		/// <summary>
		/// Path to IWAD file to use.
		/// </summary>
		public string IwadPath { get; set; }
		/// <summary>
		/// Paths to extra files to load.
		/// </summary>
		public List<string> ExtraFiles { get; set; }
		/// <summary>
		/// Path custom config file to use.
		/// </summary>
		public string ConfigFile { get; set; }
		/// <summary>
		/// Indicates whether zDoom needs to ignore block map data supplied by the map, and generate it
		/// instead.
		/// </summary>
		public bool IgnoreBlockMap { get; set; }
		/// <summary>
		/// Path to directory to save games to.
		/// </summary>
		public string SaveDirectory { get; set; }
		/// <summary>
		/// Path to the file that should be started automatically upon startup.
		/// </summary>
		public string AutoStartFile { get; set; }
		/// <summary>
		/// Indicates nature of <see cref="AutoStartFile"/>.
		/// </summary>
		public StartupFile StartUpFileKind { get; set; }
		/// <summary>
		/// Text that is appended to the command line at the end.
		/// </summary>
		public string ExtraOptions { get; set; }
		#endregion
		#region Graphics
		/// <summary>
		/// Pixel mode.
		/// </summary>
		public PixelMode PixelMode { get; set; }
		/// <summary>
		/// Width of the window in pixels.
		/// </summary>
		public int? Width { get; set; }
		/// <summary>
		/// Height of the window in pixels.
		/// </summary>
		public int? Height { get; set; }
		#endregion
		#region Disables
		/// <summary>
		/// Flags that indicates which functions to disable.
		/// </summary>
		public DisableOptions DisableFlags { get; set; }
		#endregion
		#region GamePlay
		/// <summary>
		/// Indicates whether zDoom has to make monsters fast regardless whether the game runs on Nightmare
		/// or not.
		/// </summary>
		public bool FastMonsters { get; set; }
		/// <summary>
		/// Indicates whether zDoom has to make monsters not spawn on the level.
		/// </summary>
		public bool NoMonsters { get; set; }
		/// <summary>
		/// Indicates whether zDoom has to make monsters respawn regardless whether the game runs on
		/// Nightmare or not.
		/// </summary>
		public bool RespawningMonsters { get; set; }
		/// <summary>
		/// Sets the time limit to specified number of minutes.
		/// </summary>
		public int? TimeLimit { get; set; }
		/// <summary>
		/// Sets the movement speed of the player to specified value that is a percentage of normal
		/// movement speed.
		/// </summary>
		public byte? TurboMode { get; set; }
		/// <summary>
		/// Sets difficulty level.
		/// </summary>
		public int? Difficulty { get; set; }
		#endregion
		#region Save Load
		/// <summary>
		/// Saves this configuration to the file.
		/// </summary>
		/// <param name="file">Path to the file.</param>
		/// <param name="gameFolder">Path to the folder that contains the executables.</param>
		public void Save(string file, string gameFolder)
		{
			try
			{
				string doomWadDir = ExtraFilesLookUp.DoomWadDirectory;

				List<string> files = this.ExtraFiles;

				this.ExtraFiles = new List<string>(files.Count);

				Uri folderUri = new Uri(PathUtils.EndWithBackSlash(gameFolder), UriKind.Absolute);
				foreach (string filePath in files)
				{
					// If the file is located in DOOMWADDIR, then just save the name.
					this.ExtraFiles.Add(Path.GetDirectoryName(filePath) == doomWadDir
						? Path.GetFileName(filePath)
						: PathUtils.ToRelativePath(filePath, folderUri));
				}

				using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					BinaryFormatter formatter = new BinaryFormatter();
					formatter.Serialize(fs, this);
				}

				this.ExtraFiles = files;
			}
			catch (Exception ex)
			{
				Log.Error("{0}: {1}", ex.GetType().FullName, ex.Message);
			}
		}
		/// <summary>
		/// Loads this configuration from the file.
		/// </summary>
		/// <param name="file">Path to the file.</param>
		/// <param name="gameFolder">Path to the folder that contains the executables.</param>
		public static LaunchConfiguration Load(string file, string gameFolder)
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
		#endregion
		#region Command Line
		/// <summary>
		/// Gets command line that can be used to launch zDoom with this configuration.
		/// </summary>
		/// <param name="exeFolder">Path to the folder that contains the executable.</param>
		/// <exception cref="ArgumentOutOfRangeException">Unknown enumeration value.</exception>
		public string GetCommandLine(string exeFolder)
		{
			StringBuilder line = new StringBuilder();
			// IWAD.
			if (!string.IsNullOrWhiteSpace(this.IwadPath))
			{
				line.Append("-iwad ");
				line.Append(this.IwadPath);
			}
			// Config file.
			if (!string.IsNullOrWhiteSpace(this.ConfigFile))
			{
				line.Append(" -config ");
				line.Append(GetValidPath(this.ConfigFile, null));
			}
			// Extras.
			if (this.ExtraFiles.Count > 0)
			{
				// Wads.
				var wads =
					this.ExtraFiles
						.Where(x => Path.GetExtension(x) != ".bex" && Path.GetExtension(x) != ".deh")
						.GetEnumerator();
				if (wads.MoveNext())
				{
					line.Append(" -file ");
					line.Append(GetValidPath(wads.Current, exeFolder));
					while (wads.MoveNext())
					{
						line.Append(" ");
						line.Append(GetValidPath(wads.Current, exeFolder));
					}
				}
				// Patches.
				var bexPatches =
					this.ExtraFiles.Where(x => Path.GetExtension(x) == ".bex").GetEnumerator();
				if (bexPatches.MoveNext())
				{
					line.Append(" -bex ");
					line.Append(GetValidPath(bexPatches.Current, exeFolder));
					while (bexPatches.MoveNext())
					{
						line.Append(" ");
						line.Append(GetValidPath(bexPatches.Current, exeFolder));
					}
				}
				var dehPatches =
					this.ExtraFiles.Where(x => Path.GetExtension(x) == ".deh").GetEnumerator();
				if (dehPatches.MoveNext())
				{
					line.Append(" -deh ");
					line.Append(GetValidPath(dehPatches.Current, exeFolder));
					while (dehPatches.MoveNext())
					{
						line.Append(" ");
						line.Append(GetValidPath(dehPatches.Current, exeFolder));
					}
				}
			}
			// Graphics.
			switch (this.PixelMode)
			{
				case PixelMode.NoChange:
					break;
				case PixelMode.Double:
					line.Append(" -2");
					break;
				case PixelMode.Quad:
					line.Append(" -4");
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			if (this.Width.HasValue)
			{
				line.Append(" -width ");
				line.Append(this.Width.Value);
			}
			if (this.Height.HasValue)
			{
				line.Append(" -height ");
				line.Append(this.Height.Value);
			}
			// Some more files.
			if (this.IgnoreBlockMap)
			{
				line.Append(" -blockmap");
			}
			if (!string.IsNullOrWhiteSpace(this.SaveDirectory))
			{
				line.Append(" -savedir ");
				line.Append(this.SaveDirectory);
			}
			if (!string.IsNullOrWhiteSpace(this.AutoStartFile))
			{
				switch (this.StartUpFileKind)
				{
					case StartupFile.SaveGame:
						line.Append(" -loadgame ");
						line.Append(this.AutoStartFile);
						break;
					case StartupFile.Demo:
						line.Append(" -playdemo ");
						line.Append(this.AutoStartFile);
						break;
					case StartupFile.Map:
						line.Append(" -warp ");
						line.Append(this.AutoStartFile);
						line.Append(" -warpwipe");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			// Gameplay options.
			if (this.FastMonsters)
			{
				line.Append(" -fast");
			}
			if (this.NoMonsters)
			{
				line.Append(" -nomonsters");
			}
			if (this.RespawningMonsters)
			{
				line.Append(" -respawn");
			}
			if (this.TimeLimit.HasValue)
			{
				line.Append(" -timer ");
				line.Append(this.TimeLimit.Value.ToString(CultureInfo.InvariantCulture));
			}
			if (this.TurboMode.HasValue)
			{
				line.Append(" -turbo ");
				line.Append(this.TurboMode.Value.ToString(CultureInfo.InvariantCulture));
			}
			if (this.Difficulty.HasValue)
			{
				line.Append(" -skill ");
				line.Append(this.Difficulty.Value.ToString(CultureInfo.InvariantCulture));
			}
			// Disable.
			if (this.DisableFlags.HasFlag(DisableOptions.AutoLoad))
			{
				line.Append(" -noautoload");
			}
			if (this.DisableFlags.HasFlag(DisableOptions.CompactDiskAudio))
			{
				line.Append(" -nocdaudio");
			}
			if (this.DisableFlags.HasFlag(DisableOptions.Idling))
			{
				line.Append(" -noidle");
			}
			if (this.DisableFlags.HasFlag(DisableOptions.JoyStick))
			{
				line.Append(" -nojoy");
			}
			if (!this.DisableFlags.HasFlag(DisableOptions.Sound))
			{
				if (this.DisableFlags.HasFlag(DisableOptions.Music))
				{
					line.Append(" -nomusic");
				}
				if (this.DisableFlags.HasFlag(DisableOptions.SoundEffects))
				{
					line.Append(" -nosfx");
				}
			}
			else
			{
				line.Append(" -nosound");
			}
			if (this.DisableFlags.HasFlag(DisableOptions.SpriteRenaming))
			{
				line.Append(" -oldsprites");
			}
			if (this.DisableFlags.HasFlag(DisableOptions.StartupScreens))
			{
				line.Append(" -nostartup");
			}
			// Last thing.
			if (!string.IsNullOrWhiteSpace(this.ExtraOptions))
			{
				line.Append(" ");
				line.Append(this.ExtraOptions);
			}
			return line.ToString();
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates default configuration.
		/// </summary>
		public LaunchConfiguration()
		{
			this.Name = "Default Configuration";
			this.IwadPath = "";
			this.ConfigFile = "";
			this.ExtraFiles = new List<string>();
			this.IgnoreBlockMap = false;
			this.SaveDirectory = "";
			this.StartUpFileKind = StartupFile.None;
			this.AutoStartFile = "";
			this.ExtraOptions = "";
			this.Width = null;
			this.Height = null;
			this.PixelMode = PixelMode.NoChange;
			this.DisableFlags = DisableOptions.EnableAll;
			this.FastMonsters = false;
			this.NoMonsters = false;
			this.RespawningMonsters = false;
			this.TimeLimit = null;
			this.TurboMode = null;
			this.Difficulty = null;
		}
		#endregion
		#region Utilities
		// Creates a string that represents a path to the file that is properly recognized by the command
		// line interpreter.
		private static string GetValidPath(string file, string exeFolder)
		{
			string path = null;

			if (exeFolder != null)
			{
				string doomWadDir = ExtraFilesLookUp.DoomWadDirectory;

				if (!string.IsNullOrWhiteSpace(doomWadDir) && Path.GetDirectoryName(file) == doomWadDir)
				{
					path = Path.GetFileName(file);
				}
				else
				{
					path = PathUtils.ToRelativePath(file, exeFolder);
				}
			}

			return path != null && path.Any(char.IsWhiteSpace) ? "\"" + path + "\"" : path;
		}
		#endregion
	}
	/// <summary>
	/// Enumeration of pixel modes.
	/// </summary>
	public enum PixelMode
	{
		/// <summary>
		/// Standard pixels.
		/// </summary>
		NoChange,
		/// <summary>
		/// Doubles picture size dimensions by enabling pixel doubling.
		/// </summary>
		Double,
		/// <summary>
		/// Quadruples picture size dimensions by enabling pixel quadrupling.
		/// </summary>
		Quad
	}
	/// <summary>
	/// Enumeration of flags that, when set, instruct zDoom to disable certain functions.
	/// </summary>
	[Flags]
	public enum DisableOptions
	{
		/// <summary>
		/// When set everything should be enabled.
		/// </summary>
		EnableAll = 0,
		/// <summary>
		/// When set instructs zDoom to disable CD audio.
		/// </summary>
		CompactDiskAudio = 1,
		/// <summary>
		/// When set instructs zDoom to disable function that lowers zDoom process priority when player
		/// alt-tabs away.
		/// </summary>
		Idling = 2,
		/// <summary>
		/// When set instructs zDoom to disable joy stick control method.
		/// </summary>
		JoyStick = 4,
		/// <summary>
		/// When set instructs zDoom to disable music in the game.
		/// </summary>
		Music = 8,
		/// <summary>
		/// When set instructs zDoom to disable sound effects.
		/// </summary>
		SoundEffects = 16,
		/// <summary>
		/// When set instructs zDoom to disable both music and sound effects.
		/// </summary>
		Sound = 24,
		/// <summary>
		/// When set instructs zDoom to disable startup screens for Heretic, Hexen and Strife.
		/// </summary>
		StartupScreens = 32,
		/// <summary>
		/// When set instructs zDoom to disable sprite renaming used in user-created files for Heretic,
		/// Hexen or Strife.
		/// </summary>
		SpriteRenaming = 64,
		/// <summary>
		/// When set instructs zDoom to disable auto-loading files.
		/// </summary>
		AutoLoad = 128
	}
	/// <summary>
	/// Enumeration of kinds of startup files.
	/// </summary>
	public enum StartupFile
	{
		/// <summary>
		/// Nothing is done automatically at the start.
		/// </summary>
		None,
		/// <summary>
		/// Save game.
		/// </summary>
		SaveGame,
		/// <summary>
		/// Game play demo.
		/// </summary>
		Demo,
		/// <summary>
		/// Map.
		/// </summary>
		Map
	}
}