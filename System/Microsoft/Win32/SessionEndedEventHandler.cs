using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x02000017 RID: 23
	// (Invoke) Token: 0x060001B9 RID: 441
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void SessionEndedEventHandler(object sender, SessionEndedEventArgs e);
}
