using System;
using System.Threading;

namespace System.Collections.Specialized
{
	// Token: 0x020003AD RID: 941
	[Serializable]
	public class ListDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06002314 RID: 8980 RVA: 0x000A6683 File Offset: 0x000A4883
		public ListDictionary()
		{
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x000A668B File Offset: 0x000A488B
		public ListDictionary(IComparer comparer)
		{
			this.comparer = comparer;
		}

		// Token: 0x170008E3 RID: 2275
		public object this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
				}
				ListDictionary.DictionaryNode dictionaryNode = this.head;
				if (this.comparer == null)
				{
					while (dictionaryNode != null)
					{
						object key2 = dictionaryNode.key;
						if (key2 != null && key2.Equals(key))
						{
							return dictionaryNode.value;
						}
						dictionaryNode = dictionaryNode.next;
					}
				}
				else
				{
					while (dictionaryNode != null)
					{
						object key3 = dictionaryNode.key;
						if (key3 != null && this.comparer.Compare(key3, key) == 0)
						{
							return dictionaryNode.value;
						}
						dictionaryNode = dictionaryNode.next;
					}
				}
				return null;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
				}
				this.version++;
				ListDictionary.DictionaryNode dictionaryNode = null;
				ListDictionary.DictionaryNode next;
				for (next = this.head; next != null; next = next.next)
				{
					object key2 = next.key;
					if ((this.comparer == null) ? key2.Equals(key) : (this.comparer.Compare(key2, key) == 0))
					{
						break;
					}
					dictionaryNode = next;
				}
				if (next != null)
				{
					next.value = value;
					return;
				}
				ListDictionary.DictionaryNode dictionaryNode2 = new ListDictionary.DictionaryNode();
				dictionaryNode2.key = key;
				dictionaryNode2.value = value;
				if (dictionaryNode != null)
				{
					dictionaryNode.next = dictionaryNode2;
				}
				else
				{
					this.head = dictionaryNode2;
				}
				this.count++;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x000A67D8 File Offset: 0x000A49D8
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06002319 RID: 8985 RVA: 0x000A67E0 File Offset: 0x000A49E0
		public ICollection Keys
		{
			get
			{
				return new ListDictionary.NodeKeyValueCollection(this, true);
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x0600231A RID: 8986 RVA: 0x000A67E9 File Offset: 0x000A49E9
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x0600231B RID: 8987 RVA: 0x000A67EC File Offset: 0x000A49EC
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x0600231C RID: 8988 RVA: 0x000A67EF File Offset: 0x000A49EF
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x0600231D RID: 8989 RVA: 0x000A67F2 File Offset: 0x000A49F2
		public object SyncRoot
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

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x0600231E RID: 8990 RVA: 0x000A6814 File Offset: 0x000A4A14
		public ICollection Values
		{
			get
			{
				return new ListDictionary.NodeKeyValueCollection(this, false);
			}
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x000A6820 File Offset: 0x000A4A20
		public void Add(object key, object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
			}
			this.version++;
			ListDictionary.DictionaryNode dictionaryNode = null;
			for (ListDictionary.DictionaryNode next = this.head; next != null; next = next.next)
			{
				object key2 = next.key;
				if ((this.comparer == null) ? key2.Equals(key) : (this.comparer.Compare(key2, key) == 0))
				{
					throw new ArgumentException(SR.GetString("Argument_AddingDuplicate"));
				}
				dictionaryNode = next;
			}
			ListDictionary.DictionaryNode dictionaryNode2 = new ListDictionary.DictionaryNode();
			dictionaryNode2.key = key;
			dictionaryNode2.value = value;
			if (dictionaryNode != null)
			{
				dictionaryNode.next = dictionaryNode2;
			}
			else
			{
				this.head = dictionaryNode2;
			}
			this.count++;
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x000A68D9 File Offset: 0x000A4AD9
		public void Clear()
		{
			this.count = 0;
			this.head = null;
			this.version++;
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x000A68F8 File Offset: 0x000A4AF8
		public bool Contains(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
			}
			for (ListDictionary.DictionaryNode next = this.head; next != null; next = next.next)
			{
				object key2 = next.key;
				if ((this.comparer == null) ? key2.Equals(key) : (this.comparer.Compare(key2, key) == 0))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x000A6960 File Offset: 0x000A4B60
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - index < this.count)
			{
				throw new ArgumentException(SR.GetString("Arg_InsufficientSpace"));
			}
			for (ListDictionary.DictionaryNode next = this.head; next != null; next = next.next)
			{
				array.SetValue(new DictionaryEntry(next.key, next.value), index);
				index++;
			}
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x000A69E9 File Offset: 0x000A4BE9
		public IDictionaryEnumerator GetEnumerator()
		{
			return new ListDictionary.NodeEnumerator(this);
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x000A69F1 File Offset: 0x000A4BF1
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new ListDictionary.NodeEnumerator(this);
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x000A69FC File Offset: 0x000A4BFC
		public void Remove(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
			}
			this.version++;
			ListDictionary.DictionaryNode dictionaryNode = null;
			ListDictionary.DictionaryNode next;
			for (next = this.head; next != null; next = next.next)
			{
				object key2 = next.key;
				if ((this.comparer == null) ? key2.Equals(key) : (this.comparer.Compare(key2, key) == 0))
				{
					break;
				}
				dictionaryNode = next;
			}
			if (next == null)
			{
				return;
			}
			if (next == this.head)
			{
				this.head = next.next;
			}
			else
			{
				dictionaryNode.next = next.next;
			}
			this.count--;
		}

		// Token: 0x04001FB3 RID: 8115
		private ListDictionary.DictionaryNode head;

		// Token: 0x04001FB4 RID: 8116
		private int version;

		// Token: 0x04001FB5 RID: 8117
		private int count;

		// Token: 0x04001FB6 RID: 8118
		private IComparer comparer;

		// Token: 0x04001FB7 RID: 8119
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x020007E8 RID: 2024
		private class NodeEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x060043E1 RID: 17377 RVA: 0x0011D923 File Offset: 0x0011BB23
			public NodeEnumerator(ListDictionary list)
			{
				this.list = list;
				this.version = list.version;
				this.start = true;
				this.current = null;
			}

			// Token: 0x17000F58 RID: 3928
			// (get) Token: 0x060043E2 RID: 17378 RVA: 0x0011D94C File Offset: 0x0011BB4C
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x17000F59 RID: 3929
			// (get) Token: 0x060043E3 RID: 17379 RVA: 0x0011D959 File Offset: 0x0011BB59
			public DictionaryEntry Entry
			{
				get
				{
					if (this.current == null)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					return new DictionaryEntry(this.current.key, this.current.value);
				}
			}

			// Token: 0x17000F5A RID: 3930
			// (get) Token: 0x060043E4 RID: 17380 RVA: 0x0011D98E File Offset: 0x0011BB8E
			public object Key
			{
				get
				{
					if (this.current == null)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.current.key;
				}
			}

			// Token: 0x17000F5B RID: 3931
			// (get) Token: 0x060043E5 RID: 17381 RVA: 0x0011D9B3 File Offset: 0x0011BBB3
			public object Value
			{
				get
				{
					if (this.current == null)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.current.value;
				}
			}

			// Token: 0x060043E6 RID: 17382 RVA: 0x0011D9D8 File Offset: 0x0011BBD8
			public bool MoveNext()
			{
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				if (this.start)
				{
					this.current = this.list.head;
					this.start = false;
				}
				else if (this.current != null)
				{
					this.current = this.current.next;
				}
				return this.current != null;
			}

			// Token: 0x060043E7 RID: 17383 RVA: 0x0011DA4C File Offset: 0x0011BC4C
			public void Reset()
			{
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				this.start = true;
				this.current = null;
			}

			// Token: 0x040034ED RID: 13549
			private ListDictionary list;

			// Token: 0x040034EE RID: 13550
			private ListDictionary.DictionaryNode current;

			// Token: 0x040034EF RID: 13551
			private int version;

			// Token: 0x040034F0 RID: 13552
			private bool start;
		}

		// Token: 0x020007E9 RID: 2025
		private class NodeKeyValueCollection : ICollection, IEnumerable
		{
			// Token: 0x060043E8 RID: 17384 RVA: 0x0011DA7F File Offset: 0x0011BC7F
			public NodeKeyValueCollection(ListDictionary list, bool isKeys)
			{
				this.list = list;
				this.isKeys = isKeys;
			}

			// Token: 0x060043E9 RID: 17385 RVA: 0x0011DA98 File Offset: 0x0011BC98
			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				for (ListDictionary.DictionaryNode dictionaryNode = this.list.head; dictionaryNode != null; dictionaryNode = dictionaryNode.next)
				{
					array.SetValue(this.isKeys ? dictionaryNode.key : dictionaryNode.value, index);
					index++;
				}
			}

			// Token: 0x17000F5C RID: 3932
			// (get) Token: 0x060043EA RID: 17386 RVA: 0x0011DB08 File Offset: 0x0011BD08
			int ICollection.Count
			{
				get
				{
					int num = 0;
					for (ListDictionary.DictionaryNode dictionaryNode = this.list.head; dictionaryNode != null; dictionaryNode = dictionaryNode.next)
					{
						num++;
					}
					return num;
				}
			}

			// Token: 0x17000F5D RID: 3933
			// (get) Token: 0x060043EB RID: 17387 RVA: 0x0011DB34 File Offset: 0x0011BD34
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000F5E RID: 3934
			// (get) Token: 0x060043EC RID: 17388 RVA: 0x0011DB37 File Offset: 0x0011BD37
			object ICollection.SyncRoot
			{
				get
				{
					return this.list.SyncRoot;
				}
			}

			// Token: 0x060043ED RID: 17389 RVA: 0x0011DB44 File Offset: 0x0011BD44
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new ListDictionary.NodeKeyValueCollection.NodeKeyValueEnumerator(this.list, this.isKeys);
			}

			// Token: 0x040034F1 RID: 13553
			private ListDictionary list;

			// Token: 0x040034F2 RID: 13554
			private bool isKeys;

			// Token: 0x02000925 RID: 2341
			private class NodeKeyValueEnumerator : IEnumerator
			{
				// Token: 0x06004672 RID: 18034 RVA: 0x001264D9 File Offset: 0x001246D9
				public NodeKeyValueEnumerator(ListDictionary list, bool isKeys)
				{
					this.list = list;
					this.isKeys = isKeys;
					this.version = list.version;
					this.start = true;
					this.current = null;
				}

				// Token: 0x17000FE2 RID: 4066
				// (get) Token: 0x06004673 RID: 18035 RVA: 0x00126509 File Offset: 0x00124709
				public object Current
				{
					get
					{
						if (this.current == null)
						{
							throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
						}
						if (!this.isKeys)
						{
							return this.current.value;
						}
						return this.current.key;
					}
				}

				// Token: 0x06004674 RID: 18036 RVA: 0x00126544 File Offset: 0x00124744
				public bool MoveNext()
				{
					if (this.version != this.list.version)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
					}
					if (this.start)
					{
						this.current = this.list.head;
						this.start = false;
					}
					else if (this.current != null)
					{
						this.current = this.current.next;
					}
					return this.current != null;
				}

				// Token: 0x06004675 RID: 18037 RVA: 0x001265B8 File Offset: 0x001247B8
				public void Reset()
				{
					if (this.version != this.list.version)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
					}
					this.start = true;
					this.current = null;
				}

				// Token: 0x04003DAE RID: 15790
				private ListDictionary list;

				// Token: 0x04003DAF RID: 15791
				private ListDictionary.DictionaryNode current;

				// Token: 0x04003DB0 RID: 15792
				private int version;

				// Token: 0x04003DB1 RID: 15793
				private bool isKeys;

				// Token: 0x04003DB2 RID: 15794
				private bool start;
			}
		}

		// Token: 0x020007EA RID: 2026
		[Serializable]
		private class DictionaryNode
		{
			// Token: 0x040034F3 RID: 13555
			public object key;

			// Token: 0x040034F4 RID: 13556
			public object value;

			// Token: 0x040034F5 RID: 13557
			public ListDictionary.DictionaryNode next;
		}
	}
}
