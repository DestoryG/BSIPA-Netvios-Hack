using System;

namespace Ionic.BZip2
{
	// Token: 0x02000048 RID: 72
	internal static class BZip2
	{
		// Token: 0x0600038D RID: 909 RVA: 0x00015458 File Offset: 0x00013658
		internal static T[][] InitRectangularArray<T>(int d1, int d2)
		{
			T[][] array = new T[d1][];
			for (int i = 0; i < d1; i++)
			{
				array[i] = new T[d2];
			}
			return array;
		}

		// Token: 0x04000216 RID: 534
		public static readonly int BlockSizeMultiple = 100000;

		// Token: 0x04000217 RID: 535
		public static readonly int MinBlockSize = 1;

		// Token: 0x04000218 RID: 536
		public static readonly int MaxBlockSize = 9;

		// Token: 0x04000219 RID: 537
		public static readonly int MaxAlphaSize = 258;

		// Token: 0x0400021A RID: 538
		public static readonly int MaxCodeLength = 23;

		// Token: 0x0400021B RID: 539
		public static readonly char RUNA = '\0';

		// Token: 0x0400021C RID: 540
		public static readonly char RUNB = '\u0001';

		// Token: 0x0400021D RID: 541
		public static readonly int NGroups = 6;

		// Token: 0x0400021E RID: 542
		public static readonly int G_SIZE = 50;

		// Token: 0x0400021F RID: 543
		public static readonly int N_ITERS = 4;

		// Token: 0x04000220 RID: 544
		public static readonly int MaxSelectors = 2 + 900000 / BZip2.G_SIZE;

		// Token: 0x04000221 RID: 545
		public static readonly int NUM_OVERSHOOT_BYTES = 20;

		// Token: 0x04000222 RID: 546
		internal static readonly int QSORT_STACK_SIZE = 1000;
	}
}
