using System;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001EB RID: 491
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeCloseIcmpHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060012ED RID: 4845 RVA: 0x00064019 File Offset: 0x00062219
		private SafeCloseIcmpHandle()
			: base(true)
		{
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x00064022 File Offset: 0x00062222
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			return UnsafeNetInfoNativeMethods.IcmpCloseHandle(this.handle);
		}
	}
}
