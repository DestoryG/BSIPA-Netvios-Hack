using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200009E RID: 158
	[Flags]
	internal enum CV_PUBSYMFLAGS : uint
	{
		// Token: 0x04000364 RID: 868
		fNone = 0U,
		// Token: 0x04000365 RID: 869
		fCode = 1U,
		// Token: 0x04000366 RID: 870
		fFunction = 2U,
		// Token: 0x04000367 RID: 871
		fManaged = 4U,
		// Token: 0x04000368 RID: 872
		fMSIL = 8U
	}
}
