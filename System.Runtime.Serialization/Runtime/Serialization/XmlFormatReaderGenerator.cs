using System;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000E1 RID: 225
	internal sealed class XmlFormatReaderGenerator
	{
		// Token: 0x06000CA8 RID: 3240 RVA: 0x00035B0D File Offset: 0x00033D0D
		[SecurityCritical]
		public XmlFormatReaderGenerator()
		{
			this.helper = new XmlFormatReaderGenerator.CriticalHelper();
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00035B20 File Offset: 0x00033D20
		[SecurityCritical]
		public XmlFormatClassReaderDelegate GenerateClassReader(ClassDataContract classContract)
		{
			XmlFormatClassReaderDelegate xmlFormatClassReaderDelegate;
			try
			{
				if (TD.DCGenReaderStartIsEnabled())
				{
					TD.DCGenReaderStart("Class", classContract.UnderlyingType.FullName);
				}
				xmlFormatClassReaderDelegate = this.helper.GenerateClassReader(classContract);
			}
			finally
			{
				if (TD.DCGenReaderStopIsEnabled())
				{
					TD.DCGenReaderStop();
				}
			}
			return xmlFormatClassReaderDelegate;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00035B78 File Offset: 0x00033D78
		[SecurityCritical]
		public XmlFormatCollectionReaderDelegate GenerateCollectionReader(CollectionDataContract collectionContract)
		{
			XmlFormatCollectionReaderDelegate xmlFormatCollectionReaderDelegate;
			try
			{
				if (TD.DCGenReaderStartIsEnabled())
				{
					TD.DCGenReaderStart("Collection", collectionContract.UnderlyingType.FullName);
				}
				xmlFormatCollectionReaderDelegate = this.helper.GenerateCollectionReader(collectionContract);
			}
			finally
			{
				if (TD.DCGenReaderStopIsEnabled())
				{
					TD.DCGenReaderStop();
				}
			}
			return xmlFormatCollectionReaderDelegate;
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00035BD0 File Offset: 0x00033DD0
		[SecurityCritical]
		public XmlFormatGetOnlyCollectionReaderDelegate GenerateGetOnlyCollectionReader(CollectionDataContract collectionContract)
		{
			XmlFormatGetOnlyCollectionReaderDelegate xmlFormatGetOnlyCollectionReaderDelegate;
			try
			{
				if (TD.DCGenReaderStartIsEnabled())
				{
					TD.DCGenReaderStart("GetOnlyCollection", collectionContract.UnderlyingType.FullName);
				}
				xmlFormatGetOnlyCollectionReaderDelegate = this.helper.GenerateGetOnlyCollectionReader(collectionContract);
			}
			finally
			{
				if (TD.DCGenReaderStopIsEnabled())
				{
					TD.DCGenReaderStop();
				}
			}
			return xmlFormatGetOnlyCollectionReaderDelegate;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00035C28 File Offset: 0x00033E28
		[SecuritySafeCritical]
		internal static object UnsafeGetUninitializedObject(int id)
		{
			return FormatterServices.GetUninitializedObject(DataContract.GetDataContractForInitialization(id).TypeForInitialization);
		}

		// Token: 0x0400054C RID: 1356
		[SecurityCritical]
		private XmlFormatReaderGenerator.CriticalHelper helper;

		// Token: 0x0200017C RID: 380
		private class CriticalHelper
		{
			// Token: 0x060014E6 RID: 5350 RVA: 0x000548EF File Offset: 0x00052AEF
			internal XmlFormatClassReaderDelegate GenerateClassReader(ClassDataContract classContract)
			{
				return (XmlReaderDelegator xr, XmlObjectSerializerReadContext ctx, XmlDictionaryString[] memberNames, XmlDictionaryString[] memberNamespaces) => new XmlFormatReaderInterpreter(classContract).ReadFromXml(xr, ctx, memberNames, memberNamespaces);
			}

			// Token: 0x060014E7 RID: 5351 RVA: 0x00054908 File Offset: 0x00052B08
			internal XmlFormatCollectionReaderDelegate GenerateCollectionReader(CollectionDataContract collectionContract)
			{
				return (XmlReaderDelegator xr, XmlObjectSerializerReadContext ctx, XmlDictionaryString inm, XmlDictionaryString ins, CollectionDataContract cc) => new XmlFormatReaderInterpreter(collectionContract, false).ReadCollectionFromXml(xr, ctx, inm, ins, cc);
			}

			// Token: 0x060014E8 RID: 5352 RVA: 0x00054921 File Offset: 0x00052B21
			internal XmlFormatGetOnlyCollectionReaderDelegate GenerateGetOnlyCollectionReader(CollectionDataContract collectionContract)
			{
				return delegate(XmlReaderDelegator xr, XmlObjectSerializerReadContext ctx, XmlDictionaryString inm, XmlDictionaryString ins, CollectionDataContract cc)
				{
					new XmlFormatReaderInterpreter(collectionContract, true).ReadGetOnlyCollectionFromXml(xr, ctx, inm, ins, cc);
				};
			}
		}
	}
}
