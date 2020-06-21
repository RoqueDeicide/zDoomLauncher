using System;
using System.IO;
using System.Text;

namespace Launcher
{
	/// <summary>
	/// Provides extension methods for classes <see cref="BinaryWriter"/> and <see cref="BinaryReader"/>
	/// </summary>
	public static class BinaryIoExtension
	{
		/// <summary>
		/// Writes a text to binary stream.
		/// </summary>
		/// <remarks>
		/// This method writes 64-bit signed integer that represents a number of bytes that represent the text. Text
		/// encoded in given encoding is written write after the number.
		/// </remarks>
		/// <param name="writer">  <see cref="BinaryWriter"/> object from which name this method is called.</param>
		/// <param name="s">       Text to write.</param>
		/// <param name="encoding">Encoding to use.</param>
		/// <returns>A number of written bytes.</returns>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public static long WriteLongString(this BinaryWriter writer, string s, Encoding encoding)
		{
			var bytesOfText = encoding.GetBytes(s);
			writer.Write(bytesOfText.LongLength);
			writer.Write(bytesOfText);
			return 8 + bytesOfText.LongLength;
		}

		/// <summary>
		/// Reads a long text from binary stream using specified encoding.
		/// </summary>
		/// <param name="reader">  <see cref="BinaryReader"/> object from which name this method is called.</param>
		/// <param name="s">       <see cref="string"/> object that will contain read text.</param>
		/// <param name="encoding">Encoding to use to parse bytes.</param>
		/// <returns>Number of read bytes.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public static long ReadLongString(this BinaryReader reader, out string s, Encoding encoding)
		{
			long numberOfBytes = reader.ReadInt64();
			var  bytesOfText   = new byte[numberOfBytes];
			for (long i = 0; i < numberOfBytes; i++)
			{
				bytesOfText[i] = reader.ReadByte();
			}

			s = encoding.GetString(bytesOfText);
			return 8 + numberOfBytes;
		}

		/// <summary>
		/// Reads a long text from binary stream using specified encoding.
		/// </summary>
		/// <param name="reader">  <see cref="BinaryReader"/> object from which name this method is called.</param>
		/// <param name="encoding">Encoding to use to parse bytes.</param>
		/// <returns>Read text.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public static string ReadLongString(this BinaryReader reader, Encoding encoding)
		{
			long numberOfBytes = reader.ReadInt64();
			var  bytesOfText   = new byte[numberOfBytes];
			for (long i = 0; i < numberOfBytes; i++)
			{
				bytesOfText[i] = reader.ReadByte();
			}

			return encoding.GetString(bytesOfText);
		}
	}
}