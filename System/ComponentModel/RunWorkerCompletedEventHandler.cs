using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005A9 RID: 1449
	// (Invoke) Token: 0x0600360C RID: 13836
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void RunWorkerCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e);
}
