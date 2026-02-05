using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;

namespace Mono.Collections.Generic
{
	// Token: 0x020000B3 RID: 179
	internal class Collection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00010898 File Offset: 0x0000EA98
		public int Count
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x17000016 RID: 22
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

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000371 RID: 881 RVA: 0x000108EA File Offset: 0x0000EAEA
		// (set) Token: 0x06000372 RID: 882 RVA: 0x000108F4 File Offset: 0x0000EAF4
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

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00010910 File Offset: 0x0000EB10
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00010910 File Offset: 0x0000EB10
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00010910 File Offset: 0x0000EB10
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001B RID: 27
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

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000378 RID: 888 RVA: 0x00010970 File Offset: 0x0000EB70
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000379 RID: 889 RVA: 0x00010910 File Offset: 0x0000EB10
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00010978 File Offset: 0x0000EB78
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001097B File Offset: 0x0000EB7B
		public Collection()
		{
			this.items = Empty<T>.Array;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0001098E File Offset: 0x0000EB8E
		public Collection(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.items = ((capacity == 0) ? Empty<T>.Array : new T[capacity]);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x000109B8 File Offset: 0x0000EBB8
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

		// Token: 0x0600037E RID: 894 RVA: 0x00010A08 File Offset: 0x0000EC08
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

		// Token: 0x0600037F RID: 895 RVA: 0x00010A64 File Offset: 0x0000EC64
		public bool Contains(T item)
		{
			return this.IndexOf(item) != -1;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00010A73 File Offset: 0x0000EC73
		public int IndexOf(T item)
		{
			return Array.IndexOf<T>(this.items, item, 0, this.size);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00010A88 File Offset: 0x0000EC88
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

		// Token: 0x06000382 RID: 898 RVA: 0x00010AE0 File Offset: 0x0000ECE0
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

		// Token: 0x06000383 RID: 899 RVA: 0x00010B2C File Offset: 0x0000ED2C
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

		// Token: 0x06000384 RID: 900 RVA: 0x00010B66 File Offset: 0x0000ED66
		public void Clear()
		{
			this.OnClear();
			Array.Clear(this.items, 0, this.size);
			this.size = 0;
			this.version++;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00010B95 File Offset: 0x0000ED95
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this.items, 0, array, arrayIndex, this.size);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00010BAC File Offset: 0x0000EDAC
		public T[] ToArray()
		{
			T[] array = new T[this.size];
			Array.Copy(this.items, 0, array, 0, this.size);
			return array;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00010BDA File Offset: 0x0000EDDA
		private void CheckIndex(int index)
		{
			if (index < 0 || index > this.size)
			{
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00010BF0 File Offset: 0x0000EDF0
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

		// Token: 0x06000389 RID: 905 RVA: 0x00010C51 File Offset: 0x0000EE51
		protected virtual void OnAdd(T item, int index)
		{
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00010C51 File Offset: 0x0000EE51
		protected virtual void OnInsert(T item, int index)
		{
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00010C51 File Offset: 0x0000EE51
		protected virtual void OnSet(T item, int index)
		{
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00010C51 File Offset: 0x0000EE51
		protected virtual void OnRemove(T item, int index)
		{
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00010C51 File Offset: 0x0000EE51
		protected virtual void OnClear()
		{
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00010C54 File Offset: 0x0000EE54
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

		// Token: 0x0600038F RID: 911 RVA: 0x00010C94 File Offset: 0x0000EE94
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

		// Token: 0x06000390 RID: 912 RVA: 0x00010CC4 File Offset: 0x0000EEC4
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

		// Token: 0x06000391 RID: 913 RVA: 0x00010D14 File Offset: 0x0000EF14
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00010D1C File Offset: 0x0000EF1C
		bool IList.Contains(object value)
		{
			return ((IList)this).IndexOf(value) > -1;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00010D28 File Offset: 0x0000EF28
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

		// Token: 0x06000394 RID: 916 RVA: 0x00010D6C File Offset: 0x0000EF6C
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

		// Token: 0x06000395 RID: 917 RVA: 0x00010DB8 File Offset: 0x0000EFB8
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

		// Token: 0x06000396 RID: 918 RVA: 0x00010DF8 File Offset: 0x0000EFF8
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00010B95 File Offset: 0x0000ED95
		void ICollection.CopyTo(Array array, int index)
		{
			Array.Copy(this.items, 0, array, index, this.size);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00010E01 File Offset: 0x0000F001
		public Collection<T>.Enumerator GetEnumerator()
		{
			return new Collection<T>.Enumerator(this);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00010E09 File Offset: 0x0000F009
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Collection<T>.Enumerator(this);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00010E09 File Offset: 0x0000F009
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Collection<T>.Enumerator(this);
		}

		// Token: 0x040001E9 RID: 489
		internal T[] items;

		// Token: 0x040001EA RID: 490
		internal int size;

		// Token: 0x040001EB RID: 491
		private int version;

		// Token: 0x020000B4 RID: 180
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x1700001F RID: 31
			// (get) Token: 0x0600039B RID: 923 RVA: 0x00010E16 File Offset: 0x0000F016
			public T Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x0600039C RID: 924 RVA: 0x00010E1E File Offset: 0x0000F01E
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

			// Token: 0x0600039D RID: 925 RVA: 0x00010E40 File Offset: 0x0000F040
			internal Enumerator(Collection<T> collection)
			{
				this = default(Collection<T>.Enumerator);
				this.collection = collection;
				this.version = collection.version;
			}

			// Token: 0x0600039E RID: 926 RVA: 0x00010E5C File Offset: 0x0000F05C
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

			// Token: 0x0600039F RID: 927 RVA: 0x00010EBE File Offset: 0x0000F0BE
			public void Reset()
			{
				this.CheckState();
				this.next = 0;
			}

			// Token: 0x060003A0 RID: 928 RVA: 0x00010ECD File Offset: 0x0000F0CD
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

			// Token: 0x060003A1 RID: 929 RVA: 0x00010F0B File Offset: 0x0000F10B
			public void Dispose()
			{
				this.collection = null;
			}

			// Token: 0x040001EC RID: 492
			private Collection<T> collection;

			// Token: 0x040001ED RID: 493
			private T current;

			// Token: 0x040001EE RID: 494
			private int next;

			// Token: 0x040001EF RID: 495
			private readonly int version;
		}
	}
}
