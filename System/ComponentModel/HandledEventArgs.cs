using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000556 RID: 1366
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class HandledEventArgs : EventArgs
	{
		// Token: 0x06003352 RID: 13138 RVA: 0x000E3BD6 File Offset: 0x000E1DD6
		public HandledEventArgs()
			: this(false)
		{
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x000E3BDF File Offset: 0x000E1DDF
		public HandledEventArgs(bool defaultHandledValue)
		{
			this.handled = defaultHandledValue;
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x06003354 RID: 13140 RVA: 0x000E3BEE File Offset: 0x000E1DEE
		// (set) Token: 0x06003355 RID: 13141 RVA: 0x000E3BF6 File Offset: 0x000E1DF6
		public bool Handled
		{
			get
			{
				return this.handled;
			}
			set
			{
				this.handled = value;
			}
		}

		// Token: 0x040029B1 RID: 10673
		private bool handled;
	}
}
