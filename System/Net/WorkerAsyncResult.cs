using System;

namespace System.Net
{
	// Token: 0x0200021A RID: 538
	internal class WorkerAsyncResult : LazyAsyncResult
	{
		// Token: 0x060013D1 RID: 5073 RVA: 0x000690F1 File Offset: 0x000672F1
		public WorkerAsyncResult(object asyncObject, object asyncState, AsyncCallback savedAsyncCallback, byte[] buffer, int offset, int end)
			: base(asyncObject, asyncState, savedAsyncCallback)
		{
			this.Buffer = buffer;
			this.Offset = offset;
			this.End = end;
		}

		// Token: 0x040015D1 RID: 5585
		public byte[] Buffer;

		// Token: 0x040015D2 RID: 5586
		public int Offset;

		// Token: 0x040015D3 RID: 5587
		public int End;

		// Token: 0x040015D4 RID: 5588
		public bool IsWrite;

		// Token: 0x040015D5 RID: 5589
		public WorkerAsyncResult ParentResult;

		// Token: 0x040015D6 RID: 5590
		public bool HeaderDone;

		// Token: 0x040015D7 RID: 5591
		public bool HandshakeDone;
	}
}
