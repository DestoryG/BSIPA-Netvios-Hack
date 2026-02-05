using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000021 RID: 33
	internal struct CV_PRIMITIVE_TYPE
	{
		// Token: 0x04000068 RID: 104
		private const uint CV_MMASK = 1792U;

		// Token: 0x04000069 RID: 105
		private const uint CV_TMASK = 240U;

		// Token: 0x0400006A RID: 106
		private const uint CV_SMASK = 15U;

		// Token: 0x0400006B RID: 107
		private const int CV_MSHIFT = 8;

		// Token: 0x0400006C RID: 108
		private const int CV_TSHIFT = 4;

		// Token: 0x0400006D RID: 109
		private const int CV_SSHIFT = 0;

		// Token: 0x0400006E RID: 110
		private const uint CV_FIRST_NONPRIM = 4096U;
	}
}
