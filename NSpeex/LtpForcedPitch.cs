using System;

namespace NSpeex
{
	// Token: 0x02000003 RID: 3
	internal class LtpForcedPitch : Ltp
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000022DC File Offset: 0x000004DC
		public sealed override int Quant(float[] target, float[] sw, int sws, float[] ak, float[] awk1, float[] awk2, float[] exc, int es, int start, int end, float pitch_coef, int p, int nsf, Bits bits, float[] exc2, int e2s, float[] r, int complexity)
		{
			if (pitch_coef > 0.99f)
			{
				pitch_coef = 0.99f;
			}
			for (int i = 0; i < nsf; i++)
			{
				exc[es + i] = exc[es + i - start] * pitch_coef;
			}
			return start;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000231C File Offset: 0x0000051C
		public sealed override int Unquant(float[] exc, int es, int start, float pitch_coef, int nsf, float[] gain_val, Bits bits, int count_lost, int subframe_offset, float last_pitch_gain)
		{
			if (pitch_coef > 0.99f)
			{
				pitch_coef = 0.99f;
			}
			for (int i = 0; i < nsf; i++)
			{
				exc[es + i] = exc[es + i - start] * pitch_coef;
			}
			gain_val[0] = (gain_val[2] = 0f);
			gain_val[1] = pitch_coef;
			return start;
		}
	}
}
