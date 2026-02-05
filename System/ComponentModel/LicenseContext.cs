using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200057D RID: 1405
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class LicenseContext : IServiceProvider
	{
		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x060033F1 RID: 13297 RVA: 0x000E3F21 File Offset: 0x000E2121
		public virtual LicenseUsageMode UsageMode
		{
			get
			{
				return LicenseUsageMode.Runtime;
			}
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x000E3F24 File Offset: 0x000E2124
		public virtual string GetSavedLicenseKey(Type type, Assembly resourceAssembly)
		{
			return null;
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x000E3F27 File Offset: 0x000E2127
		public virtual object GetService(Type type)
		{
			return null;
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x000E3F2A File Offset: 0x000E212A
		public virtual void SetSavedLicenseKey(Type type, string key)
		{
		}
	}
}
