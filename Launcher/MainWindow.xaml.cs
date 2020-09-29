using System;
using System.Windows;
using ModernWpf.Controls;
using ModernWpf.Media.Animation;
using Point = System.Windows.Point;
using Size = System.Windows.Size;
using PathIO = System.IO.Path;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public static MainWindow Current;

		private Point defaultWindowPosition;
		private Size  defaultWindowSize;

		public MainWindow()
		{
			Current = this;

			this.InitializeComponent();
		}

		/// <summary>
		/// Resets position of this window to default.
		/// </summary>
		public void ResetPosition()
		{
			this.Left = this.defaultWindowPosition.X;
			this.Top  = this.defaultWindowPosition.Y;
		}

		/// <summary>
		/// Resets dimensions of this window to default values.
		/// </summary>
		public void ResetSize()
		{
			this.Width  = this.defaultWindowSize.Width;
			this.Height = this.defaultWindowSize.Height;
		}

		private void SetupWindowPositionSize(object sender, RoutedEventArgs routedEventArgs)
		{
			// Store design-time dimensions and position as defaults.
			this.defaultWindowPosition = new Point(this.Left, this.Top);
			this.defaultWindowSize     = new Size(this.Width, this.Height);

			// Assign new dimensions and position if they were specified in the app configuration.
			if (AppSettings.StartAtPosition)
			{
				this.Left = AppSettings.StartingX;
				this.Top  = AppSettings.StartingY;
			}

			if (AppSettings.StartWithSize)
			{
				this.Width  = AppSettings.StartingWidth;
				this.Height = AppSettings.StartingHeight;
			}

			// Maximize the window, if necessary.
			if (AppSettings.StartMaximized)
			{
				this.WindowState = WindowState.Maximized;
			}
		}

		private void NavViewLoaded(object sender, RoutedEventArgs e)
		{
			// Load home page or settings page, if installation directory needs to be specified.
			this.MainNavigationView.SelectedItem = AppSettings.DirectoryIsValid
													   ? this.ConfigNavItem
													   : this.MainNavigationView.SettingsItem;
		}

		private void NavSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
		{
			Type pageType = null;
			if (args.IsSettingsSelected)
			{
				pageType = typeof(SettingsPage);
			}
			else if (args.SelectedItemContainer != null)
			{
				string tag = args.SelectedItemContainer.Tag.ToString();

				pageType = tag switch
						   {
							   "Home"  => typeof(MainPage),
							   "Dirs"  => typeof(DirectoriesPage),
							   "Help"  => typeof(HelpPage),
							   "About" => typeof(AboutPage),
							   _       => null
						   };
			}

			if (pageType != null)
			{
				this.NavigateTo(pageType, args.RecommendedNavigationTransitionInfo);
			}
		}

		private void NavigateTo(Type type, NavigationTransitionInfo transitionInfo)
		{
			Type currentPage = this.MainFrame.CurrentSourcePageType;

			if (type != currentPage)
			{
				this.MainFrame.Navigate(type, null, transitionInfo);
			}
		}
	}
}