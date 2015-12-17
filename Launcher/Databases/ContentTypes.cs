using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Launcher.Extensions;

namespace Launcher.Databases
{
	/// <summary>
	/// Represents an object that provides access to text content of database entry.
	/// </summary>
	[EntryContent("Text", typeof(TextContent))]
	public class TextContent : DatabaseEntryContent, IEquatableToText,
							   IEquatableToNumberContent, IEquatableToNumber, IComparableToText,
							   IComparableToNumberContent, IComparableToNumber, IConvertible,
							   IEnumerable<char>
	{
		/// <summary>
		/// Gets or sets text content.
		/// </summary>
		public string Text { get; set; }
		#region Interface
		#region Construction
		/// <summary>
		/// Creates default instance of <see cref="TextContent"/> class.
		/// </summary>
		public TextContent()
		{
			this.Text = "";
		}
		/// <summary>
		/// Creates new instance of <see cref="TextContent"/> class.
		/// </summary>
		/// <param name="text">Text to use.</param>
		public TextContent(string text)
		{
			this.Text = text;
		}
		#endregion
		#region Save/Load
		/// <summary>
		/// Writes binary representation of this content to the stream.
		/// </summary>
		/// <param name="bw"><see cref="BinaryWriter"/> object that provides access to the stream.</param>
		public override void ToBinary(BinaryWriter bw)
		{
			bw.WriteLongString(this.Text, Encoding.UTF8);
		}
		/// <summary>
		/// Reads binary data from the stream and converts it to format of this content.
		/// </summary>
		/// <param name="br"><see cref="BinaryReader"/> object that provides access to the stream.</param>
		public override void FromBinary(BinaryReader br)
		{
			br.ReadLongString(Encoding.UTF8);
		}
		/// <summary>
		/// Writes Xml representation of this content to the <see cref="XmlElement"/>.
		/// </summary>
		/// <param name="document"><see cref="XmlDocument"/> that will contain data.</param>
		/// <param name="element"> <see cref="XmlElement"/> that will contain data.</param>
		public override void ToXml(XmlDocument document, XmlElement element)
		{
			element.AppendChild(document.CreateTextNode(this.Text));
		}
		/// <summary>
		/// Reads Xml representation of this content.
		/// </summary>
		/// <param name="element"><see cref="XmlElement"/> that contains data.</param>
		/// <exception cref="XPathException">The XPath expression contains a prefix.</exception>
		public override void FromXml(XmlElement element)
		{
			XmlNode xmlTextNode = element.SelectSingleNode("./text()");
			if (xmlTextNode != null) this.Text = xmlTextNode.Value;
		}
		#endregion
		#region Equating
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(TextContent other)
		{
			return this.Text == other.Text;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(string other)
		{
			return this.Text == other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(StringBuilder other)
		{
			return this.Text == other.ToString();
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.Int64.MinValue"/> or greater
		/// than <see cref="F:System.Int64.MaxValue"/>.
		/// </exception>
		public bool Equals(IntegerContent other)
		{
			try
			{
				return Convert.ToInt64(this.Text) == other.Value;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.Decimal.MinValue"/> or greater
		/// than <see cref="F:System.Decimal.MaxValue"/>.
		/// </exception>
		public bool Equals(DecimalContent other)
		{
			try
			{
				return Convert.ToDecimal(this.Text) == other.Value;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.Double.MinValue"/> or greater
		/// than <see cref="F:System.Double.MaxValue"/>.
		/// </exception>
		public bool Equals(DoubleContent other)
		{
			try
			{
				return Math.Abs(Convert.ToDouble(this.Text) - other.Value) < 0.000001;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.Int64.MinValue"/> or greater
		/// than <see cref="F:System.Int64.MaxValue"/>.
		/// </exception>
		public bool Equals(long other)
		{
			try
			{
				return Convert.ToInt64(this.Text) == other;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.UInt64.MinValue"/> or greater
		/// than <see cref="F:System.UInt64.MaxValue"/>.
		/// </exception>
		public bool Equals(ulong other)
		{
			try
			{
				return Convert.ToUInt64(this.Text) == other;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.Int32.MinValue"/> or greater
		/// than <see cref="F:System.Int32.MaxValue"/>.
		/// </exception>
		public bool Equals(int other)
		{
			try
			{
				return Convert.ToInt32(this.Text) == other;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.UInt32.MinValue"/> or greater
		/// than <see cref="F:System.UInt32.MaxValue"/>.
		/// </exception>
		public bool Equals(uint other)
		{
			try
			{
				return Convert.ToUInt32(this.Text) == other;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.Decimal.MinValue"/> or greater
		/// than <see cref="F:System.Decimal.MaxValue"/>.
		/// </exception>
		public bool Equals(decimal other)
		{
			try
			{
				return Convert.ToDecimal(this.Text) == other;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.Single.MinValue"/> or greater
		/// than <see cref="F:System.Single.MaxValue"/>.
		/// </exception>
		public bool Equals(float other)
		{
			try
			{
				return Math.Abs(Convert.ToSingle(this.Text) - other) < 0.000001;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.Double.MinValue"/> or greater
		/// than <see cref="F:System.Double.MaxValue"/>.
		/// </exception>
		public bool Equals(double other)
		{
			try
			{
				return Math.Abs(Convert.ToDouble(this.Text) - other) < 0.000001;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		#endregion
		#region Comparing
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(string other)
		{
			return string.Compare(this.Text, other, StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(TextContent other)
		{
			return string.Compare(this.Text, other.Text, StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(StringBuilder other)
		{
			return string.Compare(this.Text, other.ToString(), StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(IntegerContent other)
		{
			return string.Compare(this.Text,
								  other.Value.ToString(CultureInfo.InvariantCulture),
								  StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(DecimalContent other)
		{
			return string.Compare(this.Text,
								  other.Value.ToString(CultureInfo.InvariantCulture),
								  StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(DoubleContent other)
		{
			return string.Compare(this.Text,
								  other.Value.ToString(CultureInfo.InvariantCulture),
								  StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(long other)
		{
			return string.Compare(this.Text,
								  other.ToString(CultureInfo.InvariantCulture),
								  StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(ulong other)
		{
			return string.Compare(this.Text,
								  other.ToString(CultureInfo.InvariantCulture),
								  StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(int other)
		{
			return string.Compare(this.Text,
								  other.ToString(CultureInfo.InvariantCulture),
								  StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(uint other)
		{
			return string.Compare(this.Text,
								  other.ToString(CultureInfo.InvariantCulture),
								  StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(decimal other)
		{
			return string.Compare(this.Text,
								  other.ToString(CultureInfo.InvariantCulture),
								  StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(float other)
		{
			return string.Compare(this.Text,
								  other.ToString(CultureInfo.InvariantCulture),
								  StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(double other)
		{
			return string.Compare(this.Text,
								  other.ToString(CultureInfo.InvariantCulture),
								  StringComparison.Ordinal);
		}
		#endregion
		#region Conversions
		/// <summary>
		/// Returns type code of this object.
		/// </summary>
		/// <returns>Type code of this object.</returns>
		public TypeCode GetTypeCode()
		{
			return TypeCode.Object;
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public bool ToBoolean(IFormatProvider provider)
		{
			try
			{
				return Convert.ToBoolean(this.Text);
			}
			catch (FormatException ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public byte ToByte(IFormatProvider provider)
		{
			try
			{
				return Convert.ToByte(this.Text);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public char ToChar(IFormatProvider provider)
		{
			try
			{
				return Convert.ToChar(this.Text);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public DateTime ToDateTime(IFormatProvider provider)
		{
			try
			{
				return Convert.ToDateTime(this.Text);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public decimal ToDecimal(IFormatProvider provider)
		{
			try
			{
				return Convert.ToDecimal(this.Text);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public double ToDouble(IFormatProvider provider)
		{
			try
			{
				return Convert.ToDouble(this.Text);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public short ToInt16(IFormatProvider provider)
		{
			try
			{
				return Convert.ToInt16(this.Text);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public int ToInt32(IFormatProvider provider)
		{
			try
			{
				return Convert.ToInt32(this.Text);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public long ToInt64(IFormatProvider provider)
		{
			try
			{
				return Convert.ToInt64(this.Text);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public sbyte ToSByte(IFormatProvider provider)
		{
			try
			{
				return Convert.ToSByte(this.Text);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public float ToSingle(IFormatProvider provider)
		{
			try
			{
				return Convert.ToSingle(this.Text);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public string ToString(IFormatProvider provider)
		{
			return this.Text;
		}
		/// <summary>
		/// Throws <see cref="InvalidCastException"/>.
		/// </summary>
		/// <param name="conversionType">Ignored.</param>
		/// <param name="provider">      Ignored.</param>
		/// <returns>Irrelevant.</returns>
		/// <exception cref="InvalidCastException">Casting to another type is not supported.</exception>
		public object ToType(Type conversionType, IFormatProvider provider)
		{
			throw new InvalidCastException("Casting to another type is not supported.");
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public ushort ToUInt16(IFormatProvider provider)
		{
			try
			{
				return Convert.ToUInt16(this.Text);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public uint ToUInt32(IFormatProvider provider)
		{
			try
			{
				return Convert.ToUInt32(this.Text);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		/// <exception cref="InvalidCastException">
		/// The content of the entry doesn't represent the value of appropriate type.
		/// </exception>
		public ulong ToUInt64(IFormatProvider provider)
		{
			try
			{
				return Convert.ToUInt64(this.Text);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("The content of the entry doesn't represent the value of appropriate type.", ex);
			}
		}
		#endregion
		#region Enumeration
		/// <summary>
		/// Enumerates this text.
		/// </summary>
		/// <returns>An object that handles enumeration.</returns>
		public IEnumerator<char> GetEnumerator()
		{
			return this.Text.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Text.GetEnumerator();
		}
		#endregion
		#endregion
	}
	/// <summary>
	/// Represents an object that provides access to integer content of database entry.
	/// </summary>
	[EntryContent("Integer", typeof(IntegerContent))]
	public class IntegerContent : DatabaseEntryContent, IEquatableToNumberContent, IEquatableToNumber,
								  IComparableToNumberContent, IComparableToNumber
	{
		#region Properties
		/// <summary>
		/// Gets or sets value of this content object.
		/// </summary>
		public long Value { get; set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates default instance of <see cref="IntegerContent"/> class.
		/// </summary>
		public IntegerContent()
		{
			this.Value = 0;
		}
		/// <summary>
		/// Creates new instance of <see cref="IntegerContent"/> class.
		/// </summary>
		/// <param name="value">Value for content.</param>
		public IntegerContent(long value)
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
			this.Value = br.ReadInt64();
		}
		/// <summary>
		/// Writes Xml representation of this content to the <see cref="XmlElement"/>.
		/// </summary>
		/// <param name="document"><see cref="XmlDocument"/> that will contain data.</param>
		/// <param name="element"> <see cref="XmlElement"/> that will contain data.</param>
		/// <exception cref="XmlException">The specified name contains an invalid character.</exception>
		public override void ToXml(XmlDocument document, XmlElement element)
		{
			element.SetAttribute("value", this.Value.ToString());
		}
		/// <summary>
		/// Reads Xml representation of this content.
		/// </summary>
		/// <param name="element"><see cref="XmlElement"/> that contains data.</param>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.Int64.MinValue"/> or greater
		/// than <see cref="F:System.Int64.MaxValue"/>.
		/// </exception>
		public override void FromXml(XmlElement element)
		{
			this.Value = Convert.ToInt64(element.GetAttribute("value"));
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
			return this.Value == other.Value;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(DecimalContent other)
		{
			return this.Value == other.Value;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(DoubleContent other)
		{
			return Math.Abs(this.Value - other.Value) < 0.000001;
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
			return this.Value == (long)other;
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(double other)
		{
			return this.Value.CompareTo(other);
		}
		#endregion
		#endregion
		#region Utilities
		#endregion
	}
	/// <summary>
	/// Represents an object that provides access to decimal content of database entry.
	/// </summary>
	[EntryContent("Decimal", typeof(DecimalContent))]
	public class DecimalContent : DatabaseEntryContent, IEquatableToNumberContent, IEquatableToNumber,
								  IComparableToNumberContent, IComparableToNumber
	{
		#region Properties
		/// <summary>
		/// Gets or sets <see cref="Decimal"/> value of this content object.
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
			element.SetAttribute("value", this.Value.ToString());
		}
		/// <summary>
		/// Reads Xml representation of this content.
		/// </summary>
		/// <param name="element"><see cref="XmlElement"/> that contains data.</param>
		/// <exception cref="FormatException">Value is not a number in a valid format.</exception>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.Decimal.MinValue"/> or greater
		/// than <see cref="F:System.Decimal.MaxValue"/>.
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
			return this.Value == other.Value;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(DecimalContent other)
		{
			return this.Value == other.Value;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(DoubleContent other)
		{
			return this.Value == (decimal)other.Value;
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
			return this.Value == (long)other;
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
			return this.Value == (decimal)other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(double other)
		{
			return this.Value == (decimal)other;
		}
		#endregion
		#region Comparing
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(double other)
		{
			return this.Value.CompareTo(other);
		}
		#endregion
		#endregion
	}
	/// <summary>
	/// Represents an object that provides access to double number content of database entry.
	/// </summary>
	[EntryContent("Double", typeof(DoubleContent))]
	public class DoubleContent : DatabaseEntryContent, IEquatableToNumberContent, IEquatableToNumber,
								 IComparableToNumberContent, IComparableToNumber
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
			element.SetAttribute("value", this.Value.ToString());
		}
		/// <summary>
		/// Reads Xml representation of this content.
		/// </summary>
		/// <param name="element"><see cref="XmlElement"/> that contains data.</param>
		/// <exception cref="FormatException">Value is not a number in a valid format.</exception>
		/// <exception cref="FormatException">Value is not a number in a valid format.</exception>
		/// <exception cref="OverflowException">
		/// Value represents a number that is less than <see cref="F:System.Double.MinValue"/> or greater
		/// than <see cref="F:System.Double.MaxValue"/>.
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
			return Math.Abs(this.Value - other.Value) < 0.000001;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(DecimalContent other)
		{
			return Math.Abs(this.Value - (double)other.Value) < 0.000001;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(DoubleContent other)
		{
			return Math.Abs(this.Value - other.Value) < 0.000001;
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
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
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(double other)
		{
			return this.Value.CompareTo(other);
		}
		#endregion
		#endregion
	}
	/// <summary>
	/// Represents an object that provides access to boolean content of database entry.
	/// </summary>
	[EntryContent("Boolean", typeof(BooleanContent))]
	public class BooleanContent : DatabaseEntryContent, IEquatable<bool>, IEquatable<BooleanContent>
	{
		#region Properties
		/// <summary>
		/// Gets or sets boolean value of this object.
		/// </summary>
		public bool Value { get; set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates default instance of <see cref="BooleanContent"/> class.
		/// </summary>
		public BooleanContent()
		{
			this.Value = false;
		}
		/// <summary>
		/// Creates new instance of <see cref="BooleanContent"/> class.
		/// </summary>
		/// <param name="value">Value of the content.</param>
		public BooleanContent(bool value)
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
			this.Value = br.ReadBoolean();
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
			element.SetAttribute("value", this.Value ? "1" : "0");
		}
		/// <summary>
		/// Reads Xml representation of this content.
		/// </summary>
		/// <param name="element"><see cref="XmlElement"/> that contains data.</param>
		public override void FromXml(XmlElement element)
		{
			this.Value = element.GetAttribute("value") == "1";
		}
		#endregion
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(bool other)
		{
			return this.Value == other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if this content object equals to other object, otherwise false.</returns>
		public bool Equals(BooleanContent other)
		{
			return this.Value == other.Value;
		}
		#endregion
	}
	#region Interfaces
	/// <summary>
	/// Combines interfaces that are implemented by numeric content types.
	/// </summary>
	public interface IEquatableToNumberContent : IEquatable<IntegerContent>, IEquatable<DecimalContent>,
												 IEquatable<DoubleContent>
	{
	}
	/// <summary>
	/// Combines interfaces that are implemented by objects that can be equated to numbers.
	/// </summary>
	public interface IEquatableToNumber : IEquatable<long>, IEquatable<ulong>, IEquatable<int>,
										  IEquatable<uint>, IEquatable<decimal>, IEquatable<float>,
										  IEquatable<double>
	{
	}
	/// <summary>
	/// Combines interfaces that are implemented by objects that can be equated to text.
	/// </summary>
	public interface IEquatableToText : IEquatable<TextContent>, IEquatable<string>, IEquatable<StringBuilder>
	{
	}
	/// <summary>
	/// Combines interfaces that are implemented by objects that can be compared to numbers.
	/// </summary>
	public interface IComparableToNumber : IComparable<long>, IComparable<ulong>, IComparable<int>,
										   IComparable<uint>, IComparable<decimal>, IComparable<float>,
										   IComparable<double>
	{
	}
	/// <summary>
	/// Combines interfaces that are implemented by objects that can be compared to numbers that are
	/// encapsulated into numeric entry content objects.
	/// </summary>
	public interface IComparableToNumberContent : IComparable<IntegerContent>, IComparable<DecimalContent>,
												  IComparable<DoubleContent>
	{
	}
	/// <summary>
	/// Combines interfaces that are implemented by objects that can be compared to text.
	/// </summary>
	public interface IComparableToText : IComparable<string>, IComparable<TextContent>,
										 IComparable<StringBuilder>
	{
	}
	#endregion
}