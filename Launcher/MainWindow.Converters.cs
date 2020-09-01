using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Launcher
{
	/// <summary>
	/// Generic converter between any non-flag enumeration and bool.
	/// </summary>
	public class EnumBooleanConverter : IValueConverter
	{
		#region IValueConverter Members
		/// <summary>
		/// Convert enum to bool.
		/// </summary>
		/// <param name="value">Enum.</param>
		/// <param name="targetType">Type.</param>
		/// <param name="parameter">Parameter.</param>
		/// <param name="culture">Ignored.</param>
		/// <returns>If <paramref name="value"/> equals <paramref name="parameter"/></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(parameter is string parameterString))
				return DependencyProperty.UnsetValue;

			if (value != null && Enum.IsDefined(value.GetType(), value) == false)
				return DependencyProperty.UnsetValue;

			if (value != null)
			{
				object parameterValue = Enum.Parse(value.GetType(), parameterString);

				return parameterValue.Equals(value);
			}

			return false;
		}
		/// <summary>
		/// Converts bool to enum.
		/// </summary>
		/// <param name="value">bool</param>
		/// <param name="targetType">target type</param>
		/// <param name="parameter">enum to return if bool is true</param>
		/// <param name="culture">Ignored.</param>
		/// <returns><see cref="DependencyProperty.UnsetValue"/> or <paramref name="parameter"/> parsed as enum.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(parameter is string parameterString))
				return DependencyProperty.UnsetValue;

			return Enum.Parse(targetType, parameterString);
		}
		#endregion
	}
}