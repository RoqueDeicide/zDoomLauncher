using System;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;

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

			Paragraph paragraph = new Paragraph();

			paragraph.Inlines.Add(new Run("Number of characters in the command line is equal to "));
			paragraph.Inlines.Add(new Bold(new Run(line.Length.ToString()))
			{
				Foreground = line.Length > 2080 ? Brushes.Red : Brushes.Green
			});
			paragraph.Inlines.Add(new Run("."));

			this.CharacterCountTextBox.Document = new FlowDocument(paragraph);
		}
	}
}