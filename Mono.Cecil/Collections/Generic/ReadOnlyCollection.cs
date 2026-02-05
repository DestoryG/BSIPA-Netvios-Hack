using System;
using System.Collections;
using System.Collections.Generic;

namespace Mono.Collections.Generic
{
	// Token: 0x0200000B RID: 11
	public sealed class ReadOnlyCollection<T> : Collection<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002BD2 File Offset: 0x00000DD2
		public static ReadOnlyCollection<T> Empty
		{
			get
			{
				ReadOnlyCollection<T> readOnlyCollection;
				if ((readOnlyCollection = ReadOnlyCollection<T>.empty) == null)
				{
					readOnlyCollection = (ReadOnlyCollection<T>.empty = new ReadOnlyCollection<T>());
				}
				return readOnlyCollection;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002BE8 File Offset: 0x00000DE8
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002BE8 File Offset: 0x00000DE8
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002BE8 File Offset: 0x00000DE8
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002BEB File Offset: 0x00000DEB
		private ReadOnlyCollection()
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002BF3 File Offset: 0x00000DF3
		public ReadOnlyCollection(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException();
			}
			this.Initialize(array, array.Length);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002C0E File Offset: 0x00000E0E
		public ReadOnlyCollection(Collection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException();
			}
			this.Initialize(collection.items, collection.size);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002C31 File Offset: 0x00000E31
		private void Initialize(T[] items, int size)
		{
			this.items = new T[size];
			Array.Copy(items, 0, this.items, 0, size);
			this.size = size;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002C55 File Offset: 0x00000E55
		internal override void Grow(int desired)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002C55 File Offset: 0x00000E55
		protected override void OnAdd(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002C55 File Offset: 0x00000E55
		protected override void OnClear()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002C55 File Offset: 0x00000E55
		protected override void OnInsert(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002C55 File Offset: 0x00000E55
		protected override void OnRemove(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002C55 File Offset: 0x00000E55
		protected override void OnSet(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0400000C RID: 12
		private static ReadOnlyCollection<T> empty;
	}
}
