using System;
using System.Collections.Generic;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A3 RID: 419
	internal sealed class BlobHeapBuffer : HeapBuffer
	{
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x0002D991 File Offset: 0x0002BB91
		public override bool IsEmpty
		{
			get
			{
				return this.length <= 1;
			}
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0002DB9B File Offset: 0x0002BD9B
		public BlobHeapBuffer()
			: base(1)
		{
			base.WriteByte(0);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0002DBBC File Offset: 0x0002BDBC
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

		// Token: 0x06000D57 RID: 3415 RVA: 0x0002DBF7 File Offset: 0x0002BDF7
		private void WriteBlob(ByteBuffer blob)
		{
			base.WriteCompressedUInt32((uint)blob.length);
			base.WriteBytes(blob);
		}

		// Token: 0x040005F2 RID: 1522
		private readonly Dictionary<ByteBuffer, uint> blobs = new Dictionary<ByteBuffer, uint>(new ByteBufferEqualityComparer());
	}
}
