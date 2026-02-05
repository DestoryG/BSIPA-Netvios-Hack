using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005C8 RID: 1480
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ActiveDesignerEventArgs : EventArgs
	{
		// Token: 0x0600374B RID: 14155 RVA: 0x000F01B0 File Offset: 0x000EE3B0
		public ActiveDesignerEventArgs(IDesignerHost oldDesigner, IDesignerHost newDesigner)
		{
			this.oldDesigner = oldDesigner;
			this.newDesigner = newDesigner;
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x0600374C RID: 14156 RVA: 0x000F01C6 File Offset: 0x000EE3C6
		public IDesignerHost OldDesigner
		{
			get
			{
				return this.oldDesigner;
			}
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x0600374D RID: 14157 RVA: 0x000F01CE File Offset: 0x000EE3CE
		public IDesignerHost NewDesigner
		{
			get
			{
				return this.newDesigner;
			}
		}

		// Token: 0x04002ADE RID: 10974
		private readonly IDesignerHost oldDesigner;

		// Token: 0x04002ADF RID: 10975
		private readonly IDesignerHost newDesigner;
	}
}
