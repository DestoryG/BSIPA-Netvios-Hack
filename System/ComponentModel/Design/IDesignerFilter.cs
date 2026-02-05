using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x020005E8 RID: 1512
	public interface IDesignerFilter
	{
		// Token: 0x060037F5 RID: 14325
		void PostFilterAttributes(IDictionary attributes);

		// Token: 0x060037F6 RID: 14326
		void PostFilterEvents(IDictionary events);

		// Token: 0x060037F7 RID: 14327
		void PostFilterProperties(IDictionary properties);

		// Token: 0x060037F8 RID: 14328
		void PreFilterAttributes(IDictionary attributes);

		// Token: 0x060037F9 RID: 14329
		void PreFilterEvents(IDictionary events);

		// Token: 0x060037FA RID: 14330
		void PreFilterProperties(IDictionary properties);
	}
}
