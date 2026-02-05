using System;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000100 RID: 256
	internal class JsonByteArrayDataContract : JsonDataContract
	{
		// Token: 0x06000FD2 RID: 4050 RVA: 0x00040FD4 File Offset: 0x0003F1D4
		public JsonByteArrayDataContract(ByteArrayDataContract traditionalByteArrayDataContract)
			: base(traditionalByteArrayDataContract)
		{
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00040FDD File Offset: 0x0003F1DD
		public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
		{
			if (context != null)
			{
				return JsonDataContract.HandleReadValue(jsonReader.ReadElementContentAsBase64(), context);
			}
			if (!JsonDataContract.TryReadNullAtTopLevel(jsonReader))
			{
				return jsonReader.ReadElementContentAsBase64();
			}
			return null;
		}
	}
}
