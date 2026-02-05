using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000580 RID: 1408
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class LicenseProvider
	{
		// Token: 0x06003412 RID: 13330
		public abstract License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions);
	}
}
