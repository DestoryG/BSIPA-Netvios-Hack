using System;

namespace Mono.Cecil.PE
{
	// Token: 0x0200019A RID: 410
	internal sealed class TextMap
	{
		// Token: 0x06000D1C RID: 3356 RVA: 0x0002D28D File Offset: 0x0002B48D
		public void AddMap(TextSegment segment, int length)
		{
			this.map[(int)segment] = new Range(this.GetStart(segment), (uint)length);
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0002D2A8 File Offset: 0x0002B4A8
		public void AddMap(TextSegment segment, int length, int align)
		{
			align--;
			this.AddMap(segment, (length + align) & ~align);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0002D2BC File Offset: 0x0002B4BC
		public void AddMap(TextSegment segment, Range range)
		{
			this.map[(int)segment] = range;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0002D2CB File Offset: 0x0002B4CB
		public Range GetRange(TextSegment segment)
		{
			return this.map[(int)segment];
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0002D2DC File Offset: 0x0002B4DC
		public DataDirectory GetDataDirectory(TextSegment segment)
		{
			Range range = this.map[(int)segment];
			return new DataDirectory((range.Length == 0U) ? 0U : range.Start, range.Length);
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0002D312 File Offset: 0x0002B512
		public uint GetRVA(TextSegment segment)
		{
			return this.map[(int)segment].Start;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0002D328 File Offset: 0x0002B528
		public uint GetNextRVA(TextSegment segment)
		{
			return this.map[(int)segment].Start + this.map[(int)segment].Length;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0002D35A File Offset: 0x0002B55A
		public int GetLength(TextSegment segment)
		{
			return (int)this.map[(int)segment].Length;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0002D370 File Offset: 0x0002B570
		private uint GetStart(TextSegment segment)
		{
			if (segment != TextSegment.ImportAddressTable)
			{
				return this.ComputeStart((int)segment);
			}
			return 8192U;
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0002D38F File Offset: 0x0002B58F
		private uint ComputeStart(int index)
		{
			index--;
			return this.map[index].Start + this.map[index].Length;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0002D3BC File Offset: 0x0002B5BC
		public uint GetLength()
		{
			Range range = this.map[16];
			return range.Start - 8192U + range.Length;
		}

		// Token: 0x040005E5 RID: 1509
		private readonly Range[] map = new Range[17];
	}
}
