using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200050C RID: 1292
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class AddingNewEventArgs : EventArgs
	{
		// Token: 0x060030FF RID: 12543 RVA: 0x000DE7D2 File Offset: 0x000DC9D2
		public AddingNewEventArgs()
		{
		}

		// Token: 0x06003100 RID: 12544 RVA: 0x000DE7DA File Offset: 0x000DC9DA
		public AddingNewEventArgs(object newObject)
		{
			this.newObject = newObject;
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x000DE7E9 File Offset: 0x000DC9E9
		// (set) Token: 0x06003102 RID: 12546 RVA: 0x000DE7F1 File Offset: 0x000DC9F1
		public object NewObject
		{
			get
			{
				return this.newObject;
			}
			set
			{
				this.newObject = value;
			}
		}

		// Token: 0x040028F9 RID: 10489
		private object newObject;
	}
}
