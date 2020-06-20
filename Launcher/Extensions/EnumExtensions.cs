using System;

namespace Launcher
{
	/// <summary>
	/// Defines extension methods for enumeration types.
	/// </summary>
	public static class EnumExtensions
	{
		[ThreadStatic] private static Enum getNameValueCache;

		/// <summary>
		/// Gets the name of the constant in the enumeration.
		/// </summary>
		/// <remarks>This method only works reliably when given constant has a unique value in the enumeration.</remarks>
		/// <typeparam name="T">Type of the enumeration.</typeparam>
		/// <param name="value">A constant which value we need.</param>
		/// <returns>A name of the constant in the enumeration that has its value match the given one.</returns>
		public static string GetName<T>(this T value) where T : Enum
		{
			// Cache the value to prevent boxing operation from creating a temporary object.
			getNameValueCache = value;

			return Enum.GetName(typeof(T), getNameValueCache);
		}
	}
}