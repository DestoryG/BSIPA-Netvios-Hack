using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200002F RID: 47
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060002A1 RID: 673 RVA: 0x00010885 File Offset: 0x0000EA85
		internal SafeLibraryHandle()
			: base(true)
		{
		}

		// Token: 0x060002A2 RID: 674
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern SafeLibraryHandle LoadLibraryEx(string libFilename, IntPtr reserved, int flags);

		// Token: 0x060002A3 RID: 675
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern bool FreeLibrary(IntPtr hModule);

		// Token: 0x060002A4 RID: 676 RVA: 0x0001088E File Offset: 0x0000EA8E
		protected override bool ReleaseHandle()
		{
			return SafeLibraryHandle.FreeLibrary(this.handle);
		}
	}
}
