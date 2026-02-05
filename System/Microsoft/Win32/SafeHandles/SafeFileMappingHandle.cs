using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200002D RID: 45
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeFileMappingHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600029A RID: 666 RVA: 0x00010859 File Offset: 0x0000EA59
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeFileMappingHandle()
			: base(true)
		{
		}

		// Token: 0x0600029B RID: 667
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool CloseHandle(IntPtr handle);

		// Token: 0x0600029C RID: 668 RVA: 0x00010862 File Offset: 0x0000EA62
		protected override bool ReleaseHandle()
		{
			return SafeFileMappingHandle.CloseHandle(this.handle);
		}
	}
}
