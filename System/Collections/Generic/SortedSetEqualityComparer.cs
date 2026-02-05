using System;

namespace System.Collections.Generic
{
	// Token: 0x020003CC RID: 972
	internal class SortedSetEqualityComparer<T> : IEqualityComparer<SortedSet<T>>
	{
		// Token: 0x06002531 RID: 9521 RVA: 0x000AD627 File Offset: 0x000AB827
		public SortedSetEqualityComparer()
			: this(null, null)
		{
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x000AD631 File Offset: 0x000AB831
		public SortedSetEqualityComparer(IComparer<T> comparer)
			: this(comparer, null)
		{
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x000AD63B File Offset: 0x000AB83B
		public SortedSetEqualityComparer(IEqualityComparer<T> memberEqualityComparer)
			: this(null, memberEqualityComparer)
		{
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x000AD645 File Offset: 0x000AB845
		public SortedSetEqualityComparer(IComparer<T> comparer, IEqualityComparer<T> memberEqualityComparer)
		{
			if (comparer == null)
			{
				this.comparer = Comparer<T>.Default;
			}
			else
			{
				this.comparer = comparer;
			}
			if (memberEqualityComparer == null)
			{
				this.e_comparer = EqualityComparer<T>.Default;
				return;
			}
			this.e_comparer = memberEqualityComparer;
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x000AD67A File Offset: 0x000AB87A
		public bool Equals(SortedSet<T> x, SortedSet<T> y)
		{
			return SortedSet<T>.SortedSetEquals(x, y, this.comparer);
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x000AD68C File Offset: 0x000AB88C
		public int GetHashCode(SortedSet<T> obj)
		{
			int num = 0;
			if (obj != null)
			{
				foreach (T t in obj)
				{
					num ^= this.e_comparer.GetHashCode(t) & int.MaxValue;
				}
			}
			return num;
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000AD6F0 File Offset: 0x000AB8F0
		public override bool Equals(object obj)
		{
			SortedSetEqualityComparer<T> sortedSetEqualityComparer = obj as SortedSetEqualityComparer<T>;
			return sortedSetEqualityComparer != null && this.comparer == sortedSetEqualityComparer.comparer;
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x000AD717 File Offset: 0x000AB917
		public override int GetHashCode()
		{
			return this.comparer.GetHashCode() ^ this.e_comparer.GetHashCode();
		}

		// Token: 0x0400203D RID: 8253
		private IComparer<T> comparer;

		// Token: 0x0400203E RID: 8254
		private IEqualityComparer<T> e_comparer;
	}
}
