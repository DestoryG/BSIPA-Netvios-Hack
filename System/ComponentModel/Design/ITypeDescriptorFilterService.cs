using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x020005F9 RID: 1529
	public interface ITypeDescriptorFilterService
	{
		// Token: 0x06003854 RID: 14420
		bool FilterAttributes(IComponent component, IDictionary attributes);

		// Token: 0x06003855 RID: 14421
		bool FilterEvents(IComponent component, IDictionary events);

		// Token: 0x06003856 RID: 14422
		bool FilterProperties(IComponent component, IDictionary properties);
	}
}
