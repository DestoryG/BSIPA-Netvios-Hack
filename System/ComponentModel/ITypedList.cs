using System;

namespace System.ComponentModel
{
	// Token: 0x0200057B RID: 1403
	public interface ITypedList
	{
		// Token: 0x060033EC RID: 13292
		string GetListName(PropertyDescriptor[] listAccessors);

		// Token: 0x060033ED RID: 13293
		PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors);
	}
}
