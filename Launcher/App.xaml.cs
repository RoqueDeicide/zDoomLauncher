using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
		}
	}
}
