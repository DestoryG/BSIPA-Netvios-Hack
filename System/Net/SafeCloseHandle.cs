using System;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001E8 RID: 488
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeCloseHandle : CriticalHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060012E3 RID: 4835 RVA: 0x00063F52 File Offset: 0x00062152
		private SafeCloseHandle()
		{
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x00063F5A File Offset: 0x0006215A
		internal IntPtr DangerousGetHandle()
		{
			return this.handle;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00063F62 File Offset: 0x00062162
		protected override bool ReleaseHandle()
		{
			return this.IsInvalid || Interlocked.Increment(ref this._disposed) != 1 || UnsafeNclNativeMethods.SafeNetHandles.CloseHandle(this.handle);
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x00063F87 File Offset: 0x00062187
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Abort()
		{
			this.ReleaseHandle();
			base.SetHandleAsInvalid();
		}

		// Token: 0x0400152A RID: 5418
		private int _disposed;
	}
}
