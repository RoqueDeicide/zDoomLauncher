using System;

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
		/// <typeparam name="ComparableType">Type of the value to clamp.</typeparam>
		/// <param name="value">Value to clamp.</param>
		/// <param name="min">  Left border of the range.</param>
		/// <param name="max">  Right border of the range.</param>
		/// <returns>Clamped value.</returns>
		public static ComparableType Clamp<ComparableType>(ComparableType value, ComparableType min, ComparableType max)
			where ComparableType : IComparable<ComparableType>
		{
			return value.CompareTo(min) < 0
					   ? min
					   : value.CompareTo(max) > 0
						   ? max
						   : value;
		}

		/// <summary>
		/// Clamps a value into the range.
		/// </summary>
		/// <typeparam name="ComparableType">Type of the value to clamp.</typeparam>
		/// <param name="value">Value to clamp.</param>
		/// <param name="min">  Left border of the range.</param>
		/// <param name="max">  Right border of the range.</param>
		public static void Clamp<ComparableType>(ref ComparableType value, ComparableType min, ComparableType max)
			where ComparableType : IComparable<ComparableType>
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

		/// <summary>
		/// Returns a value that indicates whether the specified value is not a number (System.Double.NaN).
		/// </summary>
		/// <param name="number">A double-precision floating-point number.</param>
		/// <returns><c>true</c> if d evaluates to <see cref="System.Double.NaN"/>; otherwise, <c>false</c>.</returns>
		public static bool IsNaN(this double number)
		{
			return double.IsNaN(number);
		}
	}
}