using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000117 RID: 279
	internal class XmlJsonReader : XmlBaseReader, IXmlJsonReaderInitializer
	{
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x000431D4 File Offset: 0x000413D4
		public override bool CanCanonicalize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x000431D7 File Offset: 0x000413D7
		public override string Value
		{
			get
			{
				if (this.IsAttributeValue && !this.IsLocalName("type"))
				{
					return this.UnescapeJsonString(base.Value);
				}
				return base.Value;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x00043201 File Offset: 0x00041401
		private bool IsAttributeValue
		{
			get
			{
				return base.Node.NodeType == XmlNodeType.Attribute || base.Node is XmlBaseReader.XmlAttributeTextNode;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x00043221 File Offset: 0x00041421
		private bool IsReadingCollection
		{
			get
			{
				return this.scopeDepth > 0 && this.scopes[this.scopeDepth] == JsonNodeType.Collection;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x0004323E File Offset: 0x0004143E
		private bool IsReadingComplexText
		{
			get
			{
				return !base.Node.IsAtomicValue && base.Node.NodeType == XmlNodeType.Text;
			}
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00043260 File Offset: 0x00041460
		public override void Close()
		{
			base.Close();
			OnXmlDictionaryReaderClose onXmlDictionaryReaderClose = this.onReaderClose;
			this.onReaderClose = null;
			this.ResetState();
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

		// Token: 0x0600107F RID: 4223 RVA: 0x000432B4 File Offset: 0x000414B4
		public override void EndCanonicalization()
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x000432C0 File Offset: 0x000414C0
		public override string GetAttribute(int index)
		{
			return this.UnescapeJsonString(base.GetAttribute(index));
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x000432CF File Offset: 0x000414CF
		public override string GetAttribute(string localName, string namespaceUri)
		{
			if (localName != "type")
			{
				return this.UnescapeJsonString(base.GetAttribute(localName, namespaceUri));
			}
			return base.GetAttribute(localName, namespaceUri);
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x000432F5 File Offset: 0x000414F5
		public override string GetAttribute(string name)
		{
			if (name != "type")
			{
				return this.UnescapeJsonString(base.GetAttribute(name));
			}
			return base.GetAttribute(name);
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00043319 File Offset: 0x00041519
		public override string GetAttribute(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
		{
			if (XmlDictionaryString.GetString(localName) != "type")
			{
				return this.UnescapeJsonString(base.GetAttribute(localName, namespaceUri));
			}
			return base.GetAttribute(localName, namespaceUri);
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00043344 File Offset: 0x00041544
		public override bool Read()
		{
			if (base.Node.CanMoveToElement)
			{
				this.MoveToElement();
			}
			if (base.Node.ReadState == ReadState.Closed)
			{
				return false;
			}
			if (base.Node.ExitScope)
			{
				base.ExitScope();
			}
			if (!this.buffered)
			{
				base.BufferReader.SetWindow(base.ElementNode.BufferOffset, this.maxBytesPerRead);
			}
			byte b;
			if (!this.IsReadingComplexText)
			{
				this.SkipWhitespaceInBufferReader();
				if (this.TryGetByte(out b) && (this.charactersToSkipOnNextRead[0] == b || this.charactersToSkipOnNextRead[1] == b))
				{
					base.BufferReader.SkipByte();
					this.charactersToSkipOnNextRead[0] = 0;
					this.charactersToSkipOnNextRead[1] = 0;
				}
				this.SkipWhitespaceInBufferReader();
				if (this.TryGetByte(out b) && b == 93 && this.IsReadingCollection)
				{
					base.BufferReader.SkipByte();
					this.SkipWhitespaceInBufferReader();
					this.ExitJsonScope();
				}
				if (base.BufferReader.EndOfFile)
				{
					if (this.scopeDepth > 0)
					{
						this.MoveToEndElement();
						return true;
					}
					base.MoveToEndOfFile();
					return false;
				}
			}
			b = base.BufferReader.GetByte();
			if (this.scopeDepth == 0)
			{
				this.ReadNonExistentElementName(StringHandleConstStringType.Root);
			}
			else if (this.IsReadingComplexText)
			{
				switch (this.complexTextMode)
				{
				case XmlJsonReader.JsonComplexTextMode.QuotedText:
					if (b == 92)
					{
						this.ReadEscapedCharacter(true);
					}
					else
					{
						this.ReadQuotedText(true);
					}
					break;
				case XmlJsonReader.JsonComplexTextMode.NumericalText:
					this.ReadNumericalText();
					break;
				case XmlJsonReader.JsonComplexTextMode.None:
					XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Encountered an unexpected character '{0}' in JSON.", new object[] { (char)b })));
					break;
				}
			}
			else if (this.IsReadingCollection)
			{
				this.ReadNonExistentElementName(StringHandleConstStringType.Item);
			}
			else if (b == 93)
			{
				base.BufferReader.SkipByte();
				this.MoveToEndElement();
				this.ExitJsonScope();
			}
			else if (b == 123)
			{
				base.BufferReader.SkipByte();
				this.SkipWhitespaceInBufferReader();
				b = base.BufferReader.GetByte();
				if (b == 125)
				{
					base.BufferReader.SkipByte();
					this.SkipWhitespaceInBufferReader();
					if (this.TryGetByte(out b))
					{
						if (b == 44)
						{
							base.BufferReader.SkipByte();
						}
					}
					else
					{
						this.charactersToSkipOnNextRead[0] = 44;
					}
					this.MoveToEndElement();
				}
				else
				{
					this.EnterJsonScope(JsonNodeType.Object);
					this.ParseStartElement();
				}
			}
			else if (b == 125)
			{
				base.BufferReader.SkipByte();
				if (this.expectingFirstElementInNonPrimitiveChild)
				{
					this.SkipWhitespaceInBufferReader();
					b = base.BufferReader.GetByte();
					if (b == 44 || b == 125)
					{
						base.BufferReader.SkipByte();
					}
					else
					{
						XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Encountered an unexpected character '{0}' in JSON.", new object[] { (char)b })));
					}
					this.expectingFirstElementInNonPrimitiveChild = false;
				}
				this.MoveToEndElement();
			}
			else if (b == 44)
			{
				base.BufferReader.SkipByte();
				this.MoveToEndElement();
			}
			else if (b == 34)
			{
				if (this.readServerTypeElement)
				{
					this.readServerTypeElement = false;
					this.EnterJsonScope(JsonNodeType.Object);
					this.ParseStartElement();
				}
				else if (base.Node.NodeType == XmlNodeType.Element)
				{
					if (this.expectingFirstElementInNonPrimitiveChild)
					{
						this.EnterJsonScope(JsonNodeType.Object);
						this.ParseStartElement();
					}
					else
					{
						base.BufferReader.SkipByte();
						this.ReadQuotedText(true);
					}
				}
				else if (base.Node.NodeType == XmlNodeType.EndElement)
				{
					this.EnterJsonScope(JsonNodeType.Element);
					this.ParseStartElement();
				}
				else
				{
					XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Encountered an unexpected character '{0}' in JSON.", new object[] { '"' })));
				}
			}
			else if (b == 102)
			{
				int num;
				byte[] buffer = base.BufferReader.GetBuffer(5, out num);
				if (buffer[num + 1] != 97 || buffer[num + 2] != 108 || buffer[num + 3] != 115 || buffer[num + 4] != 101)
				{
					XmlExceptionHelper.ThrowTokenExpected(this, "false", Encoding.UTF8.GetString(buffer, num, 5));
				}
				base.BufferReader.Advance(5);
				if (this.TryGetByte(out b) && !XmlJsonReader.IsWhitespace(b) && b != 44 && b != 125 && b != 93)
				{
					string text = "false";
					string @string = Encoding.UTF8.GetString(buffer, num, 4);
					char c = (char)b;
					XmlExceptionHelper.ThrowTokenExpected(this, text, @string + c.ToString());
				}
				base.MoveToAtomicText().Value.SetValue(ValueHandleType.UTF8, num, 5);
			}
			else if (b == 116)
			{
				int num2;
				byte[] buffer2 = base.BufferReader.GetBuffer(4, out num2);
				if (buffer2[num2 + 1] != 114 || buffer2[num2 + 2] != 117 || buffer2[num2 + 3] != 101)
				{
					XmlExceptionHelper.ThrowTokenExpected(this, "true", Encoding.UTF8.GetString(buffer2, num2, 4));
				}
				base.BufferReader.Advance(4);
				if (this.TryGetByte(out b) && !XmlJsonReader.IsWhitespace(b) && b != 44 && b != 125 && b != 93)
				{
					string text2 = "true";
					string string2 = Encoding.UTF8.GetString(buffer2, num2, 4);
					char c = (char)b;
					XmlExceptionHelper.ThrowTokenExpected(this, text2, string2 + c.ToString());
				}
				base.MoveToAtomicText().Value.SetValue(ValueHandleType.UTF8, num2, 4);
			}
			else if (b == 110)
			{
				int num3;
				byte[] buffer3 = base.BufferReader.GetBuffer(4, out num3);
				if (buffer3[num3 + 1] != 117 || buffer3[num3 + 2] != 108 || buffer3[num3 + 3] != 108)
				{
					XmlExceptionHelper.ThrowTokenExpected(this, "null", Encoding.UTF8.GetString(buffer3, num3, 4));
				}
				base.BufferReader.Advance(4);
				this.SkipWhitespaceInBufferReader();
				if (this.TryGetByte(out b))
				{
					if (b == 44 || b == 125)
					{
						base.BufferReader.SkipByte();
					}
					else if (b != 93)
					{
						string text3 = "null";
						string string3 = Encoding.UTF8.GetString(buffer3, num3, 4);
						char c = (char)b;
						XmlExceptionHelper.ThrowTokenExpected(this, text3, string3 + c.ToString());
					}
				}
				else
				{
					this.charactersToSkipOnNextRead[0] = 44;
					this.charactersToSkipOnNextRead[1] = 125;
				}
				this.MoveToEndElement();
			}
			else if (b == 45 || (48 <= b && b <= 57) || b == 73 || b == 78)
			{
				this.ReadNumericalText();
			}
			else
			{
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Encountered an unexpected character '{0}' in JSON.", new object[] { (char)b })));
			}
			return true;
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00043980 File Offset: 0x00041B80
		public override decimal ReadContentAsDecimal()
		{
			string text = this.ReadContentAsString();
			decimal num;
			try
			{
				num = decimal.Parse(text, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "decimal", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "decimal", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "decimal", ex3));
			}
			return num;
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x00043A0C File Offset: 0x00041C0C
		public override int ReadContentAsInt()
		{
			return XmlJsonReader.ParseInt(this.ReadContentAsString(), NumberStyles.Float);
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00043A20 File Offset: 0x00041C20
		public override long ReadContentAsLong()
		{
			string text = this.ReadContentAsString();
			long num;
			try
			{
				num = long.Parse(text, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "Int64", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "Int64", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "Int64", ex3));
			}
			return num;
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x00043AAC File Offset: 0x00041CAC
		public override int ReadValueAsBase64(byte[] buffer, int offset, int count)
		{
			if (!this.IsAttributeValue)
			{
				return base.ReadValueAsBase64(buffer, offset, count);
			}
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > buffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { buffer.Length })));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { buffer.Length - offset })));
			}
			return 0;
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x00043B84 File Offset: 0x00041D84
		public override int ReadValueChunk(char[] chars, int offset, int count)
		{
			if (!this.IsAttributeValue)
			{
				return base.ReadValueChunk(chars, offset, count);
			}
			if (chars == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > chars.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { chars.Length })));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > chars.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { chars.Length - offset })));
			}
			string text = this.UnescapeJsonString(base.Node.ValueAsString);
			int num = Math.Min(count, text.Length);
			if (num > 0)
			{
				text.CopyTo(0, chars, offset, num);
				if (base.Node.QNameType == XmlBaseReader.QNameType.Xmlns)
				{
					base.Node.Namespace.Uri.SetValue(0, 0);
				}
				else
				{
					base.Node.Value.SetValue(ValueHandleType.UTF8, 0, 0);
				}
			}
			return num;
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00043CC4 File Offset: 0x00041EC4
		public void SetInput(byte[] buffer, int offset, int count, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > buffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("On JSON writer, offset exceeded buffer size {0}.", new object[] { buffer.Length })));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("JSON size exceeded remaining buffer space, by {0} byte(s).", new object[] { buffer.Length - offset })));
			}
			this.MoveToInitial(quotas, onClose);
			ArraySegment<byte> arraySegment = JsonEncodingStreamWrapper.ProcessBuffer(buffer, offset, count, encoding);
			base.BufferReader.SetBuffer(arraySegment.Array, arraySegment.Offset, arraySegment.Count, null, null);
			this.buffered = true;
			this.ResetState();
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00043DC5 File Offset: 0x00041FC5
		public void SetInput(Stream stream, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
			}
			this.MoveToInitial(quotas, onClose);
			stream = new JsonEncodingStreamWrapper(stream, encoding, true);
			base.BufferReader.SetBuffer(stream, null, null);
			this.buffered = false;
			this.ResetState();
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00043E03 File Offset: 0x00042003
		public override void StartCanonicalization(Stream stream, bool includeComments, string[] inclusivePrefixes)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00043E10 File Offset: 0x00042010
		internal static void CheckArray(Array array, int offset, int count)
		{
			if (array == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("array"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > array.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { array.Length })));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > array.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { array.Length - offset })));
			}
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00043EDE File Offset: 0x000420DE
		protected override XmlSigningNodeWriter CreateSigningNodeWriter()
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", new object[] { "CreateSigningNodeWriter" })));
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00043F04 File Offset: 0x00042104
		private static int BreakText(byte[] buffer, int offset, int length)
		{
			if (length > 0 && (buffer[offset + length - 1] & 128) == 128)
			{
				int num = length;
				do
				{
					length--;
				}
				while (length > 0 && (buffer[offset + length] & 192) != 192);
				if (length == 0)
				{
					return num;
				}
				byte b = (byte)(buffer[offset + length] << 2);
				int num2 = 2;
				while ((b & 128) == 128)
				{
					b = (byte)(b << 1);
					num2++;
					if (num2 > 4)
					{
						return num;
					}
				}
				if (length + num2 == num)
				{
					return num;
				}
				if (length == 0)
				{
					return num;
				}
			}
			return length;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00043F84 File Offset: 0x00042184
		private static int ComputeNumericalTextLength(byte[] buffer, int offset, int offsetMax)
		{
			int num = offset;
			while (offset < offsetMax)
			{
				byte b = buffer[offset];
				if (b == 44 || b == 125 || b == 93 || XmlJsonReader.IsWhitespace(b))
				{
					break;
				}
				offset++;
			}
			return offset - num;
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00043FBC File Offset: 0x000421BC
		private static int ComputeQuotedTextLengthUntilEndQuote(byte[] buffer, int offset, int offsetMax, out bool escaped)
		{
			int num = offset;
			escaped = false;
			while (offset < offsetMax)
			{
				byte b = buffer[offset];
				if (b < 32)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("Encountered an invalid character '{0}'.", new object[] { (char)b })));
				}
				if (b == 92 || b == 239)
				{
					escaped = true;
					break;
				}
				if (b == 34)
				{
					break;
				}
				offset++;
			}
			return offset - num;
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00044020 File Offset: 0x00042220
		private static bool IsWhitespace(byte ch)
		{
			return ch == 32 || ch == 9 || ch == 10 || ch == 13;
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00044038 File Offset: 0x00042238
		private static char ParseChar(string value, NumberStyles style)
		{
			int num = XmlJsonReader.ParseInt(value, style);
			char c;
			try
			{
				c = Convert.ToChar(num);
			}
			catch (OverflowException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "char", ex));
			}
			return c;
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x0004407C File Offset: 0x0004227C
		private static int ParseInt(string value, NumberStyles style)
		{
			int num;
			try
			{
				num = int.Parse(value, style, NumberFormatInfo.InvariantInfo);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Int32", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Int32", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Int32", ex3));
			}
			return num;
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x000440FC File Offset: 0x000422FC
		private void BufferElement()
		{
			int offset = base.BufferReader.Offset;
			bool flag = false;
			byte b = 0;
			while (!flag)
			{
				int num;
				int num2;
				byte[] buffer = base.BufferReader.GetBuffer(128, out num, out num2);
				if (num + 128 != num2)
				{
					break;
				}
				int num3 = num;
				while (num3 < num2 && !flag)
				{
					byte b2 = buffer[num3];
					if (b2 == 92)
					{
						num3++;
						if (num3 >= num2)
						{
							break;
						}
					}
					else if (b == 0)
					{
						if (b2 == 39 || b2 == 34)
						{
							b = b2;
						}
						if (b2 == 58)
						{
							flag = true;
						}
					}
					else if (b2 == b)
					{
						b = 0;
					}
					num3++;
				}
				base.BufferReader.Advance(128);
			}
			base.BufferReader.Offset = offset;
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x000441B4 File Offset: 0x000423B4
		private void EnterJsonScope(JsonNodeType currentNodeType)
		{
			this.scopeDepth++;
			if (this.scopes == null)
			{
				this.scopes = new JsonNodeType[4];
			}
			else if (this.scopes.Length == this.scopeDepth)
			{
				JsonNodeType[] array = new JsonNodeType[this.scopeDepth * 2];
				Array.Copy(this.scopes, array, this.scopeDepth);
				this.scopes = array;
			}
			this.scopes[this.scopeDepth] = currentNodeType;
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0004422A File Offset: 0x0004242A
		private JsonNodeType ExitJsonScope()
		{
			JsonNodeType jsonNodeType = this.scopes[this.scopeDepth];
			this.scopes[this.scopeDepth] = JsonNodeType.None;
			this.scopeDepth--;
			return jsonNodeType;
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x00044255 File Offset: 0x00042455
		private new void MoveToEndElement()
		{
			this.ExitJsonScope();
			base.MoveToEndElement();
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00044264 File Offset: 0x00042464
		private void MoveToInitial(XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
		{
			base.MoveToInitial(quotas);
			this.maxBytesPerRead = quotas.MaxBytesPerRead;
			this.onReaderClose = onClose;
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x00044280 File Offset: 0x00042480
		private void ParseAndSetLocalName()
		{
			XmlBaseReader.XmlElementNode xmlElementNode = base.EnterScope();
			xmlElementNode.NameOffset = base.BufferReader.Offset;
			do
			{
				if (base.BufferReader.GetByte() == 92)
				{
					this.ReadEscapedCharacter(false);
				}
				else
				{
					this.ReadQuotedText(false);
				}
			}
			while (this.complexTextMode == XmlJsonReader.JsonComplexTextMode.QuotedText);
			int num = base.BufferReader.Offset - 1;
			xmlElementNode.LocalName.SetValue(xmlElementNode.NameOffset, num - xmlElementNode.NameOffset);
			xmlElementNode.NameLength = num - xmlElementNode.NameOffset;
			xmlElementNode.Namespace.Uri.SetValue(xmlElementNode.NameOffset, 0);
			xmlElementNode.Prefix.SetValue(PrefixHandleType.Empty);
			xmlElementNode.IsEmptyElement = false;
			xmlElementNode.ExitScope = false;
			xmlElementNode.BufferOffset = num;
			int num2 = (int)base.BufferReader.GetByte(xmlElementNode.NameOffset);
			if ((XmlJsonReader.charType[num2] & 1) == 0)
			{
				this.SetJsonNameWithMapping(xmlElementNode);
				return;
			}
			int i = 0;
			int num3 = xmlElementNode.NameOffset;
			while (i < xmlElementNode.NameLength)
			{
				num2 = (int)base.BufferReader.GetByte(num3);
				if ((XmlJsonReader.charType[num2] & 2) == 0 || num2 >= 128)
				{
					this.SetJsonNameWithMapping(xmlElementNode);
					return;
				}
				i++;
				num3++;
			}
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x000443A8 File Offset: 0x000425A8
		private void ParseStartElement()
		{
			if (!this.buffered)
			{
				this.BufferElement();
			}
			this.expectingFirstElementInNonPrimitiveChild = false;
			byte @byte = base.BufferReader.GetByte();
			if (@byte == 34)
			{
				base.BufferReader.SkipByte();
				this.ParseAndSetLocalName();
				this.SkipWhitespaceInBufferReader();
				this.SkipExpectedByteInBufferReader(58);
				this.SkipWhitespaceInBufferReader();
				if (base.BufferReader.GetByte() == 123)
				{
					base.BufferReader.SkipByte();
					this.expectingFirstElementInNonPrimitiveChild = true;
				}
				this.ReadAttributes();
				return;
			}
			XmlExceptionHelper.ThrowTokenExpected(this, "\"", (char)@byte);
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x00044434 File Offset: 0x00042634
		private void ReadAttributes()
		{
			XmlBaseReader.XmlAttributeNode xmlAttributeNode = base.AddAttribute();
			xmlAttributeNode.LocalName.SetConstantValue(StringHandleConstStringType.Type);
			xmlAttributeNode.Namespace.Uri.SetValue(0, 0);
			xmlAttributeNode.Prefix.SetValue(PrefixHandleType.Empty);
			this.SkipWhitespaceInBufferReader();
			byte @byte = base.BufferReader.GetByte();
			if (@byte <= 102)
			{
				if (@byte != 34)
				{
					if (@byte == 91)
					{
						xmlAttributeNode.Value.SetConstantValue(ValueHandleConstStringType.Array);
						base.BufferReader.SkipByte();
						this.EnterJsonScope(JsonNodeType.Collection);
						return;
					}
					if (@byte != 102)
					{
						goto IL_0132;
					}
				}
				else
				{
					if (!this.expectingFirstElementInNonPrimitiveChild)
					{
						xmlAttributeNode.Value.SetConstantValue(ValueHandleConstStringType.String);
						return;
					}
					xmlAttributeNode.Value.SetConstantValue(ValueHandleConstStringType.Object);
					this.ReadServerTypeAttribute(true);
					return;
				}
			}
			else if (@byte <= 116)
			{
				if (@byte == 110)
				{
					xmlAttributeNode.Value.SetConstantValue(ValueHandleConstStringType.Null);
					return;
				}
				if (@byte != 116)
				{
					goto IL_0132;
				}
			}
			else
			{
				if (@byte == 123)
				{
					xmlAttributeNode.Value.SetConstantValue(ValueHandleConstStringType.Object);
					this.ReadServerTypeAttribute(false);
					return;
				}
				if (@byte != 125)
				{
					goto IL_0132;
				}
				if (this.expectingFirstElementInNonPrimitiveChild)
				{
					xmlAttributeNode.Value.SetConstantValue(ValueHandleConstStringType.Object);
					return;
				}
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Encountered an unexpected character '{0}' in JSON.", new object[] { (char)@byte })));
				return;
			}
			xmlAttributeNode.Value.SetConstantValue(ValueHandleConstStringType.Boolean);
			return;
			IL_0132:
			if (@byte == 45 || (@byte <= 57 && @byte >= 48) || @byte == 78 || @byte == 73)
			{
				xmlAttributeNode.Value.SetConstantValue(ValueHandleConstStringType.Number);
				return;
			}
			XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Encountered an unexpected character '{0}' in JSON.", new object[] { (char)@byte })));
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x000445C0 File Offset: 0x000427C0
		private void ReadEscapedCharacter(bool moveToText)
		{
			base.BufferReader.SkipByte();
			char c = (char)base.BufferReader.GetByte();
			if (c == 'u')
			{
				base.BufferReader.SkipByte();
				int num;
				byte[] array = base.BufferReader.GetBuffer(5, out num);
				string text = Encoding.UTF8.GetString(array, num, 4);
				base.BufferReader.Advance(4);
				int num2 = (int)XmlJsonReader.ParseChar(text, NumberStyles.HexNumber);
				if (char.IsHighSurrogate((char)num2) && base.BufferReader.GetByte() == 92)
				{
					base.BufferReader.SkipByte();
					this.SkipExpectedByteInBufferReader(117);
					array = base.BufferReader.GetBuffer(5, out num);
					text = Encoding.UTF8.GetString(array, num, 4);
					base.BufferReader.Advance(4);
					char c2 = XmlJsonReader.ParseChar(text, NumberStyles.HexNumber);
					if (!char.IsLowSurrogate(c2))
					{
						XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Low surrogate char '0x{0}' not valid. Low surrogate chars range from 0xDC00 to 0xDFFF.", new object[] { text })));
					}
					num2 = new SurrogateChar(c2, (char)num2).Char;
				}
				if (array[num + 4] == 34)
				{
					base.BufferReader.SkipByte();
					if (moveToText)
					{
						base.MoveToAtomicText().Value.SetCharValue(num2);
					}
					this.complexTextMode = XmlJsonReader.JsonComplexTextMode.None;
					return;
				}
				if (moveToText)
				{
					base.MoveToComplexText().Value.SetCharValue(num2);
				}
				this.complexTextMode = XmlJsonReader.JsonComplexTextMode.QuotedText;
				return;
			}
			else
			{
				if (c <= 'b')
				{
					if (c <= '/')
					{
						if (c == '"' || c == '/')
						{
							goto IL_01CE;
						}
					}
					else
					{
						if (c == '\\')
						{
							goto IL_01CE;
						}
						if (c == 'b')
						{
							c = '\b';
							goto IL_01CE;
						}
					}
				}
				else if (c <= 'n')
				{
					if (c == 'f')
					{
						c = '\f';
						goto IL_01CE;
					}
					if (c == 'n')
					{
						c = '\n';
						goto IL_01CE;
					}
				}
				else
				{
					if (c == 'r')
					{
						c = '\r';
						goto IL_01CE;
					}
					if (c == 't')
					{
						c = '\t';
						goto IL_01CE;
					}
				}
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Encountered an unexpected character '{0}' in JSON.", new object[] { c })));
				IL_01CE:
				base.BufferReader.SkipByte();
				if (base.BufferReader.GetByte() == 34)
				{
					base.BufferReader.SkipByte();
					if (moveToText)
					{
						base.MoveToAtomicText().Value.SetCharValue((int)c);
					}
					this.complexTextMode = XmlJsonReader.JsonComplexTextMode.None;
					return;
				}
				if (moveToText)
				{
					base.MoveToComplexText().Value.SetCharValue((int)c);
				}
				this.complexTextMode = XmlJsonReader.JsonComplexTextMode.QuotedText;
				return;
			}
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x000447F8 File Offset: 0x000429F8
		private void ReadNonExistentElementName(StringHandleConstStringType elementName)
		{
			this.EnterJsonScope(JsonNodeType.Object);
			XmlBaseReader.XmlElementNode xmlElementNode = base.EnterScope();
			xmlElementNode.LocalName.SetConstantValue(elementName);
			xmlElementNode.Namespace.Uri.SetValue(xmlElementNode.NameOffset, 0);
			xmlElementNode.Prefix.SetValue(PrefixHandleType.Empty);
			xmlElementNode.BufferOffset = base.BufferReader.Offset;
			xmlElementNode.IsEmptyElement = false;
			xmlElementNode.ExitScope = false;
			this.ReadAttributes();
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x00044868 File Offset: 0x00042A68
		private int ReadNonFFFE()
		{
			int num;
			byte[] buffer = base.BufferReader.GetBuffer(3, out num);
			if (buffer[num + 1] == 191 && (buffer[num + 2] == 190 || buffer[num + 2] == 191))
			{
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("FFFE in JSON is invalid.")));
			}
			return 3;
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x000448C0 File Offset: 0x00042AC0
		private void ReadNumericalText()
		{
			int num2;
			int num3;
			int num;
			if (this.buffered)
			{
				num = XmlJsonReader.ComputeNumericalTextLength(base.BufferReader.GetBuffer(out num2, out num3), num2, num3);
			}
			else
			{
				byte[] buffer = base.BufferReader.GetBuffer(2048, out num2, out num3);
				num = XmlJsonReader.ComputeNumericalTextLength(buffer, num2, num3);
				num = XmlJsonReader.BreakText(buffer, num2, num);
			}
			base.BufferReader.Advance(num);
			if (num2 <= num3 - num)
			{
				base.MoveToAtomicText().Value.SetValue(ValueHandleType.UTF8, num2, num);
				this.complexTextMode = XmlJsonReader.JsonComplexTextMode.None;
				return;
			}
			base.MoveToComplexText().Value.SetValue(ValueHandleType.UTF8, num2, num);
			this.complexTextMode = XmlJsonReader.JsonComplexTextMode.NumericalText;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0004495C File Offset: 0x00042B5C
		private void ReadQuotedText(bool moveToText)
		{
			int offset;
			bool flag;
			int num;
			bool flag2;
			if (this.buffered)
			{
				int num2;
				num = XmlJsonReader.ComputeQuotedTextLengthUntilEndQuote(base.BufferReader.GetBuffer(out offset, out num2), offset, num2, out flag);
				flag2 = offset < num2 - num;
			}
			else
			{
				int num2;
				byte[] buffer = base.BufferReader.GetBuffer(2048, out offset, out num2);
				num = XmlJsonReader.ComputeQuotedTextLengthUntilEndQuote(buffer, offset, num2, out flag);
				flag2 = offset < num2 - num;
				num = XmlJsonReader.BreakText(buffer, offset, num);
			}
			if (flag && base.BufferReader.GetByte() == 239)
			{
				offset = base.BufferReader.Offset;
				num = this.ReadNonFFFE();
			}
			base.BufferReader.Advance(num);
			if (!flag && flag2)
			{
				if (moveToText)
				{
					base.MoveToAtomicText().Value.SetValue(ValueHandleType.UTF8, offset, num);
				}
				this.SkipExpectedByteInBufferReader(34);
				this.complexTextMode = XmlJsonReader.JsonComplexTextMode.None;
				return;
			}
			if (num == 0 && flag)
			{
				this.ReadEscapedCharacter(moveToText);
				return;
			}
			if (moveToText)
			{
				base.MoveToComplexText().Value.SetValue(ValueHandleType.UTF8, offset, num);
			}
			this.complexTextMode = XmlJsonReader.JsonComplexTextMode.QuotedText;
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x00044A58 File Offset: 0x00042C58
		private void ReadServerTypeAttribute(bool consumedObjectChar)
		{
			if (!consumedObjectChar)
			{
				this.SkipExpectedByteInBufferReader(123);
				this.SkipWhitespaceInBufferReader();
				byte @byte = base.BufferReader.GetByte();
				if (@byte != 34 && @byte != 125)
				{
					XmlExceptionHelper.ThrowTokenExpected(this, "\"", (char)@byte);
				}
			}
			else
			{
				this.SkipWhitespaceInBufferReader();
			}
			int num;
			int num2;
			byte[] array = base.BufferReader.GetBuffer(8, out num, out num2);
			if (num + 8 <= num2 && array[num] == 34 && array[num + 1] == 95 && array[num + 2] == 95 && array[num + 3] == 116 && array[num + 4] == 121 && array[num + 5] == 112 && array[num + 6] == 101 && array[num + 7] == 34)
			{
				XmlBaseReader.XmlAttributeNode xmlAttributeNode = base.AddAttribute();
				xmlAttributeNode.LocalName.SetValue(num + 1, 6);
				xmlAttributeNode.Namespace.Uri.SetValue(0, 0);
				xmlAttributeNode.Prefix.SetValue(PrefixHandleType.Empty);
				base.BufferReader.Advance(8);
				if (!this.buffered)
				{
					this.BufferElement();
				}
				this.SkipWhitespaceInBufferReader();
				this.SkipExpectedByteInBufferReader(58);
				this.SkipWhitespaceInBufferReader();
				this.SkipExpectedByteInBufferReader(34);
				array = base.BufferReader.GetBuffer(out num, out num2);
				do
				{
					if (base.BufferReader.GetByte() == 92)
					{
						this.ReadEscapedCharacter(false);
					}
					else
					{
						this.ReadQuotedText(false);
					}
				}
				while (this.complexTextMode == XmlJsonReader.JsonComplexTextMode.QuotedText);
				xmlAttributeNode.Value.SetValue(ValueHandleType.UTF8, num, base.BufferReader.Offset - 1 - num);
				this.SkipWhitespaceInBufferReader();
				if (base.BufferReader.GetByte() == 44)
				{
					base.BufferReader.SkipByte();
					this.readServerTypeElement = true;
				}
			}
			if (base.BufferReader.GetByte() == 125)
			{
				base.BufferReader.SkipByte();
				this.readServerTypeElement = false;
				this.expectingFirstElementInNonPrimitiveChild = false;
				return;
			}
			this.readServerTypeElement = true;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00044C32 File Offset: 0x00042E32
		private void ResetState()
		{
			this.complexTextMode = XmlJsonReader.JsonComplexTextMode.None;
			this.expectingFirstElementInNonPrimitiveChild = false;
			this.charactersToSkipOnNextRead = new byte[2];
			this.scopeDepth = 0;
			if (this.scopes != null && this.scopes.Length > 25)
			{
				this.scopes = null;
			}
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00044C70 File Offset: 0x00042E70
		private void SetJsonNameWithMapping(XmlBaseReader.XmlElementNode elementNode)
		{
			XmlBaseReader.Namespace @namespace = base.AddNamespace();
			@namespace.Prefix.SetValue(PrefixHandleType.A);
			@namespace.Uri.SetConstantValue(StringHandleConstStringType.Item);
			base.AddXmlnsAttribute(@namespace);
			XmlBaseReader.XmlAttributeNode xmlAttributeNode = base.AddAttribute();
			xmlAttributeNode.LocalName.SetConstantValue(StringHandleConstStringType.Item);
			xmlAttributeNode.Namespace.Uri.SetValue(0, 0);
			xmlAttributeNode.Prefix.SetValue(PrefixHandleType.Empty);
			xmlAttributeNode.Value.SetValue(ValueHandleType.UTF8, elementNode.NameOffset, elementNode.NameLength);
			elementNode.NameLength = 0;
			elementNode.Prefix.SetValue(PrefixHandleType.A);
			elementNode.LocalName.SetConstantValue(StringHandleConstStringType.Item);
			elementNode.Namespace = @namespace;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00044D14 File Offset: 0x00042F14
		private void SkipExpectedByteInBufferReader(byte characterToSkip)
		{
			if (base.BufferReader.GetByte() != characterToSkip)
			{
				char c = (char)characterToSkip;
				XmlExceptionHelper.ThrowTokenExpected(this, c.ToString(), (char)base.BufferReader.GetByte());
			}
			base.BufferReader.SkipByte();
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x00044D54 File Offset: 0x00042F54
		private void SkipWhitespaceInBufferReader()
		{
			byte b;
			while (this.TryGetByte(out b) && XmlJsonReader.IsWhitespace(b))
			{
				base.BufferReader.SkipByte();
			}
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x00044D80 File Offset: 0x00042F80
		private bool TryGetByte(out byte ch)
		{
			int num;
			int num2;
			byte[] buffer = base.BufferReader.GetBuffer(1, out num, out num2);
			if (num < num2)
			{
				ch = buffer[num];
				return true;
			}
			ch = 0;
			return false;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x00044DB0 File Offset: 0x00042FB0
		private string UnescapeJsonString(string val)
		{
			if (val == null)
			{
				return null;
			}
			StringBuilder stringBuilder = null;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < val.Length; i++)
			{
				if (val[i] == '\\')
				{
					i++;
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder();
					}
					stringBuilder.Append(val, num, num2);
					if (i >= val.Length)
					{
						XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Encountered an unexpected character '{0}' in JSON.", new object[] { val[i] })));
					}
					char c = val[i];
					if (c <= '\\')
					{
						if (c <= '\'')
						{
							if (c != '"' && c != '\'')
							{
								goto IL_017D;
							}
						}
						else if (c != '/' && c != '\\')
						{
							goto IL_017D;
						}
						stringBuilder.Append(val[i]);
					}
					else if (c <= 'f')
					{
						if (c != 'b')
						{
							if (c == 'f')
							{
								stringBuilder.Append('\f');
							}
						}
						else
						{
							stringBuilder.Append('\b');
						}
					}
					else if (c != 'n')
					{
						switch (c)
						{
						case 'r':
							stringBuilder.Append('\r');
							break;
						case 't':
							stringBuilder.Append('\t');
							break;
						case 'u':
							if (i + 3 >= val.Length)
							{
								XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Encountered an unexpected character '{0}' in JSON.", new object[] { val[i] })));
							}
							stringBuilder.Append(XmlJsonReader.ParseChar(val.Substring(i + 1, 4), NumberStyles.HexNumber));
							i += 4;
							break;
						}
					}
					else
					{
						stringBuilder.Append('\n');
					}
					IL_017D:
					num = i + 1;
					num2 = 0;
				}
				else
				{
					num2++;
				}
			}
			if (stringBuilder == null)
			{
				return val;
			}
			if (num2 > 0)
			{
				stringBuilder.Append(val, num, num2);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000832 RID: 2098
		private const int MaxTextChunk = 2048;

		// Token: 0x04000833 RID: 2099
		private static byte[] charType = new byte[]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 2, 2, 0, 2, 2,
			2, 2, 2, 2, 2, 2, 2, 2, 0, 0,
			0, 0, 0, 0, 0, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 0, 0, 0, 0, 3, 0, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 0, 0, 0, 0, 0, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3
		};

		// Token: 0x04000834 RID: 2100
		private bool buffered;

		// Token: 0x04000835 RID: 2101
		private byte[] charactersToSkipOnNextRead;

		// Token: 0x04000836 RID: 2102
		private XmlJsonReader.JsonComplexTextMode complexTextMode = XmlJsonReader.JsonComplexTextMode.None;

		// Token: 0x04000837 RID: 2103
		private bool expectingFirstElementInNonPrimitiveChild;

		// Token: 0x04000838 RID: 2104
		private int maxBytesPerRead;

		// Token: 0x04000839 RID: 2105
		private OnXmlDictionaryReaderClose onReaderClose;

		// Token: 0x0400083A RID: 2106
		private bool readServerTypeElement;

		// Token: 0x0400083B RID: 2107
		private int scopeDepth;

		// Token: 0x0400083C RID: 2108
		private JsonNodeType[] scopes;

		// Token: 0x0200018F RID: 399
		private enum JsonComplexTextMode
		{
			// Token: 0x04000A5B RID: 2651
			QuotedText,
			// Token: 0x04000A5C RID: 2652
			NumericalText,
			// Token: 0x04000A5D RID: 2653
			None
		}

		// Token: 0x02000190 RID: 400
		private static class CharType
		{
			// Token: 0x04000A5E RID: 2654
			public const byte FirstName = 1;

			// Token: 0x04000A5F RID: 2655
			public const byte Name = 2;

			// Token: 0x04000A60 RID: 2656
			public const byte None = 0;
		}
	}
}
