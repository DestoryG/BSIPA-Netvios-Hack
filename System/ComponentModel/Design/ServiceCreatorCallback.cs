using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005FE RID: 1534
	// (Invoke) Token: 0x06003873 RID: 14451
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate object ServiceCreatorCallback(IServiceContainer container, Type serviceType);
}
