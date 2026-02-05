using System;

namespace NSpeex
{
	// Token: 0x0200001D RID: 29
	public class SpeexEncoder
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x00008F7C File Offset: 0x0000717C
		public SpeexEncoder(BandMode mode)
		{
			this.bits = new Bits();
			switch (mode)
			{
			case BandMode.Narrow:
				this.encoder = new NbEncoder();
				break;
			case BandMode.Wide:
				this.encoder = new SbEncoder(false);
				break;
			case BandMode.UltraWide:
				this.encoder = new SbEncoder(true);
				break;
			default:
				throw new ArgumentException("Invalid mode", "mode");
			}
			this.frameSize = this.encoder.FrameSize;
			this.rawData = new float[this.frameSize];
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000900B File Offset: 0x0000720B
		public int SampleRate
		{
			get
			{
				return this.encoder.SamplingRate;
			}
		}

		// Token: 0x1700003E RID: 62
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00009018 File Offset: 0x00007218
		public int Quality
		{
			set
			{
				this.encoder.Quality = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00009026 File Offset: 0x00007226
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00009033 File Offset: 0x00007233
		public bool VBR
		{
			get
			{
				return this.encoder.Vbr;
			}
			set
			{
				this.encoder.Vbr = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00009041 File Offset: 0x00007241
		public int FrameSize
		{
			get
			{
				return this.frameSize;
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000904C File Offset: 0x0000724C
		public int Encode(short[] inData, int inOffset, int inCount, byte[] outData, int outOffset, int outCount)
		{
			this.bits.Reset();
			int i = 0;
			int num = 0;
			while (i < inCount)
			{
				for (int j = 0; j < this.frameSize; j++)
				{
					this.rawData[j] = (float)inData[inOffset + j + i];
				}
				num += this.encoder.Encode(this.bits, this.rawData);
				i += this.frameSize;
			}
			if (num == 0)
			{
				return 0;
			}
			return this.bits.Write(outData, outOffset, outCount);
		}

		// Token: 0x040000DE RID: 222
		public const string Version = ".Net Speex Encoder v0.0.1";

		// Token: 0x040000DF RID: 223
		private readonly IEncoder encoder;

		// Token: 0x040000E0 RID: 224
		private readonly Bits bits;

		// Token: 0x040000E1 RID: 225
		private readonly float[] rawData;

		// Token: 0x040000E2 RID: 226
		private readonly int frameSize;
	}
}
