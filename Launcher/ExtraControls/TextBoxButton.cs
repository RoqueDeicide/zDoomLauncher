using System.Windows;
using System.Windows.Controls;

namespace Launcher
{
	/// <summary>
	/// Represents a button that visually only represented by the text block.
	/// </summary>
	public class TextBoxButton : Button
	{
		static TextBoxButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxButton),
													 new FrameworkPropertyMetadata(typeof(TextBoxButton)));
		}
	}
}