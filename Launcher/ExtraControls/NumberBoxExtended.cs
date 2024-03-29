﻿using System;
using System.Windows;
using System.Windows.Input;
using ModernWpf.Controls;

namespace Launcher
{
	/// <summary>
	/// A UI Element that represents a text field for input of numbers.
	/// </summary>
	/// <remarks>
	/// This type of number box allows a default value to be used whenever the box is cleared. That means <see cref="NumberBox.ValueChanged"/> event will be
	/// fired twice if <see cref="DefaultValue"/> is set to an actual number and the first event is raised with an <see cref="double.NaN"/> value.
	/// </remarks>
	public class NumberBoxExtended : NumberBox
	{
		/// <summary>
		/// A <see cref="DependencyProperty"/> that defines a default value for the number box.
		/// </summary>
		public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(nameof(DefaultValue), typeof(double), typeof(NumberBoxExtended),
																									 new PropertyMetadata(default(double)));

		/// <summary>
		/// Gets or sets a default value of this number box.
		/// </summary>
		public double DefaultValue
		{
			get => (double)this.GetValue(DefaultValueProperty);
			set => this.SetValue(DefaultValueProperty, value);
		}

		static NumberBoxExtended()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBoxExtended), new FrameworkPropertyMetadata(typeof(NumberBoxExtended)));
		}

		/// <summary>
		/// Creates a default instance of this type.
		/// </summary>
		public NumberBoxExtended()
		{
			if (!this.DefaultValue.IsNaN())
			{
				this.Value = this.DefaultValue;
			}

			this.ValueChanged += this.SetToDefaultIfCleared;

			this.PreviewMouseWheel += this.RotateTheValue;
		}

		private void RotateTheValue(object sender, MouseWheelEventArgs args)
		{
			bool large = Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);
			int  shift = args.Delta / Math.Abs(args.Delta);
			this.Value += shift * (large ? this.LargeChange : this.SmallChange);
		}

		private void SetToDefaultIfCleared(NumberBox sender, NumberBoxValueChangedEventArgs args)
		{
			if (!double.IsNaN(args.NewValue) || double.IsNaN(this.DefaultValue)) return;

			if (!double.IsNaN(this.Minimum) && !double.IsNaN(this.Maximum) && this.Minimum >= this.Maximum)
			{
				this.ThrowError<InvalidProgramException>("has its minimum value defined as greater or equal to its maximum.");
			}

#if DEBUG
			if (!double.IsNaN(this.Minimum) && this.DefaultValue < this.Minimum ||
				!double.IsNaN(this.Maximum) && this.DefaultValue > this.Maximum)
			{
				this.ThrowError<Exception>("has its default value set out of range.");
			}
#endif
			double def = this.DefaultValue.Clamp(double.IsNaN(this.Minimum) ? double.MinValue : this.Minimum,
												 double.IsNaN(this.Maximum) ? double.MaxValue : this.Maximum);
			this.Value = def;
		}

		private void ThrowError<E>(string message) where E : Exception
		{
			var e = (E)Activator.CreateInstance(typeof(E), @$"{(string.IsNullOrEmpty(this.Name)
																	? "An unnamed NumberBox"
																	: $"A NumberBox named {this.Name}")} {message}");
			throw e;
		}
	}
}