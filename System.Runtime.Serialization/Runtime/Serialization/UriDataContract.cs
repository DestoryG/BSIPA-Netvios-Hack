using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000CE RID: 206
	internal class UriDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B8B RID: 2955 RVA: 0x0002F00C File Offset: 0x0002D20C
		internal UriDataContract()
			: base(typeof(Uri), DictionaryGlobals.UriLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x0002F028 File Offset: 0x0002D228
		internal override string WriteMethodName
		{
			get
			{
				return "WriteUri";
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x0002F02F File Offset: 0x0002D22F
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsUri";
			}
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0002F036 File Offset: 0x0002D236
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteUri((Uri)obj);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0002F044 File Offset: 0x0002D244
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsUri(), context);
			}
			if (!base.TryReadNullAtTopLevel(reader))
			{
				return reader.ReadElementContentAsUri();
			}
			return null;
		}
	}
}
