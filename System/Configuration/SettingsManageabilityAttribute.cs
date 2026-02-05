using System;

namespace System.Configuration
{
	// Token: 0x020000A0 RID: 160
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class SettingsManageabilityAttribute : Attribute
	{
		// Token: 0x060005A2 RID: 1442 RVA: 0x00022A72 File Offset: 0x00020C72
		public SettingsManageabilityAttribute(SettingsManageability manageability)
		{
			this._manageability = manageability;
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00022A81 File Offset: 0x00020C81
		public SettingsManageability Manageability
		{
			get
			{
				return this._manageability;
			}
		}

		// Token: 0x04000C3B RID: 3131
		private readonly SettingsManageability _manageability;
	}
}
