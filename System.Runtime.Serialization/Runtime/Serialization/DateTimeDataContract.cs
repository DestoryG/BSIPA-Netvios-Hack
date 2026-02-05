using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000B2 RID: 178
	internal class DateTimeDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B52 RID: 2898 RVA: 0x0002EBDF File Offset: 0x0002CDDF
		internal DateTimeDataContract()
			: base(typeof(DateTime), DictionaryGlobals.DateTimeLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x0002EBFB File Offset: 0x0002CDFB
		internal override string WriteMethodName
		{
			get
			{
				return "WriteDateTime";
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x0002EC02 File Offset: 0x0002CE02
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsDateTime";
			}
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0002EC09 File Offset: 0x0002CE09
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteDateTime((DateTime)obj);
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0002EC17 File Offset: 0x0002CE17
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsDateTime(), context);
			}
			return reader.ReadElementContentAsDateTime();
		}
	}
}
