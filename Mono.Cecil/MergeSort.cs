using System;
using System.Collections.Generic;

namespace Mono
{
	// Token: 0x02000007 RID: 7
	internal class MergeSort<T>
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020B4 File Offset: 0x000002B4
		private MergeSort(T[] elements, IComparer<T> comparer)
		{
			this.elements = elements;
			this.buffer = new T[elements.Length];
			Array.Copy(this.elements, this.buffer, elements.Length);
			this.comparer = comparer;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020EC File Offset: 0x000002EC
		public static void Sort(T[] source, IComparer<T> comparer)
		{
			MergeSort<T>.Sort(source, 0, source.Length, comparer);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020F9 File Offset: 0x000002F9
		public static void Sort(T[] source, int start, int length, IComparer<T> comparer)
		{
			new MergeSort<T>(source, comparer).Sort(start, length);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002109 File Offset: 0x00000309
		private void Sort(int start, int length)
		{
			this.TopDownSplitMerge(this.buffer, this.elements, start, length);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002120 File Offset: 0x00000320
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

		// Token: 0x0600000C RID: 12 RVA: 0x00002160 File Offset: 0x00000360
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

		// Token: 0x04000006 RID: 6
		private readonly T[] elements;

		// Token: 0x04000007 RID: 7
		private readonly T[] buffer;

		// Token: 0x04000008 RID: 8
		private readonly IComparer<T> comparer;
	}
}
