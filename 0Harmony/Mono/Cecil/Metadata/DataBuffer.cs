using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x0200019E RID: 414
	internal sealed class DataBuffer : ByteBuffer
	{
		// Token: 0x06000D42 RID: 3394 RVA: 0x0002D8D7 File Offset: 0x0002BAD7
		public DataBuffer()
			: base(0)
		{
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0002D8F8 File Offset: 0x0002BAF8
		public uint AddData(byte[] data)
		{
			uint position = (uint)this.position;
			base.WriteBytes(data);
			return position;
		}
	}
}
