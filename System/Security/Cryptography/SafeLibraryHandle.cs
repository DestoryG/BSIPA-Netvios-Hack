using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x02000457 RID: 1111
	internal sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002963 RID: 10595 RVA: 0x000BC473 File Offset: 0x000BA673
		private SafeLibraryHandle()
			: base(true)
		{
		}

		// Token: 0x06002964 RID: 10596
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool FreeLibrary([In] IntPtr hModule);

		// Token: 0x06002965 RID: 10597 RVA: 0x000BC47C File Offset: 0x000BA67C
		protected override bool ReleaseHandle()
		{
			return SafeLibraryHandle.FreeLibrary(this.handle);
		}
	}
}
