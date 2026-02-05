using System;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000110 RID: 272
	internal class JsonQNameDataContract : JsonDataContract
	{
		// Token: 0x06001039 RID: 4153 RVA: 0x000423ED File Offset: 0x000405ED
		public JsonQNameDataContract(QNameDataContract traditionalQNameDataContract)
			: base(traditionalQNameDataContract)
		{
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x000423F6 File Offset: 0x000405F6
		public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
		{
			if (context != null)
			{
				return JsonDataContract.HandleReadValue(jsonReader.ReadElementContentAsQName(), context);
			}
			if (!JsonDataContract.TryReadNullAtTopLevel(jsonReader))
			{
				return jsonReader.ReadElementContentAsQName();
			}
			return null;
		}
	}
}
