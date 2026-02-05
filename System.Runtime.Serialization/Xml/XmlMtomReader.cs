using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200003C RID: 60
	internal class XmlMtomReader : XmlDictionaryReader, IXmlLineInfo, IXmlMtomReaderInitializer
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x00016D6D File Offset: 0x00014F6D
		internal static void DecrementBufferQuota(int maxBuffer, ref int remaining, int size)
		{
			if (remaining - size <= 0)
			{
				remaining = 0;
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM buffer quota exceeded. The maximum size is {0}.", new object[] { maxBuffer })));
			}
			remaining -= size;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00016DA4 File Offset: 0x00014FA4
		private void SetReadEncodings(Encoding[] encodings)
		{
			if (encodings == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encodings");
			}
			for (int i = 0; i < encodings.Length; i++)
			{
				if (encodings[i] == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "encodings[{0}]", i));
				}
			}
			this.encodings = new Encoding[encodings.Length];
			encodings.CopyTo(this.encodings, 0);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00016E08 File Offset: 0x00015008
		private void CheckContentType(string contentType)
		{
			if (contentType != null && contentType.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("MTOM content type is invalid."), "contentType"));
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00016E2F File Offset: 0x0001502F
		public void SetInput(byte[] buffer, int offset, int count, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose onClose)
		{
			this.SetInput(new MemoryStream(buffer, offset, count), encodings, contentType, quotas, maxBufferSize, onClose);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00016E49 File Offset: 0x00015049
		public void SetInput(Stream stream, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose onClose)
		{
			this.SetReadEncodings(encodings);
			this.CheckContentType(contentType);
			this.Initialize(stream, contentType, quotas, maxBufferSize);
			this.onClose = onClose;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00016E70 File Offset: 0x00015070
		private void Initialize(Stream stream, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
			}
			this.maxBufferSize = maxBufferSize;
			this.bufferRemaining = maxBufferSize;
			string text;
			string text2;
			string text3;
			if (contentType == null)
			{
				MimeMessageReader mimeMessageReader = new MimeMessageReader(stream);
				MimeHeaders mimeHeaders = mimeMessageReader.ReadHeaders(this.maxBufferSize, ref this.bufferRemaining);
				this.ReadMessageMimeVersionHeader(mimeHeaders.MimeVersion);
				this.ReadMessageContentTypeHeader(mimeHeaders.ContentType, out text, out text2, out text3);
				stream = mimeMessageReader.GetContentStream();
				if (stream == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM message content is invalid.")));
				}
			}
			else
			{
				this.ReadMessageContentTypeHeader(new ContentTypeHeader(contentType), out text, out text2, out text3);
			}
			this.mimeReader = new MimeReader(stream, text);
			this.mimeParts = null;
			this.readingBinaryElement = false;
			XmlMtomReader.MimePart mimePart = ((text2 == null) ? this.ReadRootMimePart() : this.ReadMimePart(this.GetStartUri(text2)));
			byte[] buffer = mimePart.GetBuffer(this.maxBufferSize, ref this.bufferRemaining);
			int num = (int)mimePart.Length;
			Encoding encoding = this.ReadRootContentTypeHeader(mimePart.Headers.ContentType, this.encodings, text3);
			this.CheckContentTransferEncodingOnRoot(mimePart.Headers.ContentTransferEncoding);
			IXmlTextReaderInitializer xmlTextReaderInitializer = this.xmlReader as IXmlTextReaderInitializer;
			if (xmlTextReaderInitializer != null)
			{
				xmlTextReaderInitializer.SetInput(buffer, 0, num, encoding, quotas, null);
				return;
			}
			this.xmlReader = XmlDictionaryReader.CreateTextReader(buffer, 0, num, encoding, quotas, null);
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x00016FBB File Offset: 0x000151BB
		public override XmlDictionaryReaderQuotas Quotas
		{
			get
			{
				return this.xmlReader.Quotas;
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00016FC8 File Offset: 0x000151C8
		private void ReadMessageMimeVersionHeader(MimeVersionHeader header)
		{
			if (header != null && header.Version != MimeVersionHeader.Default.Version)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM message has invalid MIME version. Expected '{1}', got '{0}' instead.", new object[]
				{
					header.Version,
					MimeVersionHeader.Default.Version
				})));
			}
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00017020 File Offset: 0x00015220
		private void ReadMessageContentTypeHeader(ContentTypeHeader header, out string boundary, out string start, out string startInfo)
		{
			if (header == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM message content type was not found.")));
			}
			if (string.Compare(MtomGlobals.MediaType, header.MediaType, StringComparison.OrdinalIgnoreCase) != 0 || string.Compare(MtomGlobals.MediaSubtype, header.MediaSubtype, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM message is not multipart: media type should be '{0}', media subtype should be '{1}'.", new object[]
				{
					MtomGlobals.MediaType,
					MtomGlobals.MediaSubtype
				})));
			}
			string text;
			if (!header.Parameters.TryGetValue(MtomGlobals.TypeParam, out text) || MtomGlobals.XopType != text)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM msssage type is not '{0}'.", new object[] { MtomGlobals.XopType })));
			}
			if (!header.Parameters.TryGetValue(MtomGlobals.BoundaryParam, out boundary))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Required MTOM parameter '{0}' is not specified.", new object[] { MtomGlobals.BoundaryParam })));
			}
			if (!MailBnfHelper.IsValidMimeBoundary(boundary))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MIME boundary is invalid: '{0}'.", new object[] { boundary })));
			}
			if (!header.Parameters.TryGetValue(MtomGlobals.StartParam, out start))
			{
				start = null;
			}
			if (!header.Parameters.TryGetValue(MtomGlobals.StartInfoParam, out startInfo))
			{
				startInfo = null;
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00017168 File Offset: 0x00015368
		private Encoding ReadRootContentTypeHeader(ContentTypeHeader header, Encoding[] expectedEncodings, string expectedType)
		{
			if (header == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM root content type is not found.")));
			}
			if (string.Compare(MtomGlobals.XopMediaType, header.MediaType, StringComparison.OrdinalIgnoreCase) != 0 || string.Compare(MtomGlobals.XopMediaSubtype, header.MediaSubtype, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM root should have media type '{0}' and subtype '{1}'.", new object[]
				{
					MtomGlobals.XopMediaType,
					MtomGlobals.XopMediaSubtype
				})));
			}
			string text;
			if (!header.Parameters.TryGetValue(MtomGlobals.CharsetParam, out text) || text == null || text.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Required MTOM root parameter '{0}' is not specified.", new object[] { MtomGlobals.CharsetParam })));
			}
			Encoding encoding = null;
			for (int i = 0; i < this.encodings.Length; i++)
			{
				if (string.Compare(text, expectedEncodings[i].WebName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					encoding = expectedEncodings[i];
					break;
				}
			}
			if (encoding == null)
			{
				if (string.Compare(text, "utf-16LE", StringComparison.OrdinalIgnoreCase) == 0)
				{
					for (int j = 0; j < this.encodings.Length; j++)
					{
						if (string.Compare(expectedEncodings[j].WebName, Encoding.Unicode.WebName, StringComparison.OrdinalIgnoreCase) == 0)
						{
							encoding = expectedEncodings[j];
							break;
						}
					}
				}
				else if (string.Compare(text, "utf-16BE", StringComparison.OrdinalIgnoreCase) == 0)
				{
					for (int k = 0; k < this.encodings.Length; k++)
					{
						if (string.Compare(expectedEncodings[k].WebName, Encoding.BigEndianUnicode.WebName, StringComparison.OrdinalIgnoreCase) == 0)
						{
							encoding = expectedEncodings[k];
							break;
						}
					}
				}
				if (encoding == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int l = 0; l < this.encodings.Length; l++)
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(" | ");
						}
						stringBuilder.Append(this.encodings[l].WebName);
					}
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Unexpected charset on MTOM root. Expected '{1}', got '{0}' instead.", new object[]
					{
						text,
						stringBuilder.ToString()
					})));
				}
			}
			if (expectedType != null)
			{
				string text2;
				if (!header.Parameters.TryGetValue(MtomGlobals.TypeParam, out text2) || text2 == null || text2.Length == 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Required MTOM root parameter '{0}' is not specified.", new object[] { MtomGlobals.TypeParam })));
				}
				if (text2 != expectedType)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Unexpected type on MTOM root. Expected '{1}', got '{0}' instead.", new object[] { text2, expectedType })));
				}
			}
			return encoding;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000173C8 File Offset: 0x000155C8
		private void CheckContentTransferEncodingOnRoot(ContentTransferEncodingHeader header)
		{
			if (header != null && header.ContentTransferEncoding == ContentTransferEncoding.Other)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM content transfer encoding value is not supported. Raw value is '{0}', '{1}' in 7bit encoding, '{2}' in 8bit encoding, and '{3}' in binary.", new object[]
				{
					header.Value,
					ContentTransferEncodingHeader.SevenBit.ContentTransferEncodingValue,
					ContentTransferEncodingHeader.EightBit.ContentTransferEncodingValue,
					ContentTransferEncodingHeader.Binary.ContentTransferEncodingValue
				})));
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001742C File Offset: 0x0001562C
		private void CheckContentTransferEncodingOnBinaryPart(ContentTransferEncodingHeader header)
		{
			if (header == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM content transfer encoding is not present. ContentTransferEncoding header is '{0}'.", new object[] { ContentTransferEncodingHeader.Binary.ContentTransferEncodingValue })));
			}
			if (header.ContentTransferEncoding != ContentTransferEncoding.Binary)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Invalid transfer encoding for MIME part: '{0}', in binary: '{1}'.", new object[]
				{
					header.Value,
					ContentTransferEncodingHeader.Binary.ContentTransferEncodingValue
				})));
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x000174A0 File Offset: 0x000156A0
		private string GetStartUri(string startUri)
		{
			if (!startUri.StartsWith("<", StringComparison.Ordinal))
			{
				return string.Format(CultureInfo.InvariantCulture, "<{0}>", startUri);
			}
			if (startUri.EndsWith(">", StringComparison.Ordinal))
			{
				return startUri;
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Invalid MTOM start URI: '{0}'.", new object[] { startUri })));
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x000174FC File Offset: 0x000156FC
		public override bool Read()
		{
			bool flag = this.xmlReader.Read();
			if (this.xmlReader.NodeType == XmlNodeType.Element)
			{
				XmlMtomReader.XopIncludeReader xopIncludeReader = null;
				if (this.xmlReader.IsStartElement(MtomGlobals.XopIncludeLocalName, MtomGlobals.XopIncludeNamespace))
				{
					string text = null;
					while (this.xmlReader.MoveToNextAttribute())
					{
						if (this.xmlReader.LocalName == MtomGlobals.XopIncludeHrefLocalName && this.xmlReader.NamespaceURI == MtomGlobals.XopIncludeHrefNamespace)
						{
							text = this.xmlReader.Value;
						}
						else if (this.xmlReader.NamespaceURI == MtomGlobals.XopIncludeNamespace)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("xop Include element has invalid attribute: '{0}' in '{1}' namespace.", new object[]
							{
								this.xmlReader.LocalName,
								MtomGlobals.XopIncludeNamespace
							})));
						}
					}
					if (text == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("xop Include element did not specify '{0}' attribute.", new object[] { MtomGlobals.XopIncludeHrefLocalName })));
					}
					XmlMtomReader.MimePart mimePart = this.ReadMimePart(text);
					this.CheckContentTransferEncodingOnBinaryPart(mimePart.Headers.ContentTransferEncoding);
					this.part = mimePart;
					xopIncludeReader = new XmlMtomReader.XopIncludeReader(mimePart, this.xmlReader);
					xopIncludeReader.Read();
					this.xmlReader.MoveToElement();
					if (this.xmlReader.IsEmptyElement)
					{
						this.xmlReader.Read();
					}
					else
					{
						int depth = this.xmlReader.Depth;
						this.xmlReader.ReadStartElement();
						while (this.xmlReader.Depth > depth)
						{
							if (this.xmlReader.IsStartElement() && this.xmlReader.NamespaceURI == MtomGlobals.XopIncludeNamespace)
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("xop Include element has invalid element: '{0}' in '{1}' namespace.", new object[]
								{
									this.xmlReader.LocalName,
									MtomGlobals.XopIncludeNamespace
								})));
							}
							this.xmlReader.Skip();
						}
						this.xmlReader.ReadEndElement();
					}
				}
				if (xopIncludeReader != null)
				{
					this.xmlReader.MoveToContent();
					this.infosetReader = this.xmlReader;
					this.xmlReader = xopIncludeReader;
				}
			}
			if (this.xmlReader.ReadState == ReadState.EndOfFile && this.infosetReader != null)
			{
				if (!flag)
				{
					flag = this.infosetReader.Read();
				}
				this.part.Release(this.maxBufferSize, ref this.bufferRemaining);
				this.xmlReader = this.infosetReader;
				this.infosetReader = null;
			}
			return flag;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001776C File Offset: 0x0001596C
		private XmlMtomReader.MimePart ReadMimePart(string uri)
		{
			XmlMtomReader.MimePart mimePart = null;
			if (uri == null || uri.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("empty URI is invalid for MTOM MIME part.")));
			}
			string text = null;
			if (uri.StartsWith(MimeGlobals.ContentIDScheme, StringComparison.Ordinal))
			{
				text = string.Format(CultureInfo.InvariantCulture, "<{0}>", Uri.UnescapeDataString(uri.Substring(MimeGlobals.ContentIDScheme.Length)));
			}
			else if (uri.StartsWith("<", StringComparison.Ordinal))
			{
				text = uri;
			}
			if (text == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Invalid MTOM CID URI: '{0}'.", new object[] { uri })));
			}
			if (this.mimeParts != null && this.mimeParts.TryGetValue(text, out mimePart))
			{
				if (mimePart.ReferencedFromInfoset)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Specified MIME part '{0}' is referenced more than once.", new object[] { text })));
				}
			}
			else
			{
				int maxMimeParts = AppSettings.MaxMimeParts;
				while (mimePart == null && this.mimeReader.ReadNextPart())
				{
					MimeHeaders mimeHeaders = this.mimeReader.ReadHeaders(this.maxBufferSize, ref this.bufferRemaining);
					Stream contentStream = this.mimeReader.GetContentStream();
					if (contentStream == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM message content in MIME part is invalid.")));
					}
					ContentIDHeader contentIDHeader = ((mimeHeaders == null) ? null : mimeHeaders.ContentID);
					if (contentIDHeader == null || contentIDHeader.Value == null)
					{
						int num = 256;
						byte[] array = new byte[num];
						int num2;
						do
						{
							num2 = contentStream.Read(array, 0, num);
						}
						while (num2 > 0);
					}
					else
					{
						string value = mimeHeaders.ContentID.Value;
						XmlMtomReader.MimePart mimePart2 = new XmlMtomReader.MimePart(contentStream, mimeHeaders);
						if (this.mimeParts == null)
						{
							this.mimeParts = new Dictionary<string, XmlMtomReader.MimePart>();
						}
						this.mimeParts.Add(value, mimePart2);
						if (this.mimeParts.Count > maxMimeParts)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MIME parts number exceeded the maximum settings. Must be less than {0}. Specified as '{1}'.", new object[] { maxMimeParts, "microsoft:xmldictionaryreader:maxmimeparts" })));
						}
						if (value.Equals(text))
						{
							mimePart = mimePart2;
						}
						else
						{
							mimePart2.GetBuffer(this.maxBufferSize, ref this.bufferRemaining);
						}
					}
				}
				if (mimePart == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM part with URI '{0}' is not found.", new object[] { uri })));
				}
			}
			mimePart.ReferencedFromInfoset = true;
			return mimePart;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x000179A8 File Offset: 0x00015BA8
		private XmlMtomReader.MimePart ReadRootMimePart()
		{
			if (!this.mimeReader.ReadNextPart())
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM root part is not found.")));
			}
			MimeHeaders mimeHeaders = this.mimeReader.ReadHeaders(this.maxBufferSize, ref this.bufferRemaining);
			Stream contentStream = this.mimeReader.GetContentStream();
			if (contentStream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("MTOM message content in MIME part is invalid.")));
			}
			return new XmlMtomReader.MimePart(contentStream, mimeHeaders);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00017A18 File Offset: 0x00015C18
		private void AdvanceToContentOnElement()
		{
			if (this.NodeType != XmlNodeType.Attribute)
			{
				this.MoveToContent();
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x00017A2A File Offset: 0x00015C2A
		public override int AttributeCount
		{
			get
			{
				return this.xmlReader.AttributeCount;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00017A37 File Offset: 0x00015C37
		public override string BaseURI
		{
			get
			{
				return this.xmlReader.BaseURI;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00017A44 File Offset: 0x00015C44
		public override bool CanReadBinaryContent
		{
			get
			{
				return this.xmlReader.CanReadBinaryContent;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00017A51 File Offset: 0x00015C51
		public override bool CanReadValueChunk
		{
			get
			{
				return this.xmlReader.CanReadValueChunk;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00017A5E File Offset: 0x00015C5E
		public override bool CanResolveEntity
		{
			get
			{
				return this.xmlReader.CanResolveEntity;
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00017A6C File Offset: 0x00015C6C
		public override void Close()
		{
			this.xmlReader.Close();
			this.mimeReader.Close();
			OnXmlDictionaryReaderClose onXmlDictionaryReaderClose = this.onClose;
			this.onClose = null;
			if (onXmlDictionaryReaderClose != null)
			{
				try
				{
					onXmlDictionaryReaderClose(this);
				}
				catch (Exception ex)
				{
					if (Fx.IsFatal(ex))
					{
						throw;
					}
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperCallback(ex);
				}
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00017ACC File Offset: 0x00015CCC
		public override int Depth
		{
			get
			{
				return this.xmlReader.Depth;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00017AD9 File Offset: 0x00015CD9
		public override bool EOF
		{
			get
			{
				return this.xmlReader.EOF;
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00017AE6 File Offset: 0x00015CE6
		public override string GetAttribute(int index)
		{
			return this.xmlReader.GetAttribute(index);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00017AF4 File Offset: 0x00015CF4
		public override string GetAttribute(string name)
		{
			return this.xmlReader.GetAttribute(name);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00017B02 File Offset: 0x00015D02
		public override string GetAttribute(string name, string ns)
		{
			return this.xmlReader.GetAttribute(name, ns);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00017B11 File Offset: 0x00015D11
		public override string GetAttribute(XmlDictionaryString localName, XmlDictionaryString ns)
		{
			return this.xmlReader.GetAttribute(localName, ns);
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00017B20 File Offset: 0x00015D20
		public override bool HasAttributes
		{
			get
			{
				return this.xmlReader.HasAttributes;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x00017B2D File Offset: 0x00015D2D
		public override bool HasValue
		{
			get
			{
				return this.xmlReader.HasValue;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00017B3A File Offset: 0x00015D3A
		public override bool IsDefault
		{
			get
			{
				return this.xmlReader.IsDefault;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00017B47 File Offset: 0x00015D47
		public override bool IsEmptyElement
		{
			get
			{
				return this.xmlReader.IsEmptyElement;
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00017B54 File Offset: 0x00015D54
		public override bool IsLocalName(string localName)
		{
			return this.xmlReader.IsLocalName(localName);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00017B62 File Offset: 0x00015D62
		public override bool IsLocalName(XmlDictionaryString localName)
		{
			return this.xmlReader.IsLocalName(localName);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00017B70 File Offset: 0x00015D70
		public override bool IsNamespaceUri(string ns)
		{
			return this.xmlReader.IsNamespaceUri(ns);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00017B7E File Offset: 0x00015D7E
		public override bool IsNamespaceUri(XmlDictionaryString ns)
		{
			return this.xmlReader.IsNamespaceUri(ns);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00017B8C File Offset: 0x00015D8C
		public override bool IsStartElement()
		{
			return this.xmlReader.IsStartElement();
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00017B99 File Offset: 0x00015D99
		public override bool IsStartElement(string localName)
		{
			return this.xmlReader.IsStartElement(localName);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00017BA7 File Offset: 0x00015DA7
		public override bool IsStartElement(string localName, string ns)
		{
			return this.xmlReader.IsStartElement(localName, ns);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00017BB6 File Offset: 0x00015DB6
		public override bool IsStartElement(XmlDictionaryString localName, XmlDictionaryString ns)
		{
			return this.xmlReader.IsStartElement(localName, ns);
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00017BC5 File Offset: 0x00015DC5
		public override string LocalName
		{
			get
			{
				return this.xmlReader.LocalName;
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00017BD2 File Offset: 0x00015DD2
		public override string LookupNamespace(string ns)
		{
			return this.xmlReader.LookupNamespace(ns);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00017BE0 File Offset: 0x00015DE0
		public override void MoveToAttribute(int index)
		{
			this.xmlReader.MoveToAttribute(index);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00017BEE File Offset: 0x00015DEE
		public override bool MoveToAttribute(string name)
		{
			return this.xmlReader.MoveToAttribute(name);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00017BFC File Offset: 0x00015DFC
		public override bool MoveToAttribute(string name, string ns)
		{
			return this.xmlReader.MoveToAttribute(name, ns);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00017C0B File Offset: 0x00015E0B
		public override bool MoveToElement()
		{
			return this.xmlReader.MoveToElement();
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00017C18 File Offset: 0x00015E18
		public override bool MoveToFirstAttribute()
		{
			return this.xmlReader.MoveToFirstAttribute();
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00017C25 File Offset: 0x00015E25
		public override bool MoveToNextAttribute()
		{
			return this.xmlReader.MoveToNextAttribute();
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00017C32 File Offset: 0x00015E32
		public override string Name
		{
			get
			{
				return this.xmlReader.Name;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00017C3F File Offset: 0x00015E3F
		public override string NamespaceURI
		{
			get
			{
				return this.xmlReader.NamespaceURI;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00017C4C File Offset: 0x00015E4C
		public override XmlNameTable NameTable
		{
			get
			{
				return this.xmlReader.NameTable;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00017C59 File Offset: 0x00015E59
		public override XmlNodeType NodeType
		{
			get
			{
				return this.xmlReader.NodeType;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00017C66 File Offset: 0x00015E66
		public override string Prefix
		{
			get
			{
				return this.xmlReader.Prefix;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x00017C73 File Offset: 0x00015E73
		public override char QuoteChar
		{
			get
			{
				return this.xmlReader.QuoteChar;
			}
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00017C80 File Offset: 0x00015E80
		public override bool ReadAttributeValue()
		{
			return this.xmlReader.ReadAttributeValue();
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00017C8D File Offset: 0x00015E8D
		public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAs(returnType, namespaceResolver);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00017CA2 File Offset: 0x00015EA2
		public override byte[] ReadContentAsBase64()
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAsBase64();
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00017CB5 File Offset: 0x00015EB5
		public override int ReadValueAsBase64(byte[] buffer, int offset, int count)
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadValueAsBase64(buffer, offset, count);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00017CCB File Offset: 0x00015ECB
		public override int ReadContentAsBase64(byte[] buffer, int offset, int count)
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAsBase64(buffer, offset, count);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00017CE4 File Offset: 0x00015EE4
		public override int ReadElementContentAsBase64(byte[] buffer, int offset, int count)
		{
			if (!this.readingBinaryElement)
			{
				if (this.IsEmptyElement)
				{
					this.Read();
					return 0;
				}
				this.ReadStartElement();
				this.readingBinaryElement = true;
			}
			int num = this.ReadContentAsBase64(buffer, offset, count);
			if (num == 0)
			{
				this.ReadEndElement();
				this.readingBinaryElement = false;
			}
			return num;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00017D30 File Offset: 0x00015F30
		public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int count)
		{
			if (!this.readingBinaryElement)
			{
				if (this.IsEmptyElement)
				{
					this.Read();
					return 0;
				}
				this.ReadStartElement();
				this.readingBinaryElement = true;
			}
			int num = this.ReadContentAsBinHex(buffer, offset, count);
			if (num == 0)
			{
				this.ReadEndElement();
				this.readingBinaryElement = false;
			}
			return num;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00017D7C File Offset: 0x00015F7C
		public override int ReadContentAsBinHex(byte[] buffer, int offset, int count)
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAsBinHex(buffer, offset, count);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00017D92 File Offset: 0x00015F92
		public override bool ReadContentAsBoolean()
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAsBoolean();
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00017DA5 File Offset: 0x00015FA5
		public override int ReadContentAsChars(char[] chars, int index, int count)
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAsChars(chars, index, count);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00017DBB File Offset: 0x00015FBB
		public override DateTime ReadContentAsDateTime()
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAsDateTime();
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00017DCE File Offset: 0x00015FCE
		public override decimal ReadContentAsDecimal()
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAsDecimal();
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00017DE1 File Offset: 0x00015FE1
		public override double ReadContentAsDouble()
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAsDouble();
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00017DF4 File Offset: 0x00015FF4
		public override int ReadContentAsInt()
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAsInt();
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00017E07 File Offset: 0x00016007
		public override long ReadContentAsLong()
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAsLong();
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00017E1A File Offset: 0x0001601A
		public override object ReadContentAsObject()
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAsObject();
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00017E2D File Offset: 0x0001602D
		public override float ReadContentAsFloat()
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAsFloat();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00017E40 File Offset: 0x00016040
		public override string ReadContentAsString()
		{
			this.AdvanceToContentOnElement();
			return this.xmlReader.ReadContentAsString();
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00017E53 File Offset: 0x00016053
		public override string ReadInnerXml()
		{
			return this.xmlReader.ReadInnerXml();
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00017E60 File Offset: 0x00016060
		public override string ReadOuterXml()
		{
			return this.xmlReader.ReadOuterXml();
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00017E6D File Offset: 0x0001606D
		public override ReadState ReadState
		{
			get
			{
				if (this.xmlReader.ReadState != ReadState.Interactive && this.infosetReader != null)
				{
					return this.infosetReader.ReadState;
				}
				return this.xmlReader.ReadState;
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00017E9C File Offset: 0x0001609C
		public override int ReadValueChunk(char[] buffer, int index, int count)
		{
			return this.xmlReader.ReadValueChunk(buffer, index, count);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00017EAC File Offset: 0x000160AC
		public override void ResolveEntity()
		{
			this.xmlReader.ResolveEntity();
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x00017EB9 File Offset: 0x000160B9
		public override XmlReaderSettings Settings
		{
			get
			{
				return this.xmlReader.Settings;
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00017EC6 File Offset: 0x000160C6
		public override void Skip()
		{
			this.xmlReader.Skip();
		}

		// Token: 0x17000069 RID: 105
		public override string this[int index]
		{
			get
			{
				return this.xmlReader[index];
			}
		}

		// Token: 0x1700006A RID: 106
		public override string this[string name]
		{
			get
			{
				return this.xmlReader[name];
			}
		}

		// Token: 0x1700006B RID: 107
		public override string this[string name, string ns]
		{
			get
			{
				return this.xmlReader[name, ns];
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x00017EFE File Offset: 0x000160FE
		public override string Value
		{
			get
			{
				return this.xmlReader.Value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00017F0B File Offset: 0x0001610B
		public override Type ValueType
		{
			get
			{
				return this.xmlReader.ValueType;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x00017F18 File Offset: 0x00016118
		public override string XmlLang
		{
			get
			{
				return this.xmlReader.XmlLang;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00017F25 File Offset: 0x00016125
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.xmlReader.XmlSpace;
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00017F34 File Offset: 0x00016134
		public bool HasLineInfo()
		{
			if (this.xmlReader.ReadState == ReadState.Closed)
			{
				return false;
			}
			IXmlLineInfo xmlLineInfo = this.xmlReader as IXmlLineInfo;
			return xmlLineInfo != null && xmlLineInfo.HasLineInfo();
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00017F68 File Offset: 0x00016168
		public int LineNumber
		{
			get
			{
				if (this.xmlReader.ReadState == ReadState.Closed)
				{
					return 0;
				}
				IXmlLineInfo xmlLineInfo = this.xmlReader as IXmlLineInfo;
				if (xmlLineInfo == null)
				{
					return 0;
				}
				return xmlLineInfo.LineNumber;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x00017F9C File Offset: 0x0001619C
		public int LinePosition
		{
			get
			{
				if (this.xmlReader.ReadState == ReadState.Closed)
				{
					return 0;
				}
				IXmlLineInfo xmlLineInfo = this.xmlReader as IXmlLineInfo;
				if (xmlLineInfo == null)
				{
					return 0;
				}
				return xmlLineInfo.LinePosition;
			}
		}

		// Token: 0x04000200 RID: 512
		private Encoding[] encodings;

		// Token: 0x04000201 RID: 513
		private XmlDictionaryReader xmlReader;

		// Token: 0x04000202 RID: 514
		private XmlDictionaryReader infosetReader;

		// Token: 0x04000203 RID: 515
		private MimeReader mimeReader;

		// Token: 0x04000204 RID: 516
		private Dictionary<string, XmlMtomReader.MimePart> mimeParts;

		// Token: 0x04000205 RID: 517
		private OnXmlDictionaryReaderClose onClose;

		// Token: 0x04000206 RID: 518
		private bool readingBinaryElement;

		// Token: 0x04000207 RID: 519
		private int maxBufferSize;

		// Token: 0x04000208 RID: 520
		private int bufferRemaining;

		// Token: 0x04000209 RID: 521
		private XmlMtomReader.MimePart part;

		// Token: 0x02000158 RID: 344
		internal class MimePart
		{
			// Token: 0x06001333 RID: 4915 RVA: 0x0004EADE File Offset: 0x0004CCDE
			internal MimePart(Stream stream, MimeHeaders headers)
			{
				this.stream = stream;
				this.headers = headers;
			}

			// Token: 0x170003D2 RID: 978
			// (get) Token: 0x06001334 RID: 4916 RVA: 0x0004EAF4 File Offset: 0x0004CCF4
			internal Stream Stream
			{
				get
				{
					return this.stream;
				}
			}

			// Token: 0x170003D3 RID: 979
			// (get) Token: 0x06001335 RID: 4917 RVA: 0x0004EAFC File Offset: 0x0004CCFC
			internal MimeHeaders Headers
			{
				get
				{
					return this.headers;
				}
			}

			// Token: 0x170003D4 RID: 980
			// (get) Token: 0x06001336 RID: 4918 RVA: 0x0004EB04 File Offset: 0x0004CD04
			// (set) Token: 0x06001337 RID: 4919 RVA: 0x0004EB0C File Offset: 0x0004CD0C
			internal bool ReferencedFromInfoset
			{
				get
				{
					return this.isReferencedFromInfoset;
				}
				set
				{
					this.isReferencedFromInfoset = value;
				}
			}

			// Token: 0x170003D5 RID: 981
			// (get) Token: 0x06001338 RID: 4920 RVA: 0x0004EB15 File Offset: 0x0004CD15
			internal long Length
			{
				get
				{
					if (!this.stream.CanSeek)
					{
						return 0L;
					}
					return this.stream.Length;
				}
			}

			// Token: 0x06001339 RID: 4921 RVA: 0x0004EB34 File Offset: 0x0004CD34
			internal byte[] GetBuffer(int maxBuffer, ref int remaining)
			{
				if (this.buffer == null)
				{
					MemoryStream memoryStream = (this.stream.CanSeek ? new MemoryStream((int)this.stream.Length) : new MemoryStream());
					int num = 256;
					byte[] array = new byte[num];
					int num2;
					do
					{
						num2 = this.stream.Read(array, 0, num);
						XmlMtomReader.DecrementBufferQuota(maxBuffer, ref remaining, num2);
						if (num2 > 0)
						{
							memoryStream.Write(array, 0, num2);
						}
					}
					while (num2 > 0);
					memoryStream.Seek(0L, SeekOrigin.Begin);
					this.buffer = memoryStream.GetBuffer();
					this.stream = memoryStream;
				}
				return this.buffer;
			}

			// Token: 0x0600133A RID: 4922 RVA: 0x0004EBC9 File Offset: 0x0004CDC9
			internal void Release(int maxBuffer, ref int remaining)
			{
				remaining += (int)this.Length;
				this.headers.Release(ref remaining);
			}

			// Token: 0x0400095C RID: 2396
			private Stream stream;

			// Token: 0x0400095D RID: 2397
			private MimeHeaders headers;

			// Token: 0x0400095E RID: 2398
			private byte[] buffer;

			// Token: 0x0400095F RID: 2399
			private bool isReferencedFromInfoset;
		}

		// Token: 0x02000159 RID: 345
		internal class XopIncludeReader : XmlDictionaryReader, IXmlLineInfo
		{
			// Token: 0x0600133B RID: 4923 RVA: 0x0004EBE4 File Offset: 0x0004CDE4
			public XopIncludeReader(XmlMtomReader.MimePart part, XmlDictionaryReader reader)
			{
				if (part == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("part");
				}
				if (reader == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("reader");
				}
				this.part = part;
				this.parentReader = reader;
				this.readState = ReadState.Initial;
				this.nodeType = XmlNodeType.None;
				this.chunkSize = Math.Min(reader.Quotas.MaxBytesPerRead, this.chunkSize);
				this.bytesRemaining = this.chunkSize;
				this.finishedStream = false;
			}

			// Token: 0x170003D6 RID: 982
			// (get) Token: 0x0600133C RID: 4924 RVA: 0x0004EC69 File Offset: 0x0004CE69
			public override XmlDictionaryReaderQuotas Quotas
			{
				get
				{
					return this.parentReader.Quotas;
				}
			}

			// Token: 0x170003D7 RID: 983
			// (get) Token: 0x0600133D RID: 4925 RVA: 0x0004EC76 File Offset: 0x0004CE76
			public override XmlNodeType NodeType
			{
				get
				{
					if (this.readState != ReadState.Interactive)
					{
						return this.parentReader.NodeType;
					}
					return this.nodeType;
				}
			}

			// Token: 0x0600133E RID: 4926 RVA: 0x0004EC94 File Offset: 0x0004CE94
			public override bool Read()
			{
				bool flag = true;
				switch (this.readState)
				{
				case ReadState.Initial:
					this.readState = ReadState.Interactive;
					this.nodeType = XmlNodeType.Text;
					break;
				case ReadState.Interactive:
					if (this.finishedStream || (this.bytesRemaining == this.chunkSize && this.stringValue == null))
					{
						this.readState = ReadState.EndOfFile;
						this.nodeType = XmlNodeType.EndElement;
					}
					else
					{
						this.bytesRemaining = this.chunkSize;
					}
					break;
				case ReadState.EndOfFile:
					this.nodeType = XmlNodeType.None;
					flag = false;
					break;
				}
				this.stringValue = null;
				this.binHexStream = null;
				this.valueOffset = 0;
				this.valueCount = 0;
				this.stringOffset = 0;
				this.CloseStreams();
				return flag;
			}

			// Token: 0x0600133F RID: 4927 RVA: 0x0004ED44 File Offset: 0x0004CF44
			public override int ReadValueAsBase64(byte[] buffer, int offset, int count)
			{
				if (buffer == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
				}
				if (offset < 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
				}
				if (offset > buffer.Length)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { buffer.Length })));
				}
				if (count < 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
				}
				if (count > buffer.Length - offset)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { buffer.Length - offset })));
				}
				if (this.stringValue != null)
				{
					count = Math.Min(count, this.valueCount);
					if (count > 0)
					{
						Buffer.BlockCopy(this.valueBuffer, this.valueOffset, buffer, offset, count);
						this.valueOffset += count;
						this.valueCount -= count;
					}
					return count;
				}
				if (this.bytesRemaining < count)
				{
					count = this.bytesRemaining;
				}
				int i = 0;
				if (this.readState == ReadState.Interactive)
				{
					while (i < count)
					{
						int num = this.part.Stream.Read(buffer, offset + i, count - i);
						if (num == 0)
						{
							this.finishedStream = true;
							break;
						}
						i += num;
					}
				}
				this.bytesRemaining -= i;
				return i;
			}

			// Token: 0x06001340 RID: 4928 RVA: 0x0004EEA8 File Offset: 0x0004D0A8
			public override int ReadContentAsBase64(byte[] buffer, int offset, int count)
			{
				if (buffer == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
				}
				if (offset < 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
				}
				if (offset > buffer.Length)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { buffer.Length })));
				}
				if (count < 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
				}
				if (count > buffer.Length - offset)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { buffer.Length - offset })));
				}
				if (this.valueCount > 0)
				{
					count = Math.Min(count, this.valueCount);
					Buffer.BlockCopy(this.valueBuffer, this.valueOffset, buffer, offset, count);
					this.valueOffset += count;
					this.valueCount -= count;
					return count;
				}
				if (this.chunkSize < count)
				{
					count = this.chunkSize;
				}
				int i = 0;
				if (this.readState == ReadState.Interactive)
				{
					while (i < count)
					{
						int num = this.part.Stream.Read(buffer, offset + i, count - i);
						if (num == 0)
						{
							this.finishedStream = true;
							if (!this.Read())
							{
								break;
							}
						}
						i += num;
					}
				}
				this.bytesRemaining = this.chunkSize;
				return i;
			}

			// Token: 0x06001341 RID: 4929 RVA: 0x0004F00C File Offset: 0x0004D20C
			public override int ReadContentAsBinHex(byte[] buffer, int offset, int count)
			{
				if (buffer == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
				}
				if (offset < 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
				}
				if (offset > buffer.Length)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { buffer.Length })));
				}
				if (count < 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
				}
				if (count > buffer.Length - offset)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { buffer.Length - offset })));
				}
				if (this.chunkSize < count)
				{
					count = this.chunkSize;
				}
				int i = 0;
				int num = 0;
				while (i < count)
				{
					if (this.binHexStream == null)
					{
						try
						{
							this.binHexStream = new MemoryStream(new BinHexEncoding().GetBytes(this.Value));
						}
						catch (FormatException ex)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(ex.Message, ex));
						}
					}
					int num2 = this.binHexStream.Read(buffer, offset + i, count - i);
					if (num2 == 0)
					{
						this.finishedStream = true;
						if (!this.Read())
						{
							break;
						}
						num = 0;
					}
					i += num2;
					num += num2;
				}
				if (this.stringValue != null && num > 0)
				{
					this.stringValue = this.stringValue.Substring(num * 2);
					this.stringOffset = Math.Max(0, this.stringOffset - num * 2);
					this.bytesRemaining = this.chunkSize;
				}
				return i;
			}

			// Token: 0x06001342 RID: 4930 RVA: 0x0004F1A0 File Offset: 0x0004D3A0
			public override int ReadValueChunk(char[] chars, int offset, int count)
			{
				if (chars == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("chars");
				}
				if (offset < 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
				}
				if (offset > chars.Length)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { chars.Length })));
				}
				if (count < 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
				}
				if (count > chars.Length - offset)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { chars.Length - offset })));
				}
				if (this.readState != ReadState.Interactive)
				{
					return 0;
				}
				string value = this.Value;
				count = Math.Min(this.stringValue.Length - this.stringOffset, count);
				if (count > 0)
				{
					this.stringValue.CopyTo(this.stringOffset, chars, offset, count);
					this.stringOffset += count;
				}
				return count;
			}

			// Token: 0x170003D8 RID: 984
			// (get) Token: 0x06001343 RID: 4931 RVA: 0x0004F2B0 File Offset: 0x0004D4B0
			public override string Value
			{
				get
				{
					if (this.readState != ReadState.Interactive)
					{
						return string.Empty;
					}
					if (this.stringValue == null)
					{
						int i = this.bytesRemaining;
						i -= i % 3;
						if (this.valueCount > 0 && this.valueOffset > 0)
						{
							Buffer.BlockCopy(this.valueBuffer, this.valueOffset, this.valueBuffer, 0, this.valueCount);
							this.valueOffset = 0;
						}
						i -= this.valueCount;
						if (this.valueBuffer == null)
						{
							this.valueBuffer = new byte[i];
						}
						else if (this.valueBuffer.Length < i)
						{
							Array.Resize<byte>(ref this.valueBuffer, i);
						}
						byte[] array = this.valueBuffer;
						int num = 0;
						while (i > 0)
						{
							int num2 = this.part.Stream.Read(array, num, i);
							if (num2 == 0)
							{
								this.finishedStream = true;
								break;
							}
							this.bytesRemaining -= num2;
							this.valueCount += num2;
							i -= num2;
							num += num2;
						}
						this.stringValue = Convert.ToBase64String(array, 0, this.valueCount);
					}
					return this.stringValue;
				}
			}

			// Token: 0x06001344 RID: 4932 RVA: 0x0004F3C0 File Offset: 0x0004D5C0
			public override string ReadContentAsString()
			{
				int num = this.Quotas.MaxStringContentLength;
				StringBuilder stringBuilder = new StringBuilder();
				do
				{
					string value = this.Value;
					if (value.Length > num)
					{
						XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, this.Quotas.MaxStringContentLength);
					}
					num -= value.Length;
					stringBuilder.Append(value);
				}
				while (this.Read());
				return stringBuilder.ToString();
			}

			// Token: 0x170003D9 RID: 985
			// (get) Token: 0x06001345 RID: 4933 RVA: 0x0004F41F File Offset: 0x0004D61F
			public override int AttributeCount
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x170003DA RID: 986
			// (get) Token: 0x06001346 RID: 4934 RVA: 0x0004F422 File Offset: 0x0004D622
			public override string BaseURI
			{
				get
				{
					return this.parentReader.BaseURI;
				}
			}

			// Token: 0x170003DB RID: 987
			// (get) Token: 0x06001347 RID: 4935 RVA: 0x0004F42F File Offset: 0x0004D62F
			public override bool CanReadBinaryContent
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003DC RID: 988
			// (get) Token: 0x06001348 RID: 4936 RVA: 0x0004F432 File Offset: 0x0004D632
			public override bool CanReadValueChunk
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003DD RID: 989
			// (get) Token: 0x06001349 RID: 4937 RVA: 0x0004F435 File Offset: 0x0004D635
			public override bool CanResolveEntity
			{
				get
				{
					return this.parentReader.CanResolveEntity;
				}
			}

			// Token: 0x0600134A RID: 4938 RVA: 0x0004F442 File Offset: 0x0004D642
			public override void Close()
			{
				this.CloseStreams();
				this.readState = ReadState.Closed;
			}

			// Token: 0x0600134B RID: 4939 RVA: 0x0004F451 File Offset: 0x0004D651
			private void CloseStreams()
			{
				if (this.binHexStream != null)
				{
					this.binHexStream.Close();
					this.binHexStream = null;
				}
			}

			// Token: 0x170003DE RID: 990
			// (get) Token: 0x0600134C RID: 4940 RVA: 0x0004F46D File Offset: 0x0004D66D
			public override int Depth
			{
				get
				{
					if (this.readState != ReadState.Interactive)
					{
						return this.parentReader.Depth;
					}
					return this.parentReader.Depth + 1;
				}
			}

			// Token: 0x170003DF RID: 991
			// (get) Token: 0x0600134D RID: 4941 RVA: 0x0004F491 File Offset: 0x0004D691
			public override bool EOF
			{
				get
				{
					return this.readState == ReadState.EndOfFile;
				}
			}

			// Token: 0x0600134E RID: 4942 RVA: 0x0004F49C File Offset: 0x0004D69C
			public override string GetAttribute(int index)
			{
				return null;
			}

			// Token: 0x0600134F RID: 4943 RVA: 0x0004F49F File Offset: 0x0004D69F
			public override string GetAttribute(string name)
			{
				return null;
			}

			// Token: 0x06001350 RID: 4944 RVA: 0x0004F4A2 File Offset: 0x0004D6A2
			public override string GetAttribute(string name, string ns)
			{
				return null;
			}

			// Token: 0x06001351 RID: 4945 RVA: 0x0004F4A5 File Offset: 0x0004D6A5
			public override string GetAttribute(XmlDictionaryString localName, XmlDictionaryString ns)
			{
				return null;
			}

			// Token: 0x170003E0 RID: 992
			// (get) Token: 0x06001352 RID: 4946 RVA: 0x0004F4A8 File Offset: 0x0004D6A8
			public override bool HasAttributes
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003E1 RID: 993
			// (get) Token: 0x06001353 RID: 4947 RVA: 0x0004F4AB File Offset: 0x0004D6AB
			public override bool HasValue
			{
				get
				{
					return this.readState == ReadState.Interactive;
				}
			}

			// Token: 0x170003E2 RID: 994
			// (get) Token: 0x06001354 RID: 4948 RVA: 0x0004F4B6 File Offset: 0x0004D6B6
			public override bool IsDefault
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003E3 RID: 995
			// (get) Token: 0x06001355 RID: 4949 RVA: 0x0004F4B9 File Offset: 0x0004D6B9
			public override bool IsEmptyElement
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06001356 RID: 4950 RVA: 0x0004F4BC File Offset: 0x0004D6BC
			public override bool IsLocalName(string localName)
			{
				return false;
			}

			// Token: 0x06001357 RID: 4951 RVA: 0x0004F4BF File Offset: 0x0004D6BF
			public override bool IsLocalName(XmlDictionaryString localName)
			{
				return false;
			}

			// Token: 0x06001358 RID: 4952 RVA: 0x0004F4C2 File Offset: 0x0004D6C2
			public override bool IsNamespaceUri(string ns)
			{
				return false;
			}

			// Token: 0x06001359 RID: 4953 RVA: 0x0004F4C5 File Offset: 0x0004D6C5
			public override bool IsNamespaceUri(XmlDictionaryString ns)
			{
				return false;
			}

			// Token: 0x0600135A RID: 4954 RVA: 0x0004F4C8 File Offset: 0x0004D6C8
			public override bool IsStartElement()
			{
				return false;
			}

			// Token: 0x0600135B RID: 4955 RVA: 0x0004F4CB File Offset: 0x0004D6CB
			public override bool IsStartElement(string localName)
			{
				return false;
			}

			// Token: 0x0600135C RID: 4956 RVA: 0x0004F4CE File Offset: 0x0004D6CE
			public override bool IsStartElement(string localName, string ns)
			{
				return false;
			}

			// Token: 0x0600135D RID: 4957 RVA: 0x0004F4D1 File Offset: 0x0004D6D1
			public override bool IsStartElement(XmlDictionaryString localName, XmlDictionaryString ns)
			{
				return false;
			}

			// Token: 0x170003E4 RID: 996
			// (get) Token: 0x0600135E RID: 4958 RVA: 0x0004F4D4 File Offset: 0x0004D6D4
			public override string LocalName
			{
				get
				{
					if (this.readState != ReadState.Interactive)
					{
						return this.parentReader.LocalName;
					}
					return string.Empty;
				}
			}

			// Token: 0x0600135F RID: 4959 RVA: 0x0004F4F0 File Offset: 0x0004D6F0
			public override string LookupNamespace(string ns)
			{
				return this.parentReader.LookupNamespace(ns);
			}

			// Token: 0x06001360 RID: 4960 RVA: 0x0004F4FE File Offset: 0x0004D6FE
			public override void MoveToAttribute(int index)
			{
			}

			// Token: 0x06001361 RID: 4961 RVA: 0x0004F500 File Offset: 0x0004D700
			public override bool MoveToAttribute(string name)
			{
				return false;
			}

			// Token: 0x06001362 RID: 4962 RVA: 0x0004F503 File Offset: 0x0004D703
			public override bool MoveToAttribute(string name, string ns)
			{
				return false;
			}

			// Token: 0x06001363 RID: 4963 RVA: 0x0004F506 File Offset: 0x0004D706
			public override bool MoveToElement()
			{
				return false;
			}

			// Token: 0x06001364 RID: 4964 RVA: 0x0004F509 File Offset: 0x0004D709
			public override bool MoveToFirstAttribute()
			{
				return false;
			}

			// Token: 0x06001365 RID: 4965 RVA: 0x0004F50C File Offset: 0x0004D70C
			public override bool MoveToNextAttribute()
			{
				return false;
			}

			// Token: 0x170003E5 RID: 997
			// (get) Token: 0x06001366 RID: 4966 RVA: 0x0004F50F File Offset: 0x0004D70F
			public override string Name
			{
				get
				{
					if (this.readState != ReadState.Interactive)
					{
						return this.parentReader.Name;
					}
					return string.Empty;
				}
			}

			// Token: 0x170003E6 RID: 998
			// (get) Token: 0x06001367 RID: 4967 RVA: 0x0004F52B File Offset: 0x0004D72B
			public override string NamespaceURI
			{
				get
				{
					if (this.readState != ReadState.Interactive)
					{
						return this.parentReader.NamespaceURI;
					}
					return string.Empty;
				}
			}

			// Token: 0x170003E7 RID: 999
			// (get) Token: 0x06001368 RID: 4968 RVA: 0x0004F547 File Offset: 0x0004D747
			public override XmlNameTable NameTable
			{
				get
				{
					return this.parentReader.NameTable;
				}
			}

			// Token: 0x170003E8 RID: 1000
			// (get) Token: 0x06001369 RID: 4969 RVA: 0x0004F554 File Offset: 0x0004D754
			public override string Prefix
			{
				get
				{
					if (this.readState != ReadState.Interactive)
					{
						return this.parentReader.Prefix;
					}
					return string.Empty;
				}
			}

			// Token: 0x170003E9 RID: 1001
			// (get) Token: 0x0600136A RID: 4970 RVA: 0x0004F570 File Offset: 0x0004D770
			public override char QuoteChar
			{
				get
				{
					return this.parentReader.QuoteChar;
				}
			}

			// Token: 0x0600136B RID: 4971 RVA: 0x0004F57D File Offset: 0x0004D77D
			public override bool ReadAttributeValue()
			{
				return false;
			}

			// Token: 0x0600136C RID: 4972 RVA: 0x0004F580 File Offset: 0x0004D780
			public override string ReadInnerXml()
			{
				return this.ReadContentAsString();
			}

			// Token: 0x0600136D RID: 4973 RVA: 0x0004F588 File Offset: 0x0004D788
			public override string ReadOuterXml()
			{
				return this.ReadContentAsString();
			}

			// Token: 0x170003EA RID: 1002
			// (get) Token: 0x0600136E RID: 4974 RVA: 0x0004F590 File Offset: 0x0004D790
			public override ReadState ReadState
			{
				get
				{
					return this.readState;
				}
			}

			// Token: 0x0600136F RID: 4975 RVA: 0x0004F598 File Offset: 0x0004D798
			public override void ResolveEntity()
			{
			}

			// Token: 0x170003EB RID: 1003
			// (get) Token: 0x06001370 RID: 4976 RVA: 0x0004F59A File Offset: 0x0004D79A
			public override XmlReaderSettings Settings
			{
				get
				{
					return this.parentReader.Settings;
				}
			}

			// Token: 0x06001371 RID: 4977 RVA: 0x0004F5A7 File Offset: 0x0004D7A7
			public override void Skip()
			{
				this.Read();
			}

			// Token: 0x170003EC RID: 1004
			public override string this[int index]
			{
				get
				{
					return null;
				}
			}

			// Token: 0x170003ED RID: 1005
			public override string this[string name]
			{
				get
				{
					return null;
				}
			}

			// Token: 0x170003EE RID: 1006
			public override string this[string name, string ns]
			{
				get
				{
					return null;
				}
			}

			// Token: 0x170003EF RID: 1007
			// (get) Token: 0x06001375 RID: 4981 RVA: 0x0004F5B9 File Offset: 0x0004D7B9
			public override string XmlLang
			{
				get
				{
					return this.parentReader.XmlLang;
				}
			}

			// Token: 0x170003F0 RID: 1008
			// (get) Token: 0x06001376 RID: 4982 RVA: 0x0004F5C6 File Offset: 0x0004D7C6
			public override XmlSpace XmlSpace
			{
				get
				{
					return this.parentReader.XmlSpace;
				}
			}

			// Token: 0x170003F1 RID: 1009
			// (get) Token: 0x06001377 RID: 4983 RVA: 0x0004F5D3 File Offset: 0x0004D7D3
			public override Type ValueType
			{
				get
				{
					if (this.readState != ReadState.Interactive)
					{
						return this.parentReader.ValueType;
					}
					return typeof(byte[]);
				}
			}

			// Token: 0x06001378 RID: 4984 RVA: 0x0004F5F4 File Offset: 0x0004D7F4
			bool IXmlLineInfo.HasLineInfo()
			{
				return ((IXmlLineInfo)this.parentReader).HasLineInfo();
			}

			// Token: 0x170003F2 RID: 1010
			// (get) Token: 0x06001379 RID: 4985 RVA: 0x0004F606 File Offset: 0x0004D806
			int IXmlLineInfo.LineNumber
			{
				get
				{
					return ((IXmlLineInfo)this.parentReader).LineNumber;
				}
			}

			// Token: 0x170003F3 RID: 1011
			// (get) Token: 0x0600137A RID: 4986 RVA: 0x0004F618 File Offset: 0x0004D818
			int IXmlLineInfo.LinePosition
			{
				get
				{
					return ((IXmlLineInfo)this.parentReader).LinePosition;
				}
			}

			// Token: 0x04000960 RID: 2400
			private int chunkSize = 4096;

			// Token: 0x04000961 RID: 2401
			private int bytesRemaining;

			// Token: 0x04000962 RID: 2402
			private XmlMtomReader.MimePart part;

			// Token: 0x04000963 RID: 2403
			private ReadState readState;

			// Token: 0x04000964 RID: 2404
			private XmlDictionaryReader parentReader;

			// Token: 0x04000965 RID: 2405
			private string stringValue;

			// Token: 0x04000966 RID: 2406
			private int stringOffset;

			// Token: 0x04000967 RID: 2407
			private XmlNodeType nodeType;

			// Token: 0x04000968 RID: 2408
			private MemoryStream binHexStream;

			// Token: 0x04000969 RID: 2409
			private byte[] valueBuffer;

			// Token: 0x0400096A RID: 2410
			private int valueOffset;

			// Token: 0x0400096B RID: 2411
			private int valueCount;

			// Token: 0x0400096C RID: 2412
			private bool finishedStream;
		}
	}
}
