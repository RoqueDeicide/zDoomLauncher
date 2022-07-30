using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Launcher.Utilities;

namespace Launcher.Configs
{
	/// <summary>
	/// Represents a launch configuration.
	/// </summary>
	public partial class LaunchConfiguration : INotifyPropertyChanged
	{
		private string                       name;
		private IwadFile                     iwadFile;
		private ObservableCollection<string> extraFiles;
		private string                       configFile;
		private string                       saveDirectory;
		private string                       saveGamePath;
		private string                       demoPath;
		private int                          episodeIndex;
		private int                          mapIndex;
		private string                       mapName;
		private StartupAction                startupAction;
		private string                       extraOptions;
		private PixelMode                    pixelMode;
		private bool                         specifyWidth;
		private int                          width;
		private bool                         specifyHeight;
		private int                          height;
		private DisableOptions               disableFlags;
		private bool                         fastMonsters;
		private bool                         noMonsters;
		private bool                         respawningMonsters;
		private bool                         specifyTimeLimit;
		private int                          timeLimit;
		private bool                         specifyTurboMode;
		private byte                         turboMode;
		private bool                         specifyDifficulty;
		private int                          difficulty;

		/// <summary>
		/// Gets or sets the name of this configuration.
		/// </summary>
		public string Name
		{
			get => this.name;
			set
			{
				if (this.name != value)
				{
					this.name = value;
					this.OnPropertyChanged();
				}
			}
		}

		#region Files

		/// <summary>
		/// Path to IWAD file to use.
		/// </summary>
		public IwadFile IwadFile
		{
			get => this.iwadFile;
			set
			{
				if (this.iwadFile != value)
				{
					this.iwadFile = value;
					this.OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Paths to extra files to load.
		/// </summary>
		public ObservableCollection<string> ExtraFiles
		{
			get => this.extraFiles;
			set
			{
				if (Equals(value, this.extraFiles)) return;
				this.extraFiles = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Path custom config file to use.
		/// </summary>
		public string ConfigFile
		{
			get => this.configFile;
			set
			{
				if (value == this.configFile) return;
				this.configFile = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Path to directory to save games to.
		/// </summary>
		public string SaveDirectory
		{
			get => this.saveDirectory;
			set
			{
				if (value == this.saveDirectory) return;
				this.saveDirectory = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Path to the file that contains a saved game that should be loaded if <see cref="StartupAction"/> is equal to <see
		/// cref="Configs.StartupAction.LoadGame"/>.
		/// </summary>
		public string SaveGamePath
		{
			get => this.saveGamePath;
			set
			{
				if (value == this.saveGamePath) return;
				this.saveGamePath = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Path to the file that contains a game-play demo recording that should be loaded if <see cref="StartupAction"/> is equal to <see
		/// cref="Configs.StartupAction.LoadDemo"/>.
		/// </summary>
		public string DemoPath
		{
			get => this.demoPath;
			set
			{
				if (value == this.demoPath) return;
				this.demoPath = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// An index of an episode that should be loaded if <see cref="StartupAction"/> is equal to <see cref="Configs.StartupAction.LoadMapIndex"/>.
		/// </summary>
		public int EpisodeIndex
		{
			get => this.episodeIndex;
			set
			{
				if (value.Equals(this.episodeIndex)) return;
				this.episodeIndex = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// An index of a map that should be loaded if <see cref="StartupAction"/> is equal to <see cref="Configs.StartupAction.LoadMapIndex"/>.
		/// </summary>
		public int MapIndex
		{
			get => this.mapIndex;
			set
			{
				if (value.Equals(this.mapIndex)) return;
				this.mapIndex = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Name of the map that should be loaded if <see cref="StartupAction"/> is equal to <see cref="Configs.StartupAction.LoadMapName"/>.
		/// </summary>
		public string MapName
		{
			get => this.mapName;
			set
			{
				if (value == this.mapName) return;
				this.mapName = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Indicates how to start the game.
		/// </summary>
		public StartupAction StartupAction
		{
			get => this.startupAction;
			set
			{
				if (value == this.startupAction) return;
				this.startupAction = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Text that is appended to the command line at the end.
		/// </summary>
		public string ExtraOptions
		{
			get => this.extraOptions;
			set
			{
				if (value == this.extraOptions) return;
				this.extraOptions = value;
				this.OnPropertyChanged();
			}
		}

		#endregion

		#region Graphics

		/// <summary>
		/// Pixel mode.
		/// </summary>
		public PixelMode PixelMode
		{
			get => this.pixelMode;
			set
			{
				if (value == this.pixelMode) return;
				this.pixelMode = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Indicates whether custom width should be specified.
		/// </summary>
		public bool SpecifyWidth
		{
			get => this.specifyWidth;
			set
			{
				if (value == this.specifyWidth) return;
				this.specifyWidth = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Width of the window in pixels.
		/// </summary>
		public int Width
		{
			get => this.width;
			set
			{
				if (value == this.width) return;
				this.width = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Indicates whether custom height should be specified.
		/// </summary>
		public bool SpecifyHeight
		{
			get => this.specifyHeight;
			set
			{
				if (value == this.specifyHeight) return;
				this.specifyHeight = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Height of the window in pixels.
		/// </summary>
		public int Height
		{
			get => this.height;
			set
			{
				if (value == this.height) return;
				this.height = value;
				this.OnPropertyChanged();
			}
		}

		#endregion

		#region Disables

		/// <summary>
		/// Flags that indicates which functions to disable.
		/// </summary>
		public DisableOptions DisableFlags
		{
			get => this.disableFlags;
			set
			{
				if (value == this.disableFlags) return;
				this.disableFlags = value;
				this.OnPropertyChanged();
			}
		}

		#endregion

		#region GamePlay

		/// <summary>
		/// Indicates whether zDoom has to make monsters fast regardless whether the game runs on Nightmare or not.
		/// </summary>
		public bool FastMonsters
		{
			get => this.fastMonsters;
			set
			{
				if (value == this.fastMonsters) return;
				this.fastMonsters = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Indicates whether zDoom has to make monsters not spawn on the level.
		/// </summary>
		public bool NoMonsters
		{
			get => this.noMonsters;
			set
			{
				if (value == this.noMonsters) return;
				this.noMonsters = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Indicates whether zDoom has to make monsters respawn regardless whether the game runs on Nightmare or not.
		/// </summary>
		public bool RespawningMonsters
		{
			get => this.respawningMonsters;
			set
			{
				if (value == this.respawningMonsters) return;
				this.respawningMonsters = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Indicates whether a time-limit should be specified.
		/// </summary>
		public bool SpecifyTimeLimit
		{
			get => this.specifyTimeLimit;
			set
			{
				if (value == this.specifyTimeLimit) return;
				this.specifyTimeLimit = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Sets the time limit to specified number of minutes.
		/// </summary>
		public int TimeLimit
		{
			get => this.timeLimit;
			set
			{
				if (value == this.timeLimit) return;
				this.timeLimit = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Indicates whether a turbo-mode should be specified.
		/// </summary>
		public bool SpecifyTurboMode
		{
			get => this.specifyTurboMode;
			set
			{
				if (value == this.specifyTurboMode) return;
				this.specifyTurboMode = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Sets the movement speed of the player to specified value that is a percentage of normal movement speed.
		/// </summary>
		public byte TurboMode
		{
			get => this.turboMode;
			set
			{
				if (value == this.turboMode) return;
				this.turboMode = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Indicates whether a difficulty should be specified.
		/// </summary>
		public bool SpecifyDifficulty
		{
			get => this.specifyDifficulty;
			set
			{
				if (value == this.specifyDifficulty) return;
				this.specifyDifficulty = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Sets difficulty level.
		/// </summary>
		public int Difficulty
		{
			get => this.difficulty;
			set
			{
				if (value == this.difficulty) return;
				this.difficulty = value;
				this.OnPropertyChanged();
			}
		}

		#endregion

		#region Command Line

		private static void AppendExtraFileList(CommandLineArgumentsBuilder commandLine, IEnumerable<string> files, string argument, string exeFolder,
												string                      requiredExtension = null)
		{
			var selectedFiles = (from extraFile in files
								 let extension = Path.GetExtension(extraFile)
								 where requiredExtension == null ? extension != ".bex" && extension != ".deh" : extension == requiredExtension
								 select extraFile).ToList();

			commandLine.AddIf(argument, selectedFiles.Count > 0);

			foreach (string selectedFile in selectedFiles)
			{
				commandLine.Add(GetValidPath(selectedFile, exeFolder));
			}
		}

		/// <summary>
		/// Gets command line that can be used to launch zDoom with this configuration.
		/// </summary>
		/// <param name="exeFolder">Path to the folder that contains the executable.</param>
		/// <exception cref="ArgumentOutOfRangeException">Unknown enumeration value.</exception>
		public string GetCommandLine(string exeFolder)
		{
			var line = new CommandLineArgumentsBuilder();

			// IWAD.
			line.AddIf("-iwad", this.IwadFile.FileName, this.IwadFile != null && !this.IwadFile.FileName.IsNullOrWhiteSpace());

			// Config file.
			line.AddIf("-config", GetValidPath(this.ConfigFile, exeFolder, false), !this.configFile.IsNullOrWhiteSpace());

			// Extras.
			if (this.ExtraFiles.Count > 0)
			{
				AppendExtraFileList(line, this.extraFiles, "-file", exeFolder);
				AppendExtraFileList(line, this.extraFiles, "-bex",  exeFolder, ".bex");
				AppendExtraFileList(line, this.extraFiles, "-deh",  exeFolder, ".deh");
			}

			// Graphics.
			switch (this.PixelMode)
			{
				case PixelMode.Single:
					break;

				case PixelMode.Double:
					line.Add("-2");
					break;

				case PixelMode.Quad:
					line.Add("-4");
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			line.AddIf("-width",  this.width.ToString(),  this.specifyWidth);
			line.AddIf("-height", this.height.ToString(), this.specifyHeight);

			line.AddIf("-savedir", GetValidPath(this.SaveDirectory, exeFolder, false), !this.saveDirectory.IsNullOrWhiteSpace());

			switch (this.StartupAction)
			{
				case StartupAction.LoadGame:
					line.AddIf("-loadgame", GetValidPath(this.saveGamePath, exeFolder), !this.saveGamePath.IsNullOrWhiteSpace());
					break;

				case StartupAction.LoadDemo:
					line.AddIf("-playdemo", GetValidPath(this.demoPath, exeFolder), !this.demoPath.IsNullOrWhiteSpace());
					break;

				case StartupAction.LoadMapIndex:
					line.Add("-warp", this.iwadFile == null || this.iwadFile.Episodic ? $"{this.episodeIndex}" : null, $"{this.mapIndex}", "-warpwipe");
					break;

				case StartupAction.LoadMapName:
					line.AddIf("+map", this.mapName, !this.mapName.IsNullOrWhiteSpace());
					break;

				case StartupAction.None:
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			// Gameplay options.
			line.AddIf("-fast",       this.FastMonsters);
			line.AddIf("-nomonsters", this.NoMonsters);
			line.AddIf("-respawn",    this.RespawningMonsters);

			line.AddIf("-timer", this.TimeLimit.ToString(CultureInfo.InvariantCulture),  this.SpecifyTimeLimit);
			line.AddIf("-turbo", this.TurboMode.ToString(CultureInfo.InvariantCulture),  this.specifyTurboMode);
			line.AddIf("-skill", this.Difficulty.ToString(CultureInfo.InvariantCulture), this.specifyDifficulty);

			// Disable.
			line.AddIf(@"-noautoload", this.DisableFlags.HasFlag(DisableOptions.AutoLoad));
			line.AddIf(@"-nocdaudio",  this.DisableFlags.HasFlag(DisableOptions.CompactDiskAudio));
			line.AddIf(@"-noidle",     this.DisableFlags.HasFlag(DisableOptions.Idling));
			line.AddIf(@"-nojoy",      this.DisableFlags.HasFlag(DisableOptions.JoyStick));

			if (!this.DisableFlags.HasFlag(DisableOptions.Sound))
			{
				line.AddIf(@"-nomusic", this.DisableFlags.HasFlag(DisableOptions.Music));
				line.AddIf(@"-nosfx",   this.DisableFlags.HasFlag(DisableOptions.SoundEffects));
			}
			else
			{
				line.Add(@"-nosound");
			}

			line.AddIf(@"-blockmap",   this.disableFlags.HasFlag(DisableOptions.BlockMapLoad));
			line.AddIf(@"-oldsprites", this.DisableFlags.HasFlag(DisableOptions.SpriteRenaming));
			line.AddIf(@"-nostartup",  this.DisableFlags.HasFlag(DisableOptions.StartupScreens));

			// Last thing.
			if (!string.IsNullOrWhiteSpace(this.ExtraOptions))
			{
				line.LineBuilder.Append($" {this.ExtraOptions}");
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
			this.Reset();
		}

		#endregion

		#region Utilities

		// Converts between relative and absolute paths.
		private static string GetValidPath(string file, string exeFolder, bool toRelative = true)
		{
			if (file.IsNullOrWhiteSpace())
			{
				return file;
			}

			string path = null;

			if (toRelative)
			{
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
			}
			else
			{
				path = Path.IsPathRooted(file)
						   ? file
						   : PathUtils.GetLocalPath(Path.Combine(exeFolder, file));
			}

			return path;
		}

		#endregion

		/// <summary>
		/// Resets this object to its default state.
		/// </summary>
		public void Reset()
		{
			this.Name               = "Default Configuration";
			this.IwadFile           = Iwads.SupportedIwads[0];
			this.ConfigFile         = "";
			this.ExtraFiles         = new ObservableCollection<string>();
			this.SaveDirectory      = "";
			this.StartupAction      = StartupAction.None;
			this.SaveGamePath       = "";
			this.DemoPath           = "";
			this.EpisodeIndex       = 1;
			this.MapIndex           = 1;
			this.MapName            = "";
			this.ExtraOptions       = "";
			this.SpecifyWidth       = false;
			this.Width              = 1280;
			this.SpecifyHeight      = false;
			this.Height             = 720;
			this.PixelMode          = PixelMode.Single;
			this.DisableFlags       = DisableOptions.EnableAll;
			this.FastMonsters       = false;
			this.NoMonsters         = false;
			this.RespawningMonsters = false;
			this.SpecifyTimeLimit   = false;
			this.TimeLimit          = 20;
			this.SpecifyTurboMode   = false;
			this.TurboMode          = 100;
			this.SpecifyDifficulty  = false;
			this.Difficulty         = 0;
		}
	}

	/// <summary>
	/// Enumeration of pixel modes.
	/// </summary>
	public enum PixelMode
	{
		/// <summary>
		/// Standard pixels.
		/// </summary>
		Single,

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
	/// Represents objects that contain extra details to be used by the UI when working with <see cref="PixelMode"/> enumeration.
	/// </summary>
	public class PixelModeUi
	{
		/// <summary>
		/// Gets the object of type <see cref="Configs.PixelMode"/> for which this object provides extra information.
		/// </summary>
		public PixelMode PixelMode { get; }

		/// <summary>
		/// Gets the name of the pixel mode.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets the description of the pixel mode.
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <param name="pixelMode">  Pixel mode identifier.</param>
		/// <param name="name">       Name of the pixel mode.</param>
		/// <param name="description">Description of the pixel mode.</param>
		public PixelModeUi(PixelMode pixelMode, string name, string description)
		{
			this.PixelMode   = pixelMode;
			this.Name        = name;
			this.Description = description;
		}

		/// <summary>
		/// An array of objects that describe <see cref="Configs.PixelMode"/> objects.
		/// </summary>
		public static readonly PixelModeUi[] Values =
		{
			new PixelModeUi(PixelMode.Single, nameof(PixelMode.Single), "Use normal pixel mode."),
			new PixelModeUi(PixelMode.Double, nameof(PixelMode.Double), "Force renderer to use pixel doubling."),
			new PixelModeUi(PixelMode.Quad,   nameof(PixelMode.Quad),   "Force renderer to use pixel quadrupling.")
		};
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
		[FlagInfo("CD Audio", "Check this box to disable CD audio.")]
		CompactDiskAudio = 1,

		/// <summary>
		/// When set instructs zDoom to disable function that lowers zDoom process priority when player alt-tabs away.
		/// </summary>
		[FlagInfo(nameof(Idling), "Check this box to prevent zDoom from lowering its priority when minimized.")]
		Idling = 2,

		/// <summary>
		/// When set instructs zDoom to disable joy stick control method.
		/// </summary>
		[FlagInfo(nameof(JoyStick), "Check this box to disable joystick support in case non-USB device is plugged in to stop the game from slowing down by " +
									"polling the joystick input.")]
		JoyStick = 4,

		/// <summary>
		/// When set instructs zDoom to disable music in the game.
		/// </summary>
		[FlagInfo("Music", "Check this box to disable music playback.")]
		Music = 8,

		/// <summary>
		/// When set instructs zDoom to disable sound effects.
		/// </summary>
		[FlagInfo("Sound Effects", "Check this box to disable the sound effects.")]
		SoundEffects = 16,

		/// <summary>
		/// When set instructs zDoom to disable both music and sound effects.
		/// </summary>
		Sound = 24,

		/// <summary>
		/// When set instructs zDoom to disable startup screens for Heretic, Hexen and Strife.
		/// </summary>
		[FlagInfo("Startup Screens", "Check this box to disable start-up screens that are used by Heretic, Hexen and Strife.")]
		StartupScreens = 32,

		/// <summary>
		/// When set instructs zDoom to disable sprite renaming used in user-created files for Heretic, Hexen or Strife.
		/// </summary>
		[FlagInfo("Sprite Renaming", "Check this box to disable sprite renaming that's used by mods for Heretic, Hexen or Strife.")]
		SpriteRenaming = 64,

		/// <summary>
		/// When set instructs zDoom to disable auto-loading files.
		/// </summary>
		[FlagInfo("Auto-Load Files", "Check this box to prevent automatic load of files specified in \"AutoLoad\" section of the config file, as well " +
									 "as \"zvox.wad\" and files from \"skins\" directory.")]
		AutoLoad = 128,

		/// <summary>
		/// When set, instructs zDoom to generate the block map instead of loading it.
		/// </summary>
		[FlagInfo("Block Map Load", "Check this box to have the game generate the block map information instead of loading it.")]
		BlockMapLoad = 256
	}

	/// <summary>
	/// Enumeration of values that indicate how to start the game.
	/// </summary>
	public enum StartupAction
	{
		/// <summary>
		/// Start the game normally.
		/// </summary>
		None,

		/// <summary>
		/// Start the game and immediately load a save game.
		/// </summary>
		LoadGame,

		/// <summary>
		/// Start the game and immediately load a game-play demo.
		/// </summary>
		LoadDemo,

		/// <summary>
		/// Start the game and immediately load a map that was specified by an index.
		/// </summary>
		LoadMapIndex,

		/// <summary>
		/// Start the game and immediately load a map that was specified by a name.
		/// </summary>
		LoadMapName
	}
}