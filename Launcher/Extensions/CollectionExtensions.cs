using System;
using System.Collections.Generic;

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
	}
}