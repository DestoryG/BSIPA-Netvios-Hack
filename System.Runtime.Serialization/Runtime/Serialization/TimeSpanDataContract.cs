using System;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000CA RID: 202
	internal class TimeSpanDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B7D RID: 2941 RVA: 0x0002EF1E File Offset: 0x0002D11E
		internal TimeSpanDataContract()
			: this(DictionaryGlobals.TimeSpanLocalName, DictionaryGlobals.SerializationNamespace)
		{
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0002EF30 File Offset: 0x0002D130
		internal TimeSpanDataContract(XmlDictionaryString name, XmlDictionaryString ns)
			: base(typeof(TimeSpan), name, ns)
		{
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x0002EF44 File Offset: 0x0002D144
		internal override string WriteMethodName
		{
			get
			{
				return "WriteTimeSpan";
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0002EF4B File Offset: 0x0002D14B
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsTimeSpan";
			}
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0002EF52 File Offset: 0x0002D152
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteTimeSpan((TimeSpan)obj);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0002EF60 File Offset: 0x0002D160
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsTimeSpan(), context);
			}
			return reader.ReadElementContentAsTimeSpan();
		}
	}
}
