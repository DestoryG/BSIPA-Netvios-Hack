using System;

namespace Mono.Cecil.PE
{
	// Token: 0x020000D7 RID: 215
	internal sealed class TextMap
	{
		// Token: 0x0600093A RID: 2362 RVA: 0x0001E177 File Offset: 0x0001C377
		public void AddMap(TextSegment segment, int length)
		{
			this.map[(int)segment] = new Range(this.GetStart(segment), (uint)length);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0001E192 File Offset: 0x0001C392
		public void AddMap(TextSegment segment, int length, int align)
		{
			align--;
			this.AddMap(segment, (length + align) & ~align);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0001E1A6 File Offset: 0x0001C3A6
		public void AddMap(TextSegment segment, Range range)
		{
			this.map[(int)segment] = range;
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0001E1B5 File Offset: 0x0001C3B5
		public Range GetRange(TextSegment segment)
		{
			return this.map[(int)segment];
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0001E1C4 File Offset: 0x0001C3C4
		public DataDirectory GetDataDirectory(TextSegment segment)
		{
			Range range = this.map[(int)segment];
			return new DataDirectory((range.Length == 0U) ? 0U : range.Start, range.Length);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0001E1FA File Offset: 0x0001C3FA
		public uint GetRVA(TextSegment segment)
		{
			return this.map[(int)segment].Start;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0001E210 File Offset: 0x0001C410
		public uint GetNextRVA(TextSegment segment)
		{
			return this.map[(int)segment].Start + this.map[(int)segment].Length;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0001E242 File Offset: 0x0001C442
		public int GetLength(TextSegment segment)
		{
			return (int)this.map[(int)segment].Length;
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0001E258 File Offset: 0x0001C458
		private uint GetStart(TextSegment segment)
		{
			if (segment != TextSegment.ImportAddressTable)
			{
				return this.ComputeStart((int)segment);
			}
			return 8192U;
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0001E277 File Offset: 0x0001C477
		private uint ComputeStart(int index)
		{
			index--;
			return this.map[index].Start + this.map[index].Length;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0001E2A4 File Offset: 0x0001C4A4
		public uint GetLength()
		{
			Range range = this.map[16];
			return range.Start - 8192U + range.Length;
		}

		// Token: 0x04000386 RID: 902
		private readonly Range[] map = new Range[17];
	}
}
