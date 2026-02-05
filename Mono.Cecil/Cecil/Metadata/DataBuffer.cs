using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000DB RID: 219
	internal sealed class DataBuffer : ByteBuffer
	{
		// Token: 0x06000960 RID: 2400 RVA: 0x0001E7C3 File Offset: 0x0001C9C3
		public DataBuffer()
			: base(0)
		{
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0001E7E4 File Offset: 0x0001C9E4
		public uint AddData(byte[] data)
		{
			uint position = (uint)this.position;
			base.WriteBytes(data);
			return position;
		}
	}
}
