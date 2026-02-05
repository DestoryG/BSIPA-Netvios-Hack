using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000C8 RID: 200
	internal class ByteArrayDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B71 RID: 2929 RVA: 0x0002EE08 File Offset: 0x0002D008
		internal ByteArrayDataContract()
			: base(typeof(byte[]), DictionaryGlobals.ByteArrayLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x0002EE24 File Offset: 0x0002D024
		internal override string WriteMethodName
		{
			get
			{
				return "WriteBase64";
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x0002EE2B File Offset: 0x0002D02B
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsBase64";
			}
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0002EE32 File Offset: 0x0002D032
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteBase64((byte[])obj);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0002EE40 File Offset: 0x0002D040
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsBase64(), context);
			}
			if (!base.TryReadNullAtTopLevel(reader))
			{
				return reader.ReadElementContentAsBase64();
			}
			return null;
		}
	}
}
