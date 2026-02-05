using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000586 RID: 1414
	// (Invoke) Token: 0x06003430 RID: 13360
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ListChangedEventHandler(object sender, ListChangedEventArgs e);
}
