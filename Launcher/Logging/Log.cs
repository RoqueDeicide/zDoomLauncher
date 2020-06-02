using System.IO;
using Launcher.Annotations;

namespace Launcher.Logging
{
	/// <summary>
	/// Defines functions that log text to the log file.
	/// </summary>
	public static class Log
	{
		private static readonly StreamWriter LogOutput;

		static Log()
		{
			LogOutput = new StreamWriter(new FileStream("Zdl.log", FileMode.Create, FileAccess.Write,
														FileShare.Read));
		}

		/// <summary>
		/// Posts a simple message to the log.
		/// </summary>
		/// <param name="message">Message to post.</param>
		/// <param name="args">   Arguments to insert into the final message before posting it.</param>
		[StringFormatMethod("message")]
		public static void Message(string message, params object[] args)
		{
			try
			{
				LogOutput.WriteLine(message, args);
				LogOutput.Flush();
			}
			catch (IOException)
			{
			}
		}

		/// <summary>
		/// Posts a message preceded by a warning prefix to the log.
		/// </summary>
		/// <param name="message">Message to post.</param>
		/// <param name="args">   Arguments to insert into the final message before posting it.</param>
		[StringFormatMethod("message")]
		public static void Warning(string message, params object[] args)
		{
			try
			{
				LogOutput.WriteLine(" [WARNING] " + message, args);
				LogOutput.Flush();
			}
			catch (IOException)
			{
			}
		}

		/// <summary>
		/// Posts a message preceded by an error prefix to the log.
		/// </summary>
		/// <param name="message">Message to post.</param>
		/// <param name="args">   Arguments to insert into the final message before posting it.</param>
		[StringFormatMethod("message")]
		public static void Error(string message, params object[] args)
		{
			try
			{
				LogOutput.WriteLine(" [ERROR] " + message, args);
				LogOutput.Flush();
			}
			catch (IOException)
			{
			}
		}
	}
}