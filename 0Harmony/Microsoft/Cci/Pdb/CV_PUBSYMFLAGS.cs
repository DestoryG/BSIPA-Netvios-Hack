using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002C5 RID: 709
	[Flags]
	internal enum CV_PUBSYMFLAGS : uint
	{
		// Token: 0x04000D80 RID: 3456
		fNone = 0U,
		// Token: 0x04000D81 RID: 3457
		fCode = 1U,
		// Token: 0x04000D82 RID: 3458
		fFunction = 2U,
		// Token: 0x04000D83 RID: 3459
		fManaged = 4U,
		// Token: 0x04000D84 RID: 3460
		fMSIL = 8U
	}
}
