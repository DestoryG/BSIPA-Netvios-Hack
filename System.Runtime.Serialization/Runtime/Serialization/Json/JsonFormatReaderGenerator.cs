using System;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000109 RID: 265
	internal sealed class JsonFormatReaderGenerator
	{
		// Token: 0x06001024 RID: 4132 RVA: 0x00041F1F File Offset: 0x0004011F
		[SecurityCritical]
		public JsonFormatReaderGenerator()
		{
			this.helper = new JsonFormatReaderGenerator.CriticalHelper();
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x00041F34 File Offset: 0x00040134
		[SecurityCritical]
		public JsonFormatClassReaderDelegate GenerateClassReader(ClassDataContract classContract)
		{
			JsonFormatClassReaderDelegate jsonFormatClassReaderDelegate;
			try
			{
				if (TD.DCJsonGenReaderStartIsEnabled())
				{
					TD.DCJsonGenReaderStart("Class", classContract.UnderlyingType.FullName);
				}
				jsonFormatClassReaderDelegate = this.helper.GenerateClassReader(classContract);
			}
			finally
			{
				if (TD.DCJsonGenReaderStopIsEnabled())
				{
					TD.DCJsonGenReaderStop();
				}
			}
			return jsonFormatClassReaderDelegate;
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x00041F8C File Offset: 0x0004018C
		[SecurityCritical]
		public JsonFormatCollectionReaderDelegate GenerateCollectionReader(CollectionDataContract collectionContract)
		{
			JsonFormatCollectionReaderDelegate jsonFormatCollectionReaderDelegate;
			try
			{
				if (TD.DCJsonGenReaderStartIsEnabled())
				{
					TD.DCJsonGenReaderStart("Collection", collectionContract.StableName.Name);
				}
				jsonFormatCollectionReaderDelegate = this.helper.GenerateCollectionReader(collectionContract);
			}
			finally
			{
				if (TD.DCJsonGenReaderStopIsEnabled())
				{
					TD.DCJsonGenReaderStop();
				}
			}
			return jsonFormatCollectionReaderDelegate;
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x00041FE4 File Offset: 0x000401E4
		[SecurityCritical]
		public JsonFormatGetOnlyCollectionReaderDelegate GenerateGetOnlyCollectionReader(CollectionDataContract collectionContract)
		{
			JsonFormatGetOnlyCollectionReaderDelegate jsonFormatGetOnlyCollectionReaderDelegate;
			try
			{
				if (TD.DCJsonGenReaderStartIsEnabled())
				{
					TD.DCJsonGenReaderStart("GetOnlyCollection", collectionContract.UnderlyingType.FullName);
				}
				jsonFormatGetOnlyCollectionReaderDelegate = this.helper.GenerateGetOnlyCollectionReader(collectionContract);
			}
			finally
			{
				if (TD.DCJsonGenReaderStopIsEnabled())
				{
					TD.DCJsonGenReaderStop();
				}
			}
			return jsonFormatGetOnlyCollectionReaderDelegate;
		}

		// Token: 0x040007F1 RID: 2033
		[SecurityCritical]
		private JsonFormatReaderGenerator.CriticalHelper helper;

		// Token: 0x0200018C RID: 396
		private class CriticalHelper
		{
			// Token: 0x06001529 RID: 5417 RVA: 0x00055281 File Offset: 0x00053481
			internal JsonFormatClassReaderDelegate GenerateClassReader(ClassDataContract classContract)
			{
				return (XmlReaderDelegator xr, XmlObjectSerializerReadContextComplexJson ctx, XmlDictionaryString emptyDictionaryString, XmlDictionaryString[] memberNames) => new JsonFormatReaderInterpreter(classContract).ReadFromJson(xr, ctx, emptyDictionaryString, memberNames);
			}

			// Token: 0x0600152A RID: 5418 RVA: 0x0005529A File Offset: 0x0005349A
			internal JsonFormatCollectionReaderDelegate GenerateCollectionReader(CollectionDataContract collectionContract)
			{
				return (XmlReaderDelegator xr, XmlObjectSerializerReadContextComplexJson ctx, XmlDictionaryString emptyDS, XmlDictionaryString inm, CollectionDataContract cc) => new JsonFormatReaderInterpreter(collectionContract, false).ReadCollectionFromJson(xr, ctx, emptyDS, inm, cc);
			}

			// Token: 0x0600152B RID: 5419 RVA: 0x000552B3 File Offset: 0x000534B3
			internal JsonFormatGetOnlyCollectionReaderDelegate GenerateGetOnlyCollectionReader(CollectionDataContract collectionContract)
			{
				return delegate(XmlReaderDelegator xr, XmlObjectSerializerReadContextComplexJson ctx, XmlDictionaryString emptyDS, XmlDictionaryString inm, CollectionDataContract cc)
				{
					new JsonFormatReaderInterpreter(collectionContract, true).ReadGetOnlyCollectionFromJson(xr, ctx, emptyDS, inm, cc);
				};
			}
		}
	}
}
