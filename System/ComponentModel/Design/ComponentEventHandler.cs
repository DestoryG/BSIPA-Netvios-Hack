using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005D1 RID: 1489
	// (Invoke) Token: 0x06003771 RID: 14193
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ComponentEventHandler(object sender, ComponentEventArgs e);
}
