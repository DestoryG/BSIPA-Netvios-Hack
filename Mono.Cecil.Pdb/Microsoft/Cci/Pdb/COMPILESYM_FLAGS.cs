using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000092 RID: 146
	[Flags]
	internal enum COMPILESYM_FLAGS : uint
	{
		// Token: 0x04000330 RID: 816
		iLanguage = 255U,
		// Token: 0x04000331 RID: 817
		fEC = 256U,
		// Token: 0x04000332 RID: 818
		fNoDbgInfo = 512U,
		// Token: 0x04000333 RID: 819
		fLTCG = 1024U,
		// Token: 0x04000334 RID: 820
		fNoDataAlign = 2048U,
		// Token: 0x04000335 RID: 821
		fManagedPresent = 4096U,
		// Token: 0x04000336 RID: 822
		fSecurityChecks = 8192U,
		// Token: 0x04000337 RID: 823
		fHotPatch = 16384U,
		// Token: 0x04000338 RID: 824
		fCVTCIL = 32768U,
		// Token: 0x04000339 RID: 825
		fMSILModule = 65536U
	}
}
