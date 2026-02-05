using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002F8 RID: 760
	internal class SystemIPInterfaceProperties : IPInterfaceProperties
	{
		// Token: 0x06001AC5 RID: 6853 RVA: 0x00080E70 File Offset: 0x0007F070
		internal SystemIPInterfaceProperties(FixedInfo fixedInfo, IpAdapterAddresses ipAdapterAddresses)
		{
			this.adapterFlags = ipAdapterAddresses.flags;
			this.dnsSuffix = ipAdapterAddresses.dnsSuffix;
			this.dnsEnabled = fixedInfo.EnableDns;
			this.dynamicDnsEnabled = (ipAdapterAddresses.flags & AdapterFlags.DnsEnabled) > (AdapterFlags)0;
			this.multicastAddresses = SystemMulticastIPAddressInformation.ToMulticastIpAddressInformationCollection(IpAdapterAddress.MarshalIpAddressInformationCollection(ipAdapterAddresses.firstMulticastAddress));
			this.dnsAddresses = IpAdapterAddress.MarshalIpAddressCollection(ipAdapterAddresses.firstDnsServerAddress);
			this.anycastAddresses = IpAdapterAddress.MarshalIpAddressInformationCollection(ipAdapterAddresses.firstAnycastAddress);
			this.unicastAddresses = SystemUnicastIPAddressInformation.MarshalUnicastIpAddressInformationCollection(ipAdapterAddresses.firstUnicastAddress);
			this.winsServersAddresses = IpAdapterAddress.MarshalIpAddressCollection(ipAdapterAddresses.firstWinsServerAddress);
			this.gatewayAddresses = SystemGatewayIPAddressInformation.ToGatewayIpAddressInformationCollection(IpAdapterAddress.MarshalIpAddressCollection(ipAdapterAddresses.firstGatewayAddress));
			this.dhcpServers = new IPAddressCollection();
			if (ipAdapterAddresses.dhcpv4Server.address != IntPtr.Zero)
			{
				this.dhcpServers.InternalAdd(ipAdapterAddresses.dhcpv4Server.MarshalIPAddress());
			}
			if (ipAdapterAddresses.dhcpv6Server.address != IntPtr.Zero)
			{
				this.dhcpServers.InternalAdd(ipAdapterAddresses.dhcpv6Server.MarshalIPAddress());
			}
			if ((this.adapterFlags & AdapterFlags.IPv4Enabled) != (AdapterFlags)0)
			{
				this.ipv4Properties = new SystemIPv4InterfaceProperties(fixedInfo, ipAdapterAddresses);
			}
			if ((this.adapterFlags & AdapterFlags.IPv6Enabled) != (AdapterFlags)0)
			{
				this.ipv6Properties = new SystemIPv6InterfaceProperties(ipAdapterAddresses.ipv6Index, ipAdapterAddresses.mtu, ipAdapterAddresses.zoneIndices);
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001AC6 RID: 6854 RVA: 0x00080FD6 File Offset: 0x0007F1D6
		public override bool IsDnsEnabled
		{
			get
			{
				return this.dnsEnabled;
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x00080FDE File Offset: 0x0007F1DE
		public override bool IsDynamicDnsEnabled
		{
			get
			{
				return this.dynamicDnsEnabled;
			}
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x00080FE6 File Offset: 0x0007F1E6
		public override IPv4InterfaceProperties GetIPv4Properties()
		{
			if ((this.adapterFlags & AdapterFlags.IPv4Enabled) == (AdapterFlags)0)
			{
				throw new NetworkInformationException(SocketError.ProtocolNotSupported);
			}
			return this.ipv4Properties;
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x00081007 File Offset: 0x0007F207
		public override IPv6InterfaceProperties GetIPv6Properties()
		{
			if ((this.adapterFlags & AdapterFlags.IPv6Enabled) == (AdapterFlags)0)
			{
				throw new NetworkInformationException(SocketError.ProtocolNotSupported);
			}
			return this.ipv6Properties;
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06001ACA RID: 6858 RVA: 0x00081028 File Offset: 0x0007F228
		public override string DnsSuffix
		{
			get
			{
				return this.dnsSuffix;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06001ACB RID: 6859 RVA: 0x00081030 File Offset: 0x0007F230
		public override IPAddressInformationCollection AnycastAddresses
		{
			get
			{
				return this.anycastAddresses;
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001ACC RID: 6860 RVA: 0x00081038 File Offset: 0x0007F238
		public override UnicastIPAddressInformationCollection UnicastAddresses
		{
			get
			{
				return this.unicastAddresses;
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001ACD RID: 6861 RVA: 0x00081040 File Offset: 0x0007F240
		public override MulticastIPAddressInformationCollection MulticastAddresses
		{
			get
			{
				return this.multicastAddresses;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001ACE RID: 6862 RVA: 0x00081048 File Offset: 0x0007F248
		public override IPAddressCollection DnsAddresses
		{
			get
			{
				return this.dnsAddresses;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001ACF RID: 6863 RVA: 0x00081050 File Offset: 0x0007F250
		public override GatewayIPAddressInformationCollection GatewayAddresses
		{
			get
			{
				return this.gatewayAddresses;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06001AD0 RID: 6864 RVA: 0x00081058 File Offset: 0x0007F258
		public override IPAddressCollection DhcpServerAddresses
		{
			get
			{
				return this.dhcpServers;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001AD1 RID: 6865 RVA: 0x00081060 File Offset: 0x0007F260
		public override IPAddressCollection WinsServersAddresses
		{
			get
			{
				return this.winsServersAddresses;
			}
		}

		// Token: 0x04001AAC RID: 6828
		private bool dnsEnabled;

		// Token: 0x04001AAD RID: 6829
		private bool dynamicDnsEnabled;

		// Token: 0x04001AAE RID: 6830
		private IPAddressCollection dnsAddresses;

		// Token: 0x04001AAF RID: 6831
		private UnicastIPAddressInformationCollection unicastAddresses;

		// Token: 0x04001AB0 RID: 6832
		private MulticastIPAddressInformationCollection multicastAddresses;

		// Token: 0x04001AB1 RID: 6833
		private IPAddressInformationCollection anycastAddresses;

		// Token: 0x04001AB2 RID: 6834
		private AdapterFlags adapterFlags;

		// Token: 0x04001AB3 RID: 6835
		private string dnsSuffix;

		// Token: 0x04001AB4 RID: 6836
		private SystemIPv4InterfaceProperties ipv4Properties;

		// Token: 0x04001AB5 RID: 6837
		private SystemIPv6InterfaceProperties ipv6Properties;

		// Token: 0x04001AB6 RID: 6838
		private IPAddressCollection winsServersAddresses;

		// Token: 0x04001AB7 RID: 6839
		private GatewayIPAddressInformationCollection gatewayAddresses;

		// Token: 0x04001AB8 RID: 6840
		private IPAddressCollection dhcpServers;
	}
}
