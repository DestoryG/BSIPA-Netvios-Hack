using System;

namespace NSpeex
{
	// Token: 0x02000020 RID: 32
	internal abstract class LspQuant
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00012197 File Offset: 0x00010397
		protected internal LspQuant()
		{
		}

		// Token: 0x060000DB RID: 219
		public abstract void Quant(float[] lsp, float[] qlsp, int order, Bits bits);

		// Token: 0x060000DC RID: 220
		public abstract void Unquant(float[] lsp, int order, Bits bits);

		// Token: 0x060000DD RID: 221 RVA: 0x000121A0 File Offset: 0x000103A0
		protected internal void UnpackPlus(float[] lsp, int[] tab, Bits bits, float k, int ti, int li)
		{
			int num = bits.Unpack(6);
			for (int i = 0; i < ti; i++)
			{
				lsp[i + li] += k * (float)tab[num * ti + i];
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000121E4 File Offset: 0x000103E4
		protected internal static int Lsp_quant(float[] x, int xs, int[] cdbk, int nbVec, int nbDim)
		{
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < nbVec; i++)
			{
				float num4 = 0f;
				for (int j = 0; j < nbDim; j++)
				{
					float num5 = x[xs + j] - (float)cdbk[num3++];
					num4 += num5 * num5;
				}
				if (num4 < num || i == 0)
				{
					num = num4;
					num2 = i;
				}
			}
			for (int j = 0; j < nbDim; j++)
			{
				x[xs + j] -= (float)cdbk[num2 * nbDim + j];
			}
			return num2;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00012270 File Offset: 0x00010470
		protected internal static int Lsp_weight_quant(float[] x, int xs, float[] weight, int ws, int[] cdbk, int nbVec, int nbDim)
		{
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < nbVec; i++)
			{
				float num4 = 0f;
				for (int j = 0; j < nbDim; j++)
				{
					float num5 = x[xs + j] - (float)cdbk[num3++];
					num4 += weight[ws + j] * num5 * num5;
				}
				if (num4 < num || i == 0)
				{
					num = num4;
					num2 = i;
				}
			}
			for (int j = 0; j < nbDim; j++)
			{
				x[xs + j] -= (float)cdbk[num2 * nbDim + j];
			}
			return num2;
		}

		// Token: 0x040000FF RID: 255
		protected const int MAX_LSP_SIZE = 20;
	}
}
