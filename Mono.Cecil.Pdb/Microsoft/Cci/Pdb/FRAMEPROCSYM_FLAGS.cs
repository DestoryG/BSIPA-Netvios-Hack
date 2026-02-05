using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000B4 RID: 180
	[Flags]
	internal enum FRAMEPROCSYM_FLAGS : uint
	{
		// Token: 0x040003F4 RID: 1012
		fHasAlloca = 1U,
		// Token: 0x040003F5 RID: 1013
		fHasSetJmp = 2U,
		// Token: 0x040003F6 RID: 1014
		fHasLongJmp = 4U,
		// Token: 0x040003F7 RID: 1015
		fHasInlAsm = 8U,
		// Token: 0x040003F8 RID: 1016
		fHasEH = 16U,
		// Token: 0x040003F9 RID: 1017
		fInlSpec = 32U,
		// Token: 0x040003FA RID: 1018
		fHasSEH = 64U,
		// Token: 0x040003FB RID: 1019
		fNaked = 128U,
		// Token: 0x040003FC RID: 1020
		fSecurityChecks = 256U,
		// Token: 0x040003FD RID: 1021
		fAsyncEH = 512U,
		// Token: 0x040003FE RID: 1022
		fGSNoStackOrdering = 1024U,
		// Token: 0x040003FF RID: 1023
		fWasInlined = 2048U
	}
}
