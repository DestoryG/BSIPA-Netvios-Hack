using System;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x0200010C RID: 268
	internal class JsonFormatWriterGenerator
	{
		// Token: 0x06001030 RID: 4144 RVA: 0x0004203C File Offset: 0x0004023C
		[SecurityCritical]
		public JsonFormatWriterGenerator()
		{
			this.helper = new JsonFormatWriterGenerator.CriticalHelper();
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x00042050 File Offset: 0x00040250
		[SecurityCritical]
		internal JsonFormatClassWriterDelegate GenerateClassWriter(ClassDataContract classContract)
		{
			JsonFormatClassWriterDelegate jsonFormatClassWriterDelegate;
			try
			{
				if (TD.DCJsonGenWriterStartIsEnabled())
				{
					TD.DCJsonGenWriterStart("Class", classContract.UnderlyingType.FullName);
				}
				jsonFormatClassWriterDelegate = this.helper.GenerateClassWriter(classContract);
			}
			finally
			{
				if (TD.DCJsonGenWriterStopIsEnabled())
				{
					TD.DCJsonGenWriterStop();
				}
			}
			return jsonFormatClassWriterDelegate;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x000420A8 File Offset: 0x000402A8
		[SecurityCritical]
		internal JsonFormatCollectionWriterDelegate GenerateCollectionWriter(CollectionDataContract collectionContract)
		{
			JsonFormatCollectionWriterDelegate jsonFormatCollectionWriterDelegate;
			try
			{
				if (TD.DCJsonGenWriterStartIsEnabled())
				{
					TD.DCJsonGenWriterStart("Collection", collectionContract.UnderlyingType.FullName);
				}
				jsonFormatCollectionWriterDelegate = this.helper.GenerateCollectionWriter(collectionContract);
			}
			finally
			{
				if (TD.DCJsonGenWriterStopIsEnabled())
				{
					TD.DCJsonGenWriterStop();
				}
			}
			return jsonFormatCollectionWriterDelegate;
		}

		// Token: 0x040007F2 RID: 2034
		[SecurityCritical]
		private JsonFormatWriterGenerator.CriticalHelper helper;

		// Token: 0x0200018D RID: 397
		private class CriticalHelper
		{
			// Token: 0x0600152D RID: 5421 RVA: 0x000552D4 File Offset: 0x000534D4
			internal JsonFormatClassWriterDelegate GenerateClassWriter(ClassDataContract classContract)
			{
				return delegate(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, ClassDataContract dataContract, XmlDictionaryString[] memberNames)
				{
					new JsonFormatWriterInterpreter(classContract).WriteToJson(xmlWriter, obj, context, dataContract, memberNames);
				};
			}

			// Token: 0x0600152E RID: 5422 RVA: 0x000552ED File Offset: 0x000534ED
			internal JsonFormatCollectionWriterDelegate GenerateCollectionWriter(CollectionDataContract collectionContract)
			{
				return delegate(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, CollectionDataContract dataContract)
				{
					new JsonFormatWriterInterpreter(collectionContract).WriteCollectionToJson(xmlWriter, obj, context, dataContract);
				};
			}
		}
	}
}
