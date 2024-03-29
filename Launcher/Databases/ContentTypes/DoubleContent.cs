﻿using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Launcher.Databases
{
	/// <summary>
	/// Represents an object that provides access to double number content of database entry.
	/// </summary>
	[EntryContent("Double", typeof(DoubleContent))]
	public class DoubleContent : DatabaseEntryContent, IEquatableToNumberContent, IEquatableToNumber, IComparableToNumberContent, IComparableToNumber
	{
		#region Properties

		/// <summary>
		/// Gets or sets value of this content object.
		/// </summary>
		public double Value { get; set; }

		#endregion

		#region Construction

		/// <summary>
		/// Creates default instance of <see cref="DoubleContent"/> class.
		/// </summary>
		public DoubleContent()
		{
			this.Value = 0;
		}

		/// <summary>
		/// Creates new instance of <see cref="DoubleContent"/> class.
		/// </summary>
		/// <param name="value">Value of the content.</param>
		public DoubleContent(double value)
		{
			this.Value = value;
		}

		#endregion

		#region Interface

		#region Save/Load

		/// <summary>
		/// Writes binary representation of this content to the stream.
		/// </summary>
		/// <param name="bw"><see cref="BinaryWriter"/> object that provides access to the stream.</param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public override void ToBinary(BinaryWriter bw)
		{
			bw.Write(this.Value);
		}

		/// <summary>
		/// Reads binary data from the stream and converts it to format of this content.
		/// </summary>
		/// <param name="br"><see cref="BinaryReader"/> object that provides access to the stream.</param>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public override void FromBinary(BinaryReader br)
		{
			this.Value = br.ReadDouble();
		}

		/// <summary>
		/// Writes Xml representation of this content to the <see cref="XmlElement"/>.
		/// </summary>
		/// <param name="document"><see cref="XmlDocument"/> that will contain data.</param>
		/// <param name="element"> <see cref="XmlElement"/> that will contain data.</param>
		/// <exception cref="XmlException">The specified name contains an invalid character.</exception>
		/// <exception cref="ArgumentException">The node is read-only.</exception>
		public override void ToXml(XmlDocument document, XmlElement element)
		{
			element.SetAttribute("value", this.Value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Reads Xml representation of this content.
		/// </summary>
		/// <param name="element"><see cref="XmlElement"/> that contains data.</param>
		/// <exception cref="FormatException">Value is not a number in a valid format.</exception>
		/// <exception cref="FormatException">Value is not a number in a valid format.</exception>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="double.MinValue"/> or greater than <see cref="double.MaxValue"/>.
		/// </exception>
		public override void FromXml(XmlElement element)
		{
			this.Value = Convert.ToDouble(element.GetAttribute("value"));
		}

		#endregion

		#region Equating

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(IntegerContent other)
		{
			return other != null && Math.Abs(this.Value - other.Value) < 0.000001;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(DecimalContent other)
		{
			return other != null && Math.Abs(this.Value - (double)other.Value) < 0.000001;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(DoubleContent other)
		{
			return other != null && Math.Abs(this.Value - other.Value) < 0.000001;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(long other)
		{
			return Math.Abs(this.Value - other) < 0.000001;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(ulong other)
		{
			return Math.Abs(this.Value - (long)other) < 0.000001;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(int other)
		{
			return Math.Abs(this.Value - other) < 0.000001;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(uint other)
		{
			return Math.Abs(this.Value - other) < 0.000001;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(decimal other)
		{
			return Math.Abs(this.Value - (double)other) < 0.000001;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(float other)
		{
			return Math.Abs(this.Value - other) < 0.000001;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(double other)
		{
			return Math.Abs(this.Value - other) < 0.000001;
		}

		#endregion

		#region Comparing

		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>0 - This content object and other object can occupy same spot in a sorted sequence.</para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(IntegerContent other)
		{
			return this.Value.CompareTo(other.Value);
		}

		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>0 - This content object and other object can occupy same spot in a sorted sequence.</para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(DecimalContent other)
		{
			return this.Value.CompareTo(other.Value);
		}

		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>0 - This content object and other object can occupy same spot in a sorted sequence.</para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(DoubleContent other)
		{
			return this.Value.CompareTo(other.Value);
		}

		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>0 - This content object and other object can occupy same spot in a sorted sequence.</para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(long other)
		{
			return this.Value.CompareTo(other);
		}

		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>0 - This content object and other object can occupy same spot in a sorted sequence.</para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(ulong other)
		{
			return this.Value.CompareTo(other);
		}

		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>0 - This content object and other object can occupy same spot in a sorted sequence.</para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(int other)
		{
			return this.Value.CompareTo(other);
		}

		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>0 - This content object and other object can occupy same spot in a sorted sequence.</para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(uint other)
		{
			return this.Value.CompareTo(other);
		}

		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>0 - This content object and other object can occupy same spot in a sorted sequence.</para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(decimal other)
		{
			return this.Value.CompareTo(other);
		}

		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>0 - This content object and other object can occupy same spot in a sorted sequence.</para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(float other)
		{
			return this.Value.CompareTo(other);
		}

		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>0 - This content object and other object can occupy same spot in a sorted sequence.</para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(double other)
		{
			return this.Value.CompareTo(other);
		}

		#endregion

		#endregion
	}
}