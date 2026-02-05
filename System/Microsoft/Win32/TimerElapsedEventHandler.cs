using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x02000020 RID: 32
	// (Invoke) Token: 0x06000218 RID: 536
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void TimerElapsedEventHandler(object sender, TimerElapsedEventArgs e);
}
