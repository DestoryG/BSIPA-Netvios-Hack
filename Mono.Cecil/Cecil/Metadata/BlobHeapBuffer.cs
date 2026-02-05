using System;
using System.Collections.Generic;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000DF RID: 223
	internal sealed class BlobHeapBuffer : HeapBuffer
	{
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x0001E87D File Offset: 0x0001CA7D
		public override bool IsEmpty
		{
			get
			{
				return this.length <= 1;
			}
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0001EA04 File Offset: 0x0001CC04
		public BlobHeapBuffer()
			: base(1)
		{
			base.WriteByte(0);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0001EA24 File Offset: 0x0001CC24
		public uint GetBlobIndex(ByteBuffer blob)
		{
			uint position;
			if (this.blobs.TryGetValue(blob, out position))
			{
				return position;
			}
			position = (uint)this.position;
			this.WriteBlob(blob);
			this.blobs.Add(blob, position);
			return position;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0001EA5F File Offset: 0x0001CC5F
		private void WriteBlob(ByteBuffer blob)
		{
			base.WriteCompressedUInt32((uint)blob.length);
			base.WriteBytes(blob);
		}

		// Token: 0x04000393 RID: 915
		private readonly Dictionary<ByteBuffer, uint> blobs = new Dictionary<ByteBuffer, uint>(new ByteBufferEqualityComparer());
	}
}
