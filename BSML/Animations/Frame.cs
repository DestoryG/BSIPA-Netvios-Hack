using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using BeatSaberMarkupLanguage.Animations.APNG.Chunks;

namespace BeatSaberMarkupLanguage.Animations
{
	// Token: 0x020000C7 RID: 199
	public class Frame
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x00012EAA File Offset: 0x000110AA
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x00012EB2 File Offset: 0x000110B2
		internal IHDRChunk IHDRChunk { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00012EBB File Offset: 0x000110BB
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x00012EC3 File Offset: 0x000110C3
		internal fcTLChunk fcTLChunk { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00012ECC File Offset: 0x000110CC
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x00012ED4 File Offset: 0x000110D4
		internal IENDChunk IENDChunk { get; set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00012EDD File Offset: 0x000110DD
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x00012EE5 File Offset: 0x000110E5
		internal List<OtherChunk> OtherChunks
		{
			get
			{
				return this.otherChunks;
			}
			set
			{
				this.otherChunks = value;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x00012EEE File Offset: 0x000110EE
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x00012EF6 File Offset: 0x000110F6
		internal List<IDATChunk> IDATChunks
		{
			get
			{
				return this.idatChunks;
			}
			set
			{
				this.idatChunks = value;
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00012EFF File Offset: 0x000110FF
		internal void AddOtherChunk(OtherChunk chunk)
		{
			this.otherChunks.Add(chunk);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00012F0D File Offset: 0x0001110D
		internal void AddIDATChunk(IDATChunk chunk)
		{
			this.idatChunks.Add(chunk);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00012F1C File Offset: 0x0001111C
		public MemoryStream GetStream()
		{
			IHDRChunk ihdrchunk = new IHDRChunk(this.IHDRChunk);
			if (this.fcTLChunk != null)
			{
				ihdrchunk.ModifyChunkData(0, Helper.ConvertEndian(this.fcTLChunk.Width));
				ihdrchunk.ModifyChunkData(4, Helper.ConvertEndian(this.fcTLChunk.Height));
			}
			MemoryStream ms = new MemoryStream();
			ms.WriteBytes(Frame.Signature);
			ms.WriteBytes(ihdrchunk.RawData);
			this.otherChunks.ForEach(delegate(OtherChunk o)
			{
				ms.WriteBytes(o.RawData);
			});
			this.idatChunks.ForEach(delegate(IDATChunk i)
			{
				ms.WriteBytes(i.RawData);
			});
			ms.WriteBytes(this.IENDChunk.RawData);
			ms.Position = 0L;
			return ms;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00012FF4 File Offset: 0x000111F4
		public Bitmap ToBitmap()
		{
			Bitmap bitmap = (Bitmap)Image.FromStream(this.GetStream());
			Bitmap bitmap2 = new Bitmap(this.IHDRChunk.Width, this.IHDRChunk.Height);
			Graphics graphics = Graphics.FromImage(bitmap2);
			graphics.CompositingMode = CompositingMode.SourceOver;
			graphics.CompositingQuality = CompositingQuality.GammaCorrected;
			graphics.Clear(Color.FromArgb(0));
			graphics.DrawImage(bitmap, this.fcTLChunk.XOffset, this.fcTLChunk.YOffset, this.fcTLChunk.Width, this.fcTLChunk.Height);
			return bitmap2;
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00013088 File Offset: 0x00011288
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x000130CF File Offset: 0x000112CF
		public int FrameRate
		{
			get
			{
				int num = (int)this.fcTLChunk.DelayNumerator;
				double num2 = (double)(1000 / this.fcTLChunk.DelayDenominator);
				if ((int)Math.Round(num2) != 1)
				{
					num = (int)((double)this.fcTLChunk.DelayNumerator * num2);
				}
				return num;
			}
			internal set
			{
				this.fcTLChunk.DelayNumerator = (ushort)value;
				this.fcTLChunk.DelayDenominator = 1000;
			}
		}

		// Token: 0x04000150 RID: 336
		public static byte[] Signature = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };

		// Token: 0x04000151 RID: 337
		private List<IDATChunk> idatChunks = new List<IDATChunk>();

		// Token: 0x04000152 RID: 338
		private List<OtherChunk> otherChunks = new List<OtherChunk>();
	}
}
