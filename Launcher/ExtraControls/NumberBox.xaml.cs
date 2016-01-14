using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Launcher
{
	/// <summary>
	/// Defines a signature of methods that can handle <see cref="NumberBox.ValueChanged"/>
	/// </summary>
	/// <param name="sender">An object that raised the event.</param>
	/// <param name="oldValue">Old value.</param>
	/// <param name="newValue">New value.</param>
	public delegate void NumberBoxValueChangedHandler(object sender, int oldValue, int newValue);
	/// <summary>
	/// Represents a control that is used to enter and modify the numeric values.
	/// </summary>
	public partial class NumberBox : UserControl
	{
		private bool skipEvent;

		/// <summary>
		/// A dependency property that represents the value of the number box.
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(int), typeof(NumberBox),
										new PropertyMetadata(default(int)));
		/// <summary>
		/// A dependency property that represents the minimal value of the number box.
		/// </summary>
		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register("Minimum", typeof(int), typeof(NumberBox),
			new PropertyMetadata(default(int)));
		/// <summary>
		/// A dependency property that represents the maximal value of the number box.
		/// </summary>
		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register("Maximum", typeof(int), typeof(NumberBox),
			new PropertyMetadata(default(int)));
		/// <summary>
		/// A dependency property that represents the number that can added to and subtracted from the value of the number box by clicking on buttons.
		/// </summary>
		public static readonly DependencyProperty StepProperty =
			DependencyProperty.Register("Step", typeof(int), typeof(NumberBox),
			new PropertyMetadata(default(int)));

		/// <summary>
		/// Gets or sets the current value of this control.
		/// </summary>
		public int Value
		{
			get { return (int)this.GetValue(ValueProperty); }
			set { this.SetValue(ValueProperty, value); }
		}
		/// <summary>
		/// Gets or sets the minimal value of this number box.
		/// </summary>
		public int Minimum
		{
			get { return (int)this.GetValue(MinimumProperty); }
			set { this.SetValue(MinimumProperty, value); }
		}
		/// <summary>
		/// Gets or sets the maximal value of this number box.
		/// </summary>
		public int Maximum
		{
			get { return (int)this.GetValue(MaximumProperty); }
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
		/// Occurs when a value of this number box changes.
		/// </summary>
		public event NumberBoxValueChangedHandler ValueChanged;

		public NumberBox()
		{
			this.InitializeComponent();
		}

		private void ValidateText(object sender, TextChangedEventArgs e)
		{
			// Prevent this handler from running when text is guaranteed to be correct and the number is already up-to-date.
			if (this.skipEvent)
			{
				this.skipEvent = false;
				return;
			}

			// Clear off all non-number symbols.
			StringBuilder text = new StringBuilder(this.NumberTextBox.Text);

			bool textChanged = false;
			for (int i = 0; i < text.Length; i++)
			{
				char symbol = text[i];
				// Remove any non-numeric with sole exception of the '-' sign at the start.
				if (!char.IsNumber(symbol) && (symbol != '-' || i != 0))
				{
					text.Remove(i, 1);
					i--;
					textChanged = true;
				}
			}

			// Parse the number, clamp it and compare it to the existing one.
			int newValue = Convert.ToInt32(text.ToString());
			int oldValue = this.Value;

			MathExtra.Clamp(ref newValue, this.Minimum, this.Maximum);

			// Notify about the changes.
			if (oldValue != newValue)
			{
				this.Value = newValue;
				this.OnValueChanged(oldValue, newValue);
			}

			// Assign clean text to the Text property.
			if (textChanged)
			{
				this.skipEvent = true;
				this.NumberTextBox.Text = text.ToString();
			}
		}

		protected virtual void OnValueChanged(int oldvalue, int newvalue)
		{
			var handler = this.ValueChanged;
			if (handler != null) handler(this, oldvalue, newvalue);
		}

		private void IncrementValue(object sender, RoutedEventArgs e)
		{
			this.AddNumber(this.Step);
		}

		private void DecrementValue(object sender, RoutedEventArgs e)
		{
			this.AddNumber(-this.Step);
		}

		private void AddNumber(int value)
		{
			int oldValue = this.Value;
			int newValue = oldValue + value;

			MathExtra.Clamp(ref newValue, this.Minimum, this.Maximum);

			// Notify about the changes.
			if (oldValue != newValue)
			{
				this.Value = newValue;
				this.OnValueChanged(oldValue, newValue);
				this.skipEvent = true;
				this.NumberTextBox.Text = newValue.ToString(CultureInfo.InvariantCulture);
			}
		}

		private void PrepTheText(object sender, RoutedEventArgs e)
		{
			// Make the value show up in the control.
			this.skipEvent = true;
			this.NumberTextBox.Text = this.Value.ToString(CultureInfo.InvariantCulture);
		}
	}
}
