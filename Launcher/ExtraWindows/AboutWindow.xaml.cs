using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for AboutWindow.xaml
	/// </summary>
	public partial class AboutWindow
	{
		public AboutWindow()
		{
			this.InitializeComponent();

			var assembly = Assembly.GetExecutingAssembly();

			var title = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
			this.AssemblyTitleTextBox.Text = title.Title;

			var description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
			this.AssemblyDescriptionTextBox.Text = description.Description;

			var product = assembly.GetCustomAttribute<AssemblyProductAttribute>();
			this.AssemblyProductTextBox.Text = product.Product;

			var copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
			this.AssemblyCopyrightTextBox.Text = copyright.Copyright;

			this.AssemblyVersionTextBox.Text = assembly.GetName().Version.ToString();

			// ReSharper disable once ExceptionNotDocumented
			this.AssemblyFileVersionTextBox.Text = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
		}

		private void OpenBrowser(object sender, RoutedEventArgs e)
		{
			if (!(sender is Hyperlink link))
			{
				return;
			}

			Process.Start(link.NavigateUri.ToString());
		}

		private void GoToWebsite(object sender, RoutedEventArgs e)
		{
			Process.Start("https://github.com/RoqueDeicide/zDoomLauncher");
		}
	}
}