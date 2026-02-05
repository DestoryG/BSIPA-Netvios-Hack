using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000C1 RID: 193
	internal class IDDataContract : StringDataContract
	{
		// Token: 0x06000B6A RID: 2922 RVA: 0x0002ED8A File Offset: 0x0002CF8A
		internal IDDataContract()
			: base(DictionaryGlobals.XSDIDLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}
	}
}
