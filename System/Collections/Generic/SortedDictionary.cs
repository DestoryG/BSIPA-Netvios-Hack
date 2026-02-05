using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x020003C7 RID: 967
	[DebuggerTypeProxy(typeof(System_DictionaryDebugView<, >))]
	[DebuggerDisplay("Count = {Count}")]
	[global::__DynamicallyInvokable]
	[Serializable]
	public class SortedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
	{
		// Token: 0x060024A8 RID: 9384 RVA: 0x000AB0E0 File Offset: 0x000A92E0
		[global::__DynamicallyInvokable]
		public SortedDictionary()
			: this(null)
		{
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x000AB0E9 File Offset: 0x000A92E9
		[global::__DynamicallyInvokable]
		public SortedDictionary(IDictionary<TKey, TValue> dictionary)
			: this(dictionary, null)
		{
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x000AB0F4 File Offset: 0x000A92F4
		[global::__DynamicallyInvokable]
		public SortedDictionary(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
		{
			if (dictionary == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.dictionary);
			}
			this._set = new TreeSet<KeyValuePair<TKey, TValue>>(new SortedDictionary<TKey, TValue>.KeyValuePairComparer(comparer));
			foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
			{
				this._set.Add(keyValuePair);
			}
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000AB164 File Offset: 0x000A9364
		[global::__DynamicallyInvokable]
		public SortedDictionary(IComparer<TKey> comparer)
		{
			this._set = new TreeSet<KeyValuePair<TKey, TValue>>(new SortedDictionary<TKey, TValue>.KeyValuePairComparer(comparer));
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x000AB17D File Offset: 0x000A937D
		[global::__DynamicallyInvokable]
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			this._set.Add(keyValuePair);
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x000AB18C File Offset: 0x000A938C
		[global::__DynamicallyInvokable]
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			SortedSet<KeyValuePair<TKey, TValue>>.Node node = this._set.FindNode(keyValuePair);
			if (node == null)
			{
				return false;
			}
			if (keyValuePair.Value == null)
			{
				return node.Item.Value == null;
			}
			return EqualityComparer<TValue>.Default.Equals(node.Item.Value, keyValuePair.Value);
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x000AB1EC File Offset: 0x000A93EC
		[global::__DynamicallyInvokable]
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			SortedSet<KeyValuePair<TKey, TValue>>.Node node = this._set.FindNode(keyValuePair);
			if (node == null)
			{
				return false;
			}
			if (EqualityComparer<TValue>.Default.Equals(node.Item.Value, keyValuePair.Value))
			{
				this._set.Remove(keyValuePair);
				return true;
			}
			return false;
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x060024AF RID: 9391 RVA: 0x000AB239 File Offset: 0x000A9439
		[global::__DynamicallyInvokable]
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x17000941 RID: 2369
		[global::__DynamicallyInvokable]
		public TValue this[TKey key]
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (key == null)
				{
					global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
				}
				SortedSet<KeyValuePair<TKey, TValue>>.Node node = this._set.FindNode(new KeyValuePair<TKey, TValue>(key, default(TValue)));
				if (node == null)
				{
					global::System.ThrowHelper.ThrowKeyNotFoundException();
				}
				return node.Item.Value;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (key == null)
				{
					global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
				}
				SortedSet<KeyValuePair<TKey, TValue>>.Node node = this._set.FindNode(new KeyValuePair<TKey, TValue>(key, default(TValue)));
				if (node == null)
				{
					this._set.Add(new KeyValuePair<TKey, TValue>(key, value));
					return;
				}
				node.Item = new KeyValuePair<TKey, TValue>(node.Item.Key, value);
				this._set.UpdateVersion();
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x060024B2 RID: 9394 RVA: 0x000AB2F7 File Offset: 0x000A94F7
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._set.Count;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x060024B3 RID: 9395 RVA: 0x000AB304 File Offset: 0x000A9504
		[global::__DynamicallyInvokable]
		public IComparer<TKey> Comparer
		{
			[global::__DynamicallyInvokable]
			get
			{
				return ((SortedDictionary<TKey, TValue>.KeyValuePairComparer)this._set.Comparer).keyComparer;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060024B4 RID: 9396 RVA: 0x000AB31B File Offset: 0x000A951B
		[global::__DynamicallyInvokable]
		public SortedDictionary<TKey, TValue>.KeyCollection Keys
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this.keys == null)
				{
					this.keys = new SortedDictionary<TKey, TValue>.KeyCollection(this);
				}
				return this.keys;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060024B5 RID: 9397 RVA: 0x000AB337 File Offset: 0x000A9537
		[global::__DynamicallyInvokable]
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.Keys;
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060024B6 RID: 9398 RVA: 0x000AB33F File Offset: 0x000A953F
		[global::__DynamicallyInvokable]
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.Keys;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060024B7 RID: 9399 RVA: 0x000AB347 File Offset: 0x000A9547
		[global::__DynamicallyInvokable]
		public SortedDictionary<TKey, TValue>.ValueCollection Values
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this.values == null)
				{
					this.values = new SortedDictionary<TKey, TValue>.ValueCollection(this);
				}
				return this.values;
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060024B8 RID: 9400 RVA: 0x000AB363 File Offset: 0x000A9563
		[global::__DynamicallyInvokable]
		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.Values;
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060024B9 RID: 9401 RVA: 0x000AB36B File Offset: 0x000A956B
		[global::__DynamicallyInvokable]
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.Values;
			}
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000AB373 File Offset: 0x000A9573
		[global::__DynamicallyInvokable]
		public void Add(TKey key, TValue value)
		{
			if (key == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
			}
			this._set.Add(new KeyValuePair<TKey, TValue>(key, value));
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x000AB396 File Offset: 0x000A9596
		[global::__DynamicallyInvokable]
		public void Clear()
		{
			this._set.Clear();
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000AB3A4 File Offset: 0x000A95A4
		[global::__DynamicallyInvokable]
		public bool ContainsKey(TKey key)
		{
			if (key == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
			}
			return this._set.Contains(new KeyValuePair<TKey, TValue>(key, default(TValue)));
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000AB3DC File Offset: 0x000A95DC
		[global::__DynamicallyInvokable]
		public bool ContainsValue(TValue value)
		{
			bool found = false;
			if (value == null)
			{
				this._set.InOrderTreeWalk(delegate(SortedSet<KeyValuePair<TKey, TValue>>.Node node)
				{
					if (node.Item.Value == null)
					{
						found = true;
						return false;
					}
					return true;
				});
			}
			else
			{
				EqualityComparer<TValue> valueComparer = EqualityComparer<TValue>.Default;
				this._set.InOrderTreeWalk(delegate(SortedSet<KeyValuePair<TKey, TValue>>.Node node)
				{
					if (valueComparer.Equals(node.Item.Value, value))
					{
						found = true;
						return false;
					}
					return true;
				});
			}
			return found;
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x000AB45A File Offset: 0x000A965A
		[global::__DynamicallyInvokable]
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			this._set.CopyTo(array, index);
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000AB469 File Offset: 0x000A9669
		[global::__DynamicallyInvokable]
		public SortedDictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return new SortedDictionary<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x000AB472 File Offset: 0x000A9672
		[global::__DynamicallyInvokable]
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return new SortedDictionary<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000AB480 File Offset: 0x000A9680
		[global::__DynamicallyInvokable]
		public bool Remove(TKey key)
		{
			if (key == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
			}
			return this._set.Remove(new KeyValuePair<TKey, TValue>(key, default(TValue)));
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000AB4B8 File Offset: 0x000A96B8
		[global::__DynamicallyInvokable]
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (key == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
			}
			SortedSet<KeyValuePair<TKey, TValue>>.Node node = this._set.FindNode(new KeyValuePair<TKey, TValue>(key, default(TValue)));
			if (node == null)
			{
				value = default(TValue);
				return false;
			}
			value = node.Item.Value;
			return true;
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000AB50C File Offset: 0x000A970C
		[global::__DynamicallyInvokable]
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this._set).CopyTo(array, index);
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060024C4 RID: 9412 RVA: 0x000AB51B File Offset: 0x000A971B
		[global::__DynamicallyInvokable]
		bool IDictionary.IsFixedSize
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060024C5 RID: 9413 RVA: 0x000AB51E File Offset: 0x000A971E
		[global::__DynamicallyInvokable]
		bool IDictionary.IsReadOnly
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060024C6 RID: 9414 RVA: 0x000AB521 File Offset: 0x000A9721
		[global::__DynamicallyInvokable]
		ICollection IDictionary.Keys
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.Keys;
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060024C7 RID: 9415 RVA: 0x000AB529 File Offset: 0x000A9729
		[global::__DynamicallyInvokable]
		ICollection IDictionary.Values
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.Values;
			}
		}

		// Token: 0x1700094E RID: 2382
		[global::__DynamicallyInvokable]
		object IDictionary.this[object key]
		{
			[global::__DynamicallyInvokable]
			get
			{
				TValue tvalue;
				if (SortedDictionary<TKey, TValue>.IsCompatibleKey(key) && this.TryGetValue((TKey)((object)key), out tvalue))
				{
					return tvalue;
				}
				return null;
			}
			[global::__DynamicallyInvokable]
			set
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

		// Token: 0x060024CA RID: 9418 RVA: 0x000AB5DC File Offset: 0x000A97DC
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

		// Token: 0x060024CB RID: 9419 RVA: 0x000AB654 File Offset: 0x000A9854
		[global::__DynamicallyInvokable]
		bool IDictionary.Contains(object key)
		{
			return SortedDictionary<TKey, TValue>.IsCompatibleKey(key) && this.ContainsKey((TKey)((object)key));
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x000AB66C File Offset: 0x000A986C
		private static bool IsCompatibleKey(object key)
		{
			if (key == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.key);
			}
			return key is TKey;
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x000AB680 File Offset: 0x000A9880
		[global::__DynamicallyInvokable]
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new SortedDictionary<TKey, TValue>.Enumerator(this, 2);
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x000AB68E File Offset: 0x000A988E
		[global::__DynamicallyInvokable]
		void IDictionary.Remove(object key)
		{
			if (SortedDictionary<TKey, TValue>.IsCompatibleKey(key))
			{
				this.Remove((TKey)((object)key));
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060024CF RID: 9423 RVA: 0x000AB6A5 File Offset: 0x000A98A5
		[global::__DynamicallyInvokable]
		bool ICollection.IsSynchronized
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060024D0 RID: 9424 RVA: 0x000AB6A8 File Offset: 0x000A98A8
		[global::__DynamicallyInvokable]
		object ICollection.SyncRoot
		{
			[global::__DynamicallyInvokable]
			get
			{
				return ((ICollection)this._set).SyncRoot;
			}
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x000AB6B5 File Offset: 0x000A98B5
		[global::__DynamicallyInvokable]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SortedDictionary<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x04002021 RID: 8225
		[NonSerialized]
		private SortedDictionary<TKey, TValue>.KeyCollection keys;

		// Token: 0x04002022 RID: 8226
		[NonSerialized]
		private SortedDictionary<TKey, TValue>.ValueCollection values;

		// Token: 0x04002023 RID: 8227
		private TreeSet<KeyValuePair<TKey, TValue>> _set;

		// Token: 0x020007FA RID: 2042
		[global::__DynamicallyInvokable]
		public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IEnumerator, IDictionaryEnumerator
		{
			// Token: 0x0600446D RID: 17517 RVA: 0x0011EF8E File Offset: 0x0011D18E
			internal Enumerator(SortedDictionary<TKey, TValue> dictionary, int getEnumeratorRetType)
			{
				this.treeEnum = dictionary._set.GetEnumerator();
				this.getEnumeratorRetType = getEnumeratorRetType;
			}

			// Token: 0x0600446E RID: 17518 RVA: 0x0011EFA8 File Offset: 0x0011D1A8
			[global::__DynamicallyInvokable]
			public bool MoveNext()
			{
				return this.treeEnum.MoveNext();
			}

			// Token: 0x0600446F RID: 17519 RVA: 0x0011EFB5 File Offset: 0x0011D1B5
			[global::__DynamicallyInvokable]
			public void Dispose()
			{
				this.treeEnum.Dispose();
			}

			// Token: 0x17000F8A RID: 3978
			// (get) Token: 0x06004470 RID: 17520 RVA: 0x0011EFC2 File Offset: 0x0011D1C2
			[global::__DynamicallyInvokable]
			public KeyValuePair<TKey, TValue> Current
			{
				[global::__DynamicallyInvokable]
				get
				{
					return this.treeEnum.Current;
				}
			}

			// Token: 0x17000F8B RID: 3979
			// (get) Token: 0x06004471 RID: 17521 RVA: 0x0011EFCF File Offset: 0x0011D1CF
			internal bool NotStartedOrEnded
			{
				get
				{
					return this.treeEnum.NotStartedOrEnded;
				}
			}

			// Token: 0x06004472 RID: 17522 RVA: 0x0011EFDC File Offset: 0x0011D1DC
			internal void Reset()
			{
				this.treeEnum.Reset();
			}

			// Token: 0x06004473 RID: 17523 RVA: 0x0011EFE9 File Offset: 0x0011D1E9
			[global::__DynamicallyInvokable]
			void IEnumerator.Reset()
			{
				this.treeEnum.Reset();
			}

			// Token: 0x17000F8C RID: 3980
			// (get) Token: 0x06004474 RID: 17524 RVA: 0x0011EFF8 File Offset: 0x0011D1F8
			[global::__DynamicallyInvokable]
			object IEnumerator.Current
			{
				[global::__DynamicallyInvokable]
				get
				{
					if (this.NotStartedOrEnded)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					KeyValuePair<TKey, TValue> keyValuePair;
					if (this.getEnumeratorRetType == 2)
					{
						keyValuePair = this.Current;
						object obj = keyValuePair.Key;
						keyValuePair = this.Current;
						return new DictionaryEntry(obj, keyValuePair.Value);
					}
					keyValuePair = this.Current;
					TKey key = keyValuePair.Key;
					keyValuePair = this.Current;
					return new KeyValuePair<TKey, TValue>(key, keyValuePair.Value);
				}
			}

			// Token: 0x17000F8D RID: 3981
			// (get) Token: 0x06004475 RID: 17525 RVA: 0x0011F074 File Offset: 0x0011D274
			[global::__DynamicallyInvokable]
			object IDictionaryEnumerator.Key
			{
				[global::__DynamicallyInvokable]
				get
				{
					if (this.NotStartedOrEnded)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					KeyValuePair<TKey, TValue> keyValuePair = this.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x17000F8E RID: 3982
			// (get) Token: 0x06004476 RID: 17526 RVA: 0x0011F0A4 File Offset: 0x0011D2A4
			[global::__DynamicallyInvokable]
			object IDictionaryEnumerator.Value
			{
				[global::__DynamicallyInvokable]
				get
				{
					if (this.NotStartedOrEnded)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					KeyValuePair<TKey, TValue> keyValuePair = this.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x17000F8F RID: 3983
			// (get) Token: 0x06004477 RID: 17527 RVA: 0x0011F0D4 File Offset: 0x0011D2D4
			[global::__DynamicallyInvokable]
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				[global::__DynamicallyInvokable]
				get
				{
					if (this.NotStartedOrEnded)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					KeyValuePair<TKey, TValue> keyValuePair = this.Current;
					object obj = keyValuePair.Key;
					keyValuePair = this.Current;
					return new DictionaryEntry(obj, keyValuePair.Value);
				}
			}

			// Token: 0x0400352B RID: 13611
			private SortedSet<KeyValuePair<TKey, TValue>>.Enumerator treeEnum;

			// Token: 0x0400352C RID: 13612
			private int getEnumeratorRetType;

			// Token: 0x0400352D RID: 13613
			internal const int KeyValuePair = 1;

			// Token: 0x0400352E RID: 13614
			internal const int DictEntry = 2;
		}

		// Token: 0x020007FB RID: 2043
		[DebuggerTypeProxy(typeof(System_DictionaryKeyCollectionDebugView<, >))]
		[DebuggerDisplay("Count = {Count}")]
		[global::__DynamicallyInvokable]
		[Serializable]
		public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable, ICollection, IReadOnlyCollection<TKey>
		{
			// Token: 0x06004478 RID: 17528 RVA: 0x0011F11B File Offset: 0x0011D31B
			[global::__DynamicallyInvokable]
			public KeyCollection(SortedDictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.dictionary);
				}
				this.dictionary = dictionary;
			}

			// Token: 0x06004479 RID: 17529 RVA: 0x0011F133 File Offset: 0x0011D333
			[global::__DynamicallyInvokable]
			public SortedDictionary<TKey, TValue>.KeyCollection.Enumerator GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}

			// Token: 0x0600447A RID: 17530 RVA: 0x0011F140 File Offset: 0x0011D340
			[global::__DynamicallyInvokable]
			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}

			// Token: 0x0600447B RID: 17531 RVA: 0x0011F152 File Offset: 0x0011D352
			[global::__DynamicallyInvokable]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}

			// Token: 0x0600447C RID: 17532 RVA: 0x0011F164 File Offset: 0x0011D364
			[global::__DynamicallyInvokable]
			public void CopyTo(TKey[] array, int index)
			{
				if (array == null)
				{
					global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.array);
				}
				if (index < 0)
				{
					global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.index);
				}
				if (array.Length - index < this.Count)
				{
					global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				this.dictionary._set.InOrderTreeWalk(delegate(SortedSet<KeyValuePair<TKey, TValue>>.Node node)
				{
					TKey[] array2 = array;
					int index2 = index;
					index = index2 + 1;
					array2[index2] = node.Item.Key;
					return true;
				});
			}

			// Token: 0x0600447D RID: 17533 RVA: 0x0011F1E0 File Offset: 0x0011D3E0
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
				if (index < 0)
				{
					global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.arrayIndex, global::System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < this.dictionary.Count)
				{
					global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TKey[] array2 = array as TKey[];
				if (array2 != null)
				{
					this.CopyTo(array2, index);
					return;
				}
				object[] objects = (object[])array;
				if (objects == null)
				{
					global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidArrayType);
				}
				try
				{
					this.dictionary._set.InOrderTreeWalk(delegate(SortedSet<KeyValuePair<TKey, TValue>>.Node node)
					{
						object[] objects2 = objects;
						int index2 = index;
						index = index2 + 1;
						objects2[index2] = node.Item.Key;
						return true;
					});
				}
				catch (ArrayTypeMismatchException)
				{
					global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidArrayType);
				}
			}

			// Token: 0x17000F90 RID: 3984
			// (get) Token: 0x0600447E RID: 17534 RVA: 0x0011F2C0 File Offset: 0x0011D4C0
			[global::__DynamicallyInvokable]
			public int Count
			{
				[global::__DynamicallyInvokable]
				get
				{
					return this.dictionary.Count;
				}
			}

			// Token: 0x17000F91 RID: 3985
			// (get) Token: 0x0600447F RID: 17535 RVA: 0x0011F2CD File Offset: 0x0011D4CD
			[global::__DynamicallyInvokable]
			bool ICollection<TKey>.IsReadOnly
			{
				[global::__DynamicallyInvokable]
				get
				{
					return true;
				}
			}

			// Token: 0x06004480 RID: 17536 RVA: 0x0011F2D0 File Offset: 0x0011D4D0
			[global::__DynamicallyInvokable]
			void ICollection<TKey>.Add(TKey item)
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_KeyCollectionSet);
			}

			// Token: 0x06004481 RID: 17537 RVA: 0x0011F2D9 File Offset: 0x0011D4D9
			[global::__DynamicallyInvokable]
			void ICollection<TKey>.Clear()
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_KeyCollectionSet);
			}

			// Token: 0x06004482 RID: 17538 RVA: 0x0011F2E2 File Offset: 0x0011D4E2
			[global::__DynamicallyInvokable]
			bool ICollection<TKey>.Contains(TKey item)
			{
				return this.dictionary.ContainsKey(item);
			}

			// Token: 0x06004483 RID: 17539 RVA: 0x0011F2F0 File Offset: 0x0011D4F0
			[global::__DynamicallyInvokable]
			bool ICollection<TKey>.Remove(TKey item)
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_KeyCollectionSet);
				return false;
			}

			// Token: 0x17000F92 RID: 3986
			// (get) Token: 0x06004484 RID: 17540 RVA: 0x0011F2FA File Offset: 0x0011D4FA
			[global::__DynamicallyInvokable]
			bool ICollection.IsSynchronized
			{
				[global::__DynamicallyInvokable]
				get
				{
					return false;
				}
			}

			// Token: 0x17000F93 RID: 3987
			// (get) Token: 0x06004485 RID: 17541 RVA: 0x0011F2FD File Offset: 0x0011D4FD
			[global::__DynamicallyInvokable]
			object ICollection.SyncRoot
			{
				[global::__DynamicallyInvokable]
				get
				{
					return ((ICollection)this.dictionary).SyncRoot;
				}
			}

			// Token: 0x0400352F RID: 13615
			private SortedDictionary<TKey, TValue> dictionary;

			// Token: 0x02000929 RID: 2345
			[global::__DynamicallyInvokable]
			public struct Enumerator : IEnumerator<TKey>, IDisposable, IEnumerator
			{
				// Token: 0x06004689 RID: 18057 RVA: 0x00126835 File Offset: 0x00124A35
				internal Enumerator(SortedDictionary<TKey, TValue> dictionary)
				{
					this.dictEnum = dictionary.GetEnumerator();
				}

				// Token: 0x0600468A RID: 18058 RVA: 0x00126843 File Offset: 0x00124A43
				[global::__DynamicallyInvokable]
				public void Dispose()
				{
					this.dictEnum.Dispose();
				}

				// Token: 0x0600468B RID: 18059 RVA: 0x00126850 File Offset: 0x00124A50
				[global::__DynamicallyInvokable]
				public bool MoveNext()
				{
					return this.dictEnum.MoveNext();
				}

				// Token: 0x17000FE7 RID: 4071
				// (get) Token: 0x0600468C RID: 18060 RVA: 0x00126860 File Offset: 0x00124A60
				[global::__DynamicallyInvokable]
				public TKey Current
				{
					[global::__DynamicallyInvokable]
					get
					{
						KeyValuePair<TKey, TValue> keyValuePair = this.dictEnum.Current;
						return keyValuePair.Key;
					}
				}

				// Token: 0x17000FE8 RID: 4072
				// (get) Token: 0x0600468D RID: 18061 RVA: 0x00126880 File Offset: 0x00124A80
				[global::__DynamicallyInvokable]
				object IEnumerator.Current
				{
					[global::__DynamicallyInvokable]
					get
					{
						if (this.dictEnum.NotStartedOrEnded)
						{
							global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
						}
						return this.Current;
					}
				}

				// Token: 0x0600468E RID: 18062 RVA: 0x001268A1 File Offset: 0x00124AA1
				[global::__DynamicallyInvokable]
				void IEnumerator.Reset()
				{
					this.dictEnum.Reset();
				}

				// Token: 0x04003DBC RID: 15804
				private SortedDictionary<TKey, TValue>.Enumerator dictEnum;
			}
		}

		// Token: 0x020007FC RID: 2044
		[DebuggerTypeProxy(typeof(System_DictionaryValueCollectionDebugView<, >))]
		[DebuggerDisplay("Count = {Count}")]
		[global::__DynamicallyInvokable]
		[Serializable]
		public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable, ICollection, IReadOnlyCollection<TValue>
		{
			// Token: 0x06004486 RID: 17542 RVA: 0x0011F30A File Offset: 0x0011D50A
			[global::__DynamicallyInvokable]
			public ValueCollection(SortedDictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.dictionary);
				}
				this.dictionary = dictionary;
			}

			// Token: 0x06004487 RID: 17543 RVA: 0x0011F322 File Offset: 0x0011D522
			[global::__DynamicallyInvokable]
			public SortedDictionary<TKey, TValue>.ValueCollection.Enumerator GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}

			// Token: 0x06004488 RID: 17544 RVA: 0x0011F32F File Offset: 0x0011D52F
			[global::__DynamicallyInvokable]
			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}

			// Token: 0x06004489 RID: 17545 RVA: 0x0011F341 File Offset: 0x0011D541
			[global::__DynamicallyInvokable]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}

			// Token: 0x0600448A RID: 17546 RVA: 0x0011F354 File Offset: 0x0011D554
			[global::__DynamicallyInvokable]
			public void CopyTo(TValue[] array, int index)
			{
				if (array == null)
				{
					global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.array);
				}
				if (index < 0)
				{
					global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.index);
				}
				if (array.Length - index < this.Count)
				{
					global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				this.dictionary._set.InOrderTreeWalk(delegate(SortedSet<KeyValuePair<TKey, TValue>>.Node node)
				{
					TValue[] array2 = array;
					int index2 = index;
					index = index2 + 1;
					array2[index2] = node.Item.Value;
					return true;
				});
			}

			// Token: 0x0600448B RID: 17547 RVA: 0x0011F3D0 File Offset: 0x0011D5D0
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
				if (index < 0)
				{
					global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.arrayIndex, global::System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < this.dictionary.Count)
				{
					global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TValue[] array2 = array as TValue[];
				if (array2 != null)
				{
					this.CopyTo(array2, index);
					return;
				}
				object[] objects = (object[])array;
				if (objects == null)
				{
					global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidArrayType);
				}
				try
				{
					this.dictionary._set.InOrderTreeWalk(delegate(SortedSet<KeyValuePair<TKey, TValue>>.Node node)
					{
						object[] objects2 = objects;
						int index2 = index;
						index = index2 + 1;
						objects2[index2] = node.Item.Value;
						return true;
					});
				}
				catch (ArrayTypeMismatchException)
				{
					global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidArrayType);
				}
			}

			// Token: 0x17000F94 RID: 3988
			// (get) Token: 0x0600448C RID: 17548 RVA: 0x0011F4B0 File Offset: 0x0011D6B0
			[global::__DynamicallyInvokable]
			public int Count
			{
				[global::__DynamicallyInvokable]
				get
				{
					return this.dictionary.Count;
				}
			}

			// Token: 0x17000F95 RID: 3989
			// (get) Token: 0x0600448D RID: 17549 RVA: 0x0011F4BD File Offset: 0x0011D6BD
			[global::__DynamicallyInvokable]
			bool ICollection<TValue>.IsReadOnly
			{
				[global::__DynamicallyInvokable]
				get
				{
					return true;
				}
			}

			// Token: 0x0600448E RID: 17550 RVA: 0x0011F4C0 File Offset: 0x0011D6C0
			[global::__DynamicallyInvokable]
			void ICollection<TValue>.Add(TValue item)
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_ValueCollectionSet);
			}

			// Token: 0x0600448F RID: 17551 RVA: 0x0011F4C9 File Offset: 0x0011D6C9
			[global::__DynamicallyInvokable]
			void ICollection<TValue>.Clear()
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_ValueCollectionSet);
			}

			// Token: 0x06004490 RID: 17552 RVA: 0x0011F4D2 File Offset: 0x0011D6D2
			[global::__DynamicallyInvokable]
			bool ICollection<TValue>.Contains(TValue item)
			{
				return this.dictionary.ContainsValue(item);
			}

			// Token: 0x06004491 RID: 17553 RVA: 0x0011F4E0 File Offset: 0x0011D6E0
			[global::__DynamicallyInvokable]
			bool ICollection<TValue>.Remove(TValue item)
			{
				global::System.ThrowHelper.ThrowNotSupportedException(global::System.ExceptionResource.NotSupported_ValueCollectionSet);
				return false;
			}

			// Token: 0x17000F96 RID: 3990
			// (get) Token: 0x06004492 RID: 17554 RVA: 0x0011F4EA File Offset: 0x0011D6EA
			[global::__DynamicallyInvokable]
			bool ICollection.IsSynchronized
			{
				[global::__DynamicallyInvokable]
				get
				{
					return false;
				}
			}

			// Token: 0x17000F97 RID: 3991
			// (get) Token: 0x06004493 RID: 17555 RVA: 0x0011F4ED File Offset: 0x0011D6ED
			[global::__DynamicallyInvokable]
			object ICollection.SyncRoot
			{
				[global::__DynamicallyInvokable]
				get
				{
					return ((ICollection)this.dictionary).SyncRoot;
				}
			}

			// Token: 0x04003530 RID: 13616
			private SortedDictionary<TKey, TValue> dictionary;

			// Token: 0x0200092C RID: 2348
			[global::__DynamicallyInvokable]
			public struct Enumerator : IEnumerator<TValue>, IDisposable, IEnumerator
			{
				// Token: 0x06004693 RID: 18067 RVA: 0x0012692E File Offset: 0x00124B2E
				internal Enumerator(SortedDictionary<TKey, TValue> dictionary)
				{
					this.dictEnum = dictionary.GetEnumerator();
				}

				// Token: 0x06004694 RID: 18068 RVA: 0x0012693C File Offset: 0x00124B3C
				[global::__DynamicallyInvokable]
				public void Dispose()
				{
					this.dictEnum.Dispose();
				}

				// Token: 0x06004695 RID: 18069 RVA: 0x00126949 File Offset: 0x00124B49
				[global::__DynamicallyInvokable]
				public bool MoveNext()
				{
					return this.dictEnum.MoveNext();
				}

				// Token: 0x17000FE9 RID: 4073
				// (get) Token: 0x06004696 RID: 18070 RVA: 0x00126958 File Offset: 0x00124B58
				[global::__DynamicallyInvokable]
				public TValue Current
				{
					[global::__DynamicallyInvokable]
					get
					{
						KeyValuePair<TKey, TValue> keyValuePair = this.dictEnum.Current;
						return keyValuePair.Value;
					}
				}

				// Token: 0x17000FEA RID: 4074
				// (get) Token: 0x06004697 RID: 18071 RVA: 0x00126978 File Offset: 0x00124B78
				[global::__DynamicallyInvokable]
				object IEnumerator.Current
				{
					[global::__DynamicallyInvokable]
					get
					{
						if (this.dictEnum.NotStartedOrEnded)
						{
							global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
						}
						return this.Current;
					}
				}

				// Token: 0x06004698 RID: 18072 RVA: 0x00126999 File Offset: 0x00124B99
				[global::__DynamicallyInvokable]
				void IEnumerator.Reset()
				{
					this.dictEnum.Reset();
				}

				// Token: 0x04003DC1 RID: 15809
				private SortedDictionary<TKey, TValue>.Enumerator dictEnum;
			}
		}

		// Token: 0x020007FD RID: 2045
		[Serializable]
		internal class KeyValuePairComparer : Comparer<KeyValuePair<TKey, TValue>>
		{
			// Token: 0x06004494 RID: 17556 RVA: 0x0011F4FA File Offset: 0x0011D6FA
			public KeyValuePairComparer(IComparer<TKey> keyComparer)
			{
				if (keyComparer == null)
				{
					this.keyComparer = Comparer<TKey>.Default;
					return;
				}
				this.keyComparer = keyComparer;
			}

			// Token: 0x06004495 RID: 17557 RVA: 0x0011F518 File Offset: 0x0011D718
			public override int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
			{
				return this.keyComparer.Compare(x.Key, y.Key);
			}

			// Token: 0x04003531 RID: 13617
			internal IComparer<TKey> keyComparer;
		}
	}
}
