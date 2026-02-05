using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000C0 RID: 192
	internal class NCNameDataContract : StringDataContract
	{
		// Token: 0x06000B69 RID: 2921 RVA: 0x0002ED78 File Offset: 0x0002CF78
		internal NCNameDataContract()
			: base(DictionaryGlobals.NCNameLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}
	}
}
