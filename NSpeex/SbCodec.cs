using System;

namespace NSpeex
{
	// Token: 0x02000011 RID: 17
	internal class SbCodec : NbCodec
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00005301 File Offset: 0x00003501
		public SbCodec(bool ultraWide)
		{
			if (ultraWide)
			{
				this.submodes = SbCodec.BuildUwbSubModes();
				this.submodeID = 1;
				return;
			}
			this.submodes = SbCodec.BuildWbSubModes();
			this.submodeID = 3;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00005334 File Offset: 0x00003534
		protected virtual void Init(int frameSize, int subframeSize, int lpcSize, int bufSize, float foldingGain_0)
		{
			base.Init(frameSize, subframeSize, lpcSize, bufSize);
			this.fullFrameSize = 2 * frameSize;
			this.foldingGain = foldingGain_0;
			this.lag_factor = 0.002f;
			this.high = new float[this.fullFrameSize];
			this.y0 = new float[this.fullFrameSize];
			this.y1 = new float[this.fullFrameSize];
			this.x0d = new float[frameSize];
			this.g0_mem = new float[64];
			this.g1_mem = new float[64];
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000053C4 File Offset: 0x000035C4
		protected internal static SubMode[] BuildWbSubModes()
		{
			HighLspQuant highLspQuant = new HighLspQuant();
			SplitShapeSearch splitShapeSearch = new SplitShapeSearch(40, 10, 4, Codebook_Constants.hexc_10_32_table, 5, 0);
			SplitShapeSearch splitShapeSearch2 = new SplitShapeSearch(40, 8, 5, Codebook_Constants.hexc_table, 7, 1);
			SubMode[] array = new SubMode[8];
			array[1] = new SubMode(0, 0, 1, 0, highLspQuant, null, null, 0.75f, 0.75f, -1f, 36);
			array[2] = new SubMode(0, 0, 1, 0, highLspQuant, null, splitShapeSearch, 0.85f, 0.6f, -1f, 112);
			array[3] = new SubMode(0, 0, 1, 0, highLspQuant, null, splitShapeSearch2, 0.75f, 0.7f, -1f, 192);
			array[4] = new SubMode(0, 0, 1, 1, highLspQuant, null, splitShapeSearch2, 0.75f, 0.75f, -1f, 352);
			return array;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00005488 File Offset: 0x00003688
		protected internal static SubMode[] BuildUwbSubModes()
		{
			HighLspQuant highLspQuant = new HighLspQuant();
			SubMode[] array = new SubMode[8];
			array[1] = new SubMode(0, 0, 1, 0, highLspQuant, null, null, 0.75f, 0.75f, -1f, 2);
			return array;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000066 RID: 102 RVA: 0x000054C2 File Offset: 0x000036C2
		public override int FrameSize
		{
			get
			{
				return this.fullFrameSize;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000054CA File Offset: 0x000036CA
		public bool Dtx
		{
			get
			{
				return this.dtx_enabled != 0;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000068 RID: 104 RVA: 0x000054D8 File Offset: 0x000036D8
		public override float[] Exc
		{
			get
			{
				float[] array = new float[this.fullFrameSize];
				for (int i = 0; i < this.frameSize; i++)
				{
					array[2 * i] = 2f * this.excBuf[this.excIdx + i];
				}
				return array;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000069 RID: 105 RVA: 0x0000551D File Offset: 0x0000371D
		public override float[] Innov
		{
			get
			{
				return this.Exc;
			}
		}

		// Token: 0x0400006C RID: 108
		public const int SB_SUBMODES = 8;

		// Token: 0x0400006D RID: 109
		public const int SB_SUBMODE_BITS = 3;

		// Token: 0x0400006E RID: 110
		public const int QMF_ORDER = 64;

		// Token: 0x0400006F RID: 111
		public static readonly int[] SB_FRAME_SIZE = new int[] { 4, 36, 112, 192, 352, -1, -1, -1 };

		// Token: 0x04000070 RID: 112
		protected internal int fullFrameSize;

		// Token: 0x04000071 RID: 113
		protected internal float foldingGain;

		// Token: 0x04000072 RID: 114
		protected internal float[] high;

		// Token: 0x04000073 RID: 115
		protected internal float[] y0;

		// Token: 0x04000074 RID: 116
		protected internal float[] y1;

		// Token: 0x04000075 RID: 117
		protected internal float[] x0d;

		// Token: 0x04000076 RID: 118
		protected internal float[] g0_mem;

		// Token: 0x04000077 RID: 119
		protected internal float[] g1_mem;
	}
}
