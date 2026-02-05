using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x0200019B RID: 411
	internal sealed class BlobHeap : Heap
	{
		// Token: 0x06000D28 RID: 3368 RVA: 0x0002D3FF File Offset: 0x0002B5FF
		public BlobHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0002D408 File Offset: 0x0002B608
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

		// Token: 0x06000D2A RID: 3370 RVA: 0x0002D468 File Offset: 0x0002B668
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
