using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x020005E9 RID: 1513
	[ComVisible(true)]
	public interface IDesignerHost : IServiceContainer, IServiceProvider
	{
		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x060037FB RID: 14331
		bool Loading { get; }

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x060037FC RID: 14332
		bool InTransaction { get; }

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x060037FD RID: 14333
		IContainer Container { get; }

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x060037FE RID: 14334
		IComponent RootComponent { get; }

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x060037FF RID: 14335
		string RootComponentClassName { get; }

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06003800 RID: 14336
		string TransactionDescription { get; }

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x06003801 RID: 14337
		// (remove) Token: 0x06003802 RID: 14338
		event EventHandler Activated;

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x06003803 RID: 14339
		// (remove) Token: 0x06003804 RID: 14340
		event EventHandler Deactivated;

		// Token: 0x14000060 RID: 96
		// (add) Token: 0x06003805 RID: 14341
		// (remove) Token: 0x06003806 RID: 14342
		event EventHandler LoadComplete;

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x06003807 RID: 14343
		// (remove) Token: 0x06003808 RID: 14344
		event DesignerTransactionCloseEventHandler TransactionClosed;

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x06003809 RID: 14345
		// (remove) Token: 0x0600380A RID: 14346
		event DesignerTransactionCloseEventHandler TransactionClosing;

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x0600380B RID: 14347
		// (remove) Token: 0x0600380C RID: 14348
		event EventHandler TransactionOpened;

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x0600380D RID: 14349
		// (remove) Token: 0x0600380E RID: 14350
		event EventHandler TransactionOpening;

		// Token: 0x0600380F RID: 14351
		void Activate();

		// Token: 0x06003810 RID: 14352
		IComponent CreateComponent(Type componentClass);

		// Token: 0x06003811 RID: 14353
		IComponent CreateComponent(Type componentClass, string name);

		// Token: 0x06003812 RID: 14354
		DesignerTransaction CreateTransaction();

		// Token: 0x06003813 RID: 14355
		DesignerTransaction CreateTransaction(string description);

		// Token: 0x06003814 RID: 14356
		void DestroyComponent(IComponent component);

		// Token: 0x06003815 RID: 14357
		IDesigner GetDesigner(IComponent component);

		// Token: 0x06003816 RID: 14358
		Type GetType(string typeName);
	}
}
