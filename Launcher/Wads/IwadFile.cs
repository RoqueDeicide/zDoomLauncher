using System;

namespace Launcher
{
	/// <summary>
	/// Represents an IWAD file.
	/// </summary>
	public class IwadFile : IComparable<IwadFile>, IEquatable<IwadFile>
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
		/// meanings: Value Meaning Less than zero means this object is less than the <paramref name="other"/> parameter. Zero means this object is equal to
		/// <paramref name="other"/>. Greater than zero means this object is greater than <paramref name="other"/>.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public int CompareTo(IwadFile other)
		{
			return string.Compare(this.FileName, other.FileName, StringComparison.Ordinal);
		}

		/// <summary>
		/// Determines whether this is the same IWAD file as another one.
		/// </summary>
		/// <param name="other">An object that represents another IWAD file.</param>
		/// <returns><c>true</c>, this is the same IWAD file as an <paramref name="other"/> one, otherwise <c>false</c>.</returns>
		public bool Equals(IwadFile other)
		{
			if (other is null) return false;
			if (ReferenceEquals(this, other)) return true;
			return this.FileName == other.FileName;
		}

		/// <summary>
		/// Determines whether the other object is an instance of type <see cref="IwadFile"/> and if it represents the same IWAD file as this one.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>
		/// <c>true</c>, this is the same IWAD file as one represented by <paramref name="obj"/>, if the latter is of type <see cref="IwadFile"/>, otherwise
		/// <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj is null) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && this.Equals((IwadFile)obj);
		}

		/// <summary>
		/// Calculates a hash code of this IWAD file's name.
		/// </summary>
		/// <returns>Hash code of <see cref="FileName"/> property.</returns>
		public override int GetHashCode()
		{
			return this.FileName != null ? this.FileName.GetHashCode() : 0;
		}

		/// <summary>
		/// Determines whether 2 objects of type <see cref="IwadFile"/> are equal.
		/// </summary>
		/// <param name="left"> Left object.</param>
		/// <param name="right">Right object.</param>
		/// <returns><c>true</c>, if 2 objects are equal, otherwise, <c>false</c>.</returns>
		public static bool operator ==(IwadFile left, IwadFile right)
		{
			return Equals(left, right);
		}

		/// <summary>
		/// Determines whether 2 objects of type <see cref="IwadFile"/> are not equal.
		/// </summary>
		/// <param name="left"> Left object.</param>
		/// <param name="right">Right object.</param>
		/// <returns><c>true</c>, if 2 objects are not equal, otherwise, <c>false</c>.</returns>
		public static bool operator !=(IwadFile left, IwadFile right)
		{
			return !Equals(left, right);
		}

		#endregion
	}
}