using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Launcher.Extensions;

namespace Launcher.Databases
{
	/// <summary>
	/// Represents an object that provides access to text content of database entry.
	/// </summary>
	[EntryContent("Text", typeof(TextContent))]
	public class TextContent : DatabaseEntryContent, IEquatableToText,
		IEquatableToNumberContent, IEquatableToNumber, IComparableToText,
		IComparableToNumberContent, IComparableToNumber, IConvertible, IEnumerable<char>
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
		/// <param name="bw">
		/// <see cref="BinaryWriter"/> object that provides access to the stream.
		/// </param>
		public override void ToBinary(BinaryWriter bw)
		{
			bw.WriteLongString(this.Text, Encoding.UTF8);
		}
		/// <summary>
		/// Reads binary data from the stream and converts it to format of this content.
		/// </summary>
		/// <param name="br">
		/// <see cref="BinaryReader"/> object that provides access to the stream.
		/// </param>
		public override void FromBinary(BinaryReader br)
		{
			br.ReadLongString(Encoding.UTF8);
		}
		/// <summary>
		/// Writes Xml representation of this content to the <see cref="XmlElement"/>.
		/// </summary>
		/// <param name="document">
		/// <see cref="XmlDocument"/> that will contain data.
		/// </param>
		/// <param name="element"> 
		/// <see cref="XmlElement"/> that will contain data.
		/// </param>
		public override void ToXml(XmlDocument document, XmlElement element)
		{
			element.AppendChild(document.CreateTextNode(this.Text));
		}
		/// <summary>
		/// Reads Xml representation of this content.
		/// </summary>
		/// <param name="element"><see cref="XmlElement"/> that contains data.</param>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(TextContent other)
		{
			return this.Text == other.Text;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(string other)
		{
			return this.Text == other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(StringBuilder other)
		{
			return this.Text == other.ToString();
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
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
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(string other)
		{
			return String.Compare(this.Text, other, StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(TextContent other)
		{
			return String.Compare(this.Text, other.Text, StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(StringBuilder other)
		{
			return String.Compare(this.Text, other.ToString(), StringComparison.Ordinal);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(IntegerContent other)
		{
			return String.Compare
			(
				this.Text,
				other.Value.ToString(CultureInfo.InvariantCulture),
				StringComparison.Ordinal
			);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(DecimalContent other)
		{
			return String.Compare
			(
				this.Text,
				other.Value.ToString(CultureInfo.InvariantCulture),
				StringComparison.Ordinal
			);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(DoubleContent other)
		{
			return String.Compare
			(
				this.Text,
				other.Value.ToString(CultureInfo.InvariantCulture),
				StringComparison.Ordinal
			);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(long other)
		{
			return String.Compare
			(
				this.Text,
				other.ToString(CultureInfo.InvariantCulture),
				StringComparison.Ordinal
			);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(ulong other)
		{
			return String.Compare
			(
				this.Text,
				other.ToString(CultureInfo.InvariantCulture),
				StringComparison.Ordinal
			);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(int other)
		{
			return String.Compare
			(
				this.Text,
				other.ToString(CultureInfo.InvariantCulture),
				StringComparison.Ordinal
			);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(uint other)
		{
			return String.Compare
			(
				this.Text,
				other.ToString(CultureInfo.InvariantCulture),
				StringComparison.Ordinal
			);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(decimal other)
		{
			return String.Compare
			(
				this.Text,
				other.ToString(CultureInfo.InvariantCulture),
				StringComparison.Ordinal
			);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(float other)
		{
			return String.Compare
			(
				this.Text,
				other.ToString(CultureInfo.InvariantCulture),
				StringComparison.Ordinal
			);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(double other)
		{
			return String.Compare
			(
				this.Text,
				other.ToString(CultureInfo.InvariantCulture),
				StringComparison.Ordinal
			);
		}
		#endregion
		#region Conversions
		/// <summary>
		/// Returns type code of this object.
		/// </summary>
		/// <returns></returns>
		public TypeCode GetTypeCode()
		{
			return TypeCode.Object;
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public bool ToBoolean(IFormatProvider provider)
		{
			try
			{
				return Convert.ToBoolean(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public byte ToByte(IFormatProvider provider)
		{
			try
			{
				return Convert.ToByte(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public char ToChar(IFormatProvider provider)
		{
			try
			{
				return Convert.ToChar(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public DateTime ToDateTime(IFormatProvider provider)
		{
			try
			{
				return Convert.ToDateTime(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public decimal ToDecimal(IFormatProvider provider)
		{
			try
			{
				return Convert.ToDecimal(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public double ToDouble(IFormatProvider provider)
		{
			try
			{
				return Convert.ToDouble(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public short ToInt16(IFormatProvider provider)
		{
			try
			{
				return Convert.ToInt16(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public int ToInt32(IFormatProvider provider)
		{
			try
			{
				return Convert.ToInt32(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public long ToInt64(IFormatProvider provider)
		{
			try
			{
				return Convert.ToInt64(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public sbyte ToSByte(IFormatProvider provider)
		{
			try
			{
				return Convert.ToSByte(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public float ToSingle(IFormatProvider provider)
		{
			try
			{
				return Convert.ToSingle(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public string ToString(IFormatProvider provider)
		{
			try
			{
				return this.Text;
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Throws <see cref="InvalidCastException"/>.
		/// </summary>
		/// <param name="conversionType"></param>
		/// <param name="provider">      </param>
		/// <returns></returns>
		public object ToType(Type conversionType, IFormatProvider provider)
		{
			throw new InvalidCastException();
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public ushort ToUInt16(IFormatProvider provider)
		{
			try
			{
				return Convert.ToUInt16(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public uint ToUInt32(IFormatProvider provider)
		{
			try
			{
				return Convert.ToUInt32(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		/// <summary>
		/// Converts value of this content object to specific type.
		/// </summary>
		/// <param name="provider">Not used.</param>
		/// <returns>Value of this content object.</returns>
		public ulong ToUInt64(IFormatProvider provider)
		{
			try
			{
				return Convert.ToUInt64(this.Text);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
		}
		#endregion
		#region Enumeration
		/// <summary>
		/// Gets enumerator for this text.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<char> GetEnumerator()
		{
			return this.Text.GetEnumerator();
		}
		/// <summary>
		/// Gets enumerator for this text.
		/// </summary>
		/// <returns></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
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
		#region Fields
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets value of this content object.
		/// </summary>
		public long Value { get; set; }
		#endregion
		#region Events
		#endregion
		#region Interface
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
		#region Save/Load
		/// <summary>
		/// Writes binary representation of this content to the stream.
		/// </summary>
		/// <param name="bw">
		/// <see cref="BinaryWriter"/> object that provides access to the stream.
		/// </param>
		public override void ToBinary(BinaryWriter bw)
		{
			bw.Write(this.Value);
		}
		/// <summary>
		/// Reads binary data from the stream and converts it to format of this content.
		/// </summary>
		/// <param name="br">
		/// <see cref="BinaryReader"/> object that provides access to the stream.
		/// </param>
		public override void FromBinary(BinaryReader br)
		{
			this.Value = br.ReadInt64();
		}
		/// <summary>
		/// Writes Xml representation of this content to the <see cref="XmlElement"/>.
		/// </summary>
		/// <param name="document">
		/// <see cref="XmlDocument"/> that will contain data.
		/// </param>
		/// <param name="element"> 
		/// <see cref="XmlElement"/> that will contain data.
		/// </param>
		public override void ToXml(XmlDocument document, XmlElement element)
		{
			element.SetAttribute("value", this.Value.ToString());
		}
		/// <summary>
		/// Reads Xml representation of this content.
		/// </summary>
		/// <param name="element"><see cref="XmlElement"/> that contains data.</param>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(IntegerContent other)
		{
			return this.Value == other.Value;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(DecimalContent other)
		{
			return this.Value == other.Value;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(DoubleContent other)
		{
			return Math.Abs(this.Value - other.Value) < 0.000001;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(long other)
		{
			return this.Value == other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(ulong other)
		{
			return this.Value == (long)other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(int other)
		{
			return this.Value == other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(uint other)
		{
			return this.Value == other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(decimal other)
		{
			return this.Value == other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(float other)
		{
			return Math.Abs(this.Value - other) < 0.000001;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(double other)
		{
			return Math.Abs(this.Value - other) < 0.000001;
		}
		#endregion
		#region Comparing
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(IntegerContent other)
		{
			return this.Value.CompareTo(other.Value);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(DecimalContent other)
		{
			return this.Value.CompareTo(other.Value);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(DoubleContent other)
		{
			return this.Value.CompareTo(other.Value);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(long other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(ulong other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(int other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(uint other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(decimal other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(float other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
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
		#region Fields
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets <see cref="Decimal"/> value of this content object.
		/// </summary>
		public decimal Value { get; set; }
		#endregion
		#region Events
		#endregion
		#region Interface
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
		#region Save/Load
		/// <summary>
		/// Writes binary representation of this content to the stream.
		/// </summary>
		/// <param name="bw">
		/// <see cref="BinaryWriter"/> object that provides access to the stream.
		/// </param>
		public override void ToBinary(BinaryWriter bw)
		{
			bw.Write(this.Value);
		}
		/// <summary>
		/// Reads binary data from the stream and converts it to format of this content.
		/// </summary>
		/// <param name="br">
		/// <see cref="BinaryReader"/> object that provides access to the stream.
		/// </param>
		public override void FromBinary(BinaryReader br)
		{
			this.Value = br.ReadDecimal();
		}
		/// <summary>
		/// Writes Xml representation of this content to the <see cref="XmlElement"/>.
		/// </summary>
		/// <param name="document">
		/// <see cref="XmlDocument"/> that will contain data.
		/// </param>
		/// <param name="element"> 
		/// <see cref="XmlElement"/> that will contain data.
		/// </param>
		public override void ToXml(XmlDocument document, XmlElement element)
		{
			element.SetAttribute("value", this.Value.ToString());
		}
		/// <summary>
		/// Reads Xml representation of this content.
		/// </summary>
		/// <param name="element"><see cref="XmlElement"/> that contains data.</param>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(IntegerContent other)
		{
			return this.Value == other.Value;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(DecimalContent other)
		{
			return this.Value == other.Value;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(DoubleContent other)
		{
			return this.Value == (decimal)other.Value;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(long other)
		{
			return this.Value == other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(ulong other)
		{
			return this.Value == (long)other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(int other)
		{
			return this.Value == other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(uint other)
		{
			return this.Value == other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(decimal other)
		{
			return this.Value == other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(float other)
		{
			return this.Value == (decimal)other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(double other)
		{
			return this.Value == (decimal)other;
		}
		#endregion
		#region Comparing
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(IntegerContent other)
		{
			return this.Value.CompareTo(other.Value);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(DecimalContent other)
		{
			return this.Value.CompareTo(other.Value);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(DoubleContent other)
		{
			return this.Value.CompareTo(other.Value);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(long other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(ulong other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(int other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(uint other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(decimal other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(float other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
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
	/// Represents an object that provides access to double number content of database
	/// entry.
	/// </summary>
	[EntryContent("Double", typeof(DoubleContent))]
	public class DoubleContent : DatabaseEntryContent, IEquatableToNumberContent, IEquatableToNumber,
		IComparableToNumberContent, IComparableToNumber
	{
		#region Fields
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets value of this content object.
		/// </summary>
		public double Value { get; set; }
		#endregion
		#region Events
		#endregion
		#region Interface
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
		#region Save/Load
		/// <summary>
		/// Writes binary representation of this content to the stream.
		/// </summary>
		/// <param name="bw">
		/// <see cref="BinaryWriter"/> object that provides access to the stream.
		/// </param>
		public override void ToBinary(BinaryWriter bw)
		{
			bw.Write(this.Value);
		}
		/// <summary>
		/// Reads binary data from the stream and converts it to format of this content.
		/// </summary>
		/// <param name="br">
		/// <see cref="BinaryReader"/> object that provides access to the stream.
		/// </param>
		public override void FromBinary(BinaryReader br)
		{
			this.Value = br.ReadDouble();
		}
		/// <summary>
		/// Writes Xml representation of this content to the <see cref="XmlElement"/>.
		/// </summary>
		/// <param name="document">
		/// <see cref="XmlDocument"/> that will contain data.
		/// </param>
		/// <param name="element"> 
		/// <see cref="XmlElement"/> that will contain data.
		/// </param>
		public override void ToXml(XmlDocument document, XmlElement element)
		{
			element.SetAttribute("value", this.Value.ToString());
		}
		/// <summary>
		/// Reads Xml representation of this content.
		/// </summary>
		/// <param name="element"><see cref="XmlElement"/> that contains data.</param>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(IntegerContent other)
		{
			return Math.Abs(this.Value - other.Value) < 0.000001;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(DecimalContent other)
		{
			return Math.Abs(this.Value - (double)other.Value) < 0.000001;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(DoubleContent other)
		{
			return Math.Abs(this.Value - other.Value) < 0.000001;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(long other)
		{
			return Math.Abs(this.Value - other) < 0.000001;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(ulong other)
		{
			return Math.Abs(this.Value - (long)other) < 0.000001;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(int other)
		{
			return Math.Abs(this.Value - other) < 0.000001;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(uint other)
		{
			return Math.Abs(this.Value - other) < 0.000001;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(decimal other)
		{
			return Math.Abs(this.Value - (double)other) < 0.000001;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(float other)
		{
			return Math.Abs(this.Value - other) < 0.000001;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(double other)
		{
			return Math.Abs(this.Value - other) < 0.000001;
		}
		#endregion
		#region Comparing
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(IntegerContent other)
		{
			return this.Value.CompareTo(other.Value);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(DecimalContent other)
		{
			return this.Value.CompareTo(other.Value);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(DoubleContent other)
		{
			return this.Value.CompareTo(other.Value);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(long other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(ulong other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(int other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(uint other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(decimal other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
		/// </para>
		/// <para>1 - This content object should succeed other object.</para>
		/// </returns>
		public int CompareTo(float other)
		{
			return this.Value.CompareTo(other);
		}
		/// <summary>
		/// Determines relative order of this content object and other object in a sorted
		/// collection.
		/// </summary>
		/// <param name="other">Other object.</param>
		/// <returns>
		/// <para>- 1 - This content object should precede other object.</para>
		/// <para>
		/// 0 - This content object and other object can occupy same spot in a sorted
		/// sequence.
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
	/// Represents an object that provides access to boolean content of database entry.
	/// </summary>
	[EntryContent("Boolean", typeof(BooleanContent))]
	public class BooleanContent : DatabaseEntryContent, IEquatable<bool>, IEquatable<BooleanContent>
	{
		#region Fields
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets boolean value of this object.
		/// </summary>
		public bool Value { get; set; }
		#endregion
		#region Events
		#endregion
		#region Interface
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
		#region Save/Load
		/// <summary>
		/// Writes binary representation of this content to the stream.
		/// </summary>
		/// <param name="bw">
		/// <see cref="BinaryWriter"/> object that provides access to the stream.
		/// </param>
		public override void ToBinary(BinaryWriter bw)
		{
			bw.Write(this.Value);
		}
		/// <summary>
		/// Reads binary data from the stream and converts it to format of this content.
		/// </summary>
		/// <param name="br">
		/// <see cref="BinaryReader"/> object that provides access to the stream.
		/// </param>
		public override void FromBinary(BinaryReader br)
		{
			this.Value = br.ReadBoolean();
		}
		/// <summary>
		/// Writes Xml representation of this content to the <see cref="XmlElement"/>.
		/// </summary>
		/// <param name="document">
		/// <see cref="XmlDocument"/> that will contain data.
		/// </param>
		/// <param name="element"> 
		/// <see cref="XmlElement"/> that will contain data.
		/// </param>
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
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(bool other)
		{
			return this.Value == other;
		}
		/// <summary>
		/// Determines equality of this content object and other object.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this content object equals to other object, otherwise false.
		/// </returns>
		public bool Equals(BooleanContent other)
		{
			return this.Value == other.Value;
		}
		#endregion
		#region Utilities
		#endregion
	}
	#region Interfaces
	/// <summary>
	/// </summary>
	public interface IEquatableToNumberContent : IEquatable<IntegerContent>, IEquatable<DecimalContent>, IEquatable<DoubleContent>
	{
	}
	/// <summary>
	/// </summary>
	public interface IEquatableToNumber : IEquatable<long>, IEquatable<ulong>, IEquatable<int>, IEquatable<uint>, IEquatable<decimal>, IEquatable<float>, IEquatable<double>
	{
	}
	/// <summary>
	/// </summary>
	public interface IEquatableToText : IEquatable<TextContent>, IEquatable<string>, IEquatable<StringBuilder>
	{
	}
	/// <summary>
	/// </summary>
	public interface IComparableToNumber : IComparable<long>, IComparable<ulong>, IComparable<int>, IComparable<uint>, IComparable<decimal>, IComparable<float>, IComparable<double>
	{
	}
	/// <summary>
	/// </summary>
	public interface IComparableToNumberContent : IComparable<IntegerContent>, IComparable<DecimalContent>, IComparable<DoubleContent>
	{
	}
	/// <summary>
	/// </summary>
	public interface IComparableToText : IComparable<string>, IComparable<TextContent>, IComparable<StringBuilder>
	{
	}
	#endregion
}