using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Launcher.Utilities;

namespace Launcher.Databases
{
	/// <summary>
	/// Represents content of the database entry that contains a vector.
	/// </summary>
	/// <typeparam name="ComponentType">Type of vector components.</typeparam>
	[EntryContent("VectorByte",    typeof(VectorContent<byte>))]
	[EntryContent("VectorInt16",   typeof(VectorContent<short>))]
	[EntryContent("VectorInt32",   typeof(VectorContent<int>))]
	[EntryContent("VectorInt64",   typeof(VectorContent<long>))]
	[EntryContent("VectorSingle",  typeof(VectorContent<float>))]
	[EntryContent("VectorDouble",  typeof(VectorContent<double>))]
	[EntryContent("VectorDecimal", typeof(VectorContent<decimal>))]
	public class VectorContent<ComponentType> : DatabaseEntryContent, IEnumerable<ComponentType>, IEquatable<VectorContent<ComponentType>>
		where ComponentType : IEquatable<ComponentType>
	{
		#region Fields

		private ComponentType[] components;

		private static readonly string[]                            ComponentNames = { "X", "Y", "Z", "W" };
		private static readonly MethodInfo                          ParseMethod;
		private static readonly Action<BinaryWriter, ComponentType> Write;
		private static readonly Func<BinaryReader, ComponentType>   Read;

		static VectorContent()
		{
			ParseMethod = typeof(ComponentType).GetMethod("Parse", new[] { typeof(string) });
			if (ParseMethod == null)
			{
				throw new Exception($"{typeof(ComponentType).FullName} type doesn't have static Parse method.");
			}

			switch (typeof(ComponentType).Name)
			{
				case nameof(Byte):
					Write = (writer, component) => writer.Write(Cast<byte>.From(component));
					Read  = reader => Cast<ComponentType>.From(reader.ReadByte());
					break;

				case nameof(Int16):
					Write = (writer, component) => writer.Write(Cast<short>.From(component));
					Read  = reader => Cast<ComponentType>.From(reader.ReadInt16());
					break;

				case nameof(Int32):
					Write = (writer, component) => writer.Write(Cast<int>.From(component));
					Read  = reader => Cast<ComponentType>.From(reader.ReadInt32());
					break;

				case nameof(Int64):
					Write = (writer, component) => writer.Write(Cast<long>.From(component));
					Read  = reader => Cast<ComponentType>.From(reader.ReadInt64());
					break;

				case nameof(Single):
					Write = (writer, component) => writer.Write(Cast<float>.From(component));
					Read  = reader => Cast<ComponentType>.From(reader.ReadSingle());
					break;

				case nameof(Double):
					Write = (writer, component) =>
							{
								double d = Cast<double>.From(component);
								writer.Write(d);
							};
					Read = reader => Cast<ComponentType>.From(reader.ReadDouble());
					break;

				case nameof(Decimal):
					Write = (writer, component) => writer.Write(Cast<decimal>.From(component));
					Read  = reader => Cast<ComponentType>.From(reader.ReadDecimal());
					break;
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets one of the components of this vector entry content.
		/// </summary>
		/// <param name="index">Zero-based index of the component to access.</param>
		/// <returns>Requested component.</returns>
		public ComponentType this[int index]
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
		public ComponentType X
		{
			get => this.components[0];
			set => this.components[0] = value;
		}

		/// <summary>
		/// Gets or sets the first component of the vector.
		/// </summary>
		public ComponentType R
		{
			get => this.components[0];
			set => this.components[0] = value;
		}

		/// <summary>
		/// Gets or sets the second component of the vector.
		/// </summary>
		public ComponentType Y
		{
			get => this.components[1];
			set => this.components[1] = value;
		}

		/// <summary>
		/// Gets or sets the second component of the vector.
		/// </summary>
		public ComponentType G
		{
			get => this.components[1];
			set => this.components[1] = value;
		}

		/// <summary>
		/// Gets or sets the third component of the vector.
		/// </summary>
		public ComponentType Z
		{
			get => this.components[2];
			set => this.components[2] = value;
		}

		/// <summary>
		/// Gets or sets the third component of the vector.
		/// </summary>
		public ComponentType B
		{
			get => this.components[2];
			set => this.components[2] = value;
		}

		/// <summary>
		/// Gets or sets the fourth component of the vector.
		/// </summary>
		public ComponentType W
		{
			get => this.components[3];
			set => this.components[3] = value;
		}

		/// <summary>
		/// Gets or sets the fourth component of the vector.
		/// </summary>
		public ComponentType A
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
			this.components = new ComponentType[count];
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
				Write(bw, this.components[i]);
			}
		}

		/// <summary>
		/// Reads a binary representation of this vector from a stream.
		/// </summary>
		/// <param name="br">An object that provides interface with the stream.</param>
		public override void FromBinary(BinaryReader br)
		{
			int count = br.ReadInt32();
			this.components = new ComponentType[count];
			for (int i = 0; i < count; i++)
			{
				this.components[i] = Read(br);
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
		/// <exception cref="Exception">Thrown when <typeparamref name="ComponentType"/> has no static method named Parse.</exception>
		public override void FromXml(XmlElement element)
		{
			var list = new List<ComponentType>(ComponentNames.Length);
			list.AddRange(from t in ComponentNames
						  where element.HasAttribute(t)
						  select (ComponentType)ParseMethod.Invoke(null, new object[] { element.GetAttribute(t) }));

			this.components = list.ToArray();
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<ComponentType> GetEnumerator()
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
		public bool Equals(VectorContent<ComponentType> other)
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
				return this.X.Equals(other.X) && this.Y.Equals(other.Y) &&
					   this.Z.Equals(other.Z) && this.W.Equals(other.W);
			}

			return false;
		}
	}
}