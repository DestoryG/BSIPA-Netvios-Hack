using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x02000018 RID: 24
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class SessionEndingEventArgs : EventArgs
	{
		// Token: 0x060001BC RID: 444 RVA: 0x0000D523 File Offset: 0x0000B723
		public SessionEndingEventArgs(SessionEndReasons reason)
		{
			this.reason = reason;
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000D532 File Offset: 0x0000B732
		// (set) Token: 0x060001BE RID: 446 RVA: 0x0000D53A File Offset: 0x0000B73A
		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				this.cancel = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000D543 File Offset: 0x0000B743
		public SessionEndReasons Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x04000301 RID: 769
		private bool cancel;

		// Token: 0x04000302 RID: 770
		private readonly SessionEndReasons reason;
	}
}
