using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200002B RID: 43
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeEventLogReadHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000292 RID: 658 RVA: 0x0001082D File Offset: 0x0000EA2D
		internal SafeEventLogReadHandle()
			: base(true)
		{
		}

		// Token: 0x06000293 RID: 659
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern SafeEventLogReadHandle OpenEventLog(string UNCServerName, string sourceName);

		// Token: 0x06000294 RID: 660
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool CloseEventLog(IntPtr hEventLog);

		// Token: 0x06000295 RID: 661 RVA: 0x00010836 File Offset: 0x0000EA36
		protected override bool ReleaseHandle()
		{
			return SafeEventLogReadHandle.CloseEventLog(this.handle);
		}
	}
}
