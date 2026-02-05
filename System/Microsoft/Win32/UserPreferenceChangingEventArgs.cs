using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x02000025 RID: 37
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class UserPreferenceChangingEventArgs : EventArgs
	{
		// Token: 0x06000271 RID: 625 RVA: 0x0000F41F File Offset: 0x0000D61F
		public UserPreferenceChangingEventArgs(UserPreferenceCategory category)
		{
			this.category = category;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000F42E File Offset: 0x0000D62E
		public UserPreferenceCategory Category
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x0400038D RID: 909
		private readonly UserPreferenceCategory category;
	}
}
