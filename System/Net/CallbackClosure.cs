using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x020001A8 RID: 424
	internal class CallbackClosure
	{
		// Token: 0x060010B4 RID: 4276 RVA: 0x00059AFD File Offset: 0x00057CFD
		internal CallbackClosure(ExecutionContext context, AsyncCallback callback)
		{
			if (callback != null)
			{
				this.savedCallback = callback;
				this.savedContext = context;
			}
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00059B16 File Offset: 0x00057D16
		internal bool IsCompatible(AsyncCallback callback)
		{
			return callback != null && this.savedCallback != null && object.Equals(this.savedCallback, callback);
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060010B6 RID: 4278 RVA: 0x00059B36 File Offset: 0x00057D36
		internal AsyncCallback AsyncCallback
		{
			get
			{
				return this.savedCallback;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x00059B3E File Offset: 0x00057D3E
		internal ExecutionContext Context
		{
			get
			{
				return this.savedContext;
			}
		}

		// Token: 0x040013A0 RID: 5024
		private AsyncCallback savedCallback;

		// Token: 0x040013A1 RID: 5025
		private ExecutionContext savedContext;
	}
}
