using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002B2 RID: 690
	[global::__DynamicallyInvokable]
	public abstract class IPv4InterfaceProperties
	{
		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x060019A8 RID: 6568
		[global::__DynamicallyInvokable]
		public abstract bool UsesWins
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x060019A9 RID: 6569
		[global::__DynamicallyInvokable]
		public abstract bool IsDhcpEnabled
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x060019AA RID: 6570
		[global::__DynamicallyInvokable]
		public abstract bool IsAutomaticPrivateAddressingActive
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x060019AB RID: 6571
		[global::__DynamicallyInvokable]
		public abstract bool IsAutomaticPrivateAddressingEnabled
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x060019AC RID: 6572
		[global::__DynamicallyInvokable]
		public abstract int Index
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x060019AD RID: 6573
		[global::__DynamicallyInvokable]
		public abstract bool IsForwardingEnabled
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x060019AE RID: 6574
		[global::__DynamicallyInvokable]
		public abstract int Mtu
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x0007E00C File Offset: 0x0007C20C
		[global::__DynamicallyInvokable]
		protected IPv4InterfaceProperties()
		{
		}
	}
}
