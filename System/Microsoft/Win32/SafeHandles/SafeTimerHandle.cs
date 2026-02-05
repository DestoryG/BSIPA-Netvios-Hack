using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000032 RID: 50
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeTimerHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x0001091B File Offset: 0x0000EB1B
		internal SafeTimerHandle()
			: base(true)
		{
		}

		// Token: 0x060002B2 RID: 690
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool CloseHandle(IntPtr handle);

		// Token: 0x060002B3 RID: 691 RVA: 0x00010924 File Offset: 0x0000EB24
		protected override bool ReleaseHandle()
		{
			return SafeTimerHandle.CloseHandle(this.handle);
		}
	}
}
