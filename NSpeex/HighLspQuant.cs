using System;

namespace NSpeex
{
	// Token: 0x02000026 RID: 38
	internal class HighLspQuant : LspQuant
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00012F54 File Offset: 0x00011154
		public sealed override void Quant(float[] lsp, float[] qlsp, int order, Bits bits)
		{
			float[] array = new float[20];
			for (int i = 0; i < order; i++)
			{
				qlsp[i] = lsp[i];
			}
			array[0] = 1f / (qlsp[1] - qlsp[0]);
			array[order - 1] = 1f / (qlsp[order - 1] - qlsp[order - 2]);
			for (int i = 1; i < order - 1; i++)
			{
				array[i] = Math.Max(1f / (qlsp[i] - qlsp[i - 1]), 1f / (qlsp[i + 1] - qlsp[i]));
			}
			for (int i = 0; i < order; i++)
			{
				qlsp[i] -= 0.3125f * (float)i + 0.75f;
			}
			for (int i = 0; i < order; i++)
			{
				qlsp[i] *= 256f;
			}
			int num = LspQuant.Lsp_quant(qlsp, 0, Codebook_Constants.high_lsp_cdbk, 64, order);
			bits.Pack(num, 6);
			for (int i = 0; i < order; i++)
			{
				qlsp[i] *= 2f;
			}
			num = LspQuant.Lsp_weight_quant(qlsp, 0, array, 0, Codebook_Constants.high_lsp_cdbk2, 64, order);
			bits.Pack(num, 6);
			for (int i = 0; i < order; i++)
			{
				qlsp[i] *= 0.0019531f;
			}
			for (int i = 0; i < order; i++)
			{
				qlsp[i] = lsp[i] - qlsp[i];
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000130B8 File Offset: 0x000112B8
		public sealed override void Unquant(float[] lsp, int order, Bits bits)
		{
			for (int i = 0; i < order; i++)
			{
				lsp[i] = 0.3125f * (float)i + 0.75f;
			}
			base.UnpackPlus(lsp, Codebook_Constants.high_lsp_cdbk, bits, 0.0039062f, order, 0);
			base.UnpackPlus(lsp, Codebook_Constants.high_lsp_cdbk2, bits, 0.0019531f, order, 0);
		}
	}
}
