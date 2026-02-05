using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace System.Collections.Specialized
{
	// Token: 0x020003B0 RID: 944
	[global::__DynamicallyInvokable]
	[Serializable]
	public class NameValueCollection : NameObjectCollectionBase
	{
		// Token: 0x06002352 RID: 9042 RVA: 0x000A7651 File Offset: 0x000A5851
		public NameValueCollection()
		{
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x000A7659 File Offset: 0x000A5859
		public NameValueCollection(NameValueCollection col)
			: base((col != null) ? col.Comparer : null)
		{
			this.Add(col);
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x000A7674 File Offset: 0x000A5874
		[Obsolete("Please use NameValueCollection(IEqualityComparer) instead.")]
		public NameValueCollection(IHashCodeProvider hashProvider, IComparer comparer)
			: base(hashProvider, comparer)
		{
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x000A767E File Offset: 0x000A587E
		public NameValueCollection(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x000A7687 File Offset: 0x000A5887
		public NameValueCollection(IEqualityComparer equalityComparer)
			: base(equalityComparer)
		{
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x000A7690 File Offset: 0x000A5890
		public NameValueCollection(int capacity, IEqualityComparer equalityComparer)
			: base(capacity, equalityComparer)
		{
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x000A769A File Offset: 0x000A589A
		public NameValueCollection(int capacity, NameValueCollection col)
			: base(capacity, (col != null) ? col.Comparer : null)
		{
			if (col == null)
			{
				throw new ArgumentNullException("col");
			}
			base.Comparer = col.Comparer;
			this.Add(col);
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x000A76D0 File Offset: 0x000A58D0
		[Obsolete("Please use NameValueCollection(Int32, IEqualityComparer) instead.")]
		public NameValueCollection(int capacity, IHashCodeProvider hashProvider, IComparer comparer)
			: base(capacity, hashProvider, comparer)
		{
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x000A76DB File Offset: 0x000A58DB
		internal NameValueCollection(DBNull dummy)
			: base(dummy)
		{
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x000A76E4 File Offset: 0x000A58E4
		protected NameValueCollection(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x000A76EE File Offset: 0x000A58EE
		protected void InvalidateCachedArrays()
		{
			this._all = null;
			this._allKeys = null;
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x000A7700 File Offset: 0x000A5900
		private static string GetAsOneString(ArrayList list)
		{
			int num = ((list != null) ? list.Count : 0);
			if (num == 1)
			{
				return (string)list[0];
			}
			if (num > 1)
			{
				StringBuilder stringBuilder = new StringBuilder((string)list[0]);
				for (int i = 1; i < num; i++)
				{
					stringBuilder.Append(',');
					stringBuilder.Append((string)list[i]);
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x000A7774 File Offset: 0x000A5974
		private static string[] GetAsStringArray(ArrayList list)
		{
			int num = ((list != null) ? list.Count : 0);
			if (num == 0)
			{
				return null;
			}
			string[] array = new string[num];
			list.CopyTo(0, array, 0, num);
			return array;
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x000A77A8 File Offset: 0x000A59A8
		public void Add(NameValueCollection c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			this.InvalidateCachedArrays();
			int count = c.Count;
			for (int i = 0; i < count; i++)
			{
				string key = c.GetKey(i);
				string[] values = c.GetValues(i);
				if (values != null)
				{
					for (int j = 0; j < values.Length; j++)
					{
						this.Add(key, values[j]);
					}
				}
				else
				{
					this.Add(key, null);
				}
			}
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x000A7816 File Offset: 0x000A5A16
		public virtual void Clear()
		{
			if (base.IsReadOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			this.InvalidateCachedArrays();
			base.BaseClear();
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x000A783C File Offset: 0x000A5A3C
		public void CopyTo(Array dest, int index)
		{
			if (dest == null)
			{
				throw new ArgumentNullException("dest");
			}
			if (dest.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_MultiRank"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[] { index.ToString(CultureInfo.CurrentCulture) }));
			}
			if (dest.Length - index < this.Count)
			{
				throw new ArgumentException(SR.GetString("Arg_InsufficientSpace"));
			}
			int count = this.Count;
			if (this._all == null)
			{
				string[] array = new string[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = this.Get(i);
					dest.SetValue(array[i], i + index);
				}
				this._all = array;
				return;
			}
			for (int j = 0; j < count; j++)
			{
				dest.SetValue(this._all[j], j + index);
			}
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x000A791B File Offset: 0x000A5B1B
		public bool HasKeys()
		{
			return this.InternalHasKeys();
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x000A7923 File Offset: 0x000A5B23
		internal virtual bool InternalHasKeys()
		{
			return base.BaseHasKeys();
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x000A792C File Offset: 0x000A5B2C
		public virtual void Add(string name, string value)
		{
			if (base.IsReadOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			this.InvalidateCachedArrays();
			ArrayList arrayList = (ArrayList)base.BaseGet(name);
			if (arrayList == null)
			{
				arrayList = new ArrayList(1);
				if (value != null)
				{
					arrayList.Add(value);
				}
				base.BaseAdd(name, arrayList);
				return;
			}
			if (value != null)
			{
				arrayList.Add(value);
			}
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x000A7990 File Offset: 0x000A5B90
		public virtual string Get(string name)
		{
			ArrayList arrayList = (ArrayList)base.BaseGet(name);
			return NameValueCollection.GetAsOneString(arrayList);
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x000A79B0 File Offset: 0x000A5BB0
		public virtual string[] GetValues(string name)
		{
			ArrayList arrayList = (ArrayList)base.BaseGet(name);
			return NameValueCollection.GetAsStringArray(arrayList);
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x000A79D0 File Offset: 0x000A5BD0
		public virtual void Set(string name, string value)
		{
			if (base.IsReadOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			this.InvalidateCachedArrays();
			base.BaseSet(name, new ArrayList(1) { value });
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x000A7A12 File Offset: 0x000A5C12
		public virtual void Remove(string name)
		{
			this.InvalidateCachedArrays();
			base.BaseRemove(name);
		}

		// Token: 0x170008F5 RID: 2293
		public string this[string name]
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.Get(name);
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.Set(name, value);
			}
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x000A7A34 File Offset: 0x000A5C34
		public virtual string Get(int index)
		{
			ArrayList arrayList = (ArrayList)base.BaseGet(index);
			return NameValueCollection.GetAsOneString(arrayList);
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x000A7A54 File Offset: 0x000A5C54
		public virtual string[] GetValues(int index)
		{
			ArrayList arrayList = (ArrayList)base.BaseGet(index);
			return NameValueCollection.GetAsStringArray(arrayList);
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x000A7A74 File Offset: 0x000A5C74
		public virtual string GetKey(int index)
		{
			return base.BaseGetKey(index);
		}

		// Token: 0x170008F6 RID: 2294
		public string this[int index]
		{
			get
			{
				return this.Get(index);
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x0600236F RID: 9071 RVA: 0x000A7A86 File Offset: 0x000A5C86
		public virtual string[] AllKeys
		{
			get
			{
				if (this._allKeys == null)
				{
					this._allKeys = base.BaseGetAllKeys();
				}
				return this._allKeys;
			}
		}

		// Token: 0x04001FCE RID: 8142
		private string[] _all;

		// Token: 0x04001FCF RID: 8143
		private string[] _allKeys;
	}
}
