using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000613 RID: 1555
	// (Invoke) Token: 0x060038E4 RID: 14564
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ResolveNameEventHandler(object sender, ResolveNameEventArgs e);
}
