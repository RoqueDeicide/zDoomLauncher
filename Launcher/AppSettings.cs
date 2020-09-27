using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Launcher.Annotations;

namespace Launcher
{
	/// <summary>
	/// Represents an object that manages the settings of this app.
	/// </summary>
	public static partial class AppSettings
	{
		private static string zDoomDirectory;
		private static string currentExeFile;
		private static string currentConfigFile;
		private static bool   startMaximized;
		private static bool   startWithSize;
		private static int    startingWidth;
		private static int    startingHeight;
		private static bool   startAtPosition;
		private static int    startingX;
		private static int    startingY;

		/// <summary>
		/// Gets or sets the path to the directory that contains zDoom-compatible executable file.
		/// </summary>
		public static string ZDoomDirectory
		{
			get => zDoomDirectory;
			set
			{
				if (value == zDoomDirectory || !Directory.Exists(value)) return;
				zDoomDirectory = value;
				OnStaticPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the path to executable file to use.
		/// </summary>
		public static string CurrentExeFile
		{
			get => currentExeFile;
			set
			{
				if (value == currentExeFile) return;
				currentExeFile = value;
				OnStaticPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the path to the current configuration file.
		/// </summary>
		public static string CurrentConfigFile
		{
			get => currentConfigFile;
			set
			{
				if (value == currentConfigFile) return;
				currentConfigFile = value;
				OnStaticPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the value that indicates whether this application should start maximized.
		/// </summary>
		public static bool StartMaximized
		{
			get => startMaximized;
			set
			{
				if (value == startMaximized) return;
				startMaximized = value;
				OnStaticPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the value that indicates whether this application should start with custom size for the window.
		/// </summary>
		public static bool StartWithSize
		{
			get => startWithSize;
			set
			{
				if (value == startWithSize) return;
				startWithSize = value;
				OnStaticPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the width of the window to start this application with.
		/// </summary>
		public static int StartingWidth
		{
			get => startingWidth;
			set
			{
				if (value == startingWidth) return;
				startingWidth = value;
				OnStaticPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the height of the window to start this application with.
		/// </summary>
		public static int StartingHeight
		{
			get => startingHeight;
			set
			{
				if (value == startingHeight) return;
				startingHeight = value;
				OnStaticPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the value that indicates whether this application should position its window at specific
		/// location upon start.
		/// </summary>
		public static bool StartAtPosition
		{
			get => startAtPosition;
			set
			{
				if (value == startAtPosition) return;
				startAtPosition = value;
				OnStaticPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the X-coordinate of the point on the screen where to place top-left corner of the window.
		/// </summary>
		public static int StartingX
		{
			get => startingX;
			set
			{
				if (value == startingX) return;
				startingX = value;
				OnStaticPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the Y-coordinate of the point on the screen where to place top-left corner of the window.
		/// </summary>
		public static int StartingY
		{
			get => startingY;
			set
			{
				if (value == startingY) return;
				startingY = value;
				OnStaticPropertyChanged();
			}
		}

		/// <summary>
		/// Occurs when one of the static properties changes its value.
		/// </summary>
		public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

		/// <summary>
		/// Raises <see cref="StaticPropertyChanged"/> event.
		/// </summary>
		/// <param name="propertyName">
		/// Name of the property that had been changed. If not provided, the calling property's name is used.
		/// </param>
		[NotifyPropertyChangedInvocator]
		private static void OnStaticPropertyChanged([CallerMemberName] string propertyName = null)
		{
			StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
		}
	}
}