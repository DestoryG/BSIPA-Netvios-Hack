using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200059A RID: 1434
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class PropertyChangingEventArgs : EventArgs
	{
		// Token: 0x06003527 RID: 13607 RVA: 0x000E7899 File Offset: 0x000E5A99
		[global::__DynamicallyInvokable]
		public PropertyChangingEventArgs(string propertyName)
		{
			this.propertyName = propertyName;
		}

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06003528 RID: 13608 RVA: 0x000E78A8 File Offset: 0x000E5AA8
		[global::__DynamicallyInvokable]
		public virtual string PropertyName
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x04002A34 RID: 10804
		private readonly string propertyName;
	}
}
