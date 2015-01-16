using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Launcher.Extensions;

namespace Launcher.Databases
{
	/// <summary>
	/// This class defines global constants related to databases.
	/// </summary>
	public static class DatabaseGlobals
	{
		/// <summary>
		/// <see cref="Int32" /> instance that indicates beginning of
		/// group of subentries in binary representation of database entry.
		/// </summary>
		public static readonly int SubEntriesStartMarker = 132383;
		/// <summary>
		/// <see cref="Int32" /> instance that indicates end of group
		/// of subentries in binary representation of database entry.
		/// </summary>
		public static readonly int SubEntriesEndMarker = 172383;
		/// <summary>
		/// <see cref="Int32" /> instance that indicates beginning of
		/// database entry in binary representation of database entry.
		/// </summary>
		public static readonly int EntryStartMarker = 1731;
		/// <summary>
		/// <see cref="Int32" /> instance that indicates the end of
		/// database entry in binary representation of it.
		/// </summary>
		public static readonly int EntryEndMarker = 1731;
		/// <summary>
		/// <see cref="Int32" /> instance that indicates that database
		/// entry has a content.
		/// </summary>
		public static readonly int EntryContentPresentMarker = 00001;
		/// <summary>
		/// <see cref="Int32" /> instance that indicates that database
		/// entry has no content.
		/// </summary>
		public static readonly int EntryContentAbsentMarker = 11001;
		/// <summary>
		/// <see cref="Int32" /> instance that indicates that current
		/// file contains database in binary format.
		/// </summary>
		public static readonly int BinaryDatabaseFileIdentifier = 4323;
		/// <summary>
		/// <see cref="Int32" /> instance that indicates the end of
		/// binary file that stores database.
		/// </summary>
		public static readonly int BinaryDatabaseFileEndMarker = 2959;
		/// <summary>
		/// Gets the list of attributes that designate registered
		/// classes derived from <see cref="DatabaseEntryContent" />.
		/// </summary>
		public static List<EntryContentAttribute> RegisteredContentTypes;
		static DatabaseGlobals()
		{
			DatabaseGlobals.RegisteredContentTypes = new List<EntryContentAttribute>
			(
				AppDomain
					.CurrentDomain
					.GetAssemblies()
					.SelectMany(assembly=>assembly.GetTypes())
					.SelectMany(type => type.GetCustomAttributes<EntryContentAttribute>())
			);
		}
	}
	/// <summary>
	/// Represents a tree-structure database.
	/// </summary>
	public class Database : IEnumerable<string>, IEnumerable<DatabaseEntry>
	{
		#region Fields
		private readonly SortedList<string, DatabaseEntry> TopLevelEntries;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the name of this database.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Gives access to <see cref="DatabaseEntry" /> with
		/// specified name.
		/// </summary>
		/// <param name="name">
		/// The name of the entry which needs to be accessed.
		/// </param>
		public DatabaseEntry this[string name]
		{
			get
			{
				return this.TopLevelEntries[name];
			}
			set
			{
				this.TopLevelEntries[name] = value;
			}
		}
		/// <summary>
		/// Gets the extension that marks files that store database
		/// entries in binary format.
		/// </summary>
		public string BinaryFileExtension { get; private set; }
		/// <summary>
		/// Gets the extension that marks files that store database
		/// entries in Xml format.
		/// </summary>
		public string XmlFileExtension { get; private set; }
		/// <summary>
		/// Gets amount of entries in this database.
		/// </summary>
		public int Count
		{
			get { return this.TopLevelEntries.Count; }
		}
		#endregion
		#region Events
		#endregion
		#region Interface
		#region Constructors
		/// <summary>
		/// Initializes default database.
		/// </summary>
		public Database()
		{
			this.TopLevelEntries = new SortedList<string, DatabaseEntry>();
			this.BinaryFileExtension = "bdf";
			this.XmlFileExtension = "xml";
			this.Name = "";
		}
		/// <summary>
		/// Initializes default database.
		/// </summary>
		public Database(string xmlExt, string binExt)
		{
			Contract.Requires
			(
				!String.IsNullOrWhiteSpace(xmlExt)
				&&
				xmlExt.All(Char.IsLetterOrDigit),
				"Invalid extension for XML files: extension must not be null or empty and must" +
				" only contain letters and/or digits."
			);
			Contract.Requires
			(
				!String.IsNullOrWhiteSpace(xmlExt)
				&&
				xmlExt.All(Char.IsLetterOrDigit),
				"Invalid extension for binary files: extension must not be null or empty and " +
				"must only contain letters and/or digits."
			);
			Contract.Requires
			(
				xmlExt != binExt,
				"Extensions for XML and binary files must be different."
			);

			this.TopLevelEntries = new SortedList<string, DatabaseEntry>();
			this.BinaryFileExtension = binExt;
			this.XmlFileExtension = xmlExt;
			this.Name = "";
		}
		#endregion
		#region Entries Management
		/// <summary>
		/// Adds entry to the database.
		/// </summary>
		/// <param name="entry">New entry to add.</param>
		public void AddEntry(DatabaseEntry entry)
		{
			this.TopLevelEntries.Add(entry.Name, entry);
		}
		/// <summary>
		/// Removes entry from the database.
		/// </summary>
		/// <param name="name">Name of the entry to remove.</param>
		public void RemoveEntry(string name)
		{
			if (this.TopLevelEntries.ContainsKey(name))
			{
				this.TopLevelEntries.Remove(name);
			}
		}
		/// <summary>
		/// Wipes the database.
		/// </summary>
		public void Clear()
		{
			this.TopLevelEntries.Clear();
		}
		/// <summary>
		/// Determines whether this database contains an entry with
		/// specified name.
		/// </summary>
		/// <param name="name">
		/// Name of the entry which presence in this database needs to
		/// be determined.
		/// </param>
		/// <param name="searchLowerLevels">
		/// Indicates whether to search in subentries.
		/// </param>
		/// <returns>
		/// True, if this database contains an entry with specified
		/// name, otherwise returns false.
		/// </returns>
		public bool Contains(string name, bool searchLowerLevels)
		{
			return
				this.TopLevelEntries.ContainsKey(name)
				||
				(
					searchLowerLevels
					&&
					this.TopLevelEntries.Keys.Any
					(
						subEntryName => this.TopLevelEntries[subEntryName].Contains(name)
					)
				);
		}
		#endregion
		#region Loading/Saving
		/// <summary>
		/// Saves database to the file.
		/// </summary>
		/// <param name="file">
		/// File that will contain the database.
		/// </param>
		public void Save(string file)
		{
			if (!File.Exists(file))
			{
				File.Create(file).Close();
			}
			//
			string rawExtension = System.IO.Path.GetExtension(file);
			string extension = rawExtension != null ? rawExtension.Substring(1) : null;
			//
			if (extension == this.BinaryFileExtension)
			{
				this.SaveBinary(file);
			}
			else if (extension == this.XmlFileExtension)
			{
				this.SaveXml(file);
			}
			else
			{
				throw new ArgumentException(String.Format("Database.Save: Unable to recognize file extension. Use [{0}] for Xml files and [{1}] for binaries. File that has been attempted to be saved: [{2}]. Extension is [{3}]", this.XmlFileExtension, this.BinaryFileExtension, file, extension));
			}
		}
		/// <summary>
		/// Loads database from the file.
		/// </summary>
		/// <param name="file">File that contains the database.</param>
		public void Load(string file)
		{
			if (!File.Exists(file))
			{
				throw new FileNotFoundException("Database.Load: File does not exist.");
			}
			//
			string rawExtension = System.IO.Path.GetExtension(file);
			string extension = rawExtension != null ? rawExtension.Substring(1) : null;
			//
			if (extension == this.BinaryFileExtension)
			{
				this.LoadBinary(file);
			}
			else if (extension == this.XmlFileExtension)
			{
				this.LoadXml(file);
			}
			else
			{
				throw new ArgumentException(String.Format("Database.Load: Unable to recognize file extension. Use [{0}] for Xml files and [{1}] for binaries. File that has been attempted to be loaded: [{2}]. Recognized extension is [{3}]", this.XmlFileExtension, this.BinaryFileExtension, file, extension));
			}
		}
		#endregion
		#region Enumeration
		/// <summary>
		/// Gets object that allows to enumerate through list of names
		/// of the entries.
		/// </summary>
		/// <returns>
		/// Object that allows to enumerate through list of names of
		/// the entries.
		/// </returns>
		public IEnumerator<string> GetEnumerator()
		{
			return this.TopLevelEntries.Keys.GetEnumerator();
		}
		/// <summary>
		/// Gets object that allows to enumerate through list of names
		/// of the entries.
		/// </summary>
		/// <returns>
		/// Object that allows to enumerate through list of names of
		/// the entries.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.TopLevelEntries.Keys.GetEnumerator();
		}
		/// <summary>
		/// Enumerates through all database entries.
		/// </summary>
		/// <returns></returns>
		IEnumerator<DatabaseEntry> IEnumerable<DatabaseEntry>.GetEnumerator()
		{
			return this.TopLevelEntries.Values.GetEnumerator();
		}
		#endregion
		#endregion
		#region Utilities
		private void SaveBinary(string file)
		{
			FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None);
			using (BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8))
			{
				bw.Write(DatabaseGlobals.BinaryDatabaseFileIdentifier);
				for (int i = 0; i < this.TopLevelEntries.Keys.Count; i++)
				{
					bw.Write(DatabaseGlobals.EntryStartMarker);
					this.TopLevelEntries[this.TopLevelEntries.Keys[i]].ToBinary(bw);
					bw.Write(DatabaseGlobals.EntryEndMarker);
				}
			}
		}
		private void LoadBinary(string file)
		{
			FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
			using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8))
			{
				if (br.ReadInt32() != DatabaseGlobals.BinaryDatabaseFileIdentifier)
				{
					throw new ArgumentException(String.Format("Database.LoadBinary: Unable to recognize format of the file. File: {0}", file));
				}
				while (br.ReadInt32() != DatabaseGlobals.BinaryDatabaseFileEndMarker)
				{
					DatabaseEntry currentEntry = new DatabaseEntry();
					currentEntry.FromBinary(br);
					this.TopLevelEntries.Add(currentEntry.Name, currentEntry);
				}
			}
		}
		private void SaveXml(string file)
		{
			XmlDocument document = new XmlDocument();
			XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", null, null);
			document.AppendChild(declaration);
			XmlElement root = document.CreateElement("Data");
			for (int i = 0; i < this.TopLevelEntries.Keys.Count; i++)
			{
				XmlElement currentEntryElement = document.CreateElement(this.TopLevelEntries.Keys[i]);
				this.TopLevelEntries[this.TopLevelEntries.Keys[i]].ToXml(document, currentEntryElement);
				root.AppendChild(currentEntryElement);
			}
			document.AppendChild(root);
			FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None);
			document.Save(fs);
			fs.Flush();
			if (fs.CanWrite)
			{
				fs.Close();
			}
		}
		private void LoadXml(string file)
		{
			XmlDocument document = new XmlDocument();
			FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				document.Load(fs);
				foreach (XmlElement entryElement in document.ChildNodes[1].ChildNodes)
				{
					DatabaseEntry currentEntry = new DatabaseEntry();
					currentEntry.FromXml(entryElement);
					this.TopLevelEntries.Add(currentEntry.Name, currentEntry);
				}
			}
			finally
			{
				fs.Close();
			}
		}
		#endregion
	}
	/// <summary>
	/// Represents a database entry.
	/// </summary>
	public class DatabaseEntry
	{
		/// <summary>
		/// Gets or sets the name of the entry.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Gets or sets content of this entry.
		/// </summary>
		private DatabaseEntryContent Content { get; set; }
		/// <summary>
		/// Gets sorted list of subentries.
		/// </summary>
		public SortedList<string, DatabaseEntry> SubEntries { get; private set; }
		/// <summary>
		/// Provides read/write access to the subentry with specified name.
		/// </summary>
		/// <param name="name">Name of the subentry to get.</param>
		/// <returns></returns>
		public DatabaseEntry this[string name]
		{
			get { return this.SubEntries[name]; }
			set { this.SubEntries[name] = value; }
		}
		/// <summary>
		/// Creates default instance of <see cref="DatabaseEntry" /> class.
		/// </summary>
		public DatabaseEntry()
		{
			this.Name = null;
			this.Content = null;
			this.SubEntries = new SortedList<string, DatabaseEntry>();
		}
		/// <summary>
		/// Creates new instance of <see cref="DatabaseEntry" /> class.
		/// </summary>
		/// <param name="name">The name of the entry.</param>
		/// <param name="content">Content of the entry.</param>
		public DatabaseEntry(string name, DatabaseEntryContent content)
		{
			this.Name = name;
			this.Content = content;
			this.SubEntries = new SortedList<string, DatabaseEntry>();
		}
		/// <summary>
		/// Gets content of this entry.
		/// </summary>
		/// <typeparam name="T">Type of the content.</typeparam>
		/// <returns>
		/// Content of this entry if <typeparamref name="T" /> matches
		/// the type of content of this entry. Otherwise returns
		/// default value of <typeparamref name="T" />.
		/// </returns>
		public T GetContent<T>() where T : DatabaseEntryContent
		{
			return this.Content as T;
		}
		/// <summary>
		/// Changes value of content of this entry.
		/// </summary>
		/// <typeparam name="T">Type of this entry's content.</typeparam>
		/// <param name="newValue">New value for content.</param>
		public void SetContent<T>(T newValue) where T : DatabaseEntryContent
		{
			this.Content = newValue;
		}
		/// <summary>
		/// Determines whether this entry contains a subentry with
		/// specified name.
		/// </summary>
		/// <param name="name">
		/// Name of the subentry which presence in this entry needs to
		/// be determined.
		/// </param>
		/// <returns>
		/// True, if this entry contains a subentry with specified
		/// name, otherwise returns false.
		/// </returns>
		public bool Contains(string name)
		{
			return
				this.SubEntries.ContainsKey(name)
				||
				this.SubEntries.Keys
					.Any(subEntryName => this.SubEntries[subEntryName].Contains(name));
		}
		/// <summary>
		/// Creates binary representation of this entry and its subentries.
		/// </summary>
		/// <param name="bw">
		/// <see cref="BinaryWriter" /> object that provides access to
		/// binary stream to which binary representation of this entry
		/// is written to.
		/// </param>
		public void ToBinary(BinaryWriter bw)
		{
			bw.WriteLongString(this.Name, Encoding.UTF8);
			//
			if (this.Content != null)
			{
				bw.Write(DatabaseGlobals.EntryContentPresentMarker);
				bw.Write(DatabaseGlobals.RegisteredContentTypes.Find(x => x.AttributedClass == this.Content.GetType()).TypeHash);
				this.Content.ToBinary(bw);
			}
			else
			{
				bw.Write(DatabaseGlobals.EntryContentAbsentMarker);
			}
			//
			if (this.SubEntries.Count != 0)
			{
				bw.Write(DatabaseGlobals.SubEntriesStartMarker);
				for (int i = 0; i < this.SubEntries.Keys.Count; i++)
				{
					bw.Write(DatabaseGlobals.EntryStartMarker);
					this.SubEntries[this.SubEntries.Keys[i]].ToBinary(bw);
				}
				bw.Write(DatabaseGlobals.SubEntriesEndMarker);
			}
		}
		/// <summary>
		/// Parses given binary data into database entry.
		/// </summary>
		/// <param name="br">
		/// <see cref="BinaryReader" /> object that provides access to
		/// binary stream which contains binary representation of
		/// database entry.
		/// </param>
		public void FromBinary(BinaryReader br)
		{
			this.Name = br.ReadLongString(Encoding.UTF8);
			// Read the content. Use attributes to identify
			// appropriate type.
			int contentMarker = br.ReadInt32();
			if (contentMarker == DatabaseGlobals.EntryContentPresentMarker)
			{
				EntryContentAttribute contentType =
					DatabaseGlobals.RegisteredContentTypes.Find(x => x.TypeHash == br.ReadInt32());
				if (contentType != null)
				{
					ConstructorInfo constructor =
						contentType.AttributedClass.GetConstructor(Type.EmptyTypes);
					if (constructor != null)
					{
						this.Content = (DatabaseEntryContent)constructor.Invoke(null);
						this.Content.FromBinary(br);
					}
				}
			}
			// Read subentries.
			while (br.ReadInt32() != DatabaseGlobals.EntryEndMarker)
			{
				DatabaseEntry currentSubEntry = new DatabaseEntry();
				currentSubEntry.FromBinary(br);
				this.SubEntries.Add(currentSubEntry.Name, currentSubEntry);
			}
		}
		/// <summary>
		/// Records Xml representation of this entry to given <see
		/// cref="XmlElement" /> object.
		/// </summary>
		/// <param name="document">
		/// <see cref="XmlDocument" /> object that will store Xml
		/// representation of this entry.
		/// </param>
		/// <param name="hostElement">
		/// <see cref="XmlElement" /> object that will store Xml
		/// representation of this entry.
		/// </param>
		public void ToXml(XmlDocument document, XmlElement hostElement)
		{
			if (this.Content != null)
			{
				hostElement.SetAttribute("hasContent", "1");
				Type contentTypeObject = this.Content.GetType();
				EntryContentAttribute attr = DatabaseGlobals.RegisteredContentTypes.Find(x => x.AttributedClass == contentTypeObject);
				XmlElement contentElement = document.CreateElement(attr.TypeName);
				this.Content.ToXml(document, contentElement);
				hostElement.AppendChild(contentElement);
			}
			else
			{
				hostElement.SetAttribute("hasContent", "0");
			}
			if (this.SubEntries.Count != 0)
			{
				for (int i = 0; i < this.SubEntries.Keys.Count; i++)
				{
					XmlElement currentSubEntryElement = document.CreateElement(this.SubEntries.Keys[i]);
					this.SubEntries[this.SubEntries.Keys[i]].ToXml(document, currentSubEntryElement);
					hostElement.AppendChild(currentSubEntryElement);
				}
			}
		}
		/// <summary>
		/// Parses given Xml data as representation of the <see
		/// cref="DatabaseEntry" /> instance.
		/// </summary>
		/// <param name="element">
		/// <see cref="XmlElement" /> object that provides access to
		/// Xml representation of this <see cref="DatabaseEntry" /> object.
		/// </param>
		public void FromXml(XmlElement element)
		{
			this.Name = element.Name;
			int minimalAmountOfChildNodes = 0;
			List<XmlElement> children = new List<XmlElement>(element.ChildNodes.Count);
			for (int i = 0; i < element.ChildNodes.Count; i++)
			{
				if (element.ChildNodes[i] is XmlElement)
				{
					children.Add(element.ChildNodes[i] as XmlElement);
				}
			}
			if (element.GetAttribute("hasContent") == "1")
			{
				XmlElement contentElement = children[0];
				EntryContentAttribute contentType =
					DatabaseGlobals.RegisteredContentTypes.Find(x => x.TypeName == contentElement.Name);
				if (contentType != null)
				{
					ConstructorInfo constructor =
						contentType.AttributedClass.GetConstructor(Type.EmptyTypes);
					if (constructor != null)
					{
						this.Content = (DatabaseEntryContent)constructor.Invoke(null);
						this.Content.FromXml(contentElement);
						minimalAmountOfChildNodes++;
					}
				}
			}
			if (element.ChildNodes.Count > minimalAmountOfChildNodes)
			{
				for (int i = 0; i < children.Count; i++)
				{
					DatabaseEntry currentEntry = new DatabaseEntry();
					currentEntry.FromXml(children[i]);
					this.SubEntries.Add(currentEntry.Name, currentEntry);
				}
			}
		}
	}
	/// <summary>
	/// Base class for objects that represent contents of a database entry.
	/// </summary>
	public abstract class DatabaseEntryContent
	{
		/// <summary>
		/// Writes binary representation of this content into given stream.
		/// </summary>
		/// <param name="bw">
		/// <see cref="BinaryWriter" /> object that provides access to
		/// binary stream
		/// </param>
		public abstract void ToBinary(BinaryWriter bw);
		/// <summary>
		/// Parses given binary data as representation of the content.
		/// </summary>
		/// <param name="br">
		/// <see cref="BinaryReader" /> object that provides access to
		/// binary stream.
		/// </param>
		public abstract void FromBinary(BinaryReader br);
		/// <summary>
		/// Records Xml representation of this entry content to given
		/// <see cref="XmlElement" /> object.
		/// </summary>
		/// <param name="document">
		/// <see cref="XmlDocument" /> object that will store Xml
		/// representation of this entry content.
		/// </param>
		/// <param name="element">
		/// <see cref="XmlElement" /> object that will store Xml
		/// representation of this entry content.
		/// </param>
		public abstract void ToXml(XmlDocument document, XmlElement element);
		/// <summary>
		/// Parses given Xml data as representation of the <see
		/// cref="DatabaseEntryContent" /> instance.
		/// </summary>
		/// <param name="element">
		/// <see cref="XmlElement" /> object that provides access to
		/// Xml representation of this <see
		/// cref="DatabaseEntryContent" /> object.
		/// </param>
		public abstract void FromXml(XmlElement element);
	}
	/// <summary>
	/// Represents an attribute that is applied to classes derived
	/// from <see cref="DatabaseEntryContent" />.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class EntryContentAttribute : Attribute
	{
		/// <summary>
		/// Gets identifier of the class.
		/// </summary>
		public int TypeHash { get; private set; }
		/// <summary>
		/// Gets the name of the type of content.
		/// </summary>
		public string TypeName { get; private set; }
		/// <summary>
		/// Gets class to which this attribute is applied.
		/// </summary>
		public Type AttributedClass { get; private set; }
		/// <summary>
		/// Creates new instance of <see cref="EntryContentAttribute"
		/// /> class.
		/// </summary>
		/// <param name="name">The name of the type of content.</param>
		/// <param name="type">
		/// Class to which this attribute is applied.
		/// </param>
		public EntryContentAttribute(string name, Type type)
		{
			this.TypeName = name;
			this.AttributedClass = type;
			this.TypeHash = name.GetHashCode();
			EntryContentAttribute other = DatabaseGlobals.RegisteredContentTypes.Find(x => x.TypeHash == this.TypeHash);
			if (other != null)
			{
				throw new Exception(String.Format("Unable to register database entry content type with name that has the same hash value as one of the existing ones. New name = {0}, old name = {1}", this.TypeName, other.TypeName));
			}
		}
	}
}