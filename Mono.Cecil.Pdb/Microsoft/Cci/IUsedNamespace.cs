using System;

namespace Microsoft.Cci
{
	// Token: 0x02000013 RID: 19
	public interface IUsedNamespace
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600014B RID: 331
		IName Alias { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600014C RID: 332
		IName NamespaceName { get; }
	}
}
