using System;

namespace System.Collections.Specialized
{
	// Token: 0x020003AA RID: 938
	[Serializable]
	public class HybridDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x060022F7 RID: 8951 RVA: 0x000A6210 File Offset: 0x000A4410
		public HybridDictionary()
		{
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x000A6218 File Offset: 0x000A4418
		public HybridDictionary(int initialSize)
			: this(initialSize, false)
		{
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x000A6222 File Offset: 0x000A4422
		public HybridDictionary(bool caseInsensitive)
		{
			this.caseInsensitive = caseInsensitive;
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x000A6231 File Offset: 0x000A4431
		public HybridDictionary(int initialSize, bool caseInsensitive)
		{
			this.caseInsensitive = caseInsensitive;
			if (initialSize >= 6)
			{
				if (caseInsensitive)
				{
					this.hashtable = new Hashtable(initialSize, StringComparer.OrdinalIgnoreCase);
					return;
				}
				this.hashtable = new Hashtable(initialSize);
			}
		}

		// Token: 0x170008D9 RID: 2265
		public object this[object key]
		{
			get
			{
				ListDictionary listDictionary = this.list;
				if (this.hashtable != null)
				{
					return this.hashtable[key];
				}
				if (listDictionary != null)
				{
					return listDictionary[key];
				}
				if (key == null)
				{
					throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
				}
				return null;
			}
			set
			{
				if (this.hashtable != null)
				{
					this.hashtable[key] = value;
					return;
				}
				if (this.list == null)
				{
					this.list = new ListDictionary(this.caseInsensitive ? StringComparer.OrdinalIgnoreCase : null);
					this.list[key] = value;
					return;
				}
				if (this.list.Count >= 8)
				{
					this.ChangeOver();
					this.hashtable[key] = value;
					return;
				}
				this.list[key] = value;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x060022FD RID: 8957 RVA: 0x000A633B File Offset: 0x000A453B
		private ListDictionary List
		{
			get
			{
				if (this.list == null)
				{
					this.list = new ListDictionary(this.caseInsensitive ? StringComparer.OrdinalIgnoreCase : null);
				}
				return this.list;
			}
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x000A6368 File Offset: 0x000A4568
		private void ChangeOver()
		{
			IDictionaryEnumerator enumerator = this.list.GetEnumerator();
			Hashtable hashtable;
			if (this.caseInsensitive)
			{
				hashtable = new Hashtable(13, StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				hashtable = new Hashtable(13);
			}
			while (enumerator.MoveNext())
			{
				hashtable.Add(enumerator.Key, enumerator.Value);
			}
			this.hashtable = hashtable;
			this.list = null;
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x060022FF RID: 8959 RVA: 0x000A63CC File Offset: 0x000A45CC
		public int Count
		{
			get
			{
				ListDictionary listDictionary = this.list;
				if (this.hashtable != null)
				{
					return this.hashtable.Count;
				}
				if (listDictionary != null)
				{
					return listDictionary.Count;
				}
				return 0;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06002300 RID: 8960 RVA: 0x000A63FF File Offset: 0x000A45FF
		public ICollection Keys
		{
			get
			{
				if (this.hashtable != null)
				{
					return this.hashtable.Keys;
				}
				return this.List.Keys;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06002301 RID: 8961 RVA: 0x000A6420 File Offset: 0x000A4620
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06002302 RID: 8962 RVA: 0x000A6423 File Offset: 0x000A4623
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06002303 RID: 8963 RVA: 0x000A6426 File Offset: 0x000A4626
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x000A6429 File Offset: 0x000A4629
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06002305 RID: 8965 RVA: 0x000A642C File Offset: 0x000A462C
		public ICollection Values
		{
			get
			{
				if (this.hashtable != null)
				{
					return this.hashtable.Values;
				}
				return this.List.Values;
			}
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x000A6450 File Offset: 0x000A4650
		public void Add(object key, object value)
		{
			if (this.hashtable != null)
			{
				this.hashtable.Add(key, value);
				return;
			}
			if (this.list == null)
			{
				this.list = new ListDictionary(this.caseInsensitive ? StringComparer.OrdinalIgnoreCase : null);
				this.list.Add(key, value);
				return;
			}
			if (this.list.Count + 1 >= 9)
			{
				this.ChangeOver();
				this.hashtable.Add(key, value);
				return;
			}
			this.list.Add(key, value);
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x000A64D8 File Offset: 0x000A46D8
		public void Clear()
		{
			if (this.hashtable != null)
			{
				Hashtable hashtable = this.hashtable;
				this.hashtable = null;
				hashtable.Clear();
			}
			if (this.list != null)
			{
				ListDictionary listDictionary = this.list;
				this.list = null;
				listDictionary.Clear();
			}
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x000A6520 File Offset: 0x000A4720
		public bool Contains(object key)
		{
			ListDictionary listDictionary = this.list;
			if (this.hashtable != null)
			{
				return this.hashtable.Contains(key);
			}
			if (listDictionary != null)
			{
				return listDictionary.Contains(key);
			}
			if (key == null)
			{
				throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
			}
			return false;
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x000A656D File Offset: 0x000A476D
		public void CopyTo(Array array, int index)
		{
			if (this.hashtable != null)
			{
				this.hashtable.CopyTo(array, index);
				return;
			}
			this.List.CopyTo(array, index);
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x000A6594 File Offset: 0x000A4794
		public IDictionaryEnumerator GetEnumerator()
		{
			if (this.hashtable != null)
			{
				return this.hashtable.GetEnumerator();
			}
			if (this.list == null)
			{
				this.list = new ListDictionary(this.caseInsensitive ? StringComparer.OrdinalIgnoreCase : null);
			}
			return this.list.GetEnumerator();
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x000A65E4 File Offset: 0x000A47E4
		IEnumerator IEnumerable.GetEnumerator()
		{
			if (this.hashtable != null)
			{
				return this.hashtable.GetEnumerator();
			}
			if (this.list == null)
			{
				this.list = new ListDictionary(this.caseInsensitive ? StringComparer.OrdinalIgnoreCase : null);
			}
			return this.list.GetEnumerator();
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x000A6634 File Offset: 0x000A4834
		public void Remove(object key)
		{
			if (this.hashtable != null)
			{
				this.hashtable.Remove(key);
				return;
			}
			if (this.list != null)
			{
				this.list.Remove(key);
				return;
			}
			if (key == null)
			{
				throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
			}
		}

		// Token: 0x04001FAD RID: 8109
		private const int CutoverPoint = 9;

		// Token: 0x04001FAE RID: 8110
		private const int InitialHashtableSize = 13;

		// Token: 0x04001FAF RID: 8111
		private const int FixedSizeCutoverPoint = 6;

		// Token: 0x04001FB0 RID: 8112
		private ListDictionary list;

		// Token: 0x04001FB1 RID: 8113
		private Hashtable hashtable;

		// Token: 0x04001FB2 RID: 8114
		private bool caseInsensitive;
	}
}
