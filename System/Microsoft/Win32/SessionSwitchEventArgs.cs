using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x0200001B RID: 27
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class SessionSwitchEventArgs : EventArgs
	{
		// Token: 0x060001C4 RID: 452 RVA: 0x0000D54B File Offset: 0x0000B74B
		public SessionSwitchEventArgs(SessionSwitchReason reason)
		{
			this.reason = reason;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000D55A File Offset: 0x0000B75A
		public SessionSwitchReason Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x04000306 RID: 774
		private readonly SessionSwitchReason reason;
	}
}
