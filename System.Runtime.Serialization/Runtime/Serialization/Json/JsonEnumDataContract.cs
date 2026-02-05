using System;
using System.Security;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000105 RID: 261
	internal class JsonEnumDataContract : JsonDataContract
	{
		// Token: 0x06001014 RID: 4116 RVA: 0x00041E71 File Offset: 0x00040071
		[SecuritySafeCritical]
		public JsonEnumDataContract(EnumDataContract traditionalDataContract)
			: base(new JsonEnumDataContract.JsonEnumDataContractCriticalHelper(traditionalDataContract))
		{
			this.helper = base.Helper as JsonEnumDataContract.JsonEnumDataContractCriticalHelper;
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001015 RID: 4117 RVA: 0x00041E90 File Offset: 0x00040090
		public bool IsULong
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsULong;
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00041EA0 File Offset: 0x000400A0
		public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
		{
			object obj;
			if (this.IsULong)
			{
				obj = Enum.ToObject(base.TraditionalDataContract.UnderlyingType, jsonReader.ReadElementContentAsUnsignedLong());
			}
			else
			{
				obj = Enum.ToObject(base.TraditionalDataContract.UnderlyingType, jsonReader.ReadElementContentAsLong());
			}
			if (context != null)
			{
				context.AddNewObject(obj);
			}
			return obj;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00041EF0 File Offset: 0x000400F0
		public override void WriteJsonValueCore(XmlWriterDelegator jsonWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, RuntimeTypeHandle declaredTypeHandle)
		{
			if (this.IsULong)
			{
				jsonWriter.WriteUnsignedLong(((IConvertible)obj).ToUInt64(null));
				return;
			}
			jsonWriter.WriteLong(((IConvertible)obj).ToInt64(null));
		}

		// Token: 0x040007F0 RID: 2032
		[SecurityCritical]
		private JsonEnumDataContract.JsonEnumDataContractCriticalHelper helper;

		// Token: 0x0200018B RID: 395
		[SecurityCritical(SecurityCriticalScope.Everything)]
		private class JsonEnumDataContractCriticalHelper : JsonDataContract.JsonDataContractCriticalHelper
		{
			// Token: 0x06001527 RID: 5415 RVA: 0x00055264 File Offset: 0x00053464
			public JsonEnumDataContractCriticalHelper(EnumDataContract traditionalEnumDataContract)
				: base(traditionalEnumDataContract)
			{
				this.isULong = traditionalEnumDataContract.IsULong;
			}

			// Token: 0x1700046D RID: 1133
			// (get) Token: 0x06001528 RID: 5416 RVA: 0x00055279 File Offset: 0x00053479
			public bool IsULong
			{
				get
				{
					return this.isULong;
				}
			}

			// Token: 0x04000A58 RID: 2648
			private bool isULong;
		}
	}
}
