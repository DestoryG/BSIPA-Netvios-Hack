using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BeatSaberMarkupLanguage.Animations
{
	// Token: 0x020000CE RID: 206
	public class LockBitmap
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0001365D File Offset: 0x0001185D
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x00013665 File Offset: 0x00011865
		public byte[] Pixels { get; set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x0001366E File Offset: 0x0001186E
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x00013676 File Offset: 0x00011876
		public int Depth { get; private set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0001367F File Offset: 0x0001187F
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x00013687 File Offset: 0x00011887
		public int Width { get; private set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x00013690 File Offset: 0x00011890
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x00013698 File Offset: 0x00011898
		public int Height { get; private set; }

		// Token: 0x06000472 RID: 1138 RVA: 0x000136A1 File Offset: 0x000118A1
		public LockBitmap(Bitmap source)
		{
			this.source = source;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x000136BC File Offset: 0x000118BC
		public void LockBits()
		{
			try
			{
				this.Width = this.source.Width;
				this.Height = this.source.Height;
				int num = this.Width * this.Height;
				Rectangle rectangle = new Rectangle(0, 0, this.Width, this.Height);
				this.Depth = Image.GetPixelFormatSize(this.source.PixelFormat);
				if (this.Depth != 8 && this.Depth != 24 && this.Depth != 32)
				{
					throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
				}
				this.bitmapData = this.source.LockBits(rectangle, ImageLockMode.ReadWrite, this.source.PixelFormat);
				int num2 = this.Depth / 8;
				this.Pixels = new byte[num * num2];
				this.Iptr = this.bitmapData.Scan0;
				Marshal.Copy(this.Iptr, this.Pixels, 0, this.Pixels.Length);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000137C0 File Offset: 0x000119C0
		public void UnlockBits()
		{
			try
			{
				Marshal.Copy(this.Pixels, 0, this.Iptr, this.Pixels.Length);
				this.source.UnlockBits(this.bitmapData);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0001380C File Offset: 0x00011A0C
		public Color GetPixel(int x, int y)
		{
			Color color = Color.Empty;
			int num = this.Depth / 8;
			int num2 = (y * this.Width + x) * num;
			if (num2 > this.Pixels.Length - num)
			{
				throw new IndexOutOfRangeException();
			}
			if (this.Depth == 32)
			{
				byte b = this.Pixels[num2];
				byte b2 = this.Pixels[num2 + 1];
				byte b3 = this.Pixels[num2 + 2];
				color = Color.FromArgb((int)this.Pixels[num2 + 3], (int)b3, (int)b2, (int)b);
			}
			if (this.Depth == 24)
			{
				byte b4 = this.Pixels[num2];
				byte b5 = this.Pixels[num2 + 1];
				color = Color.FromArgb((int)this.Pixels[num2 + 2], (int)b5, (int)b4);
			}
			if (this.Depth == 8)
			{
				byte b6 = this.Pixels[num2];
				color = Color.FromArgb((int)b6, (int)b6, (int)b6);
			}
			return color;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000138DC File Offset: 0x00011ADC
		public void SetPixel(int x, int y, Color color)
		{
			int num = this.Depth / 8;
			int num2 = (y * this.Width + x) * num;
			if (this.Depth == 32)
			{
				this.Pixels[num2] = color.B;
				this.Pixels[num2 + 1] = color.G;
				this.Pixels[num2 + 2] = color.R;
				this.Pixels[num2 + 3] = color.A;
			}
			if (this.Depth == 24)
			{
				this.Pixels[num2] = color.B;
				this.Pixels[num2 + 1] = color.G;
				this.Pixels[num2 + 2] = color.R;
			}
			if (this.Depth == 8)
			{
				this.Pixels[num2] = color.B;
			}
		}

		// Token: 0x0400015D RID: 349
		private Bitmap source;

		// Token: 0x0400015E RID: 350
		private IntPtr Iptr = IntPtr.Zero;

		// Token: 0x0400015F RID: 351
		private BitmapData bitmapData;
	}
}
