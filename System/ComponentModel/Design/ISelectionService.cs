using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x020005F6 RID: 1526
	[ComVisible(true)]
	public interface ISelectionService
	{
		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06003842 RID: 14402
		object PrimarySelection { get; }

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06003843 RID: 14403
		int SelectionCount { get; }

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06003844 RID: 14404
		// (remove) Token: 0x06003845 RID: 14405
		event EventHandler SelectionChanged;

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06003846 RID: 14406
		// (remove) Token: 0x06003847 RID: 14407
		event EventHandler SelectionChanging;

		// Token: 0x06003848 RID: 14408
		bool GetComponentSelected(object component);

		// Token: 0x06003849 RID: 14409
		ICollection GetSelectedComponents();

		// Token: 0x0600384A RID: 14410
		void SetSelectedComponents(ICollection components);

		// Token: 0x0600384B RID: 14411
		void SetSelectedComponents(ICollection components, SelectionTypes selectionType);
	}
}
