using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.Collections
{
	// Token: 0x02000085 RID: 133
	public sealed class MapField<TKey, TValue> : IDeepCloneable<MapField<TKey, TValue>>, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IEquatable<MapField<TKey, TValue>>, IDictionary, ICollection, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
	{
		// Token: 0x06000845 RID: 2117 RVA: 0x0001D54C File Offset: 0x0001B74C
		public MapField<TKey, TValue> Clone()
		{
			MapField<TKey, TValue> mapField = new MapField<TKey, TValue>();
			if (typeof(IDeepCloneable<TValue>).IsAssignableFrom(typeof(TValue)))
			{
				using (LinkedList<KeyValuePair<TKey, TValue>>.Enumerator enumerator = this.list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<TKey, TValue> keyValuePair = enumerator.Current;
						mapField.Add(keyValuePair.Key, ((IDeepCloneable<TValue>)((object)keyValuePair.Value)).Clone());
					}
					return mapField;
				}
			}
			mapField.Add(this);
			return mapField;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0001D5E4 File Offset: 0x0001B7E4
		public void Add(TKey key, TValue value)
		{
			if (this.ContainsKey(key))
			{
				throw new ArgumentException("Key already exists in map", "key");
			}
			this[key] = value;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001D607 File Offset: 0x0001B807
		public bool ContainsKey(TKey key)
		{
			ProtoPreconditions.CheckNotNullUnconstrained<TKey>(key, "key");
			return this.map.ContainsKey(key);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0001D624 File Offset: 0x0001B824
		private bool ContainsValue(TValue value)
		{
			return this.list.Any((KeyValuePair<TKey, TValue> pair) => MapField<TKey, TValue>.ValueEqualityComparer.Equals(pair.Value, value));
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0001D658 File Offset: 0x0001B858
		public bool Remove(TKey key)
		{
			ProtoPreconditions.CheckNotNullUnconstrained<TKey>(key, "key");
			LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
			if (this.map.TryGetValue(key, out linkedListNode))
			{
				this.map.Remove(key);
				linkedListNode.List.Remove(linkedListNode);
				return true;
			}
			return false;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0001D6A0 File Offset: 0x0001B8A0
		public bool TryGetValue(TKey key, out TValue value)
		{
			LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
			if (this.map.TryGetValue(key, out linkedListNode))
			{
				value = linkedListNode.Value.Value;
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x17000242 RID: 578
		public TValue this[TKey key]
		{
			get
			{
				ProtoPreconditions.CheckNotNullUnconstrained<TKey>(key, "key");
				TValue tvalue;
				if (this.TryGetValue(key, out tvalue))
				{
					return tvalue;
				}
				throw new KeyNotFoundException();
			}
			set
			{
				ProtoPreconditions.CheckNotNullUnconstrained<TKey>(key, "key");
				if (value == null)
				{
					ProtoPreconditions.CheckNotNullUnconstrained<TValue>(value, "value");
				}
				KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>(key, value);
				LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
				if (this.map.TryGetValue(key, out linkedListNode))
				{
					linkedListNode.Value = keyValuePair;
					return;
				}
				linkedListNode = this.list.AddLast(keyValuePair);
				this.map[key] = linkedListNode;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x0001D770 File Offset: 0x0001B970
		public ICollection<TKey> Keys
		{
			get
			{
				return new MapField<TKey, TValue>.MapView<TKey>(this, (KeyValuePair<TKey, TValue> pair) => pair.Key, new Func<TKey, bool>(this.ContainsKey));
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x0001D7A4 File Offset: 0x0001B9A4
		public ICollection<TValue> Values
		{
			get
			{
				return new MapField<TKey, TValue>.MapView<TValue>(this, (KeyValuePair<TKey, TValue> pair) => pair.Value, new Func<TValue, bool>(this.ContainsValue));
			}
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0001D7D8 File Offset: 0x0001B9D8
		public void Add(IDictionary<TKey, TValue> entries)
		{
			ProtoPreconditions.CheckNotNull<IDictionary<TKey, TValue>>(entries, "entries");
			foreach (KeyValuePair<TKey, TValue> keyValuePair in entries)
			{
				this.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0001D83C File Offset: 0x0001BA3C
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0001D84E File Offset: 0x0001BA4E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0001D856 File Offset: 0x0001BA56
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001D86C File Offset: 0x0001BA6C
		public void Clear()
		{
			this.list.Clear();
			this.map.Clear();
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0001D884 File Offset: 0x0001BA84
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			TValue tvalue;
			return this.TryGetValue(item.Key, out tvalue) && MapField<TKey, TValue>.ValueEqualityComparer.Equals(item.Value, tvalue);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0001D8B6 File Offset: 0x0001BAB6
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0001D8C8 File Offset: 0x0001BAC8
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			if (item.Key == null)
			{
				throw new ArgumentException("Key is null", "item");
			}
			LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
			if (this.map.TryGetValue(item.Key, out linkedListNode) && EqualityComparer<TValue>.Default.Equals(item.Value, linkedListNode.Value.Value))
			{
				this.map.Remove(item.Key);
				linkedListNode.List.Remove(linkedListNode);
				return true;
			}
			return false;
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x0001D94C File Offset: 0x0001BB4C
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x0001D959 File Offset: 0x0001BB59
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0001D95C File Offset: 0x0001BB5C
		public override bool Equals(object other)
		{
			return this.Equals(other as MapField<TKey, TValue>);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0001D96C File Offset: 0x0001BB6C
		public override int GetHashCode()
		{
			EqualityComparer<TKey> keyEqualityComparer = MapField<TKey, TValue>.KeyEqualityComparer;
			EqualityComparer<TValue> valueEqualityComparer = MapField<TKey, TValue>.ValueEqualityComparer;
			int num = 0;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this.list)
			{
				num ^= keyEqualityComparer.GetHashCode(keyValuePair.Key) * 31 + valueEqualityComparer.GetHashCode(keyValuePair.Value);
			}
			return num;
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0001D9EC File Offset: 0x0001BBEC
		public bool Equals(MapField<TKey, TValue> other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (other.Count != this.Count)
			{
				return false;
			}
			EqualityComparer<TValue> valueEqualityComparer = MapField<TKey, TValue>.ValueEqualityComparer;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
			{
				TValue tvalue;
				if (!other.TryGetValue(keyValuePair.Key, out tvalue))
				{
					return false;
				}
				if (!valueEqualityComparer.Equals(tvalue, keyValuePair.Value))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0001DA80 File Offset: 0x0001BC80
		public void AddEntriesFrom(CodedInputStream input, MapField<TKey, TValue>.Codec codec)
		{
			MapField<TKey, TValue>.Codec.MessageAdapter messageAdapter = new MapField<TKey, TValue>.Codec.MessageAdapter(codec);
			do
			{
				messageAdapter.Reset();
				input.ReadMessage(messageAdapter);
				this[messageAdapter.Key] = messageAdapter.Value;
			}
			while (input.MaybeConsumeTag(codec.MapTag));
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0001DAC4 File Offset: 0x0001BCC4
		public void WriteTo(CodedOutputStream output, MapField<TKey, TValue>.Codec codec)
		{
			MapField<TKey, TValue>.Codec.MessageAdapter messageAdapter = new MapField<TKey, TValue>.Codec.MessageAdapter(codec);
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this.list)
			{
				messageAdapter.Key = keyValuePair.Key;
				messageAdapter.Value = keyValuePair.Value;
				output.WriteTag(codec.MapTag);
				output.WriteMessage(messageAdapter);
			}
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0001DB44 File Offset: 0x0001BD44
		public int CalculateSize(MapField<TKey, TValue>.Codec codec)
		{
			if (this.Count == 0)
			{
				return 0;
			}
			MapField<TKey, TValue>.Codec.MessageAdapter messageAdapter = new MapField<TKey, TValue>.Codec.MessageAdapter(codec);
			int num = 0;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this.list)
			{
				messageAdapter.Key = keyValuePair.Key;
				messageAdapter.Value = keyValuePair.Value;
				num += CodedOutputStream.ComputeRawVarint32Size(codec.MapTag);
				num += CodedOutputStream.ComputeMessageSize(messageAdapter);
			}
			return num;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0001DBD8 File Offset: 0x0001BDD8
		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter();
			JsonFormatter.Default.WriteDictionary(stringWriter, this);
			return stringWriter.ToString();
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0001DBFD File Offset: 0x0001BDFD
		void IDictionary.Add(object key, object value)
		{
			this.Add((TKey)((object)key), (TValue)((object)value));
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0001DC11 File Offset: 0x0001BE11
		bool IDictionary.Contains(object key)
		{
			return key is TKey && this.ContainsKey((TKey)((object)key));
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0001DC29 File Offset: 0x0001BE29
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new MapField<TKey, TValue>.DictionaryEnumerator(this.GetEnumerator());
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0001DC36 File Offset: 0x0001BE36
		void IDictionary.Remove(object key)
		{
			ProtoPreconditions.CheckNotNull<object>(key, "key");
			if (!(key is TKey))
			{
				return;
			}
			this.Remove((TKey)((object)key));
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0001DC5A File Offset: 0x0001BE5A
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this.Select((KeyValuePair<TKey, TValue> pair) => new DictionaryEntry(pair.Key, pair.Value)).ToList<DictionaryEntry>()).CopyTo(array, index);
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x0001DC8D File Offset: 0x0001BE8D
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x0001DC90 File Offset: 0x0001BE90
		ICollection IDictionary.Keys
		{
			get
			{
				return (ICollection)this.Keys;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x0001DC9D File Offset: 0x0001BE9D
		ICollection IDictionary.Values
		{
			get
			{
				return (ICollection)this.Values;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x0001DCAA File Offset: 0x0001BEAA
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x0001DCAD File Offset: 0x0001BEAD
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700024C RID: 588
		object IDictionary.this[object key]
		{
			get
			{
				ProtoPreconditions.CheckNotNull<object>(key, "key");
				if (!(key is TKey))
				{
					return null;
				}
				TValue tvalue;
				this.TryGetValue((TKey)((object)key), out tvalue);
				return tvalue;
			}
			set
			{
				this[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x0001DCFC File Offset: 0x0001BEFC
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x0001DD04 File Offset: 0x0001BF04
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
		{
			get
			{
				return this.Values;
			}
		}

		// Token: 0x04000349 RID: 841
		private static readonly EqualityComparer<TValue> ValueEqualityComparer = ProtobufEqualityComparers.GetEqualityComparer<TValue>();

		// Token: 0x0400034A RID: 842
		private static readonly EqualityComparer<TKey> KeyEqualityComparer = ProtobufEqualityComparers.GetEqualityComparer<TKey>();

		// Token: 0x0400034B RID: 843
		private readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> map = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>(MapField<TKey, TValue>.KeyEqualityComparer);

		// Token: 0x0400034C RID: 844
		private readonly LinkedList<KeyValuePair<TKey, TValue>> list = new LinkedList<KeyValuePair<TKey, TValue>>();

		// Token: 0x02000101 RID: 257
		private class DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06000A49 RID: 2633 RVA: 0x00020F21 File Offset: 0x0001F121
			internal DictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> enumerator)
			{
				this.enumerator = enumerator;
			}

			// Token: 0x06000A4A RID: 2634 RVA: 0x00020F30 File Offset: 0x0001F130
			public bool MoveNext()
			{
				return this.enumerator.MoveNext();
			}

			// Token: 0x06000A4B RID: 2635 RVA: 0x00020F3D File Offset: 0x0001F13D
			public void Reset()
			{
				this.enumerator.Reset();
			}

			// Token: 0x1700026E RID: 622
			// (get) Token: 0x06000A4C RID: 2636 RVA: 0x00020F4A File Offset: 0x0001F14A
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x1700026F RID: 623
			// (get) Token: 0x06000A4D RID: 2637 RVA: 0x00020F57 File Offset: 0x0001F157
			public DictionaryEntry Entry
			{
				get
				{
					return new DictionaryEntry(this.Key, this.Value);
				}
			}

			// Token: 0x17000270 RID: 624
			// (get) Token: 0x06000A4E RID: 2638 RVA: 0x00020F6C File Offset: 0x0001F16C
			public object Key
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this.enumerator.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x17000271 RID: 625
			// (get) Token: 0x06000A4F RID: 2639 RVA: 0x00020F94 File Offset: 0x0001F194
			public object Value
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this.enumerator.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x0400042E RID: 1070
			private readonly IEnumerator<KeyValuePair<TKey, TValue>> enumerator;
		}

		// Token: 0x02000102 RID: 258
		public sealed class Codec
		{
			// Token: 0x06000A50 RID: 2640 RVA: 0x00020FB9 File Offset: 0x0001F1B9
			public Codec(FieldCodec<TKey> keyCodec, FieldCodec<TValue> valueCodec, uint mapTag)
			{
				this.keyCodec = keyCodec;
				this.valueCodec = valueCodec;
				this.mapTag = mapTag;
			}

			// Token: 0x17000272 RID: 626
			// (get) Token: 0x06000A51 RID: 2641 RVA: 0x00020FD6 File Offset: 0x0001F1D6
			internal uint MapTag
			{
				get
				{
					return this.mapTag;
				}
			}

			// Token: 0x0400042F RID: 1071
			private readonly FieldCodec<TKey> keyCodec;

			// Token: 0x04000430 RID: 1072
			private readonly FieldCodec<TValue> valueCodec;

			// Token: 0x04000431 RID: 1073
			private readonly uint mapTag;

			// Token: 0x02000126 RID: 294
			internal class MessageAdapter : IMessage
			{
				// Token: 0x170002AA RID: 682
				// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00022D8C File Offset: 0x00020F8C
				// (set) Token: 0x06000B28 RID: 2856 RVA: 0x00022D94 File Offset: 0x00020F94
				internal TKey Key { get; set; }

				// Token: 0x170002AB RID: 683
				// (get) Token: 0x06000B29 RID: 2857 RVA: 0x00022D9D File Offset: 0x00020F9D
				// (set) Token: 0x06000B2A RID: 2858 RVA: 0x00022DA5 File Offset: 0x00020FA5
				internal TValue Value { get; set; }

				// Token: 0x06000B2B RID: 2859 RVA: 0x00022DAE File Offset: 0x00020FAE
				internal MessageAdapter(MapField<TKey, TValue>.Codec codec)
				{
					this.codec = codec;
				}

				// Token: 0x06000B2C RID: 2860 RVA: 0x00022DBD File Offset: 0x00020FBD
				internal void Reset()
				{
					this.Key = this.codec.keyCodec.DefaultValue;
					this.Value = this.codec.valueCodec.DefaultValue;
				}

				// Token: 0x06000B2D RID: 2861 RVA: 0x00022DEC File Offset: 0x00020FEC
				public void MergeFrom(CodedInputStream input)
				{
					uint num;
					while ((num = input.ReadTag()) != 0U)
					{
						if (num == this.codec.keyCodec.Tag)
						{
							this.Key = this.codec.keyCodec.Read(input);
						}
						else if (num == this.codec.valueCodec.Tag)
						{
							this.Value = this.codec.valueCodec.Read(input);
						}
						else
						{
							input.SkipLastField();
						}
					}
					if (this.Value == null)
					{
						this.Value = this.codec.valueCodec.Read(new CodedInputStream(MapField<TKey, TValue>.Codec.MessageAdapter.ZeroLengthMessageStreamData));
					}
				}

				// Token: 0x06000B2E RID: 2862 RVA: 0x00022E90 File Offset: 0x00021090
				public void WriteTo(CodedOutputStream output)
				{
					this.codec.keyCodec.WriteTagAndValue(output, this.Key);
					this.codec.valueCodec.WriteTagAndValue(output, this.Value);
				}

				// Token: 0x06000B2F RID: 2863 RVA: 0x00022EC0 File Offset: 0x000210C0
				public int CalculateSize()
				{
					return this.codec.keyCodec.CalculateSizeWithTag(this.Key) + this.codec.valueCodec.CalculateSizeWithTag(this.Value);
				}

				// Token: 0x170002AC RID: 684
				// (get) Token: 0x06000B30 RID: 2864 RVA: 0x00022EEF File Offset: 0x000210EF
				MessageDescriptor IMessage.Descriptor
				{
					get
					{
						return null;
					}
				}

				// Token: 0x040004E4 RID: 1252
				private static readonly byte[] ZeroLengthMessageStreamData = new byte[1];

				// Token: 0x040004E5 RID: 1253
				private readonly MapField<TKey, TValue>.Codec codec;
			}
		}

		// Token: 0x02000103 RID: 259
		private class MapView<T> : ICollection<T>, IEnumerable<T>, IEnumerable, ICollection
		{
			// Token: 0x06000A52 RID: 2642 RVA: 0x00020FDE File Offset: 0x0001F1DE
			internal MapView(MapField<TKey, TValue> parent, Func<KeyValuePair<TKey, TValue>, T> projection, Func<T, bool> containsCheck)
			{
				this.parent = parent;
				this.projection = projection;
				this.containsCheck = containsCheck;
			}

			// Token: 0x17000273 RID: 627
			// (get) Token: 0x06000A53 RID: 2643 RVA: 0x00020FFB File Offset: 0x0001F1FB
			public int Count
			{
				get
				{
					return this.parent.Count;
				}
			}

			// Token: 0x17000274 RID: 628
			// (get) Token: 0x06000A54 RID: 2644 RVA: 0x00021008 File Offset: 0x0001F208
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000275 RID: 629
			// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0002100B File Offset: 0x0001F20B
			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000276 RID: 630
			// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0002100E File Offset: 0x0001F20E
			public object SyncRoot
			{
				get
				{
					return this.parent;
				}
			}

			// Token: 0x06000A57 RID: 2647 RVA: 0x00021016 File Offset: 0x0001F216
			public void Add(T item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000A58 RID: 2648 RVA: 0x0002101D File Offset: 0x0001F21D
			public void Clear()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000A59 RID: 2649 RVA: 0x00021024 File Offset: 0x0001F224
			public bool Contains(T item)
			{
				return this.containsCheck(item);
			}

			// Token: 0x06000A5A RID: 2650 RVA: 0x00021034 File Offset: 0x0001F234
			public void CopyTo(T[] array, int arrayIndex)
			{
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex");
				}
				if (arrayIndex + this.Count > array.Length)
				{
					throw new ArgumentException("Not enough space in the array", "array");
				}
				foreach (T t in this)
				{
					array[arrayIndex++] = t;
				}
			}

			// Token: 0x06000A5B RID: 2651 RVA: 0x000210B0 File Offset: 0x0001F2B0
			public IEnumerator<T> GetEnumerator()
			{
				return this.parent.list.Select(this.projection).GetEnumerator();
			}

			// Token: 0x06000A5C RID: 2652 RVA: 0x000210CD File Offset: 0x0001F2CD
			public bool Remove(T item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000A5D RID: 2653 RVA: 0x000210D4 File Offset: 0x0001F2D4
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000A5E RID: 2654 RVA: 0x000210DC File Offset: 0x0001F2DC
			public void CopyTo(Array array, int index)
			{
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index + this.Count > array.Length)
				{
					throw new ArgumentException("Not enough space in the array", "array");
				}
				foreach (T t in this)
				{
					array.SetValue(t, index++);
				}
			}

			// Token: 0x04000432 RID: 1074
			private readonly MapField<TKey, TValue> parent;

			// Token: 0x04000433 RID: 1075
			private readonly Func<KeyValuePair<TKey, TValue>, T> projection;

			// Token: 0x04000434 RID: 1076
			private readonly Func<T, bool> containsCheck;
		}
	}
}
