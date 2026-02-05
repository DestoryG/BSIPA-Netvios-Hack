using System;

namespace System.ComponentModel.Design
{
	// Token: 0x020005EF RID: 1519
	public interface IExtenderProviderService
	{
		// Token: 0x06003826 RID: 14374
		void AddExtenderProvider(IExtenderProvider provider);

		// Token: 0x06003827 RID: 14375
		void RemoveExtenderProvider(IExtenderProvider provider);
	}
}
