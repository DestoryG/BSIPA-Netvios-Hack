using System;

namespace NSpeex
{
	// Token: 0x0200000C RID: 12
	internal class Misc
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00003F50 File Offset: 0x00002150
		public static float[] Window(int windowSize, int subFrameSize)
		{
			int num = subFrameSize * 7 / 2;
			int num2 = subFrameSize * 5 / 2;
			float[] array = new float[windowSize];
			for (int i = 0; i < num; i++)
			{
				array[i] = (float)(0.54 - 0.46 * Math.Cos(3.141592653589793 * (double)i / (double)num));
			}
			for (int i = 0; i < num2; i++)
			{
				array[num + i] = (float)(0.54 + 0.46 * Math.Cos(3.141592653589793 * (double)i / (double)num2));
			}
			return array;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003FE4 File Offset: 0x000021E4
		public static float[] LagWindow(int lpcSize, float lagFactor)
		{
			float[] array = new float[lpcSize + 1];
			for (int i = 0; i < lpcSize + 1; i++)
			{
				array[i] = (float)Math.Exp(-0.5 * (6.283185307179586 * (double)lagFactor * (double)i) * (6.283185307179586 * (double)lagFactor * (double)i));
			}
			return array;
		}
	}
}
