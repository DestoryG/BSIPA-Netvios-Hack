using System;
using System.Collections.Generic;
using System.Text;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000DE RID: 222
	internal class StringHeapBuffer : HeapBuffer
	{
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0001E87D File Offset: 0x0001CA7D
		public sealed override bool IsEmpty
		{
			get
			{
				return this.length <= 1;
			}
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001E88B File Offset: 0x0001CA8B
		public StringHeapBuffer()
			: base(1)
		{
			base.WriteByte(0);
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0001E8AC File Offset: 0x0001CAAC
		public virtual uint GetStringIndex(string @string)
		{
			uint num;
			if (this.strings.TryGetValue(@string, out num))
			{
				return num;
			}
			num = (uint)(this.strings.Count + 1);
			this.strings.Add(@string, num);
			return num;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0001E8E8 File Offset: 0x0001CAE8
		public uint[] WriteStrings()
		{
			List<KeyValuePair<string, uint>> list = StringHeapBuffer.SortStrings(this.strings);
			this.strings = null;
			uint[] array = new uint[list.Count + 1];
			array[0] = 0U;
			string text = string.Empty;
			foreach (KeyValuePair<string, uint> keyValuePair in list)
			{
				string key = keyValuePair.Key;
				uint value = keyValuePair.Value;
				int position = this.position;
				if (text.EndsWith(key, StringComparison.Ordinal) && !StringHeapBuffer.IsLowSurrogateChar((int)keyValuePair.Key[0]))
				{
					array[(int)value] = (uint)(position - (Encoding.UTF8.GetByteCount(keyValuePair.Key) + 1));
				}
				else
				{
					array[(int)value] = (uint)position;
					this.WriteString(key);
				}
				text = keyValuePair.Key;
			}
			return array;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0001E9C4 File Offset: 0x0001CBC4
		private static List<KeyValuePair<string, uint>> SortStrings(Dictionary<string, uint> strings)
		{
			List<KeyValuePair<string, uint>> list = new List<KeyValuePair<string, uint>>(strings);
			list.Sort(new StringHeapBuffer.SuffixSort());
			return list;
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0001E9D7 File Offset: 0x0001CBD7
		private static bool IsLowSurrogateChar(int c)
		{
			return c - 56320 <= 1023;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0001E9EA File Offset: 0x0001CBEA
		protected virtual void WriteString(string @string)
		{
			base.WriteBytes(Encoding.UTF8.GetBytes(@string));
			base.WriteByte(0);
		}

		// Token: 0x04000392 RID: 914
		protected Dictionary<string, uint> strings = new Dictionary<string, uint>(StringComparer.Ordinal);

		// Token: 0x0200014F RID: 335
		private class SuffixSort : IComparer<KeyValuePair<string, uint>>
		{
			// Token: 0x06000BD9 RID: 3033 RVA: 0x000251C8 File Offset: 0x000233C8
			public int Compare(KeyValuePair<string, uint> xPair, KeyValuePair<string, uint> yPair)
			{
				string key = xPair.Key;
				string key2 = yPair.Key;
				int num = key.Length - 1;
				int num2 = key2.Length - 1;
				while ((num >= 0) & (num2 >= 0))
				{
					if (key[num] < key2[num2])
					{
						return -1;
					}
					if (key[num] > key2[num2])
					{
						return 1;
					}
					num--;
					num2--;
				}
				return key2.Length.CompareTo(key.Length);
			}
		}
	}
}
