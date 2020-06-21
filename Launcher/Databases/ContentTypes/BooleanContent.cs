using System;
using System.IO;
using System.Xml;

namespace Launcher.Databases
{
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
			return this.Value == other?.Value;
		}

		#endregion
	}
}