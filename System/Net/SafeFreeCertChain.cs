using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001F5 RID: 501
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCertChain : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001312 RID: 4882 RVA: 0x00064499 File Offset: 0x00062699
		internal SafeFreeCertChain(IntPtr handle)
			: base(false)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x000644A9 File Offset: 0x000626A9
		internal SafeFreeCertChain(IntPtr handle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x000644BC File Offset: 0x000626BC
		public override string ToString()
		{
			return "0x" + base.DangerousGetHandle().ToString("x");
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x000644E6 File Offset: 0x000626E6
		protected override bool ReleaseHandle()
		{
			UnsafeNclNativeMethods.SafeNetHandles.CertFreeCertificateChain(this.handle);
			return true;
		}
	}
}
