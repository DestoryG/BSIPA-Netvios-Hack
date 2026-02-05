using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200002C RID: 44
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeEventLogWriteHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000296 RID: 662 RVA: 0x00010843 File Offset: 0x0000EA43
		internal SafeEventLogWriteHandle()
			: base(true)
		{
		}

		// Token: 0x06000297 RID: 663
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern SafeEventLogWriteHandle RegisterEventSource(string uncServerName, string sourceName);

		// Token: 0x06000298 RID: 664
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool DeregisterEventSource(IntPtr hEventLog);

		// Token: 0x06000299 RID: 665 RVA: 0x0001084C File Offset: 0x0000EA4C
		protected override bool ReleaseHandle()
		{
			return SafeEventLogWriteHandle.DeregisterEventSource(this.handle);
		}
	}
}
