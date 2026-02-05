using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x020003C4 RID: 964
	[DebuggerTypeProxy(typeof(System_QueueDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[ComVisible(false)]
	[global::__DynamicallyInvokable]
	[Serializable]
	public class Queue<T> : IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
	{
		// Token: 0x06002447 RID: 9287 RVA: 0x000A9B4B File Offset: 0x000A7D4B
		[global::__DynamicallyInvokable]
		public Queue()
		{
			this._array = Queue<T>._emptyArray;
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x000A9B5E File Offset: 0x000A7D5E
		[global::__DynamicallyInvokable]
		public Queue(int capacity)
		{
			if (capacity < 0)
			{
				global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.capacity, global::System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNumRequired);
			}
			this._array = new T[capacity];
			this._head = 0;
			this._tail = 0;
			this._size = 0;
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x000A9B94 File Offset: 0x000A7D94
		[global::__DynamicallyInvokable]
		public Queue(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.collection);
			}
			this._array = new T[4];
			this._size = 0;
			this._version = 0;
			foreach (T t in collection)
			{
				this.Enqueue(t);
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x0600244A RID: 9290 RVA: 0x000A9C04 File Offset: 0x000A7E04
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._size;
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x0600244B RID: 9291 RVA: 0x000A9C0C File Offset: 0x000A7E0C
		[global::__DynamicallyInvokable]
		bool ICollection.IsSynchronized
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x0600244C RID: 9292 RVA: 0x000A9C0F File Offset: 0x000A7E0F
		[global::__DynamicallyInvokable]
		object ICollection.SyncRoot
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x000A9C34 File Offset: 0x000A7E34
		[global::__DynamicallyInvokable]
		public void Clear()
		{
			if (this._head < this._tail)
			{
				Array.Clear(this._array, this._head, this._size);
			}
			else
			{
				Array.Clear(this._array, this._head, this._array.Length - this._head);
				Array.Clear(this._array, 0, this._tail);
			}
			this._head = 0;
			this._tail = 0;
			this._size = 0;
			this._version++;
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x000A9CC0 File Offset: 0x000A7EC0
		[global::__DynamicallyInvokable]
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.array);
			}
			if (arrayIndex < 0 || arrayIndex > array.Length)
			{
				global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.arrayIndex, global::System.ExceptionResource.ArgumentOutOfRange_Index);
			}
			int num = array.Length;
			if (num - arrayIndex < this._size)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidOffLen);
			}
			int num2 = ((num - arrayIndex < this._size) ? (num - arrayIndex) : this._size);
			if (num2 == 0)
			{
				return;
			}
			int num3 = ((this._array.Length - this._head < num2) ? (this._array.Length - this._head) : num2);
			Array.Copy(this._array, this._head, array, arrayIndex, num3);
			num2 -= num3;
			if (num2 > 0)
			{
				Array.Copy(this._array, 0, array, arrayIndex + this._array.Length - this._head, num2);
			}
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x000A9D7C File Offset: 0x000A7F7C
		[global::__DynamicallyInvokable]
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.array);
			}
			if (array.Rank != 1)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			if (array.GetLowerBound(0) != 0)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Arg_NonZeroLowerBound);
			}
			int length = array.Length;
			if (index < 0 || index > length)
			{
				global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.index, global::System.ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (length - index < this._size)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidOffLen);
			}
			int num = ((length - index < this._size) ? (length - index) : this._size);
			if (num == 0)
			{
				return;
			}
			try
			{
				int num2 = ((this._array.Length - this._head < num) ? (this._array.Length - this._head) : num);
				Array.Copy(this._array, this._head, array, index, num2);
				num -= num2;
				if (num > 0)
				{
					Array.Copy(this._array, 0, array, index + this._array.Length - this._head, num);
				}
			}
			catch (ArrayTypeMismatchException)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x000A9E74 File Offset: 0x000A8074
		[global::__DynamicallyInvokable]
		public void Enqueue(T item)
		{
			if (this._size == this._array.Length)
			{
				int num = (int)((long)this._array.Length * 200L / 100L);
				if (num < this._array.Length + 4)
				{
					num = this._array.Length + 4;
				}
				this.SetCapacity(num);
			}
			this._array[this._tail] = item;
			this._tail = (this._tail + 1) % this._array.Length;
			this._size++;
			this._version++;
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x000A9F0B File Offset: 0x000A810B
		[global::__DynamicallyInvokable]
		public Queue<T>.Enumerator GetEnumerator()
		{
			return new Queue<T>.Enumerator(this);
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x000A9F13 File Offset: 0x000A8113
		[global::__DynamicallyInvokable]
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Queue<T>.Enumerator(this);
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000A9F20 File Offset: 0x000A8120
		[global::__DynamicallyInvokable]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Queue<T>.Enumerator(this);
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000A9F30 File Offset: 0x000A8130
		[global::__DynamicallyInvokable]
		public T Dequeue()
		{
			if (this._size == 0)
			{
				global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EmptyQueue);
			}
			T t = this._array[this._head];
			this._array[this._head] = default(T);
			this._head = (this._head + 1) % this._array.Length;
			this._size--;
			this._version++;
			return t;
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000A9FAC File Offset: 0x000A81AC
		[global::__DynamicallyInvokable]
		public T Peek()
		{
			if (this._size == 0)
			{
				global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EmptyQueue);
			}
			return this._array[this._head];
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x000A9FD0 File Offset: 0x000A81D0
		[global::__DynamicallyInvokable]
		public bool Contains(T item)
		{
			int num = this._head;
			int size = this._size;
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			while (size-- > 0)
			{
				if (item == null)
				{
					if (this._array[num] == null)
					{
						return true;
					}
				}
				else if (this._array[num] != null && @default.Equals(this._array[num], item))
				{
					return true;
				}
				num = (num + 1) % this._array.Length;
			}
			return false;
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000AA050 File Offset: 0x000A8250
		internal T GetElement(int i)
		{
			return this._array[(this._head + i) % this._array.Length];
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000AA070 File Offset: 0x000A8270
		[global::__DynamicallyInvokable]
		public T[] ToArray()
		{
			T[] array = new T[this._size];
			if (this._size == 0)
			{
				return array;
			}
			if (this._head < this._tail)
			{
				Array.Copy(this._array, this._head, array, 0, this._size);
			}
			else
			{
				Array.Copy(this._array, this._head, array, 0, this._array.Length - this._head);
				Array.Copy(this._array, 0, array, this._array.Length - this._head, this._tail);
			}
			return array;
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000AA104 File Offset: 0x000A8304
		private void SetCapacity(int capacity)
		{
			T[] array = new T[capacity];
			if (this._size > 0)
			{
				if (this._head < this._tail)
				{
					Array.Copy(this._array, this._head, array, 0, this._size);
				}
				else
				{
					Array.Copy(this._array, this._head, array, 0, this._array.Length - this._head);
					Array.Copy(this._array, 0, array, this._array.Length - this._head, this._tail);
				}
			}
			this._array = array;
			this._head = 0;
			this._tail = ((this._size == capacity) ? 0 : this._size);
			this._version++;
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000AA1C4 File Offset: 0x000A83C4
		[global::__DynamicallyInvokable]
		public void TrimExcess()
		{
			int num = (int)((double)this._array.Length * 0.9);
			if (this._size < num)
			{
				this.SetCapacity(this._size);
			}
		}

		// Token: 0x04002004 RID: 8196
		private T[] _array;

		// Token: 0x04002005 RID: 8197
		private int _head;

		// Token: 0x04002006 RID: 8198
		private int _tail;

		// Token: 0x04002007 RID: 8199
		private int _size;

		// Token: 0x04002008 RID: 8200
		private int _version;

		// Token: 0x04002009 RID: 8201
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x0400200A RID: 8202
		private const int _MinimumGrow = 4;

		// Token: 0x0400200B RID: 8203
		private const int _ShrinkThreshold = 32;

		// Token: 0x0400200C RID: 8204
		private const int _GrowFactor = 200;

		// Token: 0x0400200D RID: 8205
		private const int _DefaultCapacity = 4;

		// Token: 0x0400200E RID: 8206
		private static T[] _emptyArray = new T[0];

		// Token: 0x020007F3 RID: 2035
		[global::__DynamicallyInvokable]
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x06004428 RID: 17448 RVA: 0x0011E55B File Offset: 0x0011C75B
			internal Enumerator(Queue<T> q)
			{
				this._q = q;
				this._version = this._q._version;
				this._index = -1;
				this._currentElement = default(T);
			}

			// Token: 0x06004429 RID: 17449 RVA: 0x0011E588 File Offset: 0x0011C788
			[global::__DynamicallyInvokable]
			public void Dispose()
			{
				this._index = -2;
				this._currentElement = default(T);
			}

			// Token: 0x0600442A RID: 17450 RVA: 0x0011E5A0 File Offset: 0x0011C7A0
			[global::__DynamicallyInvokable]
			public bool MoveNext()
			{
				if (this._version != this._q._version)
				{
					global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				if (this._index == -2)
				{
					return false;
				}
				this._index++;
				if (this._index == this._q._size)
				{
					this._index = -2;
					this._currentElement = default(T);
					return false;
				}
				this._currentElement = this._q.GetElement(this._index);
				return true;
			}

			// Token: 0x17000F73 RID: 3955
			// (get) Token: 0x0600442B RID: 17451 RVA: 0x0011E622 File Offset: 0x0011C822
			[global::__DynamicallyInvokable]
			public T Current
			{
				[global::__DynamicallyInvokable]
				get
				{
					if (this._index < 0)
					{
						if (this._index == -1)
						{
							global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumNotStarted);
						}
						else
						{
							global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumEnded);
						}
					}
					return this._currentElement;
				}
			}

			// Token: 0x17000F74 RID: 3956
			// (get) Token: 0x0600442C RID: 17452 RVA: 0x0011E64C File Offset: 0x0011C84C
			[global::__DynamicallyInvokable]
			object IEnumerator.Current
			{
				[global::__DynamicallyInvokable]
				get
				{
					if (this._index < 0)
					{
						if (this._index == -1)
						{
							global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumNotStarted);
						}
						else
						{
							global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumEnded);
						}
					}
					return this._currentElement;
				}
			}

			// Token: 0x0600442D RID: 17453 RVA: 0x0011E67B File Offset: 0x0011C87B
			[global::__DynamicallyInvokable]
			void IEnumerator.Reset()
			{
				if (this._version != this._q._version)
				{
					global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this._index = -1;
				this._currentElement = default(T);
			}

			// Token: 0x04003511 RID: 13585
			private Queue<T> _q;

			// Token: 0x04003512 RID: 13586
			private int _index;

			// Token: 0x04003513 RID: 13587
			private int _version;

			// Token: 0x04003514 RID: 13588
			private T _currentElement;
		}
	}
}
