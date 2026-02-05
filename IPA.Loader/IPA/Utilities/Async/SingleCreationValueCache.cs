using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace IPA.Utilities.Async
{
	/// <summary>
	/// A dictionary-like type intended for thread-safe value caches whose values are created only once ever.
	/// </summary>
	/// <typeparam name="TKey">the key type of the cache</typeparam>
	/// <typeparam name="TValue">the value type of the cache</typeparam>
	/// <remarks>
	/// This object basically wraps a <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> with some special handling
	/// to ensure that values are only created once ever, without having multiple parallel constructions.
	/// </remarks>
	// Token: 0x02000022 RID: 34
	public class SingleCreationValueCache<TKey, TValue>
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x00003F74 File Offset: 0x00002174
		private static KeyValuePair<TKey, global::System.ValueTuple<ManualResetEventSlim, TValue>> ExpandKeyValuePair(KeyValuePair<TKey, TValue> kvp)
		{
			return new KeyValuePair<TKey, global::System.ValueTuple<ManualResetEventSlim, TValue>>(kvp.Key, new global::System.ValueTuple<ManualResetEventSlim, TValue>(null, kvp.Value));
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003F8F File Offset: 0x0000218F
		private static KeyValuePair<TKey, TValue> CompressKeyValuePair([global::System.Runtime.CompilerServices.TupleElementNames(new string[] { null, "value" })] KeyValuePair<TKey, global::System.ValueTuple<ManualResetEventSlim, TValue>> kvp)
		{
			return new KeyValuePair<TKey, TValue>(kvp.Key, kvp.Value.Item2);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IPA.Utilities.Async.SingleCreationValueCache`2" />
		/// class that is empty, has the default concurrency level, has the default initial
		/// capacity, and uses the default comparer for the key type.
		/// </summary>
		// Token: 0x060000A5 RID: 165 RVA: 0x00003FA9 File Offset: 0x000021A9
		public SingleCreationValueCache()
		{
			this.dict = new ConcurrentDictionary<TKey, global::System.ValueTuple<ManualResetEventSlim, TValue>>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IPA.Utilities.Async.SingleCreationValueCache`2" />
		/// class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IEnumerable`1" />,
		/// has the default concurrency level, has the default initial capacity, and uses
		/// the default comparer for the key type.
		/// </summary>
		/// <param name="collection">the <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose element are to be used for the new cache</param>
		/// <exception cref="T:System.ArgumentNullException">when any arguments are null</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="collection" /> contains duplicate keys</exception>
		// Token: 0x060000A6 RID: 166 RVA: 0x00003FBC File Offset: 0x000021BC
		public SingleCreationValueCache(IEnumerable<KeyValuePair<TKey, TValue>> collection)
		{
			this.dict = new ConcurrentDictionary<TKey, global::System.ValueTuple<ManualResetEventSlim, TValue>>(collection.Select(new Func<KeyValuePair<TKey, TValue>, KeyValuePair<TKey, global::System.ValueTuple<ManualResetEventSlim, TValue>>>(SingleCreationValueCache<TKey, TValue>.ExpandKeyValuePair)));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IPA.Utilities.Async.SingleCreationValueCache`2" />
		/// class that is empty, has the default concurrency level and capacity, and uses
		/// the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.
		/// </summary>
		/// <param name="comparer">the equality comparer to use when comparing keys</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="comparer" /> is null</exception>
		// Token: 0x060000A7 RID: 167 RVA: 0x00003FE1 File Offset: 0x000021E1
		public SingleCreationValueCache(IEqualityComparer<TKey> comparer)
		{
			this.dict = new ConcurrentDictionary<TKey, global::System.ValueTuple<ManualResetEventSlim, TValue>>(comparer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IPA.Utilities.Async.SingleCreationValueCache`2" />
		/// class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IEnumerable`1" />
		/// has the default concurrency level, has the default initial capacity, and uses
		/// the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.
		/// </summary>
		/// <param name="collection">the <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are to be used for the new cache</param>
		/// <param name="comparer">the equality comparer to use when comparing keys</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="collection" /> or <paramref name="comparer" /> is null</exception>
		// Token: 0x060000A8 RID: 168 RVA: 0x00003FF5 File Offset: 0x000021F5
		public SingleCreationValueCache(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
		{
			this.dict = new ConcurrentDictionary<TKey, global::System.ValueTuple<ManualResetEventSlim, TValue>>(collection.Select(new Func<KeyValuePair<TKey, TValue>, KeyValuePair<TKey, global::System.ValueTuple<ManualResetEventSlim, TValue>>>(SingleCreationValueCache<TKey, TValue>.ExpandKeyValuePair)), comparer);
		}

		/// <summary>
		/// Gets a value that indicates whether this cache is empty. 
		/// </summary>
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000401B File Offset: 0x0000221B
		public bool IsEmpty
		{
			get
			{
				return this.dict.IsEmpty;
			}
		}

		/// <summary>
		/// Gets the number of elements that this cache contains.
		/// </summary>
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004028 File Offset: 0x00002228
		public int Count
		{
			get
			{
				return this.dict.Count;
			}
		}

		/// <summary>
		/// Clears the cache.
		/// </summary>
		// Token: 0x060000AB RID: 171 RVA: 0x00004035 File Offset: 0x00002235
		public void Clear()
		{
			this.dict.Clear();
		}

		/// <summary>
		/// Gets a value indicating whether or not this cache contains <paramref name="key" />.
		/// </summary>
		/// <param name="key">the key to search for</param>
		/// <returns><see langword="true" /> if the cache contains the key, <see langword="false" /> otherwise</returns>
		// Token: 0x060000AC RID: 172 RVA: 0x00004042 File Offset: 0x00002242
		public bool ContainsKey(TKey key)
		{
			return this.dict.ContainsKey(key);
		}

		/// <summary>
		/// Copies the key-value pairs stored by the cache to a new array, filtering all elements that are currently being
		/// created.
		/// </summary>
		/// <returns>an array containing a snapshot of the key-value pairs contained in this cache</returns>
		// Token: 0x060000AD RID: 173 RVA: 0x00004050 File Offset: 0x00002250
		public KeyValuePair<TKey, TValue>[] ToArray()
		{
			return (from k in this.dict.ToArray()
				where k.Value.Item1 == null
				select k).Select(new Func<KeyValuePair<TKey, global::System.ValueTuple<ManualResetEventSlim, TValue>>, KeyValuePair<TKey, TValue>>(SingleCreationValueCache<TKey, TValue>.CompressKeyValuePair)).ToArray<KeyValuePair<TKey, TValue>>();
		}

		/// <summary>
		/// Attempts to get the value associated with the specified key from the cache.
		/// </summary>
		/// <param name="key">the key to search for</param>
		/// <param name="value">the value retrieved, if any</param>
		/// <returns><see langword="true" /> if the value was found, <see langword="false" /> otherwise</returns>
		// Token: 0x060000AE RID: 174 RVA: 0x000040A4 File Offset: 0x000022A4
		public bool TryGetValue(TKey key, out TValue value)
		{
			global::System.ValueTuple<ManualResetEventSlim, TValue> pair;
			if (this.dict.TryGetValue(key, out pair) && pair.Item1 == null)
			{
				value = pair.Item2;
				return true;
			}
			value = default(TValue);
			return false;
		}

		/// <summary>
		/// Gets the value associated with the specified key from the cache. If it does not exist, and
		/// no creators are currently running for this key, then the creator is called to create the value
		/// and the value is added to the cache. If there is a creator currently running for the key, then
		/// this waits for the creator to finish and retrieves the value.
		/// </summary>
		/// <param name="key">the key to search for</param>
		/// <param name="creator">the delegate to use to create the value if it does not exist</param>
		/// <returns>the value that was found, or the result of <paramref name="creator" /></returns>
		// Token: 0x060000AF RID: 175 RVA: 0x000040E0 File Offset: 0x000022E0
		public TValue GetOrAdd(TKey key, Func<TKey, TValue> creator)
		{
			global::System.ValueTuple<ManualResetEventSlim, TValue> value;
			global::System.ValueTuple<ManualResetEventSlim, TValue> cmp;
			for (;;)
			{
				if (this.dict.TryGetValue(key, out value))
				{
					if (value.Item1 == null)
					{
						break;
					}
					value.Item1.Wait();
				}
				else
				{
					ManualResetEventSlim wh = new ManualResetEventSlim(false);
					cmp = new global::System.ValueTuple<ManualResetEventSlim, TValue>(wh, default(TValue));
					if (this.dict.TryAdd(key, cmp))
					{
						goto Block_2;
					}
				}
			}
			return value.Item2;
			Block_2:
			TValue val = creator(key);
			if (this.dict.TryUpdate(key, new global::System.ValueTuple<ManualResetEventSlim, TValue>(null, val), cmp))
			{
				ManualResetEventSlim wh;
				wh.Set();
				return val;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0400002E RID: 46
		[global::System.Runtime.CompilerServices.TupleElementNames(new string[] { "wh", "val" })]
		private readonly ConcurrentDictionary<TKey, global::System.ValueTuple<ManualResetEventSlim, TValue>> dict;
	}
}
