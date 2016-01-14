using System;
using System.Linq;

namespace Launcher
{
	/// <summary>
	/// Defines extra math-related functions.
	/// </summary>
	public static class MathExtra
	{
		/// <summary>
		/// Clamps a value into the range.
		/// </summary>
		/// <typeparam name="T">Type of the value to clamp.</typeparam>
		/// <param name="value">Value to clamp.</param>
		/// <param name="min">  Left border of the range.</param>
		/// <param name="max">  Right border of the range.</param>
		/// <returns>Clamped value.</returns>
		public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
		{
			return value.CompareTo(min) < 0
				? min
				: (value.CompareTo(max) > 0
					? max
					: value);
		}
		/// <summary>
		/// Clamps a value into the range.
		/// </summary>
		/// <typeparam name="T">Type of the value to clamp.</typeparam>
		/// <param name="value">Value to clamp.</param>
		/// <param name="min">  Left border of the range.</param>
		/// <param name="max">  Right border of the range.</param>
		public static void Clamp<T>(ref T value, T min, T max) where T : IComparable<T>
		{
			if (value.CompareTo(min) < 0)
			{
				value = min;
			}
			else if (value.CompareTo(max) > 0)
			{
				value = max;
			}
		}
	}
}