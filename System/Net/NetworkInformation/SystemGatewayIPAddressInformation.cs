using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002AF RID: 687
	internal class SystemGatewayIPAddressInformation : GatewayIPAddressInformation
	{
		// Token: 0x06001997 RID: 6551 RVA: 0x0007DEF5 File Offset: 0x0007C0F5
		private SystemGatewayIPAddressInformation(IPAddress address)
		{
			this.address = address;
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001998 RID: 6552 RVA: 0x0007DF04 File Offset: 0x0007C104
		public override IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0007DF0C File Offset: 0x0007C10C
		internal static GatewayIPAddressInformationCollection ToGatewayIpAddressInformationCollection(IPAddressCollection addresses)
		{
			GatewayIPAddressInformationCollection gatewayIPAddressInformationCollection = new GatewayIPAddressInformationCollection();
			foreach (IPAddress ipaddress in addresses)
			{
				gatewayIPAddressInformationCollection.InternalAdd(new SystemGatewayIPAddressInformation(ipaddress));
			}
			return gatewayIPAddressInformationCollection;
		}

		// Token: 0x04001905 RID: 6405
		private IPAddress address;
	}
}
