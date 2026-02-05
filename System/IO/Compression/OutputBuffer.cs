using System;

namespace System.IO.Compression
{
	// Token: 0x02000436 RID: 1078
	internal class OutputBuffer
	{
		// Token: 0x0600285D RID: 10333 RVA: 0x000B98BA File Offset: 0x000B7ABA
		internal void UpdateBuffer(byte[] output)
		{
			this.byteBuffer = output;
			this.pos = 0;
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x0600285E RID: 10334 RVA: 0x000B98CA File Offset: 0x000B7ACA
		internal int BytesWritten
		{
			get
			{
				return this.pos;
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x0600285F RID: 10335 RVA: 0x000B98D2 File Offset: 0x000B7AD2
		internal int FreeBytes
		{
			get
			{
				return this.byteBuffer.Length - this.pos;
			}
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x000B98E4 File Offset: 0x000B7AE4
		internal void WriteUInt16(ushort value)
		{
			byte[] array = this.byteBuffer;
			int num = this.pos;
			this.pos = num + 1;
			array[num] = (byte)value;
			byte[] array2 = this.byteBuffer;
			num = this.pos;
			this.pos = num + 1;
			array2[num] = (byte)(value >> 8);
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000B9928 File Offset: 0x000B7B28
		internal void WriteBits(int n, uint bits)
		{
			this.bitBuf |= bits << this.bitCount;
			this.bitCount += n;
			if (this.bitCount >= 16)
			{
				byte[] array = this.byteBuffer;
				int num = this.pos;
				this.pos = num + 1;
				array[num] = (byte)this.bitBuf;
				byte[] array2 = this.byteBuffer;
				num = this.pos;
				this.pos = num + 1;
				array2[num] = (byte)(this.bitBuf >> 8);
				this.bitCount -= 16;
				this.bitBuf >>= 16;
			}
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000B99C4 File Offset: 0x000B7BC4
		internal void FlushBits()
		{
			while (this.bitCount >= 8)
			{
				byte[] array = this.byteBuffer;
				int num = this.pos;
				this.pos = num + 1;
				array[num] = (byte)this.bitBuf;
				this.bitCount -= 8;
				this.bitBuf >>= 8;
			}
			if (this.bitCount > 0)
			{
				byte[] array2 = this.byteBuffer;
				int num = this.pos;
				this.pos = num + 1;
				array2[num] = (byte)this.bitBuf;
				this.bitBuf = 0U;
				this.bitCount = 0;
			}
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x000B9A4D File Offset: 0x000B7C4D
		internal void WriteBytes(byte[] byteArray, int offset, int count)
		{
			if (this.bitCount == 0)
			{
				Array.Copy(byteArray, offset, this.byteBuffer, this.pos, count);
				this.pos += count;
				return;
			}
			this.WriteBytesUnaligned(byteArray, offset, count);
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x000B9A84 File Offset: 0x000B7C84
		private void WriteBytesUnaligned(byte[] byteArray, int offset, int count)
		{
			for (int i = 0; i < count; i++)
			{
				byte b = byteArray[offset + i];
				this.WriteByteUnaligned(b);
			}
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x000B9AAA File Offset: 0x000B7CAA
		private void WriteByteUnaligned(byte b)
		{
			this.WriteBits(8, (uint)b);
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06002866 RID: 10342 RVA: 0x000B9AB4 File Offset: 0x000B7CB4
		internal int BitsInBuffer
		{
			get
			{
				return this.bitCount / 8 + 1;
			}
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x000B9AC0 File Offset: 0x000B7CC0
		internal OutputBuffer.BufferState DumpState()
		{
			OutputBuffer.BufferState bufferState;
			bufferState.pos = this.pos;
			bufferState.bitBuf = this.bitBuf;
			bufferState.bitCount = this.bitCount;
			return bufferState;
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x000B9AF5 File Offset: 0x000B7CF5
		internal void RestoreState(OutputBuffer.BufferState state)
		{
			this.pos = state.pos;
			this.bitBuf = state.bitBuf;
			this.bitCount = state.bitCount;
		}

		// Token: 0x04002227 RID: 8743
		private byte[] byteBuffer;

		// Token: 0x04002228 RID: 8744
		private int pos;

		// Token: 0x04002229 RID: 8745
		private uint bitBuf;

		// Token: 0x0400222A RID: 8746
		private int bitCount;

		// Token: 0x0200082C RID: 2092
		internal struct BufferState
		{
			// Token: 0x040035DF RID: 13791
			internal int pos;

			// Token: 0x040035E0 RID: 13792
			internal uint bitBuf;

			// Token: 0x040035E1 RID: 13793
			internal int bitCount;
		}
	}
}
