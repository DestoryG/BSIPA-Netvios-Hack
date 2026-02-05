using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x02000458 RID: 1112
	internal sealed class SafeLocalAllocHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002966 RID: 10598 RVA: 0x000BC489 File Offset: 0x000BA689
		private SafeLocalAllocHandle()
			: base(true)
		{
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x000BC492 File Offset: 0x000BA692
		internal SafeLocalAllocHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x06002968 RID: 10600 RVA: 0x000BC4A4 File Offset: 0x000BA6A4
		internal static SafeLocalAllocHandle InvalidHandle
		{
			get
			{
				SafeLocalAllocHandle safeLocalAllocHandle = new SafeLocalAllocHandle(IntPtr.Zero);
				GC.SuppressFinalize(safeLocalAllocHandle);
				return safeLocalAllocHandle;
			}
		}

		// Token: 0x06002969 RID: 10601
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr LocalFree(IntPtr handle);

		// Token: 0x0600296A RID: 10602 RVA: 0x000BC4C3 File Offset: 0x000BA6C3
		protected override bool ReleaseHandle()
		{
			return SafeLocalAllocHandle.LocalFree(this.handle) == IntPtr.Zero;
		}
	}
}
