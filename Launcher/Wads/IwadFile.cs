using System;

namespace Launcher
{
	/// <summary>
	/// Represents an IWAD file.
	/// </summary>
	public class IwadFile : IComparable<IwadFile>
	{
		#region Properties

		/// <summary>
		/// Gets the name of the file that represents this IWAD.
		/// </summary>
		public string FileName { get; }

		/// <summary>
		/// Gets the name of the game that is represented by this IWAD.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Indicates whether this IWAD represents an episodic game.
		/// </summary>
		public bool Episodic { get; }

		/// <summary>
		/// Gets or sets the value that indicates whether this IWAD file can be loaded.
		/// </summary>
		public bool Available { get; set; }

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="IwadFile"/> class.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="name">    Name of the game.</param>
		/// <param name="episodic">Indicates whether this IWAD represents an episodic game.</param>
		public IwadFile(string fileName, string name, bool episodic)
		{
			this.FileName = fileName;
			this.Name     = name;
			this.Episodic = episodic;
		}

		#endregion

		#region Interface

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has the following
		/// meanings: Value Meaning Less than zero means this object is less than the <paramref name="other"/>
		/// parameter. Zero means this object is equal to <paramref name="other"/>. Greater than zero means this object
		/// is greater than <paramref name="other"/>.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public int CompareTo(IwadFile other)
		{
			return string.Compare(this.FileName, other.FileName, StringComparison.Ordinal);
		}

		#endregion
	}
}