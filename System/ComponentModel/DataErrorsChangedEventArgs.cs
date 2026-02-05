using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000533 RID: 1331
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DataErrorsChangedEventArgs : EventArgs
	{
		// Token: 0x06003246 RID: 12870 RVA: 0x000E1570 File Offset: 0x000DF770
		[global::__DynamicallyInvokable]
		public DataErrorsChangedEventArgs(string propertyName)
		{
			this.propertyName = propertyName;
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06003247 RID: 12871 RVA: 0x000E157F File Offset: 0x000DF77F
		[global::__DynamicallyInvokable]
		public virtual string PropertyName
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x0400295F RID: 10591
		private readonly string propertyName;
	}
}
