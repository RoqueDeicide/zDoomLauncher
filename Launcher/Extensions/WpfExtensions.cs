using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Launcher.Annotations;

namespace Launcher
{
	internal enum AccentState
	{
		AccentDisabled                  = 1,
		AccentEnableGradient            = 0,
		AccentEnableTransparentGradient = 2,
		AccentEnableBlurBehind          = 3,
		AccentInvalidState              = 4
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct AccentPolicy
	{
		public AccentState AccentState;
		public int         AccentFlags;
		public int         GradientColor;
		public int         AnimationId;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct WindowCompositionAttributeData
	{
		public WindowCompositionAttribute Attribute;
		public IntPtr                     Data;
		public int                        SizeOfData;
	}

	internal enum WindowCompositionAttribute
	{
		WcaAccentPolicy = 19
	}

	/// <summary>
	/// Defines extension methods for objects that are related to WPF.
	/// </summary>
	public static class WpfExtensions
	{
		/// <summary>
		/// Looks through the sequence of parents of given dependency object in search for a first occurrence of a
		/// parent of type <typeparamref name="ParentType"/>.
		/// </summary>
		/// <typeparam name="ParentType">Type of parent to look for.</typeparam>
		/// <param name="child">A dependency which parents need to be looked through.</param>
		/// <returns>An object that represents a parent element, if found, otherwise returns <c>null</c>.</returns>
		[CanBeNull]
		public static ParentType FindVisualParent<ParentType>([CanBeNull] this DependencyObject child)
			where ParentType : DependencyObject
		{
			if (child == null)
			{
				return null;
			}

			DependencyObject currentParent = VisualTreeHelper.GetParent(child);
			while (currentParent != null)
			{
				if (currentParent is ParentType typedParent)
				{
					return typedParent;
				}

				currentParent = VisualTreeHelper.GetParent(currentParent);
			}

			return null;
		}

		/// <summary>
		/// Looks through the visual tree of the given dependency object in search for a child element of specified
		/// type.
		/// </summary>
		/// <typeparam name="ChildType">Type of a child element to look for.</typeparam>
		/// <param name="depObject">Dependency object which visual tree needs to be searched.</param>
		/// <returns>An object that represents a child element, if found, otherwise returns <c>null</c>.</returns>
		[CanBeNull]
		public static ChildType FindVisualChild<ChildType>([CanBeNull] this DependencyObject depObject)
			where ChildType : DependencyObject
		{
			if (depObject == null)
			{
				return null;
			}

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObject); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(depObject, i);

				if (child is ChildType visualChild)
				{
					return visualChild;
				}

				var innerChild = child.FindVisualChild<ChildType>();
				if (innerChild != null)
				{
					return innerChild;
				}
			}

			return null;
		}

		/// <summary>
		/// Converts WinRT color to WPF color.
		/// </summary>
		/// <param name="color">WinRT color.</param>
		/// <returns>WPF Color.</returns>
		public static Color ToWpfColor(this Windows.UI.Color color)
		{
			return Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		[DllImport("user32.dll")]
		private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

		/// <summary>
		/// Enables acrylic background effect for the window.
		/// </summary>
		/// <param name="window">Window to enable the effect on.</param>
		public static void EnableAcrylicBackground([NotNull] this Window window)
		{
			if (Environment.OSVersion.Platform      != PlatformID.Win32NT ||
				Environment.OSVersion.Version.Major < 10)
			{
				return;
			}

			var helper = new WindowInteropHelper(window);

			var accent = new AccentPolicy {AccentState = AccentState.AccentEnableBlurBehind};

			int accentSize = Marshal.SizeOf(accent);

			IntPtr accentPtr = Marshal.AllocHGlobal(accentSize);

			try
			{
				Marshal.StructureToPtr(accent, accentPtr, false);

				var data = new WindowCompositionAttributeData
						   {
							   Attribute  = WindowCompositionAttribute.WcaAccentPolicy,
							   Data       = accentPtr,
							   SizeOfData = accentSize
						   };

				SetWindowCompositionAttribute(helper.Handle, ref data);
			}
			finally
			{
				Marshal.FreeHGlobal(accentPtr);
			}
		}
	}
}