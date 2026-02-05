using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002B0 RID: 688
	[global::__DynamicallyInvokable]
	public abstract class GatewayIPAddressInformation
	{
		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x0600199A RID: 6554
		[global::__DynamicallyInvokable]
		public abstract IPAddress Address
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0007DF60 File Offset: 0x0007C160
		[global::__DynamicallyInvokable]
		protected GatewayIPAddressInformation()
		{
		}
	}
}
