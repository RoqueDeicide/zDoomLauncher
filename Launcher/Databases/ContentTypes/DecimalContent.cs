using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Launcher.Databases
{

	/// <summary>
	/// Represents an object that provides access to decimal content of database entry.
	/// </summary>
	[EntryContent("Decimal", typeof(DecimalContent))]
	public class DecimalContent : DatabaseEntryContent, IEquatableToNumberContent, IEquatableToNumber,
								  IComparableToNumberContent, IComparableToNumber
	{
		#region Properties

		/// <summary>
		/// Gets or sets <see cref="decimal"/> value of this content object.
		/// </summary>
		public decimal Value { get; set; }

		#endregion

		#region Construction

		/// <summary>
		/// Creates default instance of <see cref="DecimalContent"/> class.
		/// </summary>
		public DecimalContent()
		{
			this.Value = 0;
		}

		/// <summary>
		/// Creates new instance of <see cref="DecimalContent"/> class.
		/// </summary>
		/// <param name="value">Value of the content.</param>
		public DecimalContent(decimal value)
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
			this.Value = br.ReadDecimal();
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
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.Decimal.MinValue"/> or greater than <see
		/// cref="F:System.Decimal.MaxValue"/>.
		/// </exception>
		public override void FromXml(XmlElement element)
		{
			this.Value = Convert.ToDecimal(element.GetAttribute("value"));
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
			return this.Value == other?.Value;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(DecimalContent other)
		{
			return this.Value == other?.Value;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(DoubleContent other)
		{
			return other != null && this.Value == (decimal) other.Value;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(long other)
		{
			return this.Value == other;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(ulong other)
		{
			return this.Value == (long) other;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(int other)
		{
			return this.Value == other;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(uint other)
		{
			return this.Value == other;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(decimal other)
		{
			return this.Value == other;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(float other)
		{
			return this.Value == (decimal) other;
		}

		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(double other)
		{
			return this.Value == (decimal) other;
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