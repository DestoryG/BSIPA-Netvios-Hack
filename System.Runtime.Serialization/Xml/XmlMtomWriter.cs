using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x0200004B RID: 75
	internal class XmlMtomWriter : XmlDictionaryWriter, IXmlMtomWriterInitializer
	{
		// Token: 0x06000552 RID: 1362 RVA: 0x00019D54 File Offset: 0x00017F54
		public void SetOutput(Stream stream, Encoding encoding, int maxSizeInBytes, string startInfo, string boundary, string startUri, bool writeMessageHeaders, bool ownsStream)
		{
			if (encoding == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
			}
			if (maxSizeInBytes < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("maxSizeInBytes", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			this.maxSizeInBytes = maxSizeInBytes;
			this.encoding = encoding;
			this.isUTF8 = XmlMtomWriter.IsUTF8Encoding(encoding);
			this.Initialize(stream, startInfo, boundary, startUri, writeMessageHeaders, ownsStream);
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x00019DB8 File Offset: 0x00017FB8
		private XmlDictionaryWriter Writer
		{
			get
			{
				if (!this.IsInitialized)
				{
					this.Initialize();
				}
				return this.writer;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x00019DCE File Offset: 0x00017FCE
		private bool IsInitialized
		{
			get
			{
				return this.initialContentTypeForRootPart == null;
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00019DDC File Offset: 0x00017FDC
		private void Initialize(Stream stream, string startInfo, string boundary, string startUri, bool writeMessageHeaders, bool ownsStream)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
			}
			if (startInfo == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("startInfo");
			}
			if (boundary == null)
			{
				boundary = XmlMtomWriter.GetBoundaryString();
			}
			if (startUri == null)
			{
				startUri = XmlMtomWriter.GenerateUriForMimePart(0);
			}
			if (!MailBnfHelper.IsValidMimeBoundary(boundary))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("MIME boundary is invalid: '{0}'.", new object[] { boundary }), "boundary"));
			}
			this.ownsStream = ownsStream;
			this.isClosed = false;
			this.depth = 0;
			this.totalSizeOfMimeParts = 0;
			this.sizeOfBufferedBinaryData = 0;
			this.binaryDataChunks = null;
			this.contentType = null;
			this.contentTypeStream = null;
			this.contentID = startUri;
			if (this.mimeParts != null)
			{
				this.mimeParts.Clear();
			}
			this.mimeWriter = new MimeWriter(stream, boundary);
			this.initialContentTypeForRootPart = XmlMtomWriter.GetContentTypeForRootMimePart(this.encoding, startInfo);
			if (writeMessageHeaders)
			{
				this.initialContentTypeForMimeMessage = XmlMtomWriter.GetContentTypeForMimeMessage(boundary, startUri, startInfo);
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00019ED0 File Offset: 0x000180D0
		private void Initialize()
		{
			if (this.isClosed)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("The XmlWriter is closed.")));
			}
			if (this.initialContentTypeForRootPart != null)
			{
				if (this.initialContentTypeForMimeMessage != null)
				{
					this.mimeWriter.StartPreface();
					this.mimeWriter.WriteHeader(MimeGlobals.MimeVersionHeader, MimeGlobals.DefaultVersion);
					this.mimeWriter.WriteHeader(MimeGlobals.ContentTypeHeader, this.initialContentTypeForMimeMessage);
					this.initialContentTypeForMimeMessage = null;
				}
				this.WriteMimeHeaders(this.contentID, this.initialContentTypeForRootPart, this.isUTF8 ? MimeGlobals.Encoding8bit : MimeGlobals.EncodingBinary);
				Stream contentStream = this.mimeWriter.GetContentStream();
				IXmlTextWriterInitializer xmlTextWriterInitializer = this.writer as IXmlTextWriterInitializer;
				if (xmlTextWriterInitializer == null)
				{
					this.writer = XmlDictionaryWriter.CreateTextWriter(contentStream, this.encoding, this.ownsStream);
				}
				else
				{
					xmlTextWriterInitializer.SetOutput(contentStream, this.encoding, this.ownsStream);
				}
				this.contentID = null;
				this.initialContentTypeForRootPart = null;
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00019FC6 File Offset: 0x000181C6
		private static string GetBoundaryString()
		{
			return XmlMtomWriter.MimeBoundaryGenerator.Next();
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00019FCD File Offset: 0x000181CD
		internal static bool IsUTF8Encoding(Encoding encoding)
		{
			return encoding.WebName == "utf-8";
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00019FE0 File Offset: 0x000181E0
		private static string GetContentTypeForMimeMessage(string boundary, string startUri, string startInfo)
		{
			StringBuilder stringBuilder = new StringBuilder(string.Format(CultureInfo.InvariantCulture, "{0}/{1};{2}=\"{3}\";{4}=\"{5}\"", new object[]
			{
				MtomGlobals.MediaType,
				MtomGlobals.MediaSubtype,
				MtomGlobals.TypeParam,
				MtomGlobals.XopType,
				MtomGlobals.BoundaryParam,
				boundary
			}));
			if (startUri != null && startUri.Length > 0)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, ";{0}=\"<{1}>\"", MtomGlobals.StartParam, startUri);
			}
			if (startInfo != null && startInfo.Length > 0)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, ";{0}=\"{1}\"", MtomGlobals.StartInfoParam, startInfo);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001A080 File Offset: 0x00018280
		private static string GetContentTypeForRootMimePart(Encoding encoding, string startInfo)
		{
			string text = string.Format(CultureInfo.InvariantCulture, "{0};{1}={2}", MtomGlobals.XopType, MtomGlobals.CharsetParam, XmlMtomWriter.CharSet(encoding));
			if (startInfo != null)
			{
				text = string.Format(CultureInfo.InvariantCulture, "{0};{1}=\"{2}\"", text, MtomGlobals.TypeParam, startInfo);
			}
			return text;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001A0C8 File Offset: 0x000182C8
		private static string CharSet(Encoding enc)
		{
			string webName = enc.WebName;
			if (string.Compare(webName, Encoding.UTF8.WebName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return webName;
			}
			if (string.Compare(webName, Encoding.Unicode.WebName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "utf-16LE";
			}
			if (string.Compare(webName, Encoding.BigEndianUnicode.WebName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "utf-16BE";
			}
			return webName;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001A124 File Offset: 0x00018324
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.WriteBase64InlineIfPresent();
			this.ThrowIfElementIsXOPInclude(prefix, localName, ns);
			this.Writer.WriteStartElement(prefix, localName, ns);
			this.depth++;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001A154 File Offset: 0x00018354
		public override void WriteStartElement(string prefix, XmlDictionaryString localName, XmlDictionaryString ns)
		{
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
			}
			this.WriteBase64InlineIfPresent();
			this.ThrowIfElementIsXOPInclude(prefix, localName.Value, (ns == null) ? null : ns.Value);
			this.Writer.WriteStartElement(prefix, localName, ns);
			this.depth++;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001A1AC File Offset: 0x000183AC
		private void ThrowIfElementIsXOPInclude(string prefix, string localName, string ns)
		{
			if (ns == null)
			{
				XmlBaseWriter xmlBaseWriter = this.Writer as XmlBaseWriter;
				if (xmlBaseWriter != null)
				{
					ns = xmlBaseWriter.LookupNamespace(prefix);
				}
			}
			if (localName == MtomGlobals.XopIncludeLocalName && ns == MtomGlobals.XopIncludeNamespace)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM data must not contain xop:Include element. '{0}' element in '{1}' namespace.", new object[]
				{
					MtomGlobals.XopIncludeLocalName,
					MtomGlobals.XopIncludeNamespace
				})));
			}
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001A219 File Offset: 0x00018419
		public override void WriteEndElement()
		{
			this.WriteXOPInclude();
			this.Writer.WriteEndElement();
			this.depth--;
			this.WriteXOPBinaryParts();
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001A240 File Offset: 0x00018440
		public override void WriteFullEndElement()
		{
			this.WriteXOPInclude();
			this.Writer.WriteFullEndElement();
			this.depth--;
			this.WriteXOPBinaryParts();
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001A268 File Offset: 0x00018468
		public override void WriteValue(IStreamProvider value)
		{
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
			}
			if (this.Writer.WriteState == WriteState.Element)
			{
				if (this.binaryDataChunks == null)
				{
					this.binaryDataChunks = new List<MtomBinaryData>();
					this.contentID = XmlMtomWriter.GenerateUriForMimePart((this.mimeParts == null) ? 1 : (this.mimeParts.Count + 1));
				}
				this.binaryDataChunks.Add(new MtomBinaryData(value));
				return;
			}
			this.Writer.WriteValue(value);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001A2EC File Offset: 0x000184EC
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			if (this.Writer.WriteState != WriteState.Element)
			{
				this.Writer.WriteBase64(buffer, index, count);
				return;
			}
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
			}
			if (index < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - index)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { buffer.Length - index })));
			}
			if (this.binaryDataChunks == null)
			{
				this.binaryDataChunks = new List<MtomBinaryData>();
				this.contentID = XmlMtomWriter.GenerateUriForMimePart((this.mimeParts == null) ? 1 : (this.mimeParts.Count + 1));
			}
			int num = XmlMtomWriter.ValidateSizeOfMessage(this.maxSizeInBytes, 0, this.totalSizeOfMimeParts);
			num += XmlMtomWriter.ValidateSizeOfMessage(this.maxSizeInBytes, num, this.sizeOfBufferedBinaryData);
			num += XmlMtomWriter.ValidateSizeOfMessage(this.maxSizeInBytes, num, count);
			this.sizeOfBufferedBinaryData += count;
			this.binaryDataChunks.Add(new MtomBinaryData(buffer, index, count));
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001A42C File Offset: 0x0001862C
		internal static int ValidateSizeOfMessage(int maxSize, int offset, int size)
		{
			if (size > maxSize - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM exceeded max size in bytes. The maximum size is {0}.", new object[] { maxSize })));
			}
			return size;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001A459 File Offset: 0x00018659
		private void WriteBase64InlineIfPresent()
		{
			if (this.binaryDataChunks != null)
			{
				this.WriteBase64Inline();
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001A46C File Offset: 0x0001866C
		private void WriteBase64Inline()
		{
			foreach (MtomBinaryData mtomBinaryData in this.binaryDataChunks)
			{
				if (mtomBinaryData.type == MtomBinaryDataType.Provider)
				{
					this.Writer.WriteValue(mtomBinaryData.provider);
				}
				else
				{
					this.Writer.WriteBase64(mtomBinaryData.chunk, 0, mtomBinaryData.chunk.Length);
				}
			}
			this.sizeOfBufferedBinaryData = 0;
			this.binaryDataChunks = null;
			this.contentType = null;
			this.contentID = null;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001A504 File Offset: 0x00018704
		private void WriteXOPInclude()
		{
			if (this.binaryDataChunks == null)
			{
				return;
			}
			bool flag = true;
			long num = 0L;
			foreach (MtomBinaryData mtomBinaryData in this.binaryDataChunks)
			{
				long length = mtomBinaryData.Length;
				if (length < 0L || length > 767L - num)
				{
					flag = false;
					break;
				}
				num += length;
			}
			if (flag)
			{
				this.WriteBase64Inline();
				return;
			}
			if (this.mimeParts == null)
			{
				this.mimeParts = new List<XmlMtomWriter.MimePart>();
			}
			XmlMtomWriter.MimePart mimePart = new XmlMtomWriter.MimePart(this.binaryDataChunks, this.contentID, this.contentType, MimeGlobals.EncodingBinary, this.sizeOfBufferedBinaryData, this.maxSizeInBytes);
			this.mimeParts.Add(mimePart);
			this.totalSizeOfMimeParts += XmlMtomWriter.ValidateSizeOfMessage(this.maxSizeInBytes, this.totalSizeOfMimeParts, mimePart.sizeInBytes);
			this.totalSizeOfMimeParts += XmlMtomWriter.ValidateSizeOfMessage(this.maxSizeInBytes, this.totalSizeOfMimeParts, this.mimeWriter.GetBoundarySize());
			this.Writer.WriteStartElement(MtomGlobals.XopIncludePrefix, MtomGlobals.XopIncludeLocalName, MtomGlobals.XopIncludeNamespace);
			this.Writer.WriteStartAttribute(MtomGlobals.XopIncludeHrefLocalName, MtomGlobals.XopIncludeHrefNamespace);
			this.Writer.WriteValue(string.Format(CultureInfo.InvariantCulture, "{0}{1}", MimeGlobals.ContentIDScheme, this.contentID));
			this.Writer.WriteEndAttribute();
			this.Writer.WriteEndElement();
			this.binaryDataChunks = null;
			this.sizeOfBufferedBinaryData = 0;
			this.contentType = null;
			this.contentID = null;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001A6A0 File Offset: 0x000188A0
		public static string GenerateUriForMimePart(int index)
		{
			return string.Format(CultureInfo.InvariantCulture, "http://tempuri.org/{0}/{1}", index, DateTime.Now.Ticks);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001A6D4 File Offset: 0x000188D4
		private void WriteXOPBinaryParts()
		{
			if (this.depth > 0 || this.mimeWriter.WriteState == MimeWriterState.Closed)
			{
				return;
			}
			if (this.Writer.WriteState != WriteState.Closed)
			{
				this.Writer.Flush();
			}
			if (this.mimeParts != null)
			{
				foreach (XmlMtomWriter.MimePart mimePart in this.mimeParts)
				{
					this.WriteMimeHeaders(mimePart.contentID, mimePart.contentType, mimePart.contentTransferEncoding);
					Stream contentStream = this.mimeWriter.GetContentStream();
					int num = 256;
					byte[] array = new byte[num];
					foreach (MtomBinaryData mtomBinaryData in mimePart.binaryData)
					{
						if (mtomBinaryData.type == MtomBinaryDataType.Provider)
						{
							Stream stream = mtomBinaryData.provider.GetStream();
							if (stream == null)
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Stream returned by IStreamProvider cannot be null.")));
							}
							for (;;)
							{
								int num2 = stream.Read(array, 0, num);
								if (num2 <= 0)
								{
									break;
								}
								contentStream.Write(array, 0, num2);
								if (num < 65536 && num2 == num)
								{
									num *= 16;
									array = new byte[num];
								}
							}
							mtomBinaryData.provider.ReleaseStream(stream);
						}
						else
						{
							contentStream.Write(mtomBinaryData.chunk, 0, mtomBinaryData.chunk.Length);
						}
					}
				}
				this.mimeParts.Clear();
			}
			this.mimeWriter.Close();
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001A89C File Offset: 0x00018A9C
		private void WriteMimeHeaders(string contentID, string contentType, string contentTransferEncoding)
		{
			this.mimeWriter.StartPart();
			if (contentID != null)
			{
				this.mimeWriter.WriteHeader(MimeGlobals.ContentIDHeader, string.Format(CultureInfo.InvariantCulture, "<{0}>", contentID));
			}
			if (contentTransferEncoding != null)
			{
				this.mimeWriter.WriteHeader(MimeGlobals.ContentTransferEncodingHeader, contentTransferEncoding);
			}
			if (contentType != null)
			{
				this.mimeWriter.WriteHeader(MimeGlobals.ContentTypeHeader, contentType);
			}
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001A900 File Offset: 0x00018B00
		public override void Close()
		{
			if (!this.isClosed)
			{
				this.isClosed = true;
				if (this.IsInitialized)
				{
					this.WriteXOPInclude();
					if (this.Writer.WriteState == WriteState.Element || this.Writer.WriteState == WriteState.Attribute || this.Writer.WriteState == WriteState.Content)
					{
						this.Writer.WriteEndDocument();
					}
					this.Writer.Flush();
					this.depth = 0;
					this.WriteXOPBinaryParts();
					this.Writer.Close();
				}
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001A984 File Offset: 0x00018B84
		private void CheckIfStartContentTypeAttribute(string localName, string ns)
		{
			if (localName != null && localName == MtomGlobals.MimeContentTypeLocalName && ns != null && (ns == MtomGlobals.MimeContentTypeNamespace200406 || ns == MtomGlobals.MimeContentTypeNamespace200505))
			{
				this.contentTypeStream = new MemoryStream();
				this.infosetWriter = this.Writer;
				this.writer = XmlDictionaryWriter.CreateBinaryWriter(this.contentTypeStream);
				this.Writer.WriteStartElement("Wrapper");
				this.Writer.WriteStartAttribute(localName, ns);
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001AA04 File Offset: 0x00018C04
		private void CheckIfEndContentTypeAttribute()
		{
			if (this.contentTypeStream != null)
			{
				this.Writer.WriteEndAttribute();
				this.Writer.WriteEndElement();
				this.Writer.Flush();
				this.contentTypeStream.Position = 0L;
				XmlReader xmlReader = XmlDictionaryReader.CreateBinaryReader(this.contentTypeStream, null, XmlDictionaryReaderQuotas.Max, null, null);
				while (xmlReader.Read())
				{
					if (xmlReader.IsStartElement("Wrapper"))
					{
						this.contentType = xmlReader.GetAttribute(MtomGlobals.MimeContentTypeLocalName, MtomGlobals.MimeContentTypeNamespace200406);
						if (this.contentType == null)
						{
							this.contentType = xmlReader.GetAttribute(MtomGlobals.MimeContentTypeLocalName, MtomGlobals.MimeContentTypeNamespace200505);
							break;
						}
						break;
					}
				}
				this.writer = this.infosetWriter;
				this.infosetWriter = null;
				this.contentTypeStream = null;
				if (this.contentType != null)
				{
					this.Writer.WriteString(this.contentType);
				}
			}
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001AADE File Offset: 0x00018CDE
		public override void Flush()
		{
			if (this.IsInitialized)
			{
				this.Writer.Flush();
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001AAF3 File Offset: 0x00018CF3
		public override string LookupPrefix(string ns)
		{
			return this.Writer.LookupPrefix(ns);
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0001AB01 File Offset: 0x00018D01
		public override XmlWriterSettings Settings
		{
			get
			{
				return this.Writer.Settings;
			}
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001AB0E File Offset: 0x00018D0E
		public override void WriteAttributes(XmlReader reader, bool defattr)
		{
			this.Writer.WriteAttributes(reader, defattr);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001AB1D File Offset: 0x00018D1D
		public override void WriteBinHex(byte[] buffer, int index, int count)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteBinHex(buffer, index, count);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001AB33 File Offset: 0x00018D33
		public override void WriteCData(string text)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteCData(text);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001AB47 File Offset: 0x00018D47
		public override void WriteCharEntity(char ch)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteCharEntity(ch);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001AB5B File Offset: 0x00018D5B
		public override void WriteChars(char[] buffer, int index, int count)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteChars(buffer, index, count);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001AB71 File Offset: 0x00018D71
		public override void WriteComment(string text)
		{
			if (this.depth == 0 && this.mimeWriter.WriteState == MimeWriterState.Closed)
			{
				return;
			}
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteComment(text);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001AB9C File Offset: 0x00018D9C
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteDocType(name, pubid, sysid, subset);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001ABB4 File Offset: 0x00018DB4
		public override void WriteEndAttribute()
		{
			this.CheckIfEndContentTypeAttribute();
			this.Writer.WriteEndAttribute();
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001ABC7 File Offset: 0x00018DC7
		public override void WriteEndDocument()
		{
			this.WriteXOPInclude();
			this.Writer.WriteEndDocument();
			this.depth = 0;
			this.WriteXOPBinaryParts();
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001ABE7 File Offset: 0x00018DE7
		public override void WriteEntityRef(string name)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteEntityRef(name);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001ABFB File Offset: 0x00018DFB
		public override void WriteName(string name)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteName(name);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001AC0F File Offset: 0x00018E0F
		public override void WriteNmToken(string name)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteNmToken(name);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001AC24 File Offset: 0x00018E24
		protected override void WriteTextNode(XmlDictionaryReader reader, bool attribute)
		{
			Type valueType = reader.ValueType;
			if (valueType == typeof(string))
			{
				if (reader.CanReadValueChunk)
				{
					if (this.chars == null)
					{
						this.chars = new char[256];
					}
					int num;
					while ((num = reader.ReadValueChunk(this.chars, 0, this.chars.Length)) > 0)
					{
						this.WriteChars(this.chars, 0, num);
					}
				}
				else
				{
					this.WriteString(reader.Value);
				}
				if (!attribute)
				{
					reader.Read();
					return;
				}
			}
			else if (valueType == typeof(byte[]))
			{
				if (reader.CanReadBinaryContent)
				{
					if (this.bytes == null)
					{
						this.bytes = new byte[384];
					}
					int num2;
					while ((num2 = reader.ReadValueAsBase64(this.bytes, 0, this.bytes.Length)) > 0)
					{
						this.WriteBase64(this.bytes, 0, num2);
					}
				}
				else
				{
					this.WriteString(reader.Value);
				}
				if (!attribute)
				{
					reader.Read();
					return;
				}
			}
			else
			{
				base.WriteTextNode(reader, attribute);
			}
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001AD2D File Offset: 0x00018F2D
		public override void WriteNode(XPathNavigator navigator, bool defattr)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteNode(navigator, defattr);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001AD42 File Offset: 0x00018F42
		public override void WriteProcessingInstruction(string name, string text)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteProcessingInstruction(name, text);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001AD57 File Offset: 0x00018F57
		public override void WriteQualifiedName(string localName, string namespaceUri)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteQualifiedName(localName, namespaceUri);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001AD6C File Offset: 0x00018F6C
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteRaw(buffer, index, count);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001AD82 File Offset: 0x00018F82
		public override void WriteRaw(string data)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteRaw(data);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001AD96 File Offset: 0x00018F96
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.Writer.WriteStartAttribute(prefix, localName, ns);
			this.CheckIfStartContentTypeAttribute(localName, ns);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001ADAE File Offset: 0x00018FAE
		public override void WriteStartAttribute(string prefix, XmlDictionaryString localName, XmlDictionaryString ns)
		{
			this.Writer.WriteStartAttribute(prefix, localName, ns);
			if (localName != null && ns != null)
			{
				this.CheckIfStartContentTypeAttribute(localName.Value, ns.Value);
			}
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001ADD6 File Offset: 0x00018FD6
		public override void WriteStartDocument()
		{
			this.Writer.WriteStartDocument();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001ADE3 File Offset: 0x00018FE3
		public override void WriteStartDocument(bool standalone)
		{
			this.Writer.WriteStartDocument(standalone);
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x0001ADF1 File Offset: 0x00018FF1
		public override WriteState WriteState
		{
			get
			{
				return this.Writer.WriteState;
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001ADFE File Offset: 0x00018FFE
		public override void WriteString(string text)
		{
			if (this.depth == 0 && this.mimeWriter.WriteState == MimeWriterState.Closed && XmlConverter.IsWhitespace(text))
			{
				return;
			}
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteString(text);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001AE31 File Offset: 0x00019031
		public override void WriteString(XmlDictionaryString value)
		{
			if (this.depth == 0 && this.mimeWriter.WriteState == MimeWriterState.Closed && XmlConverter.IsWhitespace(value.Value))
			{
				return;
			}
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteString(value);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001AE69 File Offset: 0x00019069
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteSurrogateCharEntity(lowChar, highChar);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001AE7E File Offset: 0x0001907E
		public override void WriteWhitespace(string whitespace)
		{
			if (this.depth == 0 && this.mimeWriter.WriteState == MimeWriterState.Closed)
			{
				return;
			}
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteWhitespace(whitespace);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001AEAC File Offset: 0x000190AC
		public override void WriteValue(object value)
		{
			IStreamProvider streamProvider = value as IStreamProvider;
			if (streamProvider != null)
			{
				this.WriteValue(streamProvider);
				return;
			}
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteValue(value);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001AEDD File Offset: 0x000190DD
		public override void WriteValue(string value)
		{
			if (this.depth == 0 && this.mimeWriter.WriteState == MimeWriterState.Closed && XmlConverter.IsWhitespace(value))
			{
				return;
			}
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteValue(value);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001AF10 File Offset: 0x00019110
		public override void WriteValue(bool value)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteValue(value);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001AF24 File Offset: 0x00019124
		public override void WriteValue(DateTime value)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteValue(value);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001AF38 File Offset: 0x00019138
		public override void WriteValue(double value)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteValue(value);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001AF4C File Offset: 0x0001914C
		public override void WriteValue(int value)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteValue(value);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001AF60 File Offset: 0x00019160
		public override void WriteValue(long value)
		{
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteValue(value);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001AF74 File Offset: 0x00019174
		public override void WriteValue(XmlDictionaryString value)
		{
			if (this.depth == 0 && this.mimeWriter.WriteState == MimeWriterState.Closed && XmlConverter.IsWhitespace(value.Value))
			{
				return;
			}
			this.WriteBase64InlineIfPresent();
			this.Writer.WriteValue(value);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001AFAC File Offset: 0x000191AC
		public override void WriteXmlnsAttribute(string prefix, string ns)
		{
			this.Writer.WriteXmlnsAttribute(prefix, ns);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001AFBB File Offset: 0x000191BB
		public override void WriteXmlnsAttribute(string prefix, XmlDictionaryString ns)
		{
			this.Writer.WriteXmlnsAttribute(prefix, ns);
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x0001AFCA File Offset: 0x000191CA
		public override string XmlLang
		{
			get
			{
				return this.Writer.XmlLang;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x0001AFD7 File Offset: 0x000191D7
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.Writer.XmlSpace;
			}
		}

		// Token: 0x0400023F RID: 575
		private const int MaxInlinedBytes = 767;

		// Token: 0x04000240 RID: 576
		private int maxSizeInBytes;

		// Token: 0x04000241 RID: 577
		private XmlDictionaryWriter writer;

		// Token: 0x04000242 RID: 578
		private XmlDictionaryWriter infosetWriter;

		// Token: 0x04000243 RID: 579
		private MimeWriter mimeWriter;

		// Token: 0x04000244 RID: 580
		private Encoding encoding;

		// Token: 0x04000245 RID: 581
		private bool isUTF8;

		// Token: 0x04000246 RID: 582
		private string contentID;

		// Token: 0x04000247 RID: 583
		private string contentType;

		// Token: 0x04000248 RID: 584
		private string initialContentTypeForRootPart;

		// Token: 0x04000249 RID: 585
		private string initialContentTypeForMimeMessage;

		// Token: 0x0400024A RID: 586
		private MemoryStream contentTypeStream;

		// Token: 0x0400024B RID: 587
		private List<XmlMtomWriter.MimePart> mimeParts;

		// Token: 0x0400024C RID: 588
		private IList<MtomBinaryData> binaryDataChunks;

		// Token: 0x0400024D RID: 589
		private int depth;

		// Token: 0x0400024E RID: 590
		private int totalSizeOfMimeParts;

		// Token: 0x0400024F RID: 591
		private int sizeOfBufferedBinaryData;

		// Token: 0x04000250 RID: 592
		private char[] chars;

		// Token: 0x04000251 RID: 593
		private byte[] bytes;

		// Token: 0x04000252 RID: 594
		private bool isClosed;

		// Token: 0x04000253 RID: 595
		private bool ownsStream;

		// Token: 0x0200015E RID: 350
		private static class MimeBoundaryGenerator
		{
			// Token: 0x0600138B RID: 5003 RVA: 0x0004F8D8 File Offset: 0x0004DAD8
			internal static string Next()
			{
				long num = Interlocked.Increment(ref XmlMtomWriter.MimeBoundaryGenerator.id);
				return string.Format(CultureInfo.InvariantCulture, "{0}{1}", XmlMtomWriter.MimeBoundaryGenerator.prefix, num);
			}

			// Token: 0x0400097D RID: 2429
			private static long id;

			// Token: 0x0400097E RID: 2430
			private static string prefix = Guid.NewGuid().ToString() + "+id=";
		}

		// Token: 0x0200015F RID: 351
		private class MimePart
		{
			// Token: 0x0600138C RID: 5004 RVA: 0x0004F90C File Offset: 0x0004DB0C
			internal MimePart(IList<MtomBinaryData> binaryData, string contentID, string contentType, string contentTransferEncoding, int sizeOfBufferedBinaryData, int maxSizeInBytes)
			{
				this.binaryData = binaryData;
				this.contentID = contentID;
				this.contentType = contentType ?? MtomGlobals.DefaultContentTypeForBinary;
				this.contentTransferEncoding = contentTransferEncoding;
				this.sizeInBytes = XmlMtomWriter.MimePart.GetSize(contentID, contentType, contentTransferEncoding, sizeOfBufferedBinaryData, maxSizeInBytes);
			}

			// Token: 0x0600138D RID: 5005 RVA: 0x0004F958 File Offset: 0x0004DB58
			private static int GetSize(string contentID, string contentType, string contentTransferEncoding, int sizeOfBufferedBinaryData, int maxSizeInBytes)
			{
				int num = XmlMtomWriter.ValidateSizeOfMessage(maxSizeInBytes, 0, MimeGlobals.CRLF.Length * 3);
				if (contentTransferEncoding != null)
				{
					num += XmlMtomWriter.ValidateSizeOfMessage(maxSizeInBytes, num, MimeWriter.GetHeaderSize(MimeGlobals.ContentTransferEncodingHeader, contentTransferEncoding, maxSizeInBytes));
				}
				if (contentType != null)
				{
					num += XmlMtomWriter.ValidateSizeOfMessage(maxSizeInBytes, num, MimeWriter.GetHeaderSize(MimeGlobals.ContentTypeHeader, contentType, maxSizeInBytes));
				}
				if (contentID != null)
				{
					num += XmlMtomWriter.ValidateSizeOfMessage(maxSizeInBytes, num, MimeWriter.GetHeaderSize(MimeGlobals.ContentIDHeader, contentID, maxSizeInBytes));
					num += XmlMtomWriter.ValidateSizeOfMessage(maxSizeInBytes, num, 2);
				}
				return num + XmlMtomWriter.ValidateSizeOfMessage(maxSizeInBytes, num, sizeOfBufferedBinaryData);
			}

			// Token: 0x0400097F RID: 2431
			internal IList<MtomBinaryData> binaryData;

			// Token: 0x04000980 RID: 2432
			internal string contentID;

			// Token: 0x04000981 RID: 2433
			internal string contentType;

			// Token: 0x04000982 RID: 2434
			internal string contentTransferEncoding;

			// Token: 0x04000983 RID: 2435
			internal int sizeInBytes;
		}
	}
}
