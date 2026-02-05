using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005CF RID: 1487
	// (Invoke) Token: 0x0600376B RID: 14187
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ComponentChangingEventHandler(object sender, ComponentChangingEventArgs e);
}
