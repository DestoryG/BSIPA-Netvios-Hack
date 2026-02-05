using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x0200019D RID: 413
	internal sealed class ResourceBuffer : ByteBuffer
	{
		// Token: 0x06000D40 RID: 3392 RVA: 0x0002D8D7 File Offset: 0x0002BAD7
		public ResourceBuffer()
			: base(0)
		{
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0002D8E0 File Offset: 0x0002BAE0
		public uint AddResource(byte[] resource)
		{
			uint position = (uint)this.position;
			base.WriteInt32(resource.Length);
			base.WriteBytes(resource);
			return position;
		}
	}
}
