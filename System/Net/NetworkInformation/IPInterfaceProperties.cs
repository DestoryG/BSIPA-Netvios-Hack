using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002A4 RID: 676
	[global::__DynamicallyInvokable]
	public abstract class IPInterfaceProperties
	{
		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x0600193C RID: 6460
		[global::__DynamicallyInvokable]
		public abstract bool IsDnsEnabled
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x0600193D RID: 6461
		[global::__DynamicallyInvokable]
		public abstract string DnsSuffix
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x0600193E RID: 6462
		[global::__DynamicallyInvokable]
		public abstract bool IsDynamicDnsEnabled
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x0600193F RID: 6463
		[global::__DynamicallyInvokable]
		public abstract UnicastIPAddressInformationCollection UnicastAddresses
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001940 RID: 6464
		[global::__DynamicallyInvokable]
		public abstract MulticastIPAddressInformationCollection MulticastAddresses
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001941 RID: 6465
		[global::__DynamicallyInvokable]
		public abstract IPAddressInformationCollection AnycastAddresses
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001942 RID: 6466
		[global::__DynamicallyInvokable]
		public abstract IPAddressCollection DnsAddresses
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001943 RID: 6467
		[global::__DynamicallyInvokable]
		public abstract GatewayIPAddressInformationCollection GatewayAddresses
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001944 RID: 6468
		[global::__DynamicallyInvokable]
		public abstract IPAddressCollection DhcpServerAddresses
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001945 RID: 6469
		[global::__DynamicallyInvokable]
		public abstract IPAddressCollection WinsServersAddresses
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x06001946 RID: 6470
		[global::__DynamicallyInvokable]
		public abstract IPv4InterfaceProperties GetIPv4Properties();

		// Token: 0x06001947 RID: 6471
		[global::__DynamicallyInvokable]
		public abstract IPv6InterfaceProperties GetIPv6Properties();

		// Token: 0x06001948 RID: 6472 RVA: 0x0007DCDA File Offset: 0x0007BEDA
		[global::__DynamicallyInvokable]
		protected IPInterfaceProperties()
		{
		}
	}
}
