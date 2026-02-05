using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x0200045B RID: 1115
	internal sealed class SafeCertStoreHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002975 RID: 10613 RVA: 0x000BC568 File Offset: 0x000BA768
		private SafeCertStoreHandle()
			: base(true)
		{
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x000BC571 File Offset: 0x000BA771
		internal SafeCertStoreHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06002977 RID: 10615 RVA: 0x000BC584 File Offset: 0x000BA784
		internal static SafeCertStoreHandle InvalidHandle
		{
			get
			{
				SafeCertStoreHandle safeCertStoreHandle = new SafeCertStoreHandle(IntPtr.Zero);
				GC.SuppressFinalize(safeCertStoreHandle);
				return safeCertStoreHandle;
			}
		}

		// Token: 0x06002978 RID: 10616
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("crypt32.dll", SetLastError = true)]
		private static extern bool CertCloseStore(IntPtr hCertStore, uint dwFlags);

		// Token: 0x06002979 RID: 10617 RVA: 0x000BC5A3 File Offset: 0x000BA7A3
		protected override bool ReleaseHandle()
		{
			return SafeCertStoreHandle.CertCloseStore(this.handle, 0U);
		}
	}
}
