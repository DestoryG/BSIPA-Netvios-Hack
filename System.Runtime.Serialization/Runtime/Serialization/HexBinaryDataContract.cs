using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000B6 RID: 182
	internal class HexBinaryDataContract : StringDataContract
	{
		// Token: 0x06000B5F RID: 2911 RVA: 0x0002ECC4 File Offset: 0x0002CEC4
		internal HexBinaryDataContract()
			: base(DictionaryGlobals.hexBinaryLocalName, DictionaryGlobals.SchemaNamespace)
		{
		}
	}
}
