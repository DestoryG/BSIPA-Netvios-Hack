using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000BF RID: 191
	internal class NameDataContract : StringDataContract
	{
		// Token: 0x06000B68 RID: 2920 RVA: 0x0002ED66 File Offset: 0x0002CF66
		internal NameDataContract()
			: base(DictionaryGlobals.NameLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}
	}
}
