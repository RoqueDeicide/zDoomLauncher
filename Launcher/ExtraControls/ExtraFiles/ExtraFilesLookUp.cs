using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
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

		/// <summary>
		/// An observable collection of absolute paths to folder where to look for the loadable files.
		/// </summary>
		public static readonly ObservableCollection<string> Directories = new ObservableCollection<string>();
		/// <summary>
		/// An observable collection of absolute paths to all loadable files that were found in <see cref="Directories"/>.
		/// </summary>
		public static readonly ObservableCollection<FileDesc> LoadableFiles =
			new ObservableCollection<FileDesc>();
		#endregion
		#region Properties
		/// <summary>
		/// Gets the path to the directory that is mentioned in DOOMWADDIR environment variable.
		/// </summary>
		public static string DoomWadDirectory
		{
			get
			{
				try
				{
					return Environment.GetEnvironmentVariable("DOOMWADDIR") ?? "";
				}
				catch (SecurityException)
				{
					return null;
				}
			}
		}
		#endregion
		#region Events
		
		#endregion
		#region Construction
		static ExtraFilesLookUp()
		{
			Directories.CollectionChanged += RefreshDirectories;
		}
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

			return enumeration.Where(IsLoadableFile);
		}
		private static bool IsLoadableFile(string filePath)
		{
			string fileName = Path.GetFileName(filePath);
			if (fileName == null)
			{
				return false;
			}
			fileName = fileName.ToLowerInvariant();
			string extension = Path.GetExtension(fileName).ToLowerInvariant();

			return extension == ".wad" || extension == ".pk3" &&
				   !Iwads.SupportedIwads.Keys.Contains(fileName);
		}
		/// <summary>
		/// Refreshes all directories and loadable file lists.
		/// </summary>
		public static void Refresh()
		{
			// Refresh directories.
			for (int i = 0; i < Directories.Count; i++)
			{
				if (!Directory.Exists(Directories[i]))
				{
					Directories.RemoveAt(i--);
				}
			}

			// Re-add files from directories.
			RefreshFiles();
		}
		private static void RefreshFiles()
		{
			// Remove files that don't exist.
			for (int i = 0; i < LoadableFiles.Count; i++)
			{
				if (!File.Exists(LoadableFiles[i].FullPath))
				{
					LoadableFiles.RemoveAt(i--);
				}
			}

			foreach (string directory in Directories)
			{
				RefreshFilesInDirectory(directory);
			}
		}
		private static void RefreshFilesInDirectory(string directory)
		{
			//int currentEnumeratedFileIndex = 0;
			int currentCollectionFileIndex = 0;

			var enumeratedFiles = GetLoadableFiles(directory);

			foreach (string enumeratedFile in enumeratedFiles)
			{
				if (enumeratedFile != LoadableFiles[currentCollectionFileIndex].FullPath)
				{
					// Current enumerated file is a new file that wasn't in the directory before.
					LoadableFiles.Insert(currentCollectionFileIndex, new FileDesc(enumeratedFile));
				}
				// Advance to next file in the collection.
				currentCollectionFileIndex++;

				//// We are advancing to the next enumerated file.
				//currentEnumeratedFileIndex++;
			}
		}
		#endregion
		#region Utilities
		private static void RefreshDirectories(object sender, NotifyCollectionChangedEventArgs args)
		{
			var newItems = (args.NewItems ?? new string[0]).OfType<string>();
			var oldItems = (args.OldItems ?? new string[0]).OfType<string>();

			switch (args.Action)
			{
				case NotifyCollectionChangedAction.Add:
					// Add loadable files to the files collection.
					AddFilesFromDirectories(newItems);
					break;
				case NotifyCollectionChangedAction.Remove:
					// Remove loadable files that are in the directory from the collection.
					RemoveFilesInDirectories(oldItems);
					break;
				case NotifyCollectionChangedAction.Replace:
					RemoveFilesInDirectories(oldItems);
					AddFilesFromDirectories(newItems);
					break;
				case NotifyCollectionChangedAction.Move:
					// Don't do anything.
					break;
				case NotifyCollectionChangedAction.Reset:
					LoadableFiles.Clear();
					AddFilesFromDirectories(newItems);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		private static void AddFilesFromDirectories(IEnumerable<string> directories)
		{
			foreach (string loadableFile in from directory in directories
											from file in GetLoadableFiles(directory)
											select file)
			{
				LoadableFiles.Add(new FileDesc(loadableFile));
			}
		}
		private static void RemoveFilesInDirectories(IEnumerable<string> directories)
		{
			foreach (string directory in directories)
			{
				for (int i = 0; i < LoadableFiles.Count; i++)
				{
					if (LoadableFiles[i].Directory == directory)
					{
						LoadableFiles.RemoveAt(i--);
					}
				}
			}
		}
		#endregion
	}
}