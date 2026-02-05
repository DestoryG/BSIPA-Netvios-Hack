using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000526 RID: 1318
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class CollectionChangeEventArgs : EventArgs
	{
		// Token: 0x060031ED RID: 12781 RVA: 0x000E0418 File Offset: 0x000DE618
		public CollectionChangeEventArgs(CollectionChangeAction action, object element)
		{
			this.action = action;
			this.element = element;
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x060031EE RID: 12782 RVA: 0x000E042E File Offset: 0x000DE62E
		public virtual CollectionChangeAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x060031EF RID: 12783 RVA: 0x000E0436 File Offset: 0x000DE636
		public virtual object Element
		{
			get
			{
				return this.element;
			}
		}

		// Token: 0x0400294B RID: 10571
		private CollectionChangeAction action;

		// Token: 0x0400294C RID: 10572
		private object element;
	}
}
