using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x02000016 RID: 22
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class SessionEndedEventArgs : EventArgs
	{
		// Token: 0x060001B6 RID: 438 RVA: 0x0000D50C File Offset: 0x0000B70C
		public SessionEndedEventArgs(SessionEndReasons reason)
		{
			this.reason = reason;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000D51B File Offset: 0x0000B71B
		public SessionEndReasons Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x04000300 RID: 768
		private readonly SessionEndReasons reason;
	}
}
