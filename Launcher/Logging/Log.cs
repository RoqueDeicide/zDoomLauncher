using System;
using System.IO;
using System.Linq;
using Launcher.Annotations;

namespace Launcher.Logging
{
	/// <summary>
	/// Defines functions that log text to the log file.
	/// </summary>
	public static class Log
	{
		private static readonly StreamWriter sw;
		static Log()
		{
			sw = new StreamWriter(new FileStream("Zdl.log", FileMode.Create, FileAccess.Write, FileShare.Read));
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
				sw.WriteLine(message, args);
				sw.Flush();
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
				sw.WriteLine(" [WARNING] " + message, args);
				sw.Flush();
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
				sw.WriteLine(" [ERROR] " + message, args);
				sw.Flush();
			}
			catch (IOException)
			{
			}
		}
	}
}