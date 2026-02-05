using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x0200001C RID: 28
	// (Invoke) Token: 0x060001C7 RID: 455
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void SessionSwitchEventHandler(object sender, SessionSwitchEventArgs e);
}
