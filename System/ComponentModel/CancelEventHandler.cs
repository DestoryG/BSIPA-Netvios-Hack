using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000522 RID: 1314
	// (Invoke) Token: 0x060031D1 RID: 12753
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void CancelEventHandler(object sender, CancelEventArgs e);
}
