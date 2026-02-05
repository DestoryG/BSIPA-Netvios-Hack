using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000AE RID: 174
	internal class UnsignedLongDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B3E RID: 2878 RVA: 0x0002EA73 File Offset: 0x0002CC73
		internal UnsignedLongDataContract()
			: base(typeof(ulong), DictionaryGlobals.UnsignedLongLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000B3F RID: 2879 RVA: 0x0002EA8F File Offset: 0x0002CC8F
		internal override string WriteMethodName
		{
			get
			{
				return "WriteUnsignedLong";
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x0002EA96 File Offset: 0x0002CC96
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsUnsignedLong";
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0002EA9D File Offset: 0x0002CC9D
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteUnsignedLong((ulong)obj);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0002EAAB File Offset: 0x0002CCAB
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsUnsignedLong(), context);
			}
			return reader.ReadElementContentAsUnsignedLong();
		}
	}
}
