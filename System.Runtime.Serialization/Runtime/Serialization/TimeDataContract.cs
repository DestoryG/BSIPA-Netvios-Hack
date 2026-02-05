using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000B4 RID: 180
	internal class TimeDataContract : StringDataContract
	{
		// Token: 0x06000B5D RID: 2909 RVA: 0x0002ECA0 File Offset: 0x0002CEA0
		internal TimeDataContract()
			: base(DictionaryGlobals.timeLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}
	}
}
