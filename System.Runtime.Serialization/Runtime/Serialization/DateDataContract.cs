using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000B5 RID: 181
	internal class DateDataContract : StringDataContract
	{
		// Token: 0x06000B5E RID: 2910 RVA: 0x0002ECB2 File Offset: 0x0002CEB2
		internal DateDataContract()
			: base(DictionaryGlobals.dateLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}
	}
}
