using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000BA RID: 186
	internal class GDayDataContract : StringDataContract
	{
		// Token: 0x06000B63 RID: 2915 RVA: 0x0002ED0C File Offset: 0x0002CF0C
		internal GDayDataContract()
			: base(DictionaryGlobals.gDayLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}
	}
}
