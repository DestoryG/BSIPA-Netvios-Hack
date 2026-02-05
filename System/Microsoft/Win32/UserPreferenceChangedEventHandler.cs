using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x02000024 RID: 36
	// (Invoke) Token: 0x0600026E RID: 622
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void UserPreferenceChangedEventHandler(object sender, UserPreferenceChangedEventArgs e);
}
