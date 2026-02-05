using System;
using System.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000CD RID: 205
	internal abstract class SortedTable<TRow> : MetadataTable<TRow>, IComparer<TRow> where TRow : struct
	{
		// Token: 0x06000578 RID: 1400 RVA: 0x00019304 File Offset: 0x00017504
		public sealed override void Sort()
		{
			MergeSort<TRow>.Sort(this.rows, 0, this.length, this);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00019319 File Offset: 0x00017519
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

		// Token: 0x0600057A RID: 1402
		public abstract int Compare(TRow x, TRow y);
	}
}
