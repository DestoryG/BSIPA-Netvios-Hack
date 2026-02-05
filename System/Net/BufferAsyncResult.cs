using System;

namespace System.Net
{
	// Token: 0x02000220 RID: 544
	internal class BufferAsyncResult : LazyAsyncResult
	{
		// Token: 0x06001409 RID: 5129 RVA: 0x0006A73B File Offset: 0x0006893B
		public BufferAsyncResult(object asyncObject, BufferOffsetSize[] buffers, object asyncState, AsyncCallback asyncCallback)
			: base(asyncObject, asyncState, asyncCallback)
		{
			this.Buffers = buffers;
			this.IsWrite = true;
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0006A755 File Offset: 0x00068955
		public BufferAsyncResult(object asyncObject, byte[] buffer, int offset, int count, object asyncState, AsyncCallback asyncCallback)
			: this(asyncObject, buffer, offset, count, false, asyncState, asyncCallback)
		{
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0006A767 File Offset: 0x00068967
		public BufferAsyncResult(object asyncObject, byte[] buffer, int offset, int count, bool isWrite, object asyncState, AsyncCallback asyncCallback)
			: base(asyncObject, asyncState, asyncCallback)
		{
			this.Buffer = buffer;
			this.Offset = offset;
			this.Count = count;
			this.IsWrite = isWrite;
		}

		// Token: 0x04001602 RID: 5634
		public byte[] Buffer;

		// Token: 0x04001603 RID: 5635
		public BufferOffsetSize[] Buffers;

		// Token: 0x04001604 RID: 5636
		public int Offset;

		// Token: 0x04001605 RID: 5637
		public int Count;

		// Token: 0x04001606 RID: 5638
		public bool IsWrite;
	}
}
