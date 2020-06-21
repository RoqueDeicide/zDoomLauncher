using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Launcher.Databases
{
	/// <summary>
	/// Represents content of the database entry that contains a vector.
	/// </summary>
	[EntryContent("Vector", typeof(VectorContent))]
	public class VectorContent : DatabaseEntryContent, IEnumerable<decimal>, IEquatable<VectorContent>
	{
		#region Fields

		private decimal[] components;

		private static readonly string[] ComponentNames = {"X", "Y", "Z", "W"};

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets one of the components of this vector entry content.
		/// </summary>
		/// <param name="index">Zero-based index of the component to access.</param>
		/// <returns>Requested component.</returns>
		public decimal this[int index]
		{
			get => this.components[index];
			set => this.components[index] = value;
		}

		/// <summary>
		/// Gets the number of components in the vector.
		/// </summary>
		public int Count => this.components.Length;

		/// <summary>
		/// Gets or sets the first component of the vector.
		/// </summary>
		public decimal X
		{
			get => this.components[0];
			set => this.components[0] = value;
		}

		/// <summary>
		/// Gets or sets the first component of the vector.
		/// </summary>
		public decimal R
		{
			get => this.components[0];
			set => this.components[0] = value;
		}

		/// <summary>
		/// Gets or sets the second component of the vector.
		/// </summary>
		public decimal Y
		{
			get => this.components[1];
			set => this.components[1] = value;
		}

		/// <summary>
		/// Gets or sets the second component of the vector.
		/// </summary>
		public decimal G
		{
			get => this.components[1];
			set => this.components[1] = value;
		}

		/// <summary>
		/// Gets or sets the third component of the vector.
		/// </summary>
		public decimal Z
		{
			get => this.components[2];
			set => this.components[2] = value;
		}

		/// <summary>
		/// Gets or sets the third component of the vector.
		/// </summary>
		public decimal B
		{
			get => this.components[2];
			set => this.components[2] = value;
		}

		/// <summary>
		/// Gets or sets the fourth component of the vector.
		/// </summary>
		public decimal W
		{
			get => this.components[3];
			set => this.components[3] = value;
		}

		/// <summary>
		/// Gets or sets the fourth component of the vector.
		/// </summary>
		public decimal A
		{
			get => this.components[3];
			set => this.components[3] = value;
		}

		#endregion

		/// <summary>
		/// Creates an empty entry content.
		/// </summary>
		public VectorContent()
		{
			this.components = null;
		}

		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="count">Number of components in the vector.</param>
		public VectorContent(int count)
		{
			this.components = new decimal [count];
		}

		/// <summary>
		/// Writes a binary representation of this vector into a stream.
		/// </summary>
		/// <param name="bw">An object that provides interface with the stream.</param>
		public override void ToBinary(BinaryWriter bw)
		{
			bw.Write(this.components.Length);
			for (int i = 0; i < this.components.Length; i++)
			{
				bw.Write(this.components[i]);
			}
		}

		/// <summary>
		/// Reads a binary representation of this vector from a stream.
		/// </summary>
		/// <param name="br">An object that provides interface with the stream.</param>
		public override void FromBinary(BinaryReader br)
		{
			int count = br.ReadInt32();
			this.components = new decimal[count];
			for (int i = 0; i < count; i++)
			{
				this.components[i] = br.ReadDecimal();
			}
		}

		/// <summary>
		/// Writes Xml representation of this content to the <see cref="XmlElement"/>.
		/// </summary>
		/// <param name="document"><see cref="XmlDocument"/> that will contain data.</param>
		/// <param name="element"> <see cref="XmlElement"/> that will contain data.</param>
		public override void ToXml(XmlDocument document, XmlElement element)
		{
			for (int i = 0; i < this.Count; i++)
			{
				element.SetAttribute(ComponentNames[i], $"{this[i]}");
			}
		}

		/// <summary>
		/// Reads Xml representation of this content from the <see cref="XmlElement"/>.
		/// </summary>
		/// <param name="element"><see cref="XmlElement"/> that will contain data.</param>
		public override void FromXml(XmlElement element)
		{
			var list = new List<decimal>(ComponentNames.Length);
			list.AddRange(from t in ComponentNames
						  where element.HasAttribute(t)
						  select Convert.ToDecimal(element.GetAttribute(t)));

			this.components = list.ToArray();
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<decimal> GetEnumerator()
		{
			for (int i = 0; i < this.Count; i++)
			{
				yield return this.components[i];
			}
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Determines whether content within this object is equal to another.
		/// </summary>
		/// <param name="other">Another instance of this type.</param>
		/// <returns>Whether content within this object is equal to another.</returns>
		public bool Equals(VectorContent other)
		{
			if (ReferenceEquals(this, other))
			{
				return true;
			}

			if (other == null)
			{
				return false;
			}

			if (this.Count == other.Count)
			{
				return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
			}

			return false;
		}
	}
}