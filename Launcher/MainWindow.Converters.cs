using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ModernWpf;

namespace Launcher
{
	/// <summary>
	/// Determines whether a theme menu button has to be checked.
	/// </summary>
	public class ThemeMenuButtonChecker : IValueConverter
	{
		/// <summary>
		/// Creates a value that indicates whether current app theme matches the one specified by the <paramref name="parameter"/>.
		/// </summary>
		/// <param name="value">     An instance of type <see cref="Nullable{ApplicationTheme}"/> that indicates a current application theme.</param>
		/// <param name="targetType">A type of <see cref="bool"/>.</param>
		/// <param name="parameter"> A <see cref="string"/> value that indicates what menu button the value is created for.</param>
		/// <param name="culture">   Not used.</param>
		/// <returns>A value that indicates whether current app theme matches the one specified by the <paramref name="parameter"/>.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return parameter switch
				   {
					   "Light" => value is ApplicationTheme theme && theme == ApplicationTheme.Light,
					   "Dark"  => value is ApplicationTheme theme && theme == ApplicationTheme.Dark,
					   "null"  => value == null,
					   _       => false
				   };
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}

	/// <summary>
	/// Generic converter between any non-flag enumeration and bool.
	/// </summary>
	public class EnumBooleanConverter : IValueConverter
	{
		#region IValueConverter Members

		/// <summary>
		/// Convert enum to bool.
		/// </summary>
		/// <param name="value">     Enum.</param>
		/// <param name="targetType">Type.</param>
		/// <param name="parameter"> Parameter.</param>
		/// <param name="culture">   Ignored.</param>
		/// <returns>If <paramref name="value"/> equals <paramref name="parameter"/>.</returns>
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
		/// <param name="value">     bool</param>
		/// <param name="targetType">target type</param>
		/// <param name="parameter"> enum to return if bool is true</param>
		/// <param name="culture">   Ignored.</param>
		/// <returns><see cref="DependencyProperty.UnsetValue"/> or <paramref name="parameter"/> parsed as enum.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool b && !b)
			{
				return Binding.DoNothing;
			}

			if (!(parameter is string parameterString))
				return DependencyProperty.UnsetValue;

			return Enum.Parse(targetType, parameterString);
		}

		#endregion
	}

	/// <summary>
	/// Represents a value converter that add " - " to the value.
	/// </summary>
	public class DashPrefixer : IValueConverter
	{
		/// <summary>
		/// Adds " - " prefix to a string.
		/// </summary>
		/// <param name="value">     String to add prefix to.</param>
		/// <param name="targetType">Ignored.</param>
		/// <param name="parameter"> Ignored.</param>
		/// <param name="culture">   Ignored.</param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string text && !text.IsNullOrWhiteSpace())
			{
				return $" - {text}";
			}

			return value?.ToString();
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="value">     Ignored.</param>
		/// <param name="targetType">Ignored.</param>
		/// <param name="parameter"> Ignored.</param>
		/// <param name="culture">   Ignored.</param>
		/// <returns>value</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}