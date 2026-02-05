using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000034 RID: 52
	[Flags]
	internal enum LeafPointerAttr : uint
	{
		// Token: 0x040001AE RID: 430
		ptrtype = 31U,
		// Token: 0x040001AF RID: 431
		ptrmode = 224U,
		// Token: 0x040001B0 RID: 432
		isflat32 = 256U,
		// Token: 0x040001B1 RID: 433
		isvolatile = 512U,
		// Token: 0x040001B2 RID: 434
		isconst = 1024U,
		// Token: 0x040001B3 RID: 435
		isunaligned = 2048U,
		// Token: 0x040001B4 RID: 436
		isrestrict = 4096U
	}
}
