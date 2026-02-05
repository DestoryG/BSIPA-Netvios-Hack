using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000A7 RID: 167
	internal class UnsignedIntDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B2E RID: 2862 RVA: 0x0002E959 File Offset: 0x0002CB59
		internal UnsignedIntDataContract()
			: base(typeof(uint), DictionaryGlobals.UnsignedIntLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000B2F RID: 2863 RVA: 0x0002E975 File Offset: 0x0002CB75
		internal override string WriteMethodName
		{
			get
			{
				return "WriteUnsignedInt";
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0002E97C File Offset: 0x0002CB7C
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsUnsignedInt";
			}
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0002E983 File Offset: 0x0002CB83
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteUnsignedInt((uint)obj);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0002E991 File Offset: 0x0002CB91
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsUnsignedInt(), context);
			}
			return reader.ReadElementContentAsUnsignedInt();
		}
	}
}
