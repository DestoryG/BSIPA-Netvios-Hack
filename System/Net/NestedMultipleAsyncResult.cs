using System;

namespace System.Net
{
	// Token: 0x020001C9 RID: 457
	internal class NestedMultipleAsyncResult : LazyAsyncResult
	{
		// Token: 0x06001227 RID: 4647 RVA: 0x00060D6C File Offset: 0x0005EF6C
		internal NestedMultipleAsyncResult(object asyncObject, object asyncState, AsyncCallback asyncCallback, BufferOffsetSize[] buffers)
			: base(asyncObject, asyncState, asyncCallback)
		{
			this.Buffers = buffers;
			this.Size = 0;
			for (int i = 0; i < this.Buffers.Length; i++)
			{
				this.Size += this.Buffers[i].Size;
			}
		}

		// Token: 0x0400147D RID: 5245
		internal BufferOffsetSize[] Buffers;

		// Token: 0x0400147E RID: 5246
		internal int Size;
	}
}
