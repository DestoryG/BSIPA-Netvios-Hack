using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x02000019 RID: 25
	// (Invoke) Token: 0x060001C1 RID: 449
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void SessionEndingEventHandler(object sender, SessionEndingEventArgs e);
}
