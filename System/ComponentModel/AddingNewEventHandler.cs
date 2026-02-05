using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200050D RID: 1293
	// (Invoke) Token: 0x06003104 RID: 12548
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void AddingNewEventHandler(object sender, AddingNewEventArgs e);
}
