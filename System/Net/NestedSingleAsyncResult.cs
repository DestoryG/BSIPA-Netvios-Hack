using System;

namespace System.Net
{
	// Token: 0x020001CA RID: 458
	internal class NestedSingleAsyncResult : LazyAsyncResult
	{
		// Token: 0x06001228 RID: 4648 RVA: 0x00060DBE File Offset: 0x0005EFBE
		internal NestedSingleAsyncResult(object asyncObject, object asyncState, AsyncCallback asyncCallback, object result)
			: base(asyncObject, asyncState, asyncCallback, result)
		{
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00060DCB File Offset: 0x0005EFCB
		internal NestedSingleAsyncResult(object asyncObject, object asyncState, AsyncCallback asyncCallback, byte[] buffer, int offset, int size)
			: base(asyncObject, asyncState, asyncCallback)
		{
			this.Buffer = buffer;
			this.Offset = offset;
			this.Size = size;
		}

		// Token: 0x0400147F RID: 5247
		internal byte[] Buffer;

		// Token: 0x04001480 RID: 5248
		internal int Offset;

		// Token: 0x04001481 RID: 5249
		internal int Size;
	}
}
