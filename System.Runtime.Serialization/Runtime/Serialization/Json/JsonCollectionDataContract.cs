using System;
using System.Security;
using System.Threading;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000102 RID: 258
	internal class JsonCollectionDataContract : JsonDataContract
	{
		// Token: 0x06000FDC RID: 4060 RVA: 0x000411B4 File Offset: 0x0003F3B4
		[SecuritySafeCritical]
		public JsonCollectionDataContract(CollectionDataContract traditionalDataContract)
			: base(new JsonCollectionDataContract.JsonCollectionDataContractCriticalHelper(traditionalDataContract))
		{
			this.helper = base.Helper as JsonCollectionDataContract.JsonCollectionDataContractCriticalHelper;
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000FDD RID: 4061 RVA: 0x000411D4 File Offset: 0x0003F3D4
		internal JsonFormatCollectionReaderDelegate JsonFormatReaderDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (this.helper.JsonFormatReaderDelegate == null)
				{
					lock (this)
					{
						if (this.helper.JsonFormatReaderDelegate == null)
						{
							if (this.TraditionalCollectionDataContract.IsReadOnlyContract)
							{
								DataContract.ThrowInvalidDataContractException(this.TraditionalCollectionDataContract.DeserializationExceptionMessage, null);
							}
							JsonFormatCollectionReaderDelegate jsonFormatCollectionReaderDelegate = new JsonFormatReaderGenerator().GenerateCollectionReader(this.TraditionalCollectionDataContract);
							Thread.MemoryBarrier();
							this.helper.JsonFormatReaderDelegate = jsonFormatCollectionReaderDelegate;
						}
					}
				}
				return this.helper.JsonFormatReaderDelegate;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x00041270 File Offset: 0x0003F470
		internal JsonFormatGetOnlyCollectionReaderDelegate JsonFormatGetOnlyReaderDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (this.helper.JsonFormatGetOnlyReaderDelegate == null)
				{
					lock (this)
					{
						if (this.helper.JsonFormatGetOnlyReaderDelegate == null)
						{
							CollectionKind kind = this.TraditionalCollectionDataContract.Kind;
							if (base.TraditionalDataContract.UnderlyingType.IsInterface && (kind == CollectionKind.Enumerable || kind == CollectionKind.Collection || kind == CollectionKind.GenericEnumerable))
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("On type '{0}', get-only collection must have an Add method.", new object[] { DataContract.GetClrTypeFullName(base.TraditionalDataContract.UnderlyingType) })));
							}
							if (this.TraditionalCollectionDataContract.IsReadOnlyContract)
							{
								DataContract.ThrowInvalidDataContractException(this.TraditionalCollectionDataContract.DeserializationExceptionMessage, null);
							}
							JsonFormatGetOnlyCollectionReaderDelegate jsonFormatGetOnlyCollectionReaderDelegate = new JsonFormatReaderGenerator().GenerateGetOnlyCollectionReader(this.TraditionalCollectionDataContract);
							Thread.MemoryBarrier();
							this.helper.JsonFormatGetOnlyReaderDelegate = jsonFormatGetOnlyCollectionReaderDelegate;
						}
					}
				}
				return this.helper.JsonFormatGetOnlyReaderDelegate;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000FDF RID: 4063 RVA: 0x00041368 File Offset: 0x0003F568
		internal JsonFormatCollectionWriterDelegate JsonFormatWriterDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (this.helper.JsonFormatWriterDelegate == null)
				{
					lock (this)
					{
						if (this.helper.JsonFormatWriterDelegate == null)
						{
							JsonFormatCollectionWriterDelegate jsonFormatCollectionWriterDelegate = new JsonFormatWriterGenerator().GenerateCollectionWriter(this.TraditionalCollectionDataContract);
							Thread.MemoryBarrier();
							this.helper.JsonFormatWriterDelegate = jsonFormatCollectionWriterDelegate;
						}
					}
				}
				return this.helper.JsonFormatWriterDelegate;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x000413E4 File Offset: 0x0003F5E4
		private CollectionDataContract TraditionalCollectionDataContract
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.TraditionalCollectionDataContract;
			}
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x000413F4 File Offset: 0x0003F5F4
		public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
		{
			jsonReader.Read();
			object obj = null;
			if (context.IsGetOnlyCollection)
			{
				context.IsGetOnlyCollection = false;
				this.JsonFormatGetOnlyReaderDelegate(jsonReader, context, XmlDictionaryString.Empty, JsonGlobals.itemDictionaryString, this.TraditionalCollectionDataContract);
			}
			else
			{
				obj = this.JsonFormatReaderDelegate(jsonReader, context, XmlDictionaryString.Empty, JsonGlobals.itemDictionaryString, this.TraditionalCollectionDataContract);
			}
			jsonReader.ReadEndElement();
			return obj;
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0004145D File Offset: 0x0003F65D
		public override void WriteJsonValueCore(XmlWriterDelegator jsonWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, RuntimeTypeHandle declaredTypeHandle)
		{
			context.IsGetOnlyCollection = false;
			this.JsonFormatWriterDelegate(jsonWriter, obj, context, this.TraditionalCollectionDataContract);
		}

		// Token: 0x040007DC RID: 2012
		[SecurityCritical]
		private JsonCollectionDataContract.JsonCollectionDataContractCriticalHelper helper;

		// Token: 0x02000188 RID: 392
		[SecurityCritical(SecurityCriticalScope.Everything)]
		private class JsonCollectionDataContractCriticalHelper : JsonDataContract.JsonDataContractCriticalHelper
		{
			// Token: 0x06001516 RID: 5398 RVA: 0x00054CC5 File Offset: 0x00052EC5
			public JsonCollectionDataContractCriticalHelper(CollectionDataContract traditionalDataContract)
				: base(traditionalDataContract)
			{
				this.traditionalCollectionDataContract = traditionalDataContract;
			}

			// Token: 0x17000466 RID: 1126
			// (get) Token: 0x06001517 RID: 5399 RVA: 0x00054CD5 File Offset: 0x00052ED5
			// (set) Token: 0x06001518 RID: 5400 RVA: 0x00054CDD File Offset: 0x00052EDD
			internal JsonFormatCollectionReaderDelegate JsonFormatReaderDelegate
			{
				get
				{
					return this.jsonFormatReaderDelegate;
				}
				set
				{
					this.jsonFormatReaderDelegate = value;
				}
			}

			// Token: 0x17000467 RID: 1127
			// (get) Token: 0x06001519 RID: 5401 RVA: 0x00054CE6 File Offset: 0x00052EE6
			// (set) Token: 0x0600151A RID: 5402 RVA: 0x00054CEE File Offset: 0x00052EEE
			internal JsonFormatGetOnlyCollectionReaderDelegate JsonFormatGetOnlyReaderDelegate
			{
				get
				{
					return this.jsonFormatGetOnlyReaderDelegate;
				}
				set
				{
					this.jsonFormatGetOnlyReaderDelegate = value;
				}
			}

			// Token: 0x17000468 RID: 1128
			// (get) Token: 0x0600151B RID: 5403 RVA: 0x00054CF7 File Offset: 0x00052EF7
			// (set) Token: 0x0600151C RID: 5404 RVA: 0x00054CFF File Offset: 0x00052EFF
			internal JsonFormatCollectionWriterDelegate JsonFormatWriterDelegate
			{
				get
				{
					return this.jsonFormatWriterDelegate;
				}
				set
				{
					this.jsonFormatWriterDelegate = value;
				}
			}

			// Token: 0x17000469 RID: 1129
			// (get) Token: 0x0600151D RID: 5405 RVA: 0x00054D08 File Offset: 0x00052F08
			internal CollectionDataContract TraditionalCollectionDataContract
			{
				get
				{
					return this.traditionalCollectionDataContract;
				}
			}

			// Token: 0x04000A46 RID: 2630
			private JsonFormatCollectionReaderDelegate jsonFormatReaderDelegate;

			// Token: 0x04000A47 RID: 2631
			private JsonFormatGetOnlyCollectionReaderDelegate jsonFormatGetOnlyReaderDelegate;

			// Token: 0x04000A48 RID: 2632
			private JsonFormatCollectionWriterDelegate jsonFormatWriterDelegate;

			// Token: 0x04000A49 RID: 2633
			private CollectionDataContract traditionalCollectionDataContract;
		}
	}
}
