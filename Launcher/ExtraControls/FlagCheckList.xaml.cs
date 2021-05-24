using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Launcher.Annotations;

namespace Launcher
{
	/// <summary>
	/// Assigned to enumeration fields that should be changed through <see cref="FlagCheckList"/> control.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class FlagInfoAttribute : Attribute
	{
		/// <summary>
		/// Gets the name of the flag.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets description of the flag.
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Creates a new attribute.
		/// </summary>
		/// <param name="name">       Name of the flag.</param>
		/// <param name="description">Description of the flag.</param>
		public FlagInfoAttribute(string name, string description)
		{
			this.Name        = name;
			this.Description = description;
		}
	}

	/// <summary>
	/// Represents an object that contains additional information about a value from enumeration.
	/// </summary>
	public class EnumFlagDescriptor : INotifyPropertyChanged
	{
		private bool set;

		/// <summary>
		/// Gets the identifier of a flag this object provides information for.
		/// </summary>
		public Enum Value { get; }

		/// <summary>
		/// Gets the name of the value.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets description of the value.
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Indicates whether this flag is set.
		/// </summary>
		public bool Set
		{
			get => this.set;
			set
			{
				if (value == this.set) return;
				this.set = value;
				this.OnPropertyChanged();
			}
		}

		/// <summary>
		/// Creates a new descriptor.
		/// </summary>
		/// <param name="value">      Value to describe.</param>
		/// <param name="name">       Name of the value.</param>
		/// <param name="description">Description of the value.</param>
		public EnumFlagDescriptor(Enum value, string name, string description)
		{
			this.Value       = value;
			this.Name        = name;
			this.Description = description;
		}

		/// <summary>
		/// Occurs when a property changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises <see cref="PropertyChanged"/>.
		/// </summary>
		/// <param name="propertyName">Name of the property that was changed.</param>
		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	/// <summary>
	/// Represents a control that provides a list of check-boxes to manipulate an enumeration of flags.
	/// </summary>
	public partial class FlagCheckList
	{
		private ObservableCollection<EnumFlagDescriptor> Descriptors { get; }

		/// <summary>
		/// Backing field for <see cref="Flags"/> property.
		/// </summary>
		public static readonly DependencyProperty FlagsProperty =
			DependencyProperty.Register("Flags", typeof(Enum), typeof(FlagCheckList),
										new FrameworkPropertyMetadata(default(Enum), UpdateTheFlagList)
										{
											BindsTwoWayByDefault       = true,
											DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
										});

		private static void UpdateTheFlagList(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.OldValue is null && e.NewValue is null || !(d is FlagCheckList list))
			{
				return;
			}

			if (e.OldValue is null || e.NewValue is null || e.OldValue.GetType() != e.NewValue.GetType())
			{
				// Re-create the list of descriptors.
				list.Descriptors.Clear();

				if (e.NewValue != null)
				{
					Type enumType = e.NewValue.GetType();

					if (enumType.GetCustomAttribute(typeof(FlagsAttribute)) is null)
					{
						throw new NotSupportedException($"Type {enumType.FullName} doesn't have [Flags] attribute.");
					}

					var infosQuery = from fieldInfo in enumType.GetFields(BindingFlags.Public | BindingFlags.Static)
									 let flagInfo = fieldInfo.GetCustomAttribute<FlagInfoAttribute>()
									 where flagInfo != null
									 select (fieldInfo, flagInfo);

					foreach ((FieldInfo fieldInfo, FlagInfoAttribute flagInfo) in infosQuery)
					{
						var value = fieldInfo.GetValue(null) as Enum;
						list.Descriptors.Add(new EnumFlagDescriptor(value, flagInfo.Name, flagInfo.Description));
					}
				}
			}

			// Update checks.
			if (e.OldValue != null && !e.OldValue.Equals(e.NewValue))
			{
				if (!(e.NewValue is Enum newValue))
				{
					throw new NotSupportedException();
				}

				foreach (EnumFlagDescriptor descriptor in list.Descriptors)
				{
					bool newSetValue = newValue.HasFlag(descriptor.Value);
					if (newSetValue != descriptor.Set)
					{
						descriptor.Set = newSetValue;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a set of flags that being manipulated by this control.
		/// </summary>
		public Enum Flags
		{
			get => (Enum)this.GetValue(FlagsProperty);
			set => this.SetValue(FlagsProperty, value);
		}

		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		public FlagCheckList()
		{
			this.Descriptors = new ObservableCollection<EnumFlagDescriptor>();

			this.InitializeComponent();

			void BindItemsSource(object sender, RoutedEventArgs args)
			{
				this.ItemsSource = this.Descriptors;

				this.Loaded -= BindItemsSource;
			}

			this.Loaded += BindItemsSource;
		}

		private void ToggleFlag(object sender, RoutedEventArgs e)
		{
			if (sender is CheckBox {DataContext: EnumFlagDescriptor descriptor})
			{
				ulong flagsUlong = (ulong)Convert.ChangeType(this.Flags,       typeof(ulong));
				ulong valueUlong = (ulong)Convert.ChangeType(descriptor.Value, typeof(ulong));

				if (descriptor.Set)
				{
					flagsUlong &= ~valueUlong;
				}
				else
				{
					flagsUlong |= valueUlong;
				}

				this.Flags = (Enum)Enum.Parse(this.Flags.GetType(), flagsUlong.ToString());
			}
		}
	}
}