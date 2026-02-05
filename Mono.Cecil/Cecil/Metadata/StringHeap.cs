using System;
using System.Collections.Generic;
using System.Text;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000EE RID: 238
	internal class StringHeap : Heap
	{
		// Token: 0x0600098B RID: 2443 RVA: 0x0001ED6B File Offset: 0x0001CF6B
		public StringHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0001ED80 File Offset: 0x0001CF80
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

		// Token: 0x0600098D RID: 2445 RVA: 0x0001EDDC File Offset: 0x0001CFDC
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

		// Token: 0x040003ED RID: 1005
		private readonly Dictionary<uint, string> strings = new Dictionary<uint, string>();
	}
}
