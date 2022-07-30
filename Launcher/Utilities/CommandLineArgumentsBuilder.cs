using System.Linq;
using System.Text;

namespace Launcher.Utilities
{
	/// <summary>
	/// Represents objects that assist in creating a string of command line arguments.
	/// </summary>
	public class CommandLineArgumentsBuilder
	{
		/// <summary>
		/// Gets an underlying object that is constructing the string of command line arguments.
		/// </summary>
		public StringBuilder LineBuilder { get; } = new StringBuilder();

		/// <summary>
		/// Adds an argument to the string.
		/// </summary>
		/// <param name="argument">An argument to add. Any special characters are automatically escaped.</param>
		public void Add(string argument)
		{
			if (string.IsNullOrEmpty(argument))
			{
				return;
			}

			if (this.LineBuilder.Length != 0)
			{
				this.LineBuilder.Append(' ');
			}

			// Surround with quotes, if there is at least one space.
			string boundsCharacter = "";
			if (argument.Any(x => x == ' '))
			{
				boundsCharacter = "\"";
			}

			this.LineBuilder.Append(boundsCharacter);

			// Escape the quotes.
			string[] parts = argument.Split('\"');
			this.LineBuilder.Append(parts[0]);
			for (int i = 1; i < parts.Length; i++)
			{
				this.LineBuilder.Append("\"\"\"");
				this.LineBuilder.Append(parts[i]);
			}

			this.LineBuilder.Append(boundsCharacter);
		}

		/// <summary>
		/// Adds a number of arguments to the string.
		/// </summary>
		/// <param name="argument1">An argument to add. Any special characters are automatically escaped.</param>
		/// <param name="argument2">An argument to add. Any special characters are automatically escaped.</param>
		public void Add(string argument1, string argument2)
		{
			this.Add(argument1);
			this.Add(argument2);
		}

		/// <summary>
		/// Adds a number of arguments to the string.
		/// </summary>
		/// <param name="argument1">An argument to add. Any special characters are automatically escaped.</param>
		/// <param name="argument2">An argument to add. Any special characters are automatically escaped.</param>
		/// <param name="argument3">An argument to add. Any special characters are automatically escaped.</param>
		public void Add(string argument1, string argument2, string argument3)
		{
			this.Add(argument1);
			this.Add(argument2);
			this.Add(argument3);
		}

		/// <summary>
		/// Adds a number of arguments to the string.
		/// </summary>
		/// <param name="argument1">An argument to add. Any special characters are automatically escaped.</param>
		/// <param name="argument2">An argument to add. Any special characters are automatically escaped.</param>
		/// <param name="argument3">An argument to add. Any special characters are automatically escaped.</param>
		/// <param name="argument4">An argument to add. Any special characters are automatically escaped.</param>
		public void Add(string argument1, string argument2, string argument3, string argument4)
		{
			this.Add(argument1);
			this.Add(argument2);
			this.Add(argument3);
			this.Add(argument4);
		}

		/// <summary>
		/// Adds a number of arguments to the string.
		/// </summary>
		/// <param name="arguments">An array of arguments to add. Any special characters are automatically escaped.</param>
		public void Add(params string[] arguments)
		{
			foreach (string argument in arguments)
			{
				this.Add(argument);
			}
		}
		/// <summary>
		/// Adds an argument to the string, if a condition is satisfied.
		/// </summary>
		/// <param name="argument">An argument to add. Any special characters are automatically escaped.</param>
		/// <param name="condition">A <see cref="bool"/> value that indicates whether a condition for addition has been met.</param>
		public void AddIf(string argument, bool condition)
		{
			if (condition)
			{
				this.Add(argument);
			}
		}
		/// <summary>
		/// Adds a number of arguments to the string, if a condition is satisfied.
		/// </summary>
		/// <param name="argument1">An argument to add. Any special characters are automatically escaped.</param>
		/// <param name="argument2">An argument to add. Any special characters are automatically escaped.</param>
		/// <param name="condition">A <see cref="bool"/> value that indicates whether a condition for addition has been met.</param>
		public void AddIf(string argument1, string argument2, bool condition)
		{
			if (condition)
			{
				this.Add(argument1);
				this.Add(argument2);
			}
		}
		/// <summary>
		/// Adds a number of arguments to the string, if a condition is satisfied.
		/// </summary>
		/// <param name="argument1">An argument to add. Any special characters are automatically escaped.</param>
		/// <param name="argument2">An argument to add. Any special characters are automatically escaped.</param>
		/// <param name="argument3">An argument to add. Any special characters are automatically escaped.</param>
		/// <param name="condition">A <see cref="bool"/> value that indicates whether a condition for addition has been met.</param>
		public void AddIf(string argument1, string argument2, string argument3, bool condition)
		{
			if (condition)
			{
				this.Add(argument1);
				this.Add(argument2);
				this.Add(argument3);
			}
		}
		/// <inheritdoc/>
		public override string ToString()
		{
			return this.LineBuilder.ToString();
		}
	}
}