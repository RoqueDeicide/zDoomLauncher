using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Launcher.Annotations;

namespace Launcher
{
	/// <summary>
	/// Defines arrays of supported IWAD files.
	/// </summary>
	public static class Iwads
	{
		#region Fields

		private static          string            iwadFolder;
		private static readonly FileSystemWatcher Watcher;

		/// <summary>
		/// A list of supported IWAD files.
		/// </summary>
		public static readonly List<IwadFile> SupportedIwads;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the path to the folder where to look for IWAD files.
		/// </summary>
		public static string IwadFolder
		{
			get => iwadFolder;
			set
			{
				if (iwadFolder != value)
				{
					iwadFolder = value;
					UpdateAvailableIwads();
					OnStaticPropertyChanged();
				}
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when one of the static properties changes its value.
		/// </summary>
		public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

		/// <summary>
		/// Occurs before this class updates availability status of all IWAD files.
		/// </summary>
		public static event EventHandler Updating;

		/// <summary>
		/// Occurs after this class updates availability status of all IWAD files.
		/// </summary>
		public static event EventHandler Updated;

		#endregion

		#region Construction

		static Iwads()
		{
			SupportedIwads = new List<IwadFile>
							 {
								 new IwadFile("",                null,                                     false),
								 new IwadFile(@"doom1.wad",      @"Doom Shareware version",                true),
								 new IwadFile(@"doom.wad",       @"Doom Full version",                     true),
								 new IwadFile(@"doomu.wad",      @"Ultimate Doom",                         false),
								 new IwadFile(@"bfgdoom.wad",    @"Doom BFG Edition",                      true),
								 new IwadFile(@"freedoom1.wad",  @"Freedoom: Phase 1",                     false),
								 new IwadFile(@"doom2.wad",      @"Doom 2",                                false),
								 new IwadFile(@"bfgdoom2.wad",   @"Doom 2 BFG Edition",                    true),
								 new IwadFile(@"tnt.wad",        @"Final Doom — TNT: Evilution",           false),
								 new IwadFile(@"plutonia.wad",   @"Final Doom — The Plutonia Experiment",  false),
								 new IwadFile(@"freedm.wad",     @"FreeDM",                                false),
								 new IwadFile(@"freedoom2.wad",  @"Freedoom: Phase 2",                     false),
								 new IwadFile(@"doom2f.wad",     @"French Doom II",                        false),
								 new IwadFile(@"heretic1.wad",   @"Heretic (Shareware)",                   true),
								 new IwadFile(@"heretic.wad",    @"Heretic (Registered or Commercial)",    true),
								 new IwadFile(@"blasphemer.wad", @"Blasphemer",                            false),
								 new IwadFile(@"hexendemo.wad",  @"Hexen (Demo)",                          false),
								 new IwadFile(@"hexen.wad",      @"Hexen (Full)",                          false),
								 new IwadFile(@"hexdd.wad",      @"Hexen: Deathkings of the Dark Citadel", false),
								 new IwadFile(@"strife0.wad",    @"Strife (Teaser)",                       false),
								 new IwadFile(@"strife.wad",     @"Strife (Full)",                         false),
								 new IwadFile(@"chex.wad",       @"Chex Quest",                            true),
								 new IwadFile(@"chex3.wad",      @"Chex Quest 3",                          false),
								 new IwadFile(@"action2.wad",    @"Action Doom 2: Urban Brawl",            false),
								 new IwadFile(@"harm1.wad",      @"Harmony v1.1",                          false),
								 new IwadFile(@"hacx.wad",       @"Hacx v1.2",                             false),
								 new IwadFile(@"hacx2.wad",      @"Hacx v2.0",                             false)
							 };
			SupportedIwads.Sort();

			Watcher = new FileSystemWatcher
					  {
						  NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
						  Filter       = "*.wad"
					  };
			Watcher.Renamed += WatcherOnRenamed;
			Watcher.Created += WatcherOnCreated;
			Watcher.Deleted += WatcherOnDeleted;
		}

		#endregion

		#region Utilities

		[NotifyPropertyChangedInvocator]
		private static void OnStaticPropertyChanged(string propertyName = null)
		{
			var handler = StaticPropertyChanged;
			handler?.Invoke(null, new PropertyChangedEventArgs(propertyName));
		}

		private static void UpdateAvailableIwads(bool folderChanged = true)
		{
			OnUpdating();

			if (folderChanged)
			{
				Watcher.Path                = iwadFolder;
				Watcher.EnableRaisingEvents = true;
			}

			string doomWadDir = ExtraFilesLookUp.DoomWadDirectory;

			foreach (IwadFile iwad in SupportedIwads)
			{
				iwad.Available = iwad.FileName == "" || File.Exists(Path.Combine(iwadFolder, iwad.FileName)) ||
								 doomWadDir != null && File.Exists(Path.Combine(doomWadDir, iwad.FileName));
			}

			OnUpdated();
		}

		private static void WatcherOnDeleted(object sender, FileSystemEventArgs fileSystemEventArgs)
		{
			UpdateAvailableIwads(false);
		}

		private static void WatcherOnCreated(object sender, FileSystemEventArgs fileSystemEventArgs)
		{
			UpdateAvailableIwads(false);
		}

		private static void WatcherOnRenamed(object sender, RenamedEventArgs renamedEventArgs)
		{
			UpdateAvailableIwads(false);
		}

		private static void OnUpdating()
		{
			EventHandler handler = Updating;
			handler?.Invoke(null, EventArgs.Empty);
		}

		private static void OnUpdated()
		{
			EventHandler handler = Updated;
			handler?.Invoke(null, EventArgs.Empty);
		}

		#endregion
	}
}