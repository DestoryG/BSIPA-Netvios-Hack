using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000CB RID: 203
	internal class XsDurationDataContract : TimeSpanDataContract
	{
		// Token: 0x06000B83 RID: 2947 RVA: 0x0002EF83 File Offset: 0x0002D183
		internal XsDurationDataContract()
			: base(DictionaryGlobals.TimeSpanLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}
	}
}
