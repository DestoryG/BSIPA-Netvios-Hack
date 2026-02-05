using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000055 RID: 85
	internal class XmlSigningNodeWriter : XmlNodeWriter
	{
		// Token: 0x060005EC RID: 1516 RVA: 0x0001B882 File Offset: 0x00019A82
		public XmlSigningNodeWriter(bool text)
		{
			this.text = text;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001B891 File Offset: 0x00019A91
		public void SetOutput(XmlNodeWriter writer, Stream stream, bool includeComments, string[] inclusivePrefixes)
		{
			this.writer = writer;
			if (this.signingWriter == null)
			{
				this.signingWriter = new XmlCanonicalWriter();
			}
			this.signingWriter.SetOutput(stream, includeComments, inclusivePrefixes);
			this.chars = new byte[64];
			this.base64Chars = null;
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x0001B8D0 File Offset: 0x00019AD0
		// (set) Token: 0x060005EF RID: 1519 RVA: 0x0001B8D8 File Offset: 0x00019AD8
		public XmlNodeWriter NodeWriter
		{
			get
			{
				return this.writer;
			}
			set
			{
				this.writer = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x0001B8E1 File Offset: 0x00019AE1
		public XmlCanonicalWriter CanonicalWriter
		{
			get
			{
				return this.signingWriter;
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001B8E9 File Offset: 0x00019AE9
		public override void Flush()
		{
			this.writer.Flush();
			this.signingWriter.Flush();
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001B901 File Offset: 0x00019B01
		public override void Close()
		{
			this.writer.Close();
			this.signingWriter.Close();
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001B919 File Offset: 0x00019B19
		public override void WriteDeclaration()
		{
			this.writer.WriteDeclaration();
			this.signingWriter.WriteDeclaration();
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001B931 File Offset: 0x00019B31
		public override void WriteComment(string text)
		{
			this.writer.WriteComment(text);
			this.signingWriter.WriteComment(text);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001B94B File Offset: 0x00019B4B
		public override void WriteCData(string text)
		{
			this.writer.WriteCData(text);
			this.signingWriter.WriteEscapedText(text);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001B965 File Offset: 0x00019B65
		public override void WriteStartElement(string prefix, string localName)
		{
			this.writer.WriteStartElement(prefix, localName);
			this.signingWriter.WriteStartElement(prefix, localName);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001B981 File Offset: 0x00019B81
		public override void WriteStartElement(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
		{
			this.writer.WriteStartElement(prefixBuffer, prefixOffset, prefixLength, localNameBuffer, localNameOffset, localNameLength);
			this.signingWriter.WriteStartElement(prefixBuffer, prefixOffset, prefixLength, localNameBuffer, localNameOffset, localNameLength);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001B9AB File Offset: 0x00019BAB
		public override void WriteStartElement(string prefix, XmlDictionaryString localName)
		{
			this.writer.WriteStartElement(prefix, localName);
			this.signingWriter.WriteStartElement(prefix, localName.Value);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001B9CC File Offset: 0x00019BCC
		public override void WriteEndStartElement(bool isEmpty)
		{
			this.writer.WriteEndStartElement(isEmpty);
			this.signingWriter.WriteEndStartElement(isEmpty);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0001B9E6 File Offset: 0x00019BE6
		public override void WriteEndElement(string prefix, string localName)
		{
			this.writer.WriteEndElement(prefix, localName);
			this.signingWriter.WriteEndElement(prefix, localName);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001BA02 File Offset: 0x00019C02
		public override void WriteXmlnsAttribute(string prefix, string ns)
		{
			this.writer.WriteXmlnsAttribute(prefix, ns);
			this.signingWriter.WriteXmlnsAttribute(prefix, ns);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001BA1E File Offset: 0x00019C1E
		public override void WriteXmlnsAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] nsBuffer, int nsOffset, int nsLength)
		{
			this.writer.WriteXmlnsAttribute(prefixBuffer, prefixOffset, prefixLength, nsBuffer, nsOffset, nsLength);
			this.signingWriter.WriteXmlnsAttribute(prefixBuffer, prefixOffset, prefixLength, nsBuffer, nsOffset, nsLength);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001BA48 File Offset: 0x00019C48
		public override void WriteXmlnsAttribute(string prefix, XmlDictionaryString ns)
		{
			this.writer.WriteXmlnsAttribute(prefix, ns);
			this.signingWriter.WriteXmlnsAttribute(prefix, ns.Value);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0001BA69 File Offset: 0x00019C69
		public override void WriteStartAttribute(string prefix, string localName)
		{
			this.writer.WriteStartAttribute(prefix, localName);
			this.signingWriter.WriteStartAttribute(prefix, localName);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001BA85 File Offset: 0x00019C85
		public override void WriteStartAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
		{
			this.writer.WriteStartAttribute(prefixBuffer, prefixOffset, prefixLength, localNameBuffer, localNameOffset, localNameLength);
			this.signingWriter.WriteStartAttribute(prefixBuffer, prefixOffset, prefixLength, localNameBuffer, localNameOffset, localNameLength);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001BAAF File Offset: 0x00019CAF
		public override void WriteStartAttribute(string prefix, XmlDictionaryString localName)
		{
			this.writer.WriteStartAttribute(prefix, localName);
			this.signingWriter.WriteStartAttribute(prefix, localName.Value);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001BAD0 File Offset: 0x00019CD0
		public override void WriteEndAttribute()
		{
			this.writer.WriteEndAttribute();
			this.signingWriter.WriteEndAttribute();
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001BAE8 File Offset: 0x00019CE8
		public override void WriteCharEntity(int ch)
		{
			this.writer.WriteCharEntity(ch);
			this.signingWriter.WriteCharEntity(ch);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001BB02 File Offset: 0x00019D02
		public override void WriteEscapedText(string value)
		{
			this.writer.WriteEscapedText(value);
			this.signingWriter.WriteEscapedText(value);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001BB1C File Offset: 0x00019D1C
		public override void WriteEscapedText(char[] chars, int offset, int count)
		{
			this.writer.WriteEscapedText(chars, offset, count);
			this.signingWriter.WriteEscapedText(chars, offset, count);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001BB3A File Offset: 0x00019D3A
		public override void WriteEscapedText(XmlDictionaryString value)
		{
			this.writer.WriteEscapedText(value);
			this.signingWriter.WriteEscapedText(value.Value);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001BB59 File Offset: 0x00019D59
		public override void WriteEscapedText(byte[] chars, int offset, int count)
		{
			this.writer.WriteEscapedText(chars, offset, count);
			this.signingWriter.WriteEscapedText(chars, offset, count);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0001BB77 File Offset: 0x00019D77
		public override void WriteText(string value)
		{
			this.writer.WriteText(value);
			this.signingWriter.WriteText(value);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0001BB91 File Offset: 0x00019D91
		public override void WriteText(char[] chars, int offset, int count)
		{
			this.writer.WriteText(chars, offset, count);
			this.signingWriter.WriteText(chars, offset, count);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001BBAF File Offset: 0x00019DAF
		public override void WriteText(byte[] chars, int offset, int count)
		{
			this.writer.WriteText(chars, offset, count);
			this.signingWriter.WriteText(chars, offset, count);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001BBCD File Offset: 0x00019DCD
		public override void WriteText(XmlDictionaryString value)
		{
			this.writer.WriteText(value);
			this.signingWriter.WriteText(value.Value);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001BBEC File Offset: 0x00019DEC
		public override void WriteInt32Text(int value)
		{
			int num = XmlConverter.ToChars(value, this.chars, 0);
			if (this.text)
			{
				this.writer.WriteText(this.chars, 0, num);
			}
			else
			{
				this.writer.WriteInt32Text(value);
			}
			this.signingWriter.WriteText(this.chars, 0, num);
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001BC44 File Offset: 0x00019E44
		public override void WriteInt64Text(long value)
		{
			int num = XmlConverter.ToChars(value, this.chars, 0);
			if (this.text)
			{
				this.writer.WriteText(this.chars, 0, num);
			}
			else
			{
				this.writer.WriteInt64Text(value);
			}
			this.signingWriter.WriteText(this.chars, 0, num);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001BC9C File Offset: 0x00019E9C
		public override void WriteBoolText(bool value)
		{
			int num = XmlConverter.ToChars(value, this.chars, 0);
			if (this.text)
			{
				this.writer.WriteText(this.chars, 0, num);
			}
			else
			{
				this.writer.WriteBoolText(value);
			}
			this.signingWriter.WriteText(this.chars, 0, num);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0001BCF4 File Offset: 0x00019EF4
		public override void WriteUInt64Text(ulong value)
		{
			int num = XmlConverter.ToChars(value, this.chars, 0);
			if (this.text)
			{
				this.writer.WriteText(this.chars, 0, num);
			}
			else
			{
				this.writer.WriteUInt64Text(value);
			}
			this.signingWriter.WriteText(this.chars, 0, num);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001BD4C File Offset: 0x00019F4C
		public override void WriteFloatText(float value)
		{
			int num = XmlConverter.ToChars(value, this.chars, 0);
			if (this.text)
			{
				this.writer.WriteText(this.chars, 0, num);
			}
			else
			{
				this.writer.WriteFloatText(value);
			}
			this.signingWriter.WriteText(this.chars, 0, num);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0001BDA4 File Offset: 0x00019FA4
		public override void WriteDoubleText(double value)
		{
			int num = XmlConverter.ToChars(value, this.chars, 0);
			if (this.text)
			{
				this.writer.WriteText(this.chars, 0, num);
			}
			else
			{
				this.writer.WriteDoubleText(value);
			}
			this.signingWriter.WriteText(this.chars, 0, num);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0001BDFC File Offset: 0x00019FFC
		public override void WriteDecimalText(decimal value)
		{
			int num = XmlConverter.ToChars(value, this.chars, 0);
			if (this.text)
			{
				this.writer.WriteText(this.chars, 0, num);
			}
			else
			{
				this.writer.WriteDecimalText(value);
			}
			this.signingWriter.WriteText(this.chars, 0, num);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0001BE54 File Offset: 0x0001A054
		public override void WriteDateTimeText(DateTime value)
		{
			int num = XmlConverter.ToChars(value, this.chars, 0);
			if (this.text)
			{
				this.writer.WriteText(this.chars, 0, num);
			}
			else
			{
				this.writer.WriteDateTimeText(value);
			}
			this.signingWriter.WriteText(this.chars, 0, num);
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0001BEAC File Offset: 0x0001A0AC
		public override void WriteUniqueIdText(UniqueId value)
		{
			string text = XmlConverter.ToString(value);
			if (this.text)
			{
				this.writer.WriteText(text);
			}
			else
			{
				this.writer.WriteUniqueIdText(value);
			}
			this.signingWriter.WriteText(text);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0001BEF0 File Offset: 0x0001A0F0
		public override void WriteTimeSpanText(TimeSpan value)
		{
			string text = XmlConverter.ToString(value);
			if (this.text)
			{
				this.writer.WriteText(text);
			}
			else
			{
				this.writer.WriteTimeSpanText(value);
			}
			this.signingWriter.WriteText(text);
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0001BF34 File Offset: 0x0001A134
		public override void WriteGuidText(Guid value)
		{
			string text = XmlConverter.ToString(value);
			if (this.text)
			{
				this.writer.WriteText(text);
			}
			else
			{
				this.writer.WriteGuidText(value);
			}
			this.signingWriter.WriteText(text);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001BF76 File Offset: 0x0001A176
		public override void WriteStartListText()
		{
			this.writer.WriteStartListText();
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0001BF83 File Offset: 0x0001A183
		public override void WriteListSeparator()
		{
			this.writer.WriteListSeparator();
			this.signingWriter.WriteText(32);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0001BF9D File Offset: 0x0001A19D
		public override void WriteEndListText()
		{
			this.writer.WriteEndListText();
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0001BFAA File Offset: 0x0001A1AA
		public override void WriteBase64Text(byte[] trailBytes, int trailByteCount, byte[] buffer, int offset, int count)
		{
			if (trailByteCount > 0)
			{
				this.WriteBase64Text(trailBytes, 0, trailByteCount);
			}
			this.WriteBase64Text(buffer, offset, count);
			if (!this.text)
			{
				this.writer.WriteBase64Text(trailBytes, trailByteCount, buffer, offset, count);
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001BFE0 File Offset: 0x0001A1E0
		private void WriteBase64Text(byte[] buffer, int offset, int count)
		{
			if (this.base64Chars == null)
			{
				this.base64Chars = new byte[512];
			}
			Base64Encoding base64Encoding = XmlConverter.Base64Encoding;
			while (count >= 3)
			{
				int num = Math.Min(this.base64Chars.Length / 4 * 3, count - count % 3);
				int num2 = num / 3 * 4;
				base64Encoding.GetChars(buffer, offset, num, this.base64Chars, 0);
				this.signingWriter.WriteText(this.base64Chars, 0, num2);
				if (this.text)
				{
					this.writer.WriteText(this.base64Chars, 0, num2);
				}
				offset += num;
				count -= num;
			}
			if (count > 0)
			{
				base64Encoding.GetChars(buffer, offset, count, this.base64Chars, 0);
				this.signingWriter.WriteText(this.base64Chars, 0, 4);
				if (this.text)
				{
					this.writer.WriteText(this.base64Chars, 0, 4);
				}
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0001C0BC File Offset: 0x0001A2BC
		public override void WriteQualifiedName(string prefix, XmlDictionaryString localName)
		{
			this.writer.WriteQualifiedName(prefix, localName);
			if (prefix.Length != 0)
			{
				this.signingWriter.WriteText(prefix);
				this.signingWriter.WriteText(":");
			}
			this.signingWriter.WriteText(localName.Value);
		}

		// Token: 0x0400028E RID: 654
		private XmlNodeWriter writer;

		// Token: 0x0400028F RID: 655
		private XmlCanonicalWriter signingWriter;

		// Token: 0x04000290 RID: 656
		private byte[] chars;

		// Token: 0x04000291 RID: 657
		private byte[] base64Chars;

		// Token: 0x04000292 RID: 658
		private bool text;
	}
}
