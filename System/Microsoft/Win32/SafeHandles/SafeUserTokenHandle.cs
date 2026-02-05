using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000034 RID: 52
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeUserTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x00010950 File Offset: 0x0000EB50
		internal SafeUserTokenHandle()
			: base(true)
		{
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00010959 File Offset: 0x0000EB59
		internal SafeUserTokenHandle(IntPtr existingHandle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(existingHandle);
		}

		// Token: 0x060002B9 RID: 697
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool DuplicateTokenEx(SafeHandle hToken, int access, NativeMethods.SECURITY_ATTRIBUTES tokenAttributes, int impersonationLevel, int tokenType, out SafeUserTokenHandle hNewToken);

		// Token: 0x060002BA RID: 698
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool CloseHandle(IntPtr handle);

		// Token: 0x060002BB RID: 699 RVA: 0x00010969 File Offset: 0x0000EB69
		protected override bool ReleaseHandle()
		{
			return SafeUserTokenHandle.CloseHandle(this.handle);
		}
	}
}
