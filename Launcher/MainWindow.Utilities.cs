using Launcher.Databases;

namespace Launcher
{
	public partial class MainWindow
	{
		private static TextContent ToEntryContent(bool condition, string text)
		{
			return new TextContent(condition ? text : "Nothing");
		}
	}
}