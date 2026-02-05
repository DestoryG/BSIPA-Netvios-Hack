using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005C9 RID: 1481
	// (Invoke) Token: 0x0600374F RID: 14159
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ActiveDesignerEventHandler(object sender, ActiveDesignerEventArgs e);
}
