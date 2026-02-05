using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200002E RID: 46
	internal class XmlBinaryNodeWriter : XmlStreamNodeWriter
	{
		// Token: 0x06000278 RID: 632 RVA: 0x0000DAB4 File Offset: 0x0000BCB4
		public void SetOutput(Stream stream, IXmlDictionary dictionary, XmlBinaryWriterSession session, bool ownsStream)
		{
			this.dictionary = dictionary;
			this.session = session;
			this.inAttribute = false;
			this.inList = false;
			this.attributeValue.Clear();
			this.textNodeOffset = -1;
			base.SetOutput(stream, ownsStream, null);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000DAEE File Offset: 0x0000BCEE
		private void WriteNode(XmlBinaryNodeType nodeType)
		{
			base.WriteByte((byte)nodeType);
			this.textNodeOffset = -1;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000DAFF File Offset: 0x0000BCFF
		private void WroteAttributeValue()
		{
			if (this.wroteAttributeValue && !this.inList)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("Only a single typed value may be written inside an attribute or content.")));
			}
			this.wroteAttributeValue = true;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000DB2D File Offset: 0x0000BD2D
		private void WriteTextNode(XmlBinaryNodeType nodeType)
		{
			if (this.inAttribute)
			{
				this.WroteAttributeValue();
			}
			base.WriteByte((byte)nodeType);
			this.textNodeOffset = base.BufferOffset - 1;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000DB53 File Offset: 0x0000BD53
		private byte[] GetTextNodeBuffer(int size, out int offset)
		{
			if (this.inAttribute)
			{
				this.WroteAttributeValue();
			}
			byte[] buffer = base.GetBuffer(size, out offset);
			this.textNodeOffset = offset;
			return buffer;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000DB74 File Offset: 0x0000BD74
		private void WriteTextNodeWithLength(XmlBinaryNodeType nodeType, int length)
		{
			int num;
			byte[] textNodeBuffer = this.GetTextNodeBuffer(5, out num);
			if (length < 256)
			{
				textNodeBuffer[num] = (byte)nodeType;
				textNodeBuffer[num + 1] = (byte)length;
				base.Advance(2);
				return;
			}
			if (length < 65536)
			{
				textNodeBuffer[num] = (byte)(nodeType + 2);
				textNodeBuffer[num + 1] = (byte)length;
				length >>= 8;
				textNodeBuffer[num + 2] = (byte)length;
				base.Advance(3);
				return;
			}
			textNodeBuffer[num] = (byte)(nodeType + 4);
			textNodeBuffer[num + 1] = (byte)length;
			length >>= 8;
			textNodeBuffer[num + 2] = (byte)length;
			length >>= 8;
			textNodeBuffer[num + 3] = (byte)length;
			length >>= 8;
			textNodeBuffer[num + 4] = (byte)length;
			base.Advance(5);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000DC0C File Offset: 0x0000BE0C
		private void WriteTextNodeWithInt64(XmlBinaryNodeType nodeType, long value)
		{
			int num;
			byte[] textNodeBuffer = this.GetTextNodeBuffer(9, out num);
			textNodeBuffer[num] = (byte)nodeType;
			textNodeBuffer[num + 1] = (byte)value;
			value >>= 8;
			textNodeBuffer[num + 2] = (byte)value;
			value >>= 8;
			textNodeBuffer[num + 3] = (byte)value;
			value >>= 8;
			textNodeBuffer[num + 4] = (byte)value;
			value >>= 8;
			textNodeBuffer[num + 5] = (byte)value;
			value >>= 8;
			textNodeBuffer[num + 6] = (byte)value;
			value >>= 8;
			textNodeBuffer[num + 7] = (byte)value;
			value >>= 8;
			textNodeBuffer[num + 8] = (byte)value;
			base.Advance(9);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000DC8A File Offset: 0x0000BE8A
		public override void WriteDeclaration()
		{
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000DC8C File Offset: 0x0000BE8C
		public override void WriteStartElement(string prefix, string localName)
		{
			if (prefix.Length == 0)
			{
				this.WriteNode(XmlBinaryNodeType.MinElement);
				this.WriteName(localName);
				return;
			}
			char c = prefix[0];
			if (prefix.Length == 1 && c >= 'a' && c <= 'z')
			{
				this.WritePrefixNode(XmlBinaryNodeType.PrefixElementA, (int)(c - 'a'));
				this.WriteName(localName);
				return;
			}
			this.WriteNode(XmlBinaryNodeType.Element);
			this.WriteName(prefix);
			this.WriteName(localName);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000DCF6 File Offset: 0x0000BEF6
		private void WritePrefixNode(XmlBinaryNodeType nodeType, int ch)
		{
			this.WriteNode(nodeType + ch);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000DD04 File Offset: 0x0000BF04
		public override void WriteStartElement(string prefix, XmlDictionaryString localName)
		{
			int num;
			if (!this.TryGetKey(localName, out num))
			{
				this.WriteStartElement(prefix, localName.Value);
				return;
			}
			if (prefix.Length == 0)
			{
				this.WriteNode(XmlBinaryNodeType.ShortDictionaryElement);
				this.WriteDictionaryString(localName, num);
				return;
			}
			char c = prefix[0];
			if (prefix.Length == 1 && c >= 'a' && c <= 'z')
			{
				this.WritePrefixNode(XmlBinaryNodeType.PrefixDictionaryElementA, (int)(c - 'a'));
				this.WriteDictionaryString(localName, num);
				return;
			}
			this.WriteNode(XmlBinaryNodeType.DictionaryElement);
			this.WriteName(prefix);
			this.WriteDictionaryString(localName, num);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000DD8A File Offset: 0x0000BF8A
		public override void WriteEndStartElement(bool isEmpty)
		{
			if (isEmpty)
			{
				this.WriteEndElement();
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000DD95 File Offset: 0x0000BF95
		public override void WriteEndElement(string prefix, string localName)
		{
			this.WriteEndElement();
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000DDA0 File Offset: 0x0000BFA0
		private void WriteEndElement()
		{
			if (this.textNodeOffset != -1)
			{
				byte[] streamBuffer = base.StreamBuffer;
				XmlBinaryNodeType xmlBinaryNodeType = (XmlBinaryNodeType)streamBuffer[this.textNodeOffset];
				streamBuffer[this.textNodeOffset] = (byte)(xmlBinaryNodeType + 1);
				this.textNodeOffset = -1;
				return;
			}
			this.WriteNode(XmlBinaryNodeType.EndElement);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000DDE0 File Offset: 0x0000BFE0
		public override void WriteStartAttribute(string prefix, string localName)
		{
			if (prefix.Length == 0)
			{
				this.WriteNode(XmlBinaryNodeType.MinAttribute);
				this.WriteName(localName);
			}
			else
			{
				char c = prefix[0];
				if (prefix.Length == 1 && c >= 'a' && c <= 'z')
				{
					this.WritePrefixNode(XmlBinaryNodeType.PrefixAttributeA, (int)(c - 'a'));
					this.WriteName(localName);
				}
				else
				{
					this.WriteNode(XmlBinaryNodeType.Attribute);
					this.WriteName(prefix);
					this.WriteName(localName);
				}
			}
			this.inAttribute = true;
			this.wroteAttributeValue = false;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000DE58 File Offset: 0x0000C058
		public override void WriteStartAttribute(string prefix, XmlDictionaryString localName)
		{
			int num;
			if (!this.TryGetKey(localName, out num))
			{
				this.WriteStartAttribute(prefix, localName.Value);
				return;
			}
			if (prefix.Length == 0)
			{
				this.WriteNode(XmlBinaryNodeType.ShortDictionaryAttribute);
				this.WriteDictionaryString(localName, num);
			}
			else
			{
				char c = prefix[0];
				if (prefix.Length == 1 && c >= 'a' && c <= 'z')
				{
					this.WritePrefixNode(XmlBinaryNodeType.PrefixDictionaryAttributeA, (int)(c - 'a'));
					this.WriteDictionaryString(localName, num);
				}
				else
				{
					this.WriteNode(XmlBinaryNodeType.DictionaryAttribute);
					this.WriteName(prefix);
					this.WriteDictionaryString(localName, num);
				}
			}
			this.inAttribute = true;
			this.wroteAttributeValue = false;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000DEEC File Offset: 0x0000C0EC
		public override void WriteEndAttribute()
		{
			this.inAttribute = false;
			if (!this.wroteAttributeValue)
			{
				this.attributeValue.WriteTo(this);
			}
			this.textNodeOffset = -1;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000DF10 File Offset: 0x0000C110
		public override void WriteXmlnsAttribute(string prefix, string ns)
		{
			if (prefix.Length == 0)
			{
				this.WriteNode(XmlBinaryNodeType.ShortXmlnsAttribute);
				this.WriteName(ns);
				return;
			}
			this.WriteNode(XmlBinaryNodeType.XmlnsAttribute);
			this.WriteName(prefix);
			this.WriteName(ns);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000DF40 File Offset: 0x0000C140
		public override void WriteXmlnsAttribute(string prefix, XmlDictionaryString ns)
		{
			int num;
			if (!this.TryGetKey(ns, out num))
			{
				this.WriteXmlnsAttribute(prefix, ns.Value);
				return;
			}
			if (prefix.Length == 0)
			{
				this.WriteNode(XmlBinaryNodeType.ShortDictionaryXmlnsAttribute);
				this.WriteDictionaryString(ns, num);
				return;
			}
			this.WriteNode(XmlBinaryNodeType.DictionaryXmlnsAttribute);
			this.WriteName(prefix);
			this.WriteDictionaryString(ns, num);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000DF98 File Offset: 0x0000C198
		private bool TryGetKey(XmlDictionaryString s, out int key)
		{
			key = -1;
			if (s.Dictionary == this.dictionary)
			{
				key = s.Key * 2;
				return true;
			}
			XmlDictionaryString xmlDictionaryString;
			if (this.dictionary != null && this.dictionary.TryLookup(s, out xmlDictionaryString))
			{
				key = xmlDictionaryString.Key * 2;
				return true;
			}
			if (this.session == null)
			{
				return false;
			}
			int num;
			if (!this.session.TryLookup(s, out num) && !this.session.TryAdd(s, out num))
			{
				return false;
			}
			key = num * 2 + 1;
			return true;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000E01A File Offset: 0x0000C21A
		private void WriteDictionaryString(XmlDictionaryString s, int key)
		{
			this.WriteMultiByteInt32(key);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000E024 File Offset: 0x0000C224
		[SecuritySafeCritical]
		private unsafe void WriteName(string s)
		{
			int length = s.Length;
			if (length == 0)
			{
				base.WriteByte(0);
				return;
			}
			fixed (string text = s)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.UnsafeWriteName(ptr, length);
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000E060 File Offset: 0x0000C260
		[SecurityCritical]
		private unsafe void UnsafeWriteName(char* chars, int charCount)
		{
			if (charCount < 42)
			{
				int num;
				byte[] buffer = base.GetBuffer(1 + charCount * 3, out num);
				int num2 = base.UnsafeGetUTF8Chars(chars, charCount, buffer, num + 1);
				buffer[num] = (byte)num2;
				base.Advance(1 + num2);
				return;
			}
			int num3 = base.UnsafeGetUTF8Length(chars, charCount);
			this.WriteMultiByteInt32(num3);
			base.UnsafeWriteUTF8Chars(chars, charCount);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000E0B4 File Offset: 0x0000C2B4
		private void WriteMultiByteInt32(int i)
		{
			int num;
			byte[] buffer = base.GetBuffer(5, out num);
			int num2 = num;
			while (((long)i & (long)((ulong)(-128))) != 0L)
			{
				buffer[num++] = (byte)((i & 127) | 128);
				i >>= 7;
			}
			buffer[num++] = (byte)i;
			base.Advance(num - num2);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000E100 File Offset: 0x0000C300
		public override void WriteComment(string value)
		{
			this.WriteNode(XmlBinaryNodeType.Comment);
			this.WriteName(value);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000E110 File Offset: 0x0000C310
		public override void WriteCData(string value)
		{
			this.WriteText(value);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000E119 File Offset: 0x0000C319
		private void WriteEmptyText()
		{
			this.WriteTextNode(XmlBinaryNodeType.EmptyText);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000E126 File Offset: 0x0000C326
		public override void WriteBoolText(bool value)
		{
			if (value)
			{
				this.WriteTextNode(XmlBinaryNodeType.TrueText);
				return;
			}
			this.WriteTextNode(XmlBinaryNodeType.FalseText);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000E144 File Offset: 0x0000C344
		public override void WriteInt32Text(int value)
		{
			if (value >= -128 && value < 128)
			{
				if (value == 0)
				{
					this.WriteTextNode(XmlBinaryNodeType.MinText);
					return;
				}
				if (value == 1)
				{
					this.WriteTextNode(XmlBinaryNodeType.OneText);
					return;
				}
				int num;
				byte[] textNodeBuffer = this.GetTextNodeBuffer(2, out num);
				textNodeBuffer[num] = 136;
				textNodeBuffer[num + 1] = (byte)value;
				base.Advance(2);
				return;
			}
			else
			{
				if (value >= -32768 && value < 32768)
				{
					int num2;
					byte[] textNodeBuffer2 = this.GetTextNodeBuffer(3, out num2);
					textNodeBuffer2[num2] = 138;
					textNodeBuffer2[num2 + 1] = (byte)value;
					value >>= 8;
					textNodeBuffer2[num2 + 2] = (byte)value;
					base.Advance(3);
					return;
				}
				int num3;
				byte[] textNodeBuffer3 = this.GetTextNodeBuffer(5, out num3);
				textNodeBuffer3[num3] = 140;
				textNodeBuffer3[num3 + 1] = (byte)value;
				value >>= 8;
				textNodeBuffer3[num3 + 2] = (byte)value;
				value >>= 8;
				textNodeBuffer3[num3 + 3] = (byte)value;
				value >>= 8;
				textNodeBuffer3[num3 + 4] = (byte)value;
				base.Advance(5);
				return;
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000E219 File Offset: 0x0000C419
		public override void WriteInt64Text(long value)
		{
			if (value >= -2147483648L && value <= 2147483647L)
			{
				this.WriteInt32Text((int)value);
				return;
			}
			this.WriteTextNodeWithInt64(XmlBinaryNodeType.Int64Text, value);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000E242 File Offset: 0x0000C442
		public override void WriteUInt64Text(ulong value)
		{
			if (value <= 9223372036854775807UL)
			{
				this.WriteInt64Text((long)value);
				return;
			}
			this.WriteTextNodeWithInt64(XmlBinaryNodeType.UInt64Text, (long)value);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000E264 File Offset: 0x0000C464
		private void WriteInt64(long value)
		{
			int num;
			byte[] buffer = base.GetBuffer(8, out num);
			buffer[num] = (byte)value;
			value >>= 8;
			buffer[num + 1] = (byte)value;
			value >>= 8;
			buffer[num + 2] = (byte)value;
			value >>= 8;
			buffer[num + 3] = (byte)value;
			value >>= 8;
			buffer[num + 4] = (byte)value;
			value >>= 8;
			buffer[num + 5] = (byte)value;
			value >>= 8;
			buffer[num + 6] = (byte)value;
			value >>= 8;
			buffer[num + 7] = (byte)value;
			base.Advance(8);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000E2DC File Offset: 0x0000C4DC
		public override void WriteBase64Text(byte[] trailBytes, int trailByteCount, byte[] base64Buffer, int base64Offset, int base64Count)
		{
			if (this.inAttribute)
			{
				this.attributeValue.WriteBase64Text(trailBytes, trailByteCount, base64Buffer, base64Offset, base64Count);
				return;
			}
			int num = trailByteCount + base64Count;
			if (num > 0)
			{
				this.WriteTextNodeWithLength(XmlBinaryNodeType.Bytes8Text, num);
				if (trailByteCount > 0)
				{
					int num2;
					byte[] buffer = base.GetBuffer(trailByteCount, out num2);
					for (int i = 0; i < trailByteCount; i++)
					{
						buffer[num2 + i] = trailBytes[i];
					}
					base.Advance(trailByteCount);
				}
				if (base64Count > 0)
				{
					base.WriteBytes(base64Buffer, base64Offset, base64Count);
					return;
				}
			}
			else
			{
				this.WriteEmptyText();
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000E35C File Offset: 0x0000C55C
		public override void WriteText(XmlDictionaryString value)
		{
			if (this.inAttribute)
			{
				this.attributeValue.WriteText(value);
				return;
			}
			int num;
			if (!this.TryGetKey(value, out num))
			{
				this.WriteText(value.Value);
				return;
			}
			this.WriteTextNode(XmlBinaryNodeType.DictionaryText);
			this.WriteDictionaryString(value, num);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000E3AC File Offset: 0x0000C5AC
		[SecuritySafeCritical]
		public unsafe override void WriteText(string value)
		{
			if (this.inAttribute)
			{
				this.attributeValue.WriteText(value);
				return;
			}
			if (value.Length > 0)
			{
				fixed (string text = value)
				{
					char* ptr = text;
					if (ptr != null)
					{
						ptr += RuntimeHelpers.OffsetToStringData / 2;
					}
					this.UnsafeWriteText(ptr, value.Length);
				}
				return;
			}
			this.WriteEmptyText();
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000E400 File Offset: 0x0000C600
		[SecuritySafeCritical]
		public unsafe override void WriteText(char[] chars, int offset, int count)
		{
			if (this.inAttribute)
			{
				this.attributeValue.WriteText(new string(chars, offset, count));
				return;
			}
			if (count > 0)
			{
				fixed (char* ptr = &chars[offset])
				{
					char* ptr2 = ptr;
					this.UnsafeWriteText(ptr2, count);
				}
				return;
			}
			this.WriteEmptyText();
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000E44A File Offset: 0x0000C64A
		public override void WriteText(byte[] chars, int charOffset, int charCount)
		{
			this.WriteTextNodeWithLength(XmlBinaryNodeType.Chars8Text, charCount);
			base.WriteBytes(chars, charOffset, charCount);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000E464 File Offset: 0x0000C664
		[SecurityCritical]
		private unsafe void UnsafeWriteText(char* chars, int charCount)
		{
			if (charCount == 1)
			{
				char c = *chars;
				if (c == '0')
				{
					this.WriteTextNode(XmlBinaryNodeType.MinText);
					return;
				}
				if (c == '1')
				{
					this.WriteTextNode(XmlBinaryNodeType.OneText);
					return;
				}
			}
			if (charCount <= 85)
			{
				int num;
				byte[] buffer = base.GetBuffer(2 + charCount * 3, out num);
				int num2 = base.UnsafeGetUTF8Chars(chars, charCount, buffer, num + 2);
				if (num2 / 2 <= charCount)
				{
					buffer[num] = 152;
				}
				else
				{
					buffer[num] = 182;
					num2 = base.UnsafeGetUnicodeChars(chars, charCount, buffer, num + 2);
				}
				this.textNodeOffset = num;
				buffer[num + 1] = (byte)num2;
				base.Advance(2 + num2);
				return;
			}
			int num3 = base.UnsafeGetUTF8Length(chars, charCount);
			if (num3 / 2 > charCount)
			{
				this.WriteTextNodeWithLength(XmlBinaryNodeType.UnicodeChars8Text, charCount * 2);
				base.UnsafeWriteUnicodeChars(chars, charCount);
				return;
			}
			this.WriteTextNodeWithLength(XmlBinaryNodeType.Chars8Text, num3);
			base.UnsafeWriteUTF8Chars(chars, charCount);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000E534 File Offset: 0x0000C734
		public override void WriteEscapedText(string value)
		{
			this.WriteText(value);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000E53D File Offset: 0x0000C73D
		public override void WriteEscapedText(XmlDictionaryString value)
		{
			this.WriteText(value);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000E546 File Offset: 0x0000C746
		public override void WriteEscapedText(char[] chars, int offset, int count)
		{
			this.WriteText(chars, offset, count);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000E551 File Offset: 0x0000C751
		public override void WriteEscapedText(byte[] chars, int offset, int count)
		{
			this.WriteText(chars, offset, count);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000E55C File Offset: 0x0000C75C
		public override void WriteCharEntity(int ch)
		{
			if (ch > 65535)
			{
				SurrogateChar surrogateChar = new SurrogateChar(ch);
				char[] array = new char[] { surrogateChar.HighChar, surrogateChar.LowChar };
				this.WriteText(array, 0, 2);
				return;
			}
			char[] array2 = new char[] { (char)ch };
			this.WriteText(array2, 0, 1);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000E5B4 File Offset: 0x0000C7B4
		[SecuritySafeCritical]
		public unsafe override void WriteFloatText(float f)
		{
			long num;
			if (f >= -9.223372E+18f && f <= 9.223372E+18f && (float)(num = (long)f) == f)
			{
				this.WriteInt64Text(num);
				return;
			}
			int num2;
			byte[] textNodeBuffer = this.GetTextNodeBuffer(5, out num2);
			byte* ptr = (byte*)(&f);
			textNodeBuffer[num2] = 144;
			textNodeBuffer[num2 + 1] = *ptr;
			textNodeBuffer[num2 + 2] = ptr[1];
			textNodeBuffer[num2 + 3] = ptr[2];
			textNodeBuffer[num2 + 4] = ptr[3];
			base.Advance(5);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000E620 File Offset: 0x0000C820
		[SecuritySafeCritical]
		public unsafe override void WriteDoubleText(double d)
		{
			float num;
			if (d >= -3.4028234663852886E+38 && d <= 3.4028234663852886E+38 && (double)(num = (float)d) == d)
			{
				this.WriteFloatText(num);
				return;
			}
			int num2;
			byte[] textNodeBuffer = this.GetTextNodeBuffer(9, out num2);
			byte* ptr = (byte*)(&d);
			textNodeBuffer[num2] = 146;
			textNodeBuffer[num2 + 1] = *ptr;
			textNodeBuffer[num2 + 2] = ptr[1];
			textNodeBuffer[num2 + 3] = ptr[2];
			textNodeBuffer[num2 + 4] = ptr[3];
			textNodeBuffer[num2 + 5] = ptr[4];
			textNodeBuffer[num2 + 6] = ptr[5];
			textNodeBuffer[num2 + 7] = ptr[6];
			textNodeBuffer[num2 + 8] = ptr[7];
			base.Advance(9);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000E6B8 File Offset: 0x0000C8B8
		[SecuritySafeCritical]
		public unsafe override void WriteDecimalText(decimal d)
		{
			int num;
			byte[] textNodeBuffer = this.GetTextNodeBuffer(17, out num);
			byte* ptr = (byte*)(&d);
			textNodeBuffer[num++] = 148;
			for (int i = 0; i < 16; i++)
			{
				textNodeBuffer[num++] = ptr[i];
			}
			base.Advance(17);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000E700 File Offset: 0x0000C900
		public override void WriteDateTimeText(DateTime dt)
		{
			this.WriteTextNodeWithInt64(XmlBinaryNodeType.DateTimeText, dt.ToBinary());
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000E714 File Offset: 0x0000C914
		public override void WriteUniqueIdText(UniqueId value)
		{
			if (value.IsGuid)
			{
				int num;
				byte[] textNodeBuffer = this.GetTextNodeBuffer(17, out num);
				textNodeBuffer[num] = 172;
				value.TryGetGuid(textNodeBuffer, num + 1);
				base.Advance(17);
				return;
			}
			this.WriteText(value.ToString());
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000E75C File Offset: 0x0000C95C
		public override void WriteGuidText(Guid guid)
		{
			int num;
			byte[] textNodeBuffer = this.GetTextNodeBuffer(17, out num);
			textNodeBuffer[num] = 176;
			Buffer.BlockCopy(guid.ToByteArray(), 0, textNodeBuffer, num + 1, 16);
			base.Advance(17);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000E797 File Offset: 0x0000C997
		public override void WriteTimeSpanText(TimeSpan value)
		{
			this.WriteTextNodeWithInt64(XmlBinaryNodeType.TimeSpanText, value.Ticks);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000E7AB File Offset: 0x0000C9AB
		public override void WriteStartListText()
		{
			this.inList = true;
			this.WriteNode(XmlBinaryNodeType.StartListText);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000E7BF File Offset: 0x0000C9BF
		public override void WriteListSeparator()
		{
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000E7C1 File Offset: 0x0000C9C1
		public override void WriteEndListText()
		{
			this.inList = false;
			this.wroteAttributeValue = true;
			this.WriteNode(XmlBinaryNodeType.EndListText);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000E7DC File Offset: 0x0000C9DC
		public void WriteArrayNode()
		{
			this.WriteNode(XmlBinaryNodeType.Array);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000E7E5 File Offset: 0x0000C9E5
		private void WriteArrayInfo(XmlBinaryNodeType nodeType, int count)
		{
			this.WriteNode(nodeType);
			this.WriteMultiByteInt32(count);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000E7F5 File Offset: 0x0000C9F5
		[SecurityCritical]
		public unsafe void UnsafeWriteArray(XmlBinaryNodeType nodeType, int count, byte* array, byte* arrayMax)
		{
			this.WriteArrayInfo(nodeType, count);
			this.UnsafeWriteArray(array, (int)((long)(arrayMax - array)));
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000E80E File Offset: 0x0000CA0E
		[SecurityCritical]
		private unsafe void UnsafeWriteArray(byte* array, int byteCount)
		{
			base.UnsafeWriteBytes(array, byteCount);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000E818 File Offset: 0x0000CA18
		public void WriteDateTimeArray(DateTime[] array, int offset, int count)
		{
			this.WriteArrayInfo(XmlBinaryNodeType.DateTimeTextWithEndElement, count);
			for (int i = 0; i < count; i++)
			{
				this.WriteInt64(array[offset + i].ToBinary());
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000E854 File Offset: 0x0000CA54
		public void WriteGuidArray(Guid[] array, int offset, int count)
		{
			this.WriteArrayInfo(XmlBinaryNodeType.GuidTextWithEndElement, count);
			for (int i = 0; i < count; i++)
			{
				byte[] array2 = array[offset + i].ToByteArray();
				base.WriteBytes(array2, 0, 16);
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000E894 File Offset: 0x0000CA94
		public void WriteTimeSpanArray(TimeSpan[] array, int offset, int count)
		{
			this.WriteArrayInfo(XmlBinaryNodeType.TimeSpanTextWithEndElement, count);
			for (int i = 0; i < count; i++)
			{
				this.WriteInt64(array[offset + i].Ticks);
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000E8D0 File Offset: 0x0000CAD0
		public override void WriteQualifiedName(string prefix, XmlDictionaryString localName)
		{
			if (prefix.Length == 0)
			{
				this.WriteText(localName);
				return;
			}
			char c = prefix[0];
			int num;
			if (prefix.Length == 1 && c >= 'a' && c <= 'z' && this.TryGetKey(localName, out num))
			{
				this.WriteTextNode(XmlBinaryNodeType.QNameDictionaryText);
				base.WriteByte((byte)(c - 'a'));
				this.WriteDictionaryString(localName, num);
				return;
			}
			this.WriteText(prefix);
			this.WriteText(":");
			this.WriteText(localName);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000E94B File Offset: 0x0000CB4B
		protected override void FlushBuffer()
		{
			base.FlushBuffer();
			this.textNodeOffset = -1;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000E95A File Offset: 0x0000CB5A
		public override void Close()
		{
			base.Close();
			this.attributeValue.Clear();
		}

		// Token: 0x0400019E RID: 414
		private IXmlDictionary dictionary;

		// Token: 0x0400019F RID: 415
		private XmlBinaryWriterSession session;

		// Token: 0x040001A0 RID: 416
		private bool inAttribute;

		// Token: 0x040001A1 RID: 417
		private bool inList;

		// Token: 0x040001A2 RID: 418
		private bool wroteAttributeValue;

		// Token: 0x040001A3 RID: 419
		private XmlBinaryNodeWriter.AttributeValue attributeValue;

		// Token: 0x040001A4 RID: 420
		private const int maxBytesPerChar = 3;

		// Token: 0x040001A5 RID: 421
		private int textNodeOffset;

		// Token: 0x02000149 RID: 329
		private struct AttributeValue
		{
			// Token: 0x06001296 RID: 4758 RVA: 0x0004D668 File Offset: 0x0004B868
			public void Clear()
			{
				this.captureText = null;
				this.captureXText = null;
				this.captureStream = null;
			}

			// Token: 0x06001297 RID: 4759 RVA: 0x0004D680 File Offset: 0x0004B880
			public void WriteText(string s)
			{
				if (this.captureStream != null)
				{
					this.captureText = XmlConverter.Base64Encoding.GetString(this.captureStream.GetBuffer(), 0, (int)this.captureStream.Length);
					this.captureStream = null;
				}
				if (this.captureXText != null)
				{
					this.captureText = this.captureXText.Value;
					this.captureXText = null;
				}
				if (this.captureText == null || this.captureText.Length == 0)
				{
					this.captureText = s;
					return;
				}
				this.captureText += s;
			}

			// Token: 0x06001298 RID: 4760 RVA: 0x0004D713 File Offset: 0x0004B913
			public void WriteText(XmlDictionaryString s)
			{
				if (this.captureText != null || this.captureStream != null)
				{
					this.WriteText(s.Value);
					return;
				}
				this.captureXText = s;
			}

			// Token: 0x06001299 RID: 4761 RVA: 0x0004D73C File Offset: 0x0004B93C
			public void WriteBase64Text(byte[] trailBytes, int trailByteCount, byte[] buffer, int offset, int count)
			{
				if (this.captureText != null || this.captureXText != null)
				{
					if (trailByteCount > 0)
					{
						this.WriteText(XmlConverter.Base64Encoding.GetString(trailBytes, 0, trailByteCount));
					}
					this.WriteText(XmlConverter.Base64Encoding.GetString(buffer, offset, count));
					return;
				}
				if (this.captureStream == null)
				{
					this.captureStream = new MemoryStream();
				}
				if (trailByteCount > 0)
				{
					this.captureStream.Write(trailBytes, 0, trailByteCount);
				}
				this.captureStream.Write(buffer, offset, count);
			}

			// Token: 0x0600129A RID: 4762 RVA: 0x0004D7BC File Offset: 0x0004B9BC
			public void WriteTo(XmlBinaryNodeWriter writer)
			{
				if (this.captureText != null)
				{
					writer.WriteText(this.captureText);
					this.captureText = null;
					return;
				}
				if (this.captureXText != null)
				{
					writer.WriteText(this.captureXText);
					this.captureXText = null;
					return;
				}
				if (this.captureStream != null)
				{
					writer.WriteBase64Text(null, 0, this.captureStream.GetBuffer(), 0, (int)this.captureStream.Length);
					this.captureStream = null;
					return;
				}
				writer.WriteEmptyText();
			}

			// Token: 0x0400091F RID: 2335
			private string captureText;

			// Token: 0x04000920 RID: 2336
			private XmlDictionaryString captureXText;

			// Token: 0x04000921 RID: 2337
			private MemoryStream captureStream;
		}
	}
}
