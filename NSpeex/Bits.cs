using System;

namespace NSpeex
{
	// Token: 0x02000029 RID: 41
	internal class Bits
	{
		// Token: 0x06000114 RID: 276 RVA: 0x00014880 File Offset: 0x00012A80
		public Bits()
		{
			this.bytes = new byte[1024];
			this.bytePtr = 0;
			this.bitPtr = 0;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000148A8 File Offset: 0x00012AA8
		public void Advance(int n)
		{
			this.bytePtr += n >> 3;
			this.bitPtr += n & 7;
			if (this.bitPtr > 7)
			{
				this.bitPtr -= 8;
				this.bytePtr++;
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000148FA File Offset: 0x00012AFA
		public int Peek()
		{
			return ((this.bytes[this.bytePtr] & byte.MaxValue) >> 7 - this.bitPtr) & 1;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00014920 File Offset: 0x00012B20
		public void ReadFrom(byte[] newbytes, int offset, int len)
		{
			if (this.bytes.Length < len)
			{
				this.bytes = new byte[len];
			}
			for (int i = 0; i < len; i++)
			{
				this.bytes[i] = newbytes[offset + i];
			}
			this.bytePtr = 0;
			this.bitPtr = 0;
			this.nbBits = len * 8;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00014974 File Offset: 0x00012B74
		public int BitsRemaining()
		{
			return this.nbBits - (this.bytePtr * 8 + this.bitPtr);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0001498C File Offset: 0x00012B8C
		public int Unpack(int nbBits)
		{
			int num = 0;
			while (nbBits != 0)
			{
				num <<= 1;
				num |= ((this.bytes[this.bytePtr] & byte.MaxValue) >> 7 - this.bitPtr) & 1;
				this.bitPtr++;
				if (this.bitPtr == 8)
				{
					this.bitPtr = 0;
					this.bytePtr++;
				}
				nbBits--;
			}
			return num;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000149FC File Offset: 0x00012BFC
		public void Pack(int data, int nbBits)
		{
			while (this.bytePtr + (nbBits + this.bitPtr >> 3) >= this.bytes.Length)
			{
				int num = this.bytes.Length * 2;
				byte[] array = new byte[num];
				Array.Copy(this.bytes, 0, array, 0, this.bytes.Length);
				this.bytes = array;
			}
			while (nbBits > 0)
			{
				int num2 = (data >> nbBits - 1) & 1;
				byte[] array2 = this.bytes;
				int num3 = this.bytePtr;
				array2[num3] |= (byte)(num2 << 7 - this.bitPtr);
				this.bitPtr++;
				if (this.bitPtr == 8)
				{
					this.bitPtr = 0;
					this.bytePtr++;
				}
				nbBits--;
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00014AC5 File Offset: 0x00012CC5
		public void InsertTerminator()
		{
			if (this.bitPtr > 0)
			{
				this.Pack(0, 1);
			}
			while (this.bitPtr != 0)
			{
				this.Pack(1, 1);
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00014AEC File Offset: 0x00012CEC
		public int Write(byte[] buffer, int offset, int maxBytes)
		{
			int num = this.bitPtr;
			int num2 = this.bytePtr;
			byte[] array = this.bytes;
			this.InsertTerminator();
			this.bitPtr = num;
			this.bytePtr = num2;
			this.bytes = array;
			if (maxBytes > this.BufferSize)
			{
				maxBytes = this.BufferSize;
			}
			Array.Copy(this.bytes, 0, buffer, offset, maxBytes);
			return maxBytes;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00014B4A File Offset: 0x00012D4A
		public void Reset()
		{
			Array.Clear(this.bytes, 0, this.bytes.Length);
			this.bytePtr = 0;
			this.bitPtr = 0;
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00014B6E File Offset: 0x00012D6E
		public int BufferSize
		{
			get
			{
				return this.bytePtr + ((this.bitPtr > 0) ? 1 : 0);
			}
		}

		// Token: 0x04000135 RID: 309
		public const int DefaultBufferSize = 1024;

		// Token: 0x04000136 RID: 310
		private byte[] bytes;

		// Token: 0x04000137 RID: 311
		private int bytePtr;

		// Token: 0x04000138 RID: 312
		private int bitPtr;

		// Token: 0x04000139 RID: 313
		private int nbBits;
	}
}
