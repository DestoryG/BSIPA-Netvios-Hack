using System;
using System.Security;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001E9 RID: 489
	[SuppressUnmanagedCodeSecurity]
	internal sealed class HttpRequestQueueV2Handle : CriticalHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060012E7 RID: 4839 RVA: 0x00063F96 File Offset: 0x00062196
		private HttpRequestQueueV2Handle()
		{
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x00063F9E File Offset: 0x0006219E
		internal IntPtr DangerousGetHandle()
		{
			return this.handle;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x00063FA6 File Offset: 0x000621A6
		protected override bool ReleaseHandle()
		{
			return this.IsInvalid || Interlocked.Increment(ref this.disposed) != 1 || UnsafeNclNativeMethods.SafeNetHandles.HttpCloseRequestQueue(this.handle) == 0U;
		}

		// Token: 0x0400152B RID: 5419
		private int disposed;
	}
}
