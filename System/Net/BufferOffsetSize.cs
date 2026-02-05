using System;

namespace System.Net
{
	// Token: 0x02000196 RID: 406
	internal class BufferOffsetSize
	{
		// Token: 0x06000FB7 RID: 4023 RVA: 0x0005222C File Offset: 0x0005042C
		internal BufferOffsetSize(byte[] buffer, int offset, int size, bool copyBuffer)
		{
			if (copyBuffer)
			{
				byte[] array = new byte[size];
				global::System.Buffer.BlockCopy(buffer, offset, array, 0, size);
				offset = 0;
				buffer = array;
			}
			this.Buffer = buffer;
			this.Offset = offset;
			this.Size = size;
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0005226F File Offset: 0x0005046F
		internal BufferOffsetSize(byte[] buffer, bool copyBuffer)
			: this(buffer, 0, buffer.Length, copyBuffer)
		{
		}

		// Token: 0x040012DA RID: 4826
		internal byte[] Buffer;

		// Token: 0x040012DB RID: 4827
		internal int Offset;

		// Token: 0x040012DC RID: 4828
		internal int Size;
	}
}
