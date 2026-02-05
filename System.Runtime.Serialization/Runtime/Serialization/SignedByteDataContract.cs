using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000A2 RID: 162
	internal class SignedByteDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B15 RID: 2837 RVA: 0x0002E792 File Offset: 0x0002C992
		internal SignedByteDataContract()
			: base(typeof(sbyte), DictionaryGlobals.SignedByteLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000B16 RID: 2838 RVA: 0x0002E7AE File Offset: 0x0002C9AE
		internal override string WriteMethodName
		{
			get
			{
				return "WriteSignedByte";
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0002E7B5 File Offset: 0x0002C9B5
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsSignedByte";
			}
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0002E7BC File Offset: 0x0002C9BC
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteSignedByte((sbyte)obj);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0002E7CA File Offset: 0x0002C9CA
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsSignedByte(), context);
			}
			return reader.ReadElementContentAsSignedByte();
		}
	}
}
