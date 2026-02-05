using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000112 RID: 274
	[TypeForwardedFrom("System.ServiceModel.Web, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
	public static class JsonReaderWriterFactory
	{
		// Token: 0x0600104C RID: 4172 RVA: 0x000429F0 File Offset: 0x00040BF0
		public static XmlDictionaryReader CreateJsonReader(Stream stream, XmlDictionaryReaderQuotas quotas)
		{
			return JsonReaderWriterFactory.CreateJsonReader(stream, null, quotas, null);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x000429FB File Offset: 0x00040BFB
		public static XmlDictionaryReader CreateJsonReader(byte[] buffer, XmlDictionaryReaderQuotas quotas)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			return JsonReaderWriterFactory.CreateJsonReader(buffer, 0, buffer.Length, null, quotas, null);
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00042A18 File Offset: 0x00040C18
		public static XmlDictionaryReader CreateJsonReader(Stream stream, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
		{
			XmlJsonReader xmlJsonReader = new XmlJsonReader();
			xmlJsonReader.SetInput(stream, encoding, quotas, onClose);
			return xmlJsonReader;
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00042A29 File Offset: 0x00040C29
		public static XmlDictionaryReader CreateJsonReader(byte[] buffer, int offset, int count, XmlDictionaryReaderQuotas quotas)
		{
			return JsonReaderWriterFactory.CreateJsonReader(buffer, offset, count, null, quotas, null);
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00042A36 File Offset: 0x00040C36
		public static XmlDictionaryReader CreateJsonReader(byte[] buffer, int offset, int count, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
		{
			XmlJsonReader xmlJsonReader = new XmlJsonReader();
			xmlJsonReader.SetInput(buffer, offset, count, encoding, quotas, onClose);
			return xmlJsonReader;
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00042A4B File Offset: 0x00040C4B
		public static XmlDictionaryWriter CreateJsonWriter(Stream stream)
		{
			return JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x00042A59 File Offset: 0x00040C59
		public static XmlDictionaryWriter CreateJsonWriter(Stream stream, Encoding encoding)
		{
			return JsonReaderWriterFactory.CreateJsonWriter(stream, encoding, true);
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x00042A63 File Offset: 0x00040C63
		public static XmlDictionaryWriter CreateJsonWriter(Stream stream, Encoding encoding, bool ownsStream)
		{
			return JsonReaderWriterFactory.CreateJsonWriter(stream, encoding, ownsStream, false);
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00042A6E File Offset: 0x00040C6E
		public static XmlDictionaryWriter CreateJsonWriter(Stream stream, Encoding encoding, bool ownsStream, bool indent)
		{
			return JsonReaderWriterFactory.CreateJsonWriter(stream, encoding, ownsStream, indent, "  ");
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00042A7E File Offset: 0x00040C7E
		public static XmlDictionaryWriter CreateJsonWriter(Stream stream, Encoding encoding, bool ownsStream, bool indent, string indentChars)
		{
			XmlJsonWriter xmlJsonWriter = new XmlJsonWriter(indent, indentChars);
			xmlJsonWriter.SetOutput(stream, encoding, ownsStream);
			return xmlJsonWriter;
		}

		// Token: 0x04000830 RID: 2096
		private const string DefaultIndentChars = "  ";
	}
}
