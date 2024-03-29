﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Launcher
{
	/// <summary>
	/// Represents a button that represents either a arrow down or up.
	/// </summary>
	public class SpinnerButton : Button
	{
		/// <summary>
		/// A dependency property that represents the value that indicates whether this button is pointing upwards.
		/// </summary>
		public static readonly DependencyProperty IsUpProperty =
			DependencyProperty.Register(nameof(IsUp), typeof(bool), typeof(SpinnerButton),
										new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>
		/// A dependency property that represents the collection of points that form a polygon that is displayed by the button.
		/// </summary>
		public static readonly DependencyProperty GeometryProperty =
			DependencyProperty.Register(nameof(Geometry), typeof(PointCollection), typeof(SpinnerButton),
										new FrameworkPropertyMetadata(default(PointCollection), FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>
		/// Gets or sets the value that indicates whether this button is pointing upwards.
		/// </summary>
		public bool IsUp
		{
			get => (bool)this.GetValue(IsUpProperty);
			set => this.SetValue(IsUpProperty, value);
		}

		/// <summary>
		/// Gets or sets the collection of points that form a polygon that is displayed by the button.
		/// </summary>
		public PointCollection Geometry
		{
			get => (PointCollection)this.GetValue(GeometryProperty);
			set => this.SetValue(GeometryProperty, value);
		}

		static SpinnerButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinnerButton), new FrameworkPropertyMetadata(typeof(SpinnerButton)));
		}
	}
}