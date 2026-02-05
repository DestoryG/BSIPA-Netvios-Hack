using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000A3 RID: 163
	internal class UnsignedByteDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B1A RID: 2842 RVA: 0x0002E7ED File Offset: 0x0002C9ED
		internal UnsignedByteDataContract()
			: base(typeof(byte), DictionaryGlobals.UnsignedByteLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x0002E809 File Offset: 0x0002CA09
		internal override string WriteMethodName
		{
			get
			{
				return "WriteUnsignedByte";
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x0002E810 File Offset: 0x0002CA10
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsUnsignedByte";
			}
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0002E817 File Offset: 0x0002CA17
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteUnsignedByte((byte)obj);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0002E825 File Offset: 0x0002CA25
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsUnsignedByte(), context);
			}
			return reader.ReadElementContentAsUnsignedByte();
		}
	}
}
