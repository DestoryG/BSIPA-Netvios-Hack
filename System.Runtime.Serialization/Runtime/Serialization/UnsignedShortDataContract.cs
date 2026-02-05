using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000A5 RID: 165
	internal class UnsignedShortDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B24 RID: 2852 RVA: 0x0002E8A3 File Offset: 0x0002CAA3
		internal UnsignedShortDataContract()
			: base(typeof(ushort), DictionaryGlobals.UnsignedShortLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x0002E8BF File Offset: 0x0002CABF
		internal override string WriteMethodName
		{
			get
			{
				return "WriteUnsignedShort";
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x0002E8C6 File Offset: 0x0002CAC6
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsUnsignedShort";
			}
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0002E8CD File Offset: 0x0002CACD
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteUnsignedShort((ushort)obj);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0002E8DB File Offset: 0x0002CADB
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsUnsignedShort(), context);
			}
			return reader.ReadElementContentAsUnsignedShort();
		}
	}
}
