using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000D8 RID: 216
	internal sealed class BlobHeap : Heap
	{
		// Token: 0x06000946 RID: 2374 RVA: 0x0001E2E7 File Offset: 0x0001C4E7
		public BlobHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0001E2F0 File Offset: 0x0001C4F0
		public byte[] Read(uint index)
		{
			if (index == 0U || (ulong)index > (ulong)((long)(this.data.Length - 1)))
			{
				return Empty<byte>.Array;
			}
			int num = (int)index;
			int num2 = (int)this.data.ReadCompressedUInt32(ref num);
			if (num2 > this.data.Length - num)
			{
				return Empty<byte>.Array;
			}
			byte[] array = new byte[num2];
			Buffer.BlockCopy(this.data, num, array, 0, num2);
			return array;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0001E350 File Offset: 0x0001C550
		public void GetView(uint signature, out byte[] buffer, out int index, out int length)
		{
			if (signature == 0U || (ulong)signature > (ulong)((long)(this.data.Length - 1)))
			{
				buffer = null;
				index = (length = 0);
				return;
			}
			buffer = this.data;
			index = (int)signature;
			length = (int)buffer.ReadCompressedUInt32(ref index);
		}
	}
}
