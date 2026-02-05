using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002A7 RID: 679
	[Flags]
	internal enum CV_LVARFLAGS : ushort
	{
		// Token: 0x04000CFF RID: 3327
		fIsParam = 1,
		// Token: 0x04000D00 RID: 3328
		fAddrTaken = 2,
		// Token: 0x04000D01 RID: 3329
		fCompGenx = 4,
		// Token: 0x04000D02 RID: 3330
		fIsAggregate = 8,
		// Token: 0x04000D03 RID: 3331
		fIsAggregated = 16,
		// Token: 0x04000D04 RID: 3332
		fIsAliased = 32,
		// Token: 0x04000D05 RID: 3333
		fIsAlias = 64
	}
}
