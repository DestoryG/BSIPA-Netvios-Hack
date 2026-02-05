using System;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000CC RID: 204
	internal class GuidDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B84 RID: 2948 RVA: 0x0002EF95 File Offset: 0x0002D195
		internal GuidDataContract()
			: this(DictionaryGlobals.GuidLocalName, DictionaryGlobals.SerializationNamespace)
		{
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0002EFA7 File Offset: 0x0002D1A7
		internal GuidDataContract(XmlDictionaryString name, XmlDictionaryString ns)
			: base(typeof(Guid), name, ns)
		{
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x0002EFBB File Offset: 0x0002D1BB
		internal override string WriteMethodName
		{
			get
			{
				return "WriteGuid";
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x0002EFC2 File Offset: 0x0002D1C2
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsGuid";
			}
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002EFC9 File Offset: 0x0002D1C9
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteGuid((Guid)obj);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0002EFD7 File Offset: 0x0002D1D7
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsGuid(), context);
			}
			return reader.ReadElementContentAsGuid();
		}
	}
}
