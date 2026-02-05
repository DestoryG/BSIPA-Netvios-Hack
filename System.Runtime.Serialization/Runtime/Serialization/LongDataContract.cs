using System;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000A8 RID: 168
	internal class LongDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B33 RID: 2867 RVA: 0x0002E9B4 File Offset: 0x0002CBB4
		internal LongDataContract()
			: this(DictionaryGlobals.LongLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0002E9C6 File Offset: 0x0002CBC6
		internal LongDataContract(XmlDictionaryString name, XmlDictionaryString ns)
			: base(typeof(long), name, ns)
		{
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x0002E9DA File Offset: 0x0002CBDA
		internal override string WriteMethodName
		{
			get
			{
				return "WriteLong";
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x0002E9E1 File Offset: 0x0002CBE1
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsLong";
			}
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0002E9E8 File Offset: 0x0002CBE8
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteLong((long)obj);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0002E9F6 File Offset: 0x0002CBF6
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsLong(), context);
			}
			return reader.ReadElementContentAsLong();
		}
	}
}
