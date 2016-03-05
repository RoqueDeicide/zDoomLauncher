using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for AboutWindow.xaml
	/// </summary>
	public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			this.InitializeComponent();

			Assembly assembly = Assembly.GetExecutingAssembly();

			AssemblyTitleAttribute title = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
			this.AssemblyTitleTextBox.Text = title.Title;

			AssemblyDescriptionAttribute description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
			this.AssemblyDescriptionTextBox.Text = description.Description;

			AssemblyProductAttribute product = assembly.GetCustomAttribute<AssemblyProductAttribute>();
			this.AssemblyProductTextBox.Text = product.Product;

			AssemblyCopyrightAttribute copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
			this.AssemblyCopyrightTextBox.Text = copyright.Copyright;

			this.AssemblyVersionTextBox.Text = assembly.GetName().Version.ToString();

			// ReSharper disable once ExceptionNotDocumented
			this.AssemblyFileVersionTextBox.Text = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
		}
		private void OpenBrowser(object sender, RoutedEventArgs e)
		{
			Hyperlink link = sender as Hyperlink;
			if (link == null)
			{
				return;
			}

			Process.Start(link.NavigateUri.ToString());
		}
	}
}