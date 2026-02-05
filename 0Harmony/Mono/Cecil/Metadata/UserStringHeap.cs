using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B6 RID: 438
	internal sealed class UserStringHeap : StringHeap
	{
		// Token: 0x06000D76 RID: 3446 RVA: 0x0002DFF4 File Offset: 0x0002C1F4
		public UserStringHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0002E000 File Offset: 0x0002C200
		protected override string ReadStringAt(uint index)
		{
			int num = (int)index;
			uint num2 = (uint)((ulong)this.data.ReadCompressedUInt32(ref num) & 18446744073709551614UL);
			if (num2 < 1U)
			{
				return string.Empty;
			}
			char[] array = new char[num2 / 2U];
			int num3 = num;
			int num4 = 0;
			while ((long)num3 < (long)num + (long)((ulong)num2))
			{
				array[num4++] = (char)((int)this.data[num3] | ((int)this.data[num3 + 1] << 8));
				num3 += 2;
			}
			return new string(array);
		}
	}
}
