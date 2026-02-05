using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Specialized
{
	// Token: 0x020003B4 RID: 948
	[Serializable]
	public class OrderedDictionary : IOrderedDictionary, IDictionary, ICollection, IEnumerable, ISerializable, IDeserializationCallback
	{
		// Token: 0x06002389 RID: 9097 RVA: 0x000A80D6 File Offset: 0x000A62D6
		public OrderedDictionary()
			: this(0)
		{
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x000A80DF File Offset: 0x000A62DF
		public OrderedDictionary(int capacity)
			: this(capacity, null)
		{
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x000A80E9 File Offset: 0x000A62E9
		public OrderedDictionary(IEqualityComparer comparer)
			: this(0, comparer)
		{
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x000A80F3 File Offset: 0x000A62F3
		public OrderedDictionary(int capacity, IEqualityComparer comparer)
		{
			this._initialCapacity = capacity;
			this._comparer = comparer;
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x000A810C File Offset: 0x000A630C
		private OrderedDictionary(OrderedDictionary dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this._readOnly = true;
			this._objectsArray = dictionary._objectsArray;
			this._objectsTable = dictionary._objectsTable;
			this._comparer = dictionary._comparer;
			this._initialCapacity = dictionary._initialCapacity;
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x000A8164 File Offset: 0x000A6364
		protected OrderedDictionary(SerializationInfo info, StreamingContext context)
		{
			this._siInfo = info;
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x0600238F RID: 9103 RVA: 0x000A8173 File Offset: 0x000A6373
		public int Count
		{
			get
			{
				return this.objectsArray.Count;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x000A8180 File Offset: 0x000A6380
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this._readOnly;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06002391 RID: 9105 RVA: 0x000A8188 File Offset: 0x000A6388
		public bool IsReadOnly
		{
			get
			{
				return this._readOnly;
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06002392 RID: 9106 RVA: 0x000A8190 File Offset: 0x000A6390
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06002393 RID: 9107 RVA: 0x000A8193 File Offset: 0x000A6393
		public ICollection Keys
		{
			get
			{
				return new OrderedDictionary.OrderedDictionaryKeyValueCollection(this.objectsArray, true);
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06002394 RID: 9108 RVA: 0x000A81A1 File Offset: 0x000A63A1
		private ArrayList objectsArray
		{
			get
			{
				if (this._objectsArray == null)
				{
					this._objectsArray = new ArrayList(this._initialCapacity);
				}
				return this._objectsArray;
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06002395 RID: 9109 RVA: 0x000A81C2 File Offset: 0x000A63C2
		private Hashtable objectsTable
		{
			get
			{
				if (this._objectsTable == null)
				{
					this._objectsTable = new Hashtable(this._initialCapacity, this._comparer);
				}
				return this._objectsTable;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06002396 RID: 9110 RVA: 0x000A81E9 File Offset: 0x000A63E9
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x17000905 RID: 2309
		public object this[int index]
		{
			get
			{
				return ((DictionaryEntry)this.objectsArray[index]).Value;
			}
			set
			{
				if (this._readOnly)
				{
					throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
				}
				if (index < 0 || index >= this.objectsArray.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				object key = ((DictionaryEntry)this.objectsArray[index]).Key;
				this.objectsArray[index] = new DictionaryEntry(key, value);
				this.objectsTable[key] = value;
			}
		}

		// Token: 0x17000906 RID: 2310
		public object this[object key]
		{
			get
			{
				return this.objectsTable[key];
			}
			set
			{
				if (this._readOnly)
				{
					throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
				}
				if (this.objectsTable.Contains(key))
				{
					this.objectsTable[key] = value;
					this.objectsArray[this.IndexOfKey(key)] = new DictionaryEntry(key, value);
					return;
				}
				this.Add(key, value);
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x0600239B RID: 9115 RVA: 0x000A832B File Offset: 0x000A652B
		public ICollection Values
		{
			get
			{
				return new OrderedDictionary.OrderedDictionaryKeyValueCollection(this.objectsArray, false);
			}
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x000A8339 File Offset: 0x000A6539
		public void Add(object key, object value)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
			}
			this.objectsTable.Add(key, value);
			this.objectsArray.Add(new DictionaryEntry(key, value));
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x000A8378 File Offset: 0x000A6578
		public void Clear()
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
			}
			this.objectsTable.Clear();
			this.objectsArray.Clear();
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x000A83A8 File Offset: 0x000A65A8
		public OrderedDictionary AsReadOnly()
		{
			return new OrderedDictionary(this);
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x000A83B0 File Offset: 0x000A65B0
		public bool Contains(object key)
		{
			return this.objectsTable.Contains(key);
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x000A83BE File Offset: 0x000A65BE
		public void CopyTo(Array array, int index)
		{
			this.objectsTable.CopyTo(array, index);
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x000A83D0 File Offset: 0x000A65D0
		private int IndexOfKey(object key)
		{
			for (int i = 0; i < this.objectsArray.Count; i++)
			{
				object key2 = ((DictionaryEntry)this.objectsArray[i]).Key;
				if (this._comparer != null)
				{
					if (this._comparer.Equals(key2, key))
					{
						return i;
					}
				}
				else if (key2.Equals(key))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x000A8434 File Offset: 0x000A6634
		public void Insert(int index, object key, object value)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
			}
			if (index > this.Count || index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.objectsTable.Add(key, value);
			this.objectsArray.Insert(index, new DictionaryEntry(key, value));
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x000A8498 File Offset: 0x000A6698
		protected virtual void OnDeserialization(object sender)
		{
			if (this._siInfo == null)
			{
				throw new SerializationException(SR.GetString("Serialization_InvalidOnDeser"));
			}
			this._comparer = (IEqualityComparer)this._siInfo.GetValue("KeyComparer", typeof(IEqualityComparer));
			this._readOnly = this._siInfo.GetBoolean("ReadOnly");
			this._initialCapacity = this._siInfo.GetInt32("InitialCapacity");
			object[] array = (object[])this._siInfo.GetValue("ArrayList", typeof(object[]));
			if (array != null)
			{
				foreach (object obj in array)
				{
					DictionaryEntry dictionaryEntry;
					try
					{
						dictionaryEntry = (DictionaryEntry)obj;
					}
					catch
					{
						throw new SerializationException(SR.GetString("OrderedDictionary_SerializationMismatch"));
					}
					this.objectsArray.Add(dictionaryEntry);
					this.objectsTable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
				}
			}
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x000A859C File Offset: 0x000A679C
		public void RemoveAt(int index)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
			}
			if (index >= this.Count || index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			object key = ((DictionaryEntry)this.objectsArray[index]).Key;
			this.objectsArray.RemoveAt(index);
			this.objectsTable.Remove(key);
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x000A860C File Offset: 0x000A680C
		public void Remove(object key)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
			}
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int num = this.IndexOfKey(key);
			if (num < 0)
			{
				return;
			}
			this.objectsTable.Remove(key);
			this.objectsArray.RemoveAt(num);
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x000A8664 File Offset: 0x000A6864
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new OrderedDictionary.OrderedDictionaryEnumerator(this.objectsArray, 3);
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x000A8672 File Offset: 0x000A6872
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new OrderedDictionary.OrderedDictionaryEnumerator(this.objectsArray, 3);
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x000A8680 File Offset: 0x000A6880
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("KeyComparer", this._comparer, typeof(IEqualityComparer));
			info.AddValue("ReadOnly", this._readOnly);
			info.AddValue("InitialCapacity", this._initialCapacity);
			object[] array = new object[this.Count];
			this._objectsArray.CopyTo(array);
			info.AddValue("ArrayList", array);
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x000A86FC File Offset: 0x000A68FC
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.OnDeserialization(sender);
		}

		// Token: 0x04001FDB RID: 8155
		private ArrayList _objectsArray;

		// Token: 0x04001FDC RID: 8156
		private Hashtable _objectsTable;

		// Token: 0x04001FDD RID: 8157
		private int _initialCapacity;

		// Token: 0x04001FDE RID: 8158
		private IEqualityComparer _comparer;

		// Token: 0x04001FDF RID: 8159
		private bool _readOnly;

		// Token: 0x04001FE0 RID: 8160
		private object _syncRoot;

		// Token: 0x04001FE1 RID: 8161
		private SerializationInfo _siInfo;

		// Token: 0x04001FE2 RID: 8162
		private const string KeyComparerName = "KeyComparer";

		// Token: 0x04001FE3 RID: 8163
		private const string ArrayListName = "ArrayList";

		// Token: 0x04001FE4 RID: 8164
		private const string ReadOnlyName = "ReadOnly";

		// Token: 0x04001FE5 RID: 8165
		private const string InitCapacityName = "InitialCapacity";

		// Token: 0x020007EE RID: 2030
		private class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x060043FC RID: 17404 RVA: 0x0011DD6A File Offset: 0x0011BF6A
			internal OrderedDictionaryEnumerator(ArrayList array, int objectReturnType)
			{
				this.arrayEnumerator = array.GetEnumerator();
				this._objectReturnType = objectReturnType;
			}

			// Token: 0x17000F64 RID: 3940
			// (get) Token: 0x060043FD RID: 17405 RVA: 0x0011DD88 File Offset: 0x0011BF88
			public object Current
			{
				get
				{
					if (this._objectReturnType == 1)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)this.arrayEnumerator.Current;
						return dictionaryEntry.Key;
					}
					if (this._objectReturnType == 2)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)this.arrayEnumerator.Current;
						return dictionaryEntry.Value;
					}
					return this.Entry;
				}
			}

			// Token: 0x17000F65 RID: 3941
			// (get) Token: 0x060043FE RID: 17406 RVA: 0x0011DDE4 File Offset: 0x0011BFE4
			public DictionaryEntry Entry
			{
				get
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)this.arrayEnumerator.Current;
					object key = dictionaryEntry.Key;
					dictionaryEntry = (DictionaryEntry)this.arrayEnumerator.Current;
					return new DictionaryEntry(key, dictionaryEntry.Value);
				}
			}

			// Token: 0x17000F66 RID: 3942
			// (get) Token: 0x060043FF RID: 17407 RVA: 0x0011DE28 File Offset: 0x0011C028
			public object Key
			{
				get
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)this.arrayEnumerator.Current;
					return dictionaryEntry.Key;
				}
			}

			// Token: 0x17000F67 RID: 3943
			// (get) Token: 0x06004400 RID: 17408 RVA: 0x0011DE50 File Offset: 0x0011C050
			public object Value
			{
				get
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)this.arrayEnumerator.Current;
					return dictionaryEntry.Value;
				}
			}

			// Token: 0x06004401 RID: 17409 RVA: 0x0011DE75 File Offset: 0x0011C075
			public bool MoveNext()
			{
				return this.arrayEnumerator.MoveNext();
			}

			// Token: 0x06004402 RID: 17410 RVA: 0x0011DE82 File Offset: 0x0011C082
			public void Reset()
			{
				this.arrayEnumerator.Reset();
			}

			// Token: 0x040034FC RID: 13564
			private int _objectReturnType;

			// Token: 0x040034FD RID: 13565
			internal const int Keys = 1;

			// Token: 0x040034FE RID: 13566
			internal const int Values = 2;

			// Token: 0x040034FF RID: 13567
			internal const int DictionaryEntry = 3;

			// Token: 0x04003500 RID: 13568
			private IEnumerator arrayEnumerator;
		}

		// Token: 0x020007EF RID: 2031
		private class OrderedDictionaryKeyValueCollection : ICollection, IEnumerable
		{
			// Token: 0x06004403 RID: 17411 RVA: 0x0011DE8F File Offset: 0x0011C08F
			public OrderedDictionaryKeyValueCollection(ArrayList array, bool isKeys)
			{
				this._objects = array;
				this.isKeys = isKeys;
			}

			// Token: 0x06004404 RID: 17412 RVA: 0x0011DEA8 File Offset: 0x0011C0A8
			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				foreach (object obj in this._objects)
				{
					array.SetValue(this.isKeys ? ((DictionaryEntry)obj).Key : ((DictionaryEntry)obj).Value, index);
					index++;
				}
			}

			// Token: 0x17000F68 RID: 3944
			// (get) Token: 0x06004405 RID: 17413 RVA: 0x0011DF44 File Offset: 0x0011C144
			int ICollection.Count
			{
				get
				{
					return this._objects.Count;
				}
			}

			// Token: 0x17000F69 RID: 3945
			// (get) Token: 0x06004406 RID: 17414 RVA: 0x0011DF51 File Offset: 0x0011C151
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000F6A RID: 3946
			// (get) Token: 0x06004407 RID: 17415 RVA: 0x0011DF54 File Offset: 0x0011C154
			object ICollection.SyncRoot
			{
				get
				{
					return this._objects.SyncRoot;
				}
			}

			// Token: 0x06004408 RID: 17416 RVA: 0x0011DF61 File Offset: 0x0011C161
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new OrderedDictionary.OrderedDictionaryEnumerator(this._objects, this.isKeys ? 1 : 2);
			}

			// Token: 0x04003501 RID: 13569
			private ArrayList _objects;

			// Token: 0x04003502 RID: 13570
			private bool isKeys;
		}
	}
}
