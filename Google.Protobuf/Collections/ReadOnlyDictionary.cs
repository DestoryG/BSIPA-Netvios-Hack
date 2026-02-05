using System;
using System.Collections;
using System.Collections.Generic;

namespace Google.Protobuf.Collections
{
	// Token: 0x02000087 RID: 135
	internal sealed class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		// Token: 0x06000876 RID: 2166 RVA: 0x0001DE38 File Offset: 0x0001C038
		public ReadOnlyDictionary(IDictionary<TKey, TValue> wrapped)
		{
			this.wrapped = wrapped;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0001DE47 File Offset: 0x0001C047
		public void Add(TKey key, TValue value)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0001DE4E File Offset: 0x0001C04E
		public bool ContainsKey(TKey key)
		{
			return this.wrapped.ContainsKey(key);
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0001DE5C File Offset: 0x0001C05C
		public ICollection<TKey> Keys
		{
			get
			{
				return this.wrapped.Keys;
			}
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0001DE69 File Offset: 0x0001C069
		public bool Remove(TKey key)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0001DE70 File Offset: 0x0001C070
		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.wrapped.TryGetValue(key, out value);
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x0001DE7F File Offset: 0x0001C07F
		public ICollection<TValue> Values
		{
			get
			{
				return this.wrapped.Values;
			}
		}

		// Token: 0x17000255 RID: 597
		public TValue this[TKey key]
		{
			get
			{
				return this.wrapped[key];
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0001DEA1 File Offset: 0x0001C0A1
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0001DEA8 File Offset: 0x0001C0A8
		public void Clear()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0001DEAF File Offset: 0x0001C0AF
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return this.wrapped.Contains(item);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0001DEBD File Offset: 0x0001C0BD
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.wrapped.CopyTo(array, arrayIndex);
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x0001DECC File Offset: 0x0001C0CC
		public int Count
		{
			get
			{
				return this.wrapped.Count;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x0001DED9 File Offset: 0x0001C0D9
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0001DEDC File Offset: 0x0001C0DC
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0001DEE3 File Offset: 0x0001C0E3
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.wrapped.GetEnumerator();
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0001DEF0 File Offset: 0x0001C0F0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.wrapped.GetEnumerator();
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0001DEFD File Offset: 0x0001C0FD
		public override bool Equals(object obj)
		{
			return this.wrapped.Equals(obj);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0001DF0B File Offset: 0x0001C10B
		public override int GetHashCode()
		{
			return this.wrapped.GetHashCode();
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0001DF18 File Offset: 0x0001C118
		public override string ToString()
		{
			return this.wrapped.ToString();
		}

		// Token: 0x04000351 RID: 849
		private readonly IDictionary<TKey, TValue> wrapped;
	}
}
