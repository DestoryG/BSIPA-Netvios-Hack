using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000A4 RID: 164
	internal class ShortDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B1F RID: 2847 RVA: 0x0002E848 File Offset: 0x0002CA48
		internal ShortDataContract()
			: base(typeof(short), DictionaryGlobals.ShortLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x0002E864 File Offset: 0x0002CA64
		internal override string WriteMethodName
		{
			get
			{
				return "WriteShort";
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x0002E86B File Offset: 0x0002CA6B
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsShort";
			}
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0002E872 File Offset: 0x0002CA72
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteShort((short)obj);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0002E880 File Offset: 0x0002CA80
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsShort(), context);
			}
			return reader.ReadElementContentAsShort();
		}
	}
}
