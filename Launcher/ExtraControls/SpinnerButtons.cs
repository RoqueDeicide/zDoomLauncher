using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Launcher
{
	/// <summary>
	/// Represents a set of 2 buttons, one on top of the other.
	/// </summary>
	public class SpinnerButtons : Control
	{
		#region Fields

		/// <summary>
		/// A dependency property that represents the brush that is used to render the spinner buttons when they are moused over.
		/// </summary>
		public static readonly DependencyProperty HighlightedBrushProperty =
			DependencyProperty.Register("HighlightedBrush", typeof(Brush), typeof(SpinnerButtons),
										new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>
		/// A dependency property that represents the brush that is used to render the spinner buttons when they are pressed.
		/// </summary>
		public static readonly DependencyProperty PressedBrushProperty =
			DependencyProperty.Register("PressedBrush", typeof(Brush), typeof(SpinnerButtons),
										new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>
		/// A dependency property that represents the brush that is used to render the spinner buttons when they are disabled.
		/// </summary>
		public static readonly DependencyProperty DisabledBrushProperty =
			DependencyProperty.Register("DisabledBrush", typeof(Brush), typeof(SpinnerButtons),
										new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>
		/// A dependency property that indicates whether Down button is disabled.
		/// </summary>
		public static readonly DependencyProperty AtBottomProperty =
			DependencyProperty.Register("AtBottom", typeof(bool), typeof(SpinnerButtons), new PropertyMetadata(default(bool)));

		/// <summary>
		/// A dependency property that indicates whether Up button is disabled.
		/// </summary>
		public static readonly DependencyProperty AtTopProperty =
			DependencyProperty.Register("AtTop", typeof(bool), typeof(SpinnerButtons), new PropertyMetadata(default(bool)));

		private SpinnerButton upButton;
		private SpinnerButton downButton;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the brush that is used to render the spinner buttons when they are moused over.
		/// </summary>
		public Brush HighlightedBrush
		{
			get => (Brush)this.GetValue(HighlightedBrushProperty);
			set => this.SetValue(HighlightedBrushProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush that is used to render the spinner buttons when they are pressed.
		/// </summary>
		public Brush PressedBrush
		{
			get => (Brush)this.GetValue(PressedBrushProperty);
			set => this.SetValue(PressedBrushProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush that is used to render the spinner buttons when they are disabled.
		/// </summary>
		public Brush DisabledBrush
		{
			get => (Brush)this.GetValue(DisabledBrushProperty);
			set => this.SetValue(DisabledBrushProperty, value);
		}

		/// <summary>
		/// Gets or sets the value that indicates whether Down button is disabled.
		/// </summary>
		public bool AtBottom
		{
			get => (bool)this.GetValue(AtBottomProperty);
			set => this.SetValue(AtBottomProperty, value);
		}

		/// <summary>
		/// Gets or sets the value that indicates whether Up button is disabled.
		/// </summary>
		public bool AtTop
		{
			get => (bool)this.GetValue(AtTopProperty);
			set => this.SetValue(AtTopProperty, value);
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the Up button is clicked.
		/// </summary>
		public event RoutedEventHandler ClickUp;

		/// <summary>
		/// Occurs when the Down button is clicked.
		/// </summary>
		public event RoutedEventHandler ClickDown;

		#endregion

		#region Construction

		static SpinnerButtons()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinnerButtons), new FrameworkPropertyMetadata(typeof(SpinnerButtons)));
		}

		#endregion

		#region Interface

		/// <summary>
		/// Processes a new template that has been applied to this control.
		/// </summary>
		public override void OnApplyTemplate()
		{
			this.upButton   = this.GetTemplateChild("UpButton") as SpinnerButton;
			this.downButton = this.GetTemplateChild("DownButton") as SpinnerButton;

			if (this.upButton == null || this.downButton == null)
			{
				return;
			}

			this.upButton.Click   += (sender, args) => this.OnClickUp(args);
			this.downButton.Click += (sender, args) => this.OnClickDown(args);
		}

		#endregion

		#region Utilities

		/// <summary>
		/// Raises <see cref="ClickUp"/> event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected virtual void OnClickUp(RoutedEventArgs e)
		{
			this.ClickUp?.Invoke(this, e);
		}

		/// <summary>
		/// Raises <see cref="ClickDown"/> event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected virtual void OnClickDown(RoutedEventArgs e)
		{
			this.ClickDown?.Invoke(this, e);
		}

		#endregion
	}
}