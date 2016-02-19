using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using Launcher.Logging;

namespace Launcher
{
	/// <summary>
	/// Represents API that looks for extra loadable files.
	/// </summary>
	public static class ExtraFilesLookUp
	{
		#region Fields
		private static readonly string[] emptyArray = {};

		#endregion
		#region Properties
		
		#endregion
		#region Events
		
		#endregion
		#region Construction
		
		#endregion
		#region Interface
		/// <summary>
		/// Looks for the files that can be loaded with the game in the specified folder.
		/// </summary>
		/// <param name="folder">Path to the folder to for files in.</param>
		/// <returns>An object that can be enumerated through for loadable files.</returns>
		public static IEnumerable<string> GetLoadableFiles(string folder)
		{
			Log.Message("Looking for loadable files in {0}", folder);

			if (File.Exists(folder))
			{
				Log.Message("Provided name is a name of the file.");
				return emptyArray;
			}
			if (!Directory.Exists(folder))
			{
				Log.Message("Cannot find the folder.");
				return emptyArray;
			}

			IEnumerable<string> enumeration;
			try
			{
				enumeration = Directory.EnumerateFiles(folder, "*.*", SearchOption.TopDirectoryOnly);
			}
			catch (Exception ex)
			{
				Log.Message("Unable to enumerate the folder due to an error: {0}", ex.Message);
				return emptyArray;
			}


			return
				from file in enumeration
				select Path.GetFileName(file) into fileName
				where fileName != null
				let extension = Path.GetExtension(fileName).ToLowerInvariant()
				where extension == ".wad" || extension == ".pk3"
				where !Iwads.SupportedIwads.Keys.Contains(fileName.ToLowerInvariant())
				select fileName;
		}
		#endregion
		#region Utilities
		
		#endregion
	}
}