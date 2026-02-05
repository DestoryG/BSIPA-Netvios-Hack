using System;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001ED RID: 493
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeSspiAuthDataHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060012F1 RID: 4849 RVA: 0x00064045 File Offset: 0x00062245
		public SafeSspiAuthDataHandle()
			: base(true)
		{
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0006404E File Offset: 0x0006224E
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SspiHelper.SspiFreeAuthIdentity(this.handle) == SecurityStatus.OK;
		}
	}
}
