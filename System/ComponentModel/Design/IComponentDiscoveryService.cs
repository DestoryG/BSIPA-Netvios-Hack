using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x020005E4 RID: 1508
	public interface IComponentDiscoveryService
	{
		// Token: 0x060037E4 RID: 14308
		ICollection GetComponentTypes(IDesignerHost designerHost, Type baseType);
	}
}
