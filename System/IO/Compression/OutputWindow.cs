using System;

namespace System.IO.Compression
{
	// Token: 0x02000437 RID: 1079
	internal class OutputWindow
	{
		// Token: 0x0600286A RID: 10346 RVA: 0x000B9B24 File Offset: 0x000B7D24
		public void Write(byte b)
		{
			byte[] array = this.window;
			int num = this.end;
			this.end = num + 1;
			array[num] = b;
			this.end &= 32767;
			this.bytesUsed++;
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x000B9B6C File Offset: 0x000B7D6C
		public void WriteLengthDistance(int length, int distance)
		{
			this.bytesUsed += length;
			int num = (this.end - distance) & 32767;
			int num2 = 32768 - length;
			if (num > num2 || this.end >= num2)
			{
				while (length-- > 0)
				{
					byte[] array = this.window;
					int num3 = this.end;
					this.end = num3 + 1;
					array[num3] = this.window[num++];
					this.end &= 32767;
					num &= 32767;
				}
				return;
			}
			if (length <= distance)
			{
				Array.Copy(this.window, num, this.window, this.end, length);
				this.end += length;
				return;
			}
			while (length-- > 0)
			{
				byte[] array2 = this.window;
				int num3 = this.end;
				this.end = num3 + 1;
				array2[num3] = this.window[num++];
			}
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x000B9C54 File Offset: 0x000B7E54
		public int CopyFrom(InputBuffer input, int length)
		{
			length = Math.Min(Math.Min(length, 32768 - this.bytesUsed), input.AvailableBytes);
			int num = 32768 - this.end;
			int num2;
			if (length > num)
			{
				num2 = input.CopyTo(this.window, this.end, num);
				if (num2 == num)
				{
					num2 += input.CopyTo(this.window, 0, length - num);
				}
			}
			else
			{
				num2 = input.CopyTo(this.window, this.end, length);
			}
			this.end = (this.end + num2) & 32767;
			this.bytesUsed += num2;
			return num2;
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x0600286D RID: 10349 RVA: 0x000B9CF5 File Offset: 0x000B7EF5
		public int FreeBytes
		{
			get
			{
				return 32768 - this.bytesUsed;
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x0600286E RID: 10350 RVA: 0x000B9D03 File Offset: 0x000B7F03
		public int AvailableBytes
		{
			get
			{
				return this.bytesUsed;
			}
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x000B9D0C File Offset: 0x000B7F0C
		public int CopyTo(byte[] output, int offset, int length)
		{
			int num;
			if (length > this.bytesUsed)
			{
				num = this.end;
				length = this.bytesUsed;
			}
			else
			{
				num = (this.end - this.bytesUsed + length) & 32767;
			}
			int num2 = length;
			int num3 = length - num;
			if (num3 > 0)
			{
				Array.Copy(this.window, 32768 - num3, output, offset, num3);
				offset += num3;
				length = num;
			}
			Array.Copy(this.window, num - length, output, offset, length);
			this.bytesUsed -= num2;
			return num2;
		}

		// Token: 0x0400222B RID: 8747
		private const int WindowSize = 32768;

		// Token: 0x0400222C RID: 8748
		private const int WindowMask = 32767;

		// Token: 0x0400222D RID: 8749
		private byte[] window = new byte[32768];

		// Token: 0x0400222E RID: 8750
		private int end;

		// Token: 0x0400222F RID: 8751
		private int bytesUsed;
	}
}
