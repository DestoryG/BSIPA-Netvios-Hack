using System;

namespace NSpeex
{
	// Token: 0x0200000B RID: 11
	internal class NbCodec
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00003A6C File Offset: 0x00001C6C
		public NbCodec()
		{
			this.m_lsp = new Lsp();
			this.filters = new Filters();
			this.Nbinit();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003A90 File Offset: 0x00001C90
		private void Nbinit()
		{
			this.submodes = NbCodec.BuildNbSubModes();
			this.submodeID = 5;
			this.Init(160, 40, 10, 640);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003AB8 File Offset: 0x00001CB8
		protected virtual void Init(int frameSize, int subframeSize, int lpcSize, int bufSize)
		{
			this.first = 1;
			this.frameSize = frameSize;
			this.windowSize = frameSize * 3 / 2;
			this.subframeSize = subframeSize;
			this.nbSubframes = frameSize / subframeSize;
			this.lpcSize = lpcSize;
			this.bufSize = bufSize;
			this.min_pitch = 17;
			this.max_pitch = 144;
			this.preemph = 0f;
			this.pre_mem = 0f;
			this.gamma1 = 0.9f;
			this.gamma2 = 0.6f;
			this.lag_factor = 0.01f;
			this.lpc_floor = 1.0001f;
			this.frmBuf = new float[bufSize];
			this.frmIdx = bufSize - this.windowSize;
			this.excBuf = new float[bufSize];
			this.excIdx = bufSize - this.windowSize;
			this.innov = new float[frameSize];
			this.lpc = new float[lpcSize + 1];
			this.qlsp = new float[lpcSize];
			this.old_qlsp = new float[lpcSize];
			this.interp_qlsp = new float[lpcSize];
			this.interp_qlpc = new float[lpcSize + 1];
			this.mem_sp = new float[5 * lpcSize];
			this.pi_gain = new float[this.nbSubframes];
			this.awk1 = new float[lpcSize + 1];
			this.awk2 = new float[lpcSize + 1];
			this.awk3 = new float[lpcSize + 1];
			this.voc_m1 = (this.voc_m2 = (this.voc_mean = 0f));
			this.voc_offset = 0;
			this.dtx_enabled = 0;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003C4C File Offset: 0x00001E4C
		private static SubMode[] BuildNbSubModes()
		{
			Ltp3Tap ltp3Tap = new Ltp3Tap(Codebook_Constants.gain_cdbk_nb, 7, 7);
			Ltp3Tap ltp3Tap2 = new Ltp3Tap(Codebook_Constants.gain_cdbk_lbr, 5, 0);
			Ltp3Tap ltp3Tap3 = new Ltp3Tap(Codebook_Constants.gain_cdbk_lbr, 5, 7);
			Ltp3Tap ltp3Tap4 = new Ltp3Tap(Codebook_Constants.gain_cdbk_lbr, 5, 7);
			LtpForcedPitch ltpForcedPitch = new LtpForcedPitch();
			NoiseSearch noiseSearch = new NoiseSearch();
			SplitShapeSearch splitShapeSearch = new SplitShapeSearch(40, 10, 4, Codebook_Constants.exc_10_16_table, 4, 0);
			SplitShapeSearch splitShapeSearch2 = new SplitShapeSearch(40, 10, 4, Codebook_Constants.exc_10_32_table, 5, 0);
			SplitShapeSearch splitShapeSearch3 = new SplitShapeSearch(40, 5, 8, Codebook_Constants.exc_5_64_table, 6, 0);
			SplitShapeSearch splitShapeSearch4 = new SplitShapeSearch(40, 8, 5, Codebook_Constants.exc_8_128_table, 7, 0);
			SplitShapeSearch splitShapeSearch5 = new SplitShapeSearch(40, 5, 8, Codebook_Constants.exc_5_256_table, 8, 0);
			SplitShapeSearch splitShapeSearch6 = new SplitShapeSearch(40, 20, 2, Codebook_Constants.exc_20_32_table, 5, 0);
			NbLspQuant nbLspQuant = new NbLspQuant();
			LbrLspQuant lbrLspQuant = new LbrLspQuant();
			SubMode[] array = new SubMode[16];
			array[1] = new SubMode(0, 1, 0, 0, lbrLspQuant, ltpForcedPitch, noiseSearch, 0.7f, 0.7f, -1f, 43);
			array[2] = new SubMode(0, 0, 0, 0, lbrLspQuant, ltp3Tap2, splitShapeSearch, 0.7f, 0.5f, 0.55f, 119);
			array[3] = new SubMode(-1, 0, 1, 0, lbrLspQuant, ltp3Tap3, splitShapeSearch2, 0.7f, 0.55f, 0.45f, 160);
			array[4] = new SubMode(-1, 0, 1, 0, lbrLspQuant, ltp3Tap4, splitShapeSearch4, 0.7f, 0.63f, 0.35f, 220);
			array[5] = new SubMode(-1, 0, 3, 0, nbLspQuant, ltp3Tap, splitShapeSearch3, 0.7f, 0.65f, 0.25f, 300);
			array[6] = new SubMode(-1, 0, 3, 0, nbLspQuant, ltp3Tap, splitShapeSearch5, 0.68f, 0.65f, 0.1f, 364);
			array[7] = new SubMode(-1, 0, 3, 1, nbLspQuant, ltp3Tap, splitShapeSearch3, 0.65f, 0.65f, -1f, 492);
			array[8] = new SubMode(0, 1, 0, 0, lbrLspQuant, ltpForcedPitch, splitShapeSearch6, 0.7f, 0.5f, 0.65f, 79);
			return array;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00003E4C File Offset: 0x0000204C
		public virtual int FrameSize
		{
			get
			{
				return this.frameSize;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00003E54 File Offset: 0x00002054
		public float[] PiGain
		{
			get
			{
				return this.pi_gain;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00003E5C File Offset: 0x0000205C
		public virtual float[] Exc
		{
			get
			{
				float[] array = new float[this.frameSize];
				Array.Copy(this.excBuf, this.excIdx, array, 0, this.frameSize);
				return array;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00003E8F File Offset: 0x0000208F
		public virtual float[] Innov
		{
			get
			{
				return this.innov;
			}
		}

		// Token: 0x0400001B RID: 27
		protected const float VERY_SMALL = 0f;

		// Token: 0x0400001C RID: 28
		protected const int NB_SUBMODES = 16;

		// Token: 0x0400001D RID: 29
		protected const int NB_SUBMODE_BITS = 4;

		// Token: 0x0400001E RID: 30
		protected static readonly int[] NB_FRAME_SIZE = new int[]
		{
			5, 43, 119, 160, 220, 300, 364, 492, 79, 1,
			1, 1, 1, 1, 1, 1
		};

		// Token: 0x0400001F RID: 31
		protected static readonly float[] exc_gain_quant_scal1 = new float[] { -0.35f, 0.05f };

		// Token: 0x04000020 RID: 32
		protected static readonly float[] exc_gain_quant_scal3 = new float[] { -2.79475f, -1.81066f, -1.16985f, -0.848119f, -0.58719f, -0.329818f, -0.063266f, 0.282826f };

		// Token: 0x04000021 RID: 33
		protected internal Lsp m_lsp;

		// Token: 0x04000022 RID: 34
		protected internal Filters filters;

		// Token: 0x04000023 RID: 35
		protected internal SubMode[] submodes;

		// Token: 0x04000024 RID: 36
		protected internal int submodeID;

		// Token: 0x04000025 RID: 37
		protected internal int first;

		// Token: 0x04000026 RID: 38
		protected internal int frameSize;

		// Token: 0x04000027 RID: 39
		protected internal int subframeSize;

		// Token: 0x04000028 RID: 40
		protected internal int nbSubframes;

		// Token: 0x04000029 RID: 41
		protected internal int windowSize;

		// Token: 0x0400002A RID: 42
		protected internal int lpcSize;

		// Token: 0x0400002B RID: 43
		protected internal int bufSize;

		// Token: 0x0400002C RID: 44
		protected internal int min_pitch;

		// Token: 0x0400002D RID: 45
		protected internal int max_pitch;

		// Token: 0x0400002E RID: 46
		protected internal float gamma1;

		// Token: 0x0400002F RID: 47
		protected internal float gamma2;

		// Token: 0x04000030 RID: 48
		protected internal float lag_factor;

		// Token: 0x04000031 RID: 49
		protected internal float lpc_floor;

		// Token: 0x04000032 RID: 50
		protected internal float preemph;

		// Token: 0x04000033 RID: 51
		protected internal float pre_mem;

		// Token: 0x04000034 RID: 52
		protected internal float[] frmBuf;

		// Token: 0x04000035 RID: 53
		protected internal int frmIdx;

		// Token: 0x04000036 RID: 54
		protected internal float[] excBuf;

		// Token: 0x04000037 RID: 55
		protected internal int excIdx;

		// Token: 0x04000038 RID: 56
		protected internal float[] innov;

		// Token: 0x04000039 RID: 57
		protected internal float[] lpc;

		// Token: 0x0400003A RID: 58
		protected internal float[] qlsp;

		// Token: 0x0400003B RID: 59
		protected internal float[] old_qlsp;

		// Token: 0x0400003C RID: 60
		protected internal float[] interp_qlsp;

		// Token: 0x0400003D RID: 61
		protected internal float[] interp_qlpc;

		// Token: 0x0400003E RID: 62
		protected internal float[] mem_sp;

		// Token: 0x0400003F RID: 63
		protected internal float[] pi_gain;

		// Token: 0x04000040 RID: 64
		protected internal float[] awk1;

		// Token: 0x04000041 RID: 65
		protected internal float[] awk2;

		// Token: 0x04000042 RID: 66
		protected internal float[] awk3;

		// Token: 0x04000043 RID: 67
		protected internal float voc_m1;

		// Token: 0x04000044 RID: 68
		protected internal float voc_m2;

		// Token: 0x04000045 RID: 69
		protected internal float voc_mean;

		// Token: 0x04000046 RID: 70
		protected internal int voc_offset;

		// Token: 0x04000047 RID: 71
		protected internal int dtx_enabled;
	}
}
