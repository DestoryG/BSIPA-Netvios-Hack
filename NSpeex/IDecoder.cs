using System;

namespace NSpeex
{
	// Token: 0x02000012 RID: 18
	internal interface IDecoder
	{
		// Token: 0x0600006B RID: 107
		int Decode(Bits bits, float[] xout);

		// Token: 0x0600006C RID: 108
		void DecodeStereo(float[] data, int frameSize);

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600006D RID: 109
		// (set) Token: 0x0600006E RID: 110
		bool PerceptualEnhancement { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600006F RID: 111
		int FrameSize { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000070 RID: 112
		bool Dtx { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000071 RID: 113
		float[] PiGain { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000072 RID: 114
		float[] Exc { get; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000073 RID: 115
		float[] Innov { get; }
	}
}
