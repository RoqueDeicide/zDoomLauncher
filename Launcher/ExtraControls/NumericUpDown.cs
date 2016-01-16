using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Launcher.Logging;

namespace Launcher
{
	/// <summary>
	/// Defines a signature of methods that can handle <see cref="NumericUpDown.ValueChanged"/>
	/// </summary>
	/// <param name="sender">An object that raised the event.</param>
	/// <param name="oldValue">Old value.</param>
	/// <param name="newValue">New value.</param>
	public delegate void NumericUpDownValueChangedHandler(object sender, int? oldValue, int? newValue);
	/// <summary>
	/// Represents an equivalent of <see cref="T:System.Windows.Forms.NumericUpDown"/> in WPF.
	/// </summary>
	public class NumericUpDown : Control
	{
		#region Fields
		#region Dependency Fields
		/// <summary>
		/// A dependency property that represents the value of the number box.
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(int?), typeof(NumericUpDown),
										new FrameworkPropertyMetadata
											(default(int?),
											 FrameworkPropertyMetadataOptions.AffectsRender |
											 FrameworkPropertyMetadataOptions.AffectsParentMeasure));
		/// <summary>
		/// A dependency property that represents the minimal value of the number box.
		/// </summary>
		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register("Minimum", typeof(int?), typeof(NumericUpDown),
										new PropertyMetadata(default(int)));
		/// <summary>
		/// A dependency property that represents the maximal value of the number box.
		/// </summary>
		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register("Maximum", typeof(int?), typeof(NumericUpDown),
										new PropertyMetadata(default(int)));
		/// <summary>
		/// A dependency property that represents the number that can added to and subtracted from the value of the number box by clicking on buttons.
		/// </summary>
		public static readonly DependencyProperty StepProperty =
			DependencyProperty.Register("Step", typeof(int), typeof(NumericUpDown),
										new PropertyMetadata(default(int)));
		/// <summary>
		/// A dependency property that represents the brush that is used to render the spinner buttons when they are moused over.
		/// </summary>
		public static readonly DependencyProperty HighlightedBrushProperty =
			DependencyProperty.Register("HighlightedBrush", typeof(Brush), typeof(NumericUpDown),
										new FrameworkPropertyMetadata
											(default(Brush),
											 FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// A dependency property that represents the brush that is used to render the spinner buttons when they are pressed.
		/// </summary>
		public static readonly DependencyProperty PressedBrushProperty =
			DependencyProperty.Register("PressedBrush", typeof(Brush), typeof(NumericUpDown),
										new FrameworkPropertyMetadata
											(default(Brush),
											 FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// A dependency property that represents the brush that is used to render the spinner buttons when they are disabled.
		/// </summary>
		public static readonly DependencyProperty DisabledBrushProperty =
			DependencyProperty.Register("DisabledBrush", typeof(Brush), typeof(NumericUpDown),
										new FrameworkPropertyMetadata
											(default(Brush),
											 FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// A dependency property that represents the number to default the value to when incrementing or decrementing value that was previously equal to null.
		/// </summary>
		public static readonly DependencyProperty RestartPositionProperty =
			DependencyProperty.Register("RestartPosition", typeof(int?), typeof(NumericUpDown),
										new PropertyMetadata(default(int?)));
		#endregion
		private string lastText;
		private bool ignoreTextUpdate;
		private TextBox valueBox;
		#endregion
		#region Properties
		#region Dependency Properties
		/// <summary>
		/// Gets or sets the current value of this control.
		/// </summary>
		public int? Value
		{
			get { return (int?)this.GetValue(ValueProperty); }
			set
			{
				int? oldValue = this.Value;
				if (oldValue == value)
				{
					return;
				}

				this.SetValue(ValueProperty, value);
			
				this.OnValueChanged(oldValue, value);
			}
		}
		/// <summary>
		/// Gets or sets the minimal value of this number box.
		/// </summary>
		public int? Minimum
		{
			get { return (int?)this.GetValue(MinimumProperty); }
			set { this.SetValue(MinimumProperty, value); }
		}
		/// <summary>
		/// Gets or sets the maximal value of this number box.
		/// </summary>
		public int? Maximum
		{
			get { return (int?)this.GetValue(MaximumProperty); }
			set { this.SetValue(MaximumProperty, value); }
		}
		/// <summary>
		/// Gets or sets the number that can added to and subtracted from the value of the number box by clicking on buttons.
		/// </summary>
		public int Step
		{
			get { return (int)this.GetValue(StepProperty); }
			set { this.SetValue(StepProperty, value); }
		}
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
		/// Gets or sets the number to default the value to when incrementing or decrementing value that was previously equal to null.
		/// </summary>
		public int? RestartPosition
		{
			get { return (int?)this.GetValue(RestartPositionProperty); }
			set { this.SetValue(RestartPositionProperty, value); }
		}
		#endregion
		#endregion
		#region Events
		/// <summary>
		/// Occurs when the value of this control changes.
		/// </summary>
		public event NumericUpDownValueChangedHandler ValueChanged;
		#endregion
		#region Construction
		static NumericUpDown()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown),
													 new FrameworkPropertyMetadata(typeof(NumericUpDown)));
		}
		#endregion
		#region Interface
		/// <summary>
		/// Invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
		/// </summary>
		public override void OnApplyTemplate()
		{
			SpinnerButton upButton = this.GetTemplateChild("UpButton") as SpinnerButton;
			SpinnerButton downButton = this.GetTemplateChild("DownButton") as SpinnerButton;

			if (upButton == null || downButton == null)
			{
				return;
			}

			upButton.Click += this.Increment;
			downButton.Click += this.Decrement;

			TextBox valueTextBox = this.GetTemplateChild("ValueTextBox") as TextBox;

			if (valueTextBox == null)
			{
				return;
			}

			this.valueBox = valueTextBox;

			valueTextBox.TextChanged += this.UpdateValueFromText;
			valueTextBox.MouseRightButtonUp += this.ClearNumber;

			upButton.MouseWheel += this.RotateValue;
			downButton.MouseWheel += this.RotateValue;
			valueTextBox.MouseWheel += this.RotateValue;

			// Assign the current value to the text box once the loading is complete.
			this.Loaded += (sender, args) =>
			{
				this.ignoreTextUpdate = true;
				this.valueBox.Text = this.Value == null ? "" : this.Value.ToString();
			};
		}
		#endregion
		#region Utilities
		private void Increment(object sender, RoutedEventArgs routedEventArgs)
		{
			if (this.Step == 0)
			{
				return;
			}

			this.AddValue(this.Step);
		}
		private void Decrement(object sender, RoutedEventArgs e)
		{
			if (this.Step == 0)
			{
				return;
			}

			this.AddValue(-this.Step);
		}
		private void RotateValue(object sender, MouseWheelEventArgs mouseWheelEventArgs)
		{
			if (this.Step == 0)
			{
				return;
			}

			this.AddValue(Math.Sign(mouseWheelEventArgs.Delta) * this.Step);
		}
		private void UpdateValueFromText(object sender, TextChangedEventArgs textChangedEventArgs)
		{
			if (this.ignoreTextUpdate)
			{
				this.ignoreTextUpdate = false;
				return;
			}

			TextBox box = sender as TextBox;
			if (box == null)
			{
				return;
			}

			string currentText = box.Text;

			int newValue;
			// Set the value to null, if current text is empty or is equal to minus sign.
			//
			// Equality to minus sign is used here to allow the user to enter negative values without having to write a number and then add minus sign to it (Only relevant when negative values are supported).
			if (string.IsNullOrWhiteSpace(currentText) ||
				(currentText == "-" && (this.Minimum ?? int.MinValue) < 0))
			{
				this.UpdateValue(null, false);

				this.lastText = currentText;

				return;
			}
			if (int.TryParse(currentText, out newValue))
			{
				this.UpdateValue(newValue, false);

				this.lastText = currentText;

				return;
			}

			// Revert to the old text value.
			this.ignoreTextUpdate = true;
			box.Text = this.lastText;
		}
		private void ClearNumber(object sender, MouseButtonEventArgs e)
		{
			string currentText = this.valueBox.Text;
			if (string.IsNullOrWhiteSpace(currentText))
			{
				return;
			}

			this.valueBox.Text = "";
		}

		private void AddValue(int value)
		{
			int? currentValue = this.Value;
			if (currentValue == null)
			{
				// Change it to either restarting position or minimal value or 0.
				this.UpdateValue(this.RestartPosition ?? this.Minimum ?? 0, true);
			}
			else
			{
				this.UpdateValue((int)(currentValue + value), true);
			}
		}
		private void UpdateValue(int? value, bool updateText)
		{
			if (value == null)
			{
				this.Value = null;
				return;				// Nothing else needed, since null can only be passed via text manipulation.
			}

			int newValue = value.Value;

			int clampedValue = MathExtra.Clamp(newValue,
											   this.Minimum ?? int.MinValue,
											   this.Maximum ?? int.MaxValue);

			// Update text only if new text value has to be clamped, or if the update was caused by (de/in)crement.
			if (clampedValue != newValue || updateText)
			{
				this.ignoreTextUpdate = true;
				this.valueBox.Text = clampedValue.ToString(CultureInfo.InvariantCulture);
				this.lastText = this.valueBox.Text;
			}

			this.Value = clampedValue;
		}
		protected virtual void OnValueChanged(int? oldvalue, int? newvalue)
		{
			var handler = this.ValueChanged;
			if (handler != null) handler(this, oldvalue, newvalue);
		}
		#endregion
	}
}
