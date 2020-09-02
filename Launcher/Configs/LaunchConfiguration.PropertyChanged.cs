using System.ComponentModel;
using System.Runtime.CompilerServices;
using Launcher.Annotations;

namespace Launcher.Configs
{
	public partial class LaunchConfiguration
	{
		/// <summary>
		/// Occurs when the value on one of the properties of this object changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises <see cref="PropertyChanged"/> event.
		/// </summary>
		/// <param name="propertyName">Name of the property that had changed.</param>
		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}