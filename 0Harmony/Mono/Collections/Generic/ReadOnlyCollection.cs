using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Mono.Collections.Generic
{
	// Token: 0x020000B5 RID: 181
	internal sealed class ReadOnlyCollection<T> : Collection<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00010F14 File Offset: 0x0000F114
		public static ReadOnlyCollection<T> Empty
		{
			get
			{
				if (ReadOnlyCollection<T>.empty != null)
				{
					return ReadOnlyCollection<T>.empty;
				}
				Interlocked.CompareExchange<ReadOnlyCollection<T>>(ref ReadOnlyCollection<T>.empty, new ReadOnlyCollection<T>(), null);
				return ReadOnlyCollection<T>.empty;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00010F39 File Offset: 0x0000F139
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00010F39 File Offset: 0x0000F139
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00010F39 File Offset: 0x0000F139
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00010F3C File Offset: 0x0000F13C
		private ReadOnlyCollection()
		{
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00010F44 File Offset: 0x0000F144
		public ReadOnlyCollection(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException();
			}
			this.Initialize(array, array.Length);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00010F5F File Offset: 0x0000F15F
		public ReadOnlyCollection(Collection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException();
			}
			this.Initialize(collection.items, collection.size);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00010F82 File Offset: 0x0000F182
		private void Initialize(T[] items, int size)
		{
			this.items = new T[size];
			Array.Copy(items, 0, this.items, 0, size);
			this.size = size;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00010FA6 File Offset: 0x0000F1A6
		internal override void Grow(int desired)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00010FA6 File Offset: 0x0000F1A6
		protected override void OnAdd(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00010FA6 File Offset: 0x0000F1A6
		protected override void OnClear()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00010FA6 File Offset: 0x0000F1A6
		protected override void OnInsert(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00010FA6 File Offset: 0x0000F1A6
		protected override void OnRemove(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00010FA6 File Offset: 0x0000F1A6
		protected override void OnSet(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x040001F0 RID: 496
		private static ReadOnlyCollection<T> empty;
	}
}
