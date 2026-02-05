using System;
using System.Collections;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005DA RID: 1498
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesigntimeLicenseContext : LicenseContext
	{
		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x060037AF RID: 14255 RVA: 0x000F0812 File Offset: 0x000EEA12
		public override LicenseUsageMode UsageMode
		{
			get
			{
				return LicenseUsageMode.Designtime;
			}
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x000F0815 File Offset: 0x000EEA15
		public override string GetSavedLicenseKey(Type type, Assembly resourceAssembly)
		{
			return null;
		}

		// Token: 0x060037B1 RID: 14257 RVA: 0x000F0818 File Offset: 0x000EEA18
		public override void SetSavedLicenseKey(Type type, string key)
		{
			this.savedLicenseKeys[type.AssemblyQualifiedName] = key;
		}

		// Token: 0x04002AF4 RID: 10996
		internal Hashtable savedLicenseKeys = new Hashtable();
	}
}
