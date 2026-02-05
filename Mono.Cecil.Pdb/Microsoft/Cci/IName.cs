using System;

namespace Microsoft.Cci
{
	// Token: 0x02000014 RID: 20
	public interface IName
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600014D RID: 333
		int UniqueKey { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600014E RID: 334
		int UniqueKeyIgnoringCase { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600014F RID: 335
		string Value { get; }
	}
}
