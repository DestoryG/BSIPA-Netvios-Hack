using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000B1 RID: 177
	internal class DecimalDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B4D RID: 2893 RVA: 0x0002EB84 File Offset: 0x0002CD84
		internal DecimalDataContract()
			: base(typeof(decimal), DictionaryGlobals.DecimalLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x0002EBA0 File Offset: 0x0002CDA0
		internal override string WriteMethodName
		{
			get
			{
				return "WriteDecimal";
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0002EBA7 File Offset: 0x0002CDA7
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsDecimal";
			}
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0002EBAE File Offset: 0x0002CDAE
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteDecimal((decimal)obj);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0002EBBC File Offset: 0x0002CDBC
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsDecimal(), context);
			}
			return reader.ReadElementContentAsDecimal();
		}
	}
}
