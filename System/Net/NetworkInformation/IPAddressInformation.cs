using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200029F RID: 671
	[global::__DynamicallyInvokable]
	public abstract class IPAddressInformation
	{
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x060018FE RID: 6398
		[global::__DynamicallyInvokable]
		public abstract IPAddress Address
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060018FF RID: 6399
		[global::__DynamicallyInvokable]
		public abstract bool IsDnsEligible
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001900 RID: 6400
		[global::__DynamicallyInvokable]
		public abstract bool IsTransient
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x0007DBC9 File Offset: 0x0007BDC9
		[global::__DynamicallyInvokable]
		protected IPAddressInformation()
		{
		}
	}
}
