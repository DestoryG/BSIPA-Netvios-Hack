using System;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x0200009F RID: 159
	internal class CharDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B09 RID: 2825 RVA: 0x0002E6C0 File Offset: 0x0002C8C0
		internal CharDataContract()
			: this(DictionaryGlobals.CharLocalName, DictionaryGlobals.SerializationNamespace)
		{
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0002E6D2 File Offset: 0x0002C8D2
		internal CharDataContract(XmlDictionaryString name, XmlDictionaryString ns)
			: base(typeof(char), name, ns)
		{
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x0002E6E6 File Offset: 0x0002C8E6
		internal override string WriteMethodName
		{
			get
			{
				return "WriteChar";
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x0002E6ED File Offset: 0x0002C8ED
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsChar";
			}
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0002E6F4 File Offset: 0x0002C8F4
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteChar((char)obj);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0002E702 File Offset: 0x0002C902
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsChar(), context);
			}
			return reader.ReadElementContentAsChar();
		}
	}
}
