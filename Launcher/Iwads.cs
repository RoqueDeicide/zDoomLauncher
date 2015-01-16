using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Launcher.Logging;

namespace Launcher
{
	/// <summary>
	/// Defines arrays of supported IWAD files.
	/// </summary>
	public static class Iwads
	{
		/// <summary>
		/// A list of supported IWAD files.
		/// </summary>
		public static readonly SortedList<string, string> SupportedIwads =
			new SortedList<string, string>
		{
			{"doom1.wad", "Doom Shareware version"},
			{"doom.wad", "Doom Full version"},
			{"doomu.wad", "Ultimate Doom"},
			{"bfgdoom.wad", "Doom BFG Edition"},
			{"freedoom1.wad", "Freedoom: Phase 1"},
			{"doom2.wad", "Doom 2"},
			{"bfgdoom2.wad", "Doom 2 BFG Edition"},
			{"tnt.wad", "Final Doom — TNT: Evilution"},
			{"plutonia.wad", "Final Doom — The Plutonia Experiment"},
			{"freedm.wad", "FreeDM"},
			{"freedoom2.wad", "Freedoom: Phase 2"},
			{"doom2f.wad", "French Doom II"},
			{"heretic1.wad", "Heretic (Shareware)"},
			{"heretic.wad", "Heretic (Registered or Commercial)"},
			{"blasphemer.wad", "Blasphemer"},
			{"hexendemo.wad", "Hexen (Demo)"},
			{"hexen.wad", "Hexen (Full)"},
			{"hexdd.wad", "Hexen: Deathkings of the Dark Citadel"},
			{"strife0.wad", "Strife (Teaser)"},
			{"strife.wad", "Strife (Full)"},
			{"chex.wad", "Chex Quest"},
			{"chex3.wad", "Chex Quest 3"},
			{"action2.wad", "Action Doom 2: Urban Brawl"},
			{"harm1.wad", "Harmony v1.1"},
			{"hacx.wad", "Hacx v1.2"},
			{"hacx2.wad", "Hacx v2.0"}
		};
		/// <summary>
		/// A list of episodic games.
		/// </summary>
		public static readonly string[] EpisodicIwads =
		{
			"doom1.wad",
			"doom.wad",
			"bfgdoom.wad",
			"heretic1.wad",
			"heretic.wad",
			"chex.wad"
		};
		/// <summary>
		/// Finds IWAD files this launcher can load, that are also present in the base directory.
		/// </summary>
		/// <param name="folder">A folder where to search for IWAD files.</param>
		/// <returns>A list of IWAD file names.</returns>
		public static IEnumerable<string> FindSupportedIwads(string folder)
		{
			// Scan for IWADs in the given folder.
			return Iwads.SupportedIwads.Keys.Where(x =>
			{
				if (File.Exists(Path.Combine(folder, x)))
				{
					Log.Message("Found {0}.", x.ToLowerInvariant());
					return true;
				}
				return false;
			});
		}
	}
}
