using System;
using System.Collections.Generic;
using System.Text;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A1 RID: 417
	internal class StringHeapBuffer : HeapBuffer
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x0002D991 File Offset: 0x0002BB91
		public sealed override bool IsEmpty
		{
			get
			{
				return this.length <= 1;
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0002D99F File Offset: 0x0002BB9F
		public StringHeapBuffer()
			: base(1)
		{
			base.WriteByte(0);
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0002D9C0 File Offset: 0x0002BBC0
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

		// Token: 0x06000D4E RID: 3406 RVA: 0x0002D9FC File Offset: 0x0002BBFC
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

		// Token: 0x06000D4F RID: 3407 RVA: 0x0002DAD8 File Offset: 0x0002BCD8
		private static List<KeyValuePair<string, uint>> SortStrings(Dictionary<string, uint> strings)
		{
			List<KeyValuePair<string, uint>> list = new List<KeyValuePair<string, uint>>(strings);
			list.Sort(new StringHeapBuffer.SuffixSort());
			return list;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0002DAEB File Offset: 0x0002BCEB
		private static bool IsLowSurrogateChar(int c)
		{
			return c - 56320 <= 1023;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0002DAFE File Offset: 0x0002BCFE
		protected virtual void WriteString(string @string)
		{
			base.WriteBytes(Encoding.UTF8.GetBytes(@string));
			base.WriteByte(0);
		}

		// Token: 0x040005F1 RID: 1521
		protected Dictionary<string, uint> strings = new Dictionary<string, uint>(StringComparer.Ordinal);

		// Token: 0x020001A2 RID: 418
		private class SuffixSort : IComparer<KeyValuePair<string, uint>>
		{
			// Token: 0x06000D52 RID: 3410 RVA: 0x0002DB18 File Offset: 0x0002BD18
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
