﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
