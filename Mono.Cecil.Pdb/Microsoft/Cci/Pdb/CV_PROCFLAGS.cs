using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200007E RID: 126
	[Flags]
	internal enum CV_PROCFLAGS : byte
	{
		// Token: 0x040002D8 RID: 728
		CV_PFLAG_NOFPO = 1,
		// Token: 0x040002D9 RID: 729
		CV_PFLAG_INT = 2,
		// Token: 0x040002DA RID: 730
		CV_PFLAG_FAR = 4,
		// Token: 0x040002DB RID: 731
		CV_PFLAG_NEVER = 8,
		// Token: 0x040002DC RID: 732
		CV_PFLAG_NOTREACHED = 16,
		// Token: 0x040002DD RID: 733
		CV_PFLAG_CUST_CALL = 32,
		// Token: 0x040002DE RID: 734
		CV_PFLAG_NOINLINE = 64,
		// Token: 0x040002DF RID: 735
		CV_PFLAG_OPTDBGINFO = 128
	}
}
