using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002A5 RID: 677
	[Flags]
	internal enum CV_PROCFLAGS : byte
	{
		// Token: 0x04000CF4 RID: 3316
		CV_PFLAG_NOFPO = 1,
		// Token: 0x04000CF5 RID: 3317
		CV_PFLAG_INT = 2,
		// Token: 0x04000CF6 RID: 3318
		CV_PFLAG_FAR = 4,
		// Token: 0x04000CF7 RID: 3319
		CV_PFLAG_NEVER = 8,
		// Token: 0x04000CF8 RID: 3320
		CV_PFLAG_NOTREACHED = 16,
		// Token: 0x04000CF9 RID: 3321
		CV_PFLAG_CUST_CALL = 32,
		// Token: 0x04000CFA RID: 3322
		CV_PFLAG_NOINLINE = 64,
		// Token: 0x04000CFB RID: 3323
		CV_PFLAG_OPTDBGINFO = 128
	}
}
