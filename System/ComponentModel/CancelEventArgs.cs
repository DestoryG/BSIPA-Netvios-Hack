using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000521 RID: 1313
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class CancelEventArgs : EventArgs
	{
		// Token: 0x060031CC RID: 12748 RVA: 0x000E006D File Offset: 0x000DE26D
		[global::__DynamicallyInvokable]
		public CancelEventArgs()
			: this(false)
		{
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x000E0076 File Offset: 0x000DE276
		[global::__DynamicallyInvokable]
		public CancelEventArgs(bool cancel)
		{
			this.cancel = cancel;
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x060031CE RID: 12750 RVA: 0x000E0085 File Offset: 0x000DE285
		// (set) Token: 0x060031CF RID: 12751 RVA: 0x000E008D File Offset: 0x000DE28D
		[global::__DynamicallyInvokable]
		public bool Cancel
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.cancel;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.cancel = value;
			}
		}

		// Token: 0x04002936 RID: 10550
		private bool cancel;
	}
}
