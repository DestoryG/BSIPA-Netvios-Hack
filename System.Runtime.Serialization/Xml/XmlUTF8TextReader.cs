using System;
using System.IO;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000058 RID: 88
	internal class XmlUTF8TextReader : XmlBaseReader, IXmlLineInfo, IXmlTextReaderInitializer
	{
		// Token: 0x06000644 RID: 1604 RVA: 0x0001C976 File Offset: 0x0001AB76
		public XmlUTF8TextReader()
		{
			this.prefix = new PrefixHandle(base.BufferReader);
			this.localName = new StringHandle(base.BufferReader);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0001C9A0 File Offset: 0x0001ABA0
		public void SetInput(byte[] buffer, int offset, int count, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
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
			this.MoveToInitial(quotas, onClose);
			ArraySegment<byte> arraySegment = EncodingStreamWrapper.ProcessBuffer(buffer, offset, count, encoding);
			base.BufferReader.SetBuffer(arraySegment.Array, arraySegment.Offset, arraySegment.Count, null, null);
			this.buffered = true;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001CAA0 File Offset: 0x0001ACA0
		public void SetInput(Stream stream, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
			}
			this.MoveToInitial(quotas, onClose);
			stream = new EncodingStreamWrapper(stream, encoding);
			base.BufferReader.SetBuffer(stream, null, null);
			this.buffered = false;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001CAD7 File Offset: 0x0001ACD7
		private void MoveToInitial(XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
		{
			base.MoveToInitial(quotas);
			this.maxBytesPerRead = quotas.MaxBytesPerRead;
			this.onClose = onClose;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001CAF4 File Offset: 0x0001ACF4
		public override void Close()
		{
			this.rowOffsets = null;
			base.Close();
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

		// Token: 0x06000649 RID: 1609 RVA: 0x0001CB48 File Offset: 0x0001AD48
		private void SkipWhitespace()
		{
			while (!base.BufferReader.EndOfFile && (XmlUTF8TextReader.charType[(int)base.BufferReader.GetByte()] & 4) != 0)
			{
				base.BufferReader.SkipByte();
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001CB7C File Offset: 0x0001AD7C
		private void ReadDeclaration()
		{
			if (!this.buffered)
			{
				this.BufferElement();
			}
			int num;
			byte[] array = base.BufferReader.GetBuffer(5, out num);
			if (array[num] != 63 || array[num + 1] != 120 || array[num + 2] != 109 || array[num + 3] != 108 || (XmlUTF8TextReader.charType[(int)array[num + 4]] & 4) == 0)
			{
				XmlExceptionHelper.ThrowProcessingInstructionNotSupported(this);
			}
			if (base.Node.ReadState != ReadState.Initial)
			{
				XmlExceptionHelper.ThrowDeclarationNotFirst(this);
			}
			base.BufferReader.Advance(5);
			int num2 = num + 1;
			int num3 = 3;
			int offset = base.BufferReader.Offset;
			this.SkipWhitespace();
			this.ReadAttributes();
			int i;
			for (i = base.BufferReader.Offset - offset; i > 0; i--)
			{
				byte @byte = base.BufferReader.GetByte(offset + i - 1);
				if ((XmlUTF8TextReader.charType[(int)@byte] & 4) == 0)
				{
					break;
				}
			}
			array = base.BufferReader.GetBuffer(2, out num);
			if (array[num] != 63 || array[num + 1] != 62)
			{
				XmlExceptionHelper.ThrowTokenExpected(this, "?>", Encoding.UTF8.GetString(array, num, 2));
			}
			base.BufferReader.Advance(2);
			XmlBaseReader.XmlDeclarationNode xmlDeclarationNode = base.MoveToDeclaration();
			xmlDeclarationNode.LocalName.SetValue(num2, num3);
			xmlDeclarationNode.Value.SetValue(ValueHandleType.UTF8, offset, i);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001CCC0 File Offset: 0x0001AEC0
		private void VerifyNCName(string s)
		{
			try
			{
				XmlConvert.VerifyNCName(s);
			}
			catch (XmlException ex)
			{
				XmlExceptionHelper.ThrowXmlException(this, ex);
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001CCF0 File Offset: 0x0001AEF0
		private void ReadQualifiedName(PrefixHandle prefix, StringHandle localName)
		{
			int i;
			int num;
			byte[] buffer = base.BufferReader.GetBuffer(out i, out num);
			int num2 = 0;
			int num3 = 0;
			int num4 = i;
			int num5;
			if (i < num)
			{
				num5 = (int)buffer[i];
				num3 = num5;
				if ((XmlUTF8TextReader.charType[num5] & 1) == 0)
				{
					num2 |= 128;
				}
				num2 |= num5;
				for (i++; i < num; i++)
				{
					num5 = (int)buffer[i];
					if ((XmlUTF8TextReader.charType[num5] & 2) == 0)
					{
						break;
					}
					num2 |= num5;
				}
			}
			else
			{
				num2 |= 128;
				num5 = 0;
			}
			if (num5 == 58)
			{
				int num6 = i - num4;
				if (num6 == 1 && num3 >= 97 && num3 <= 122)
				{
					prefix.SetValue(PrefixHandle.GetAlphaPrefix(num3 - 97));
				}
				else
				{
					prefix.SetValue(num4, num6);
				}
				i++;
				int num7 = i;
				if (i < num)
				{
					num5 = (int)buffer[i];
					if ((XmlUTF8TextReader.charType[num5] & 1) == 0)
					{
						num2 |= 128;
					}
					num2 |= num5;
					for (i++; i < num; i++)
					{
						num5 = (int)buffer[i];
						if ((XmlUTF8TextReader.charType[num5] & 2) == 0)
						{
							break;
						}
						num2 |= num5;
					}
				}
				else
				{
					num2 |= 128;
				}
				localName.SetValue(num7, i - num7);
				if (num2 >= 128)
				{
					this.VerifyNCName(prefix.GetString());
					this.VerifyNCName(localName.GetString());
				}
			}
			else
			{
				prefix.SetValue(PrefixHandleType.Empty);
				localName.SetValue(num4, i - num4);
				if (num2 >= 128)
				{
					this.VerifyNCName(localName.GetString());
				}
			}
			base.BufferReader.Advance(i - num4);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0001CE6C File Offset: 0x0001B06C
		private int ReadAttributeText(byte[] buffer, int offset, int offsetMax)
		{
			byte[] array = XmlUTF8TextReader.charType;
			int num = offset;
			while (offset < offsetMax && (array[(int)buffer[offset]] & 16) != 0)
			{
				offset++;
			}
			return offset - num;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001CE9C File Offset: 0x0001B09C
		private void ReadAttributes()
		{
			int num = 0;
			if (this.buffered)
			{
				num = base.BufferReader.Offset;
			}
			for (;;)
			{
				this.ReadQualifiedName(this.prefix, this.localName);
				if (base.BufferReader.GetByte() != 61)
				{
					this.SkipWhitespace();
					if (base.BufferReader.GetByte() != 61)
					{
						XmlExceptionHelper.ThrowTokenExpected(this, "=", (char)base.BufferReader.GetByte());
					}
				}
				base.BufferReader.SkipByte();
				byte b = base.BufferReader.GetByte();
				if (b != 34 && b != 39)
				{
					this.SkipWhitespace();
					b = base.BufferReader.GetByte();
					if (b != 34 && b != 39)
					{
						XmlExceptionHelper.ThrowTokenExpected(this, "\"", (char)base.BufferReader.GetByte());
					}
				}
				base.BufferReader.SkipByte();
				bool flag = false;
				int offset = base.BufferReader.Offset;
				byte b2;
				for (;;)
				{
					int num2;
					int num3;
					byte[] buffer = base.BufferReader.GetBuffer(out num2, out num3);
					int num4 = this.ReadAttributeText(buffer, num2, num3);
					base.BufferReader.Advance(num4);
					b2 = base.BufferReader.GetByte();
					if (b2 == b)
					{
						break;
					}
					if (b2 == 38)
					{
						this.ReadCharRef();
						flag = true;
					}
					else if (b2 == 39 || b2 == 34)
					{
						base.BufferReader.SkipByte();
					}
					else if (b2 == 10 || b2 == 13 || b2 == 9)
					{
						base.BufferReader.SkipByte();
						flag = true;
					}
					else if (b2 == 239)
					{
						this.ReadNonFFFE();
					}
					else
					{
						char c = (char)b;
						XmlExceptionHelper.ThrowTokenExpected(this, c.ToString(), (char)b2);
					}
				}
				int num5 = base.BufferReader.Offset - offset;
				XmlBaseReader.XmlAttributeNode xmlAttributeNode;
				if (this.prefix.IsXmlns)
				{
					XmlBaseReader.Namespace @namespace = base.AddNamespace();
					this.localName.ToPrefixHandle(@namespace.Prefix);
					@namespace.Uri.SetValue(offset, num5, flag);
					xmlAttributeNode = base.AddXmlnsAttribute(@namespace);
				}
				else if (this.prefix.IsEmpty && this.localName.IsXmlns)
				{
					XmlBaseReader.Namespace namespace2 = base.AddNamespace();
					namespace2.Prefix.SetValue(PrefixHandleType.Empty);
					namespace2.Uri.SetValue(offset, num5, flag);
					xmlAttributeNode = base.AddXmlnsAttribute(namespace2);
				}
				else if (this.prefix.IsXml)
				{
					xmlAttributeNode = base.AddXmlAttribute();
					xmlAttributeNode.Prefix.SetValue(this.prefix);
					xmlAttributeNode.LocalName.SetValue(this.localName);
					xmlAttributeNode.Value.SetValue(flag ? ValueHandleType.EscapedUTF8 : ValueHandleType.UTF8, offset, num5);
					base.FixXmlAttribute(xmlAttributeNode);
				}
				else
				{
					xmlAttributeNode = base.AddAttribute();
					xmlAttributeNode.Prefix.SetValue(this.prefix);
					xmlAttributeNode.LocalName.SetValue(this.localName);
					xmlAttributeNode.Value.SetValue(flag ? ValueHandleType.EscapedUTF8 : ValueHandleType.UTF8, offset, num5);
				}
				xmlAttributeNode.QuoteChar = (char)b;
				base.BufferReader.SkipByte();
				b2 = base.BufferReader.GetByte();
				bool flag2 = false;
				while ((XmlUTF8TextReader.charType[(int)b2] & 4) != 0)
				{
					flag2 = true;
					base.BufferReader.SkipByte();
					b2 = base.BufferReader.GetByte();
				}
				if (b2 == 62 || b2 == 47 || b2 == 63)
				{
					break;
				}
				if (!flag2)
				{
					XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("Whitespace must appear between attributes.")));
				}
			}
			if (this.buffered && base.BufferReader.Offset - num > this.maxBytesPerRead)
			{
				XmlExceptionHelper.ThrowMaxBytesPerReadExceeded(this, this.maxBytesPerRead);
			}
			base.ProcessAttributes();
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001D214 File Offset: 0x0001B414
		private void ReadNonFFFE()
		{
			int num;
			byte[] buffer = base.BufferReader.GetBuffer(3, out num);
			if (buffer[num + 1] == 191 && (buffer[num + 2] == 190 || buffer[num + 2] == 191))
			{
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("Characters with hexadecimal values 0xFFFE and 0xFFFF are not valid.")));
			}
			base.BufferReader.Advance(3);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001D275 File Offset: 0x0001B475
		private bool IsNextCharacterNonFFFE(byte[] buffer, int offset)
		{
			return buffer[offset + 1] != 191 || (buffer[offset + 2] != 190 && buffer[offset + 2] != 191);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001D2A0 File Offset: 0x0001B4A0
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
					if (b == 0)
					{
						if (b2 == 39 || b2 == 34)
						{
							b = b2;
						}
						if (b2 == 62)
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

		// Token: 0x06000652 RID: 1618 RVA: 0x0001D340 File Offset: 0x0001B540
		private new void ReadStartElement()
		{
			if (!this.buffered)
			{
				this.BufferElement();
			}
			XmlBaseReader.XmlElementNode xmlElementNode = base.EnterScope();
			xmlElementNode.NameOffset = base.BufferReader.Offset;
			this.ReadQualifiedName(xmlElementNode.Prefix, xmlElementNode.LocalName);
			xmlElementNode.NameLength = base.BufferReader.Offset - xmlElementNode.NameOffset;
			byte b = base.BufferReader.GetByte();
			while ((XmlUTF8TextReader.charType[(int)b] & 4) != 0)
			{
				base.BufferReader.SkipByte();
				b = base.BufferReader.GetByte();
			}
			if (b != 62 && b != 47)
			{
				this.ReadAttributes();
				b = base.BufferReader.GetByte();
			}
			xmlElementNode.Namespace = base.LookupNamespace(xmlElementNode.Prefix);
			bool flag = false;
			if (b == 47)
			{
				flag = true;
				base.BufferReader.SkipByte();
			}
			xmlElementNode.IsEmptyElement = flag;
			xmlElementNode.ExitScope = flag;
			if (base.BufferReader.GetByte() != 62)
			{
				XmlExceptionHelper.ThrowTokenExpected(this, ">", (char)base.BufferReader.GetByte());
			}
			base.BufferReader.SkipByte();
			xmlElementNode.BufferOffset = base.BufferReader.Offset;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001D460 File Offset: 0x0001B660
		private new void ReadEndElement()
		{
			base.BufferReader.SkipByte();
			XmlBaseReader.XmlElementNode elementNode = base.ElementNode;
			int nameOffset = elementNode.NameOffset;
			int nameLength = elementNode.NameLength;
			int num;
			byte[] buffer = base.BufferReader.GetBuffer(nameLength, out num);
			for (int i = 0; i < nameLength; i++)
			{
				if (buffer[num + i] != buffer[nameOffset + i])
				{
					this.ReadQualifiedName(this.prefix, this.localName);
					XmlExceptionHelper.ThrowTagMismatch(this, elementNode.Prefix.GetString(), elementNode.LocalName.GetString(), this.prefix.GetString(), this.localName.GetString());
				}
			}
			base.BufferReader.Advance(nameLength);
			if (base.BufferReader.GetByte() != 62)
			{
				this.SkipWhitespace();
				if (base.BufferReader.GetByte() != 62)
				{
					XmlExceptionHelper.ThrowTokenExpected(this, ">", (char)base.BufferReader.GetByte());
				}
			}
			base.BufferReader.SkipByte();
			base.MoveToEndElement();
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0001D558 File Offset: 0x0001B758
		private void ReadComment()
		{
			base.BufferReader.SkipByte();
			if (base.BufferReader.GetByte() != 45)
			{
				XmlExceptionHelper.ThrowTokenExpected(this, "--", (char)base.BufferReader.GetByte());
			}
			base.BufferReader.SkipByte();
			int offset = base.BufferReader.Offset;
			for (;;)
			{
				byte @byte = base.BufferReader.GetByte();
				if (@byte != 45)
				{
					if ((XmlUTF8TextReader.charType[(int)@byte] & 64) == 0)
					{
						if (@byte == 239)
						{
							this.ReadNonFFFE();
						}
						else
						{
							XmlExceptionHelper.ThrowInvalidXml(this, @byte);
						}
					}
					else
					{
						base.BufferReader.SkipByte();
					}
				}
				else
				{
					int num;
					byte[] buffer = base.BufferReader.GetBuffer(3, out num);
					if (buffer[num] == 45 && buffer[num + 1] == 45)
					{
						if (buffer[num + 2] == 62)
						{
							break;
						}
						XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("XML comments cannot contain '--' or end with '-'.")));
					}
					base.BufferReader.SkipByte();
				}
			}
			int num2 = base.BufferReader.Offset - offset;
			base.MoveToComment().Value.SetValue(ValueHandleType.UTF8, offset, num2);
			base.BufferReader.Advance(3);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0001D670 File Offset: 0x0001B870
		private void ReadCData()
		{
			int num;
			byte[] array = base.BufferReader.GetBuffer(7, out num);
			if (array[num] != 91 || array[num + 1] != 67 || array[num + 2] != 68 || array[num + 3] != 65 || array[num + 4] != 84 || array[num + 5] != 65 || array[num + 6] != 91)
			{
				XmlExceptionHelper.ThrowTokenExpected(this, "[CDATA[", Encoding.UTF8.GetString(array, num, 7));
			}
			base.BufferReader.Advance(7);
			int offset = base.BufferReader.Offset;
			for (;;)
			{
				byte @byte = base.BufferReader.GetByte();
				if (@byte != 93)
				{
					if (@byte == 239)
					{
						this.ReadNonFFFE();
					}
					else
					{
						base.BufferReader.SkipByte();
					}
				}
				else
				{
					array = base.BufferReader.GetBuffer(3, out num);
					if (array[num] == 93 && array[num + 1] == 93 && array[num + 2] == 62)
					{
						break;
					}
					base.BufferReader.SkipByte();
				}
			}
			int num2 = base.BufferReader.Offset - offset;
			base.MoveToCData().Value.SetValue(ValueHandleType.UTF8, offset, num2);
			base.BufferReader.Advance(3);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0001D790 File Offset: 0x0001B990
		private int ReadCharRef()
		{
			int offset = base.BufferReader.Offset;
			base.BufferReader.SkipByte();
			while (base.BufferReader.GetByte() != 59)
			{
				base.BufferReader.SkipByte();
			}
			base.BufferReader.SkipByte();
			int num = base.BufferReader.Offset - offset;
			base.BufferReader.Offset = offset;
			int charEntity = base.BufferReader.GetCharEntity(offset, num);
			base.BufferReader.Advance(num);
			return charEntity;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0001D810 File Offset: 0x0001BA10
		private void ReadWhitespace()
		{
			int num;
			int num3;
			if (this.buffered)
			{
				int num2;
				byte[] array = base.BufferReader.GetBuffer(out num, out num2);
				num3 = this.ReadWhitespace(array, num, num2);
			}
			else
			{
				int num2;
				byte[] array = base.BufferReader.GetBuffer(2048, out num, out num2);
				num3 = this.ReadWhitespace(array, num, num2);
				num3 = this.BreakText(array, num, num3);
			}
			base.BufferReader.Advance(num3);
			base.MoveToWhitespaceText().Value.SetValue(ValueHandleType.UTF8, num, num3);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0001D88C File Offset: 0x0001BA8C
		private int ReadWhitespace(byte[] buffer, int offset, int offsetMax)
		{
			byte[] array = XmlUTF8TextReader.charType;
			int num = offset;
			while (offset < offsetMax && (array[(int)buffer[offset]] & 32) != 0)
			{
				offset++;
			}
			return offset - num;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001D8BC File Offset: 0x0001BABC
		private int ReadText(byte[] buffer, int offset, int offsetMax)
		{
			byte[] array = XmlUTF8TextReader.charType;
			int num = offset;
			while (offset < offsetMax && (array[(int)buffer[offset]] & 8) != 0)
			{
				offset++;
			}
			return offset - num;
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0001D8E8 File Offset: 0x0001BAE8
		private int ReadTextAndWatchForInvalidCharacters(byte[] buffer, int offset, int offsetMax)
		{
			byte[] array = XmlUTF8TextReader.charType;
			int num = offset;
			while (offset < offsetMax && ((array[(int)buffer[offset]] & 8) != 0 || buffer[offset] == 239))
			{
				if (buffer[offset] != 239)
				{
					offset++;
				}
				else if (offset + 2 < offsetMax)
				{
					if (this.IsNextCharacterNonFFFE(buffer, offset))
					{
						offset += 3;
					}
					else
					{
						XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("Characters with hexadecimal values 0xFFFE and 0xFFFF are not valid.")));
					}
				}
				else
				{
					if (base.BufferReader.Offset < offset)
					{
						break;
					}
					int num2;
					base.BufferReader.GetBuffer(3, out num2);
				}
			}
			return offset - num;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001D978 File Offset: 0x0001BB78
		private int BreakText(byte[] buffer, int offset, int length)
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

		// Token: 0x0600065C RID: 1628 RVA: 0x0001D9F8 File Offset: 0x0001BBF8
		private void ReadText(bool hasLeadingByteOf0xEF)
		{
			int num;
			int num2;
			byte[] array;
			int num3;
			if (this.buffered)
			{
				array = base.BufferReader.GetBuffer(out num, out num2);
				if (hasLeadingByteOf0xEF)
				{
					num3 = this.ReadTextAndWatchForInvalidCharacters(array, num, num2);
				}
				else
				{
					num3 = this.ReadText(array, num, num2);
				}
			}
			else
			{
				array = base.BufferReader.GetBuffer(2048, out num, out num2);
				if (hasLeadingByteOf0xEF)
				{
					num3 = this.ReadTextAndWatchForInvalidCharacters(array, num, num2);
				}
				else
				{
					num3 = this.ReadText(array, num, num2);
				}
				num3 = this.BreakText(array, num, num3);
			}
			base.BufferReader.Advance(num3);
			if (num < num2 - 1 - num3 && array[num + num3] == 60 && array[num + num3 + 1] != 33)
			{
				base.MoveToAtomicText().Value.SetValue(ValueHandleType.UTF8, num, num3);
				return;
			}
			base.MoveToComplexText().Value.SetValue(ValueHandleType.UTF8, num, num3);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001DAC4 File Offset: 0x0001BCC4
		private void ReadEscapedText()
		{
			int num = this.ReadCharRef();
			if (num < 256 && (XmlUTF8TextReader.charType[num] & 4) != 0)
			{
				base.MoveToWhitespaceText().Value.SetCharValue(num);
				return;
			}
			base.MoveToComplexText().Value.SetCharValue(num);
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0001DB10 File Offset: 0x0001BD10
		public override bool Read()
		{
			if (base.Node.ReadState == ReadState.Closed)
			{
				return false;
			}
			if (base.Node.CanMoveToElement)
			{
				this.MoveToElement();
			}
			base.SignNode();
			if (base.Node.ExitScope)
			{
				base.ExitScope();
			}
			if (!this.buffered)
			{
				base.BufferReader.SetWindow(base.ElementNode.BufferOffset, this.maxBytesPerRead);
			}
			if (base.BufferReader.EndOfFile)
			{
				base.MoveToEndOfFile();
				return false;
			}
			byte b = base.BufferReader.GetByte();
			if (b == 60)
			{
				base.BufferReader.SkipByte();
				b = base.BufferReader.GetByte();
				if (b == 47)
				{
					this.ReadEndElement();
				}
				else if (b == 33)
				{
					base.BufferReader.SkipByte();
					b = base.BufferReader.GetByte();
					if (b == 45)
					{
						this.ReadComment();
					}
					else
					{
						if (base.OutsideRootElement)
						{
							XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("CData elements not valid at top level of an XML document.")));
						}
						this.ReadCData();
					}
				}
				else if (b == 63)
				{
					this.ReadDeclaration();
				}
				else
				{
					this.ReadStartElement();
				}
			}
			else if ((XmlUTF8TextReader.charType[(int)b] & 32) != 0)
			{
				this.ReadWhitespace();
			}
			else if (base.OutsideRootElement && b != 13)
			{
				XmlExceptionHelper.ThrowInvalidRootData(this);
			}
			else if ((XmlUTF8TextReader.charType[(int)b] & 8) != 0)
			{
				this.ReadText(false);
			}
			else if (b == 38)
			{
				this.ReadEscapedText();
			}
			else if (b == 13)
			{
				base.BufferReader.SkipByte();
				if (!base.BufferReader.EndOfFile && base.BufferReader.GetByte() == 10)
				{
					this.ReadWhitespace();
				}
				else
				{
					base.MoveToComplexText().Value.SetCharValue(10);
				}
			}
			else if (b == 93)
			{
				int num;
				byte[] buffer = base.BufferReader.GetBuffer(3, out num);
				if (buffer[num] == 93 && buffer[num + 1] == 93 && buffer[num + 2] == 62)
				{
					XmlExceptionHelper.ThrowXmlException(this, new XmlException(global::System.Runtime.Serialization.SR.GetString("']]>' not valid in text node content.")));
				}
				base.BufferReader.SkipByte();
				base.MoveToComplexText().Value.SetCharValue(93);
			}
			else if (b == 239)
			{
				this.ReadText(true);
			}
			else
			{
				XmlExceptionHelper.ThrowInvalidXml(this, b);
			}
			return true;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0001DD58 File Offset: 0x0001BF58
		protected override XmlSigningNodeWriter CreateSigningNodeWriter()
		{
			return new XmlSigningNodeWriter(true);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001DD60 File Offset: 0x0001BF60
		public bool HasLineInfo()
		{
			return true;
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0001DD64 File Offset: 0x0001BF64
		public int LineNumber
		{
			get
			{
				int num;
				int num2;
				this.GetPosition(out num, out num2);
				return num;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x0001DD7C File Offset: 0x0001BF7C
		public int LinePosition
		{
			get
			{
				int num;
				int num2;
				this.GetPosition(out num, out num2);
				return num2;
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001DD94 File Offset: 0x0001BF94
		private void GetPosition(out int row, out int column)
		{
			if (this.rowOffsets == null)
			{
				this.rowOffsets = base.BufferReader.GetRows();
			}
			int offset = base.BufferReader.Offset;
			int num = 0;
			while (num < this.rowOffsets.Length - 1 && this.rowOffsets[num + 1] < offset)
			{
				num++;
			}
			row = num + 1;
			column = offset - this.rowOffsets[num] + 1;
		}

		// Token: 0x040002A0 RID: 672
		private const int MaxTextChunk = 2048;

		// Token: 0x040002A1 RID: 673
		private PrefixHandle prefix;

		// Token: 0x040002A2 RID: 674
		private StringHandle localName;

		// Token: 0x040002A3 RID: 675
		private int[] rowOffsets;

		// Token: 0x040002A4 RID: 676
		private OnXmlDictionaryReaderClose onClose;

		// Token: 0x040002A5 RID: 677
		private bool buffered;

		// Token: 0x040002A6 RID: 678
		private int maxBytesPerRead;

		// Token: 0x040002A7 RID: 679
		private static byte[] charType = new byte[]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 108,
			108, 0, 0, 68, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 124, 88, 72, 88, 88, 88, 64, 72,
			88, 88, 88, 88, 88, 90, 90, 88, 90, 90,
			90, 90, 90, 90, 90, 90, 90, 90, 88, 88,
			64, 88, 88, 88, 88, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 88, 88, 80, 88, 91, 88, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 88, 88, 88, 88, 88, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 3,
			91, 91, 91, 91, 91, 91, 91, 91, 91, 91,
			91, 91, 91, 91, 91, 91
		};

		// Token: 0x02000168 RID: 360
		private static class CharType
		{
			// Token: 0x0400099A RID: 2458
			public const byte None = 0;

			// Token: 0x0400099B RID: 2459
			public const byte FirstName = 1;

			// Token: 0x0400099C RID: 2460
			public const byte Name = 2;

			// Token: 0x0400099D RID: 2461
			public const byte Whitespace = 4;

			// Token: 0x0400099E RID: 2462
			public const byte Text = 8;

			// Token: 0x0400099F RID: 2463
			public const byte AttributeText = 16;

			// Token: 0x040009A0 RID: 2464
			public const byte SpecialWhitespace = 32;

			// Token: 0x040009A1 RID: 2465
			public const byte Comment = 64;
		}
	}
}
