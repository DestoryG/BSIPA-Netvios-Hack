using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x020005ED RID: 1517
	[ComVisible(true)]
	public interface IEventBindingService
	{
		// Token: 0x0600381D RID: 14365
		string CreateUniqueMethodName(IComponent component, EventDescriptor e);

		// Token: 0x0600381E RID: 14366
		ICollection GetCompatibleMethods(EventDescriptor e);

		// Token: 0x0600381F RID: 14367
		EventDescriptor GetEvent(PropertyDescriptor property);

		// Token: 0x06003820 RID: 14368
		PropertyDescriptorCollection GetEventProperties(EventDescriptorCollection events);

		// Token: 0x06003821 RID: 14369
		PropertyDescriptor GetEventProperty(EventDescriptor e);

		// Token: 0x06003822 RID: 14370
		bool ShowCode();

		// Token: 0x06003823 RID: 14371
		bool ShowCode(int lineNumber);

		// Token: 0x06003824 RID: 14372
		bool ShowCode(IComponent component, EventDescriptor e);
	}
}
