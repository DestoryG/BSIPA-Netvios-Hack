using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x020003C5 RID: 965
	[DebuggerTypeProxy(typeof(System_DictionaryDebugView<, >))]
	[DebuggerDisplay("Count = {Count}")]
	[ComVisible(false)]
	[global::__DynamicallyInvokable]
	[Serializable]
	public class SortedList<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
	{
		// Token: 0x0600245C RID: 9308 RVA: 0x000AA208 File Offset: 0x000A8408
		[global::__DynamicallyInvokable]
		public SortedList()
		{
			this.keys = SortedList<TKey, TValue>.emptyKeys;
			this.values = SortedList<TKey, TValue>.emptyValues;
			this._size = 0;
			this.comparer = Comparer<TKey>.Default;
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x000AA238 File Offset: 0x000A8438
		[global::__DynamicallyInvokable]
		public SortedList(int capacity)
		{
			if (capacity < 0)
			{
				global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.capacity, global::System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNumRequired);
			}
			this.keys = new TKey[capacity];
			this.values = new TValue[capacity];
			this.comparer = Comparer<TKey>.Default;
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x000AA26F File Offset: 0x000A846F
		[global::__DynamicallyInvokable]
		public SortedList(IComparer<TKey> comparer)
			: this()
		{
			if (comparer != null)
			{
				this.comparer = comparer;
			}
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x000AA281 File Offset: 0x000A8481
		[global::__DynamicallyInvokable]
		public SortedList(int capacity, IComparer<TKey> comparer)
			: this(comparer)
		{
			this.Capacity = capacity;
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000AA291 File Offset: 0x000A8491
		[global::__DynamicallyInvokable]
		public SortedList(IDictionary<TKey, TValue> dictionary)
			: this(dictionary, null)
		{
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000AA29C File Offset: 0x000A849C
		[global::__DynamicallyInvokable]
		public SortedList(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
			: this((dictionary != null) ? dictionary.Count : 0, comparer)
		{
			if (dictionary == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.dictionary);
			}
			dictionary.Keys.CopyTo(this.keys, 0);
			dictionary.Values.CopyTo(this.values, 0);
			Array.Sort<TKey, TValue>(this.keys, this.values, comparer);
			this._size = dictionary.Count;
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x000AA308 File Offset: 0x000A8508
		[global::__DynamicallyInvokable]
		public void Add(TKey key, TValue value)
		{
			if (key == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
			}
			int num = Array.BinarySearch<TKey>(this.keys, 0, this._size, key, this.comparer);
			if (num >= 0)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_AddingDuplicate);
			}
			this.Insert(~num, key, value);
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x000AA351 File Offset: 0x000A8551
		[global::__DynamicallyInvokable]
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			this.Add(keyValuePair.Key, keyValuePair.Value);
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x000AA368 File Offset: 0x000A8568
		[global::__DynamicallyInvokable]
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = this.IndexOfKey(keyValuePair.Key);
			return num >= 0 && EqualityComparer<TValue>.Default.Equals(this.values[num], keyValuePair.Value);
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x000AA3AC File Offset: 0x000A85AC
		[global::__DynamicallyInvokable]
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = this.IndexOfKey(keyValuePair.Key);
			if (num >= 0 && EqualityComparer<TValue>.Default.Equals(this.values[num], keyValuePair.Value))
			{
				this.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x000AA3F4 File Offset: 0x000A85F4
		// (set) Token: 0x06002467 RID: 9319 RVA: 0x000AA400 File Offset: 0x000A8600
		[global::__DynamicallyInvokable]
		public int Capacity
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.keys.Length;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value != this.keys.Length)
				{
					if (value < this._size)
					{
						global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.value, global::System.ExceptionResource.ArgumentOutOfRange_SmallCapacity);
					}
					if (value > 0)
					{
						TKey[] array = new TKey[value];
						TValue[] array2 = new TValue[value];
						if (this._size > 0)
						{
							Array.Copy(this.keys, 0, array, 0, this._size);
							Array.Copy(this.values, 0, array2, 0, this._size);
						}
						this.keys = array;
						this.values = array2;
						return;
					}
					this.keys = SortedList<TKey, TValue>.emptyKeys;
					this.values = SortedList<TKey, TValue>.emptyValues;
				}
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06002468 RID: 9320 RVA: 0x000AA492 File Offset: 0x000A8692
		[global::__DynamicallyInvokable]
		public IComparer<TKey> Comparer
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.comparer;
			}
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000AA49C File Offset: 0x000A869C
		[global::__DynamicallyInvokable]
		void IDictionary.Add(object key, object value)
		{
			if (key == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
			}
			global::System.ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, global::System.ExceptionArgument.value);
			try
			{
				TKey tkey = (TKey)((object)key);
				try
				{
					this.Add(tkey, (TValue)((object)value));
				}
				catch (InvalidCastException)
				{
					global::System.ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
				}
			}
			catch (InvalidCastException)
			{
				global::System.ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x000AA514 File Offset: 0x000A8714
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._size;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x0600246B RID: 9323 RVA: 0x000AA51C File Offset: 0x000A871C
		[global::__DynamicallyInvokable]
		public IList<TKey> Keys
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.GetKeyListHelper();
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x000AA524 File Offset: 0x000A8724
		[global::__DynamicallyInvokable]
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.GetKeyListHelper();
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x0600246D RID: 9325 RVA: 0x000AA52C File Offset: 0x000A872C
		[global::__DynamicallyInvokable]
		ICollection IDictionary.Keys
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.GetKeyListHelper();
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x000AA534 File Offset: 0x000A8734
		[global::__DynamicallyInvokable]
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.GetKeyListHelper();
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x0600246F RID: 9327 RVA: 0x000AA53C File Offset: 0x000A873C
		[global::__DynamicallyInvokable]
		public IList<TValue> Values
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.GetValueListHelper();
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x000AA544 File Offset: 0x000A8744
		[global::__DynamicallyInvokable]
		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.GetValueListHelper();
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x000AA54C File Offset: 0x000A874C
		[global::__DynamicallyInvokable]
		ICollection IDictionary.Values
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.GetValueListHelper();
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06002472 RID: 9330 RVA: 0x000AA554 File Offset: 0x000A8754
		[global::__DynamicallyInvokable]
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.GetValueListHelper();
			}
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x000AA55C File Offset: 0x000A875C
		private SortedList<TKey, TValue>.KeyList GetKeyListHelper()
		{
			if (this.keyList == null)
			{
				this.keyList = new SortedList<TKey, TValue>.KeyList(this);
			}
			return this.keyList;
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x000AA578 File Offset: 0x000A8778
		private SortedList<TKey, TValue>.ValueList GetValueListHelper()
		{
			if (this.valueList == null)
			{
				this.valueList = new SortedList<TKey, TValue>.ValueList(this);
			}
			return this.valueList;
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06002475 RID: 9333 RVA: 0x000AA594 File Offset: 0x000A8794
		[global::__DynamicallyInvokable]
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06002476 RID: 9334 RVA: 0x000AA597 File Offset: 0x000A8797
		[global::__DynamicallyInvokable]
		bool IDictionary.IsReadOnly
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06002477 RID: 9335 RVA: 0x000AA59A File Offset: 0x000A879A
		[global::__DynamicallyInvokable]
		bool IDictionary.IsFixedSize
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06002478 RID: 9336 RVA: 0x000AA59D File Offset: 0x000A879D
		[global::__DynamicallyInvokable]
		bool ICollection.IsSynchronized
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06002479 RID: 9337 RVA: 0x000AA5A0 File Offset: 0x000A87A0
		[global::__DynamicallyInvokable]
		object ICollection.SyncRoot
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x000AA5C2 File Offset: 0x000A87C2
		[global::__DynamicallyInvokable]
		public void Clear()
		{
			this.version++;
			Array.Clear(this.keys, 0, this._size);
			Array.Clear(this.values, 0, this._size);
			this._size = 0;
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x000AA5FD File Offset: 0x000A87FD
		[global::__DynamicallyInvokable]
		bool IDictionary.Contains(object key)
		{
			return SortedList<TKey, TValue>.IsCompatibleKey(key) && this.ContainsKey((TKey)((object)key));
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x000AA615 File Offset: 0x000A8815
		[global::__DynamicallyInvokable]
		public bool ContainsKey(TKey key)
		{
			return this.IndexOfKey(key) >= 0;
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000AA624 File Offset: 0x000A8824
		[global::__DynamicallyInvokable]
		public bool ContainsValue(TValue value)
		{
			return this.IndexOfValue(value) >= 0;
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000AA634 File Offset: 0x000A8834
		[global::__DynamicallyInvokable]
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (array == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.array);
			}
			if (arrayIndex < 0 || arrayIndex > array.Length)
			{
				global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.arrayIndex, global::System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - arrayIndex < this.Count)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			for (int i = 0; i < this.Count; i++)
			{
				KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>(this.keys[i], this.values[i]);
				array[arrayIndex + i] = keyValuePair;
			}
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x000AA6AC File Offset: 0x000A88AC
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
			if (array.Length - arrayIndex < this.Count)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
			if (array2 != null)
			{
				for (int i = 0; i < this.Count; i++)
				{
					array2[i + arrayIndex] = new KeyValuePair<TKey, TValue>(this.keys[i], this.values[i]);
				}
				return;
			}
			object[] array3 = array as object[];
			if (array3 == null)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidArrayType);
			}
			try
			{
				for (int j = 0; j < this.Count; j++)
				{
					array3[j + arrayIndex] = new KeyValuePair<TKey, TValue>(this.keys[j], this.values[j]);
				}
			}
			catch (ArrayTypeMismatchException)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x000AA7B4 File Offset: 0x000A89B4
		private void EnsureCapacity(int min)
		{
			int num = ((this.keys.Length == 0) ? 4 : (this.keys.Length * 2));
			if (num > 2146435071)
			{
				num = 2146435071;
			}
			if (num < min)
			{
				num = min;
			}
			this.Capacity = num;
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000AA7F3 File Offset: 0x000A89F3
		private TValue GetByIndex(int index)
		{
			if (index < 0 || index >= this._size)
			{
				global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.index, global::System.ExceptionResource.ArgumentOutOfRange_Index);
			}
			return this.values[index];
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000AA817 File Offset: 0x000A8A17
		[global::__DynamicallyInvokable]
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return new SortedList<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000AA825 File Offset: 0x000A8A25
		[global::__DynamicallyInvokable]
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return new SortedList<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x000AA833 File Offset: 0x000A8A33
		[global::__DynamicallyInvokable]
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new SortedList<TKey, TValue>.Enumerator(this, 2);
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x000AA841 File Offset: 0x000A8A41
		[global::__DynamicallyInvokable]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SortedList<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000AA84F File Offset: 0x000A8A4F
		private TKey GetKey(int index)
		{
			if (index < 0 || index >= this._size)
			{
				global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.index, global::System.ExceptionResource.ArgumentOutOfRange_Index);
			}
			return this.keys[index];
		}

		// Token: 0x1700093B RID: 2363
		[global::__DynamicallyInvokable]
		public TValue this[TKey key]
		{
			[global::__DynamicallyInvokable]
			get
			{
				int num = this.IndexOfKey(key);
				if (num >= 0)
				{
					return this.values[num];
				}
				global::System.ThrowHelper.ThrowKeyNotFoundException();
				return default(TValue);
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (key == null)
				{
					global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
				}
				int num = Array.BinarySearch<TKey>(this.keys, 0, this._size, key, this.comparer);
				if (num >= 0)
				{
					this.values[num] = value;
					this.version++;
					return;
				}
				this.Insert(~num, key, value);
			}
		}

		// Token: 0x1700093C RID: 2364
		[global::__DynamicallyInvokable]
		object IDictionary.this[object key]
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (SortedList<TKey, TValue>.IsCompatibleKey(key))
				{
					int num = this.IndexOfKey((TKey)((object)key));
					if (num >= 0)
					{
						return this.values[num];
					}
				}
				return null;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (!SortedList<TKey, TValue>.IsCompatibleKey(key))
				{
					global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
				}
				global::System.ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, global::System.ExceptionArgument.value);
				try
				{
					TKey tkey = (TKey)((object)key);
					try
					{
						this[tkey] = (TValue)((object)value);
					}
					catch (InvalidCastException)
					{
						global::System.ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
					}
				}
				catch (InvalidCastException)
				{
					global::System.ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
				}
			}
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x000AA9C4 File Offset: 0x000A8BC4
		[global::__DynamicallyInvokable]
		public int IndexOfKey(TKey key)
		{
			if (key == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
			}
			int num = Array.BinarySearch<TKey>(this.keys, 0, this._size, key, this.comparer);
			if (num < 0)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000AAA00 File Offset: 0x000A8C00
		[global::__DynamicallyInvokable]
		public int IndexOfValue(TValue value)
		{
			return Array.IndexOf<TValue>(this.values, value, 0, this._size);
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x000AAA18 File Offset: 0x000A8C18
		private void Insert(int index, TKey key, TValue value)
		{
			if (this._size == this.keys.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			if (index < this._size)
			{
				Array.Copy(this.keys, index, this.keys, index + 1, this._size - index);
				Array.Copy(this.values, index, this.values, index + 1, this._size - index);
			}
			this.keys[index] = key;
			this.values[index] = value;
			this._size++;
			this.version++;
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x000AAABC File Offset: 0x000A8CBC
		[global::__DynamicallyInvokable]
		public bool TryGetValue(TKey key, out TValue value)
		{
			int num = this.IndexOfKey(key);
			if (num >= 0)
			{
				value = this.values[num];
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x000AAAF4 File Offset: 0x000A8CF4
		[global::__DynamicallyInvokable]
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this._size)
			{
				global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.index, global::System.ExceptionResource.ArgumentOutOfRange_Index);
			}
			this._size--;
			if (index < this._size)
			{
				Array.Copy(this.keys, index + 1, this.keys, index, this._size - index);
				Array.Copy(this.values, index + 1, this.values, index, this._size - index);
			}
			this.keys[this._size] = default(TKey);
			this.values[this._size] = default(TValue);
			this.version++;
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x000AABAC File Offset: 0x000A8DAC
		[global::__DynamicallyInvokable]
		public bool Remove(TKey key)
		{
			int num = this.IndexOfKey(key);
			if (num >= 0)
			{
				this.RemoveAt(num);
			}
			return num >= 0;
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x000AABD3 File Offset: 0x000A8DD3
		[global::__DynamicallyInvokable]
		void IDictionary.Remove(object key)
		{
			if (SortedList<TKey, TValue>.IsCompatibleKey(key))
			{
				this.Remove((TKey)((object)key));
			}
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000AABEC File Offset: 0x000A8DEC
		[global::__DynamicallyInvokable]
		public void TrimExcess()
		{
			int num = (int)((double)this.keys.Length * 0.9);
			if (this._size < num)
			{
				this.Capacity = this._size;
			}
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000AAC23 File Offset: 0x000A8E23
		private static bool IsCompatibleKey(object key)
		{
			if (key == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
			}
			return key is TKey;
		}

		// Token: 0x0400200F RID: 8207
		private TKey[] keys;

		// Token: 0x04002010 RID: 8208
		private TValue[] values;

		// Token: 0x04002011 RID: 8209
		private int _size;

		// Token: 0x04002012 RID: 8210
		private int version;

		// Token: 0x04002013 RID: 8211
		private IComparer<TKey> comparer;

		// Token: 0x04002014 RID: 8212
		private SortedList<TKey, TValue>.KeyList keyList;

		// Token: 0x04002015 RID: 8213
		private SortedList<TKey, TValue>.ValueList valueList;

		// Token: 0x04002016 RID: 8214
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x04002017 RID: 8215
		private static TKey[] emptyKeys = new TKey[0];

		// Token: 0x04002018 RID: 8216
		private static TValue[] emptyValues = new TValue[0];

		// Token: 0x04002019 RID: 8217
		private const int _defaultCapacity = 4;

		// Token: 0x0400201A RID: 8218
		private const int MaxArrayLength = 2146435071;

		// Token: 0x020007F4 RID: 2036
		[Serializable]
		private struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IEnumerator, IDictionaryEnumerator
		{
			// Token: 0x0600442E RID: 17454 RVA: 0x0011E6AA File Offset: 0x0011C8AA
			internal Enumerator(SortedList<TKey, TValue> sortedList, int getEnumeratorRetType)
			{
				this._sortedList = sortedList;
				this.index = 0;
				this.version = this._sortedList.version;
				this.getEnumeratorRetType = getEnumeratorRetType;
				this.key = default(TKey);
				this.value = default(TValue);
			}

			// Token: 0x0600442F RID: 17455 RVA: 0x0011E6EA File Offset: 0x0011C8EA
			public void Dispose()
			{
				this.index = 0;
				this.key = default(TKey);
				this.value = default(TValue);
			}

			// Token: 0x17000F75 RID: 3957
			// (get) Token: 0x06004430 RID: 17456 RVA: 0x0011E70B File Offset: 0x0011C90B
			object IDictionaryEnumerator.Key
			{
				get
				{
					if (this.index == 0 || this.index == this._sortedList.Count + 1)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.key;
				}
			}

			// Token: 0x06004431 RID: 17457 RVA: 0x0011E73C File Offset: 0x0011C93C
			public bool MoveNext()
			{
				if (this.version != this._sortedList.version)
				{
					global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				if (this.index < this._sortedList.Count)
				{
					this.key = this._sortedList.keys[this.index];
					this.value = this._sortedList.values[this.index];
					this.index++;
					return true;
				}
				this.index = this._sortedList.Count + 1;
				this.key = default(TKey);
				this.value = default(TValue);
				return false;
			}

			// Token: 0x17000F76 RID: 3958
			// (get) Token: 0x06004432 RID: 17458 RVA: 0x0011E7EC File Offset: 0x0011C9EC
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (this.index == 0 || this.index == this._sortedList.Count + 1)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return new DictionaryEntry(this.key, this.value);
				}
			}

			// Token: 0x17000F77 RID: 3959
			// (get) Token: 0x06004433 RID: 17459 RVA: 0x0011E838 File Offset: 0x0011CA38
			public KeyValuePair<TKey, TValue> Current
			{
				get
				{
					return new KeyValuePair<TKey, TValue>(this.key, this.value);
				}
			}

			// Token: 0x17000F78 RID: 3960
			// (get) Token: 0x06004434 RID: 17460 RVA: 0x0011E84C File Offset: 0x0011CA4C
			object IEnumerator.Current
			{
				get
				{
					if (this.index == 0 || this.index == this._sortedList.Count + 1)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					if (this.getEnumeratorRetType == 2)
					{
						return new DictionaryEntry(this.key, this.value);
					}
					return new KeyValuePair<TKey, TValue>(this.key, this.value);
				}
			}

			// Token: 0x17000F79 RID: 3961
			// (get) Token: 0x06004435 RID: 17461 RVA: 0x0011E8BD File Offset: 0x0011CABD
			object IDictionaryEnumerator.Value
			{
				get
				{
					if (this.index == 0 || this.index == this._sortedList.Count + 1)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.value;
				}
			}

			// Token: 0x06004436 RID: 17462 RVA: 0x0011E8EE File Offset: 0x0011CAEE
			void IEnumerator.Reset()
			{
				if (this.version != this._sortedList.version)
				{
					global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.index = 0;
				this.key = default(TKey);
				this.value = default(TValue);
			}

			// Token: 0x04003515 RID: 13589
			private SortedList<TKey, TValue> _sortedList;

			// Token: 0x04003516 RID: 13590
			private TKey key;

			// Token: 0x04003517 RID: 13591
			private TValue value;

			// Token: 0x04003518 RID: 13592
			private int index;

			// Token: 0x04003519 RID: 13593
			private int version;

			// Token: 0x0400351A RID: 13594
			private int getEnumeratorRetType;

			// Token: 0x0400351B RID: 13595
			internal const int KeyValuePair = 1;

			// Token: 0x0400351C RID: 13596
			internal const int DictEntry = 2;
		}

		// Token: 0x020007F5 RID: 2037
		[Serializable]
		private sealed class SortedListKeyEnumerator : IEnumerator<TKey>, IDisposable, IEnumerator
		{
			// Token: 0x06004437 RID: 17463 RVA: 0x0011E929 File Offset: 0x0011CB29
			internal SortedListKeyEnumerator(SortedList<TKey, TValue> sortedList)
			{
				this._sortedList = sortedList;
				this.version = sortedList.version;
			}

			// Token: 0x06004438 RID: 17464 RVA: 0x0011E944 File Offset: 0x0011CB44
			public void Dispose()
			{
				this.index = 0;
				this.currentKey = default(TKey);
			}

			// Token: 0x06004439 RID: 17465 RVA: 0x0011E95C File Offset: 0x0011CB5C
			public bool MoveNext()
			{
				if (this.version != this._sortedList.version)
				{
					global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				if (this.index < this._sortedList.Count)
				{
					this.currentKey = this._sortedList.keys[this.index];
					this.index++;
					return true;
				}
				this.index = this._sortedList.Count + 1;
				this.currentKey = default(TKey);
				return false;
			}

			// Token: 0x17000F7A RID: 3962
			// (get) Token: 0x0600443A RID: 17466 RVA: 0x0011E9E2 File Offset: 0x0011CBE2
			public TKey Current
			{
				get
				{
					return this.currentKey;
				}
			}

			// Token: 0x17000F7B RID: 3963
			// (get) Token: 0x0600443B RID: 17467 RVA: 0x0011E9EA File Offset: 0x0011CBEA
			object IEnumerator.Current
			{
				get
				{
					if (this.index == 0 || this.index == this._sortedList.Count + 1)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.currentKey;
				}
			}

			// Token: 0x0600443C RID: 17468 RVA: 0x0011EA1B File Offset: 0x0011CC1B
			void IEnumerator.Reset()
			{
				if (this.version != this._sortedList.version)
				{
					global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.index = 0;
				this.currentKey = default(TKey);
			}

			// Token: 0x0400351D RID: 13597
			private SortedList<TKey, TValue> _sortedList;

			// Token: 0x0400351E RID: 13598
			private int index;

			// Token: 0x0400351F RID: 13599
			private int version;

			// Token: 0x04003520 RID: 13600
			private TKey currentKey;
		}

		// Token: 0x020007F6 RID: 2038
		[Serializable]
		private sealed class SortedListValueEnumerator : IEnumerator<TValue>, IDisposable, IEnumerator
		{
			// Token: 0x0600443D RID: 17469 RVA: 0x0011EA4A File Offset: 0x0011CC4A
			internal SortedListValueEnumerator(SortedList<TKey, TValue> sortedList)
			{
				this._sortedList = sortedList;
				this.version = sortedList.version;
			}

			// Token: 0x0600443E RID: 17470 RVA: 0x0011EA65 File Offset: 0x0011CC65
			public void Dispose()
			{
				this.index = 0;
				this.currentValue = default(TValue);
			}

			// Token: 0x0600443F RID: 17471 RVA: 0x0011EA7C File Offset: 0x0011CC7C
			public bool MoveNext()
			{
				if (this.version != this._sortedList.version)
				{
					global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				if (this.index < this._sortedList.Count)
				{
					this.currentValue = this._sortedList.values[this.index];
					this.index++;
					return true;
				}
				this.index = this._sortedList.Count + 1;
				this.currentValue = default(TValue);
				return false;
			}

			// Token: 0x17000F7C RID: 3964
			// (get) Token: 0x06004440 RID: 17472 RVA: 0x0011EB02 File Offset: 0x0011CD02
			public TValue Current
			{
				get
				{
					return this.currentValue;
				}
			}

			// Token: 0x17000F7D RID: 3965
			// (get) Token: 0x06004441 RID: 17473 RVA: 0x0011EB0A File Offset: 0x0011CD0A
			object IEnumerator.Current
			{
				get
				{
					if (this.index == 0 || this.index == this._sortedList.Count + 1)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.currentValue;
				}
			}

			// Token: 0x06004442 RID: 17474 RVA: 0x0011EB3B File Offset: 0x0011CD3B
			void IEnumerator.Reset()
			{
				if (this.version != this._sortedList.version)
				{
					global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.index = 0;
				this.currentValue = default(TValue);
			}

			// Token: 0x04003521 RID: 13601
			private SortedList<TKey, TValue> _sortedList;

			// Token: 0x04003522 RID: 13602
			private int index;

			// Token: 0x04003523 RID: 13603
			private int version;

			// Token: 0x04003524 RID: 13604
			private TValue currentValue;
		}

		// Token: 0x020007F7 RID: 2039
		[DebuggerTypeProxy(typeof(System_DictionaryKeyCollectionDebugView<, >))]
		[DebuggerDisplay("Count = {Count}")]
		[Serializable]
		private sealed class KeyList : IList<TKey>, ICollection<TKey>, IEnumerable<TKey>, IEnumerable, ICollection
		{
			// Token: 0x06004443 RID: 17475 RVA: 0x0011EB6A File Offset: 0x0011CD6A
			internal KeyList(SortedList<TKey, TValue> dictionary)
			{
				this._dict = dictionary;
			}

			// Token: 0x17000F7E RID: 3966
			// (get) Token: 0x06004444 RID: 17476 RVA: 0x0011EB79 File Offset: 0x0011CD79
			public int Count
			{
				get
				{
					return this._dict._size;
				}
			}

			// Token: 0x17000F7F RID: 3967
			// (get) Token: 0x06004445 RID: 17477 RVA: 0x0011EB86 File Offset: 0x0011CD86
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000F80 RID: 3968
			// (get) Token: 0x06004446 RID: 17478 RVA: 0x0011EB89 File Offset: 0x0011CD89
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000F81 RID: 3969
			// (get) Token: 0x06004447 RID: 17479 RVA: 0x0011EB8C File Offset: 0x0011CD8C
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this._dict).SyncRoot;
				}
			}

			// Token: 0x06004448 RID: 17480 RVA: 0x0011EB99 File Offset: 0x0011CD99
			public void Add(TKey key)
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x06004449 RID: 17481 RVA: 0x0011EBA2 File Offset: 0x0011CDA2
			public void Clear()
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x0600444A RID: 17482 RVA: 0x0011EBAB File Offset: 0x0011CDAB
			public bool Contains(TKey key)
			{
				return this._dict.ContainsKey(key);
			}

			// Token: 0x0600444B RID: 17483 RVA: 0x0011EBB9 File Offset: 0x0011CDB9
			public void CopyTo(TKey[] array, int arrayIndex)
			{
				Array.Copy(this._dict.keys, 0, array, arrayIndex, this._dict.Count);
			}

			// Token: 0x0600444C RID: 17484 RVA: 0x0011EBDC File Offset: 0x0011CDDC
			void ICollection.CopyTo(Array array, int arrayIndex)
			{
				if (array != null && array.Rank != 1)
				{
					global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Arg_RankMultiDimNotSupported);
				}
				try
				{
					Array.Copy(this._dict.keys, 0, array, arrayIndex, this._dict.Count);
				}
				catch (ArrayTypeMismatchException)
				{
					global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidArrayType);
				}
			}

			// Token: 0x0600444D RID: 17485 RVA: 0x0011EC38 File Offset: 0x0011CE38
			public void Insert(int index, TKey value)
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x17000F82 RID: 3970
			public TKey this[int index]
			{
				get
				{
					return this._dict.GetKey(index);
				}
				set
				{
					global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_KeyCollectionSet);
				}
			}

			// Token: 0x06004450 RID: 17488 RVA: 0x0011EC58 File Offset: 0x0011CE58
			public IEnumerator<TKey> GetEnumerator()
			{
				return new SortedList<TKey, TValue>.SortedListKeyEnumerator(this._dict);
			}

			// Token: 0x06004451 RID: 17489 RVA: 0x0011EC65 File Offset: 0x0011CE65
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SortedList<TKey, TValue>.SortedListKeyEnumerator(this._dict);
			}

			// Token: 0x06004452 RID: 17490 RVA: 0x0011EC74 File Offset: 0x0011CE74
			public int IndexOf(TKey key)
			{
				if (key == null)
				{
					global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
				}
				int num = Array.BinarySearch<TKey>(this._dict.keys, 0, this._dict.Count, key, this._dict.comparer);
				if (num >= 0)
				{
					return num;
				}
				return -1;
			}

			// Token: 0x06004453 RID: 17491 RVA: 0x0011ECBF File Offset: 0x0011CEBF
			public bool Remove(TKey key)
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_SortedListNestedWrite);
				return false;
			}

			// Token: 0x06004454 RID: 17492 RVA: 0x0011ECC9 File Offset: 0x0011CEC9
			public void RemoveAt(int index)
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x04003525 RID: 13605
			private SortedList<TKey, TValue> _dict;
		}

		// Token: 0x020007F8 RID: 2040
		[DebuggerTypeProxy(typeof(System_DictionaryValueCollectionDebugView<, >))]
		[DebuggerDisplay("Count = {Count}")]
		[Serializable]
		private sealed class ValueList : IList<TValue>, ICollection<TValue>, IEnumerable<TValue>, IEnumerable, ICollection
		{
			// Token: 0x06004455 RID: 17493 RVA: 0x0011ECD2 File Offset: 0x0011CED2
			internal ValueList(SortedList<TKey, TValue> dictionary)
			{
				this._dict = dictionary;
			}

			// Token: 0x17000F83 RID: 3971
			// (get) Token: 0x06004456 RID: 17494 RVA: 0x0011ECE1 File Offset: 0x0011CEE1
			public int Count
			{
				get
				{
					return this._dict._size;
				}
			}

			// Token: 0x17000F84 RID: 3972
			// (get) Token: 0x06004457 RID: 17495 RVA: 0x0011ECEE File Offset: 0x0011CEEE
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000F85 RID: 3973
			// (get) Token: 0x06004458 RID: 17496 RVA: 0x0011ECF1 File Offset: 0x0011CEF1
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000F86 RID: 3974
			// (get) Token: 0x06004459 RID: 17497 RVA: 0x0011ECF4 File Offset: 0x0011CEF4
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this._dict).SyncRoot;
				}
			}

			// Token: 0x0600445A RID: 17498 RVA: 0x0011ED01 File Offset: 0x0011CF01
			public void Add(TValue key)
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x0600445B RID: 17499 RVA: 0x0011ED0A File Offset: 0x0011CF0A
			public void Clear()
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x0600445C RID: 17500 RVA: 0x0011ED13 File Offset: 0x0011CF13
			public bool Contains(TValue value)
			{
				return this._dict.ContainsValue(value);
			}

			// Token: 0x0600445D RID: 17501 RVA: 0x0011ED21 File Offset: 0x0011CF21
			public void CopyTo(TValue[] array, int arrayIndex)
			{
				Array.Copy(this._dict.values, 0, array, arrayIndex, this._dict.Count);
			}

			// Token: 0x0600445E RID: 17502 RVA: 0x0011ED44 File Offset: 0x0011CF44
			void ICollection.CopyTo(Array array, int arrayIndex)
			{
				if (array != null && array.Rank != 1)
				{
					global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Arg_RankMultiDimNotSupported);
				}
				try
				{
					Array.Copy(this._dict.values, 0, array, arrayIndex, this._dict.Count);
				}
				catch (ArrayTypeMismatchException)
				{
					global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidArrayType);
				}
			}

			// Token: 0x0600445F RID: 17503 RVA: 0x0011EDA0 File Offset: 0x0011CFA0
			public void Insert(int index, TValue value)
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x17000F87 RID: 3975
			public TValue this[int index]
			{
				get
				{
					return this._dict.GetByIndex(index);
				}
				set
				{
					global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_SortedListNestedWrite);
				}
			}

			// Token: 0x06004462 RID: 17506 RVA: 0x0011EDC0 File Offset: 0x0011CFC0
			public IEnumerator<TValue> GetEnumerator()
			{
				return new SortedList<TKey, TValue>.SortedListValueEnumerator(this._dict);
			}

			// Token: 0x06004463 RID: 17507 RVA: 0x0011EDCD File Offset: 0x0011CFCD
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SortedList<TKey, TValue>.SortedListValueEnumerator(this._dict);
			}

			// Token: 0x06004464 RID: 17508 RVA: 0x0011EDDA File Offset: 0x0011CFDA
			public int IndexOf(TValue value)
			{
				return Array.IndexOf<TValue>(this._dict.values, value, 0, this._dict.Count);
			}

			// Token: 0x06004465 RID: 17509 RVA: 0x0011EDF9 File Offset: 0x0011CFF9
			public bool Remove(TValue value)
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_SortedListNestedWrite);
				return false;
			}

			// Token: 0x06004466 RID: 17510 RVA: 0x0011EE03 File Offset: 0x0011D003
			public void RemoveAt(int index)
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x04003526 RID: 13606
			private SortedList<TKey, TValue> _dict;
		}
	}
}
