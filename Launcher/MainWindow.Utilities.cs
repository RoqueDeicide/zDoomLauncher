using Launcher.Databases;

namespace Launcher
{
	public partial class MainWindow
	{
		private static TextContent ToEntryContent(bool condition, string text)
		{
			return new TextContent(condition ? text : "Nothing");
		}

		private void UpdateWindowTitle()
		{
			bool hasName = !string.IsNullOrWhiteSpace(this.config?.Name);
			bool hasFile = !string.IsNullOrWhiteSpace(this.CurrentConfigFile);

			this.Title = $"ZDoom Launcher{(hasName ? " - " : "")}{(hasName ? this.config.Name : "")}" +
						 $"{(hasFile ? " - " : "")}{(hasFile ? this.CurrentConfigFile : "")}";
		}
	}
}