using System;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000113 RID: 275
	internal class JsonStringDataContract : JsonDataContract
	{
		// Token: 0x06001056 RID: 4182 RVA: 0x00042A91 File Offset: 0x00040C91
		public JsonStringDataContract(StringDataContract traditionalStringDataContract)
			: base(traditionalStringDataContract)
		{
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00042A9A File Offset: 0x00040C9A
		public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
		{
			if (context != null)
			{
				return JsonDataContract.HandleReadValue(jsonReader.ReadElementContentAsString(), context);
			}
			if (!JsonDataContract.TryReadNullAtTopLevel(jsonReader))
			{
				return jsonReader.ReadElementContentAsString();
			}
			return null;
		}
	}
}
