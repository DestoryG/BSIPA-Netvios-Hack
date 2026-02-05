using System;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001F8 RID: 504
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCertContext : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600131F RID: 4895 RVA: 0x0006477C File Offset: 0x0006297C
		internal SafeFreeCertContext()
			: base(true)
		{
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00064785 File Offset: 0x00062985
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Set(IntPtr value)
		{
			this.handle = value;
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0006478E File Offset: 0x0006298E
		protected override bool ReleaseHandle()
		{
			UnsafeNclNativeMethods.SafeNetHandles.CertFreeCertificateContext(this.handle);
			return true;
		}

		// Token: 0x04001543 RID: 5443
		private const string CRYPT32 = "crypt32.dll";

		// Token: 0x04001544 RID: 5444
		private const string ADVAPI32 = "advapi32.dll";

		// Token: 0x04001545 RID: 5445
		private const uint CRYPT_ACQUIRE_SILENT_FLAG = 64U;
	}
}
