using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002B9 RID: 697
	[Flags]
	internal enum COMPILESYM_FLAGS : uint
	{
		// Token: 0x04000D4C RID: 3404
		iLanguage = 255U,
		// Token: 0x04000D4D RID: 3405
		fEC = 256U,
		// Token: 0x04000D4E RID: 3406
		fNoDbgInfo = 512U,
		// Token: 0x04000D4F RID: 3407
		fLTCG = 1024U,
		// Token: 0x04000D50 RID: 3408
		fNoDataAlign = 2048U,
		// Token: 0x04000D51 RID: 3409
		fManagedPresent = 4096U,
		// Token: 0x04000D52 RID: 3410
		fSecurityChecks = 8192U,
		// Token: 0x04000D53 RID: 3411
		fHotPatch = 16384U,
		// Token: 0x04000D54 RID: 3412
		fCVTCIL = 32768U,
		// Token: 0x04000D55 RID: 3413
		fMSILModule = 65536U
	}
}
