using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x02000023 RID: 35
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class UserPreferenceChangedEventArgs : EventArgs
	{
		// Token: 0x0600026B RID: 619 RVA: 0x0000F408 File Offset: 0x0000D608
		public UserPreferenceChangedEventArgs(UserPreferenceCategory category)
		{
			this.category = category;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000F417 File Offset: 0x0000D617
		public UserPreferenceCategory Category
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x0400038C RID: 908
		private readonly UserPreferenceCategory category;
	}
}
