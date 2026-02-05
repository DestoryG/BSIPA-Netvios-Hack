using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200002E RID: 46
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeFileMapViewHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600029D RID: 669 RVA: 0x0001086F File Offset: 0x0000EA6F
		internal SafeFileMapViewHandle()
			: base(true)
		{
		}

		// Token: 0x0600029E RID: 670
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		internal static extern SafeFileMapViewHandle MapViewOfFile(SafeFileMappingHandle hFileMappingObject, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, UIntPtr dwNumberOfBytesToMap);

		// Token: 0x0600029F RID: 671
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool UnmapViewOfFile(IntPtr handle);

		// Token: 0x060002A0 RID: 672 RVA: 0x00010878 File Offset: 0x0000EA78
		protected override bool ReleaseHandle()
		{
			return SafeFileMapViewHandle.UnmapViewOfFile(this.handle);
		}
	}
}
