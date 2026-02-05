using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A8 RID: 424
	internal sealed class GuidHeap : Heap
	{
		// Token: 0x06000D5D RID: 3421 RVA: 0x0002D3FF File Offset: 0x0002B5FF
		public GuidHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0002DCD0 File Offset: 0x0002BED0
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
