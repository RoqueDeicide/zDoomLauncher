using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Launcher
{
	/// <summary>
	/// Represents a single button in <see cref="SpinnerButtons"/>.
	/// </summary>
	public class SpinnerButton : Button
	{
		/// <summary>
		/// A dependency property that represents the value that indicates whether this button is pointing upwards.
		/// </summary>
		public static readonly DependencyProperty IsUpProperty =
			DependencyProperty.Register("IsUp", typeof(bool), typeof(SpinnerButton),
										new FrameworkPropertyMetadata
											(default(bool),
											 FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// A dependency property that represents the brush that is used to render the spinner buttons when they are moused over.
		/// </summary>
		public static readonly DependencyProperty HighlightedBrushProperty =
			DependencyProperty.Register("HighlightedBrush", typeof(Brush), typeof(SpinnerButton),
										new FrameworkPropertyMetadata
											(default(Brush),
											 FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// A dependency property that represents the brush that is used to render the spinner buttons when they are pressed.
		/// </summary>
		public static readonly DependencyProperty PressedBrushProperty =
			DependencyProperty.Register("PressedBrush", typeof(Brush), typeof(SpinnerButton),
										new FrameworkPropertyMetadata
											(default(Brush),
											 FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// A dependency property that represents the brush that is used to render the spinner buttons when they are disabled.
		/// </summary>
		public static readonly DependencyProperty DisabledBrushProperty =
			DependencyProperty.Register("DisabledBrush", typeof(Brush), typeof(SpinnerButton),
										new FrameworkPropertyMetadata
											(default(Brush),
											 FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// A dependency property that represents the collection of points that form a polygon that is displayed by the button.
		/// </summary>
		public static readonly DependencyProperty GeometryProperty =
			DependencyProperty.Register("Geometry", typeof(PointCollection), typeof(SpinnerButton),
										new FrameworkPropertyMetadata
											(default(PointCollection),
											 FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// Gets or sets the brush that is used to render the spinner buttons when they are moused over.
		/// </summary>
		public Brush HighlightedBrush
		{
			get { return (Brush)this.GetValue(HighlightedBrushProperty); }
			set { this.SetValue(HighlightedBrushProperty, value); }
		}
		/// <summary>
		/// Gets or sets the brush that is used to render the spinner buttons when they are pressed.
		/// </summary>
		public Brush PressedBrush
		{
			get { return (Brush)this.GetValue(PressedBrushProperty); }
			set { this.SetValue(PressedBrushProperty, value); }
		}
		/// <summary>
		/// Gets or sets the brush that is used to render the spinner buttons when they are disabled.
		/// </summary>
		public Brush DisabledBrush
		{
			get { return (Brush)this.GetValue(DisabledBrushProperty); }
			set { this.SetValue(DisabledBrushProperty, value); }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this button is pointing upwards.
		/// </summary>
		public bool IsUp
		{
			get { return (bool)this.GetValue(IsUpProperty); }
			set { this.SetValue(IsUpProperty, value); }
		}
		/// <summary>
		/// Gets or sets the collection of points that form a polygon that is displayed by the button.
		/// </summary>
		public PointCollection Geometry
		{
			get { return (PointCollection)this.GetValue(GeometryProperty); }
			set { this.SetValue(GeometryProperty, value); }
		}
		static SpinnerButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinnerButton), new FrameworkPropertyMetadata(typeof(SpinnerButton)));
		}
	}
}
