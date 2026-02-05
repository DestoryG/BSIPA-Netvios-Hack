using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000080 RID: 128
	[Flags]
	internal enum CV_LVARFLAGS : ushort
	{
		// Token: 0x040002E3 RID: 739
		fIsParam = 1,
		// Token: 0x040002E4 RID: 740
		fAddrTaken = 2,
		// Token: 0x040002E5 RID: 741
		fCompGenx = 4,
		// Token: 0x040002E6 RID: 742
		fIsAggregate = 8,
		// Token: 0x040002E7 RID: 743
		fIsAggregated = 16,
		// Token: 0x040002E8 RID: 744
		fIsAliased = 32,
		// Token: 0x040002E9 RID: 745
		fIsAlias = 64
	}
}
