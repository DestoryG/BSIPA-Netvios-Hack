using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Specialized
{
	// Token: 0x020003AE RID: 942
	[Serializable]
	public abstract class NameObjectCollectionBase : ICollection, IEnumerable, ISerializable, IDeserializationCallback
	{
		// Token: 0x06002326 RID: 8998 RVA: 0x000A6AA5 File Offset: 0x000A4CA5
		protected NameObjectCollectionBase()
			: this(NameObjectCollectionBase.defaultComparer)
		{
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x000A6AB4 File Offset: 0x000A4CB4
		protected NameObjectCollectionBase(IEqualityComparer equalityComparer)
		{
			IEqualityComparer equalityComparer2;
			if (equalityComparer != null)
			{
				equalityComparer2 = equalityComparer;
			}
			else
			{
				IEqualityComparer equalityComparer3 = NameObjectCollectionBase.defaultComparer;
				equalityComparer2 = equalityComparer3;
			}
			this._keyComparer = equalityComparer2;
			this.Reset();
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x000A6AE0 File Offset: 0x000A4CE0
		protected NameObjectCollectionBase(int capacity, IEqualityComparer equalityComparer)
			: this(equalityComparer)
		{
			this.Reset(capacity);
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x000A6AF0 File Offset: 0x000A4CF0
		[Obsolete("Please use NameObjectCollectionBase(IEqualityComparer) instead.")]
		protected NameObjectCollectionBase(IHashCodeProvider hashProvider, IComparer comparer)
		{
			this._keyComparer = new CompatibleComparer(comparer, hashProvider);
			this.Reset();
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x000A6B0B File Offset: 0x000A4D0B
		[Obsolete("Please use NameObjectCollectionBase(Int32, IEqualityComparer) instead.")]
		protected NameObjectCollectionBase(int capacity, IHashCodeProvider hashProvider, IComparer comparer)
		{
			this._keyComparer = new CompatibleComparer(comparer, hashProvider);
			this.Reset(capacity);
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x000A6B27 File Offset: 0x000A4D27
		protected NameObjectCollectionBase(int capacity)
		{
			this._keyComparer = StringComparer.InvariantCultureIgnoreCase;
			this.Reset(capacity);
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x000A6B41 File Offset: 0x000A4D41
		internal NameObjectCollectionBase(DBNull dummy)
		{
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x000A6B49 File Offset: 0x000A4D49
		protected NameObjectCollectionBase(SerializationInfo info, StreamingContext context)
		{
			this._serializationInfo = info;
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x000A6B58 File Offset: 0x000A4D58
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("ReadOnly", this._readOnly);
			if (this._keyComparer == NameObjectCollectionBase.defaultComparer)
			{
				info.AddValue("HashProvider", CompatibleComparer.DefaultHashCodeProvider, typeof(IHashCodeProvider));
				info.AddValue("Comparer", CompatibleComparer.DefaultComparer, typeof(IComparer));
			}
			else if (this._keyComparer == null)
			{
				info.AddValue("HashProvider", null, typeof(IHashCodeProvider));
				info.AddValue("Comparer", null, typeof(IComparer));
			}
			else if (this._keyComparer is CompatibleComparer)
			{
				CompatibleComparer compatibleComparer = (CompatibleComparer)this._keyComparer;
				info.AddValue("HashProvider", compatibleComparer.HashCodeProvider, typeof(IHashCodeProvider));
				info.AddValue("Comparer", compatibleComparer.Comparer, typeof(IComparer));
			}
			else
			{
				info.AddValue("KeyComparer", this._keyComparer, typeof(IEqualityComparer));
			}
			int count = this._entriesArray.Count;
			info.AddValue("Count", count);
			string[] array = new string[count];
			object[] array2 = new object[count];
			for (int i = 0; i < count; i++)
			{
				NameObjectCollectionBase.NameObjectEntry nameObjectEntry = (NameObjectCollectionBase.NameObjectEntry)this._entriesArray[i];
				array[i] = nameObjectEntry.Key;
				array2[i] = nameObjectEntry.Value;
			}
			info.AddValue("Keys", array, typeof(string[]));
			info.AddValue("Values", array2, typeof(object[]));
			info.AddValue("Version", this._version);
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x000A6D0C File Offset: 0x000A4F0C
		public virtual void OnDeserialization(object sender)
		{
			if (this._keyComparer != null)
			{
				return;
			}
			if (this._serializationInfo == null)
			{
				throw new SerializationException();
			}
			SerializationInfo serializationInfo = this._serializationInfo;
			this._serializationInfo = null;
			bool flag = false;
			int num = 0;
			string[] array = null;
			object[] array2 = null;
			IHashCodeProvider hashCodeProvider = null;
			IComparer comparer = null;
			bool flag2 = false;
			int num2 = 0;
			SerializationInfoEnumerator enumerator = serializationInfo.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name = enumerator.Name;
				uint num3 = global::<PrivateImplementationDetails>.ComputeStringHash(name);
				if (num3 <= 1573770551U)
				{
					if (num3 <= 1202781175U)
					{
						if (num3 != 891156946U)
						{
							if (num3 == 1202781175U)
							{
								if (name == "ReadOnly")
								{
									flag = serializationInfo.GetBoolean("ReadOnly");
								}
							}
						}
						else if (name == "Comparer")
						{
							comparer = (IComparer)serializationInfo.GetValue("Comparer", typeof(IComparer));
						}
					}
					else if (num3 != 1228509323U)
					{
						if (num3 == 1573770551U)
						{
							if (name == "Version")
							{
								flag2 = true;
								num2 = serializationInfo.GetInt32("Version");
							}
						}
					}
					else if (name == "KeyComparer")
					{
						this._keyComparer = (IEqualityComparer)serializationInfo.GetValue("KeyComparer", typeof(IEqualityComparer));
					}
				}
				else if (num3 <= 1944240600U)
				{
					if (num3 != 1613443821U)
					{
						if (num3 == 1944240600U)
						{
							if (name == "HashProvider")
							{
								hashCodeProvider = (IHashCodeProvider)serializationInfo.GetValue("HashProvider", typeof(IHashCodeProvider));
							}
						}
					}
					else if (name == "Keys")
					{
						array = (string[])serializationInfo.GetValue("Keys", typeof(string[]));
					}
				}
				else if (num3 != 2370642523U)
				{
					if (num3 == 3790059668U)
					{
						if (name == "Count")
						{
							num = serializationInfo.GetInt32("Count");
						}
					}
				}
				else if (name == "Values")
				{
					array2 = (object[])serializationInfo.GetValue("Values", typeof(object[]));
				}
			}
			if (this._keyComparer == null)
			{
				if (comparer == null || hashCodeProvider == null)
				{
					throw new SerializationException();
				}
				this._keyComparer = new CompatibleComparer(comparer, hashCodeProvider);
			}
			if (array == null || array2 == null)
			{
				throw new SerializationException();
			}
			this.Reset(num);
			for (int i = 0; i < num; i++)
			{
				this.BaseAdd(array[i], array2[i]);
			}
			this._readOnly = flag;
			if (flag2)
			{
				this._version = num2;
			}
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x000A6FEA File Offset: 0x000A51EA
		private void Reset()
		{
			this._entriesArray = new ArrayList();
			this._entriesTable = new Hashtable(this._keyComparer);
			this._nullKeyEntry = null;
			this._version++;
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x000A7021 File Offset: 0x000A5221
		private void Reset(int capacity)
		{
			this._entriesArray = new ArrayList(capacity);
			this._entriesTable = new Hashtable(capacity, this._keyComparer);
			this._nullKeyEntry = null;
			this._version++;
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x000A705A File Offset: 0x000A525A
		private NameObjectCollectionBase.NameObjectEntry FindEntry(string key)
		{
			if (key != null)
			{
				return (NameObjectCollectionBase.NameObjectEntry)this._entriesTable[key];
			}
			return this._nullKeyEntry;
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06002333 RID: 9011 RVA: 0x000A707B File Offset: 0x000A527B
		// (set) Token: 0x06002334 RID: 9012 RVA: 0x000A7083 File Offset: 0x000A5283
		internal IEqualityComparer Comparer
		{
			get
			{
				return this._keyComparer;
			}
			set
			{
				this._keyComparer = value;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06002335 RID: 9013 RVA: 0x000A708C File Offset: 0x000A528C
		// (set) Token: 0x06002336 RID: 9014 RVA: 0x000A7094 File Offset: 0x000A5294
		protected bool IsReadOnly
		{
			get
			{
				return this._readOnly;
			}
			set
			{
				this._readOnly = value;
			}
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x000A709D File Offset: 0x000A529D
		protected bool BaseHasKeys()
		{
			return this._entriesTable.Count > 0;
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x000A70B0 File Offset: 0x000A52B0
		protected void BaseAdd(string name, object value)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			NameObjectCollectionBase.NameObjectEntry nameObjectEntry = new NameObjectCollectionBase.NameObjectEntry(name, value);
			if (name != null)
			{
				if (this._entriesTable[name] == null)
				{
					this._entriesTable.Add(name, nameObjectEntry);
				}
			}
			else if (this._nullKeyEntry == null)
			{
				this._nullKeyEntry = nameObjectEntry;
			}
			this._entriesArray.Add(nameObjectEntry);
			this._version++;
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x000A7130 File Offset: 0x000A5330
		protected void BaseRemove(string name)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			if (name != null)
			{
				this._entriesTable.Remove(name);
				for (int i = this._entriesArray.Count - 1; i >= 0; i--)
				{
					if (this._keyComparer.Equals(name, this.BaseGetKey(i)))
					{
						this._entriesArray.RemoveAt(i);
					}
				}
			}
			else
			{
				this._nullKeyEntry = null;
				for (int j = this._entriesArray.Count - 1; j >= 0; j--)
				{
					if (this.BaseGetKey(j) == null)
					{
						this._entriesArray.RemoveAt(j);
					}
				}
			}
			this._version++;
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x000A71E8 File Offset: 0x000A53E8
		protected void BaseRemoveAt(int index)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			string text = this.BaseGetKey(index);
			if (text != null)
			{
				this._entriesTable.Remove(text);
			}
			else
			{
				this._nullKeyEntry = null;
			}
			this._entriesArray.RemoveAt(index);
			this._version++;
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x000A724B File Offset: 0x000A544B
		protected void BaseClear()
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			this.Reset();
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x000A726C File Offset: 0x000A546C
		protected object BaseGet(string name)
		{
			NameObjectCollectionBase.NameObjectEntry nameObjectEntry = this.FindEntry(name);
			if (nameObjectEntry == null)
			{
				return null;
			}
			return nameObjectEntry.Value;
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x000A728C File Offset: 0x000A548C
		protected void BaseSet(string name, object value)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			NameObjectCollectionBase.NameObjectEntry nameObjectEntry = this.FindEntry(name);
			if (nameObjectEntry != null)
			{
				nameObjectEntry.Value = value;
				this._version++;
				return;
			}
			this.BaseAdd(name, value);
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x000A72DC File Offset: 0x000A54DC
		protected object BaseGet(int index)
		{
			NameObjectCollectionBase.NameObjectEntry nameObjectEntry = (NameObjectCollectionBase.NameObjectEntry)this._entriesArray[index];
			return nameObjectEntry.Value;
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x000A7304 File Offset: 0x000A5504
		protected string BaseGetKey(int index)
		{
			NameObjectCollectionBase.NameObjectEntry nameObjectEntry = (NameObjectCollectionBase.NameObjectEntry)this._entriesArray[index];
			return nameObjectEntry.Key;
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x000A732C File Offset: 0x000A552C
		protected void BaseSet(int index, object value)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			NameObjectCollectionBase.NameObjectEntry nameObjectEntry = (NameObjectCollectionBase.NameObjectEntry)this._entriesArray[index];
			nameObjectEntry.Value = value;
			this._version++;
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x000A7378 File Offset: 0x000A5578
		public virtual IEnumerator GetEnumerator()
		{
			return new NameObjectCollectionBase.NameObjectKeysEnumerator(this);
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06002342 RID: 9026 RVA: 0x000A7380 File Offset: 0x000A5580
		public virtual int Count
		{
			get
			{
				return this._entriesArray.Count;
			}
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x000A7390 File Offset: 0x000A5590
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_MultiRank"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[] { index.ToString(CultureInfo.CurrentCulture) }));
			}
			if (array.Length - index < this._entriesArray.Count)
			{
				throw new ArgumentException(SR.GetString("Arg_InsufficientSpace"));
			}
			foreach (object obj in this)
			{
				array.SetValue(obj, index++);
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06002344 RID: 9028 RVA: 0x000A743A File Offset: 0x000A563A
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

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06002345 RID: 9029 RVA: 0x000A745C File Offset: 0x000A565C
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x000A7460 File Offset: 0x000A5660
		protected string[] BaseGetAllKeys()
		{
			int count = this._entriesArray.Count;
			string[] array = new string[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.BaseGetKey(i);
			}
			return array;
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x000A7498 File Offset: 0x000A5698
		protected object[] BaseGetAllValues()
		{
			int count = this._entriesArray.Count;
			object[] array = new object[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.BaseGet(i);
			}
			return array;
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x000A74D0 File Offset: 0x000A56D0
		protected object[] BaseGetAllValues(Type type)
		{
			int count = this._entriesArray.Count;
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			object[] array = (object[])SecurityUtils.ArrayCreateInstance(type, count);
			for (int i = 0; i < count; i++)
			{
				array[i] = this.BaseGet(i);
			}
			return array;
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06002349 RID: 9033 RVA: 0x000A7521 File Offset: 0x000A5721
		public virtual NameObjectCollectionBase.KeysCollection Keys
		{
			get
			{
				if (this._keys == null)
				{
					this._keys = new NameObjectCollectionBase.KeysCollection(this);
				}
				return this._keys;
			}
		}

		// Token: 0x04001FB8 RID: 8120
		private const string ReadOnlyName = "ReadOnly";

		// Token: 0x04001FB9 RID: 8121
		private const string CountName = "Count";

		// Token: 0x04001FBA RID: 8122
		private const string ComparerName = "Comparer";

		// Token: 0x04001FBB RID: 8123
		private const string HashCodeProviderName = "HashProvider";

		// Token: 0x04001FBC RID: 8124
		private const string KeysName = "Keys";

		// Token: 0x04001FBD RID: 8125
		private const string ValuesName = "Values";

		// Token: 0x04001FBE RID: 8126
		private const string KeyComparerName = "KeyComparer";

		// Token: 0x04001FBF RID: 8127
		private const string VersionName = "Version";

		// Token: 0x04001FC0 RID: 8128
		private bool _readOnly;

		// Token: 0x04001FC1 RID: 8129
		private ArrayList _entriesArray;

		// Token: 0x04001FC2 RID: 8130
		private IEqualityComparer _keyComparer;

		// Token: 0x04001FC3 RID: 8131
		private volatile Hashtable _entriesTable;

		// Token: 0x04001FC4 RID: 8132
		private volatile NameObjectCollectionBase.NameObjectEntry _nullKeyEntry;

		// Token: 0x04001FC5 RID: 8133
		private NameObjectCollectionBase.KeysCollection _keys;

		// Token: 0x04001FC6 RID: 8134
		private SerializationInfo _serializationInfo;

		// Token: 0x04001FC7 RID: 8135
		private int _version;

		// Token: 0x04001FC8 RID: 8136
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x04001FC9 RID: 8137
		private static StringComparer defaultComparer = StringComparer.InvariantCultureIgnoreCase;

		// Token: 0x020007EB RID: 2027
		internal class NameObjectEntry
		{
			// Token: 0x060043EF RID: 17391 RVA: 0x0011DB5F File Offset: 0x0011BD5F
			internal NameObjectEntry(string name, object value)
			{
				this.Key = name;
				this.Value = value;
			}

			// Token: 0x040034F6 RID: 13558
			internal string Key;

			// Token: 0x040034F7 RID: 13559
			internal object Value;
		}

		// Token: 0x020007EC RID: 2028
		[Serializable]
		internal class NameObjectKeysEnumerator : IEnumerator
		{
			// Token: 0x060043F0 RID: 17392 RVA: 0x0011DB75 File Offset: 0x0011BD75
			internal NameObjectKeysEnumerator(NameObjectCollectionBase coll)
			{
				this._coll = coll;
				this._version = this._coll._version;
				this._pos = -1;
			}

			// Token: 0x060043F1 RID: 17393 RVA: 0x0011DB9C File Offset: 0x0011BD9C
			public bool MoveNext()
			{
				if (this._version != this._coll._version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				if (this._pos < this._coll.Count - 1)
				{
					this._pos++;
					return true;
				}
				this._pos = this._coll.Count;
				return false;
			}

			// Token: 0x060043F2 RID: 17394 RVA: 0x0011DC03 File Offset: 0x0011BE03
			public void Reset()
			{
				if (this._version != this._coll._version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				this._pos = -1;
			}

			// Token: 0x17000F5F RID: 3935
			// (get) Token: 0x060043F3 RID: 17395 RVA: 0x0011DC2F File Offset: 0x0011BE2F
			public object Current
			{
				get
				{
					if (this._pos >= 0 && this._pos < this._coll.Count)
					{
						return this._coll.BaseGetKey(this._pos);
					}
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
				}
			}

			// Token: 0x040034F8 RID: 13560
			private int _pos;

			// Token: 0x040034F9 RID: 13561
			private NameObjectCollectionBase _coll;

			// Token: 0x040034FA RID: 13562
			private int _version;
		}

		// Token: 0x020007ED RID: 2029
		[Serializable]
		public class KeysCollection : ICollection, IEnumerable
		{
			// Token: 0x060043F4 RID: 17396 RVA: 0x0011DC6E File Offset: 0x0011BE6E
			internal KeysCollection(NameObjectCollectionBase coll)
			{
				this._coll = coll;
			}

			// Token: 0x060043F5 RID: 17397 RVA: 0x0011DC7D File Offset: 0x0011BE7D
			public virtual string Get(int index)
			{
				return this._coll.BaseGetKey(index);
			}

			// Token: 0x17000F60 RID: 3936
			public string this[int index]
			{
				get
				{
					return this.Get(index);
				}
			}

			// Token: 0x060043F7 RID: 17399 RVA: 0x0011DC94 File Offset: 0x0011BE94
			public IEnumerator GetEnumerator()
			{
				return new NameObjectCollectionBase.NameObjectKeysEnumerator(this._coll);
			}

			// Token: 0x17000F61 RID: 3937
			// (get) Token: 0x060043F8 RID: 17400 RVA: 0x0011DCA1 File Offset: 0x0011BEA1
			public int Count
			{
				get
				{
					return this._coll.Count;
				}
			}

			// Token: 0x060043F9 RID: 17401 RVA: 0x0011DCB0 File Offset: 0x0011BEB0
			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException(SR.GetString("Arg_MultiRank"));
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[] { index.ToString(CultureInfo.CurrentCulture) }));
				}
				if (array.Length - index < this._coll.Count)
				{
					throw new ArgumentException(SR.GetString("Arg_InsufficientSpace"));
				}
				foreach (object obj in this)
				{
					array.SetValue(obj, index++);
				}
			}

			// Token: 0x17000F62 RID: 3938
			// (get) Token: 0x060043FA RID: 17402 RVA: 0x0011DD5A File Offset: 0x0011BF5A
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this._coll).SyncRoot;
				}
			}

			// Token: 0x17000F63 RID: 3939
			// (get) Token: 0x060043FB RID: 17403 RVA: 0x0011DD67 File Offset: 0x0011BF67
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x040034FB RID: 13563
			private NameObjectCollectionBase _coll;
		}
	}
}
