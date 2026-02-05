using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005D3 RID: 1491
	// (Invoke) Token: 0x06003779 RID: 14201
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ComponentRenameEventHandler(object sender, ComponentRenameEventArgs e);
}
