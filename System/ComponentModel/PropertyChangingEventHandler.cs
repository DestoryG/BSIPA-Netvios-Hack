using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200059B RID: 1435
	// (Invoke) Token: 0x0600352A RID: 13610
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void PropertyChangingEventHandler(object sender, PropertyChangingEventArgs e);
}
