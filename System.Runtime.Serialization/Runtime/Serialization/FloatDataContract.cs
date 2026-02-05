using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000AF RID: 175
	internal class FloatDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B43 RID: 2883 RVA: 0x0002EACE File Offset: 0x0002CCCE
		internal FloatDataContract()
			: base(typeof(float), DictionaryGlobals.FloatLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x0002EAEA File Offset: 0x0002CCEA
		internal override string WriteMethodName
		{
			get
			{
				return "WriteFloat";
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0002EAF1 File Offset: 0x0002CCF1
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsFloat";
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0002EAF8 File Offset: 0x0002CCF8
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteFloat((float)obj);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0002EB06 File Offset: 0x0002CD06
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsFloat(), context);
			}
			return reader.ReadElementContentAsFloat();
		}
	}
}
