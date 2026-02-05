using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000530 RID: 1328
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class ContainerFilterService
	{
		// Token: 0x0600322D RID: 12845 RVA: 0x000E10B0 File Offset: 0x000DF2B0
		public virtual ComponentCollection FilterComponents(ComponentCollection components)
		{
			return components;
		}
	}
}
