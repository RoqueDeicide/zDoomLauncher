using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Launcher
{
	/// <summary>
	/// Defines extension methods for collection types.
	/// </summary>
	public static class CollectionExtensions
	{
		/// <summary>
		/// Performs binary search for an item in a list that is sorted in ascending order.
		/// </summary>
		/// <typeparam name="ItemType">Type of objects contained in the list.</typeparam>
		/// <param name="list">List to perform the binary search on.</param>
		/// <param name="item">Item to look for.</param>
		/// <returns>
		/// If <paramref name="item"/> has been found, a zero-based index of the occurrence is returned, otherwise, a
		/// bitwise complement of the zero-based index where the item should be inserted to maintain the sorted
		/// sequence.
		/// </returns>
		public static int BinarySearch<ItemType>(this IList<ItemType> list, ItemType item)
			where ItemType : IComparable<ItemType>
		{
			int leftIndex  = 0;
			int rightIndex = list.Count;

			while (leftIndex != rightIndex)
			{
				int middleIndex = (rightIndex - leftIndex) / 2 + leftIndex;

				int comparison = item.CompareTo(list[middleIndex]);
				// Normalize the comparison result in case of comparator's shenanigans.
				comparison /= Math.Abs(comparison != 0 ? comparison : 1);
				switch (comparison)
				{
					case 0:
						return middleIndex;

					case -1:
						rightIndex = middleIndex;
						break;

					case 1:
						leftIndex = middleIndex + 1;
						break;
				}
			}

			return ~leftIndex;
		}

		/// <summary>
		/// Performs binary search for an item in a list that is sorted in ascending order.
		/// </summary>
		/// <typeparam name="ItemType">Type of objects contained in the list.</typeparam>
		/// <param name="list">              List to perform the binary search on.</param>
		/// <param name="item">              Item to look for.</param>
		/// <param name="comparisonFunction">A function to use for comparison of items.</param>
		/// <returns>
		/// If <paramref name="item"/> has been found, a zero-based index of the occurrence is returned, otherwise, a
		/// bitwise complement of the zero-based index where the item should be inserted to maintain the sorted
		/// sequence.
		/// </returns>
		public static int BinarySearch<ItemType>(this IList<ItemType>          list, ItemType item,
												 Func<ItemType, ItemType, int> comparisonFunction)
		{
			int leftIndex  = 0;
			int rightIndex = list.Count;

			while (leftIndex != rightIndex)
			{
				int middleIndex = (rightIndex - leftIndex) / 2 + leftIndex;

				int comparison = comparisonFunction(item, list[middleIndex]);
				// Normalize the comparison result in case of comparator's shenanigans.
				comparison /= Math.Abs(comparison != 0 ? comparison : 1);
				switch (comparison)
				{
					case 0:
						return middleIndex;

					case -1:
						rightIndex = middleIndex;
						break;

					case 1:
						leftIndex = middleIndex + 1;
						break;
				}
			}

			return ~leftIndex;
		}

		/// <summary>
		/// Adds an <paramref name="item"/> to the <paramref name="sortedList"/> in a way that maintains a sorted
		/// sequence.
		/// </summary>
		/// <typeparam name="T">Type of items in the list.</typeparam>
		/// <param name="sortedList">A sorted list.</param>
		/// <param name="item">      Item to add.</param>
		public static void AddToSorted<T>(this IList<T> sortedList, T item)
			where T : IComparable<T>
		{
			int index = sortedList.BinarySearch(item);
			if (index < 0)
			{
				sortedList.Insert(~index, item);
			}
		}

		/// <summary>
		/// Adds an <paramref name="item"/> to the <paramref name="sortedList"/> in a way that maintains a sorted
		/// sequence.
		/// </summary>
		/// <typeparam name="T">Type of items in the list.</typeparam>
		/// <param name="sortedList">        A sorted list.</param>
		/// <param name="item">              Item to add.</param>
		/// <param name="comparisonFunction">A function to use as a comparator.</param>
		public static void AddToSorted<T>(this IList<T> sortedList, T item, Func<T, T, int> comparisonFunction)
		{
			int index = sortedList.BinarySearch(item, comparisonFunction);
			if (index < 0)
			{
				sortedList.Insert(~index, item);
			}
		}

		/// <summary>
		/// Removes an <paramref name="item"/> from the <paramref name="sortedList"/> by employing binary search
		/// algorithm.
		/// </summary>
		/// <typeparam name="T">Type of items in the list.</typeparam>
		/// <param name="sortedList">A sorted list.</param>
		/// <param name="item">      An item to remove.</param>
		public static void RemoveFromSorted<T>(this IList<T> sortedList, T item)
			where T : IComparable<T>
		{
			int index = sortedList.BinarySearch(item);

			if (index < 0)
			{
				return;
			}

			sortedList.RemoveAt(index);
		}

		/// <summary>
		/// Removes an <paramref name="item"/> from the <paramref name="sortedList"/> by employing binary search
		/// algorithm.
		/// </summary>
		/// <typeparam name="T">Type of items in the list.</typeparam>
		/// <param name="sortedList">        A sorted list.</param>
		/// <param name="item">              An item to remove.</param>
		/// <param name="comparisonFunction">A function to use as a comparator.</param>
		public static void RemoveFromSorted<T>(this IList<T> sortedList, T item, Func<T, T, int> comparisonFunction)
		{
			int index = sortedList.BinarySearch(item, comparisonFunction);

			if (index < 0)
			{
				return;
			}

			sortedList.RemoveAt(index);
		}

		/// <summary>
		/// Adds a range of items to an observable collection.
		/// </summary>
		/// <typeparam name="T">Type of items in the collection.</typeparam>
		/// <param name="collection">Collection to add items to.</param>
		/// <param name="items">     A range of items to add.</param>
		public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
		{
			if (collection is null)
			{
				throw new ArgumentNullException(nameof(collection), @"Cannot add items to a null collection.");
			}

			if (items is null)
			{
				return;
			}

			foreach (T item in items)
			{
				collection.Add(item);
			}
		}
	}
}