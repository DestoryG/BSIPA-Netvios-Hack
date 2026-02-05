using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x02000459 RID: 1113
	internal sealed class SafeCryptProvHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600296B RID: 10603 RVA: 0x000BC4DA File Offset: 0x000BA6DA
		private SafeCryptProvHandle()
			: base(true)
		{
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x000BC4E3 File Offset: 0x000BA6E3
		internal SafeCryptProvHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x0600296D RID: 10605 RVA: 0x000BC4F4 File Offset: 0x000BA6F4
		internal static SafeCryptProvHandle InvalidHandle
		{
			get
			{
				SafeCryptProvHandle safeCryptProvHandle = new SafeCryptProvHandle(IntPtr.Zero);
				GC.SuppressFinalize(safeCryptProvHandle);
				return safeCryptProvHandle;
			}
		}

		// Token: 0x0600296E RID: 10606
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool CryptReleaseContext(IntPtr hCryptProv, uint dwFlags);

		// Token: 0x0600296F RID: 10607 RVA: 0x000BC513 File Offset: 0x000BA713
		protected override bool ReleaseHandle()
		{
			return SafeCryptProvHandle.CryptReleaseContext(this.handle, 0U);
		}
	}
}
