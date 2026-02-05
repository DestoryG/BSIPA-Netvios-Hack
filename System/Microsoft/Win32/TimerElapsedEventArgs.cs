using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x0200001F RID: 31
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class TimerElapsedEventArgs : EventArgs
	{
		// Token: 0x06000215 RID: 533 RVA: 0x0000F2D7 File Offset: 0x0000D4D7
		public TimerElapsedEventArgs(IntPtr timerId)
		{
			this.timerId = timerId;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000F2E6 File Offset: 0x0000D4E6
		public IntPtr TimerId
		{
			get
			{
				return this.timerId;
			}
		}

		// Token: 0x0400033A RID: 826
		private readonly IntPtr timerId;
	}
}
