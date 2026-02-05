using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200054A RID: 1354
	// (Invoke) Token: 0x060032E0 RID: 13024
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void DoWorkEventHandler(object sender, DoWorkEventArgs e);
}
