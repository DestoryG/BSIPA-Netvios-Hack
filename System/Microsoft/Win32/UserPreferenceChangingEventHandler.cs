using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x02000026 RID: 38
	// (Invoke) Token: 0x06000274 RID: 628
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void UserPreferenceChangingEventHandler(object sender, UserPreferenceChangingEventArgs e);
}
