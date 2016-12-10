using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Launcher.Annotations;

namespace Launcher
{
	/// <summary>
	/// Defines extension methods for objects that are related to WPF.
	/// </summary>
	public static class WpfExtensions
	{
		/// <summary>
		/// Looks through the sequence of parents of given dependency object in search for a first
		/// occurrence of a parent of type <typeparamref name="ParentType"/>.
		/// </summary>
		/// <typeparam name="ParentType">Type of parent to look for.</typeparam>
		/// <param name="child">A dependency which parents need to be looked through.</param>
		/// <returns>
		/// An object that represents a parent element, if found, otherwise returns <c>null</c>.
		/// </returns>
		[CanBeNull]
		public static ParentType FindVisualParent<ParentType>([CanBeNull] this DependencyObject child)
			where ParentType : DependencyObject
		{
			if (child == null)
			{
				return null;
			}

			var currentParent = VisualTreeHelper.GetParent(child);
			while (currentParent != null)
			{
				ParentType typedParent = currentParent as ParentType;
				if (typedParent != null)
				{
					return typedParent;
				}

				currentParent = VisualTreeHelper.GetParent(currentParent);
			}

			return null;
		}
		/// <summary>
		/// Looks through the visual tree of the given dependency object in search for a child
		/// element of specified type.
		/// </summary>
		/// <typeparam name="ChildType">Type of a child element to look for.</typeparam>
		/// <param name="depObject">Dependency object which visual tree needs to be searched.</param>
		/// <returns>
		/// An object that represents a child element, if found, otherwise returns <c>null</c>.
		/// </returns>
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
				var child = VisualTreeHelper.GetChild(depObject, i);

				var visualChild = child as ChildType;
				if (visualChild != null)
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
	}
}