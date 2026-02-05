using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002FD RID: 765
	internal class SystemUnicastIPAddressInformation : UnicastIPAddressInformation
	{
		// Token: 0x06001B11 RID: 6929 RVA: 0x00081410 File Offset: 0x0007F610
		internal SystemUnicastIPAddressInformation(IpAdapterUnicastAddress adapterAddress)
		{
			IPAddress ipaddress = adapterAddress.address.MarshalIPAddress();
			this.innerInfo = new SystemIPAddressInformation(ipaddress, adapterAddress.flags);
			this.prefixOrigin = adapterAddress.prefixOrigin;
			this.suffixOrigin = adapterAddress.suffixOrigin;
			this.dadState = adapterAddress.dadState;
			this.validLifetime = adapterAddress.validLifetime;
			this.preferredLifetime = adapterAddress.preferredLifetime;
			this.dhcpLeaseLifetime = (long)((ulong)adapterAddress.leaseLifetime);
			this.prefixLength = adapterAddress.prefixLength;
			if (ipaddress.AddressFamily == AddressFamily.InterNetwork)
			{
				this.ipv4Mask = SystemUnicastIPAddressInformation.PrefixLengthToSubnetMask(this.prefixLength, ipaddress.AddressFamily);
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001B12 RID: 6930 RVA: 0x000814B7 File Offset: 0x0007F6B7
		public override IPAddress Address
		{
			get
			{
				return this.innerInfo.Address;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x000814C4 File Offset: 0x0007F6C4
		public override IPAddress IPv4Mask
		{
			get
			{
				if (this.Address.AddressFamily != AddressFamily.InterNetwork)
				{
					return IPAddress.Any;
				}
				return this.ipv4Mask;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001B14 RID: 6932 RVA: 0x000814E0 File Offset: 0x0007F6E0
		public override int PrefixLength
		{
			get
			{
				return (int)this.prefixLength;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001B15 RID: 6933 RVA: 0x000814E8 File Offset: 0x0007F6E8
		public override bool IsTransient
		{
			get
			{
				return this.innerInfo.IsTransient;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001B16 RID: 6934 RVA: 0x000814F5 File Offset: 0x0007F6F5
		public override bool IsDnsEligible
		{
			get
			{
				return this.innerInfo.IsDnsEligible;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x00081502 File Offset: 0x0007F702
		public override PrefixOrigin PrefixOrigin
		{
			get
			{
				return this.prefixOrigin;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001B18 RID: 6936 RVA: 0x0008150A File Offset: 0x0007F70A
		public override SuffixOrigin SuffixOrigin
		{
			get
			{
				return this.suffixOrigin;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x00081512 File Offset: 0x0007F712
		public override DuplicateAddressDetectionState DuplicateAddressDetectionState
		{
			get
			{
				return this.dadState;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06001B1A RID: 6938 RVA: 0x0008151A File Offset: 0x0007F71A
		public override long AddressValidLifetime
		{
			get
			{
				return (long)((ulong)this.validLifetime);
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001B1B RID: 6939 RVA: 0x00081523 File Offset: 0x0007F723
		public override long AddressPreferredLifetime
		{
			get
			{
				return (long)((ulong)this.preferredLifetime);
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001B1C RID: 6940 RVA: 0x0008152C File Offset: 0x0007F72C
		public override long DhcpLeaseLifetime
		{
			get
			{
				return this.dhcpLeaseLifetime;
			}
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x00081534 File Offset: 0x0007F734
		internal static UnicastIPAddressInformationCollection MarshalUnicastIpAddressInformationCollection(IntPtr ptr)
		{
			UnicastIPAddressInformationCollection unicastIPAddressInformationCollection = new UnicastIPAddressInformationCollection();
			while (ptr != IntPtr.Zero)
			{
				IpAdapterUnicastAddress ipAdapterUnicastAddress = (IpAdapterUnicastAddress)Marshal.PtrToStructure(ptr, typeof(IpAdapterUnicastAddress));
				unicastIPAddressInformationCollection.InternalAdd(new SystemUnicastIPAddressInformation(ipAdapterUnicastAddress));
				ptr = ipAdapterUnicastAddress.next;
			}
			return unicastIPAddressInformationCollection;
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x00081584 File Offset: 0x0007F784
		private static IPAddress PrefixLengthToSubnetMask(byte prefixLength, AddressFamily family)
		{
			byte[] array;
			if (family == AddressFamily.InterNetwork)
			{
				array = new byte[4];
			}
			else
			{
				array = new byte[16];
			}
			for (int i = 0; i < (int)prefixLength; i++)
			{
				byte[] array2 = array;
				int num = i / 8;
				array2[num] |= (byte)(128 >> i % 8);
			}
			return new IPAddress(array);
		}

		// Token: 0x04001ABD RID: 6845
		private long dhcpLeaseLifetime;

		// Token: 0x04001ABE RID: 6846
		private SystemIPAddressInformation innerInfo;

		// Token: 0x04001ABF RID: 6847
		private IPAddress ipv4Mask;

		// Token: 0x04001AC0 RID: 6848
		private PrefixOrigin prefixOrigin;

		// Token: 0x04001AC1 RID: 6849
		private SuffixOrigin suffixOrigin;

		// Token: 0x04001AC2 RID: 6850
		private DuplicateAddressDetectionState dadState;

		// Token: 0x04001AC3 RID: 6851
		private uint validLifetime;

		// Token: 0x04001AC4 RID: 6852
		private uint preferredLifetime;

		// Token: 0x04001AC5 RID: 6853
		private byte prefixLength;
	}
}
