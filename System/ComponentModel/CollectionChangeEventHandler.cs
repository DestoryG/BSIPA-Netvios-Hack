using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000527 RID: 1319
	// (Invoke) Token: 0x060031F1 RID: 12785
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void CollectionChangeEventHandler(object sender, CollectionChangeEventArgs e);
}
