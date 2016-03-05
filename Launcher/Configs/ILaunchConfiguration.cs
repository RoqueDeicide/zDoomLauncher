using System;
using System.Linq;

namespace Launcher.Configs
{
	/// <summary>
	/// Defines common functionality of launch configurations.
	/// </summary>
	public interface ILaunchConfiguration
	{
		/// <summary>
		/// Gets or sets the name of the configuration.
		/// </summary>
		string Name { get; set; }
		/// <summary>
		/// When implemented in derived class, gets the command line that can be used to launch the game
		/// with this configuration.
		/// </summary>
		/// <param name="exeFolder">Path to the folder that contains the executable file.</param>
		string GetCommandLine(string exeFolder);
	}
}