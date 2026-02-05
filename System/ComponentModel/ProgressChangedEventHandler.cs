using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000597 RID: 1431
	// (Invoke) Token: 0x0600351E RID: 13598
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);
}
