using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000118 RID: 280
	internal class XmlJsonWriter : XmlDictionaryWriter, IXmlJsonWriterInitializer
	{
		// Token: 0x060010AB RID: 4267 RVA: 0x00044F9A File Offset: 0x0004319A
		public XmlJsonWriter()
			: this(false, null)
		{
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00044FA4 File Offset: 0x000431A4
		public XmlJsonWriter(bool indent, string indentChars)
		{
			this.indent = indent;
			if (indent)
			{
				if (indentChars == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("indentChars");
				}
				this.indentChars = indentChars;
			}
			this.InitializeWriter();
			if (XmlJsonWriter.CharacterAbbrevs == null)
			{
				XmlJsonWriter.CharacterAbbrevs = XmlJsonWriter.GetCharacterAbbrevs();
			}
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x00044FE4 File Offset: 0x000431E4
		private static char[] GetCharacterAbbrevs()
		{
			char[] array = new char[32];
			for (int i = 0; i < 32; i++)
			{
				char c;
				if (!LocalAppContextSwitches.DoNotUseEcmaScriptV6EscapeControlCharacter && XmlJsonWriter.TryEscapeControlCharacter((char)i, out c))
				{
					array[i] = c;
				}
				else
				{
					array[i] = '\0';
				}
			}
			return array;
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x00045024 File Offset: 0x00043224
		private static bool TryEscapeControlCharacter(char ch, out char abbrev)
		{
			switch (ch)
			{
			case '\b':
				abbrev = 'b';
				return true;
			case '\t':
				abbrev = 't';
				return true;
			case '\n':
				abbrev = 'n';
				return true;
			case '\f':
				abbrev = 'f';
				return true;
			case '\r':
				abbrev = 'r';
				return true;
			}
			abbrev = ' ';
			return false;
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x00045078 File Offset: 0x00043278
		public override XmlWriterSettings Settings
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x0004507C File Offset: 0x0004327C
		public override WriteState WriteState
		{
			get
			{
				if (this.writeState == WriteState.Closed)
				{
					return WriteState.Closed;
				}
				if (this.HasOpenAttribute)
				{
					return WriteState.Attribute;
				}
				switch (this.nodeType)
				{
				case JsonNodeType.None:
					return WriteState.Start;
				case JsonNodeType.Element:
					return WriteState.Element;
				case JsonNodeType.EndElement:
				case JsonNodeType.QuotedText:
				case JsonNodeType.StandaloneText:
					return WriteState.Content;
				}
				return WriteState.Error;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x000450CC File Offset: 0x000432CC
		public override string XmlLang
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x000450CF File Offset: 0x000432CF
		public override XmlSpace XmlSpace
		{
			get
			{
				return XmlSpace.None;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x000450D2 File Offset: 0x000432D2
		private static BinHexEncoding BinHexEncoding
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlJsonWriter.binHexEncoding == null)
				{
					XmlJsonWriter.binHexEncoding = new BinHexEncoding();
				}
				return XmlJsonWriter.binHexEncoding;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x000450EA File Offset: 0x000432EA
		private bool HasOpenAttribute
		{
			get
			{
				return this.isWritingDataTypeAttribute || this.isWritingServerTypeAttribute || this.IsWritingNameAttribute || this.isWritingXmlnsAttribute;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x0004510C File Offset: 0x0004330C
		private bool IsClosed
		{
			get
			{
				return this.WriteState == WriteState.Closed;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060010B6 RID: 4278 RVA: 0x00045117 File Offset: 0x00043317
		private bool IsWritingCollection
		{
			get
			{
				return this.depth > 0 && this.scopes[this.depth] == JsonNodeType.Collection;
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x00045134 File Offset: 0x00043334
		private bool IsWritingNameAttribute
		{
			get
			{
				return (this.nameState & XmlJsonWriter.NameState.IsWritingNameAttribute) == XmlJsonWriter.NameState.IsWritingNameAttribute;
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x060010B8 RID: 4280 RVA: 0x00045141 File Offset: 0x00043341
		private bool IsWritingNameWithMapping
		{
			get
			{
				return (this.nameState & XmlJsonWriter.NameState.IsWritingNameWithMapping) == XmlJsonWriter.NameState.IsWritingNameWithMapping;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x060010B9 RID: 4281 RVA: 0x0004514E File Offset: 0x0004334E
		private bool WrittenNameWithMapping
		{
			get
			{
				return (this.nameState & XmlJsonWriter.NameState.WrittenNameWithMapping) == XmlJsonWriter.NameState.WrittenNameWithMapping;
			}
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x0004515C File Offset: 0x0004335C
		public override void Close()
		{
			if (!this.IsClosed)
			{
				try
				{
					this.WriteEndDocument();
				}
				finally
				{
					try
					{
						this.nodeWriter.Flush();
						this.nodeWriter.Close();
					}
					finally
					{
						this.writeState = WriteState.Closed;
						if (this.depth != 0)
						{
							this.depth = 0;
						}
					}
				}
			}
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x000451C8 File Offset: 0x000433C8
		public override void Flush()
		{
			if (this.IsClosed)
			{
				XmlJsonWriter.ThrowClosed();
			}
			this.nodeWriter.Flush();
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x000451E4 File Offset: 0x000433E4
		public override string LookupPrefix(string ns)
		{
			if (ns == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("ns");
			}
			if (ns == "http://www.w3.org/2000/xmlns/")
			{
				return "xmlns";
			}
			if (ns == "http://www.w3.org/XML/1998/namespace")
			{
				return "xml";
			}
			if (ns == string.Empty)
			{
				return string.Empty;
			}
			return null;
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x0004523C File Offset: 0x0004343C
		public void SetOutput(Stream stream, Encoding encoding, bool ownsStream)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
			}
			if (encoding == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
			}
			if (encoding.WebName != Encoding.UTF8.WebName)
			{
				stream = new JsonEncodingStreamWrapper(stream, encoding, false);
			}
			else
			{
				encoding = null;
			}
			if (this.nodeWriter == null)
			{
				this.nodeWriter = new XmlJsonWriter.JsonNodeWriter();
			}
			this.nodeWriter.SetOutput(stream, ownsStream, encoding);
			this.InitializeWriter();
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x000452B2 File Offset: 0x000434B2
		public override void WriteArray(string prefix, string localName, string namespaceUri, bool[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x000452C8 File Offset: 0x000434C8
		public override void WriteArray(string prefix, string localName, string namespaceUri, short[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x000452DE File Offset: 0x000434DE
		public override void WriteArray(string prefix, string localName, string namespaceUri, int[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x000452F4 File Offset: 0x000434F4
		public override void WriteArray(string prefix, string localName, string namespaceUri, long[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0004530A File Offset: 0x0004350A
		public override void WriteArray(string prefix, string localName, string namespaceUri, float[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00045320 File Offset: 0x00043520
		public override void WriteArray(string prefix, string localName, string namespaceUri, double[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00045336 File Offset: 0x00043536
		public override void WriteArray(string prefix, string localName, string namespaceUri, decimal[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x0004534C File Offset: 0x0004354C
		public override void WriteArray(string prefix, string localName, string namespaceUri, DateTime[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x00045362 File Offset: 0x00043562
		public override void WriteArray(string prefix, string localName, string namespaceUri, Guid[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x00045378 File Offset: 0x00043578
		public override void WriteArray(string prefix, string localName, string namespaceUri, TimeSpan[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0004538E File Offset: 0x0004358E
		public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, bool[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x000453A4 File Offset: 0x000435A4
		public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, decimal[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x000453BA File Offset: 0x000435BA
		public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, double[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x000453D0 File Offset: 0x000435D0
		public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, float[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x000453E6 File Offset: 0x000435E6
		public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, int[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x000453FC File Offset: 0x000435FC
		public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, long[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x00045412 File Offset: 0x00043612
		public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, short[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00045428 File Offset: 0x00043628
		public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, DateTime[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0004543E File Offset: 0x0004363E
		public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, Guid[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00045454 File Offset: 0x00043654
		public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, TimeSpan[] array, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0004546C File Offset: 0x0004366C
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			if (index < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - index)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("JSON size exceeded remaining buffer space, by {0} byte(s).", new object[] { buffer.Length - index })));
			}
			this.StartText();
			this.nodeWriter.WriteBase64Text(buffer, 0, buffer, index, count);
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00045510 File Offset: 0x00043710
		public override void WriteBinHex(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			if (index < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - index)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("JSON size exceeded remaining buffer space, by {0} byte(s).", new object[] { buffer.Length - index })));
			}
			this.StartText();
			this.WriteEscapedJsonString(XmlJsonWriter.BinHexEncoding.GetString(buffer, index, count));
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x000455B5 File Offset: 0x000437B5
		public override void WriteCData(string text)
		{
			this.WriteString(text);
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x000455BE File Offset: 0x000437BE
		public override void WriteCharEntity(char ch)
		{
			this.WriteString(ch.ToString());
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x000455D0 File Offset: 0x000437D0
		public override void WriteChars(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			if (index < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - index)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("JSON size exceeded remaining buffer space, by {0} byte(s).", new object[] { buffer.Length - index })));
			}
			this.WriteString(new string(buffer, index, count));
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0004566A File Offset: 0x0004386A
		public override void WriteComment(string text)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", new object[] { "WriteComment" })));
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x0004568E File Offset: 0x0004388E
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", new object[] { "WriteDocType" })));
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x000456B4 File Offset: 0x000438B4
		public override void WriteEndAttribute()
		{
			if (this.IsClosed)
			{
				XmlJsonWriter.ThrowClosed();
			}
			if (!this.HasOpenAttribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("WriteEndAttribute was called while there is no open attribute.")));
			}
			if (this.isWritingDataTypeAttribute)
			{
				string text = this.attributeText;
				if (!(text == "number"))
				{
					if (!(text == "string"))
					{
						if (!(text == "array"))
						{
							if (!(text == "object"))
							{
								if (!(text == "null"))
								{
									if (!(text == "boolean"))
									{
										throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Unexpected attribute value '{0}'.", new object[] { this.attributeText })));
									}
									this.ThrowIfServerTypeWritten("boolean");
									this.dataType = XmlJsonWriter.JsonDataType.Boolean;
								}
								else
								{
									this.ThrowIfServerTypeWritten("null");
									this.dataType = XmlJsonWriter.JsonDataType.Null;
								}
							}
							else
							{
								this.dataType = XmlJsonWriter.JsonDataType.Object;
							}
						}
						else
						{
							this.ThrowIfServerTypeWritten("array");
							this.dataType = XmlJsonWriter.JsonDataType.Array;
						}
					}
					else
					{
						this.ThrowIfServerTypeWritten("string");
						this.dataType = XmlJsonWriter.JsonDataType.String;
					}
				}
				else
				{
					this.ThrowIfServerTypeWritten("number");
					this.dataType = XmlJsonWriter.JsonDataType.Number;
				}
				this.attributeText = null;
				this.isWritingDataTypeAttribute = false;
				if (!this.IsWritingNameWithMapping || this.WrittenNameWithMapping)
				{
					this.WriteDataTypeServerType();
					return;
				}
			}
			else if (this.isWritingServerTypeAttribute)
			{
				this.serverTypeValue = this.attributeText;
				this.attributeText = null;
				this.isWritingServerTypeAttribute = false;
				if ((!this.IsWritingNameWithMapping || this.WrittenNameWithMapping) && this.dataType == XmlJsonWriter.JsonDataType.Object)
				{
					this.WriteServerTypeAttribute();
					return;
				}
			}
			else
			{
				if (this.IsWritingNameAttribute)
				{
					this.WriteJsonElementName(this.attributeText);
					this.attributeText = null;
					this.nameState = XmlJsonWriter.NameState.IsWritingNameWithMapping | XmlJsonWriter.NameState.WrittenNameWithMapping;
					this.WriteDataTypeServerType();
					return;
				}
				if (this.isWritingXmlnsAttribute)
				{
					if (!string.IsNullOrEmpty(this.attributeText) && this.isWritingXmlnsAttributeDefaultNs)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("ns", SR.GetString("JSON namespace is specified as '{0}' but it must be empty.", new object[] { this.attributeText }));
					}
					this.attributeText = null;
					this.isWritingXmlnsAttribute = false;
					this.isWritingXmlnsAttributeDefaultNs = false;
				}
			}
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x000458CD File Offset: 0x00043ACD
		public override void WriteEndDocument()
		{
			if (this.IsClosed)
			{
				XmlJsonWriter.ThrowClosed();
			}
			if (this.nodeType != JsonNodeType.None)
			{
				while (this.depth > 0)
				{
					this.WriteEndElement();
				}
			}
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x000458F8 File Offset: 0x00043AF8
		public override void WriteEndElement()
		{
			if (this.IsClosed)
			{
				XmlJsonWriter.ThrowClosed();
			}
			if (this.depth == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Encountered an end element while there was no open element in JSON writer.")));
			}
			if (this.HasOpenAttribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute must be closed first before calling {0} method.", new object[] { "WriteEndElement" })));
			}
			this.endElementBuffer = false;
			JsonNodeType jsonNodeType = this.ExitScope();
			if (jsonNodeType == JsonNodeType.Collection)
			{
				this.indentLevel--;
				if (this.indent)
				{
					if (this.nodeType == JsonNodeType.Element)
					{
						this.nodeWriter.WriteText(32);
					}
					else
					{
						this.WriteNewLine();
						this.WriteIndent();
					}
				}
				this.nodeWriter.WriteText(93);
				jsonNodeType = this.ExitScope();
			}
			else if (this.nodeType == JsonNodeType.QuotedText)
			{
				this.WriteJsonQuote();
			}
			else if (this.nodeType == JsonNodeType.Element)
			{
				if (this.dataType == XmlJsonWriter.JsonDataType.None && this.serverTypeValue != null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("On JSON writer data type '{0}' must be specified. Object string is '{1}', server type string is '{2}'.", new object[] { "type", "object", "__type" })));
				}
				if (this.IsWritingNameWithMapping && !this.WrittenNameWithMapping)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("On JSON writer data type '{0}' must be specified. Object string is '{1}', server type string is '{2}'.", new object[]
					{
						"item",
						string.Empty,
						"item"
					})));
				}
				if (this.dataType == XmlJsonWriter.JsonDataType.None || this.dataType == XmlJsonWriter.JsonDataType.String)
				{
					this.nodeWriter.WriteText(34);
					this.nodeWriter.WriteText(34);
				}
			}
			if (this.depth != 0)
			{
				if (jsonNodeType == JsonNodeType.Element)
				{
					this.endElementBuffer = true;
				}
				else if (jsonNodeType == JsonNodeType.Object)
				{
					this.indentLevel--;
					if (this.indent)
					{
						if (this.nodeType == JsonNodeType.Element)
						{
							this.nodeWriter.WriteText(32);
						}
						else
						{
							this.WriteNewLine();
							this.WriteIndent();
						}
					}
					this.nodeWriter.WriteText(125);
					if (this.depth > 0 && this.scopes[this.depth] == JsonNodeType.Element)
					{
						this.ExitScope();
						this.endElementBuffer = true;
					}
				}
			}
			this.dataType = XmlJsonWriter.JsonDataType.None;
			this.nodeType = JsonNodeType.EndElement;
			this.nameState = XmlJsonWriter.NameState.None;
			this.wroteServerTypeAttribute = false;
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00045B33 File Offset: 0x00043D33
		public override void WriteEntityRef(string name)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", new object[] { "WriteEntityRef" })));
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00045B57 File Offset: 0x00043D57
		public override void WriteFullEndElement()
		{
			this.WriteEndElement();
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00045B60 File Offset: 0x00043D60
		public override void WriteProcessingInstruction(string name, string text)
		{
			if (this.IsClosed)
			{
				XmlJsonWriter.ThrowClosed();
			}
			if (!name.Equals("xml", StringComparison.OrdinalIgnoreCase))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("processing instruction is not supported in JSON writer."), "name"));
			}
			if (this.WriteState != WriteState.Start)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Attempt to write invalid XML declration.")));
			}
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00045BBF File Offset: 0x00043DBF
		public override void WriteQualifiedName(string localName, string ns)
		{
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
			}
			if (localName.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("localName", SR.GetString("Empty string is invalid as a local name."));
			}
			if (ns == null)
			{
				ns = string.Empty;
			}
			base.WriteQualifiedName(localName, ns);
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00045BFE File Offset: 0x00043DFE
		public override void WriteRaw(string data)
		{
			this.WriteString(data);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00045C08 File Offset: 0x00043E08
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			if (index < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - index)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("JSON size exceeded remaining buffer space, by {0} byte(s).", new object[] { buffer.Length - index })));
			}
			this.WriteString(new string(buffer, index, count));
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00045CA4 File Offset: 0x00043EA4
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (this.IsClosed)
			{
				XmlJsonWriter.ThrowClosed();
			}
			if (!string.IsNullOrEmpty(prefix))
			{
				if (!this.IsWritingNameWithMapping || !(prefix == "xmlns"))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("prefix", SR.GetString("JSON prefix must be null or empty. '{0}' is specified instead.", new object[] { prefix }));
				}
				if (ns != null && ns != "http://www.w3.org/2000/xmlns/")
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The prefix '{0}' is bound to the namespace '{1}' and cannot be changed to '{2}'.", new object[] { "xmlns", "http://www.w3.org/2000/xmlns/", ns }), "ns"));
				}
			}
			else if (this.IsWritingNameWithMapping && ns == "http://www.w3.org/2000/xmlns/" && localName != "xmlns")
			{
				prefix = "xmlns";
			}
			if (!string.IsNullOrEmpty(ns))
			{
				if (this.IsWritingNameWithMapping && ns == "http://www.w3.org/2000/xmlns/")
				{
					prefix = "xmlns";
				}
				else
				{
					if (!string.IsNullOrEmpty(prefix) || !(localName == "xmlns") || !(ns == "http://www.w3.org/2000/xmlns/"))
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("ns", SR.GetString("JSON namespace is specified as '{0}' but it must be empty.", new object[] { ns }));
					}
					prefix = "xmlns";
					this.isWritingXmlnsAttributeDefaultNs = true;
				}
			}
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
			}
			if (localName.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("localName", SR.GetString("Empty string is invalid as a local name."));
			}
			if (this.nodeType != JsonNodeType.Element && !this.wroteServerTypeAttribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute must have an owner element.")));
			}
			if (this.HasOpenAttribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute must be closed first before calling {0} method.", new object[] { "WriteStartAttribute" })));
			}
			if (prefix == "xmlns")
			{
				this.isWritingXmlnsAttribute = true;
				return;
			}
			if (localName == "type")
			{
				if (this.dataType != XmlJsonWriter.JsonDataType.None)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute '{0}' is already written.", new object[] { "type" })));
				}
				this.isWritingDataTypeAttribute = true;
				return;
			}
			else if (localName == "__type")
			{
				if (this.serverTypeValue != null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute '{0}' is already written.", new object[] { "__type" })));
				}
				if (this.dataType != XmlJsonWriter.JsonDataType.None && this.dataType != XmlJsonWriter.JsonDataType.Object)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Server type is specified for invalid data type in JSON. Server type: '{0}', type: '{1}', dataType: '{2}', object: '{3}'.", new object[]
					{
						"__type",
						"type",
						this.dataType.ToString().ToLowerInvariant(),
						"object"
					})));
				}
				this.isWritingServerTypeAttribute = true;
				return;
			}
			else
			{
				if (!(localName == "item"))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("localName", SR.GetString("Unexpected attribute local name '{0}'.", new object[] { localName }));
				}
				if (this.WrittenNameWithMapping)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute '{0}' is already written.", new object[] { "item" })));
				}
				if (!this.IsWritingNameWithMapping)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Encountered an end element while there was no open element in JSON writer.")));
				}
				this.nameState |= XmlJsonWriter.NameState.IsWritingNameAttribute;
				return;
			}
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00045FD4 File Offset: 0x000441D4
		public override void WriteStartDocument(bool standalone)
		{
			this.WriteStartDocument();
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00045FDC File Offset: 0x000441DC
		public override void WriteStartDocument()
		{
			if (this.IsClosed)
			{
				XmlJsonWriter.ThrowClosed();
			}
			if (this.WriteState != WriteState.Start)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid write state {1} for '{0}' method.", new object[]
				{
					"WriteStartDocument",
					this.WriteState.ToString()
				})));
			}
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00046038 File Offset: 0x00044238
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (localName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
			}
			if (localName.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("localName", SR.GetString("Empty string is invalid as a local name."));
			}
			if (!string.IsNullOrEmpty(prefix) && (string.IsNullOrEmpty(ns) || !this.TrySetWritingNameWithMapping(localName, ns)))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("prefix", SR.GetString("JSON prefix must be null or empty. '{0}' is specified instead.", new object[] { prefix }));
			}
			if (!string.IsNullOrEmpty(ns) && !this.TrySetWritingNameWithMapping(localName, ns))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("ns", SR.GetString("JSON namespace is specified as '{0}' but it must be empty.", new object[] { ns }));
			}
			if (this.IsClosed)
			{
				XmlJsonWriter.ThrowClosed();
			}
			if (this.HasOpenAttribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute must be closed first before calling {0} method.", new object[] { "WriteStartElement" })));
			}
			if (this.nodeType != JsonNodeType.None && this.depth == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Multiple root element is not allowed on JSON writer.")));
			}
			switch (this.nodeType)
			{
			case JsonNodeType.None:
				if (!localName.Equals("root"))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid root element name '{0}' (root element is '{1}' in JSON).", new object[] { localName, "root" })));
				}
				this.EnterScope(JsonNodeType.Element);
				goto IL_027E;
			case JsonNodeType.Element:
				if (this.dataType != XmlJsonWriter.JsonDataType.Array && this.dataType != XmlJsonWriter.JsonDataType.Object)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Either Object or Array of JSON node type must be specified.")));
				}
				if (this.indent)
				{
					this.WriteNewLine();
					this.WriteIndent();
				}
				if (!this.IsWritingCollection)
				{
					if (this.nameState != XmlJsonWriter.NameState.IsWritingNameWithMapping)
					{
						this.WriteJsonElementName(localName);
					}
				}
				else if (!localName.Equals("item"))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid JSON item name '{0}' for array element (item element is '{1}' in JSON).", new object[] { localName, "item" })));
				}
				this.EnterScope(JsonNodeType.Element);
				goto IL_027E;
			case JsonNodeType.EndElement:
				if (this.endElementBuffer)
				{
					this.nodeWriter.WriteText(44);
				}
				if (this.indent)
				{
					this.WriteNewLine();
					this.WriteIndent();
				}
				if (!this.IsWritingCollection)
				{
					if (this.nameState != XmlJsonWriter.NameState.IsWritingNameWithMapping)
					{
						this.WriteJsonElementName(localName);
					}
				}
				else if (!localName.Equals("item"))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid JSON item name '{0}' for array element (item element is '{1}' in JSON).", new object[] { localName, "item" })));
				}
				this.EnterScope(JsonNodeType.Element);
				goto IL_027E;
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid call to JSON WriteStartElement method.")));
			IL_027E:
			this.isWritingDataTypeAttribute = false;
			this.isWritingServerTypeAttribute = false;
			this.isWritingXmlnsAttribute = false;
			this.wroteServerTypeAttribute = false;
			this.serverTypeValue = null;
			this.dataType = XmlJsonWriter.JsonDataType.None;
			this.nodeType = JsonNodeType.Element;
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x000462F4 File Offset: 0x000444F4
		public override void WriteString(string text)
		{
			if (this.HasOpenAttribute && text != null)
			{
				this.attributeText += text;
				return;
			}
			if (text == null)
			{
				text = string.Empty;
			}
			if ((this.dataType != XmlJsonWriter.JsonDataType.Array && this.dataType != XmlJsonWriter.JsonDataType.Object && this.nodeType != JsonNodeType.EndElement) || !XmlConverter.IsWhitespace(text))
			{
				this.StartText();
				this.WriteEscapedJsonString(text);
			}
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00046359 File Offset: 0x00044559
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.WriteString(highChar + lowChar);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00046372 File Offset: 0x00044572
		public override void WriteValue(bool value)
		{
			this.StartText();
			this.nodeWriter.WriteBoolText(value);
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00046386 File Offset: 0x00044586
		public override void WriteValue(decimal value)
		{
			this.StartText();
			this.nodeWriter.WriteDecimalText(value);
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0004639A File Offset: 0x0004459A
		public override void WriteValue(double value)
		{
			this.StartText();
			this.nodeWriter.WriteDoubleText(value);
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x000463AE File Offset: 0x000445AE
		public override void WriteValue(float value)
		{
			this.StartText();
			this.nodeWriter.WriteFloatText(value);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x000463C2 File Offset: 0x000445C2
		public override void WriteValue(int value)
		{
			this.StartText();
			this.nodeWriter.WriteInt32Text(value);
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x000463D6 File Offset: 0x000445D6
		public override void WriteValue(long value)
		{
			this.StartText();
			this.nodeWriter.WriteInt64Text(value);
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x000463EA File Offset: 0x000445EA
		public override void WriteValue(Guid value)
		{
			this.StartText();
			this.nodeWriter.WriteGuidText(value);
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x000463FE File Offset: 0x000445FE
		public override void WriteValue(DateTime value)
		{
			this.StartText();
			this.nodeWriter.WriteDateTimeText(value);
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00046412 File Offset: 0x00044612
		public override void WriteValue(string value)
		{
			this.WriteString(value);
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x0004641B File Offset: 0x0004461B
		public override void WriteValue(TimeSpan value)
		{
			this.StartText();
			this.nodeWriter.WriteTimeSpanText(value);
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0004642F File Offset: 0x0004462F
		public override void WriteValue(UniqueId value)
		{
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
			}
			this.StartText();
			this.nodeWriter.WriteUniqueIdText(value);
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00046458 File Offset: 0x00044658
		public override void WriteValue(object value)
		{
			if (this.IsClosed)
			{
				XmlJsonWriter.ThrowClosed();
			}
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
			}
			if (value is Array)
			{
				this.WriteValue((Array)value);
				return;
			}
			if (value is IStreamProvider)
			{
				this.WriteValue((IStreamProvider)value);
				return;
			}
			this.WritePrimitiveValue(value);
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x000464B4 File Offset: 0x000446B4
		public override void WriteWhitespace(string ws)
		{
			if (this.IsClosed)
			{
				XmlJsonWriter.ThrowClosed();
			}
			if (ws == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("ws");
			}
			foreach (char c in ws)
			{
				if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("ws", SR.GetString("Only whitespace characters are allowed for {1} method. The specified value is '{0}'", new object[]
					{
						c.ToString(),
						"WriteWhitespace"
					}));
				}
			}
			this.WriteString(ws);
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0004653D File Offset: 0x0004473D
		public override void WriteXmlAttribute(string localName, string value)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", new object[] { "WriteXmlAttribute" })));
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00046561 File Offset: 0x00044761
		public override void WriteXmlAttribute(XmlDictionaryString localName, XmlDictionaryString value)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", new object[] { "WriteXmlAttribute" })));
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00046585 File Offset: 0x00044785
		public override void WriteXmlnsAttribute(string prefix, string namespaceUri)
		{
			if (!this.IsWritingNameWithMapping)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", new object[] { "WriteXmlnsAttribute" })));
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x000465B2 File Offset: 0x000447B2
		public override void WriteXmlnsAttribute(string prefix, XmlDictionaryString namespaceUri)
		{
			if (!this.IsWritingNameWithMapping)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", new object[] { "WriteXmlnsAttribute" })));
			}
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x000465DF File Offset: 0x000447DF
		internal static bool CharacterNeedsEscaping(char ch)
		{
			return ch == '/' || ch == '"' || ch < ' ' || ch == '\\' || (ch >= '\ud800' && (ch <= '\udfff' || ch >= '\ufffe'));
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00046616 File Offset: 0x00044816
		private static void ThrowClosed()
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("JSON writer is already closed.")));
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0004662C File Offset: 0x0004482C
		private void CheckText(JsonNodeType nextNodeType)
		{
			if (this.IsClosed)
			{
				XmlJsonWriter.ThrowClosed();
			}
			if (this.depth == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Text cannot be written outside the root element.")));
			}
			if (nextNodeType == JsonNodeType.StandaloneText && this.nodeType == JsonNodeType.QuotedText)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON writer cannot write standalone text after quoted text.")));
			}
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00046688 File Offset: 0x00044888
		private void EnterScope(JsonNodeType currentNodeType)
		{
			this.depth++;
			if (this.scopes == null)
			{
				this.scopes = new JsonNodeType[4];
			}
			else if (this.scopes.Length == this.depth)
			{
				JsonNodeType[] array = new JsonNodeType[this.depth * 2];
				Array.Copy(this.scopes, array, this.depth);
				this.scopes = array;
			}
			this.scopes[this.depth] = currentNodeType;
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x000466FE File Offset: 0x000448FE
		private JsonNodeType ExitScope()
		{
			JsonNodeType jsonNodeType = this.scopes[this.depth];
			this.scopes[this.depth] = JsonNodeType.None;
			this.depth--;
			return jsonNodeType;
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0004672C File Offset: 0x0004492C
		private void InitializeWriter()
		{
			this.nodeType = JsonNodeType.None;
			this.dataType = XmlJsonWriter.JsonDataType.None;
			this.isWritingDataTypeAttribute = false;
			this.wroteServerTypeAttribute = false;
			this.isWritingServerTypeAttribute = false;
			this.serverTypeValue = null;
			this.attributeText = null;
			if (this.depth != 0)
			{
				this.depth = 0;
			}
			if (this.scopes != null && this.scopes.Length > 25)
			{
				this.scopes = null;
			}
			this.writeState = WriteState.Start;
			this.endElementBuffer = false;
			this.indentLevel = 0;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x000467A9 File Offset: 0x000449A9
		private static bool IsUnicodeNewlineCharacter(char c)
		{
			return c == '\u0085' || c == '\u2028' || c == '\u2029';
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x000467C8 File Offset: 0x000449C8
		private void StartText()
		{
			if (this.HasOpenAttribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("On JSON writer WriteString must be used for writing attribute values.")));
			}
			if (this.dataType == XmlJsonWriter.JsonDataType.None && this.serverTypeValue != null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("On JSON writer data type '{0}' must be specified. Object string is '{1}', server type string is '{2}'.", new object[] { "type", "object", "__type" })));
			}
			if (this.IsWritingNameWithMapping && !this.WrittenNameWithMapping)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("On JSON writer data type '{0}' must be specified. Object string is '{1}', server type string is '{2}'.", new object[]
				{
					"item",
					string.Empty,
					"item"
				})));
			}
			if (this.dataType == XmlJsonWriter.JsonDataType.String || this.dataType == XmlJsonWriter.JsonDataType.None)
			{
				this.CheckText(JsonNodeType.QuotedText);
				if (this.nodeType != JsonNodeType.QuotedText)
				{
					this.WriteJsonQuote();
				}
				this.nodeType = JsonNodeType.QuotedText;
				return;
			}
			if (this.dataType == XmlJsonWriter.JsonDataType.Number || this.dataType == XmlJsonWriter.JsonDataType.Boolean)
			{
				this.CheckText(JsonNodeType.StandaloneText);
				this.nodeType = JsonNodeType.StandaloneText;
				return;
			}
			this.ThrowInvalidAttributeContent();
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x000468D0 File Offset: 0x00044AD0
		private void ThrowIfServerTypeWritten(string dataTypeSpecified)
		{
			if (this.serverTypeValue != null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("The specified data type is invalid for server type. Type: '{0}', specified data type: '{1}', server type: '{2}', object '{3}'.", new object[] { "type", dataTypeSpecified, "__type", "object" })));
			}
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0004691C File Offset: 0x00044B1C
		private void ThrowInvalidAttributeContent()
		{
			if (this.HasOpenAttribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid method call state between start and end attribute.")));
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON writer cannot write text after non-text attribute. Data type is '{0}'.", new object[] { this.dataType.ToString().ToLowerInvariant() })));
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00046979 File Offset: 0x00044B79
		private bool TrySetWritingNameWithMapping(string localName, string ns)
		{
			if (localName.Equals("item") && ns.Equals("item"))
			{
				this.nameState = XmlJsonWriter.NameState.IsWritingNameWithMapping;
				return true;
			}
			return false;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x000469A0 File Offset: 0x00044BA0
		private void WriteDataTypeServerType()
		{
			if (this.dataType != XmlJsonWriter.JsonDataType.None)
			{
				XmlJsonWriter.JsonDataType jsonDataType = this.dataType;
				if (jsonDataType != XmlJsonWriter.JsonDataType.Null)
				{
					if (jsonDataType != XmlJsonWriter.JsonDataType.Object)
					{
						if (jsonDataType == XmlJsonWriter.JsonDataType.Array)
						{
							this.EnterScope(JsonNodeType.Collection);
							this.nodeWriter.WriteText(91);
							this.indentLevel++;
						}
					}
					else
					{
						this.EnterScope(JsonNodeType.Object);
						this.nodeWriter.WriteText(123);
						this.indentLevel++;
					}
				}
				else
				{
					this.nodeWriter.WriteText("null");
				}
				if (this.serverTypeValue != null)
				{
					this.WriteServerTypeAttribute();
				}
			}
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00046A30 File Offset: 0x00044C30
		[SecuritySafeCritical]
		private unsafe void WriteEscapedJsonString(string str)
		{
			fixed (string text = str)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				int num = 0;
				int i;
				for (i = 0; i < str.Length; i++)
				{
					char c = ptr[i];
					if (c <= '/')
					{
						if (c == '/' || c == '"')
						{
							this.nodeWriter.WriteChars(ptr + num, i - num);
							this.nodeWriter.WriteText(92);
							this.nodeWriter.WriteText((int)c);
							num = i + 1;
						}
						else if (c < ' ')
						{
							this.nodeWriter.WriteChars(ptr + num, i - num);
							this.nodeWriter.WriteText(92);
							if (XmlJsonWriter.CharacterAbbrevs[(int)c] == '\0')
							{
								this.nodeWriter.WriteText(117);
								this.nodeWriter.WriteText(string.Format(CultureInfo.InvariantCulture, "{0:x4}", (int)c));
								num = i + 1;
							}
							else
							{
								this.nodeWriter.WriteText((int)XmlJsonWriter.CharacterAbbrevs[(int)c]);
								num = i + 1;
							}
						}
					}
					else if (c == '\\')
					{
						this.nodeWriter.WriteChars(ptr + num, i - num);
						this.nodeWriter.WriteText(92);
						this.nodeWriter.WriteText((int)c);
						num = i + 1;
					}
					else if ((c >= '\ud800' && (c <= '\udfff' || c >= '\ufffe')) || XmlJsonWriter.IsUnicodeNewlineCharacter(c))
					{
						this.nodeWriter.WriteChars(ptr + num, i - num);
						this.nodeWriter.WriteText(92);
						this.nodeWriter.WriteText(117);
						this.nodeWriter.WriteText(string.Format(CultureInfo.InvariantCulture, "{0:x4}", (int)c));
						num = i + 1;
					}
				}
				if (num < i)
				{
					this.nodeWriter.WriteChars(ptr + num, i - num);
				}
			}
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00046C10 File Offset: 0x00044E10
		private void WriteIndent()
		{
			for (int i = 0; i < this.indentLevel; i++)
			{
				this.nodeWriter.WriteText(this.indentChars);
			}
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00046C3F File Offset: 0x00044E3F
		private void WriteNewLine()
		{
			this.nodeWriter.WriteText(13);
			this.nodeWriter.WriteText(10);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00046C5B File Offset: 0x00044E5B
		private void WriteJsonElementName(string localName)
		{
			this.WriteJsonQuote();
			this.WriteEscapedJsonString(localName);
			this.WriteJsonQuote();
			this.nodeWriter.WriteText(58);
			if (this.indent)
			{
				this.nodeWriter.WriteText(32);
			}
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00046C92 File Offset: 0x00044E92
		private void WriteJsonQuote()
		{
			this.nodeWriter.WriteText(34);
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00046CA4 File Offset: 0x00044EA4
		private void WritePrimitiveValue(object value)
		{
			if (this.IsClosed)
			{
				XmlJsonWriter.ThrowClosed();
			}
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
			}
			if (value is ulong)
			{
				this.WriteValue((ulong)value);
				return;
			}
			if (value is string)
			{
				this.WriteValue((string)value);
				return;
			}
			if (value is int)
			{
				this.WriteValue((int)value);
				return;
			}
			if (value is long)
			{
				this.WriteValue((long)value);
				return;
			}
			if (value is bool)
			{
				this.WriteValue((bool)value);
				return;
			}
			if (value is double)
			{
				this.WriteValue((double)value);
				return;
			}
			if (value is DateTime)
			{
				this.WriteValue((DateTime)value);
				return;
			}
			if (value is float)
			{
				this.WriteValue((float)value);
				return;
			}
			if (value is decimal)
			{
				this.WriteValue((decimal)value);
				return;
			}
			if (value is XmlDictionaryString)
			{
				this.WriteValue((XmlDictionaryString)value);
				return;
			}
			if (value is UniqueId)
			{
				this.WriteValue((UniqueId)value);
				return;
			}
			if (value is Guid)
			{
				this.WriteValue((Guid)value);
				return;
			}
			if (value is TimeSpan)
			{
				this.WriteValue((TimeSpan)value);
				return;
			}
			if (value.GetType().IsArray)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Nested array is not supported in JSON: '{0}'"), "value"));
			}
			base.WriteValue(value);
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00046E10 File Offset: 0x00045010
		private void WriteServerTypeAttribute()
		{
			string text = this.serverTypeValue;
			XmlJsonWriter.JsonDataType jsonDataType = this.dataType;
			XmlJsonWriter.NameState nameState = this.nameState;
			base.WriteStartElement("__type");
			this.WriteValue(text);
			this.WriteEndElement();
			this.dataType = jsonDataType;
			this.nameState = nameState;
			this.wroteServerTypeAttribute = true;
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00046E5F File Offset: 0x0004505F
		private void WriteValue(ulong value)
		{
			this.StartText();
			this.nodeWriter.WriteUInt64Text(value);
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00046E74 File Offset: 0x00045074
		private void WriteValue(Array array)
		{
			XmlJsonWriter.JsonDataType jsonDataType = this.dataType;
			this.dataType = XmlJsonWriter.JsonDataType.String;
			this.StartText();
			for (int i = 0; i < array.Length; i++)
			{
				if (i != 0)
				{
					this.nodeWriter.WriteText(32);
				}
				this.WritePrimitiveValue(array.GetValue(i));
			}
			this.dataType = jsonDataType;
		}

		// Token: 0x0400083D RID: 2109
		private const char BACK_SLASH = '\\';

		// Token: 0x0400083E RID: 2110
		private const char FORWARD_SLASH = '/';

		// Token: 0x0400083F RID: 2111
		private const char HIGH_SURROGATE_START = '\ud800';

		// Token: 0x04000840 RID: 2112
		private const char LOW_SURROGATE_END = '\udfff';

		// Token: 0x04000841 RID: 2113
		private const char MAX_CHAR = '\ufffe';

		// Token: 0x04000842 RID: 2114
		private const char WHITESPACE = ' ';

		// Token: 0x04000843 RID: 2115
		private const char CARRIAGE_RETURN = '\r';

		// Token: 0x04000844 RID: 2116
		private const char NEWLINE = '\n';

		// Token: 0x04000845 RID: 2117
		private const char BACKSPACE = '\b';

		// Token: 0x04000846 RID: 2118
		private const char FORM_FEED = '\f';

		// Token: 0x04000847 RID: 2119
		private const char HORIZONTAL_TABULATION = '\t';

		// Token: 0x04000848 RID: 2120
		private const string xmlNamespace = "http://www.w3.org/XML/1998/namespace";

		// Token: 0x04000849 RID: 2121
		private const string xmlnsNamespace = "http://www.w3.org/2000/xmlns/";

		// Token: 0x0400084A RID: 2122
		[SecurityCritical]
		private static BinHexEncoding binHexEncoding;

		// Token: 0x0400084B RID: 2123
		private static char[] CharacterAbbrevs;

		// Token: 0x0400084C RID: 2124
		private string attributeText;

		// Token: 0x0400084D RID: 2125
		private XmlJsonWriter.JsonDataType dataType;

		// Token: 0x0400084E RID: 2126
		private int depth;

		// Token: 0x0400084F RID: 2127
		private bool endElementBuffer;

		// Token: 0x04000850 RID: 2128
		private bool isWritingDataTypeAttribute;

		// Token: 0x04000851 RID: 2129
		private bool isWritingServerTypeAttribute;

		// Token: 0x04000852 RID: 2130
		private bool isWritingXmlnsAttribute;

		// Token: 0x04000853 RID: 2131
		private bool isWritingXmlnsAttributeDefaultNs;

		// Token: 0x04000854 RID: 2132
		private XmlJsonWriter.NameState nameState;

		// Token: 0x04000855 RID: 2133
		private JsonNodeType nodeType;

		// Token: 0x04000856 RID: 2134
		private XmlJsonWriter.JsonNodeWriter nodeWriter;

		// Token: 0x04000857 RID: 2135
		private JsonNodeType[] scopes;

		// Token: 0x04000858 RID: 2136
		private string serverTypeValue;

		// Token: 0x04000859 RID: 2137
		private WriteState writeState;

		// Token: 0x0400085A RID: 2138
		private bool wroteServerTypeAttribute;

		// Token: 0x0400085B RID: 2139
		private bool indent;

		// Token: 0x0400085C RID: 2140
		private string indentChars;

		// Token: 0x0400085D RID: 2141
		private int indentLevel;

		// Token: 0x02000191 RID: 401
		private enum JsonDataType
		{
			// Token: 0x04000A62 RID: 2658
			None,
			// Token: 0x04000A63 RID: 2659
			Null,
			// Token: 0x04000A64 RID: 2660
			Boolean,
			// Token: 0x04000A65 RID: 2661
			Number,
			// Token: 0x04000A66 RID: 2662
			String,
			// Token: 0x04000A67 RID: 2663
			Object,
			// Token: 0x04000A68 RID: 2664
			Array
		}

		// Token: 0x02000192 RID: 402
		[Flags]
		private enum NameState
		{
			// Token: 0x04000A6A RID: 2666
			None = 0,
			// Token: 0x04000A6B RID: 2667
			IsWritingNameWithMapping = 1,
			// Token: 0x04000A6C RID: 2668
			IsWritingNameAttribute = 2,
			// Token: 0x04000A6D RID: 2669
			WrittenNameWithMapping = 4
		}

		// Token: 0x02000193 RID: 403
		private class JsonNodeWriter : XmlUTF8NodeWriter
		{
			// Token: 0x06001533 RID: 5427 RVA: 0x00055380 File Offset: 0x00053580
			[SecurityCritical]
			internal unsafe void WriteChars(char* chars, int charCount)
			{
				base.UnsafeWriteUTF8Chars(chars, charCount);
			}
		}
	}
}
