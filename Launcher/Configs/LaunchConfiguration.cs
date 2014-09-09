using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Documents;

namespace Launcher.Configs
{
	/// <summary>
	/// Represents a launch configuration.
	/// </summary>
	public class LaunchConfiguration : ILaunchConfiguration
	{
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
		/// Indicates whether zDoom needs to ignore block map data supplied by the map, and generate it instead.
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
		public StartupFile StartUpFileKind{ get; set; }
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
		/// Number of bits per pixel.
		/// </summary>
		public ColorMode ColorMode { get; set; }
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
		/// Indicates whether zDoom has to make monsters fast regardless whether the game runs on Nightmare or not.
		/// </summary>
		public bool FastMonsters { get; set; }
		/// <summary>
		/// Indicates whether zDoom has to make monsters not spawn on the level.
		/// </summary>
		public bool NoMonsters { get; set; }
		/// <summary>
		/// Indicates whether zDoom has to make monsters respawn regardless whether the game runs on Nightmare or not.
		/// </summary>
		public bool RespawningMonsters { get; set; }
		/// <summary>
		/// Sets the time limit to specified number of minutes.
		/// </summary>
		public int? TimeLimit { get; set; }
		/// <summary>
		/// Sets the movement speed of the player to specified value that is a percentage of normal movement speed.
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
		public void Save(string file)
		{
			using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(fs, this);
			}
		}
		/// <summary>
		/// Loads this configuration from the file.
		/// </summary>
		/// <param name="file">Path to the file.</param>
		public static LaunchConfiguration Load(string file)
		{
			using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				return (LaunchConfiguration)formatter.Deserialize(fs);
			}
		}
		#endregion
		#region Command Line
		/// <summary>
		/// Gets command line that can be used to launch zDoom with this configuration.
		/// </summary>
		public string CommandLine
		{
			get
			{
				StringBuilder line = new StringBuilder();
				line.Append("zdoom.exe");
				// IWAD.
				if (this.IwadPath != null)
				{
					line.Append(" -iwad ");
					line.Append(this.IwadPath);
				}
				// Config file.
				if (this.ConfigFile != null)
				{
					line.Append(" -config ");
					line.Append(this.ConfigFile);
				}
				// Extras.
				if (this.ExtraFiles.Count > 0)
				{
					// Wads.
					var wads =
						this.ExtraFiles.Where(x => Path.GetExtension(x) == ".wad").GetEnumerator();
					wads.Reset();
					if (wads.MoveNext())
					{
						line.Append(" -file ");
						line.Append(wads.Current);
						while (wads.MoveNext())
						{
							line.Append(" ");
							line.Append(wads.Current);
						}
					}
					// Patches.
					var bexPatches =
						this.ExtraFiles.Where(x => Path.GetExtension(x) == ".bex").GetEnumerator();
					bexPatches.Reset();
					if (bexPatches.MoveNext())
					{
						line.Append(" -bex ");
						line.Append(bexPatches.Current);
						while (bexPatches.MoveNext())
						{
							line.Append(" ");
							line.Append(bexPatches.Current);
						}
					}
					var dehPatches =
						this.ExtraFiles.Where(x => Path.GetExtension(x) == ".deh").GetEnumerator();
					dehPatches.Reset();
					if (dehPatches.MoveNext())
					{
						line.Append(" -deh ");
						line.Append(dehPatches.Current);
						while (dehPatches.MoveNext())
						{
							line.Append(" ");
							line.Append(dehPatches.Current);
						}
					}
				}
				// Graphics.
				switch (this.ColorMode)
				{
					case ColorMode.Bits8:
						line.Append(" -bits 8");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
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
				if (this.SaveDirectory != null)
				{
					line.Append(" -savedir ");
					line.Append(this.SaveDirectory);
				}
				if (this.AutoStartFile != null)
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
				if (this.ExtraOptions != null)
				{
					line.Append(" ");
					line.Append(this.ExtraOptions);
				}
				return line.ToString();
			}
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
	/// Enumeration of color modes.
	/// </summary>
	public enum ColorMode
	{
		/// <summary>
		/// 8-bit color mode.
		/// </summary>
		Bits8
	}
	/// <summary>
	/// Enumeration of flags that, when set, instruct zDoom to disable certain functions.
	/// </summary>
	[Flags]
	public enum DisableOptions
	{
		/// <summary>
		/// When set instructs zDoom to disable CD audio.
		/// </summary>
		CompactDiskAudio = 1,
		/// <summary>
		/// When set instructs zDoom to disable function that lowers zDoom process priority when player alt-tabs away.
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
		/// When set instructs zDoom to disable sprite renaming used in user-created files for Heretic, Hexen or Strife.
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
