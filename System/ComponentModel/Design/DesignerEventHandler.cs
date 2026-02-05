using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005DF RID: 1503
	// (Invoke) Token: 0x060037C9 RID: 14281
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void DesignerEventHandler(object sender, DesignerEventArgs e);
}
