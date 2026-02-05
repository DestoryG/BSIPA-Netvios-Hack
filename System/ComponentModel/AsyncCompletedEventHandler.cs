using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000512 RID: 1298
	// (Invoke) Token: 0x06003124 RID: 12580
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void AsyncCompletedEventHandler(object sender, AsyncCompletedEventArgs e);
}
