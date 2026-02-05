using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;

namespace Mono.Collections.Generic
{
	// Token: 0x0200000A RID: 10
	public class Collection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002663 File Offset: 0x00000863
		public int Count
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x17000002 RID: 2
		public T this[int index]
		{
			get
			{
				if (index >= this.size)
				{
					throw new ArgumentOutOfRangeException();
				}
				return this.items[index];
			}
			set
			{
				this.CheckIndex(index);
				if (index == this.size)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.OnSet(value, index);
				this.items[index] = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000026B5 File Offset: 0x000008B5
		// (set) Token: 0x0600001F RID: 31 RVA: 0x000026BF File Offset: 0x000008BF
		public int Capacity
		{
			get
			{
				return this.items.Length;
			}
			set
			{
				if (value < 0 || value < this.size)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.Resize(value);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000026DB File Offset: 0x000008DB
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000026DB File Offset: 0x000008DB
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000026DB File Offset: 0x000008DB
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000007 RID: 7
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this.CheckIndex(index);
				try
				{
					this[index] = (T)((object)value);
					return;
				}
				catch (InvalidCastException)
				{
				}
				catch (NullReferenceException)
				{
				}
				throw new ArgumentException();
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002738 File Offset: 0x00000938
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000026DB File Offset: 0x000008DB
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002740 File Offset: 0x00000940
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002743 File Offset: 0x00000943
		public Collection()
		{
			this.items = Empty<T>.Array;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002756 File Offset: 0x00000956
		public Collection(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.items = new T[capacity];
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002774 File Offset: 0x00000974
		public Collection(ICollection<T> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			this.items = new T[items.Count];
			items.CopyTo(this.items, 0);
			this.size = this.items.Length;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000027C4 File Offset: 0x000009C4
		public void Add(T item)
		{
			if (this.size == this.items.Length)
			{
				this.Grow(1);
			}
			this.OnAdd(item, this.size);
			T[] array = this.items;
			int num = this.size;
			this.size = num + 1;
			array[num] = item;
			this.version++;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002820 File Offset: 0x00000A20
		public bool Contains(T item)
		{
			return this.IndexOf(item) != -1;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000282F File Offset: 0x00000A2F
		public int IndexOf(T item)
		{
			return Array.IndexOf<T>(this.items, item, 0, this.size);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002844 File Offset: 0x00000A44
		public void Insert(int index, T item)
		{
			this.CheckIndex(index);
			if (this.size == this.items.Length)
			{
				this.Grow(1);
			}
			this.OnInsert(item, index);
			this.Shift(index, 1);
			this.items[index] = item;
			this.version++;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000289C File Offset: 0x00000A9C
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.size)
			{
				throw new ArgumentOutOfRangeException();
			}
			T t = this.items[index];
			this.OnRemove(t, index);
			this.Shift(index, -1);
			this.version++;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000028E8 File Offset: 0x00000AE8
		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num == -1)
			{
				return false;
			}
			this.OnRemove(item, num);
			this.Shift(num, -1);
			this.version++;
			return true;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002922 File Offset: 0x00000B22
		public void Clear()
		{
			this.OnClear();
			Array.Clear(this.items, 0, this.size);
			this.size = 0;
			this.version++;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002951 File Offset: 0x00000B51
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this.items, 0, array, arrayIndex, this.size);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002968 File Offset: 0x00000B68
		public T[] ToArray()
		{
			T[] array = new T[this.size];
			Array.Copy(this.items, 0, array, 0, this.size);
			return array;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002996 File Offset: 0x00000B96
		private void CheckIndex(int index)
		{
			if (index < 0 || index > this.size)
			{
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000029AC File Offset: 0x00000BAC
		private void Shift(int start, int delta)
		{
			if (delta < 0)
			{
				start -= delta;
			}
			if (start < this.size)
			{
				Array.Copy(this.items, start, this.items, start + delta, this.size - start);
			}
			this.size += delta;
			if (delta < 0)
			{
				Array.Clear(this.items, this.size, -delta);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A0D File Offset: 0x00000C0D
		protected virtual void OnAdd(T item, int index)
		{
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002A0D File Offset: 0x00000C0D
		protected virtual void OnInsert(T item, int index)
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002A0D File Offset: 0x00000C0D
		protected virtual void OnSet(T item, int index)
		{
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002A0D File Offset: 0x00000C0D
		protected virtual void OnRemove(T item, int index)
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002A0D File Offset: 0x00000C0D
		protected virtual void OnClear()
		{
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002A10 File Offset: 0x00000C10
		internal virtual void Grow(int desired)
		{
			int num = this.size + desired;
			if (num <= this.items.Length)
			{
				return;
			}
			num = Math.Max(Math.Max(this.items.Length * 2, 4), num);
			this.Resize(num);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002A50 File Offset: 0x00000C50
		protected void Resize(int new_size)
		{
			if (new_size == this.size)
			{
				return;
			}
			if (new_size < this.size)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.items = this.items.Resize(new_size);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002A80 File Offset: 0x00000C80
		int IList.Add(object value)
		{
			try
			{
				this.Add((T)((object)value));
				return this.size - 1;
			}
			catch (InvalidCastException)
			{
			}
			catch (NullReferenceException)
			{
			}
			throw new ArgumentException();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002AD0 File Offset: 0x00000CD0
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002AD8 File Offset: 0x00000CD8
		bool IList.Contains(object value)
		{
			return ((IList)this).IndexOf(value) > -1;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002AE4 File Offset: 0x00000CE4
		int IList.IndexOf(object value)
		{
			try
			{
				return this.IndexOf((T)((object)value));
			}
			catch (InvalidCastException)
			{
			}
			catch (NullReferenceException)
			{
			}
			return -1;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002B28 File Offset: 0x00000D28
		void IList.Insert(int index, object value)
		{
			this.CheckIndex(index);
			try
			{
				this.Insert(index, (T)((object)value));
				return;
			}
			catch (InvalidCastException)
			{
			}
			catch (NullReferenceException)
			{
			}
			throw new ArgumentException();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002B74 File Offset: 0x00000D74
		void IList.Remove(object value)
		{
			try
			{
				this.Remove((T)((object)value));
			}
			catch (InvalidCastException)
			{
			}
			catch (NullReferenceException)
			{
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002BB4 File Offset: 0x00000DB4
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002951 File Offset: 0x00000B51
		void ICollection.CopyTo(Array array, int index)
		{
			Array.Copy(this.items, 0, array, index, this.size);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002BBD File Offset: 0x00000DBD
		public Collection<T>.Enumerator GetEnumerator()
		{
			return new Collection<T>.Enumerator(this);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002BC5 File Offset: 0x00000DC5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Collection<T>.Enumerator(this);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002BC5 File Offset: 0x00000DC5
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Collection<T>.Enumerator(this);
		}

		// Token: 0x04000009 RID: 9
		internal T[] items;

		// Token: 0x0400000A RID: 10
		internal int size;

		// Token: 0x0400000B RID: 11
		private int version;

		// Token: 0x02000138 RID: 312
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x1700032B RID: 811
			// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0002499C File Offset: 0x00022B9C
			public T Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x1700032C RID: 812
			// (get) Token: 0x06000B6C RID: 2924 RVA: 0x000249A4 File Offset: 0x00022BA4
			object IEnumerator.Current
			{
				get
				{
					this.CheckState();
					if (this.next <= 0)
					{
						throw new InvalidOperationException();
					}
					return this.current;
				}
			}

			// Token: 0x06000B6D RID: 2925 RVA: 0x000249C6 File Offset: 0x00022BC6
			internal Enumerator(Collection<T> collection)
			{
				this = default(Collection<T>.Enumerator);
				this.collection = collection;
				this.version = collection.version;
			}

			// Token: 0x06000B6E RID: 2926 RVA: 0x000249E4 File Offset: 0x00022BE4
			public bool MoveNext()
			{
				this.CheckState();
				if (this.next < 0)
				{
					return false;
				}
				if (this.next < this.collection.size)
				{
					T[] items = this.collection.items;
					int num = this.next;
					this.next = num + 1;
					this.current = items[num];
					return true;
				}
				this.next = -1;
				return false;
			}

			// Token: 0x06000B6F RID: 2927 RVA: 0x00024A46 File Offset: 0x00022C46
			public void Reset()
			{
				this.CheckState();
				this.next = 0;
			}

			// Token: 0x06000B70 RID: 2928 RVA: 0x00024A55 File Offset: 0x00022C55
			private void CheckState()
			{
				if (this.collection == null)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.version != this.collection.version)
				{
					throw new InvalidOperationException();
				}
			}

			// Token: 0x06000B71 RID: 2929 RVA: 0x00024A93 File Offset: 0x00022C93
			public void Dispose()
			{
				this.collection = null;
			}

			// Token: 0x040006EF RID: 1775
			private Collection<T> collection;

			// Token: 0x040006F0 RID: 1776
			private T current;

			// Token: 0x040006F1 RID: 1777
			private int next;

			// Token: 0x040006F2 RID: 1778
			private readonly int version;
		}
	}
}
