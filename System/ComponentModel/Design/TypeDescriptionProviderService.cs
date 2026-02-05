using System;

namespace System.ComponentModel.Design
{
	// Token: 0x020005C7 RID: 1479
	public abstract class TypeDescriptionProviderService
	{
		// Token: 0x06003748 RID: 14152
		public abstract TypeDescriptionProvider GetProvider(object instance);

		// Token: 0x06003749 RID: 14153
		public abstract TypeDescriptionProvider GetProvider(Type type);
	}
}
