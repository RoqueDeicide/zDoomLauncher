using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Windows.UI.ViewManagement;
using Launcher.Utilities;
using ModernWpf;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for AccentColorPicker.xaml
	/// </summary>
	public partial class AccentColorPicker
	{
		public AccentColorPicker()
		{
			this.InitializeComponent();

			UiSettings.Current.ColorValuesChanged         += (sender, args) => { this.UpdateSystemSettingButton(); };
			ThemeManager.Current.ActualAccentColorChanged += (sender, args) => { this.UpdateSystemSettingButton(); };
			this.UpdateSystemSettingButton();
		}

		private void UpdateSystemSettingButton()
		{
			this.SystemSettingButton.Background =
				new SolidColorBrush(UiSettings.Current.GetColorValue(UIColorType.Accent).ToWpfColor());
		}

		private void SelectSystemSetting(object sender, RoutedEventArgs e)
		{
			ThemeManager.Current.AccentColor = null;
		}
	}

	/// <summary>
	/// Represents a pre-defined list of objects that describe window accent colors.
	/// </summary>
	public class AccentColors : List<AccentColor>
	{
		public AccentColors()
		{
			this.Add("#FFB900", "Yellow gold");
			this.Add("#FF8C00", "Gold");
			this.Add("#F7630C", "Orange bright");
			this.Add("#CA5010", "Orange dark");
			this.Add("#DA3B01", "Rust");
			this.Add("#EF6950", "Pale rust");
			this.Add("#D13438", "Brick red");
			this.Add("#FF4343", "Mod red");
			this.Add("#E74856", "Pale red");
			this.Add("#E81123", "Red");
			this.Add("#EA005E", "Rose bright");
			this.Add("#C30052", "Rose");
			this.Add("#E3008C", "Plum light");
			this.Add("#BF0077", "Plum");
			this.Add("#C239B3", "Orchid light");
			this.Add("#9A0089", "Orchid");
			this.Add("#0078D7", "Default blue");
			this.Add("#0063B1", "Navy blue");
			this.Add("#8E8CD8", "Purple shadow");
			this.Add("#6B69D6", "Purple shadow Dark");
			this.Add("#8764B8", "Iris pastel");
			this.Add("#744DA9", "Iris spring");
			this.Add("#B146C2", "Violet red light");
			this.Add("#881798", "Violet red");
			this.Add("#0099BC", "Cool blue bright");
			this.Add("#2D7D9A", "Cool blue");
			this.Add("#00B7C3", "Seafoam");
			this.Add("#038387", "Seafoam team");
			this.Add("#00B294", "Mint light");
			this.Add("#018574", "Mint dark");
			this.Add("#00CC6A", "Turf green");
			this.Add("#10893E", "Sport green");
			this.Add("#7A7574", "Gray");
			this.Add("#5D5A58", "Gray brown");
			this.Add("#68768A", "Steel blue");
			this.Add("#515C6B", "Metal blue");
			this.Add("#567C73", "Pale moss");
			this.Add("#486860", "Moss");
			this.Add("#498205", "Meadow green");
			this.Add("#107C10", "Green");
			this.Add("#767676", "Overcast");
			this.Add("#4C4A48", "Storm");
			this.Add("#69797E", "Blue gray");
			this.Add("#4A5459", "Gray dark");
			this.Add("#647C64", "Liddy green");
			this.Add("#525E54", "Sage");
			this.Add("#847545", "Camouflage desert");
			this.Add("#7E735F", "Camouflage");
		}

		public void Add(string colorDescription, string colorName)
		{
			object o = ColorConverter.ConvertFromString(colorDescription);
			if (o is Color color)
			{
				this.Add(new AccentColor(color, colorName));
			}
		}
	}

	/// <summary>
	/// Represents a window accent color.
	/// </summary>
	public class AccentColor
	{
		/// <summary>
		/// Gets the color object.
		/// </summary>
		public Color Color { get; }

		/// <summary>
		/// Gets the name of the accent color.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets the brush that can be used to paint UI elements in this color.
		/// </summary>
		public SolidColorBrush Brush { get; }

		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <param name="color">Accent color.</param>
		/// <param name="name"> Name of the accent color.</param>
		public AccentColor(Color color, string name)
		{
			this.Color = color;
			this.Name  = name;
			this.Brush = new SolidColorBrush(color);
		}
	}
}