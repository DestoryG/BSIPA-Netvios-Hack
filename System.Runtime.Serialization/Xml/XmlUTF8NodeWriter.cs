using System;
using System.IO;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200005B RID: 91
	internal class XmlUTF8NodeWriter : XmlStreamNodeWriter
	{
		// Token: 0x0600066B RID: 1643 RVA: 0x0001DEB3 File Offset: 0x0001C0B3
		public XmlUTF8NodeWriter()
			: this(XmlUTF8NodeWriter.defaultIsEscapedAttributeChar, XmlUTF8NodeWriter.defaultIsEscapedElementChar)
		{
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0001DEC5 File Offset: 0x0001C0C5
		public XmlUTF8NodeWriter(bool[] isEscapedAttributeChar, bool[] isEscapedElementChar)
		{
			this.isEscapedAttributeChar = isEscapedAttributeChar;
			this.isEscapedElementChar = isEscapedElementChar;
			this.inAttribute = false;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001DEE4 File Offset: 0x0001C0E4
		public new void SetOutput(Stream stream, bool ownsStream, Encoding encoding)
		{
			Encoding encoding2 = null;
			if (encoding != null && encoding.CodePage == Encoding.UTF8.CodePage)
			{
				encoding2 = encoding;
				encoding = null;
			}
			base.SetOutput(stream, ownsStream, encoding2);
			this.encoding = encoding;
			this.inAttribute = false;
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x0001DF24 File Offset: 0x0001C124
		public Encoding Encoding
		{
			get
			{
				return this.encoding;
			}
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001DF2C File Offset: 0x0001C12C
		private byte[] GetCharEntityBuffer()
		{
			if (this.entityChars == null)
			{
				this.entityChars = new byte[32];
			}
			return this.entityChars;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0001DF49 File Offset: 0x0001C149
		private char[] GetCharBuffer(int charCount)
		{
			if (charCount >= 256)
			{
				return new char[charCount];
			}
			if (this.chars == null || this.chars.Length < charCount)
			{
				this.chars = new char[charCount];
			}
			return this.chars;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001DF80 File Offset: 0x0001C180
		public override void WriteDeclaration()
		{
			if (this.encoding == null)
			{
				base.WriteUTF8Chars(XmlUTF8NodeWriter.utf8Decl, 0, XmlUTF8NodeWriter.utf8Decl.Length);
				return;
			}
			base.WriteUTF8Chars(XmlUTF8NodeWriter.startDecl, 0, XmlUTF8NodeWriter.startDecl.Length);
			if (this.encoding.WebName == Encoding.BigEndianUnicode.WebName)
			{
				base.WriteUTF8Chars("utf-16BE");
			}
			else
			{
				base.WriteUTF8Chars(this.encoding.WebName);
			}
			base.WriteUTF8Chars(XmlUTF8NodeWriter.endDecl, 0, XmlUTF8NodeWriter.endDecl.Length);
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001E00C File Offset: 0x0001C20C
		public override void WriteCData(string text)
		{
			int num;
			byte[] buffer = base.GetBuffer(9, out num);
			buffer[num] = 60;
			buffer[num + 1] = 33;
			buffer[num + 2] = 91;
			buffer[num + 3] = 67;
			buffer[num + 4] = 68;
			buffer[num + 5] = 65;
			buffer[num + 6] = 84;
			buffer[num + 7] = 65;
			buffer[num + 8] = 91;
			base.Advance(9);
			base.WriteUTF8Chars(text);
			byte[] buffer2 = base.GetBuffer(3, out num);
			buffer2[num] = 93;
			buffer2[num + 1] = 93;
			buffer2[num + 2] = 62;
			base.Advance(3);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001E090 File Offset: 0x0001C290
		private void WriteStartComment()
		{
			int num;
			byte[] buffer = base.GetBuffer(4, out num);
			buffer[num] = 60;
			buffer[num + 1] = 33;
			buffer[num + 2] = 45;
			buffer[num + 3] = 45;
			base.Advance(4);
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001E0C8 File Offset: 0x0001C2C8
		private void WriteEndComment()
		{
			int num;
			byte[] buffer = base.GetBuffer(3, out num);
			buffer[num] = 45;
			buffer[num + 1] = 45;
			buffer[num + 2] = 62;
			base.Advance(3);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001E0F7 File Offset: 0x0001C2F7
		public override void WriteComment(string text)
		{
			this.WriteStartComment();
			base.WriteUTF8Chars(text);
			this.WriteEndComment();
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001E10C File Offset: 0x0001C30C
		public override void WriteStartElement(string prefix, string localName)
		{
			base.WriteByte('<');
			if (prefix.Length != 0)
			{
				this.WritePrefix(prefix);
				base.WriteByte(':');
			}
			this.WriteLocalName(localName);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001E134 File Offset: 0x0001C334
		public override void WriteStartElement(string prefix, XmlDictionaryString localName)
		{
			this.WriteStartElement(prefix, localName.Value);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001E143 File Offset: 0x0001C343
		public override void WriteStartElement(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
		{
			base.WriteByte('<');
			if (prefixLength != 0)
			{
				this.WritePrefix(prefixBuffer, prefixOffset, prefixLength);
				base.WriteByte(':');
			}
			this.WriteLocalName(localNameBuffer, localNameOffset, localNameLength);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001E16D File Offset: 0x0001C36D
		public override void WriteEndStartElement(bool isEmpty)
		{
			if (!isEmpty)
			{
				base.WriteByte('>');
				return;
			}
			base.WriteBytes('/', '>');
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001E185 File Offset: 0x0001C385
		public override void WriteEndElement(string prefix, string localName)
		{
			base.WriteBytes('<', '/');
			if (prefix.Length != 0)
			{
				this.WritePrefix(prefix);
				base.WriteByte(':');
			}
			this.WriteLocalName(localName);
			base.WriteByte('>');
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001E1B7 File Offset: 0x0001C3B7
		public override void WriteEndElement(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
		{
			base.WriteBytes('<', '/');
			if (prefixLength != 0)
			{
				this.WritePrefix(prefixBuffer, prefixOffset, prefixLength);
				base.WriteByte(':');
			}
			this.WriteLocalName(localNameBuffer, localNameOffset, localNameLength);
			base.WriteByte('>');
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001E1EC File Offset: 0x0001C3EC
		private void WriteStartXmlnsAttribute()
		{
			int num;
			byte[] buffer = base.GetBuffer(6, out num);
			buffer[num] = 32;
			buffer[num + 1] = 120;
			buffer[num + 2] = 109;
			buffer[num + 3] = 108;
			buffer[num + 4] = 110;
			buffer[num + 5] = 115;
			base.Advance(6);
			this.inAttribute = true;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001E237 File Offset: 0x0001C437
		public override void WriteXmlnsAttribute(string prefix, string ns)
		{
			this.WriteStartXmlnsAttribute();
			if (prefix.Length != 0)
			{
				base.WriteByte(':');
				this.WritePrefix(prefix);
			}
			base.WriteBytes('=', '"');
			this.WriteEscapedText(ns);
			this.WriteEndAttribute();
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001E26D File Offset: 0x0001C46D
		public override void WriteXmlnsAttribute(string prefix, XmlDictionaryString ns)
		{
			this.WriteXmlnsAttribute(prefix, ns.Value);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0001E27C File Offset: 0x0001C47C
		public override void WriteXmlnsAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] nsBuffer, int nsOffset, int nsLength)
		{
			this.WriteStartXmlnsAttribute();
			if (prefixLength != 0)
			{
				base.WriteByte(':');
				this.WritePrefix(prefixBuffer, prefixOffset, prefixLength);
			}
			base.WriteBytes('=', '"');
			this.WriteEscapedText(nsBuffer, nsOffset, nsLength);
			this.WriteEndAttribute();
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001E2B4 File Offset: 0x0001C4B4
		public override void WriteStartAttribute(string prefix, string localName)
		{
			base.WriteByte(' ');
			if (prefix.Length != 0)
			{
				this.WritePrefix(prefix);
				base.WriteByte(':');
			}
			this.WriteLocalName(localName);
			base.WriteBytes('=', '"');
			this.inAttribute = true;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001E2ED File Offset: 0x0001C4ED
		public override void WriteStartAttribute(string prefix, XmlDictionaryString localName)
		{
			this.WriteStartAttribute(prefix, localName.Value);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001E2FC File Offset: 0x0001C4FC
		public override void WriteStartAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
		{
			base.WriteByte(' ');
			if (prefixLength != 0)
			{
				this.WritePrefix(prefixBuffer, prefixOffset, prefixLength);
				base.WriteByte(':');
			}
			this.WriteLocalName(localNameBuffer, localNameOffset, localNameLength);
			base.WriteBytes('=', '"');
			this.inAttribute = true;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001E337 File Offset: 0x0001C537
		public override void WriteEndAttribute()
		{
			base.WriteByte('"');
			this.inAttribute = false;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001E348 File Offset: 0x0001C548
		private void WritePrefix(string prefix)
		{
			if (prefix.Length == 1)
			{
				base.WriteUTF8Char((int)prefix[0]);
				return;
			}
			base.WriteUTF8Chars(prefix);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001E368 File Offset: 0x0001C568
		private void WritePrefix(byte[] prefixBuffer, int prefixOffset, int prefixLength)
		{
			if (prefixLength == 1)
			{
				base.WriteUTF8Char((int)prefixBuffer[prefixOffset]);
				return;
			}
			base.WriteUTF8Chars(prefixBuffer, prefixOffset, prefixLength);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001E381 File Offset: 0x0001C581
		private void WriteLocalName(string localName)
		{
			base.WriteUTF8Chars(localName);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001E38A File Offset: 0x0001C58A
		private void WriteLocalName(byte[] localNameBuffer, int localNameOffset, int localNameLength)
		{
			base.WriteUTF8Chars(localNameBuffer, localNameOffset, localNameLength);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001E395 File Offset: 0x0001C595
		public override void WriteEscapedText(XmlDictionaryString s)
		{
			this.WriteEscapedText(s.Value);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001E3A4 File Offset: 0x0001C5A4
		[SecuritySafeCritical]
		public unsafe override void WriteEscapedText(string s)
		{
			int length = s.Length;
			if (length > 0)
			{
				fixed (string text = s)
				{
					char* ptr = text;
					if (ptr != null)
					{
						ptr += RuntimeHelpers.OffsetToStringData / 2;
					}
					this.UnsafeWriteEscapedText(ptr, length);
				}
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001E3D8 File Offset: 0x0001C5D8
		[SecuritySafeCritical]
		public unsafe override void WriteEscapedText(char[] s, int offset, int count)
		{
			if (count > 0)
			{
				fixed (char* ptr = &s[offset])
				{
					char* ptr2 = ptr;
					this.UnsafeWriteEscapedText(ptr2, count);
				}
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001E400 File Offset: 0x0001C600
		[SecurityCritical]
		private unsafe void UnsafeWriteEscapedText(char* chars, int count)
		{
			bool[] array = (this.inAttribute ? this.isEscapedAttributeChar : this.isEscapedElementChar);
			int num = array.Length;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				char c = chars[i];
				if (((int)c < num && array[(int)c]) || c >= '\ufffe')
				{
					base.UnsafeWriteUTF8Chars(chars + num2, i - num2);
					this.WriteCharEntity((int)c);
					num2 = i + 1;
				}
			}
			base.UnsafeWriteUTF8Chars(chars + num2, count - num2);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001E480 File Offset: 0x0001C680
		public override void WriteEscapedText(byte[] chars, int offset, int count)
		{
			bool[] array = (this.inAttribute ? this.isEscapedAttributeChar : this.isEscapedElementChar);
			int num = array.Length;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				byte b = chars[offset + i];
				if ((int)b < num && array[(int)b])
				{
					base.WriteUTF8Chars(chars, offset + num2, i - num2);
					this.WriteCharEntity((int)b);
					num2 = i + 1;
				}
				else if (b == 239 && offset + i + 2 < count)
				{
					int num3 = (int)chars[offset + i + 1];
					byte b2 = chars[offset + i + 2];
					if (num3 == 191 && (b2 == 190 || b2 == 191))
					{
						base.WriteUTF8Chars(chars, offset + num2, i - num2);
						this.WriteCharEntity((b2 == 190) ? 65534 : 65535);
						num2 = i + 3;
					}
				}
			}
			base.WriteUTF8Chars(chars, offset + num2, count - num2);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001E55C File Offset: 0x0001C75C
		public void WriteText(int ch)
		{
			base.WriteUTF8Char(ch);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001E565 File Offset: 0x0001C765
		public override void WriteText(byte[] chars, int offset, int count)
		{
			base.WriteUTF8Chars(chars, offset, count);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001E570 File Offset: 0x0001C770
		[SecuritySafeCritical]
		public unsafe override void WriteText(char[] chars, int offset, int count)
		{
			if (count > 0)
			{
				fixed (char* ptr = &chars[offset])
				{
					char* ptr2 = ptr;
					base.UnsafeWriteUTF8Chars(ptr2, count);
				}
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001E597 File Offset: 0x0001C797
		public override void WriteText(string value)
		{
			base.WriteUTF8Chars(value);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001E5A0 File Offset: 0x0001C7A0
		public override void WriteText(XmlDictionaryString value)
		{
			base.WriteUTF8Chars(value.Value);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001E5B0 File Offset: 0x0001C7B0
		public void WriteLessThanCharEntity()
		{
			int num;
			byte[] buffer = base.GetBuffer(4, out num);
			buffer[num] = 38;
			buffer[num + 1] = 108;
			buffer[num + 2] = 116;
			buffer[num + 3] = 59;
			base.Advance(4);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001E5E8 File Offset: 0x0001C7E8
		public void WriteGreaterThanCharEntity()
		{
			int num;
			byte[] buffer = base.GetBuffer(4, out num);
			buffer[num] = 38;
			buffer[num + 1] = 103;
			buffer[num + 2] = 116;
			buffer[num + 3] = 59;
			base.Advance(4);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001E620 File Offset: 0x0001C820
		public void WriteAmpersandCharEntity()
		{
			int num;
			byte[] buffer = base.GetBuffer(5, out num);
			buffer[num] = 38;
			buffer[num + 1] = 97;
			buffer[num + 2] = 109;
			buffer[num + 3] = 112;
			buffer[num + 4] = 59;
			base.Advance(5);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001E660 File Offset: 0x0001C860
		public void WriteApostropheCharEntity()
		{
			int num;
			byte[] buffer = base.GetBuffer(6, out num);
			buffer[num] = 38;
			buffer[num + 1] = 97;
			buffer[num + 2] = 112;
			buffer[num + 3] = 111;
			buffer[num + 4] = 115;
			buffer[num + 5] = 59;
			base.Advance(6);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001E6A4 File Offset: 0x0001C8A4
		public void WriteQuoteCharEntity()
		{
			int num;
			byte[] buffer = base.GetBuffer(6, out num);
			buffer[num] = 38;
			buffer[num + 1] = 113;
			buffer[num + 2] = 117;
			buffer[num + 3] = 111;
			buffer[num + 4] = 116;
			buffer[num + 5] = 59;
			base.Advance(6);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001E6E8 File Offset: 0x0001C8E8
		private void WriteHexCharEntity(int ch)
		{
			byte[] charEntityBuffer = this.GetCharEntityBuffer();
			int num = 32;
			charEntityBuffer[--num] = 59;
			num -= this.ToBase16(charEntityBuffer, num, (uint)ch);
			charEntityBuffer[--num] = 120;
			charEntityBuffer[--num] = 35;
			charEntityBuffer[--num] = 38;
			base.WriteUTF8Chars(charEntityBuffer, num, 32 - num);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001E73C File Offset: 0x0001C93C
		public override void WriteCharEntity(int ch)
		{
			if (ch <= 38)
			{
				if (ch == 34)
				{
					this.WriteQuoteCharEntity();
					return;
				}
				if (ch == 38)
				{
					this.WriteAmpersandCharEntity();
					return;
				}
			}
			else
			{
				if (ch == 39)
				{
					this.WriteApostropheCharEntity();
					return;
				}
				if (ch == 60)
				{
					this.WriteLessThanCharEntity();
					return;
				}
				if (ch == 62)
				{
					this.WriteGreaterThanCharEntity();
					return;
				}
			}
			this.WriteHexCharEntity(ch);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0001E798 File Offset: 0x0001C998
		private int ToBase16(byte[] chars, int offset, uint value)
		{
			int num = 0;
			do
			{
				num++;
				chars[--offset] = XmlUTF8NodeWriter.digits[(int)(value & 15U)];
				value /= 16U;
			}
			while (value != 0U);
			return num;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0001E7C8 File Offset: 0x0001C9C8
		public override void WriteBoolText(bool value)
		{
			int num;
			byte[] buffer = base.GetBuffer(5, out num);
			base.Advance(XmlConverter.ToChars(value, buffer, num));
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001E7F0 File Offset: 0x0001C9F0
		public override void WriteDecimalText(decimal value)
		{
			int num;
			byte[] buffer = base.GetBuffer(40, out num);
			base.Advance(XmlConverter.ToChars(value, buffer, num));
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001E818 File Offset: 0x0001CA18
		public override void WriteDoubleText(double value)
		{
			int num;
			byte[] buffer = base.GetBuffer(32, out num);
			base.Advance(XmlConverter.ToChars(value, buffer, num));
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0001E840 File Offset: 0x0001CA40
		public override void WriteFloatText(float value)
		{
			int num;
			byte[] buffer = base.GetBuffer(16, out num);
			base.Advance(XmlConverter.ToChars(value, buffer, num));
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0001E868 File Offset: 0x0001CA68
		public override void WriteDateTimeText(DateTime value)
		{
			int num;
			byte[] buffer = base.GetBuffer(64, out num);
			base.Advance(XmlConverter.ToChars(value, buffer, num));
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0001E890 File Offset: 0x0001CA90
		public override void WriteUniqueIdText(UniqueId value)
		{
			if (value.IsGuid)
			{
				int charArrayLength = value.CharArrayLength;
				char[] charBuffer = this.GetCharBuffer(charArrayLength);
				value.ToCharArray(charBuffer, 0);
				this.WriteText(charBuffer, 0, charArrayLength);
				return;
			}
			this.WriteEscapedText(value.ToString());
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0001E8D4 File Offset: 0x0001CAD4
		public override void WriteInt32Text(int value)
		{
			int num;
			byte[] buffer = base.GetBuffer(16, out num);
			base.Advance(XmlConverter.ToChars(value, buffer, num));
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0001E8FC File Offset: 0x0001CAFC
		public override void WriteInt64Text(long value)
		{
			int num;
			byte[] buffer = base.GetBuffer(32, out num);
			base.Advance(XmlConverter.ToChars(value, buffer, num));
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0001E924 File Offset: 0x0001CB24
		public override void WriteUInt64Text(ulong value)
		{
			int num;
			byte[] buffer = base.GetBuffer(32, out num);
			base.Advance(XmlConverter.ToChars(value, buffer, num));
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001E94A File Offset: 0x0001CB4A
		public override void WriteGuidText(Guid value)
		{
			this.WriteText(value.ToString());
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001E95F File Offset: 0x0001CB5F
		public override void WriteBase64Text(byte[] trailBytes, int trailByteCount, byte[] buffer, int offset, int count)
		{
			if (trailByteCount > 0)
			{
				this.InternalWriteBase64Text(trailBytes, 0, trailByteCount);
			}
			this.InternalWriteBase64Text(buffer, offset, count);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0001E97C File Offset: 0x0001CB7C
		private void InternalWriteBase64Text(byte[] buffer, int offset, int count)
		{
			Base64Encoding base64Encoding = XmlConverter.Base64Encoding;
			while (count >= 3)
			{
				int num = Math.Min(384, count - count % 3);
				int num2 = num / 3 * 4;
				int num3;
				byte[] buffer2 = base.GetBuffer(num2, out num3);
				base.Advance(base64Encoding.GetChars(buffer, offset, num, buffer2, num3));
				offset += num;
				count -= num;
			}
			if (count > 0)
			{
				int num4;
				byte[] buffer3 = base.GetBuffer(4, out num4);
				base.Advance(base64Encoding.GetChars(buffer, offset, count, buffer3, num4));
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001E9F4 File Offset: 0x0001CBF4
		internal override AsyncCompletionResult WriteBase64TextAsync(AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs> xmlNodeWriterState)
		{
			if (this.internalWriteBase64TextAsyncWriter == null)
			{
				this.internalWriteBase64TextAsyncWriter = new XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter(this);
			}
			return this.internalWriteBase64TextAsyncWriter.StartAsync(xmlNodeWriterState);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0001EA16 File Offset: 0x0001CC16
		public override IAsyncResult BeginWriteBase64Text(byte[] trailBytes, int trailByteCount, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return new XmlUTF8NodeWriter.WriteBase64TextAsyncResult(trailBytes, trailByteCount, buffer, offset, count, this, callback, state);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001EA29 File Offset: 0x0001CC29
		public override void EndWriteBase64Text(IAsyncResult result)
		{
			XmlUTF8NodeWriter.WriteBase64TextAsyncResult.End(result);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001EA31 File Offset: 0x0001CC31
		private IAsyncResult BeginInternalWriteBase64Text(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return new XmlUTF8NodeWriter.InternalWriteBase64TextAsyncResult(buffer, offset, count, this, callback, state);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0001EA40 File Offset: 0x0001CC40
		private void EndInternalWriteBase64Text(IAsyncResult result)
		{
			XmlUTF8NodeWriter.InternalWriteBase64TextAsyncResult.End(result);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001EA48 File Offset: 0x0001CC48
		public override void WriteTimeSpanText(TimeSpan value)
		{
			this.WriteText(XmlConvert.ToString(value));
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0001EA56 File Offset: 0x0001CC56
		public override void WriteStartListText()
		{
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0001EA58 File Offset: 0x0001CC58
		public override void WriteListSeparator()
		{
			base.WriteByte(' ');
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0001EA62 File Offset: 0x0001CC62
		public override void WriteEndListText()
		{
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0001EA64 File Offset: 0x0001CC64
		public override void WriteQualifiedName(string prefix, XmlDictionaryString localName)
		{
			if (prefix.Length != 0)
			{
				this.WritePrefix(prefix);
				base.WriteByte(':');
			}
			this.WriteText(localName);
		}

		// Token: 0x040002A9 RID: 681
		private byte[] entityChars;

		// Token: 0x040002AA RID: 682
		private bool[] isEscapedAttributeChar;

		// Token: 0x040002AB RID: 683
		private bool[] isEscapedElementChar;

		// Token: 0x040002AC RID: 684
		private bool inAttribute;

		// Token: 0x040002AD RID: 685
		private const int bufferLength = 512;

		// Token: 0x040002AE RID: 686
		private const int maxEntityLength = 32;

		// Token: 0x040002AF RID: 687
		private const int maxBytesPerChar = 3;

		// Token: 0x040002B0 RID: 688
		private Encoding encoding;

		// Token: 0x040002B1 RID: 689
		private char[] chars;

		// Token: 0x040002B2 RID: 690
		private XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter internalWriteBase64TextAsyncWriter;

		// Token: 0x040002B3 RID: 691
		private static readonly byte[] startDecl = new byte[]
		{
			60, 63, 120, 109, 108, 32, 118, 101, 114, 115,
			105, 111, 110, 61, 34, 49, 46, 48, 34, 32,
			101, 110, 99, 111, 100, 105, 110, 103, 61, 34
		};

		// Token: 0x040002B4 RID: 692
		private static readonly byte[] endDecl = new byte[] { 34, 63, 62 };

		// Token: 0x040002B5 RID: 693
		private static readonly byte[] utf8Decl = new byte[]
		{
			60, 63, 120, 109, 108, 32, 118, 101, 114, 115,
			105, 111, 110, 61, 34, 49, 46, 48, 34, 32,
			101, 110, 99, 111, 100, 105, 110, 103, 61, 34,
			117, 116, 102, 45, 56, 34, 63, 62
		};

		// Token: 0x040002B6 RID: 694
		private static readonly byte[] digits = new byte[]
		{
			48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
			65, 66, 67, 68, 69, 70
		};

		// Token: 0x040002B7 RID: 695
		private static readonly bool[] defaultIsEscapedAttributeChar = new bool[]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true, true, true, true, true, true, true, true, true,
			true, true, true, true, true, true, true, true, true, true,
			true, true, false, false, true, false, false, false, true, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, true, false
		};

		// Token: 0x040002B8 RID: 696
		private static readonly bool[] defaultIsEscapedElementChar = new bool[]
		{
			true, true, true, true, true, true, true, true, true, false,
			false, true, true, true, true, true, true, true, true, true,
			true, true, true, true, true, true, true, true, true, true,
			true, true, false, false, false, false, false, false, true, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			true, false, true, false
		};

		// Token: 0x02000169 RID: 361
		private class InternalWriteBase64TextAsyncWriter
		{
			// Token: 0x060013D8 RID: 5080 RVA: 0x0004FE78 File Offset: 0x0004E078
			public InternalWriteBase64TextAsyncWriter(XmlUTF8NodeWriter writer)
			{
				this.writer = writer;
				this.writerState = new AsyncEventArgs<XmlWriteBase64AsyncArguments>();
				this.writerArgs = new XmlWriteBase64AsyncArguments();
			}

			// Token: 0x060013D9 RID: 5081 RVA: 0x0004FEA0 File Offset: 0x0004E0A0
			internal AsyncCompletionResult StartAsync(AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs> xmlNodeWriterState)
			{
				this.nodeState = xmlNodeWriterState;
				XmlNodeWriterWriteBase64TextArgs arguments = xmlNodeWriterState.Arguments;
				if (arguments.TrailCount > 0)
				{
					this.writerArgs.Buffer = arguments.TrailBuffer;
					this.writerArgs.Offset = 0;
					this.writerArgs.Count = arguments.TrailCount;
					this.writerState.Set(XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter.onTrailByteComplete, this.writerArgs, this);
					if (this.InternalWriteBase64TextAsync(this.writerState) != 1)
					{
						return 0;
					}
					this.writerState.Complete(true);
				}
				if (this.WriteBufferAsync() == 1)
				{
					this.nodeState = null;
					return 1;
				}
				return 0;
			}

			// Token: 0x060013DA RID: 5082 RVA: 0x0004FF3C File Offset: 0x0004E13C
			private static void OnTrailBytesComplete(IAsyncEventArgs eventArgs)
			{
				XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter internalWriteBase64TextAsyncWriter = (XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter)eventArgs.AsyncState;
				bool flag = false;
				try
				{
					if (eventArgs.Exception != null)
					{
						Exception exception = eventArgs.Exception;
						flag = true;
					}
					else if (internalWriteBase64TextAsyncWriter.WriteBufferAsync() == 1)
					{
						flag = true;
					}
				}
				catch (Exception ex)
				{
					if (Fx.IsFatal(ex))
					{
						throw;
					}
					flag = true;
				}
				if (flag)
				{
					AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs> asyncEventArgs = internalWriteBase64TextAsyncWriter.nodeState;
					internalWriteBase64TextAsyncWriter.nodeState = null;
					asyncEventArgs.Complete(false, eventArgs.Exception);
				}
			}

			// Token: 0x060013DB RID: 5083 RVA: 0x0004FFB4 File Offset: 0x0004E1B4
			private AsyncCompletionResult WriteBufferAsync()
			{
				this.writerArgs.Buffer = this.nodeState.Arguments.Buffer;
				this.writerArgs.Offset = this.nodeState.Arguments.Offset;
				this.writerArgs.Count = this.nodeState.Arguments.Count;
				this.writerState.Set(XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter.onWriteComplete, this.writerArgs, this);
				if (this.InternalWriteBase64TextAsync(this.writerState) == 1)
				{
					this.writerState.Complete(true);
					return 1;
				}
				return 0;
			}

			// Token: 0x060013DC RID: 5084 RVA: 0x00050048 File Offset: 0x0004E248
			private static void OnWriteComplete(IAsyncEventArgs eventArgs)
			{
				XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter internalWriteBase64TextAsyncWriter = (XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter)eventArgs.AsyncState;
				AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs> asyncEventArgs = internalWriteBase64TextAsyncWriter.nodeState;
				internalWriteBase64TextAsyncWriter.nodeState = null;
				asyncEventArgs.Complete(false, eventArgs.Exception);
			}

			// Token: 0x060013DD RID: 5085 RVA: 0x0005007C File Offset: 0x0004E27C
			private AsyncCompletionResult InternalWriteBase64TextAsync(AsyncEventArgs<XmlWriteBase64AsyncArguments> writerState)
			{
				XmlStreamNodeWriter.GetBufferAsyncEventArgs getBufferAsyncEventArgs = this.getBufferState;
				XmlStreamNodeWriter.GetBufferArgs getBufferArgs = this.getBufferArgs;
				XmlWriteBase64AsyncArguments arguments = writerState.Arguments;
				if (getBufferAsyncEventArgs == null)
				{
					getBufferAsyncEventArgs = new XmlStreamNodeWriter.GetBufferAsyncEventArgs();
					getBufferArgs = new XmlStreamNodeWriter.GetBufferArgs();
					this.getBufferState = getBufferAsyncEventArgs;
					this.getBufferArgs = getBufferArgs;
				}
				Base64Encoding base64Encoding = XmlConverter.Base64Encoding;
				while (arguments.Count >= 3)
				{
					int num = Math.Min(384, arguments.Count - arguments.Count % 3);
					int num2 = num / 3 * 4;
					getBufferArgs.Count = num2;
					getBufferAsyncEventArgs.Set(XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter.onGetBufferComplete, getBufferArgs, this);
					if (this.writer.GetBufferAsync(getBufferAsyncEventArgs) != 1)
					{
						return 0;
					}
					XmlStreamNodeWriter.GetBufferEventResult result = getBufferAsyncEventArgs.Result;
					getBufferAsyncEventArgs.Complete(true);
					this.writer.Advance(base64Encoding.GetChars(arguments.Buffer, arguments.Offset, num, result.Buffer, result.Offset));
					arguments.Offset += num;
					arguments.Count -= num;
				}
				if (arguments.Count > 0)
				{
					getBufferArgs.Count = 4;
					getBufferAsyncEventArgs.Set(XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter.onGetBufferComplete, getBufferArgs, this);
					if (this.writer.GetBufferAsync(getBufferAsyncEventArgs) != 1)
					{
						return 0;
					}
					XmlStreamNodeWriter.GetBufferEventResult result2 = getBufferAsyncEventArgs.Result;
					getBufferAsyncEventArgs.Complete(true);
					this.writer.Advance(base64Encoding.GetChars(arguments.Buffer, arguments.Offset, arguments.Count, result2.Buffer, result2.Offset));
				}
				return 1;
			}

			// Token: 0x060013DE RID: 5086 RVA: 0x000501E8 File Offset: 0x0004E3E8
			private static void OnGetBufferComplete(IAsyncEventArgs state)
			{
				XmlStreamNodeWriter.GetBufferEventResult result = ((XmlStreamNodeWriter.GetBufferAsyncEventArgs)state).Result;
				XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter internalWriteBase64TextAsyncWriter = (XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter)state.AsyncState;
				XmlWriteBase64AsyncArguments arguments = internalWriteBase64TextAsyncWriter.writerState.Arguments;
				Exception ex = null;
				bool flag = false;
				try
				{
					if (state.Exception != null)
					{
						ex = state.Exception;
						flag = true;
					}
					else
					{
						byte[] buffer = result.Buffer;
						int offset = result.Offset;
						Base64Encoding base64Encoding = XmlConverter.Base64Encoding;
						int num = Math.Min(384, arguments.Count - arguments.Count % 3);
						int num2 = num / 3;
						internalWriteBase64TextAsyncWriter.writer.Advance(base64Encoding.GetChars(arguments.Buffer, arguments.Offset, num, buffer, offset));
						if (num >= 3)
						{
							arguments.Offset += num;
							arguments.Count -= num;
						}
						if (internalWriteBase64TextAsyncWriter.InternalWriteBase64TextAsync(internalWriteBase64TextAsyncWriter.writerState) == 1)
						{
							flag = true;
						}
					}
				}
				catch (Exception ex2)
				{
					if (Fx.IsFatal(ex2))
					{
						throw;
					}
					ex = ex2;
					flag = true;
				}
				if (flag)
				{
					internalWriteBase64TextAsyncWriter.writerState.Complete(false, ex);
				}
			}

			// Token: 0x040009A2 RID: 2466
			private AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs> nodeState;

			// Token: 0x040009A3 RID: 2467
			private AsyncEventArgs<XmlWriteBase64AsyncArguments> writerState;

			// Token: 0x040009A4 RID: 2468
			private XmlWriteBase64AsyncArguments writerArgs;

			// Token: 0x040009A5 RID: 2469
			private XmlUTF8NodeWriter writer;

			// Token: 0x040009A6 RID: 2470
			private XmlStreamNodeWriter.GetBufferAsyncEventArgs getBufferState;

			// Token: 0x040009A7 RID: 2471
			private XmlStreamNodeWriter.GetBufferArgs getBufferArgs;

			// Token: 0x040009A8 RID: 2472
			private static AsyncEventArgsCallback onTrailByteComplete = new AsyncEventArgsCallback(XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter.OnTrailBytesComplete);

			// Token: 0x040009A9 RID: 2473
			private static AsyncEventArgsCallback onWriteComplete = new AsyncEventArgsCallback(XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter.OnWriteComplete);

			// Token: 0x040009AA RID: 2474
			private static AsyncEventArgsCallback onGetBufferComplete = new AsyncEventArgsCallback(XmlUTF8NodeWriter.InternalWriteBase64TextAsyncWriter.OnGetBufferComplete);
		}

		// Token: 0x0200016A RID: 362
		private class WriteBase64TextAsyncResult : AsyncResult
		{
			// Token: 0x060013E0 RID: 5088 RVA: 0x00050330 File Offset: 0x0004E530
			public WriteBase64TextAsyncResult(byte[] trailBytes, int trailByteCount, byte[] buffer, int offset, int count, XmlUTF8NodeWriter writer, AsyncCallback callback, object state)
				: base(callback, state)
			{
				this.writer = writer;
				this.trailBytes = trailBytes;
				this.trailByteCount = trailByteCount;
				this.buffer = buffer;
				this.offset = offset;
				this.count = count;
				if (this.HandleWriteTrailBytes(null))
				{
					base.Complete(true);
				}
			}

			// Token: 0x060013E1 RID: 5089 RVA: 0x00050384 File Offset: 0x0004E584
			private static bool OnTrailBytesComplete(IAsyncResult result)
			{
				return ((XmlUTF8NodeWriter.WriteBase64TextAsyncResult)result.AsyncState).HandleWriteTrailBytes(result);
			}

			// Token: 0x060013E2 RID: 5090 RVA: 0x00050397 File Offset: 0x0004E597
			private static bool OnComplete(IAsyncResult result)
			{
				return ((XmlUTF8NodeWriter.WriteBase64TextAsyncResult)result.AsyncState).HandleWriteBase64Text(result);
			}

			// Token: 0x060013E3 RID: 5091 RVA: 0x000503AC File Offset: 0x0004E5AC
			private bool HandleWriteTrailBytes(IAsyncResult result)
			{
				if (this.trailByteCount > 0)
				{
					if (result == null)
					{
						result = this.writer.BeginInternalWriteBase64Text(this.trailBytes, 0, this.trailByteCount, base.PrepareAsyncCompletion(XmlUTF8NodeWriter.WriteBase64TextAsyncResult.onTrailBytesComplete), this);
						if (!result.CompletedSynchronously)
						{
							return false;
						}
					}
					this.writer.EndInternalWriteBase64Text(result);
				}
				return this.HandleWriteBase64Text(null);
			}

			// Token: 0x060013E4 RID: 5092 RVA: 0x00050408 File Offset: 0x0004E608
			private bool HandleWriteBase64Text(IAsyncResult result)
			{
				if (result == null)
				{
					result = this.writer.BeginInternalWriteBase64Text(this.buffer, this.offset, this.count, base.PrepareAsyncCompletion(XmlUTF8NodeWriter.WriteBase64TextAsyncResult.onComplete), this);
					if (!result.CompletedSynchronously)
					{
						return false;
					}
				}
				this.writer.EndInternalWriteBase64Text(result);
				return true;
			}

			// Token: 0x060013E5 RID: 5093 RVA: 0x0005045A File Offset: 0x0004E65A
			public static void End(IAsyncResult result)
			{
				AsyncResult.End<XmlUTF8NodeWriter.WriteBase64TextAsyncResult>(result);
			}

			// Token: 0x040009AB RID: 2475
			private static AsyncResult.AsyncCompletion onTrailBytesComplete = new AsyncResult.AsyncCompletion(XmlUTF8NodeWriter.WriteBase64TextAsyncResult.OnTrailBytesComplete);

			// Token: 0x040009AC RID: 2476
			private static AsyncResult.AsyncCompletion onComplete = new AsyncResult.AsyncCompletion(XmlUTF8NodeWriter.WriteBase64TextAsyncResult.OnComplete);

			// Token: 0x040009AD RID: 2477
			private byte[] trailBytes;

			// Token: 0x040009AE RID: 2478
			private int trailByteCount;

			// Token: 0x040009AF RID: 2479
			private byte[] buffer;

			// Token: 0x040009B0 RID: 2480
			private int offset;

			// Token: 0x040009B1 RID: 2481
			private int count;

			// Token: 0x040009B2 RID: 2482
			private XmlUTF8NodeWriter writer;
		}

		// Token: 0x0200016B RID: 363
		private class InternalWriteBase64TextAsyncResult : AsyncResult
		{
			// Token: 0x060013E7 RID: 5095 RVA: 0x00050488 File Offset: 0x0004E688
			public InternalWriteBase64TextAsyncResult(byte[] buffer, int offset, int count, XmlUTF8NodeWriter writer, AsyncCallback callback, object state)
				: base(callback, state)
			{
				this.buffer = buffer;
				this.offset = offset;
				this.count = count;
				this.writer = writer;
				this.encoding = XmlConverter.Base64Encoding;
				if (this.ContinueWork())
				{
					base.Complete(true);
				}
			}

			// Token: 0x060013E8 RID: 5096 RVA: 0x000504D6 File Offset: 0x0004E6D6
			private static bool OnWriteTrailingCharacters(IAsyncResult result)
			{
				return ((XmlUTF8NodeWriter.InternalWriteBase64TextAsyncResult)result.AsyncState).HandleWriteTrailingCharacters(result);
			}

			// Token: 0x060013E9 RID: 5097 RVA: 0x000504E9 File Offset: 0x0004E6E9
			private bool ContinueWork()
			{
				while (this.count >= 3)
				{
					if (!this.HandleWriteCharacters(null))
					{
						return false;
					}
				}
				return this.count <= 0 || this.HandleWriteTrailingCharacters(null);
			}

			// Token: 0x060013EA RID: 5098 RVA: 0x00050514 File Offset: 0x0004E714
			private bool HandleWriteCharacters(IAsyncResult result)
			{
				int num = Math.Min(384, this.count - this.count % 3);
				int num2 = num / 3 * 4;
				if (result == null)
				{
					result = this.writer.BeginGetBuffer(num2, XmlUTF8NodeWriter.InternalWriteBase64TextAsyncResult.onWriteCharacters, this);
					if (!result.CompletedSynchronously)
					{
						return false;
					}
				}
				int num3;
				byte[] array = this.writer.EndGetBuffer(result, out num3);
				this.writer.Advance(this.encoding.GetChars(this.buffer, this.offset, num, array, num3));
				this.offset += num;
				this.count -= num;
				return true;
			}

			// Token: 0x060013EB RID: 5099 RVA: 0x000505B4 File Offset: 0x0004E7B4
			private bool HandleWriteTrailingCharacters(IAsyncResult result)
			{
				if (result == null)
				{
					result = this.writer.BeginGetBuffer(4, base.PrepareAsyncCompletion(XmlUTF8NodeWriter.InternalWriteBase64TextAsyncResult.onWriteTrailingCharacters), this);
					if (!result.CompletedSynchronously)
					{
						return false;
					}
				}
				int num;
				byte[] array = this.writer.EndGetBuffer(result, out num);
				this.writer.Advance(this.encoding.GetChars(this.buffer, this.offset, this.count, array, num));
				return true;
			}

			// Token: 0x060013EC RID: 5100 RVA: 0x00050624 File Offset: 0x0004E824
			private static void OnWriteCharacters(IAsyncResult result)
			{
				if (result.CompletedSynchronously)
				{
					return;
				}
				XmlUTF8NodeWriter.InternalWriteBase64TextAsyncResult internalWriteBase64TextAsyncResult = (XmlUTF8NodeWriter.InternalWriteBase64TextAsyncResult)result.AsyncState;
				Exception ex = null;
				bool flag = false;
				try
				{
					internalWriteBase64TextAsyncResult.HandleWriteCharacters(result);
					flag = internalWriteBase64TextAsyncResult.ContinueWork();
				}
				catch (Exception ex2)
				{
					if (Fx.IsFatal(ex2))
					{
						throw;
					}
					flag = true;
					ex = ex2;
				}
				if (flag)
				{
					internalWriteBase64TextAsyncResult.Complete(false, ex);
				}
			}

			// Token: 0x060013ED RID: 5101 RVA: 0x00050688 File Offset: 0x0004E888
			public static void End(IAsyncResult result)
			{
				AsyncResult.End<XmlUTF8NodeWriter.InternalWriteBase64TextAsyncResult>(result);
			}

			// Token: 0x040009B3 RID: 2483
			private byte[] buffer;

			// Token: 0x040009B4 RID: 2484
			private int offset;

			// Token: 0x040009B5 RID: 2485
			private int count;

			// Token: 0x040009B6 RID: 2486
			private Base64Encoding encoding;

			// Token: 0x040009B7 RID: 2487
			private XmlUTF8NodeWriter writer;

			// Token: 0x040009B8 RID: 2488
			private static AsyncCallback onWriteCharacters = Fx.ThunkCallback(new AsyncCallback(XmlUTF8NodeWriter.InternalWriteBase64TextAsyncResult.OnWriteCharacters));

			// Token: 0x040009B9 RID: 2489
			private static AsyncResult.AsyncCompletion onWriteTrailingCharacters = new AsyncResult.AsyncCompletion(XmlUTF8NodeWriter.InternalWriteBase64TextAsyncResult.OnWriteTrailingCharacters);
		}
	}
}
