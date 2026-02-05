using System;
using System.IO;
using Ionic.Crc;

namespace Ionic.BZip2
{
	// Token: 0x02000045 RID: 69
	public class BZip2InputStream : Stream
	{
		// Token: 0x06000365 RID: 869 RVA: 0x00013E13 File Offset: 0x00012013
		public BZip2InputStream(Stream input)
			: this(input, false)
		{
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00013E1D File Offset: 0x0001201D
		public BZip2InputStream(Stream input, bool leaveOpen)
		{
			this.input = input;
			this._leaveOpen = leaveOpen;
			this.init();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00013E54 File Offset: 0x00012054
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (offset < 0)
			{
				throw new IndexOutOfRangeException(string.Format("offset ({0}) must be > 0", offset));
			}
			if (count < 0)
			{
				throw new IndexOutOfRangeException(string.Format("count ({0}) must be > 0", count));
			}
			if (offset + count > buffer.Length)
			{
				throw new IndexOutOfRangeException(string.Format("offset({0}) count({1}) bLength({2})", offset, count, buffer.Length));
			}
			if (this.input == null)
			{
				throw new IOException("the stream is not open");
			}
			int num = offset + count;
			int num2 = offset;
			int num3;
			while (num2 < num && (num3 = this.ReadByte()) >= 0)
			{
				buffer[num2++] = (byte)num3;
			}
			if (num2 != offset)
			{
				return num2 - offset;
			}
			return -1;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00013F00 File Offset: 0x00012100
		private void MakeMaps()
		{
			bool[] inUse = this.data.inUse;
			byte[] seqToUnseq = this.data.seqToUnseq;
			int num = 0;
			for (int i = 0; i < 256; i++)
			{
				if (inUse[i])
				{
					seqToUnseq[num++] = (byte)i;
				}
			}
			this.nInUse = num;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00013F4C File Offset: 0x0001214C
		public override int ReadByte()
		{
			int num = this.currentChar;
			this.totalBytesRead += 1L;
			switch (this.currentState)
			{
			case BZip2InputStream.CState.EOF:
				return -1;
			case BZip2InputStream.CState.START_BLOCK:
				throw new IOException("bad state");
			case BZip2InputStream.CState.RAND_PART_A:
				throw new IOException("bad state");
			case BZip2InputStream.CState.RAND_PART_B:
				this.SetupRandPartB();
				break;
			case BZip2InputStream.CState.RAND_PART_C:
				this.SetupRandPartC();
				break;
			case BZip2InputStream.CState.NO_RAND_PART_A:
				throw new IOException("bad state");
			case BZip2InputStream.CState.NO_RAND_PART_B:
				this.SetupNoRandPartB();
				break;
			case BZip2InputStream.CState.NO_RAND_PART_C:
				this.SetupNoRandPartC();
				break;
			default:
				throw new IOException("bad state");
			}
			return num;
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600036A RID: 874 RVA: 0x00013FED File Offset: 0x000121ED
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("BZip2Stream");
				}
				return this.input.CanRead;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0001400D File Offset: 0x0001220D
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00014010 File Offset: 0x00012210
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("BZip2Stream");
				}
				return this.input.CanWrite;
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00014030 File Offset: 0x00012230
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("BZip2Stream");
			}
			this.input.Flush();
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00014050 File Offset: 0x00012250
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00014057 File Offset: 0x00012257
		// (set) Token: 0x06000370 RID: 880 RVA: 0x0001405F File Offset: 0x0001225F
		public override long Position
		{
			get
			{
				return this.totalBytesRead;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00014066 File Offset: 0x00012266
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0001406D File Offset: 0x0001226D
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00014074 File Offset: 0x00012274
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0001407C File Offset: 0x0001227C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this.input != null)
					{
						this.input.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000140C8 File Offset: 0x000122C8
		private void init()
		{
			if (this.input == null)
			{
				throw new IOException("No input Stream");
			}
			if (!this.input.CanRead)
			{
				throw new IOException("Unreadable input Stream");
			}
			this.CheckMagicChar('B', 0);
			this.CheckMagicChar('Z', 1);
			this.CheckMagicChar('h', 2);
			int num = this.input.ReadByte();
			if (num < 49 || num > 57)
			{
				throw new IOException("Stream is not BZip2 formatted: illegal blocksize " + (char)num);
			}
			this.blockSize100k = num - 48;
			this.InitBlock();
			this.SetupBlock();
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00014160 File Offset: 0x00012360
		private void CheckMagicChar(char expected, int position)
		{
			int num = this.input.ReadByte();
			if (num != (int)expected)
			{
				string text = string.Format("Not a valid BZip2 stream. byte {0}, expected '{1}', got '{2}'", position, (int)expected, num);
				throw new IOException(text);
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000141A4 File Offset: 0x000123A4
		private void InitBlock()
		{
			char c = this.bsGetUByte();
			char c2 = this.bsGetUByte();
			char c3 = this.bsGetUByte();
			char c4 = this.bsGetUByte();
			char c5 = this.bsGetUByte();
			char c6 = this.bsGetUByte();
			if (c == '\u0017' && c2 == 'r' && c3 == 'E' && c4 == '8' && c5 == 'P' && c6 == '\u0090')
			{
				this.complete();
				return;
			}
			if (c != '1' || c2 != 'A' || c3 != 'Y' || c4 != '&' || c5 != 'S' || c6 != 'Y')
			{
				this.currentState = BZip2InputStream.CState.EOF;
				string text = string.Format("bad block header at offset 0x{0:X}", this.input.Position);
				throw new IOException(text);
			}
			this.storedBlockCRC = this.bsGetInt();
			this.blockRandomised = this.GetBits(1) == 1;
			if (this.data == null)
			{
				this.data = new BZip2InputStream.DecompressionState(this.blockSize100k);
			}
			this.getAndMoveToFrontDecode();
			this.crc.Reset();
			this.currentState = BZip2InputStream.CState.START_BLOCK;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000142A0 File Offset: 0x000124A0
		private void EndBlock()
		{
			this.computedBlockCRC = (uint)this.crc.Crc32Result;
			if (this.storedBlockCRC != this.computedBlockCRC)
			{
				string text = string.Format("BZip2 CRC error (expected {0:X8}, computed {1:X8})", this.storedBlockCRC, this.computedBlockCRC);
				throw new IOException(text);
			}
			this.computedCombinedCRC = (this.computedCombinedCRC << 1) | (this.computedCombinedCRC >> 31);
			this.computedCombinedCRC ^= this.computedBlockCRC;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00014320 File Offset: 0x00012520
		private void complete()
		{
			this.storedCombinedCRC = this.bsGetInt();
			this.currentState = BZip2InputStream.CState.EOF;
			this.data = null;
			if (this.storedCombinedCRC != this.computedCombinedCRC)
			{
				string text = string.Format("BZip2 CRC error (expected {0:X8}, computed {1:X8})", this.storedCombinedCRC, this.computedCombinedCRC);
				throw new IOException(text);
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00014380 File Offset: 0x00012580
		public override void Close()
		{
			Stream stream = this.input;
			if (stream != null)
			{
				try
				{
					if (!this._leaveOpen)
					{
						stream.Close();
					}
				}
				finally
				{
					this.data = null;
					this.input = null;
				}
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x000143C8 File Offset: 0x000125C8
		private int GetBits(int n)
		{
			int num = this.bsLive;
			int num2 = this.bsBuff;
			if (num < n)
			{
				for (;;)
				{
					int num3 = this.input.ReadByte();
					if (num3 < 0)
					{
						break;
					}
					num2 = (num2 << 8) | num3;
					num += 8;
					if (num >= n)
					{
						goto Block_2;
					}
				}
				throw new IOException("unexpected end of stream");
				Block_2:
				this.bsBuff = num2;
			}
			this.bsLive = num - n;
			return (num2 >> num - n) & ((1 << n) - 1);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00014434 File Offset: 0x00012634
		private bool bsGetBit()
		{
			int bits = this.GetBits(1);
			return bits != 0;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00014450 File Offset: 0x00012650
		private char bsGetUByte()
		{
			return (char)this.GetBits(8);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0001445A File Offset: 0x0001265A
		private uint bsGetInt()
		{
			return (uint)((((((this.GetBits(8) << 8) | this.GetBits(8)) << 8) | this.GetBits(8)) << 8) | this.GetBits(8));
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00014484 File Offset: 0x00012684
		private static void hbCreateDecodeTables(int[] limit, int[] bbase, int[] perm, char[] length, int minLen, int maxLen, int alphaSize)
		{
			int i = minLen;
			int num = 0;
			while (i <= maxLen)
			{
				for (int j = 0; j < alphaSize; j++)
				{
					if ((int)length[j] == i)
					{
						perm[num++] = j;
					}
				}
				i++;
			}
			int num2 = BZip2.MaxCodeLength;
			while (--num2 > 0)
			{
				bbase[num2] = 0;
				limit[num2] = 0;
			}
			for (int k = 0; k < alphaSize; k++)
			{
				bbase[(int)(length[k] + '\u0001')]++;
			}
			int l = 1;
			int num3 = bbase[0];
			while (l < BZip2.MaxCodeLength)
			{
				num3 += bbase[l];
				bbase[l] = num3;
				l++;
			}
			int m = minLen;
			int num4 = 0;
			int num5 = bbase[m];
			while (m <= maxLen)
			{
				int num6 = bbase[m + 1];
				num4 += num6 - num5;
				num5 = num6;
				limit[m] = num4 - 1;
				num4 <<= 1;
				m++;
			}
			for (int n = minLen + 1; n <= maxLen; n++)
			{
				bbase[n] = (limit[n - 1] + 1 << 1) - bbase[n];
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0001458C File Offset: 0x0001278C
		private void recvDecodingTables()
		{
			BZip2InputStream.DecompressionState decompressionState = this.data;
			bool[] inUse = decompressionState.inUse;
			byte[] recvDecodingTables_pos = decompressionState.recvDecodingTables_pos;
			int num = 0;
			for (int i = 0; i < 16; i++)
			{
				if (this.bsGetBit())
				{
					num |= 1 << i;
				}
			}
			int num2 = 256;
			while (--num2 >= 0)
			{
				inUse[num2] = false;
			}
			for (int j = 0; j < 16; j++)
			{
				if ((num & (1 << j)) != 0)
				{
					int num3 = j << 4;
					for (int k = 0; k < 16; k++)
					{
						if (this.bsGetBit())
						{
							inUse[num3 + k] = true;
						}
					}
				}
			}
			this.MakeMaps();
			int num4 = this.nInUse + 2;
			int bits = this.GetBits(3);
			int bits2 = this.GetBits(15);
			for (int l = 0; l < bits2; l++)
			{
				int num5 = 0;
				while (this.bsGetBit())
				{
					num5++;
				}
				decompressionState.selectorMtf[l] = (byte)num5;
			}
			int num6 = bits;
			while (--num6 >= 0)
			{
				recvDecodingTables_pos[num6] = (byte)num6;
			}
			for (int m = 0; m < bits2; m++)
			{
				int n = (int)decompressionState.selectorMtf[m];
				byte b = recvDecodingTables_pos[n];
				while (n > 0)
				{
					recvDecodingTables_pos[n] = recvDecodingTables_pos[n - 1];
					n--;
				}
				recvDecodingTables_pos[0] = b;
				decompressionState.selector[m] = b;
			}
			char[][] temp_charArray2d = decompressionState.temp_charArray2d;
			for (int num7 = 0; num7 < bits; num7++)
			{
				int num8 = this.GetBits(5);
				char[] array = temp_charArray2d[num7];
				for (int num9 = 0; num9 < num4; num9++)
				{
					while (this.bsGetBit())
					{
						num8 += (this.bsGetBit() ? (-1) : 1);
					}
					array[num9] = (char)num8;
				}
			}
			this.createHuffmanDecodingTables(num4, bits);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00014748 File Offset: 0x00012948
		private void createHuffmanDecodingTables(int alphaSize, int nGroups)
		{
			BZip2InputStream.DecompressionState decompressionState = this.data;
			char[][] temp_charArray2d = decompressionState.temp_charArray2d;
			for (int i = 0; i < nGroups; i++)
			{
				int num = 32;
				int num2 = 0;
				char[] array = temp_charArray2d[i];
				int num3 = alphaSize;
				while (--num3 >= 0)
				{
					char c = array[num3];
					if ((int)c > num2)
					{
						num2 = (int)c;
					}
					if ((int)c < num)
					{
						num = (int)c;
					}
				}
				BZip2InputStream.hbCreateDecodeTables(decompressionState.gLimit[i], decompressionState.gBase[i], decompressionState.gPerm[i], temp_charArray2d[i], num, num2, alphaSize);
				decompressionState.gMinlen[i] = num;
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000147D0 File Offset: 0x000129D0
		private void getAndMoveToFrontDecode()
		{
			BZip2InputStream.DecompressionState decompressionState = this.data;
			this.origPtr = this.GetBits(24);
			if (this.origPtr < 0)
			{
				throw new IOException("BZ_DATA_ERROR");
			}
			if (this.origPtr > 10 + BZip2.BlockSizeMultiple * this.blockSize100k)
			{
				throw new IOException("BZ_DATA_ERROR");
			}
			this.recvDecodingTables();
			byte[] getAndMoveToFrontDecode_yy = decompressionState.getAndMoveToFrontDecode_yy;
			int num = this.blockSize100k * BZip2.BlockSizeMultiple;
			int num2 = 256;
			while (--num2 >= 0)
			{
				getAndMoveToFrontDecode_yy[num2] = (byte)num2;
				decompressionState.unzftab[num2] = 0;
			}
			int num3 = 0;
			int num4 = BZip2.G_SIZE - 1;
			int num5 = this.nInUse + 1;
			int num6 = this.getAndMoveToFrontDecode0(0);
			int num7 = this.bsBuff;
			int i = this.bsLive;
			int num8 = -1;
			int num9 = (int)(decompressionState.selector[num3] & byte.MaxValue);
			int[] array = decompressionState.gBase[num9];
			int[] array2 = decompressionState.gLimit[num9];
			int[] array3 = decompressionState.gPerm[num9];
			int num10 = decompressionState.gMinlen[num9];
			while (num6 != num5)
			{
				if (num6 == (int)BZip2.RUNA || num6 == (int)BZip2.RUNB)
				{
					int num11 = -1;
					int num12 = 1;
					for (;;)
					{
						if (num6 == (int)BZip2.RUNA)
						{
							num11 += num12;
						}
						else
						{
							if (num6 != (int)BZip2.RUNB)
							{
								break;
							}
							num11 += num12 << 1;
						}
						if (num4 == 0)
						{
							num4 = BZip2.G_SIZE - 1;
							num9 = (int)(decompressionState.selector[++num3] & byte.MaxValue);
							array = decompressionState.gBase[num9];
							array2 = decompressionState.gLimit[num9];
							array3 = decompressionState.gPerm[num9];
							num10 = decompressionState.gMinlen[num9];
						}
						else
						{
							num4--;
						}
						int num13 = num10;
						while (i < num13)
						{
							int num14 = this.input.ReadByte();
							if (num14 < 0)
							{
								goto IL_01B9;
							}
							num7 = (num7 << 8) | num14;
							i += 8;
						}
						int j = (num7 >> i - num13) & ((1 << num13) - 1);
						i -= num13;
						while (j > array2[num13])
						{
							num13++;
							while (i < 1)
							{
								int num15 = this.input.ReadByte();
								if (num15 < 0)
								{
									goto IL_0215;
								}
								num7 = (num7 << 8) | num15;
								i += 8;
							}
							i--;
							j = (j << 1) | ((num7 >> i) & 1);
						}
						num6 = array3[j - array[num13]];
						num12 <<= 1;
					}
					byte b = decompressionState.seqToUnseq[(int)getAndMoveToFrontDecode_yy[0]];
					decompressionState.unzftab[(int)(b & byte.MaxValue)] += num11 + 1;
					while (num11-- >= 0)
					{
						decompressionState.ll8[++num8] = b;
					}
					if (num8 >= num)
					{
						throw new IOException("block overrun");
					}
					continue;
					IL_01B9:
					throw new IOException("unexpected end of stream");
					IL_0215:
					throw new IOException("unexpected end of stream");
				}
				if (++num8 >= num)
				{
					throw new IOException("block overrun");
				}
				byte b2 = getAndMoveToFrontDecode_yy[num6 - 1];
				decompressionState.unzftab[(int)(decompressionState.seqToUnseq[(int)b2] & byte.MaxValue)]++;
				decompressionState.ll8[num8] = decompressionState.seqToUnseq[(int)b2];
				if (num6 <= 16)
				{
					int k = num6 - 1;
					while (k > 0)
					{
						getAndMoveToFrontDecode_yy[k] = getAndMoveToFrontDecode_yy[--k];
					}
				}
				else
				{
					Buffer.BlockCopy(getAndMoveToFrontDecode_yy, 0, getAndMoveToFrontDecode_yy, 1, num6 - 1);
				}
				getAndMoveToFrontDecode_yy[0] = b2;
				if (num4 == 0)
				{
					num4 = BZip2.G_SIZE - 1;
					num9 = (int)(decompressionState.selector[++num3] & byte.MaxValue);
					array = decompressionState.gBase[num9];
					array2 = decompressionState.gLimit[num9];
					array3 = decompressionState.gPerm[num9];
					num10 = decompressionState.gMinlen[num9];
				}
				else
				{
					num4--;
				}
				int num16 = num10;
				while (i < num16)
				{
					int num17 = this.input.ReadByte();
					if (num17 < 0)
					{
						throw new IOException("unexpected end of stream");
					}
					num7 = (num7 << 8) | num17;
					i += 8;
				}
				int l = (num7 >> i - num16) & ((1 << num16) - 1);
				i -= num16;
				while (l > array2[num16])
				{
					num16++;
					while (i < 1)
					{
						int num18 = this.input.ReadByte();
						if (num18 < 0)
						{
							throw new IOException("unexpected end of stream");
						}
						num7 = (num7 << 8) | num18;
						i += 8;
					}
					i--;
					l = (l << 1) | ((num7 >> i) & 1);
				}
				num6 = array3[l - array[num16]];
			}
			this.last = num8;
			this.bsLive = i;
			this.bsBuff = num7;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00014C5C File Offset: 0x00012E5C
		private int getAndMoveToFrontDecode0(int groupNo)
		{
			BZip2InputStream.DecompressionState decompressionState = this.data;
			int num = (int)(decompressionState.selector[groupNo] & byte.MaxValue);
			int[] array = decompressionState.gLimit[num];
			int num2 = decompressionState.gMinlen[num];
			int i = this.GetBits(num2);
			int j = this.bsLive;
			int num3 = this.bsBuff;
			while (i > array[num2])
			{
				num2++;
				while (j < 1)
				{
					int num4 = this.input.ReadByte();
					if (num4 < 0)
					{
						throw new IOException("unexpected end of stream");
					}
					num3 = (num3 << 8) | num4;
					j += 8;
				}
				j--;
				i = (i << 1) | ((num3 >> j) & 1);
			}
			this.bsLive = j;
			this.bsBuff = num3;
			return decompressionState.gPerm[num][i - decompressionState.gBase[num][num2]];
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00014D2C File Offset: 0x00012F2C
		private void SetupBlock()
		{
			if (this.data == null)
			{
				return;
			}
			BZip2InputStream.DecompressionState decompressionState = this.data;
			int[] array = decompressionState.initTT(this.last + 1);
			int i;
			for (i = 0; i <= 255; i++)
			{
				if (decompressionState.unzftab[i] < 0 || decompressionState.unzftab[i] > this.last)
				{
					throw new Exception("BZ_DATA_ERROR");
				}
			}
			decompressionState.cftab[0] = 0;
			for (i = 1; i <= 256; i++)
			{
				decompressionState.cftab[i] = decompressionState.unzftab[i - 1];
			}
			for (i = 1; i <= 256; i++)
			{
				decompressionState.cftab[i] += decompressionState.cftab[i - 1];
			}
			for (i = 0; i <= 256; i++)
			{
				if (decompressionState.cftab[i] < 0 || decompressionState.cftab[i] > this.last + 1)
				{
					string text = string.Format("BZ_DATA_ERROR: cftab[{0}]={1} last={2}", i, decompressionState.cftab[i], this.last);
					throw new Exception(text);
				}
			}
			for (i = 1; i <= 256; i++)
			{
				if (decompressionState.cftab[i - 1] > decompressionState.cftab[i])
				{
					throw new Exception("BZ_DATA_ERROR");
				}
			}
			i = 0;
			int num = this.last;
			while (i <= num)
			{
				array[decompressionState.cftab[(int)(decompressionState.ll8[i] & byte.MaxValue)]++] = i;
				i++;
			}
			if (this.origPtr < 0 || this.origPtr >= array.Length)
			{
				throw new IOException("stream corrupted");
			}
			this.su_tPos = array[this.origPtr];
			this.su_count = 0;
			this.su_i2 = 0;
			this.su_ch2 = 256;
			if (this.blockRandomised)
			{
				this.su_rNToGo = 0;
				this.su_rTPos = 0;
				this.SetupRandPartA();
				return;
			}
			this.SetupNoRandPartA();
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00014F24 File Offset: 0x00013124
		private void SetupRandPartA()
		{
			if (this.su_i2 <= this.last)
			{
				this.su_chPrev = this.su_ch2;
				int num = (int)(this.data.ll8[this.su_tPos] & byte.MaxValue);
				this.su_tPos = this.data.tt[this.su_tPos];
				if (this.su_rNToGo == 0)
				{
					this.su_rNToGo = Rand.Rnums(this.su_rTPos) - 1;
					if (++this.su_rTPos == 512)
					{
						this.su_rTPos = 0;
					}
				}
				else
				{
					this.su_rNToGo--;
				}
				num = (this.su_ch2 = num ^ ((this.su_rNToGo == 1) ? 1 : 0));
				this.su_i2++;
				this.currentChar = num;
				this.currentState = BZip2InputStream.CState.RAND_PART_B;
				this.crc.UpdateCRC((byte)num);
				return;
			}
			this.EndBlock();
			this.InitBlock();
			this.SetupBlock();
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0001501C File Offset: 0x0001321C
		private void SetupNoRandPartA()
		{
			if (this.su_i2 <= this.last)
			{
				this.su_chPrev = this.su_ch2;
				int num = (int)(this.data.ll8[this.su_tPos] & byte.MaxValue);
				this.su_ch2 = num;
				this.su_tPos = this.data.tt[this.su_tPos];
				this.su_i2++;
				this.currentChar = num;
				this.currentState = BZip2InputStream.CState.NO_RAND_PART_B;
				this.crc.UpdateCRC((byte)num);
				return;
			}
			this.currentState = BZip2InputStream.CState.NO_RAND_PART_A;
			this.EndBlock();
			this.InitBlock();
			this.SetupBlock();
		}

		// Token: 0x06000387 RID: 903 RVA: 0x000150C0 File Offset: 0x000132C0
		private void SetupRandPartB()
		{
			if (this.su_ch2 != this.su_chPrev)
			{
				this.currentState = BZip2InputStream.CState.RAND_PART_A;
				this.su_count = 1;
				this.SetupRandPartA();
				return;
			}
			if (++this.su_count >= 4)
			{
				this.su_z = (char)(this.data.ll8[this.su_tPos] & byte.MaxValue);
				this.su_tPos = this.data.tt[this.su_tPos];
				if (this.su_rNToGo == 0)
				{
					this.su_rNToGo = Rand.Rnums(this.su_rTPos) - 1;
					if (++this.su_rTPos == 512)
					{
						this.su_rTPos = 0;
					}
				}
				else
				{
					this.su_rNToGo--;
				}
				this.su_j2 = 0;
				this.currentState = BZip2InputStream.CState.RAND_PART_C;
				if (this.su_rNToGo == 1)
				{
					this.su_z ^= '\u0001';
				}
				this.SetupRandPartC();
				return;
			}
			this.currentState = BZip2InputStream.CState.RAND_PART_A;
			this.SetupRandPartA();
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000151C4 File Offset: 0x000133C4
		private void SetupRandPartC()
		{
			if (this.su_j2 < (int)this.su_z)
			{
				this.currentChar = this.su_ch2;
				this.crc.UpdateCRC((byte)this.su_ch2);
				this.su_j2++;
				return;
			}
			this.currentState = BZip2InputStream.CState.RAND_PART_A;
			this.su_i2++;
			this.su_count = 0;
			this.SetupRandPartA();
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00015230 File Offset: 0x00013430
		private void SetupNoRandPartB()
		{
			if (this.su_ch2 != this.su_chPrev)
			{
				this.su_count = 1;
				this.SetupNoRandPartA();
				return;
			}
			if (++this.su_count >= 4)
			{
				this.su_z = (char)(this.data.ll8[this.su_tPos] & byte.MaxValue);
				this.su_tPos = this.data.tt[this.su_tPos];
				this.su_j2 = 0;
				this.SetupNoRandPartC();
				return;
			}
			this.SetupNoRandPartA();
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000152B8 File Offset: 0x000134B8
		private void SetupNoRandPartC()
		{
			if (this.su_j2 < (int)this.su_z)
			{
				int num = this.su_ch2;
				this.currentChar = num;
				this.crc.UpdateCRC((byte)num);
				this.su_j2++;
				this.currentState = BZip2InputStream.CState.NO_RAND_PART_C;
				return;
			}
			this.su_i2++;
			this.su_count = 0;
			this.SetupNoRandPartA();
		}

		// Token: 0x040001E2 RID: 482
		private bool _disposed;

		// Token: 0x040001E3 RID: 483
		private bool _leaveOpen;

		// Token: 0x040001E4 RID: 484
		private long totalBytesRead;

		// Token: 0x040001E5 RID: 485
		private int last;

		// Token: 0x040001E6 RID: 486
		private int origPtr;

		// Token: 0x040001E7 RID: 487
		private int blockSize100k;

		// Token: 0x040001E8 RID: 488
		private bool blockRandomised;

		// Token: 0x040001E9 RID: 489
		private int bsBuff;

		// Token: 0x040001EA RID: 490
		private int bsLive;

		// Token: 0x040001EB RID: 491
		private readonly CRC32 crc = new CRC32(true);

		// Token: 0x040001EC RID: 492
		private int nInUse;

		// Token: 0x040001ED RID: 493
		private Stream input;

		// Token: 0x040001EE RID: 494
		private int currentChar = -1;

		// Token: 0x040001EF RID: 495
		private BZip2InputStream.CState currentState = BZip2InputStream.CState.START_BLOCK;

		// Token: 0x040001F0 RID: 496
		private uint storedBlockCRC;

		// Token: 0x040001F1 RID: 497
		private uint storedCombinedCRC;

		// Token: 0x040001F2 RID: 498
		private uint computedBlockCRC;

		// Token: 0x040001F3 RID: 499
		private uint computedCombinedCRC;

		// Token: 0x040001F4 RID: 500
		private int su_count;

		// Token: 0x040001F5 RID: 501
		private int su_ch2;

		// Token: 0x040001F6 RID: 502
		private int su_chPrev;

		// Token: 0x040001F7 RID: 503
		private int su_i2;

		// Token: 0x040001F8 RID: 504
		private int su_j2;

		// Token: 0x040001F9 RID: 505
		private int su_rNToGo;

		// Token: 0x040001FA RID: 506
		private int su_rTPos;

		// Token: 0x040001FB RID: 507
		private int su_tPos;

		// Token: 0x040001FC RID: 508
		private char su_z;

		// Token: 0x040001FD RID: 509
		private BZip2InputStream.DecompressionState data;

		// Token: 0x02000046 RID: 70
		private enum CState
		{
			// Token: 0x040001FF RID: 511
			EOF,
			// Token: 0x04000200 RID: 512
			START_BLOCK,
			// Token: 0x04000201 RID: 513
			RAND_PART_A,
			// Token: 0x04000202 RID: 514
			RAND_PART_B,
			// Token: 0x04000203 RID: 515
			RAND_PART_C,
			// Token: 0x04000204 RID: 516
			NO_RAND_PART_A,
			// Token: 0x04000205 RID: 517
			NO_RAND_PART_B,
			// Token: 0x04000206 RID: 518
			NO_RAND_PART_C
		}

		// Token: 0x02000047 RID: 71
		private sealed class DecompressionState
		{
			// Token: 0x0600038B RID: 907 RVA: 0x00015320 File Offset: 0x00013520
			public DecompressionState(int blockSize100k)
			{
				this.unzftab = new int[256];
				this.gLimit = BZip2.InitRectangularArray<int>(BZip2.NGroups, BZip2.MaxAlphaSize);
				this.gBase = BZip2.InitRectangularArray<int>(BZip2.NGroups, BZip2.MaxAlphaSize);
				this.gPerm = BZip2.InitRectangularArray<int>(BZip2.NGroups, BZip2.MaxAlphaSize);
				this.gMinlen = new int[BZip2.NGroups];
				this.cftab = new int[257];
				this.getAndMoveToFrontDecode_yy = new byte[256];
				this.temp_charArray2d = BZip2.InitRectangularArray<char>(BZip2.NGroups, BZip2.MaxAlphaSize);
				this.recvDecodingTables_pos = new byte[BZip2.NGroups];
				this.ll8 = new byte[blockSize100k * BZip2.BlockSizeMultiple];
			}

			// Token: 0x0600038C RID: 908 RVA: 0x0001542C File Offset: 0x0001362C
			public int[] initTT(int length)
			{
				int[] array = this.tt;
				if (array == null || array.Length < length)
				{
					array = (this.tt = new int[length]);
				}
				return array;
			}

			// Token: 0x04000207 RID: 519
			public readonly bool[] inUse = new bool[256];

			// Token: 0x04000208 RID: 520
			public readonly byte[] seqToUnseq = new byte[256];

			// Token: 0x04000209 RID: 521
			public readonly byte[] selector = new byte[BZip2.MaxSelectors];

			// Token: 0x0400020A RID: 522
			public readonly byte[] selectorMtf = new byte[BZip2.MaxSelectors];

			// Token: 0x0400020B RID: 523
			public readonly int[] unzftab;

			// Token: 0x0400020C RID: 524
			public readonly int[][] gLimit;

			// Token: 0x0400020D RID: 525
			public readonly int[][] gBase;

			// Token: 0x0400020E RID: 526
			public readonly int[][] gPerm;

			// Token: 0x0400020F RID: 527
			public readonly int[] gMinlen;

			// Token: 0x04000210 RID: 528
			public readonly int[] cftab;

			// Token: 0x04000211 RID: 529
			public readonly byte[] getAndMoveToFrontDecode_yy;

			// Token: 0x04000212 RID: 530
			public readonly char[][] temp_charArray2d;

			// Token: 0x04000213 RID: 531
			public readonly byte[] recvDecodingTables_pos;

			// Token: 0x04000214 RID: 532
			public int[] tt;

			// Token: 0x04000215 RID: 533
			public byte[] ll8;
		}
	}
}
