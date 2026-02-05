using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000DA RID: 218
	internal sealed class ResourceBuffer : ByteBuffer
	{
		// Token: 0x0600095E RID: 2398 RVA: 0x0001E7C3 File Offset: 0x0001C9C3
		public ResourceBuffer()
			: base(0)
		{
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0001E7CC File Offset: 0x0001C9CC
		public uint AddResource(byte[] resource)
		{
			uint position = (uint)this.position;
			base.WriteInt32(resource.Length);
			base.WriteBytes(resource);
			return position;
		}
	}
}
