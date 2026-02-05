using System;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000B3 RID: 179
	internal class StringDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B57 RID: 2903 RVA: 0x0002EC3A File Offset: 0x0002CE3A
		internal StringDataContract()
			: this(DictionaryGlobals.StringLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0002EC4C File Offset: 0x0002CE4C
		internal StringDataContract(XmlDictionaryString name, XmlDictionaryString ns)
			: base(typeof(string), name, ns)
		{
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0002EC60 File Offset: 0x0002CE60
		internal override string WriteMethodName
		{
			get
			{
				return "WriteString";
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000B5A RID: 2906 RVA: 0x0002EC67 File Offset: 0x0002CE67
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsString";
			}
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0002EC6E File Offset: 0x0002CE6E
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteString((string)obj);
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0002EC7C File Offset: 0x0002CE7C
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsString(), context);
			}
			if (!base.TryReadNullAtTopLevel(reader))
			{
				return reader.ReadElementContentAsString();
			}
			return null;
		}
	}
}
