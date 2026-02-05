using System;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000C9 RID: 201
	internal class ObjectDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B76 RID: 2934 RVA: 0x0002EE64 File Offset: 0x0002D064
		internal ObjectDataContract()
			: base(typeof(object), DictionaryGlobals.ObjectLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x0002EE80 File Offset: 0x0002D080
		internal override string WriteMethodName
		{
			get
			{
				return "WriteAnyType";
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x0002EE87 File Offset: 0x0002D087
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsAnyType";
			}
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0002EE8E File Offset: 0x0002D08E
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0002EE90 File Offset: 0x0002D090
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			object obj;
			if (reader.IsEmptyElement)
			{
				reader.Skip();
				obj = new object();
			}
			else
			{
				string localName = reader.LocalName;
				string namespaceURI = reader.NamespaceURI;
				reader.Read();
				try
				{
					reader.ReadEndElement();
					obj = new object();
				}
				catch (XmlException ex)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Element {0} from namespace {1} cannot have child contents to be deserialized as an object. Please use XElement to deserialize this pattern of XML.", new object[] { localName, namespaceURI }), ex));
				}
			}
			if (context != null)
			{
				return base.HandleReadValue(obj, context);
			}
			return obj;
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x0002EF18 File Offset: 0x0002D118
		internal override bool CanContainReferences
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x0002EF1B File Offset: 0x0002D11B
		internal override bool IsPrimitive
		{
			get
			{
				return false;
			}
		}
	}
}
