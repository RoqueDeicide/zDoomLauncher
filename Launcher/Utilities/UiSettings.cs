using Windows.UI.ViewManagement;

namespace Launcher.Utilities
{
	/// <summary>
	/// Defines an interface with WinRT <see cref="UISettings"/> class.
	/// </summary>
	public static class UiSettings
	{
		public static readonly UISettings Current = new UISettings();
	}
}