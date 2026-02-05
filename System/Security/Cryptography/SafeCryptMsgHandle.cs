using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x0200045C RID: 1116
	internal sealed class SafeCryptMsgHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600297A RID: 10618 RVA: 0x000BC5B1 File Offset: 0x000BA7B1
		private SafeCryptMsgHandle()
			: base(true)
		{
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x000BC5BA File Offset: 0x000BA7BA
		internal SafeCryptMsgHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x0600297C RID: 10620 RVA: 0x000BC5CC File Offset: 0x000BA7CC
		internal static SafeCryptMsgHandle InvalidHandle
		{
			get
			{
				SafeCryptMsgHandle safeCryptMsgHandle = new SafeCryptMsgHandle(IntPtr.Zero);
				GC.SuppressFinalize(safeCryptMsgHandle);
				return safeCryptMsgHandle;
			}
		}

		// Token: 0x0600297D RID: 10621
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("crypt32.dll", SetLastError = true)]
		private static extern bool CryptMsgClose(IntPtr handle);

		// Token: 0x0600297E RID: 10622 RVA: 0x000BC5EB File Offset: 0x000BA7EB
		protected override bool ReleaseHandle()
		{
			return SafeCryptMsgHandle.CryptMsgClose(this.handle);
		}
	}
}
