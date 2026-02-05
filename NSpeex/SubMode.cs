using System;

namespace NSpeex
{
	// Token: 0x02000019 RID: 25
	internal class SubMode
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00008CB4 File Offset: 0x00006EB4
		public SubMode(int lbrPitch, int forcedPitchGain, int haveSubframeGain, int doubleCodebook, LspQuant lspQuant, Ltp ltp, CodebookSearch innovation, float lpcEnhK1, float lpcEnhK2, float combGain, int bitsPerFrame)
		{
			this.lbrPitch = lbrPitch;
			this.forcedPitchGain = forcedPitchGain;
			this.haveSubframeGain = haveSubframeGain;
			this.doubleCodebook = doubleCodebook;
			this.lsqQuant = lspQuant;
			this.ltp = ltp;
			this.innovation = innovation;
			this.lpcEnhK1 = lpcEnhK1;
			this.lpcEnhK2 = lpcEnhK2;
			this.combGain = combGain;
			this.bitsPerFrame = bitsPerFrame;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00008D1C File Offset: 0x00006F1C
		public int LbrPitch
		{
			get
			{
				return this.lbrPitch;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00008D24 File Offset: 0x00006F24
		public int ForcedPitchGain
		{
			get
			{
				return this.forcedPitchGain;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00008D2C File Offset: 0x00006F2C
		public int HaveSubframeGain
		{
			get
			{
				return this.haveSubframeGain;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00008D34 File Offset: 0x00006F34
		public int DoubleCodebook
		{
			get
			{
				return this.doubleCodebook;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00008D3C File Offset: 0x00006F3C
		public LspQuant LsqQuant
		{
			get
			{
				return this.lsqQuant;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00008D44 File Offset: 0x00006F44
		public Ltp Ltp
		{
			get
			{
				return this.ltp;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00008D4C File Offset: 0x00006F4C
		public CodebookSearch Innovation
		{
			get
			{
				return this.innovation;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00008D54 File Offset: 0x00006F54
		public float LpcEnhK1
		{
			get
			{
				return this.lpcEnhK1;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00008D5C File Offset: 0x00006F5C
		public float LpcEnhK2
		{
			get
			{
				return this.lpcEnhK2;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00008D64 File Offset: 0x00006F64
		public float CombGain
		{
			get
			{
				return this.combGain;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00008D6C File Offset: 0x00006F6C
		public int BitsPerFrame
		{
			get
			{
				return this.bitsPerFrame;
			}
		}

		// Token: 0x040000CE RID: 206
		private readonly int lbrPitch;

		// Token: 0x040000CF RID: 207
		private readonly int forcedPitchGain;

		// Token: 0x040000D0 RID: 208
		private readonly int haveSubframeGain;

		// Token: 0x040000D1 RID: 209
		private readonly int doubleCodebook;

		// Token: 0x040000D2 RID: 210
		private readonly LspQuant lsqQuant;

		// Token: 0x040000D3 RID: 211
		private readonly Ltp ltp;

		// Token: 0x040000D4 RID: 212
		private readonly CodebookSearch innovation;

		// Token: 0x040000D5 RID: 213
		private readonly float lpcEnhK1;

		// Token: 0x040000D6 RID: 214
		private readonly float lpcEnhK2;

		// Token: 0x040000D7 RID: 215
		private readonly float combGain;

		// Token: 0x040000D8 RID: 216
		private readonly int bitsPerFrame;
	}
}
