using System;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000114 RID: 276
	internal class JsonUriDataContract : JsonDataContract
	{
		// Token: 0x06001058 RID: 4184 RVA: 0x00042ABC File Offset: 0x00040CBC
		public JsonUriDataContract(UriDataContract traditionalUriDataContract)
			: base(traditionalUriDataContract)
		{
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00042AC5 File Offset: 0x00040CC5
		public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
		{
			if (context != null)
			{
				return JsonDataContract.HandleReadValue(jsonReader.ReadElementContentAsUri(), context);
			}
			if (!JsonDataContract.TryReadNullAtTopLevel(jsonReader))
			{
				return jsonReader.ReadElementContentAsUri();
			}
			return null;
		}
	}
}
