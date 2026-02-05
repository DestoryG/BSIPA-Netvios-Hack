using System;

namespace NSpeex
{
	// Token: 0x0200002A RID: 42
	internal class Stereo
	{
		// Token: 0x0600011F RID: 287 RVA: 0x00014B84 File Offset: 0x00012D84
		public Stereo()
		{
			this.smooth_right = 1f;
			this.smooth_left = 1f;
			this.e_ratio = 0.5f;
			this.balance = 1f;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00014BB8 File Offset: 0x00012DB8
		public static void Encode(Bits bits, float[] data, int frameSize)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < frameSize; i++)
			{
				num += data[2 * i] * data[2 * i];
				num2 += data[2 * i + 1] * data[2 * i + 1];
				data[i] = 0.5f * (data[2 * i] + data[2 * i + 1]);
				num3 += data[i] * data[i];
			}
			float num4 = (num + 1f) / (num2 + 1f);
			float num5 = num3 / (1f + num + num2);
			bits.Pack(14, 5);
			bits.Pack(9, 4);
			num4 = (float)(4.0 * Math.Log((double)num4));
			if (num4 > 0f)
			{
				bits.Pack(0, 1);
			}
			else
			{
				bits.Pack(1, 1);
			}
			num4 = (float)Math.Floor((double)(0.5f + Math.Abs(num4)));
			if (num4 > 30f)
			{
				num4 = 31f;
			}
			bits.Pack((int)num4, 5);
			int num6 = VQ.Index(num5, Stereo.e_ratio_quant, 4);
			bits.Pack(num6, 2);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00014CD0 File Offset: 0x00012ED0
		public void Decode(float[] data, int frameSize)
		{
			float num = 0f;
			for (int i = frameSize - 1; i >= 0; i--)
			{
				num += data[i] * data[i];
			}
			float num2 = num / this.e_ratio;
			float num3 = num2 * this.balance / (1f + this.balance);
			float num4 = num2 - num3;
			num3 = (float)Math.Sqrt((double)(num3 / (num + 0.01f)));
			num4 = (float)Math.Sqrt((double)(num4 / (num + 0.01f)));
			for (int i = frameSize - 1; i >= 0; i--)
			{
				float num5 = data[i];
				this.smooth_left = 0.98f * this.smooth_left + 0.02f * num3;
				this.smooth_right = 0.98f * this.smooth_right + 0.02f * num4;
				data[2 * i] = this.smooth_left * num5;
				data[2 * i + 1] = this.smooth_right * num5;
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00014DA8 File Offset: 0x00012FA8
		public void Init(Bits bits)
		{
			float num = 1f;
			if (bits.Unpack(1) != 0)
			{
				num = -1f;
			}
			int num2 = bits.Unpack(5);
			this.balance = (float)Math.Exp((double)num * 0.25 * (double)num2);
			num2 = bits.Unpack(2);
			this.e_ratio = Stereo.e_ratio_quant[num2];
		}

		// Token: 0x0400013A RID: 314
		private const int SPEEX_INBAND_STEREO = 9;

		// Token: 0x0400013B RID: 315
		private static readonly float[] e_ratio_quant = new float[] { 0.25f, 0.315f, 0.397f, 0.5f };

		// Token: 0x0400013C RID: 316
		private float balance;

		// Token: 0x0400013D RID: 317
		private float e_ratio;

		// Token: 0x0400013E RID: 318
		private float smooth_left;

		// Token: 0x0400013F RID: 319
		private float smooth_right;
	}
}
