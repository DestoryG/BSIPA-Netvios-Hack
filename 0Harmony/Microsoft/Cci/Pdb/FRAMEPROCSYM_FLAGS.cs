using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002DB RID: 731
	[Flags]
	internal enum FRAMEPROCSYM_FLAGS : uint
	{
		// Token: 0x04000E10 RID: 3600
		fHasAlloca = 1U,
		// Token: 0x04000E11 RID: 3601
		fHasSetJmp = 2U,
		// Token: 0x04000E12 RID: 3602
		fHasLongJmp = 4U,
		// Token: 0x04000E13 RID: 3603
		fHasInlAsm = 8U,
		// Token: 0x04000E14 RID: 3604
		fHasEH = 16U,
		// Token: 0x04000E15 RID: 3605
		fInlSpec = 32U,
		// Token: 0x04000E16 RID: 3606
		fHasSEH = 64U,
		// Token: 0x04000E17 RID: 3607
		fNaked = 128U,
		// Token: 0x04000E18 RID: 3608
		fSecurityChecks = 256U,
		// Token: 0x04000E19 RID: 3609
		fAsyncEH = 512U,
		// Token: 0x04000E1A RID: 3610
		fGSNoStackOrdering = 1024U,
		// Token: 0x04000E1B RID: 3611
		fWasInlined = 2048U
	}
}
