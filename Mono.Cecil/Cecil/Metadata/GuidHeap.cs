using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000E4 RID: 228
	internal sealed class GuidHeap : Heap
	{
		// Token: 0x06000979 RID: 2425 RVA: 0x0001E2E7 File Offset: 0x0001C4E7
		public GuidHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0001EB38 File Offset: 0x0001CD38
		public Guid Read(uint index)
		{
			if (index == 0U || (ulong)(index - 1U + 16U) > (ulong)((long)this.data.Length))
			{
				return default(Guid);
			}
			byte[] array = new byte[16];
			Buffer.BlockCopy(this.data, (int)((index - 1U) * 16U), array, 0, 16);
			return new Guid(array);
		}
	}
}
