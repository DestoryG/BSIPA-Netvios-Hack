using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002B3 RID: 691
	[global::__DynamicallyInvokable]
	public abstract class IPv6InterfaceProperties
	{
		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x060019B0 RID: 6576
		[global::__DynamicallyInvokable]
		public abstract int Index
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x060019B1 RID: 6577
		[global::__DynamicallyInvokable]
		public abstract int Mtu
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x0007E014 File Offset: 0x0007C214
		[global::__DynamicallyInvokable]
		public virtual long GetScopeId(ScopeLevel scopeLevel)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x0007E01B File Offset: 0x0007C21B
		[global::__DynamicallyInvokable]
		protected IPv6InterfaceProperties()
		{
		}
	}
}
