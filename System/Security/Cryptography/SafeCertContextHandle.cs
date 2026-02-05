using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x0200045A RID: 1114
	internal sealed class SafeCertContextHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002970 RID: 10608 RVA: 0x000BC521 File Offset: 0x000BA721
		private SafeCertContextHandle()
			: base(true)
		{
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x000BC52A File Offset: 0x000BA72A
		internal SafeCertContextHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x06002972 RID: 10610 RVA: 0x000BC53C File Offset: 0x000BA73C
		internal static SafeCertContextHandle InvalidHandle
		{
			get
			{
				SafeCertContextHandle safeCertContextHandle = new SafeCertContextHandle(IntPtr.Zero);
				GC.SuppressFinalize(safeCertContextHandle);
				return safeCertContextHandle;
			}
		}

		// Token: 0x06002973 RID: 10611
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("crypt32.dll", SetLastError = true)]
		private static extern bool CertFreeCertificateContext(IntPtr pCertContext);

		// Token: 0x06002974 RID: 10612 RVA: 0x000BC55B File Offset: 0x000BA75B
		protected override bool ReleaseHandle()
		{
			return SafeCertContextHandle.CertFreeCertificateContext(this.handle);
		}
	}
}
