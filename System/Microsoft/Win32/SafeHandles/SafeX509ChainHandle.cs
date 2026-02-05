using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000035 RID: 53
	[SecurityCritical]
	public sealed class SafeX509ChainHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060002BC RID: 700 RVA: 0x00010976 File Offset: 0x0000EB76
		private SafeX509ChainHandle()
			: base(true)
		{
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0001097F File Offset: 0x0000EB7F
		internal SafeX509ChainHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060002BE RID: 702 RVA: 0x00010990 File Offset: 0x0000EB90
		internal static SafeX509ChainHandle InvalidHandle
		{
			get
			{
				SafeX509ChainHandle safeX509ChainHandle = new SafeX509ChainHandle(IntPtr.Zero);
				GC.SuppressFinalize(safeX509ChainHandle);
				return safeX509ChainHandle;
			}
		}

		// Token: 0x060002BF RID: 703
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("crypt32.dll", SetLastError = true)]
		private static extern void CertFreeCertificateChain(IntPtr handle);

		// Token: 0x060002C0 RID: 704 RVA: 0x000109AF File Offset: 0x0000EBAF
		[SecurityCritical]
		protected override bool ReleaseHandle()
		{
			SafeX509ChainHandle.CertFreeCertificateChain(this.handle);
			return true;
		}
	}
}
