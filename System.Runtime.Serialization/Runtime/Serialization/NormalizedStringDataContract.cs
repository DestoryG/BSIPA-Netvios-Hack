using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000BC RID: 188
	internal class NormalizedStringDataContract : StringDataContract
	{
		// Token: 0x06000B65 RID: 2917 RVA: 0x0002ED30 File Offset: 0x0002CF30
		internal NormalizedStringDataContract()
			: base(DictionaryGlobals.normalizedStringLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}
	}
}
