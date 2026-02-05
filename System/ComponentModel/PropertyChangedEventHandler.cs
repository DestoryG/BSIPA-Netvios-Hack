using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000599 RID: 1433
	// (Invoke) Token: 0x06003524 RID: 13604
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);
}
