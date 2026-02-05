using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000BE RID: 190
	internal class LanguageDataContract : StringDataContract
	{
		// Token: 0x06000B67 RID: 2919 RVA: 0x0002ED54 File Offset: 0x0002CF54
		internal LanguageDataContract()
			: base(DictionaryGlobals.languageLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}
	}
}
