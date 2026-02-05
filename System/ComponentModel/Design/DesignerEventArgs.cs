using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005DE RID: 1502
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesignerEventArgs : EventArgs
	{
		// Token: 0x060037C6 RID: 14278 RVA: 0x000F0D86 File Offset: 0x000EEF86
		public DesignerEventArgs(IDesignerHost host)
		{
			this.host = host;
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x060037C7 RID: 14279 RVA: 0x000F0D95 File Offset: 0x000EEF95
		public IDesignerHost Designer
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x04002AF9 RID: 11001
		private readonly IDesignerHost host;
	}
}
