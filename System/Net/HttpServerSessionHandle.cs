using System;
using System.Security;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001EA RID: 490
	[SuppressUnmanagedCodeSecurity]
	internal sealed class HttpServerSessionHandle : CriticalHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060012EA RID: 4842 RVA: 0x00063FCE File Offset: 0x000621CE
		internal HttpServerSessionHandle(ulong id)
		{
			this.serverSessionId = id;
			base.SetHandle(new IntPtr(1));
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00063FE9 File Offset: 0x000621E9
		internal ulong DangerousGetServerSessionId()
		{
			return this.serverSessionId;
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x00063FF1 File Offset: 0x000621F1
		protected override bool ReleaseHandle()
		{
			return this.IsInvalid || Interlocked.Increment(ref this.disposed) != 1 || UnsafeNclNativeMethods.HttpApi.HttpCloseServerSession(this.serverSessionId) == 0U;
		}

		// Token: 0x0400152C RID: 5420
		private int disposed;

		// Token: 0x0400152D RID: 5421
		private ulong serverSessionId;
	}
}
