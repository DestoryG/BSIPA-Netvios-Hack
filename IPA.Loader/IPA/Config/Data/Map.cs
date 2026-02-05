using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IPA.Utilities;

namespace IPA.Config.Data
{
	/// <summary>
	/// A ordered map of <see cref="T:System.String" /> to <see cref="T:IPA.Config.Data.Value" /> for serialization by an <see cref="T:IPA.Config.IConfigProvider" />.
	/// Use <see cref="M:IPA.Config.Data.Value.Map" /> or <see cref="M:IPA.Config.Data.Value.From(System.Collections.Generic.IDictionary{System.String,IPA.Config.Data.Value})" /> to create.
	/// </summary>
	// Token: 0x02000095 RID: 149
	public sealed class Map : Value, IDictionary<string, Value>, ICollection<KeyValuePair<string, Value>>, IEnumerable<KeyValuePair<string, Value>>, IEnumerable
	{
		// Token: 0x060003B3 RID: 947 RVA: 0x000133B9 File Offset: 0x000115B9
		internal Map()
		{
		}

		/// <summary>
		/// Accesses the <see cref="T:IPA.Config.Data.Value" /> at <paramref name="key" /> in the map.
		/// </summary>
		/// <param name="key">the key to get the value associated with</param>
		/// <returns>the value associated with the <paramref name="key" /></returns>
		/// <seealso cref="P:System.Collections.Generic.IDictionary`2.Item(`0)" />
		// Token: 0x170000A4 RID: 164
		public Value this[string key]
		{
			get
			{
				return this.values[key];
			}
			set
			{
				this.values[key] = value;
			}
		}

		/// <summary>
		/// Gets a collection of the keys for the <see cref="T:IPA.Config.Data.Map" />.
		/// </summary>
		/// <seealso cref="P:System.Collections.Generic.IDictionary`2.Keys" />
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x000133F4 File Offset: 0x000115F4
		public ICollection<string> Keys
		{
			get
			{
				return this.keyOrder;
			}
		}

		/// <summary>
		/// Gets a collection of the values in the <see cref="T:IPA.Config.Data.Map" />.
		/// </summary>
		/// <remarks>
		/// Unlike all other iterables given by <see cref="T:IPA.Config.Data.Map" />, this does <i>not</i>
		/// guarantee that order is maintained.
		/// </remarks>
		/// <seealso cref="P:System.Collections.Generic.IDictionary`2.Values" />
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x000133FC File Offset: 0x000115FC
		public ICollection<Value> Values
		{
			get
			{
				return this.values.Values;
			}
		}

		/// <summary>
		/// Gets the number of key-value pairs in this <see cref="T:IPA.Config.Data.Map" />.
		/// </summary>
		/// <seealso cref="P:System.Collections.Generic.ICollection`1.Count" />
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00013409 File Offset: 0x00011609
		public int Count
		{
			get
			{
				return this.values.Count;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00013416 File Offset: 0x00011616
		bool ICollection<KeyValuePair<string, Value>>.IsReadOnly
		{
			get
			{
				return ((ICollection<KeyValuePair<string, Value>>)this.values).IsReadOnly;
			}
		}

		/// <summary>
		/// Adds a new <see cref="T:IPA.Config.Data.Value" /> with a given key.
		/// </summary>
		/// <param name="key">the key to put the value at</param>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Value" /> to add</param>
		/// <seealso cref="M:System.Collections.Generic.IDictionary`2.Add(`0,`1)" />
		// Token: 0x060003BA RID: 954 RVA: 0x00013423 File Offset: 0x00011623
		public void Add(string key, Value value)
		{
			this.values.Add(key, value);
			this.keyOrder.Add(key);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0001343E File Offset: 0x0001163E
		void ICollection<KeyValuePair<string, Value>>.Add(KeyValuePair<string, Value> item)
		{
			this.Add(item.Key, item.Value);
		}

		/// <summary>
		/// Clears the <see cref="T:IPA.Config.Data.Map" /> of its key-value pairs.
		/// </summary>
		/// <seealso cref="M:System.Collections.Generic.ICollection`1.Clear" />
		// Token: 0x060003BC RID: 956 RVA: 0x00013454 File Offset: 0x00011654
		public void Clear()
		{
			this.values.Clear();
			this.keyOrder.Clear();
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0001346C File Offset: 0x0001166C
		bool ICollection<KeyValuePair<string, Value>>.Contains(KeyValuePair<string, Value> item)
		{
			return ((ICollection<KeyValuePair<string, Value>>)this.values).Contains(item);
		}

		/// <summary>
		/// Checks if the <see cref="T:IPA.Config.Data.Map" /> contains a given <paramref name="key" />.
		/// </summary>
		/// <param name="key">the key to check for</param>
		/// <returns><see langword="true" /> if the key exists, otherwise <see langword="false" /></returns>
		/// <seealso cref="M:System.Collections.Generic.IDictionary`2.ContainsKey(`0)" />
		// Token: 0x060003BE RID: 958 RVA: 0x0001347A File Offset: 0x0001167A
		public bool ContainsKey(string key)
		{
			return this.values.ContainsKey(key);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00013488 File Offset: 0x00011688
		void ICollection<KeyValuePair<string, Value>>.CopyTo(KeyValuePair<string, Value>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, Value>>)this.values).CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Enumerates the <see cref="T:IPA.Config.Data.Map" />'s key-value pairs.
		/// </summary>
		/// <returns>an <see cref="T:System.Collections.Generic.IEnumerator`1" /> of key-value pairs in this <see cref="T:IPA.Config.Data.Map" /></returns>
		/// <seealso cref="M:System.Collections.Generic.IEnumerable`1.GetEnumerator" />
		// Token: 0x060003C0 RID: 960 RVA: 0x00013497 File Offset: 0x00011697
		public IEnumerator<KeyValuePair<string, Value>> GetEnumerator()
		{
			foreach (string key in this.keyOrder)
			{
				yield return new KeyValuePair<string, Value>(key, this[key]);
			}
			List<string>.Enumerator enumerator = default(List<string>.Enumerator);
			yield break;
			yield break;
		}

		/// <summary>
		/// Removes the object associated with a key in this <see cref="T:IPA.Config.Data.Map" />.
		/// </summary>
		/// <param name="key">the key to remove</param>
		/// <returns><see langword="true" /> if the key existed, <see langword="false" /> otherwise</returns>
		/// <seealso cref="M:System.Collections.Generic.IDictionary`2.Remove(`0)" />
		// Token: 0x060003C1 RID: 961 RVA: 0x000134A6 File Offset: 0x000116A6
		public bool Remove(string key)
		{
			return this.values.Remove(key) && this.keyOrder.Remove(key);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x000134C4 File Offset: 0x000116C4
		bool ICollection<KeyValuePair<string, Value>>.Remove(KeyValuePair<string, Value> item)
		{
			return ((ICollection<KeyValuePair<string, Value>>)this.values).Remove(item) && (this.keyOrder.Remove(item.Key) || true);
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">the key of the value to get</param>
		/// <param name="value">the target location of the retrieved object</param>
		/// <returns><see langword="true" /> if the key was found and <paramref name="value" /> set, <see langword="false" /> otherwise</returns>
		/// <seealso cref="M:System.Collections.Generic.IDictionary`2.TryGetValue(`0,`1@)" />
		// Token: 0x060003C3 RID: 963 RVA: 0x000134ED File Offset: 0x000116ED
		public bool TryGetValue(string key, out Value value)
		{
			return this.values.TryGetValue(key, out value);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x000134FC File Offset: 0x000116FC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Converts this <see cref="T:IPA.Config.Data.Value" /> into a human-readable format.
		/// </summary>
		/// <returns>a JSON-like set of key-value pairs</returns>
		// Token: 0x060003C5 RID: 965 RVA: 0x00013504 File Offset: 0x00011704
		public override string ToString()
		{
			return "{" + string.Join(",", this.Select(delegate(KeyValuePair<string, Value> p)
			{
				string text = "\"";
				string key = p.Key;
				string text2 = "\":";
				Value value = p.Value;
				return text + key + text2 + (((value != null) ? value.ToString() : null) ?? "null");
			}).StrJP()) + "}";
		}

		// Token: 0x0400012E RID: 302
		private readonly Dictionary<string, Value> values = new Dictionary<string, Value>();

		// Token: 0x0400012F RID: 303
		private readonly List<string> keyOrder = new List<string>();
	}
}
