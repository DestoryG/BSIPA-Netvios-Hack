using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000518 RID: 1304
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class ComponentEditor
	{
		// Token: 0x06003166 RID: 12646 RVA: 0x000DF4EC File Offset: 0x000DD6EC
		public bool EditComponent(object component)
		{
			return this.EditComponent(null, component);
		}

		// Token: 0x06003167 RID: 12647
		public abstract bool EditComponent(ITypeDescriptorContext context, object component);
	}
}
