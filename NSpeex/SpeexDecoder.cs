using System;

namespace NSpeex
{
	// Token: 0x02000006 RID: 6
	public class SpeexDecoder
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000025A0 File Offset: 0x000007A0
		public SpeexDecoder(BandMode mode, bool enhanced = true)
		{
			this.bits = new Bits();
			switch (mode)
			{
			case BandMode.Narrow:
				this.decoder = new NbDecoder();
				this.sampleRate = 8000;
				break;
			case BandMode.Wide:
				this.decoder = new SbDecoder(false);
				this.sampleRate = 16000;
				break;
			case BandMode.UltraWide:
				this.decoder = new SbDecoder(true);
				this.sampleRate = 32000;
				break;
			default:
				this.decoder = new NbDecoder();
				this.sampleRate = 8000;
				break;
			}
			this.decoder.PerceptualEnhancement = enhanced;
			this.frameSize = this.decoder.FrameSize;
			this.decodedData = new float[this.sampleRate * 2];
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002664 File Offset: 0x00000864
		public int FrameSize
		{
			get
			{
				return this.decoder.FrameSize;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002671 File Offset: 0x00000871
		public int SampleRate
		{
			get
			{
				return this.sampleRate;
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000267C File Offset: 0x0000087C
		public int Decode(byte[] inData, int inOffset, int inCount, short[] outData, int outOffset, bool lostFrame)
		{
			if (this.decodedData.Length < outData.Length * 2)
			{
				this.decodedData = new float[outData.Length * 2];
			}
			if (lostFrame || inData == null)
			{
				this.decoder.Decode(null, this.decodedData);
				int i = 0;
				while (i < this.frameSize)
				{
					outData[outOffset] = SpeexDecoder.ConvertToShort(this.decodedData[i]);
					i++;
					outOffset++;
				}
				return this.frameSize;
			}
			this.bits.ReadFrom(inData, inOffset, inCount);
			int num = 0;
			while (this.decoder.Decode(this.bits, this.decodedData) == 0)
			{
				int j = 0;
				while (j < this.frameSize)
				{
					outData[outOffset] = SpeexDecoder.ConvertToShort(this.decodedData[j]);
					j++;
					outOffset++;
				}
				num += this.frameSize;
			}
			return num;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002752 File Offset: 0x00000952
		private static short ConvertToShort(float value)
		{
			if (value > 32767f)
			{
				value = 32767f;
			}
			else if (value < -32768f)
			{
				value = -32768f;
			}
			return (short)Math.Round((double)value);
		}

		// Token: 0x04000001 RID: 1
		private readonly int sampleRate;

		// Token: 0x04000002 RID: 2
		private float[] decodedData;

		// Token: 0x04000003 RID: 3
		private readonly Bits bits;

		// Token: 0x04000004 RID: 4
		private readonly IDecoder decoder;

		// Token: 0x04000005 RID: 5
		private readonly int frameSize;
	}
}
