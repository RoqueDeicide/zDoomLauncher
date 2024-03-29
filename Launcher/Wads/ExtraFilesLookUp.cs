﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security;
using Launcher.Annotations;
using Launcher.Utilities;

namespace Launcher
{
	/// <summary>
	/// Represents API that looks for extra loadable files.
	/// </summary>
	public static class ExtraFilesLookUp
	{
		#region Fields

		private static readonly string[] EmptyArray = { };

		private static readonly string[] WadBlacklist =
		{
			@"zdoom.pk3",
			@"gzdoom.pk3",
			@"nerve.wad"
		};

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
		[CanBeNull]
		public static string DoomWadDirectory
		{
			get
			{
				try
				{
					return Environment.GetEnvironmentVariable("DOOMWADDIR");
				}
				catch (SecurityException)
				{
					return null;
				}
			}
		}

		#endregion

		#region Construction

		static ExtraFilesLookUp()
		{
			Directories.CollectionChanged += DirectoriesCollectionChanged;

			AppSettings.StaticPropertyChanged +=
				(sender, args) =>
				{
					if (args.PropertyName == nameof(AppSettings.ZDoomDirectory))
					{
						AddDirectory(AppSettings.ZDoomDirectory);
					}
				};
		}

		#endregion

		#region Interface

		/// <summary>
		/// Adds a directory to <see cref="Directories"/> collection.
		/// </summary>
		/// <remarks>Insertion is done in manner that forms a sorted sequence.</remarks>
		/// <param name="directory">Full path to the directory.</param>
		public static void AddDirectory(string directory)
		{
#if DEBUG
			if (!Directory.Exists(directory))
			{
				throw new DirectoryNotFoundException("Trying to add a directory that doesn't exist to the list.");
			}
#endif
			int index = Directories.BinarySearch(directory);

			if (index < 0)
			{
				Directories.Insert(~index, directory);
			}
		}

		/// <summary>
		/// Removes a directory from <see cref="Directories"/> collection.
		/// </summary>
		/// <param name="directory">Full path to the directory to remove.</param>
		public static void RemoveDirectory(string directory)
		{
			int index = Directories.BinarySearch(directory);

			if (index < 0)
			{
				return;
			}

			Directories.RemoveAt(index);
		}

		/// <summary>
		/// Looks for the files that can be loaded with the game in the specified folder.
		/// </summary>
		/// <param name="folder">Path to the folder to look for files in.</param>
		/// <returns>An object that can be enumerated through for loadable files.</returns>
		public static IEnumerable<string> GetLoadableFiles(string folder)
		{
			Log.Message("Looking for loadable files in {0}", folder);

			if (File.Exists(folder))
			{
				Log.Message("Provided name is a name of the file.");
				return EmptyArray;
			}

			if (!Directory.Exists(folder))
			{
				Log.Message("Cannot find the folder.");
				return EmptyArray;
			}

			IEnumerable<string> enumeration;
			try
			{
				enumeration = Directory.EnumerateFiles(folder, "*.*", SearchOption.TopDirectoryOnly);
			}
			catch (Exception ex)
			{
				Log.Message("Unable to enumerate the folder due to an error: {0}", ex.Message);
				return EmptyArray;
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

			bool Predicate(IwadFile iwad) => iwad.FileName.Equals(fileName);

#if DEBUG
			// For debugger.
			bool isIwad = Iwads.SupportedIwads.Any(Predicate);

			bool isLoadable = extension == ".wad" || extension == ".pk3";

			bool isBlacklisted = WadBlacklist.Contains(fileName);

			return !isIwad && isLoadable && !isBlacklisted;
#else
			return (extension == ".wad" || extension == ".pk3") &&
				   !WadBlacklist.Contains(fileName)             &&
				   !Iwads.SupportedIwads.Any(Predicate);
#endif
		}

		/// <summary>
		/// Refreshes all directories and loadable file lists.
		/// </summary>
		public static void Refresh()
		{
			// Remove directories that don't exist anymore.
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
			var enumeratedFiles = GetLoadableFiles(directory);

			foreach (string enumeratedFile in enumeratedFiles)
			{
				AddLoadableFile(enumeratedFile);
			}
		}

		#endregion

		#region Utilities

		private static void AddLoadableFile(string file)
		{
			LoadableFiles.AddToSorted(new FileDesc(file));
		}

		private static void DirectoriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			var newItems = (args.NewItems ?? Array.Empty<string>()).OfType<string>();
			var oldItems = (args.OldItems ?? Array.Empty<string>()).OfType<string>();

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
				AddLoadableFile(loadableFile);
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