using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001F6 RID: 502
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCertChainList : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001316 RID: 4886 RVA: 0x000644F4 File Offset: 0x000626F4
		internal SafeFreeCertChainList()
			: base(true)
		{
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00064500 File Offset: 0x00062700
		public override string ToString()
		{
			return "0x" + base.DangerousGetHandle().ToString("x");
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0006452A File Offset: 0x0006272A
		protected override bool ReleaseHandle()
		{
			UnsafeNclNativeMethods.SafeNetHandles.CertFreeCertificateChainList(this.handle);
			return true;
		}
	}
}
