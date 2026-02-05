using System;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000CF RID: 207
	internal class QNameDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B90 RID: 2960 RVA: 0x0002F068 File Offset: 0x0002D268
		internal QNameDataContract()
			: base(typeof(XmlQualifiedName), DictionaryGlobals.QNameLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x0002F084 File Offset: 0x0002D284
		internal override string WriteMethodName
		{
			get
			{
				return "WriteQName";
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0002F08B File Offset: 0x0002D28B
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsQName";
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x0002F092 File Offset: 0x0002D292
		internal override bool IsPrimitive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0002F095 File Offset: 0x0002D295
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteQName((XmlQualifiedName)obj);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0002F0A3 File Offset: 0x0002D2A3
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsQName(), context);
			}
			if (!base.TryReadNullAtTopLevel(reader))
			{
				return reader.ReadElementContentAsQName();
			}
			return null;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0002F0C8 File Offset: 0x0002D2C8
		internal override void WriteRootElement(XmlWriterDelegator writer, XmlDictionaryString name, XmlDictionaryString ns)
		{
			if (ns == DictionaryGlobals.SerializationNamespace)
			{
				writer.WriteStartElement("z", name, ns);
				return;
			}
			if (ns != null && ns.Value != null && ns.Value.Length > 0)
			{
				writer.WriteStartElement("q", name, ns);
				return;
			}
			writer.WriteStartElement(name, ns);
		}
	}
}
