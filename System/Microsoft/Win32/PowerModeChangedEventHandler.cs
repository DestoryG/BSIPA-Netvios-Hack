using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x02000013 RID: 19
	// (Invoke) Token: 0x0600019F RID: 415
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void PowerModeChangedEventHandler(object sender, PowerModeChangedEventArgs e);
}
