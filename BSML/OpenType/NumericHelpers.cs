using System;

namespace BeatSaberMarkupLanguage.OpenType
{
	// Token: 0x02000077 RID: 119
	public static class NumericHelpers
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000C38A File Offset: 0x0000A58A
		public static uint Log2(uint x)
		{
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return (uint)(NumericHelpers.NumBitsSet(x) - 1);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000C3B8 File Offset: 0x0000A5B8
		public static uint NextPow2(uint x)
		{
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x + 1U;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000C3E4 File Offset: 0x0000A5E4
		public static int NumBitsSet(uint x)
		{
			x -= (x >> 1) & 1431655765U;
			x = ((x >> 2) & 858993459U) + (x & 858993459U);
			x = ((x >> 4) + x) & 252645135U;
			x += x >> 8;
			x += x >> 16;
			return (int)(x & 63U);
		}
	}
}
