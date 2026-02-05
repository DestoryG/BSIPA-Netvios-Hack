using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005D0 RID: 1488
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ComponentEventArgs : EventArgs
	{
		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x0600376E RID: 14190 RVA: 0x000F0361 File Offset: 0x000EE561
		public virtual IComponent Component
		{
			get
			{
				return this.component;
			}
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x000F0369 File Offset: 0x000EE569
		public ComponentEventArgs(IComponent component)
		{
			this.component = component;
		}

		// Token: 0x04002AE9 RID: 10985
		private IComponent component;
	}
}
