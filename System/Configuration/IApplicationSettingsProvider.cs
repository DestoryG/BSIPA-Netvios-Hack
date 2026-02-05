using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x0200008E RID: 142
	public interface IApplicationSettingsProvider
	{
		// Token: 0x06000567 RID: 1383
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property);

		// Token: 0x06000568 RID: 1384
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Reset(SettingsContext context);

		// Token: 0x06000569 RID: 1385
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Upgrade(SettingsContext context, SettingsPropertyCollection properties);
	}
}
