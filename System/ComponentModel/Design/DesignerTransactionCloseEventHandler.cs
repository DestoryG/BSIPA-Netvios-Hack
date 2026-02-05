using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005D7 RID: 1495
	// (Invoke) Token: 0x06003795 RID: 14229
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void DesignerTransactionCloseEventHandler(object sender, DesignerTransactionCloseEventArgs e);
}
