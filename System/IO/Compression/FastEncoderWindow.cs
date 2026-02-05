using System;
using System.Diagnostics;

namespace System.IO.Compression
{
	// Token: 0x0200042A RID: 1066
	internal class FastEncoderWindow
	{
		// Token: 0x060027F9 RID: 10233 RVA: 0x000B7912 File Offset: 0x000B5B12
		public FastEncoderWindow()
		{
			this.ResetWindow();
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x060027FA RID: 10234 RVA: 0x000B7920 File Offset: 0x000B5B20
		public int BytesAvailable
		{
			get
			{
				return this.bufEnd - this.bufPos;
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x060027FB RID: 10235 RVA: 0x000B7930 File Offset: 0x000B5B30
		public DeflateInput UnprocessedInput
		{
			get
			{
				return new DeflateInput
				{
					Buffer = this.window,
					StartIndex = this.bufPos,
					Count = this.bufEnd - this.bufPos
				};
			}
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x000B796F File Offset: 0x000B5B6F
		public void FlushWindow()
		{
			this.ResetWindow();
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000B7978 File Offset: 0x000B5B78
		private void ResetWindow()
		{
			this.window = new byte[16646];
			this.prev = new ushort[8450];
			this.lookup = new ushort[2048];
			this.bufPos = 8192;
			this.bufEnd = this.bufPos;
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x060027FE RID: 10238 RVA: 0x000B79CC File Offset: 0x000B5BCC
		public int FreeWindowSpace
		{
			get
			{
				return 16384 - this.bufEnd;
			}
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x000B79DA File Offset: 0x000B5BDA
		public void CopyBytes(byte[] inputBuffer, int startIndex, int count)
		{
			Array.Copy(inputBuffer, startIndex, this.window, this.bufEnd, count);
			this.bufEnd += count;
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x000B7A00 File Offset: 0x000B5C00
		public void MoveWindows()
		{
			Array.Copy(this.window, this.bufPos - 8192, this.window, 0, 8192);
			for (int i = 0; i < 2048; i++)
			{
				int num = (int)(this.lookup[i] - 8192);
				if (num <= 0)
				{
					this.lookup[i] = 0;
				}
				else
				{
					this.lookup[i] = (ushort)num;
				}
			}
			for (int i = 0; i < 8192; i++)
			{
				long num2 = (long)((ulong)this.prev[i] - 8192UL);
				if (num2 <= 0L)
				{
					this.prev[i] = 0;
				}
				else
				{
					this.prev[i] = (ushort)num2;
				}
			}
			this.bufPos = 8192;
			this.bufEnd = this.bufPos;
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000B7ABA File Offset: 0x000B5CBA
		private uint HashValue(uint hash, byte b)
		{
			return (hash << 4) ^ (uint)b;
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x000B7AC4 File Offset: 0x000B5CC4
		private uint InsertString(ref uint hash)
		{
			hash = this.HashValue(hash, this.window[this.bufPos + 2]);
			uint num = (uint)this.lookup[(int)(hash & 2047U)];
			this.lookup[(int)(hash & 2047U)] = (ushort)this.bufPos;
			this.prev[this.bufPos & 8191] = (ushort)num;
			return num;
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x000B7B28 File Offset: 0x000B5D28
		private void InsertStrings(ref uint hash, int matchLen)
		{
			if (this.bufEnd - this.bufPos <= matchLen)
			{
				this.bufPos += matchLen - 1;
				return;
			}
			while (--matchLen > 0)
			{
				this.InsertString(ref hash);
				this.bufPos++;
			}
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x000B7B78 File Offset: 0x000B5D78
		internal bool GetNextSymbolOrMatch(Match match)
		{
			uint num = this.HashValue(0U, this.window[this.bufPos]);
			num = this.HashValue(num, this.window[this.bufPos + 1]);
			int num2 = 0;
			int num3;
			if (this.bufEnd - this.bufPos <= 3)
			{
				num3 = 0;
			}
			else
			{
				int num4 = (int)this.InsertString(ref num);
				if (num4 != 0)
				{
					num3 = this.FindMatch(num4, out num2, 32, 32);
					if (this.bufPos + num3 > this.bufEnd)
					{
						num3 = this.bufEnd - this.bufPos;
					}
				}
				else
				{
					num3 = 0;
				}
			}
			if (num3 < 3)
			{
				match.State = MatchState.HasSymbol;
				match.Symbol = this.window[this.bufPos];
				this.bufPos++;
			}
			else
			{
				this.bufPos++;
				if (num3 <= 6)
				{
					int num5 = 0;
					int num6 = (int)this.InsertString(ref num);
					int num7;
					if (num6 != 0)
					{
						num7 = this.FindMatch(num6, out num5, (num3 < 4) ? 32 : 8, 32);
						if (this.bufPos + num7 > this.bufEnd)
						{
							num7 = this.bufEnd - this.bufPos;
						}
					}
					else
					{
						num7 = 0;
					}
					if (num7 > num3)
					{
						match.State = MatchState.HasSymbolAndMatch;
						match.Symbol = this.window[this.bufPos - 1];
						match.Position = num5;
						match.Length = num7;
						this.bufPos++;
						num3 = num7;
						this.InsertStrings(ref num, num3);
					}
					else
					{
						match.State = MatchState.HasMatch;
						match.Position = num2;
						match.Length = num3;
						num3--;
						this.bufPos++;
						this.InsertStrings(ref num, num3);
					}
				}
				else
				{
					match.State = MatchState.HasMatch;
					match.Position = num2;
					match.Length = num3;
					this.InsertStrings(ref num, num3);
				}
			}
			if (this.bufPos == 16384)
			{
				this.MoveWindows();
			}
			return true;
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x000B7D48 File Offset: 0x000B5F48
		private int FindMatch(int search, out int matchPos, int searchDepth, int niceLength)
		{
			int num = 0;
			int num2 = 0;
			int num3 = this.bufPos - 8192;
			byte b = this.window[this.bufPos];
			while (search > num3)
			{
				if (this.window[search + num] == b)
				{
					int num4 = 0;
					while (num4 < 258 && this.window[this.bufPos + num4] == this.window[search + num4])
					{
						num4++;
					}
					if (num4 > num)
					{
						num = num4;
						num2 = search;
						if (num4 > 32)
						{
							break;
						}
						b = this.window[this.bufPos + num4];
					}
				}
				if (--searchDepth == 0)
				{
					break;
				}
				search = (int)this.prev[search & 8191];
			}
			matchPos = this.bufPos - num2 - 1;
			if (num == 3 && matchPos >= 16384)
			{
				return 0;
			}
			return num;
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x000B7E10 File Offset: 0x000B6010
		[Conditional("DEBUG")]
		private void VerifyHashes()
		{
			for (int i = 0; i < 2048; i++)
			{
				ushort num = this.lookup[i];
				while (num != 0 && this.bufPos - (int)num < 8192)
				{
					ushort num2 = this.prev[(int)(num & 8191)];
					if (this.bufPos - (int)num2 >= 8192)
					{
						break;
					}
					num = num2;
				}
			}
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x000B7E6A File Offset: 0x000B606A
		private uint RecalculateHash(int position)
		{
			return (uint)((((int)this.window[position] << 8) ^ ((int)this.window[position + 1] << 4) ^ (int)this.window[position + 2]) & 2047);
		}

		// Token: 0x040021B7 RID: 8631
		private byte[] window;

		// Token: 0x040021B8 RID: 8632
		private int bufPos;

		// Token: 0x040021B9 RID: 8633
		private int bufEnd;

		// Token: 0x040021BA RID: 8634
		private const int FastEncoderHashShift = 4;

		// Token: 0x040021BB RID: 8635
		private const int FastEncoderHashtableSize = 2048;

		// Token: 0x040021BC RID: 8636
		private const int FastEncoderHashMask = 2047;

		// Token: 0x040021BD RID: 8637
		private const int FastEncoderWindowSize = 8192;

		// Token: 0x040021BE RID: 8638
		private const int FastEncoderWindowMask = 8191;

		// Token: 0x040021BF RID: 8639
		private const int FastEncoderMatch3DistThreshold = 16384;

		// Token: 0x040021C0 RID: 8640
		internal const int MaxMatch = 258;

		// Token: 0x040021C1 RID: 8641
		internal const int MinMatch = 3;

		// Token: 0x040021C2 RID: 8642
		private const int SearchDepth = 32;

		// Token: 0x040021C3 RID: 8643
		private const int GoodLength = 4;

		// Token: 0x040021C4 RID: 8644
		private const int NiceLength = 32;

		// Token: 0x040021C5 RID: 8645
		private const int LazyMatchThreshold = 6;

		// Token: 0x040021C6 RID: 8646
		private ushort[] prev;

		// Token: 0x040021C7 RID: 8647
		private ushort[] lookup;
	}
}
