using System;

namespace System.IO.Compression
{
	// Token: 0x02000434 RID: 1076
	internal class InputBuffer
	{
		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06002848 RID: 10312 RVA: 0x000B9524 File Offset: 0x000B7724
		public int AvailableBits
		{
			get
			{
				return this.bitsInBuffer;
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06002849 RID: 10313 RVA: 0x000B952C File Offset: 0x000B772C
		public int AvailableBytes
		{
			get
			{
				return this.end - this.start + this.bitsInBuffer / 8;
			}
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000B9544 File Offset: 0x000B7744
		public bool EnsureBitsAvailable(int count)
		{
			if (this.bitsInBuffer < count)
			{
				if (this.NeedsInput())
				{
					return false;
				}
				uint num = this.bitBuffer;
				byte[] array = this.buffer;
				int num2 = this.start;
				this.start = num2 + 1;
				this.bitBuffer = num | (array[num2] << (this.bitsInBuffer & 31));
				this.bitsInBuffer += 8;
				if (this.bitsInBuffer < count)
				{
					if (this.NeedsInput())
					{
						return false;
					}
					uint num3 = this.bitBuffer;
					byte[] array2 = this.buffer;
					num2 = this.start;
					this.start = num2 + 1;
					this.bitBuffer = num3 | (array2[num2] << (this.bitsInBuffer & 31));
					this.bitsInBuffer += 8;
				}
			}
			return true;
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x000B95F8 File Offset: 0x000B77F8
		public uint TryLoad16Bits()
		{
			if (this.bitsInBuffer < 8)
			{
				if (this.start < this.end)
				{
					uint num = this.bitBuffer;
					byte[] array = this.buffer;
					int num2 = this.start;
					this.start = num2 + 1;
					this.bitBuffer = num | (array[num2] << (this.bitsInBuffer & 31));
					this.bitsInBuffer += 8;
				}
				if (this.start < this.end)
				{
					uint num3 = this.bitBuffer;
					byte[] array2 = this.buffer;
					int num2 = this.start;
					this.start = num2 + 1;
					this.bitBuffer = num3 | (array2[num2] << (this.bitsInBuffer & 31));
					this.bitsInBuffer += 8;
				}
			}
			else if (this.bitsInBuffer < 16 && this.start < this.end)
			{
				uint num4 = this.bitBuffer;
				byte[] array3 = this.buffer;
				int num2 = this.start;
				this.start = num2 + 1;
				this.bitBuffer = num4 | (array3[num2] << (this.bitsInBuffer & 31));
				this.bitsInBuffer += 8;
			}
			return this.bitBuffer;
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000B9707 File Offset: 0x000B7907
		private uint GetBitMask(int count)
		{
			return (1U << count) - 1U;
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000B9714 File Offset: 0x000B7914
		public int GetBits(int count)
		{
			if (!this.EnsureBitsAvailable(count))
			{
				return -1;
			}
			int num = (int)(this.bitBuffer & this.GetBitMask(count));
			this.bitBuffer >>= count;
			this.bitsInBuffer -= count;
			return num;
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x000B975C File Offset: 0x000B795C
		public int CopyTo(byte[] output, int offset, int length)
		{
			int num = 0;
			while (this.bitsInBuffer > 0 && length > 0)
			{
				output[offset++] = (byte)this.bitBuffer;
				this.bitBuffer >>= 8;
				this.bitsInBuffer -= 8;
				length--;
				num++;
			}
			if (length == 0)
			{
				return num;
			}
			int num2 = this.end - this.start;
			if (length > num2)
			{
				length = num2;
			}
			Array.Copy(this.buffer, this.start, output, offset, length);
			this.start += length;
			return num + length;
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000B97ED File Offset: 0x000B79ED
		public bool NeedsInput()
		{
			return this.start == this.end;
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000B97FD File Offset: 0x000B79FD
		public void SetInput(byte[] buffer, int offset, int length)
		{
			this.buffer = buffer;
			this.start = offset;
			this.end = offset + length;
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x000B9816 File Offset: 0x000B7A16
		public void SkipBits(int n)
		{
			this.bitBuffer >>= n;
			this.bitsInBuffer -= n;
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x000B9837 File Offset: 0x000B7A37
		public void SkipToByteBoundary()
		{
			this.bitBuffer >>= this.bitsInBuffer % 8;
			this.bitsInBuffer -= this.bitsInBuffer % 8;
		}

		// Token: 0x0400221E RID: 8734
		private byte[] buffer;

		// Token: 0x0400221F RID: 8735
		private int start;

		// Token: 0x04002220 RID: 8736
		private int end;

		// Token: 0x04002221 RID: 8737
		private uint bitBuffer;

		// Token: 0x04002222 RID: 8738
		private int bitsInBuffer;
	}
}
