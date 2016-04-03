using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Launcher
{
	/// <summary>
	/// Defines a signature of methods that can handle <see cref="NumericUpDown.ValueChanged"/>
	/// </summary>
	/// <param name="sender">  An object that raised the event.</param>
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
		/// A dependency property that represents the number that can added to and subtracted from the
		/// value of the number box by clicking on buttons.
		/// </summary>
		public static readonly DependencyProperty StepProperty =
			DependencyProperty.Register("Step", typeof(int), typeof(NumericUpDown),
										new PropertyMetadata(default(int)));
		/// <summary>
		/// A dependency property that represents the brush that is used to render the spinner buttons when
		/// they are moused over.
		/// </summary>
		public static readonly DependencyProperty HighlightedBrushProperty =
			DependencyProperty.Register("HighlightedBrush", typeof(Brush), typeof(NumericUpDown),
										new FrameworkPropertyMetadata
											(default(Brush),
											 FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// A dependency property that represents the brush that is used to render the spinner buttons when
		/// they are pressed.
		/// </summary>
		public static readonly DependencyProperty PressedBrushProperty =
			DependencyProperty.Register("PressedBrush", typeof(Brush), typeof(NumericUpDown),
										new FrameworkPropertyMetadata
											(default(Brush),
											 FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// A dependency property that represents the brush that is used to render the spinner buttons when
		/// they are disabled.
		/// </summary>
		public static readonly DependencyProperty DisabledBrushProperty =
			DependencyProperty.Register("DisabledBrush", typeof(Brush), typeof(NumericUpDown),
										new FrameworkPropertyMetadata
											(default(Brush),
											 FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// A dependency property that represents the number to default the value to when incrementing or
		/// decrementing value that was previously equal to null.
		/// </summary>
		public static readonly DependencyProperty RestartPositionProperty =
			DependencyProperty.Register("RestartPosition", typeof(int?), typeof(NumericUpDown),
										new PropertyMetadata(default(int?)));
		/// <summary>
		/// A dependency property that represents the factor <see cref="Step"/> is multiplied by when
		/// incrementing/decrementing the <see cref="Value"/> while Shift key is pressed.
		/// </summary>
		public static readonly DependencyProperty ShiftStepMultiplierProperty =
			DependencyProperty.Register("ShiftStepMultiplier", typeof(uint), typeof(NumericUpDown),
										new PropertyMetadata(default(uint)));
		#endregion
		private bool valueIsCommited;
		private TextBox valueBox;
		private SpinnerButton upButton;
		private SpinnerButton downButton;
		private bool readyToClear;
		private readonly DispatcherTimer timer;
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

				this.UpdateButtons();

				this.OnValueChanged(oldValue, value);
			}
		}
		/// <summary>
		/// Gets or sets the minimal value of this number box.
		/// </summary>
		public int? Minimum
		{
			get { return (int?)this.GetValue(MinimumProperty); }
			set
			{
				this.SetValue(MinimumProperty, value);

				this.UpdateButtons();
			}
		}
		/// <summary>
		/// Gets or sets the maximal value of this number box.
		/// </summary>
		public int? Maximum
		{
			get { return (int?)this.GetValue(MaximumProperty); }
			set
			{
				this.SetValue(MaximumProperty, value);

				this.UpdateButtons();
			}
		}
		/// <summary>
		/// Gets or sets the number that can added to and subtracted from the value of the number box by
		/// clicking on buttons.
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
		/// Gets or sets the number to default the value to when incrementing or decrementing value that
		/// was previously equal to null.
		/// </summary>
		public int? RestartPosition
		{
			get { return (int?)this.GetValue(RestartPositionProperty); }
			set { this.SetValue(RestartPositionProperty, value); }
		}
		/// <summary>
		/// Gets or sets the factor <see cref="Step"/> is multiplied by when incrementing/decrementing the
		/// <see cref="Value"/> while Shift key is pressed.
		/// </summary>
		public uint ShiftStepMultiplier
		{
			get { return (uint)this.GetValue(ShiftStepMultiplierProperty); }
			set { this.SetValue(ShiftStepMultiplierProperty, value); }
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
		public NumericUpDown()
		{
			this.timer = new DispatcherTimer();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Invoked whenever application code or internal processes call
		/// <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
		/// </summary>
		public override void OnApplyTemplate()
		{
			this.upButton = this.GetTemplateChild("UpButton") as SpinnerButton;
			this.downButton = this.GetTemplateChild("DownButton") as SpinnerButton;

			if (this.upButton == null || this.downButton == null)
			{
				return;
			}

			this.upButton.Click += this.Increment;
			this.downButton.Click += this.Decrement;

			this.valueBox = this.GetTemplateChild("ValueTextBox") as TextBox;

			if (this.valueBox == null)
			{
				return;
			}

			this.timer.Interval = new TimeSpan(0, 0, 2);
			this.timer.Tick += this.UnprepareClearing;

			this.valueBox.ContextMenu = null;

			this.valueBox.PreviewMouseRightButtonDown += this.PrepareForNumberClearing;
			this.valueBox.PreviewMouseRightButtonUp += this.ClearNumber;
			this.valueBox.KeyDown += this.ValueBoxOnKeyDown;
			this.valueBox.LostKeyboardFocus += this.ValueBoxOnLostKeyboardFocus;
			this.valueBox.TextChanged += this.MarkValueUncommited;

			this.upButton.MouseWheel += this.RotateValue;
			this.downButton.MouseWheel += this.RotateValue;
			this.valueBox.MouseWheel += this.RotateValue;

			// Assign the current value to the text box once the loading is complete.
			this.Loaded += (sender, args) =>
			{
				this.valueIsCommited = true;
				this.valueBox.Text = this.Value?.ToString() ?? "";

				this.UpdateButtons();
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
		private void MarkValueUncommited(object sender, TextChangedEventArgs textChangedEventArgs)
		{
			this.valueIsCommited = false;
		}
		private void ValueBoxOnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs keyboardFocusChangedEventArgs)
		{
			if (!this.valueIsCommited)
			{
				this.CommitText(this.valueBox.Text);
			}
		}
		private void ValueBoxOnKeyDown(object sender, KeyEventArgs keyEventArgs)
		{
			TextBox box = sender as TextBox;
			if (box == null)
			{
				return;
			}

			if (keyEventArgs.Key == Key.Enter)
			{
				this.CommitText(box.Text);
			}
		}
		private void PrepareForNumberClearing(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			this.readyToClear = true;
			this.timer.Start();
		}
		private void UnprepareClearing(object sender, EventArgs eventArgs)
		{
			this.readyToClear = false;
			this.timer.Stop();
		}
		private void ClearNumber(object sender, MouseButtonEventArgs e)
		{
			if (this.readyToClear)
			{
				this.CommitText("");
				this.readyToClear = false;
				this.timer.Stop();
				e.Handled = true;
			}
		}

		private void AddValue(int value)
		{
			int? currentValue = this.Value;
			int valueToCommit;
			if (currentValue == null)
			{
				// Change it to either restarting position or minimal value or 0.
				valueToCommit = this.RestartPosition ?? this.Minimum ?? 0;
			}
			else
			{
				bool shiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

				valueToCommit = (int)(currentValue + value * (shiftDown ? this.ShiftStepMultiplier : 1));
			}

			this.CommitNumber(valueToCommit);
		}
		private void CommitNumber(int value)
		{
			MathExtra.Clamp(ref value, this.Minimum ?? int.MinValue, this.Maximum ?? int.MaxValue);
			this.Value = value;

			this.valueIsCommited = true;
			this.valueBox.Text = value.ToString(CultureInfo.InvariantCulture);
		}
		private void CommitText(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				this.Value = null;
				this.valueIsCommited = true;
				this.valueBox.Text = "";
				return;
			}

			int number;
			if (!int.TryParse(value, out number))
			{
				// Clear off all non-numeric characters.
				StringBuilder builder = new StringBuilder(value.Length);

				for (int i = 0; i < value.Length; i++)
				{
					char symbol = value[i];
					if (char.IsNumber(symbol) || symbol == '-' && i == 0)
					{
						builder.Append(symbol);
					}
				}

				number = int.Parse(builder.ToString());
			}

			this.CommitNumber(number);
		}
		protected virtual void OnValueChanged(int? oldvalue, int? newvalue)
		{
			var handler = this.ValueChanged;
			handler?.Invoke(this, oldvalue, newvalue);
		}
		private void UpdateButtons()
		{
			if (this.downButton != null)
			{
				this.downButton.IsEnabled = this.Value != (this.Minimum ?? int.MinValue);
			}
			if (this.upButton != null)
			{
				this.upButton.IsEnabled = this.Value != (this.Maximum ?? int.MaxValue);
			}
		}
		#endregion
	}
}