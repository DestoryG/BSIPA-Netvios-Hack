using System;
using System.Collections.Generic;
using System.Text;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B2 RID: 434
	internal class StringHeap : Heap
	{
		// Token: 0x06000D6F RID: 3439 RVA: 0x0002DF03 File Offset: 0x0002C103
		public StringHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0002DF18 File Offset: 0x0002C118
		public string Read(uint index)
		{
			if (index == 0U)
			{
				return string.Empty;
			}
			string text;
			if (this.strings.TryGetValue(index, out text))
			{
				return text;
			}
			if ((ulong)index > (ulong)((long)(this.data.Length - 1)))
			{
				return string.Empty;
			}
			text = this.ReadStringAt(index);
			if (text.Length != 0)
			{
				this.strings.Add(index, text);
			}
			return text;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0002DF74 File Offset: 0x0002C174
		protected virtual string ReadStringAt(uint index)
		{
			int num = 0;
			int num2 = (int)index;
			while (this.data[num2] != 0)
			{
				num++;
				num2++;
			}
			return Encoding.UTF8.GetString(this.data, (int)index, num);
		}

		// Token: 0x0400064C RID: 1612
		private readonly Dictionary<uint, string> strings = new Dictionary<uint, string>();
	}
}
