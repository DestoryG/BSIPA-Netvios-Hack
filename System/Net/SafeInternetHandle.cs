using System;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001EC RID: 492
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeInternetHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060012EF RID: 4847 RVA: 0x0006402F File Offset: 0x0006222F
		public SafeInternetHandle()
			: base(true)
		{
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x00064038 File Offset: 0x00062238
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.WinHttp.WinHttpCloseHandle(this.handle);
		}
	}
}
