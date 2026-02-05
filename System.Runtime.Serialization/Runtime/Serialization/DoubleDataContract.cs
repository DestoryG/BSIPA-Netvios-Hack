using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000B0 RID: 176
	internal class DoubleDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B48 RID: 2888 RVA: 0x0002EB29 File Offset: 0x0002CD29
		internal DoubleDataContract()
			: base(typeof(double), DictionaryGlobals.DoubleLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x0002EB45 File Offset: 0x0002CD45
		internal override string WriteMethodName
		{
			get
			{
				return "WriteDouble";
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x0002EB4C File Offset: 0x0002CD4C
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsDouble";
			}
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0002EB53 File Offset: 0x0002CD53
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteDouble((double)obj);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0002EB61 File Offset: 0x0002CD61
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsDouble(), context);
			}
			return reader.ReadElementContentAsDouble();
		}
	}
}
