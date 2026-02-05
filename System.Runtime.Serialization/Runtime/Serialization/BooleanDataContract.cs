using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000A1 RID: 161
	internal class BooleanDataContract : PrimitiveDataContract
	{
		// Token: 0x06000B10 RID: 2832 RVA: 0x0002E737 File Offset: 0x0002C937
		internal BooleanDataContract()
			: base(typeof(bool), DictionaryGlobals.BooleanLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x0002E753 File Offset: 0x0002C953
		internal override string WriteMethodName
		{
			get
			{
				return "WriteBoolean";
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x0002E75A File Offset: 0x0002C95A
		internal override string ReadMethodName
		{
			get
			{
				return "ReadElementContentAsBoolean";
			}
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0002E761 File Offset: 0x0002C961
		public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
		{
			writer.WriteBoolean((bool)obj);
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0002E76F File Offset: 0x0002C96F
		public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
		{
			if (context != null)
			{
				return base.HandleReadValue(reader.ReadElementContentAsBoolean(), context);
			}
			return reader.ReadElementContentAsBoolean();
		}
	}
}
