using System;

namespace System.ComponentModel.Design
{
	// Token: 0x020005E7 RID: 1511
	public interface IDesignerEventService
	{
		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x060037EB RID: 14315
		IDesignerHost ActiveDesigner { get; }

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x060037EC RID: 14316
		DesignerCollection Designers { get; }

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x060037ED RID: 14317
		// (remove) Token: 0x060037EE RID: 14318
		event ActiveDesignerEventHandler ActiveDesignerChanged;

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x060037EF RID: 14319
		// (remove) Token: 0x060037F0 RID: 14320
		event DesignerEventHandler DesignerCreated;

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x060037F1 RID: 14321
		// (remove) Token: 0x060037F2 RID: 14322
		event DesignerEventHandler DesignerDisposed;

		// Token: 0x1400005D RID: 93
		// (add) Token: 0x060037F3 RID: 14323
		// (remove) Token: 0x060037F4 RID: 14324
		event EventHandler SelectionChanged;
	}
}
