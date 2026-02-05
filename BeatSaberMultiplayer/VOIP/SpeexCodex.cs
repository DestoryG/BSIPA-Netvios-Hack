using System;
using NSpeex;

namespace BeatSaberMultiplayer.VOIP
{
	// Token: 0x0200008A RID: 138
	public class SpeexCodex
	{
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x00026269 File Offset: 0x00024469
		// (set) Token: 0x0600095D RID: 2397 RVA: 0x00026271 File Offset: 0x00024471
		public int dataSize { get; set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0002627A File Offset: 0x0002447A
		// (set) Token: 0x0600095F RID: 2399 RVA: 0x00026282 File Offset: 0x00024482
		public BandMode mode { get; set; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x0002628B File Offset: 0x0002448B
		private BandMode SpeedxMode
		{
			get
			{
				if (this.mode == BandMode.UltraWide)
				{
					return BandMode.UltraWide;
				}
				if (this.mode == BandMode.Wide)
				{
					return BandMode.Wide;
				}
				return BandMode.Narrow;
			}
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x000262A4 File Offset: 0x000244A4
		public static SpeexCodex Create(BandMode mode)
		{
			return new SpeexCodex(mode);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x000262AC File Offset: 0x000244AC
		public SpeexCodex(BandMode mode)
		{
			this.mode = mode;
			this.encoder = new SpeexEncoder(this.SpeedxMode);
			this.decoder = new SpeexDecoder(this.SpeedxMode, false);
			this.encoder.VBR = true;
			this.encoder.Quality = 4;
			this.dataSize = this.encoder.FrameSize * ((mode == BandMode.Narrow) ? 8 : ((mode == BandMode.Wide) ? 8 : 2));
			this.encodingBuffer = new short[this.dataSize];
			this.encodedBuffer = new byte[this.dataSize];
			this.decodeBuffer = new short[this.dataSize];
			this.decodedBuffer = new float[this.dataSize];
			float num = (float)AudioUtils.GetFrequency(mode) / (float)this.dataSize;
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x00026375 File Offset: 0x00024575
		public float[] Decode(byte[] data)
		{
			this.decoder.Decode(data, 0, data.Length, this.decodeBuffer, 0, false);
			SpeexCodex.Convert(this.decodeBuffer, ref this.decodedBuffer);
			return this.decodedBuffer;
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x000263A8 File Offset: 0x000245A8
		public byte[] Encode(float[] data)
		{
			SpeexCodex.Convert(data, ref this.encodingBuffer);
			byte[] array = new byte[this.encoder.Encode(this.encodingBuffer, 0, this.encodingBuffer.Length, this.encodedBuffer, 0, this.encodedBuffer.Length)];
			Buffer.BlockCopy(this.encodedBuffer, 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00026404 File Offset: 0x00024604
		private static void Convert(short[] data, ref float[] target)
		{
			for (int i = 0; i < data.Length; i++)
			{
				target[i] = (float)data[i] / 32767f;
			}
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00026430 File Offset: 0x00024630
		private static void Convert(float[] data, ref short[] target)
		{
			for (int i = 0; i < data.Length; i++)
			{
				target[i] = (short)(data[i] * 32767f);
			}
		}

		// Token: 0x0400047B RID: 1147
		private short[] encodingBuffer;

		// Token: 0x0400047C RID: 1148
		private byte[] encodedBuffer;

		// Token: 0x0400047D RID: 1149
		private readonly SpeexEncoder encoder;

		// Token: 0x0400047E RID: 1150
		private short[] decodeBuffer;

		// Token: 0x0400047F RID: 1151
		private float[] decodedBuffer;

		// Token: 0x04000480 RID: 1152
		private readonly SpeexDecoder decoder;
	}
}
