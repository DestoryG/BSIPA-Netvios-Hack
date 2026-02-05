using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Collections.Specialized
{
	// Token: 0x020003B7 RID: 951
	[DesignerSerializer("System.Diagnostics.Design.StringDictionaryCodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Serializable]
	public class StringDictionary : IEnumerable
	{
		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x060023CB RID: 9163 RVA: 0x000A88CF File Offset: 0x000A6ACF
		public virtual int Count
		{
			get
			{
				return this.contents.Count;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x060023CC RID: 9164 RVA: 0x000A88DC File Offset: 0x000A6ADC
		public virtual bool IsSynchronized
		{
			get
			{
				return this.contents.IsSynchronized;
			}
		}

		// Token: 0x17000913 RID: 2323
		public virtual string this[string key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				return (string)this.contents[key.ToLower(CultureInfo.InvariantCulture)];
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				this.contents[key.ToLower(CultureInfo.InvariantCulture)] = value;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x060023CF RID: 9167 RVA: 0x000A893B File Offset: 0x000A6B3B
		public virtual ICollection Keys
		{
			get
			{
				return this.contents.Keys;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x060023D0 RID: 9168 RVA: 0x000A8948 File Offset: 0x000A6B48
		public virtual object SyncRoot
		{
			get
			{
				return this.contents.SyncRoot;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x060023D1 RID: 9169 RVA: 0x000A8955 File Offset: 0x000A6B55
		public virtual ICollection Values
		{
			get
			{
				return this.contents.Values;
			}
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x000A8962 File Offset: 0x000A6B62
		public virtual void Add(string key, string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.contents.Add(key.ToLower(CultureInfo.InvariantCulture), value);
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x000A8989 File Offset: 0x000A6B89
		public virtual void Clear()
		{
			this.contents.Clear();
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x000A8996 File Offset: 0x000A6B96
		public virtual bool ContainsKey(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this.contents.ContainsKey(key.ToLower(CultureInfo.InvariantCulture));
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x000A89BC File Offset: 0x000A6BBC
		public virtual bool ContainsValue(string value)
		{
			return this.contents.ContainsValue(value);
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x000A89CA File Offset: 0x000A6BCA
		public virtual void CopyTo(Array array, int index)
		{
			this.contents.CopyTo(array, index);
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x000A89D9 File Offset: 0x000A6BD9
		public virtual IEnumerator GetEnumerator()
		{
			return this.contents.GetEnumerator();
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x000A89E6 File Offset: 0x000A6BE6
		public virtual void Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.contents.Remove(key.ToLower(CultureInfo.InvariantCulture));
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x000A8A0C File Offset: 0x000A6C0C
		internal void ReplaceHashtable(Hashtable useThisHashtableInstead)
		{
			this.contents = useThisHashtableInstead;
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x000A8A15 File Offset: 0x000A6C15
		internal IDictionary<string, string> AsGenericDictionary()
		{
			return new StringDictionary.GenericAdapter(this);
		}

		// Token: 0x04001FE9 RID: 8169
		internal Hashtable contents = new Hashtable();

		// Token: 0x020007F0 RID: 2032
		private class GenericAdapter : IDictionary<string, string>, ICollection<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<string, string>>, IEnumerable
		{
			// Token: 0x06004409 RID: 17417 RVA: 0x0011DF7A File Offset: 0x0011C17A
			internal GenericAdapter(StringDictionary stringDictionary)
			{
				this.m_stringDictionary = stringDictionary;
			}

			// Token: 0x0600440A RID: 17418 RVA: 0x0011DF89 File Offset: 0x0011C189
			public void Add(string key, string value)
			{
				this[key] = value;
			}

			// Token: 0x0600440B RID: 17419 RVA: 0x0011DF93 File Offset: 0x0011C193
			public bool ContainsKey(string key)
			{
				return this.m_stringDictionary.ContainsKey(key);
			}

			// Token: 0x0600440C RID: 17420 RVA: 0x0011DFA1 File Offset: 0x0011C1A1
			public void Clear()
			{
				this.m_stringDictionary.Clear();
			}

			// Token: 0x17000F6B RID: 3947
			// (get) Token: 0x0600440D RID: 17421 RVA: 0x0011DFAE File Offset: 0x0011C1AE
			public int Count
			{
				get
				{
					return this.m_stringDictionary.Count;
				}
			}

			// Token: 0x17000F6C RID: 3948
			public string this[string key]
			{
				get
				{
					if (key == null)
					{
						throw new ArgumentNullException("key");
					}
					if (!this.m_stringDictionary.ContainsKey(key))
					{
						throw new KeyNotFoundException();
					}
					return this.m_stringDictionary[key];
				}
				set
				{
					if (key == null)
					{
						throw new ArgumentNullException("key");
					}
					this.m_stringDictionary[key] = value;
				}
			}

			// Token: 0x17000F6D RID: 3949
			// (get) Token: 0x06004410 RID: 17424 RVA: 0x0011E008 File Offset: 0x0011C208
			public ICollection<string> Keys
			{
				get
				{
					if (this._keys == null)
					{
						this._keys = new StringDictionary.GenericAdapter.ICollectionToGenericCollectionAdapter(this.m_stringDictionary, StringDictionary.GenericAdapter.KeyOrValue.Key);
					}
					return this._keys;
				}
			}

			// Token: 0x17000F6E RID: 3950
			// (get) Token: 0x06004411 RID: 17425 RVA: 0x0011E02A File Offset: 0x0011C22A
			public ICollection<string> Values
			{
				get
				{
					if (this._values == null)
					{
						this._values = new StringDictionary.GenericAdapter.ICollectionToGenericCollectionAdapter(this.m_stringDictionary, StringDictionary.GenericAdapter.KeyOrValue.Value);
					}
					return this._values;
				}
			}

			// Token: 0x06004412 RID: 17426 RVA: 0x0011E04C File Offset: 0x0011C24C
			public bool Remove(string key)
			{
				if (!this.m_stringDictionary.ContainsKey(key))
				{
					return false;
				}
				this.m_stringDictionary.Remove(key);
				return true;
			}

			// Token: 0x06004413 RID: 17427 RVA: 0x0011E06B File Offset: 0x0011C26B
			public bool TryGetValue(string key, out string value)
			{
				if (!this.m_stringDictionary.ContainsKey(key))
				{
					value = null;
					return false;
				}
				value = this.m_stringDictionary[key];
				return true;
			}

			// Token: 0x06004414 RID: 17428 RVA: 0x0011E08F File Offset: 0x0011C28F
			void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item)
			{
				this.m_stringDictionary.Add(item.Key, item.Value);
			}

			// Token: 0x06004415 RID: 17429 RVA: 0x0011E0AC File Offset: 0x0011C2AC
			bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item)
			{
				string text;
				return this.TryGetValue(item.Key, out text) && text.Equals(item.Value);
			}

			// Token: 0x06004416 RID: 17430 RVA: 0x0011E0DC File Offset: 0x0011C2DC
			void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array", SR.GetString("ArgumentNull_Array"));
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (array.Length - arrayIndex < this.Count)
				{
					throw new ArgumentException(SR.GetString("Arg_ArrayPlusOffTooSmall"));
				}
				int num = arrayIndex;
				foreach (object obj in this.m_stringDictionary)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					array[num++] = new KeyValuePair<string, string>((string)dictionaryEntry.Key, (string)dictionaryEntry.Value);
				}
			}

			// Token: 0x17000F6F RID: 3951
			// (get) Token: 0x06004417 RID: 17431 RVA: 0x0011E1A8 File Offset: 0x0011C3A8
			bool ICollection<KeyValuePair<string, string>>.IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06004418 RID: 17432 RVA: 0x0011E1AC File Offset: 0x0011C3AC
			bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
			{
				if (!((ICollection<KeyValuePair<string, string>>)this).Contains(item))
				{
					return false;
				}
				this.m_stringDictionary.Remove(item.Key);
				return true;
			}

			// Token: 0x06004419 RID: 17433 RVA: 0x0011E1D9 File Offset: 0x0011C3D9
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x0600441A RID: 17434 RVA: 0x0011E1E1 File Offset: 0x0011C3E1
			public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
			{
				foreach (object obj in this.m_stringDictionary)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					yield return new KeyValuePair<string, string>((string)dictionaryEntry.Key, (string)dictionaryEntry.Value);
				}
				IEnumerator enumerator = null;
				yield break;
				yield break;
			}

			// Token: 0x04003503 RID: 13571
			private StringDictionary m_stringDictionary;

			// Token: 0x04003504 RID: 13572
			private StringDictionary.GenericAdapter.ICollectionToGenericCollectionAdapter _values;

			// Token: 0x04003505 RID: 13573
			private StringDictionary.GenericAdapter.ICollectionToGenericCollectionAdapter _keys;

			// Token: 0x02000926 RID: 2342
			internal enum KeyOrValue
			{
				// Token: 0x04003DB4 RID: 15796
				Key,
				// Token: 0x04003DB5 RID: 15797
				Value
			}

			// Token: 0x02000927 RID: 2343
			private class ICollectionToGenericCollectionAdapter : ICollection<string>, IEnumerable<string>, IEnumerable
			{
				// Token: 0x06004676 RID: 18038 RVA: 0x001265EB File Offset: 0x001247EB
				public ICollectionToGenericCollectionAdapter(StringDictionary source, StringDictionary.GenericAdapter.KeyOrValue keyOrValue)
				{
					if (source == null)
					{
						throw new ArgumentNullException("source");
					}
					this._internal = source;
					this._keyOrValue = keyOrValue;
				}

				// Token: 0x06004677 RID: 18039 RVA: 0x0012660F File Offset: 0x0012480F
				public void Add(string item)
				{
					this.ThrowNotSupportedException();
				}

				// Token: 0x06004678 RID: 18040 RVA: 0x00126617 File Offset: 0x00124817
				public void Clear()
				{
					this.ThrowNotSupportedException();
				}

				// Token: 0x06004679 RID: 18041 RVA: 0x0012661F File Offset: 0x0012481F
				public void ThrowNotSupportedException()
				{
					if (this._keyOrValue == StringDictionary.GenericAdapter.KeyOrValue.Key)
					{
						throw new NotSupportedException(SR.GetString("NotSupported_KeyCollectionSet"));
					}
					throw new NotSupportedException(SR.GetString("NotSupported_ValueCollectionSet"));
				}

				// Token: 0x0600467A RID: 18042 RVA: 0x00126648 File Offset: 0x00124848
				public bool Contains(string item)
				{
					if (this._keyOrValue == StringDictionary.GenericAdapter.KeyOrValue.Key)
					{
						return this._internal.ContainsKey(item);
					}
					return this._internal.ContainsValue(item);
				}

				// Token: 0x0600467B RID: 18043 RVA: 0x0012666C File Offset: 0x0012486C
				public void CopyTo(string[] array, int arrayIndex)
				{
					ICollection underlyingCollection = this.GetUnderlyingCollection();
					underlyingCollection.CopyTo(array, arrayIndex);
				}

				// Token: 0x17000FE3 RID: 4067
				// (get) Token: 0x0600467C RID: 18044 RVA: 0x00126688 File Offset: 0x00124888
				public int Count
				{
					get
					{
						return this._internal.Count;
					}
				}

				// Token: 0x17000FE4 RID: 4068
				// (get) Token: 0x0600467D RID: 18045 RVA: 0x00126695 File Offset: 0x00124895
				public bool IsReadOnly
				{
					get
					{
						return true;
					}
				}

				// Token: 0x0600467E RID: 18046 RVA: 0x00126698 File Offset: 0x00124898
				public bool Remove(string item)
				{
					this.ThrowNotSupportedException();
					return false;
				}

				// Token: 0x0600467F RID: 18047 RVA: 0x001266A1 File Offset: 0x001248A1
				private ICollection GetUnderlyingCollection()
				{
					if (this._keyOrValue == StringDictionary.GenericAdapter.KeyOrValue.Key)
					{
						return this._internal.Keys;
					}
					return this._internal.Values;
				}

				// Token: 0x06004680 RID: 18048 RVA: 0x001266C2 File Offset: 0x001248C2
				public IEnumerator<string> GetEnumerator()
				{
					ICollection underlyingCollection = this.GetUnderlyingCollection();
					foreach (object obj in underlyingCollection)
					{
						string text = (string)obj;
						yield return text;
					}
					IEnumerator enumerator = null;
					yield break;
					yield break;
				}

				// Token: 0x06004681 RID: 18049 RVA: 0x001266D1 File Offset: 0x001248D1
				IEnumerator IEnumerable.GetEnumerator()
				{
					return this.GetUnderlyingCollection().GetEnumerator();
				}

				// Token: 0x04003DB6 RID: 15798
				private StringDictionary _internal;

				// Token: 0x04003DB7 RID: 15799
				private StringDictionary.GenericAdapter.KeyOrValue _keyOrValue;
			}
		}
	}
}
