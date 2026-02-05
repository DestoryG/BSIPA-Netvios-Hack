using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002FF RID: 767
	internal interface IUsedNamespace
	{
		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060011E6 RID: 4582
		IName Alias { get; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060011E7 RID: 4583
		IName NamespaceName { get; }
	}
}
