using System;
using System.Collections.Generic;

namespace Mono
{
	// Token: 0x020000B1 RID: 177
	internal class MergeSort<T>
	{
		// Token: 0x0600035F RID: 863 RVA: 0x0001021A File Offset: 0x0000E41A
		private MergeSort(T[] elements, IComparer<T> comparer)
		{
			this.elements = elements;
			this.buffer = new T[elements.Length];
			Array.Copy(this.elements, this.buffer, elements.Length);
			this.comparer = comparer;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00010252 File Offset: 0x0000E452
		public static void Sort(T[] source, IComparer<T> comparer)
		{
			MergeSort<T>.Sort(source, 0, source.Length, comparer);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0001025F File Offset: 0x0000E45F
		public static void Sort(T[] source, int start, int length, IComparer<T> comparer)
		{
			new MergeSort<T>(source, comparer).Sort(start, length);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0001026F File Offset: 0x0000E46F
		private void Sort(int start, int length)
		{
			this.TopDownSplitMerge(this.buffer, this.elements, start, length);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00010288 File Offset: 0x0000E488
		private void TopDownSplitMerge(T[] a, T[] b, int start, int end)
		{
			if (end - start < 2)
			{
				return;
			}
			int num = (end + start) / 2;
			this.TopDownSplitMerge(b, a, start, num);
			this.TopDownSplitMerge(b, a, num, end);
			this.TopDownMerge(a, b, start, num, end);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000102C8 File Offset: 0x0000E4C8
		private void TopDownMerge(T[] a, T[] b, int start, int middle, int end)
		{
			int num = start;
			int num2 = middle;
			for (int i = start; i < end; i++)
			{
				if (num < middle && (num2 >= end || this.comparer.Compare(a[num], a[num2]) <= 0))
				{
					b[i] = a[num++];
				}
				else
				{
					b[i] = a[num2++];
				}
			}
		}

		// Token: 0x040001E6 RID: 486
		private readonly T[] elements;

		// Token: 0x040001E7 RID: 487
		private readonly T[] buffer;

		// Token: 0x040001E8 RID: 488
		private readonly IComparer<T> comparer;
	}
}
