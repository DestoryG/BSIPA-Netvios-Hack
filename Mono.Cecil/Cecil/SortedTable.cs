using System;
using System.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200001E RID: 30
	internal abstract class SortedTable<TRow> : MetadataTable<TRow>, IComparer<TRow> where TRow : struct
	{
		// Token: 0x06000207 RID: 519 RVA: 0x0000AE5C File Offset: 0x0000905C
		public sealed override void Sort()
		{
			MergeSort<TRow>.Sort(this.rows, 0, this.length, this);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000AE71 File Offset: 0x00009071
		protected static int Compare(uint x, uint y)
		{
			if (x == y)
			{
				return 0;
			}
			if (x <= y)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06000209 RID: 521
		public abstract int Compare(TRow x, TRow y);
	}
}
