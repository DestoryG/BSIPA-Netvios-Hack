using System;

namespace NSpeex
{
	// Token: 0x0200000D RID: 13
	internal interface IEncoder
	{
		// Token: 0x0600003D RID: 61
		int Encode(Bits bits, float[] ins0);

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600003E RID: 62
		int EncodedFrameSize { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003F RID: 63
		int FrameSize { get; }

		// Token: 0x1700000A RID: 10
		// (set) Token: 0x06000040 RID: 64
		int Quality { set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000041 RID: 65
		// (set) Token: 0x06000042 RID: 66
		int BitRate { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000043 RID: 67
		float[] PiGain { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000044 RID: 68
		float[] Exc { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000045 RID: 69
		float[] Innov { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000046 RID: 70
		// (set) Token: 0x06000047 RID: 71
		int Mode { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000048 RID: 72
		// (set) Token: 0x06000049 RID: 73
		bool Vbr { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004A RID: 74
		// (set) Token: 0x0600004B RID: 75
		bool Vad { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004C RID: 76
		// (set) Token: 0x0600004D RID: 77
		bool Dtx { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004E RID: 78
		// (set) Token: 0x0600004F RID: 79
		int Abr { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000050 RID: 80
		// (set) Token: 0x06000051 RID: 81
		float VbrQuality { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000052 RID: 82
		// (set) Token: 0x06000053 RID: 83
		int Complexity { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000054 RID: 84
		// (set) Token: 0x06000055 RID: 85
		int SamplingRate { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000056 RID: 86
		int LookAhead { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000057 RID: 87
		float RelativeQuality { get; }
	}
}
