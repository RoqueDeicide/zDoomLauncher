using System;
using System.IO;

namespace Launcher
{
	/// <summary>
	/// Defines some extra functions for working with paths.
	/// </summary>
	public static class PathUtils
	{
		/// <summary>
		/// Gets the path to the file that doesn't contain any redirection markers.
		/// </summary>
		/// <param name="path">Path to get the shortest absolute version of.</param>
		/// <returns>The path that doesn't contain redirection markers (like ..\).</returns>
		public static string GetLocalPath(string path)
		{
			var pathUri = new Uri(path, UriKind.RelativeOrAbsolute);

			var localPath = pathUri.LocalPath;

			return localPath;
		}

		/// <summary>
		/// Converts an absolute path to the file or a folder to the path that is relative to the specified folder.
		/// </summary>
		/// <param name="fullPath">  Path to make into relative one.</param>
		/// <param name="folderPath">Path to the folder to relative to which the result will be.</param>
		/// <returns>A path that is leads to the file or folder relative to the specified folder.</returns>
		public static string ToRelativePath(string fullPath, string folderPath)
		{
			return ToRelativePath(new Uri(fullPath,                             UriKind.RelativeOrAbsolute),
								  new Uri(EndWithBackSlashInternal(folderPath), UriKind.Absolute));
		}

		/// <summary>
		/// Converts an absolute path to the file or a folder to the path that is relative to the specified folder.
		/// </summary>
		/// <param name="fullPath"> Path to make into relative one.</param>
		/// <param name="folderUri">Path to the folder to relative to which the result will be.</param>
		/// <returns>A path that is leads to the file or folder relative to the specified folder.</returns>
		public static string ToRelativePath(string fullPath, Uri folderUri)
		{
			return ToRelativePath(new Uri(fullPath, UriKind.RelativeOrAbsolute), folderUri);
		}

		/// <summary>
		/// Converts an absolute path to the file or a folder to the path that is relative to the specified folder.
		/// </summary>
		/// <param name="fullUri">  Path to make into relative one.</param>
		/// <param name="folderUri">Path to the folder to relative to which the result will be.</param>
		/// <returns>A path that is leads to the file or folder relative to the specified folder.</returns>
		public static string ToRelativePath(Uri fullUri, Uri folderUri)
		{
			// Create the relative URI.
			var relativeUri = folderUri.MakeRelativeUri(fullUri);

			// Convert URI to its text representation.
			var relativePath = relativeUri.ToString();

			// Unescape the URI string (e.g. convert %20 to spaces).
			relativePath = Uri.UnescapeDataString(relativePath);

			// Create a Windows path by replacing forward slashes with back slashes.
			return relativePath.Replace('/', '\\');
		}

		/// <summary>
		/// Makes sure that path to the directory ends with a backslash.
		/// </summary>
		/// <param name="folderPath">Path to the folder.</param>
		/// <returns>
		/// If <paramref name="folderPath"/> ends with a backslash, then it gets returned as is, otherwise a new string that
		/// represents <paramref name="folderPath"/> with an appended backslash is returned.
		/// </returns>
		/// <exception cref="ArgumentException">Cannot make a path to the file end with a backslash.</exception>
		public static string EndWithBackSlash(string folderPath)
		{
			if (string.IsNullOrEmpty(Path.GetFileName(folderPath)))
			{
				throw new ArgumentException("Cannot make a path to the file end with a backslash.");
			}

			return EndWithBackSlashInternal(folderPath);
		}

		private static string EndWithBackSlashInternal(string folderPath)
		{
			return folderPath.EndsWith("\\")
					   ? folderPath
					   : $"{folderPath}\\";
		}
	}
}