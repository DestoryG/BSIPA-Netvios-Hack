using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x020003C6 RID: 966
	[DebuggerTypeProxy(typeof(System_StackDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[ComVisible(false)]
	[global::__DynamicallyInvokable]
	[Serializable]
	public class Stack<T> : IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
	{
		// Token: 0x06002495 RID: 9365 RVA: 0x000AAC4F File Offset: 0x000A8E4F
		[global::__DynamicallyInvokable]
		public Stack()
		{
			this._array = Stack<T>._emptyArray;
			this._size = 0;
			this._version = 0;
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x000AAC70 File Offset: 0x000A8E70
		[global::__DynamicallyInvokable]
		public Stack(int capacity)
		{
			if (capacity < 0)
			{
				global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.capacity, global::System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNumRequired);
			}
			this._array = new T[capacity];
			this._size = 0;
			this._version = 0;
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x000AACA0 File Offset: 0x000A8EA0
		[global::__DynamicallyInvokable]
		public Stack(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.collection);
			}
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 != null)
			{
				int count = collection2.Count;
				this._array = new T[count];
				collection2.CopyTo(this._array, 0);
				this._size = count;
				return;
			}
			this._size = 0;
			this._array = new T[4];
			foreach (T t in collection)
			{
				this.Push(t);
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06002498 RID: 9368 RVA: 0x000AAD3C File Offset: 0x000A8F3C
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._size;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06002499 RID: 9369 RVA: 0x000AAD44 File Offset: 0x000A8F44
		[global::__DynamicallyInvokable]
		bool ICollection.IsSynchronized
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x0600249A RID: 9370 RVA: 0x000AAD47 File Offset: 0x000A8F47
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

		// Token: 0x0600249B RID: 9371 RVA: 0x000AAD69 File Offset: 0x000A8F69
		[global::__DynamicallyInvokable]
		public void Clear()
		{
			Array.Clear(this._array, 0, this._size);
			this._size = 0;
			this._version++;
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x000AAD94 File Offset: 0x000A8F94
		[global::__DynamicallyInvokable]
		public bool Contains(T item)
		{
			int size = this._size;
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			while (size-- > 0)
			{
				if (item == null)
				{
					if (this._array[size] == null)
					{
						return true;
					}
				}
				else if (this._array[size] != null && @default.Equals(this._array[size], item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x000AAE00 File Offset: 0x000A9000
		[global::__DynamicallyInvokable]
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.array);
			}
			if (arrayIndex < 0 || arrayIndex > array.Length)
			{
				global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.arrayIndex, global::System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - arrayIndex < this._size)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidOffLen);
			}
			Array.Copy(this._array, 0, array, arrayIndex, this._size);
			Array.Reverse(array, arrayIndex, this._size);
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x000AAE60 File Offset: 0x000A9060
		[global::__DynamicallyInvokable]
		void ICollection.CopyTo(Array array, int arrayIndex)
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
			if (arrayIndex < 0 || arrayIndex > array.Length)
			{
				global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.arrayIndex, global::System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - arrayIndex < this._size)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidOffLen);
			}
			try
			{
				Array.Copy(this._array, 0, array, arrayIndex, this._size);
				Array.Reverse(array, arrayIndex, this._size);
			}
			catch (ArrayTypeMismatchException)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x000AAF00 File Offset: 0x000A9100
		[global::__DynamicallyInvokable]
		public Stack<T>.Enumerator GetEnumerator()
		{
			return new Stack<T>.Enumerator(this);
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x000AAF08 File Offset: 0x000A9108
		[global::__DynamicallyInvokable]
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Stack<T>.Enumerator(this);
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x000AAF15 File Offset: 0x000A9115
		[global::__DynamicallyInvokable]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Stack<T>.Enumerator(this);
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x000AAF24 File Offset: 0x000A9124
		[global::__DynamicallyInvokable]
		public void TrimExcess()
		{
			int num = (int)((double)this._array.Length * 0.9);
			if (this._size < num)
			{
				T[] array = new T[this._size];
				Array.Copy(this._array, 0, array, 0, this._size);
				this._array = array;
				this._version++;
			}
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000AAF84 File Offset: 0x000A9184
		[global::__DynamicallyInvokable]
		public T Peek()
		{
			if (this._size == 0)
			{
				global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EmptyStack);
			}
			return this._array[this._size - 1];
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000AAFA8 File Offset: 0x000A91A8
		[global::__DynamicallyInvokable]
		public T Pop()
		{
			if (this._size == 0)
			{
				global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EmptyStack);
			}
			this._version++;
			T[] array = this._array;
			int num = this._size - 1;
			this._size = num;
			T t = array[num];
			this._array[this._size] = default(T);
			return t;
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000AB00C File Offset: 0x000A920C
		[global::__DynamicallyInvokable]
		public void Push(T item)
		{
			if (this._size == this._array.Length)
			{
				T[] array = new T[(this._array.Length == 0) ? 4 : (2 * this._array.Length)];
				Array.Copy(this._array, 0, array, 0, this._size);
				this._array = array;
			}
			T[] array2 = this._array;
			int size = this._size;
			this._size = size + 1;
			array2[size] = item;
			this._version++;
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x000AB08C File Offset: 0x000A928C
		[global::__DynamicallyInvokable]
		public T[] ToArray()
		{
			T[] array = new T[this._size];
			for (int i = 0; i < this._size; i++)
			{
				array[i] = this._array[this._size - i - 1];
			}
			return array;
		}

		// Token: 0x0400201B RID: 8219
		private T[] _array;

		// Token: 0x0400201C RID: 8220
		private int _size;

		// Token: 0x0400201D RID: 8221
		private int _version;

		// Token: 0x0400201E RID: 8222
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x0400201F RID: 8223
		private const int _defaultCapacity = 4;

		// Token: 0x04002020 RID: 8224
		private static T[] _emptyArray = new T[0];

		// Token: 0x020007F9 RID: 2041
		[global::__DynamicallyInvokable]
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x06004467 RID: 17511 RVA: 0x0011EE0C File Offset: 0x0011D00C
			internal Enumerator(Stack<T> stack)
			{
				this._stack = stack;
				this._version = this._stack._version;
				this._index = -2;
				this.currentElement = default(T);
			}

			// Token: 0x06004468 RID: 17512 RVA: 0x0011EE3A File Offset: 0x0011D03A
			[global::__DynamicallyInvokable]
			public void Dispose()
			{
				this._index = -1;
			}

			// Token: 0x06004469 RID: 17513 RVA: 0x0011EE44 File Offset: 0x0011D044
			[global::__DynamicallyInvokable]
			public bool MoveNext()
			{
				if (this._version != this._stack._version)
				{
					global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				bool flag;
				if (this._index == -2)
				{
					this._index = this._stack._size - 1;
					flag = this._index >= 0;
					if (flag)
					{
						this.currentElement = this._stack._array[this._index];
					}
					return flag;
				}
				if (this._index == -1)
				{
					return false;
				}
				int num = this._index - 1;
				this._index = num;
				flag = num >= 0;
				if (flag)
				{
					this.currentElement = this._stack._array[this._index];
				}
				else
				{
					this.currentElement = default(T);
				}
				return flag;
			}

			// Token: 0x17000F88 RID: 3976
			// (get) Token: 0x0600446A RID: 17514 RVA: 0x0011EF07 File Offset: 0x0011D107
			[global::__DynamicallyInvokable]
			public T Current
			{
				[global::__DynamicallyInvokable]
				get
				{
					if (this._index == -2)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumNotStarted);
					}
					if (this._index == -1)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumEnded);
					}
					return this.currentElement;
				}
			}

			// Token: 0x17000F89 RID: 3977
			// (get) Token: 0x0600446B RID: 17515 RVA: 0x0011EF30 File Offset: 0x0011D130
			[global::__DynamicallyInvokable]
			object IEnumerator.Current
			{
				[global::__DynamicallyInvokable]
				get
				{
					if (this._index == -2)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumNotStarted);
					}
					if (this._index == -1)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumEnded);
					}
					return this.currentElement;
				}
			}

			// Token: 0x0600446C RID: 17516 RVA: 0x0011EF5E File Offset: 0x0011D15E
			[global::__DynamicallyInvokable]
			void IEnumerator.Reset()
			{
				if (this._version != this._stack._version)
				{
					global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this._index = -2;
				this.currentElement = default(T);
			}

			// Token: 0x04003527 RID: 13607
			private Stack<T> _stack;

			// Token: 0x04003528 RID: 13608
			private int _index;

			// Token: 0x04003529 RID: 13609
			private int _version;

			// Token: 0x0400352A RID: 13610
			private T currentElement;
		}
	}
}
