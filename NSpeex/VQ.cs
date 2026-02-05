using System;

namespace NSpeex
{
	// Token: 0x02000005 RID: 5
	internal class VQ
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002380 File Offset: 0x00000580
		public static int Index(float ins0, float[] codebook, int entries)
		{
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < entries; i++)
			{
				float num3 = ins0 - codebook[i];
				num3 *= num3;
				if (i == 0 || num3 < num)
				{
					num = num3;
					num2 = i;
				}
			}
			return num2;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023B8 File Offset: 0x000005B8
		public static int Index(float[] ins0, float[] codebook, int len, int entries)
		{
			int num = 0;
			float num2 = 0f;
			int num3 = 0;
			for (int i = 0; i < entries; i++)
			{
				float num4 = 0f;
				for (int j = 0; j < len; j++)
				{
					float num5 = ins0[j] - codebook[num++];
					num4 += num5 * num5;
				}
				if (i == 0 || num4 < num2)
				{
					num2 = num4;
					num3 = i;
				}
			}
			return num3;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002418 File Offset: 0x00000618
		public static void Nbest(float[] ins0, int offset, float[] codebook, int len, int entries, float[] E, int N, int[] nbest, float[] best_dist)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < entries; i++)
			{
				float num3 = 0.5f * E[i];
				for (int j = 0; j < len; j++)
				{
					num3 -= ins0[offset + j] * codebook[num++];
				}
				if (i < N || num3 < best_dist[N - 1])
				{
					int num4 = N - 1;
					while (num4 >= 1 && (num4 > num2 || num3 < best_dist[num4 - 1]))
					{
						best_dist[num4] = best_dist[num4 - 1];
						nbest[num4] = nbest[num4 - 1];
						num4--;
					}
					best_dist[num4] = num3;
					nbest[num4] = i;
					num2++;
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000024BC File Offset: 0x000006BC
		public static void Nbest_sign(float[] ins0, int offset, float[] codebook, int len, int entries, float[] E, int N, int[] nbest, float[] best_dist)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < entries; i++)
			{
				float num3 = 0f;
				for (int j = 0; j < len; j++)
				{
					num3 -= ins0[offset + j] * codebook[num++];
				}
				int num4;
				if (num3 > 0f)
				{
					num4 = 1;
					num3 = -num3;
				}
				else
				{
					num4 = 0;
				}
				num3 += 0.5f * E[i];
				if (i < N || num3 < best_dist[N - 1])
				{
					int num5 = N - 1;
					while (num5 >= 1 && (num5 > num2 || num3 < best_dist[num5 - 1]))
					{
						best_dist[num5] = best_dist[num5 - 1];
						nbest[num5] = nbest[num5 - 1];
						num5--;
					}
					best_dist[num5] = num3;
					nbest[num5] = i;
					num2++;
					if (num4 != 0)
					{
						nbest[num5] += entries;
					}
				}
			}
		}
	}
}
