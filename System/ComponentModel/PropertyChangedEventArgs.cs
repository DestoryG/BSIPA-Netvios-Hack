using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000598 RID: 1432
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class PropertyChangedEventArgs : EventArgs
	{
		// Token: 0x06003521 RID: 13601 RVA: 0x000E7882 File Offset: 0x000E5A82
		[global::__DynamicallyInvokable]
		public PropertyChangedEventArgs(string propertyName)
		{
			this.propertyName = propertyName;
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06003522 RID: 13602 RVA: 0x000E7891 File Offset: 0x000E5A91
		[global::__DynamicallyInvokable]
		public virtual string PropertyName
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x04002A33 RID: 10803
		private readonly string propertyName;
	}
}
