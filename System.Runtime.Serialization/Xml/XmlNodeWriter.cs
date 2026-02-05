using System;
using System.Runtime;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000053 RID: 83
	internal abstract class XmlNodeWriter
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x0001B74A File Offset: 0x0001994A
		public static XmlNodeWriter Null
		{
			get
			{
				if (XmlNodeWriter.nullNodeWriter == null)
				{
					XmlNodeWriter.nullNodeWriter = new XmlNodeWriter.XmlNullNodeWriter();
				}
				return XmlNodeWriter.nullNodeWriter;
			}
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001B762 File Offset: 0x00019962
		internal virtual AsyncCompletionResult WriteBase64TextAsync(AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs> state)
		{
			throw Fx.AssertAndThrow("WriteBase64TextAsync not implemented.");
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001B76E File Offset: 0x0001996E
		public virtual IAsyncResult BeginWriteBase64Text(byte[] trailBuffer, int trailCount, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return new XmlNodeWriter.WriteBase64TextAsyncResult(trailBuffer, trailCount, buffer, offset, count, this, callback, state);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001B781 File Offset: 0x00019981
		public virtual void EndWriteBase64Text(IAsyncResult result)
		{
			ScheduleActionItemAsyncResult.End(result);
		}

		// Token: 0x060005B5 RID: 1461
		public abstract void Flush();

		// Token: 0x060005B6 RID: 1462
		public abstract void Close();

		// Token: 0x060005B7 RID: 1463
		public abstract void WriteDeclaration();

		// Token: 0x060005B8 RID: 1464
		public abstract void WriteComment(string text);

		// Token: 0x060005B9 RID: 1465
		public abstract void WriteCData(string text);

		// Token: 0x060005BA RID: 1466
		public abstract void WriteStartElement(string prefix, string localName);

		// Token: 0x060005BB RID: 1467 RVA: 0x0001B789 File Offset: 0x00019989
		public virtual void WriteStartElement(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
		{
			this.WriteStartElement(Encoding.UTF8.GetString(prefixBuffer, prefixOffset, prefixLength), Encoding.UTF8.GetString(localNameBuffer, localNameOffset, localNameLength));
		}

		// Token: 0x060005BC RID: 1468
		public abstract void WriteStartElement(string prefix, XmlDictionaryString localName);

		// Token: 0x060005BD RID: 1469
		public abstract void WriteEndStartElement(bool isEmpty);

		// Token: 0x060005BE RID: 1470
		public abstract void WriteEndElement(string prefix, string localName);

		// Token: 0x060005BF RID: 1471 RVA: 0x0001B7AE File Offset: 0x000199AE
		public virtual void WriteEndElement(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
		{
			this.WriteEndElement(Encoding.UTF8.GetString(prefixBuffer, prefixOffset, prefixLength), Encoding.UTF8.GetString(localNameBuffer, localNameOffset, localNameLength));
		}

		// Token: 0x060005C0 RID: 1472
		public abstract void WriteXmlnsAttribute(string prefix, string ns);

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001B7D3 File Offset: 0x000199D3
		public virtual void WriteXmlnsAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] nsBuffer, int nsOffset, int nsLength)
		{
			this.WriteXmlnsAttribute(Encoding.UTF8.GetString(prefixBuffer, prefixOffset, prefixLength), Encoding.UTF8.GetString(nsBuffer, nsOffset, nsLength));
		}

		// Token: 0x060005C2 RID: 1474
		public abstract void WriteXmlnsAttribute(string prefix, XmlDictionaryString ns);

		// Token: 0x060005C3 RID: 1475
		public abstract void WriteStartAttribute(string prefix, string localName);

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001B7F8 File Offset: 0x000199F8
		public virtual void WriteStartAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
		{
			this.WriteStartAttribute(Encoding.UTF8.GetString(prefixBuffer, prefixOffset, prefixLength), Encoding.UTF8.GetString(localNameBuffer, localNameOffset, localNameLength));
		}

		// Token: 0x060005C5 RID: 1477
		public abstract void WriteStartAttribute(string prefix, XmlDictionaryString localName);

		// Token: 0x060005C6 RID: 1478
		public abstract void WriteEndAttribute();

		// Token: 0x060005C7 RID: 1479
		public abstract void WriteCharEntity(int ch);

		// Token: 0x060005C8 RID: 1480
		public abstract void WriteEscapedText(string value);

		// Token: 0x060005C9 RID: 1481
		public abstract void WriteEscapedText(XmlDictionaryString value);

		// Token: 0x060005CA RID: 1482
		public abstract void WriteEscapedText(char[] chars, int offset, int count);

		// Token: 0x060005CB RID: 1483
		public abstract void WriteEscapedText(byte[] buffer, int offset, int count);

		// Token: 0x060005CC RID: 1484
		public abstract void WriteText(string value);

		// Token: 0x060005CD RID: 1485
		public abstract void WriteText(XmlDictionaryString value);

		// Token: 0x060005CE RID: 1486
		public abstract void WriteText(char[] chars, int offset, int count);

		// Token: 0x060005CF RID: 1487
		public abstract void WriteText(byte[] buffer, int offset, int count);

		// Token: 0x060005D0 RID: 1488
		public abstract void WriteInt32Text(int value);

		// Token: 0x060005D1 RID: 1489
		public abstract void WriteInt64Text(long value);

		// Token: 0x060005D2 RID: 1490
		public abstract void WriteBoolText(bool value);

		// Token: 0x060005D3 RID: 1491
		public abstract void WriteUInt64Text(ulong value);

		// Token: 0x060005D4 RID: 1492
		public abstract void WriteFloatText(float value);

		// Token: 0x060005D5 RID: 1493
		public abstract void WriteDoubleText(double value);

		// Token: 0x060005D6 RID: 1494
		public abstract void WriteDecimalText(decimal value);

		// Token: 0x060005D7 RID: 1495
		public abstract void WriteDateTimeText(DateTime value);

		// Token: 0x060005D8 RID: 1496
		public abstract void WriteUniqueIdText(UniqueId value);

		// Token: 0x060005D9 RID: 1497
		public abstract void WriteTimeSpanText(TimeSpan value);

		// Token: 0x060005DA RID: 1498
		public abstract void WriteGuidText(Guid value);

		// Token: 0x060005DB RID: 1499
		public abstract void WriteStartListText();

		// Token: 0x060005DC RID: 1500
		public abstract void WriteListSeparator();

		// Token: 0x060005DD RID: 1501
		public abstract void WriteEndListText();

		// Token: 0x060005DE RID: 1502
		public abstract void WriteBase64Text(byte[] trailBuffer, int trailCount, byte[] buffer, int offset, int count);

		// Token: 0x060005DF RID: 1503
		public abstract void WriteQualifiedName(string prefix, XmlDictionaryString localName);

		// Token: 0x04000288 RID: 648
		private static XmlNodeWriter nullNodeWriter;

		// Token: 0x02000160 RID: 352
		private class XmlNullNodeWriter : XmlNodeWriter
		{
			// Token: 0x0600138E RID: 5006 RVA: 0x0004F9E1 File Offset: 0x0004DBE1
			public override void Flush()
			{
			}

			// Token: 0x0600138F RID: 5007 RVA: 0x0004F9E3 File Offset: 0x0004DBE3
			public override void Close()
			{
			}

			// Token: 0x06001390 RID: 5008 RVA: 0x0004F9E5 File Offset: 0x0004DBE5
			public override void WriteDeclaration()
			{
			}

			// Token: 0x06001391 RID: 5009 RVA: 0x0004F9E7 File Offset: 0x0004DBE7
			public override void WriteComment(string text)
			{
			}

			// Token: 0x06001392 RID: 5010 RVA: 0x0004F9E9 File Offset: 0x0004DBE9
			public override void WriteCData(string text)
			{
			}

			// Token: 0x06001393 RID: 5011 RVA: 0x0004F9EB File Offset: 0x0004DBEB
			public override void WriteStartElement(string prefix, string localName)
			{
			}

			// Token: 0x06001394 RID: 5012 RVA: 0x0004F9ED File Offset: 0x0004DBED
			public override void WriteStartElement(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
			{
			}

			// Token: 0x06001395 RID: 5013 RVA: 0x0004F9EF File Offset: 0x0004DBEF
			public override void WriteStartElement(string prefix, XmlDictionaryString localName)
			{
			}

			// Token: 0x06001396 RID: 5014 RVA: 0x0004F9F1 File Offset: 0x0004DBF1
			public override void WriteEndStartElement(bool isEmpty)
			{
			}

			// Token: 0x06001397 RID: 5015 RVA: 0x0004F9F3 File Offset: 0x0004DBF3
			public override void WriteEndElement(string prefix, string localName)
			{
			}

			// Token: 0x06001398 RID: 5016 RVA: 0x0004F9F5 File Offset: 0x0004DBF5
			public override void WriteEndElement(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
			{
			}

			// Token: 0x06001399 RID: 5017 RVA: 0x0004F9F7 File Offset: 0x0004DBF7
			public override void WriteXmlnsAttribute(string prefix, string ns)
			{
			}

			// Token: 0x0600139A RID: 5018 RVA: 0x0004F9F9 File Offset: 0x0004DBF9
			public override void WriteXmlnsAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] nsBuffer, int nsOffset, int nsLength)
			{
			}

			// Token: 0x0600139B RID: 5019 RVA: 0x0004F9FB File Offset: 0x0004DBFB
			public override void WriteXmlnsAttribute(string prefix, XmlDictionaryString ns)
			{
			}

			// Token: 0x0600139C RID: 5020 RVA: 0x0004F9FD File Offset: 0x0004DBFD
			public override void WriteStartAttribute(string prefix, string localName)
			{
			}

			// Token: 0x0600139D RID: 5021 RVA: 0x0004F9FF File Offset: 0x0004DBFF
			public override void WriteStartAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
			{
			}

			// Token: 0x0600139E RID: 5022 RVA: 0x0004FA01 File Offset: 0x0004DC01
			public override void WriteStartAttribute(string prefix, XmlDictionaryString localName)
			{
			}

			// Token: 0x0600139F RID: 5023 RVA: 0x0004FA03 File Offset: 0x0004DC03
			public override void WriteEndAttribute()
			{
			}

			// Token: 0x060013A0 RID: 5024 RVA: 0x0004FA05 File Offset: 0x0004DC05
			public override void WriteCharEntity(int ch)
			{
			}

			// Token: 0x060013A1 RID: 5025 RVA: 0x0004FA07 File Offset: 0x0004DC07
			public override void WriteEscapedText(string value)
			{
			}

			// Token: 0x060013A2 RID: 5026 RVA: 0x0004FA09 File Offset: 0x0004DC09
			public override void WriteEscapedText(XmlDictionaryString value)
			{
			}

			// Token: 0x060013A3 RID: 5027 RVA: 0x0004FA0B File Offset: 0x0004DC0B
			public override void WriteEscapedText(char[] chars, int offset, int count)
			{
			}

			// Token: 0x060013A4 RID: 5028 RVA: 0x0004FA0D File Offset: 0x0004DC0D
			public override void WriteEscapedText(byte[] buffer, int offset, int count)
			{
			}

			// Token: 0x060013A5 RID: 5029 RVA: 0x0004FA0F File Offset: 0x0004DC0F
			public override void WriteText(string value)
			{
			}

			// Token: 0x060013A6 RID: 5030 RVA: 0x0004FA11 File Offset: 0x0004DC11
			public override void WriteText(XmlDictionaryString value)
			{
			}

			// Token: 0x060013A7 RID: 5031 RVA: 0x0004FA13 File Offset: 0x0004DC13
			public override void WriteText(char[] chars, int offset, int count)
			{
			}

			// Token: 0x060013A8 RID: 5032 RVA: 0x0004FA15 File Offset: 0x0004DC15
			public override void WriteText(byte[] buffer, int offset, int count)
			{
			}

			// Token: 0x060013A9 RID: 5033 RVA: 0x0004FA17 File Offset: 0x0004DC17
			public override void WriteInt32Text(int value)
			{
			}

			// Token: 0x060013AA RID: 5034 RVA: 0x0004FA19 File Offset: 0x0004DC19
			public override void WriteInt64Text(long value)
			{
			}

			// Token: 0x060013AB RID: 5035 RVA: 0x0004FA1B File Offset: 0x0004DC1B
			public override void WriteBoolText(bool value)
			{
			}

			// Token: 0x060013AC RID: 5036 RVA: 0x0004FA1D File Offset: 0x0004DC1D
			public override void WriteUInt64Text(ulong value)
			{
			}

			// Token: 0x060013AD RID: 5037 RVA: 0x0004FA1F File Offset: 0x0004DC1F
			public override void WriteFloatText(float value)
			{
			}

			// Token: 0x060013AE RID: 5038 RVA: 0x0004FA21 File Offset: 0x0004DC21
			public override void WriteDoubleText(double value)
			{
			}

			// Token: 0x060013AF RID: 5039 RVA: 0x0004FA23 File Offset: 0x0004DC23
			public override void WriteDecimalText(decimal value)
			{
			}

			// Token: 0x060013B0 RID: 5040 RVA: 0x0004FA25 File Offset: 0x0004DC25
			public override void WriteDateTimeText(DateTime value)
			{
			}

			// Token: 0x060013B1 RID: 5041 RVA: 0x0004FA27 File Offset: 0x0004DC27
			public override void WriteUniqueIdText(UniqueId value)
			{
			}

			// Token: 0x060013B2 RID: 5042 RVA: 0x0004FA29 File Offset: 0x0004DC29
			public override void WriteTimeSpanText(TimeSpan value)
			{
			}

			// Token: 0x060013B3 RID: 5043 RVA: 0x0004FA2B File Offset: 0x0004DC2B
			public override void WriteGuidText(Guid value)
			{
			}

			// Token: 0x060013B4 RID: 5044 RVA: 0x0004FA2D File Offset: 0x0004DC2D
			public override void WriteStartListText()
			{
			}

			// Token: 0x060013B5 RID: 5045 RVA: 0x0004FA2F File Offset: 0x0004DC2F
			public override void WriteListSeparator()
			{
			}

			// Token: 0x060013B6 RID: 5046 RVA: 0x0004FA31 File Offset: 0x0004DC31
			public override void WriteEndListText()
			{
			}

			// Token: 0x060013B7 RID: 5047 RVA: 0x0004FA33 File Offset: 0x0004DC33
			public override void WriteBase64Text(byte[] trailBuffer, int trailCount, byte[] buffer, int offset, int count)
			{
			}

			// Token: 0x060013B8 RID: 5048 RVA: 0x0004FA35 File Offset: 0x0004DC35
			public override void WriteQualifiedName(string prefix, XmlDictionaryString localName)
			{
			}
		}

		// Token: 0x02000161 RID: 353
		private class WriteBase64TextAsyncResult : ScheduleActionItemAsyncResult
		{
			// Token: 0x060013BA RID: 5050 RVA: 0x0004FA3F File Offset: 0x0004DC3F
			public WriteBase64TextAsyncResult(byte[] trailBuffer, int trailCount, byte[] buffer, int offset, int count, XmlNodeWriter nodeWriter, AsyncCallback callback, object state)
				: base(callback, state)
			{
				this.trailBuffer = trailBuffer;
				this.trailCount = trailCount;
				this.buffer = buffer;
				this.offset = offset;
				this.count = count;
				this.nodeWriter = nodeWriter;
				base.Schedule();
			}

			// Token: 0x060013BB RID: 5051 RVA: 0x0004FA7E File Offset: 0x0004DC7E
			protected override void OnDoWork()
			{
				this.nodeWriter.WriteBase64Text(this.trailBuffer, this.trailCount, this.buffer, this.offset, this.count);
			}

			// Token: 0x04000984 RID: 2436
			private byte[] trailBuffer;

			// Token: 0x04000985 RID: 2437
			private int trailCount;

			// Token: 0x04000986 RID: 2438
			private byte[] buffer;

			// Token: 0x04000987 RID: 2439
			private int offset;

			// Token: 0x04000988 RID: 2440
			private int count;

			// Token: 0x04000989 RID: 2441
			private XmlNodeWriter nodeWriter;
		}
	}
}
