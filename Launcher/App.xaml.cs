﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Launcher.Logging;

namespace Launcher
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private void StartUp(object sender, StartupEventArgs e)
		{
			AppDomain.CurrentDomain.UnhandledException += (o, args) =>
			{
				Log.Error("An error has occurred:");

				Exception ex = args.ExceptionObject as Exception;

				if (ex == null)
				{
					Log.Error("Unknown format of the error: additional information cannot be provided.");
				}
				else
				{
					Log.Error(ex.Message);
					Log.Error(ex.StackTrace);

					Exception inner = ex.InnerException;
					while (inner != null)
					{
						Log.Error("Caused by:");
						Log.Error(inner.Message);
						Log.Error(inner.StackTrace);

						inner = inner.InnerException;
					}
				}
			};

			// Make sure that the working directory is not screwed up when launching the app from the
			// link.
			string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (exePath == null)
			{
				return;
			}
			Directory.SetCurrentDirectory(exePath);
		}
	}
}