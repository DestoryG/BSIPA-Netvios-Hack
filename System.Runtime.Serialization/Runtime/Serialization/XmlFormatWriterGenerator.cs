using System;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;

namespace System.Runtime.Serialization
{
	// Token: 0x020000E4 RID: 228
	internal sealed class XmlFormatWriterGenerator
	{
		// Token: 0x06000CB5 RID: 3253 RVA: 0x00035C3A File Offset: 0x00033E3A
		[SecurityCritical]
		public XmlFormatWriterGenerator()
		{
			this.helper = new XmlFormatWriterGenerator.CriticalHelper();
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x00035C50 File Offset: 0x00033E50
		[SecurityCritical]
		internal XmlFormatClassWriterDelegate GenerateClassWriter(ClassDataContract classContract)
		{
			XmlFormatClassWriterDelegate xmlFormatClassWriterDelegate;
			try
			{
				if (TD.DCGenWriterStartIsEnabled())
				{
					TD.DCGenWriterStart("Class", classContract.UnderlyingType.FullName);
				}
				xmlFormatClassWriterDelegate = this.helper.GenerateClassWriter(classContract);
			}
			finally
			{
				if (TD.DCGenWriterStopIsEnabled())
				{
					TD.DCGenWriterStop();
				}
			}
			return xmlFormatClassWriterDelegate;
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00035CA8 File Offset: 0x00033EA8
		[SecurityCritical]
		internal XmlFormatCollectionWriterDelegate GenerateCollectionWriter(CollectionDataContract collectionContract)
		{
			XmlFormatCollectionWriterDelegate xmlFormatCollectionWriterDelegate;
			try
			{
				if (TD.DCGenWriterStartIsEnabled())
				{
					TD.DCGenWriterStart("Collection", collectionContract.UnderlyingType.FullName);
				}
				xmlFormatCollectionWriterDelegate = this.helper.GenerateCollectionWriter(collectionContract);
			}
			finally
			{
				if (TD.DCGenWriterStopIsEnabled())
				{
					TD.DCGenWriterStop();
				}
			}
			return xmlFormatCollectionWriterDelegate;
		}

		// Token: 0x0400054D RID: 1357
		[SecurityCritical]
		private XmlFormatWriterGenerator.CriticalHelper helper;

		// Token: 0x0200017D RID: 381
		private class CriticalHelper
		{
			// Token: 0x060014EA RID: 5354 RVA: 0x00054942 File Offset: 0x00052B42
			internal XmlFormatClassWriterDelegate GenerateClassWriter(ClassDataContract classContract)
			{
				return delegate(XmlWriterDelegator xw, object obj, XmlObjectSerializerWriteContext ctx, ClassDataContract ctr)
				{
					new XmlFormatWriterInterpreter(classContract).WriteToXml(xw, obj, ctx, ctr);
				};
			}

			// Token: 0x060014EB RID: 5355 RVA: 0x0005495B File Offset: 0x00052B5B
			internal XmlFormatCollectionWriterDelegate GenerateCollectionWriter(CollectionDataContract collectionContract)
			{
				return delegate(XmlWriterDelegator xw, object obj, XmlObjectSerializerWriteContext ctx, CollectionDataContract ctr)
				{
					new XmlFormatWriterInterpreter(collectionContract).WriteCollectionToXml(xw, obj, ctx, ctr);
				};
			}
		}
	}
}
