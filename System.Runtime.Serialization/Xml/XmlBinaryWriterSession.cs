using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Xml
{
	// Token: 0x02000030 RID: 48
	public class XmlBinaryWriterSession
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x0000F4E0 File Offset: 0x0000D6E0
		public XmlBinaryWriterSession()
		{
			this.nextKey = 0;
			this.maps = new XmlBinaryWriterSession.PriorityDictionary<IXmlDictionary, XmlBinaryWriterSession.IntArray>();
			this.strings = new XmlBinaryWriterSession.PriorityDictionary<string, int>();
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000F508 File Offset: 0x0000D708
		public virtual bool TryAdd(XmlDictionaryString value, out int key)
		{
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
			}
			XmlBinaryWriterSession.IntArray intArray;
			if (!this.maps.TryGetValue(value.Dictionary, out intArray))
			{
				key = this.Add(value.Value);
				intArray = this.AddKeys(value.Dictionary, value.Key + 1);
				intArray[value.Key] = key + 1;
				return true;
			}
			key = intArray[value.Key] - 1;
			if (key != -1)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("The specified key already exists in the dictionary.")));
			}
			key = this.Add(value.Value);
			intArray[value.Key] = key + 1;
			return true;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000F5B8 File Offset: 0x0000D7B8
		private int Add(string s)
		{
			int num = this.nextKey;
			this.nextKey = num + 1;
			int num2 = num;
			this.strings.Add(s, num2);
			return num2;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000F5E8 File Offset: 0x0000D7E8
		private XmlBinaryWriterSession.IntArray AddKeys(IXmlDictionary dictionary, int minCount)
		{
			XmlBinaryWriterSession.IntArray intArray = new XmlBinaryWriterSession.IntArray(Math.Max(minCount, 16));
			this.maps.Add(dictionary, intArray);
			return intArray;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000F611 File Offset: 0x0000D811
		public void Reset()
		{
			this.nextKey = 0;
			this.maps.Clear();
			this.strings.Clear();
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000F630 File Offset: 0x0000D830
		internal bool TryLookup(XmlDictionaryString s, out int key)
		{
			XmlBinaryWriterSession.IntArray intArray;
			if (this.maps.TryGetValue(s.Dictionary, out intArray))
			{
				key = intArray[s.Key] - 1;
				if (key != -1)
				{
					return true;
				}
			}
			if (this.strings.TryGetValue(s.Value, out key))
			{
				if (intArray == null)
				{
					intArray = this.AddKeys(s.Dictionary, s.Key + 1);
				}
				intArray[s.Key] = key + 1;
				return true;
			}
			key = -1;
			return false;
		}

		// Token: 0x040001A9 RID: 425
		private XmlBinaryWriterSession.PriorityDictionary<string, int> strings;

		// Token: 0x040001AA RID: 426
		private XmlBinaryWriterSession.PriorityDictionary<IXmlDictionary, XmlBinaryWriterSession.IntArray> maps;

		// Token: 0x040001AB RID: 427
		private int nextKey;

		// Token: 0x0200014A RID: 330
		private class PriorityDictionary<K, V> where K : class
		{
			// Token: 0x0600129B RID: 4763 RVA: 0x0004D837 File Offset: 0x0004BA37
			public PriorityDictionary()
			{
				this.list = new XmlBinaryWriterSession.PriorityDictionary<K, V>.Entry[16];
			}

			// Token: 0x0600129C RID: 4764 RVA: 0x0004D84C File Offset: 0x0004BA4C
			public void Clear()
			{
				this.now = 0;
				this.listCount = 0;
				Array.Clear(this.list, 0, this.list.Length);
				if (this.dictionary != null)
				{
					this.dictionary.Clear();
				}
			}

			// Token: 0x0600129D RID: 4765 RVA: 0x0004D884 File Offset: 0x0004BA84
			public bool TryGetValue(K key, out V value)
			{
				for (int i = 0; i < this.listCount; i++)
				{
					if (this.list[i].Key == key)
					{
						value = this.list[i].Value;
						this.list[i].Time = this.Now;
						return true;
					}
				}
				for (int j = 0; j < this.listCount; j++)
				{
					if (this.list[j].Key.Equals(key))
					{
						value = this.list[j].Value;
						this.list[j].Time = this.Now;
						return true;
					}
				}
				if (this.dictionary == null)
				{
					value = default(V);
					return false;
				}
				if (!this.dictionary.TryGetValue(key, out value))
				{
					return false;
				}
				int num = 0;
				int num2 = this.list[0].Time;
				for (int k = 1; k < this.listCount; k++)
				{
					if (this.list[k].Time < num2)
					{
						num = k;
						num2 = this.list[k].Time;
					}
				}
				this.list[num].Key = key;
				this.list[num].Value = value;
				this.list[num].Time = this.Now;
				return true;
			}

			// Token: 0x0600129E RID: 4766 RVA: 0x0004DA0C File Offset: 0x0004BC0C
			public void Add(K key, V value)
			{
				if (this.listCount < this.list.Length)
				{
					this.list[this.listCount].Key = key;
					this.list[this.listCount].Value = value;
					this.listCount++;
					return;
				}
				if (this.dictionary == null)
				{
					this.dictionary = new Dictionary<K, V>();
					for (int i = 0; i < this.listCount; i++)
					{
						this.dictionary.Add(this.list[i].Key, this.list[i].Value);
					}
				}
				this.dictionary.Add(key, value);
			}

			// Token: 0x170003B2 RID: 946
			// (get) Token: 0x0600129F RID: 4767 RVA: 0x0004DAC4 File Offset: 0x0004BCC4
			private int Now
			{
				get
				{
					int num = this.now + 1;
					this.now = num;
					if (num == 2147483647)
					{
						this.DecreaseAll();
					}
					return this.now;
				}
			}

			// Token: 0x060012A0 RID: 4768 RVA: 0x0004DAF8 File Offset: 0x0004BCF8
			private void DecreaseAll()
			{
				for (int i = 0; i < this.listCount; i++)
				{
					XmlBinaryWriterSession.PriorityDictionary<K, V>.Entry[] array = this.list;
					int num = i;
					array[num].Time = array[num].Time / 2;
				}
				this.now /= 2;
			}

			// Token: 0x04000922 RID: 2338
			private Dictionary<K, V> dictionary;

			// Token: 0x04000923 RID: 2339
			private XmlBinaryWriterSession.PriorityDictionary<K, V>.Entry[] list;

			// Token: 0x04000924 RID: 2340
			private int listCount;

			// Token: 0x04000925 RID: 2341
			private int now;

			// Token: 0x020001A9 RID: 425
			private struct Entry
			{
				// Token: 0x04000A8A RID: 2698
				public K Key;

				// Token: 0x04000A8B RID: 2699
				public V Value;

				// Token: 0x04000A8C RID: 2700
				public int Time;
			}
		}

		// Token: 0x0200014B RID: 331
		private class IntArray
		{
			// Token: 0x060012A1 RID: 4769 RVA: 0x0004DB3A File Offset: 0x0004BD3A
			public IntArray(int size)
			{
				this.array = new int[size];
			}

			// Token: 0x170003B3 RID: 947
			public int this[int index]
			{
				get
				{
					if (index >= this.array.Length)
					{
						return 0;
					}
					return this.array[index];
				}
				set
				{
					if (index >= this.array.Length)
					{
						int[] array = new int[Math.Max(index + 1, this.array.Length * 2)];
						Array.Copy(this.array, array, this.array.Length);
						this.array = array;
					}
					this.array[index] = value;
				}
			}

			// Token: 0x04000926 RID: 2342
			private int[] array;
		}
	}
}
