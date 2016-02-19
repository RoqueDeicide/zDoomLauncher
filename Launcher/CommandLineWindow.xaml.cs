using System;
using System.Linq;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for CommandLineWindow.xaml
	/// </summary>
	public partial class CommandLineWindow
	{
		public CommandLineWindow()
		{
			this.InitializeComponent();
		}
		public CommandLineWindow(string line)
		{
			this.InitializeComponent();
			this.CommandLineTextBox.Text = line;
		}
	}
}