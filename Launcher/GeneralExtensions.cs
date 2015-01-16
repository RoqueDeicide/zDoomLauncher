using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Launcher
{
	/// <summary>
	/// Defines some miscellaneous extension methods.
	/// </summary>
	public static class GeneralExtensions
	{
		/// <summary>
		/// Indicates whether this collection is null or is empty.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="collection">Collection itself.</param>
		/// <returns>
		/// True, if <paramref name="collection"/> is null or number its elements is equal
		/// to zero.
		/// </returns>
		public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
		{
			return collection == null || collection.Count == 0;
		}
		/// <summary>
		/// Indicates whether this collection is null or is too small.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="collection">  Collection itself.</param>
		/// <param name="minimalCount">
		/// Minimal number of elements that must be inside the collection.
		/// </param>
		/// <returns>
		/// True, if <paramref name="collection"/> is null, or it contains smaller number
		/// elements then one defined by <paramref name="minimalCount"/> .
		/// </returns>
		public static bool IsNullOrTooSmall<T>(this ICollection<T> collection, int minimalCount)
		{
			return collection == null || collection.Count < minimalCount;
		}
		/// <summary>
		/// Creates a string that is a list of text representation of all elements of the
		/// collection separated by a comma.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="collection">Collection.</param>
		/// <returns>Text representation of the collection.</returns>
		public static string ContentsToString<T>(this IEnumerable<T> collection)
		{
			return ContentsToString(collection, ",");
		}
		/// <summary>
		/// Creates a string that is a list of text representation of all elements of the
		/// collection separated by a comma.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="collection">Collection.</param>
		/// <param name="separator"> Text to insert between elements.</param>
		/// <returns>Text representation of the collection.</returns>
		public static string ContentsToString<T>(this IEnumerable<T> collection, string separator)
		{
			StringBuilder builder = new StringBuilder();
			IEnumerator<T> enumerator = collection.GetEnumerator();
			enumerator.Reset();
			enumerator.MoveNext();
			builder.Append(enumerator.Current);
			while (enumerator.MoveNext())
			{
				builder.Append(separator);
				builder.Append(enumerator.Current);
			}
			return builder.ToString();
		}
		/// <summary>
		/// Creates a string that is a list of text representation of all elements of the
		/// collection separated by a comma.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="collection">Collection.</param>
		/// <param name="separator"> Character to insert between elements.</param>
		/// <returns>Text representation of the collection.</returns>
		public static string ContentsToString<T>(this IEnumerable<T> collection, char separator)
		{
			return ContentsToString(collection, new string(separator, 1));
		}
		/// <summary>
		/// Finds zero-based indexes of all occurrences of given substring in the text.
		/// </summary>
		/// <param name="text">     Text to look for substrings in.</param>
		/// <param name="substring">Piece of text to look for.</param>
		/// <param name="options">  Text comparison options.</param>
		/// <returns>A list of all indexes.</returns>
		public static List<int> AllIndexesOf(this string text, string substring, StringComparison options)
		{
			if (String.IsNullOrEmpty(text))
			{
				throw new ArgumentException("Cannot perform search in the empty string.");
			}
			if (String.IsNullOrEmpty(substring))
			{
				throw new ArgumentException("Cannot perform search for an empty string.");
			}
			List<int> indexes = new List<int>(text.Length / substring.Length);
			for (int i = text.IndexOf(substring, options); i != -1; )
			{
				indexes.Add(i);
				i = text.IndexOf(substring, i + substring.Length, options);
			}
			return indexes;
		}
		/// <summary>
		/// Finds zero-based indexes of all occurrences of given substring in the text
		/// using the invariant culture.
		/// </summary>
		/// <param name="text">     Text to look for substrings in.</param>
		/// <param name="substring">Piece of text to look for.</param>
		/// <returns>A list of all indexes.</returns>
		public static List<int> AllIndexesOf(this string text, string substring)
		{
			return AllIndexesOf(text, substring, StringComparison.InvariantCulture);
		}
		/// <summary>
		/// Changes a number with a specified index in the text.
		/// </summary>
		/// <param name="text">  Text.</param>
		/// <param name="index"> 
		/// Zero-based index of the number that we need in a sequence of numbers in the
		/// text.
		/// </param>
		/// <param name="number">New value to assign.</param>
		/// <returns>Text with a new number.</returns>
		public static string ChangeNumber(this string text, int index, int number)
		{
			// Find the number with specified index.
			int currentCharacterIndex = 0;		// Index of the character we are currently processing.
			bool goingThroughTheNumber = false;	// Was the previous character a digit?
			int currentNumberIndex = -1;		// Index of the number in a string.
			int currentNumberStart = -1;		// Index of the first digit of current/last number.
			bool foundOurNumber = false;		// Did we find the number we needed?
			while (currentCharacterIndex < text.Length)
			{
				// Check the current character. If its a digit, then we are going through
				// or into a number, otherwise we either finished going through the number
				// or we are going through the plain text.
				if (Char.IsDigit(text[currentCharacterIndex]))
				{
					if (!goingThroughTheNumber)
					{
						// Found another number.

						// Save its beginning.
						currentNumberStart = currentCharacterIndex;
						if (++currentNumberIndex == index)
						{
							// Found our number. So lets find its end.
							foundOurNumber = true;
						}
					}
					goingThroughTheNumber = true;
				}
				else
				{
					if (goingThroughTheNumber)
					{
						// End the number.
						goingThroughTheNumber = false;
						if (foundOurNumber)
						{
							// End the search as we have found the end of the number we
							// needed.
							break;
						}
					}
				}
				// Advance to the next character.
				currentCharacterIndex++;
			}
			if (!foundOurNumber)
			{
				throw new Exception("Unable to find the number with the specified index.");
			}
			// Extract the number and split the string in two.
			StringBuilder builder = new StringBuilder(text.Length + 20);
			// Add the first half to the resultant string.
			builder.Append(text.Substring(0, currentNumberStart));
			// Add the string representation of the new number to the string.
			builder.Append(number);
			// Add the last half.
			builder.Append(text.Substring(currentCharacterIndex));

			return builder.ToString();
		}
		/// <summary>
		/// Gets file that contains the assembly.
		/// </summary>
		/// <param name="assembly">Assembly.</param>
		/// <returns>Full path to the .dll file.</returns>
		public static string GetLocation(this Assembly assembly)
		{
			return Uri.UnescapeDataString(new UriBuilder(assembly.CodeBase).Path);
		}
		/// <summary>
		/// Performs an action for each element in the collection.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <param name="collection">Collection to act upon.</param>
		/// <param name="action">    
		/// Action to perform for each element in the collection.
		/// </param>
		public static void Foreach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (T item in collection)
			{
				action(item);
			}
		}
		/// <summary>
		/// Performs an action for each element in the collection.
		/// </summary>
		/// <param name="collection">Collection to act upon.</param>
		/// <param name="action">    
		/// Action to perform for each element in the collection.
		/// </param>
		public static void Foreach(this IEnumerable collection, Action<object> action)
		{
			foreach (object item in collection)
			{
				action(item);
			}
		}
		/// <summary>
		/// Performs an action for each element in the collection.
		/// </summary>
		/// <typeparam name="T">Type of elements in the collection.</typeparam>
		/// <typeparam name="TResult">Ignored.</typeparam>
		/// <param name="collection">Collection to act upon.</param>
		/// <param name="action">    
		/// Action to perform for each element in the collection.
		/// </param>
		public static void Foreach<T, TResult>(this IEnumerable<T> collection, Func<T, TResult> action)
		{
			foreach (T item in collection)
			{
				action(item);
			}
		}
		/// <summary>
		/// Performs an action for each element in the collection.
		/// </summary>
		/// <typeparam name="TResult">Ignored.</typeparam>
		/// <param name="collection">Collection to act upon.</param>
		/// <param name="action">    
		/// Action to perform for each element in the collection.
		/// </param>
		public static void Foreach<TResult>(this IEnumerable collection, Func<object, TResult> action)
		{
			foreach (object item in collection)
			{
				action(item);
			}
		}
	}
	/// <summary>
	/// Contains old value of the property that has been changed.
	/// </summary>
	public class ValueChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Old value.
		/// </summary>
		public object OldValue { get; set; }
	}
}