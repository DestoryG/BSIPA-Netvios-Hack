using System;
using System.IO;
using System.Text;

namespace NSpeex
{
	// Token: 0x02000025 RID: 37
	public class PcmWaveWriter : AudioFileWriter
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x0001297F File Offset: 0x00010B7F
		public PcmWaveWriter(int sampleRate, int channels)
		{
			this.sampleRate = sampleRate;
			this.channels = channels;
			this.isPCM = true;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0001299C File Offset: 0x00010B9C
		public PcmWaveWriter(int mode, int quality, int sampleRate, int channels, int nframes, bool vbr)
		{
			this.mode = mode;
			this.quality = quality;
			this.sampleRate = sampleRate;
			this.channels = channels;
			this.nframes = nframes;
			this.vbr = vbr;
			this.isPCM = false;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000129D8 File Offset: 0x00010BD8
		public override void Close()
		{
			this.raf.BaseStream.Seek(4L, SeekOrigin.Begin);
			int num = (int)this.raf.BaseStream.Length - 8;
			this.raf.Write(num);
			this.raf.BaseStream.Seek(40L, SeekOrigin.Begin);
			this.raf.Write(this.size);
			this.raf.Close();
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00012A4A File Offset: 0x00010C4A
		public override void Open(Stream stream)
		{
			this.raf = new BinaryWriter(stream);
			this.size = 0;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00012A60 File Offset: 0x00010C60
		public override void WriteHeader(string comment)
		{
			byte[] array = Encoding.UTF8.GetBytes("RIFF");
			this.raf.Write(array, 0, array.Length);
			this.raf.Write(0);
			array = Encoding.UTF8.GetBytes("WAVE");
			this.raf.Write(array, 0, array.Length);
			array = Encoding.UTF8.GetBytes("fmt ");
			this.raf.Write(array, 0, array.Length);
			if (this.isPCM)
			{
				this.raf.Write(16);
				this.raf.Write(1);
				this.raf.Write((short)this.channels);
				this.raf.Write(this.sampleRate);
				this.raf.Write(this.sampleRate * this.channels * 2);
				this.raf.Write((short)(this.channels * 2));
				this.raf.Write(16);
			}
			else
			{
				int length = comment.Length;
				this.raf.Write((short)(100 + length));
				this.raf.Write(-24311);
				this.raf.Write((short)this.channels);
				this.raf.Write(this.sampleRate);
				this.raf.Write(PcmWaveWriter.CalculateEffectiveBitrate(this.mode, this.channels, this.quality) + 7 >> 3);
				this.raf.Write((short)PcmWaveWriter.CalculateBlockSize(this.mode, this.channels, this.quality));
				this.raf.Write((short)this.quality);
				this.raf.Write((short)(82 + length));
				this.raf.Write(1);
				this.raf.Write(0);
				this.raf.Write(AudioFileWriter.BuildSpeexHeader(this.sampleRate, this.mode, this.channels, this.vbr, this.nframes));
				this.raf.Write(comment);
			}
			array = Encoding.UTF8.GetBytes("data");
			this.raf.Write(array, 0, array.Length);
			this.raf.Write(0);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00012C90 File Offset: 0x00010E90
		public override void WritePacket(byte[] data, int offset, int len)
		{
			this.raf.Write(data, offset, len);
			this.size += len;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00012CAE File Offset: 0x00010EAE
		private static int CalculateEffectiveBitrate(int mode, int channels, int quality)
		{
			return (PcmWaveWriter.WAVE_FRAME_SIZES[mode, channels - 1, quality] * PcmWaveWriter.WAVE_BITS_PER_FRAME[mode, channels - 1, quality] + 7 >> 3) * 50 * 8 / PcmWaveWriter.WAVE_BITS_PER_FRAME[mode, channels - 1, quality];
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00012CE8 File Offset: 0x00010EE8
		private static int CalculateBlockSize(int mode, int channels, int quality)
		{
			return PcmWaveWriter.WAVE_FRAME_SIZES[mode, channels - 1, quality] * PcmWaveWriter.WAVE_BITS_PER_FRAME[mode, channels - 1, quality] + 7 >> 3;
		}

		// Token: 0x04000104 RID: 260
		protected const short WAVE_FORMAT_PCM = 1;

		// Token: 0x04000105 RID: 261
		protected const short WAVE_FORMAT_SPEEX = -24311;

		// Token: 0x04000106 RID: 262
		public static readonly int[,,] WAVE_FRAME_SIZES = new int[,,]
		{
			{
				{
					8, 8, 8, 1, 1, 2, 2, 2, 2, 2,
					2
				},
				{
					2, 1, 1, 7, 7, 8, 8, 8, 8, 3,
					3
				}
			},
			{
				{
					8, 8, 8, 2, 1, 1, 2, 2, 2, 2,
					2
				},
				{
					1, 2, 2, 8, 7, 6, 3, 3, 3, 3,
					3
				}
			},
			{
				{
					8, 8, 8, 1, 2, 2, 1, 1, 1, 1,
					1
				},
				{
					2, 1, 1, 7, 8, 3, 6, 6, 5, 5,
					5
				}
			}
		};

		// Token: 0x04000107 RID: 263
		public static readonly int[,,] WAVE_BITS_PER_FRAME = new int[,,]
		{
			{
				{
					43, 79, 119, 160, 160, 220, 220, 300, 300, 364,
					492
				},
				{
					60, 96, 136, 177, 177, 237, 237, 317, 317, 381,
					509
				}
			},
			{
				{
					79, 115, 155, 196, 256, 336, 412, 476, 556, 684,
					844
				},
				{
					96, 132, 172, 213, 273, 353, 429, 493, 573, 701,
					861
				}
			},
			{
				{
					83, 151, 191, 232, 292, 372, 448, 512, 592, 720,
					880
				},
				{
					100, 168, 208, 249, 309, 389, 465, 529, 609, 737,
					897
				}
			}
		};

		// Token: 0x04000108 RID: 264
		private BinaryWriter raf;

		// Token: 0x04000109 RID: 265
		private readonly int mode;

		// Token: 0x0400010A RID: 266
		private readonly int quality;

		// Token: 0x0400010B RID: 267
		private readonly int sampleRate;

		// Token: 0x0400010C RID: 268
		private readonly int channels;

		// Token: 0x0400010D RID: 269
		private readonly int nframes;

		// Token: 0x0400010E RID: 270
		private readonly bool vbr;

		// Token: 0x0400010F RID: 271
		private bool isPCM;

		// Token: 0x04000110 RID: 272
		private int size;
	}
}
