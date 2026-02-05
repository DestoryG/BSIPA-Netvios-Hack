using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000A6 RID: 166
	internal class IntDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B29 RID: 2857 RVA: 0x0002E8FE File Offset: 0x0002CAFE
		internal IntDataContract()
			: base(typeof(int), DictionaryGlobals.IntLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x0002E91A File Offset: 0x0002CB1A
		internal override string WriteMethodName
		{
			get
			{
				return "WriteInt";
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x0002E921 File Offset: 0x0002CB21
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsInt";
			}
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0002E928 File Offset: 0x0002CB28
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteInt((int)obj);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0002E936 File Offset: 0x0002CB36
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsInt(), context);
			}
			return reader.ReadElementContentAsInt();
		}
	}
}
