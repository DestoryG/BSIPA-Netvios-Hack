using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000247 RID: 583
	internal struct CV_PRIMITIVE_TYPE
	{
		// Token: 0x04000A82 RID: 2690
		private const uint CV_MMASK = 1792U;

		// Token: 0x04000A83 RID: 2691
		private const uint CV_TMASK = 240U;

		// Token: 0x04000A84 RID: 2692
		private const uint CV_SMASK = 15U;

		// Token: 0x04000A85 RID: 2693
		private const int CV_MSHIFT = 8;

		// Token: 0x04000A86 RID: 2694
		private const int CV_TSHIFT = 4;

		// Token: 0x04000A87 RID: 2695
		private const int CV_SSHIFT = 0;

		// Token: 0x04000A88 RID: 2696
		private const uint CV_FIRST_NONPRIM = 4096U;
	}
}
