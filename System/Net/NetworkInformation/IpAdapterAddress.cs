using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002BC RID: 700
	internal struct IpAdapterAddress
	{
		// Token: 0x060019B6 RID: 6582 RVA: 0x0007E074 File Offset: 0x0007C274
		internal static IPAddressCollection MarshalIpAddressCollection(IntPtr ptr)
		{
			IPAddressCollection ipaddressCollection = new IPAddressCollection();
			while (ptr != IntPtr.Zero)
			{
				IpAdapterAddress ipAdapterAddress = (IpAdapterAddress)Marshal.PtrToStructure(ptr, typeof(IpAdapterAddress));
				IPAddress ipaddress = ipAdapterAddress.address.MarshalIPAddress();
				ipaddressCollection.InternalAdd(ipaddress);
				ptr = ipAdapterAddress.next;
			}
			return ipaddressCollection;
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x0007E0CC File Offset: 0x0007C2CC
		internal static IPAddressInformationCollection MarshalIpAddressInformationCollection(IntPtr ptr)
		{
			IPAddressInformationCollection ipaddressInformationCollection = new IPAddressInformationCollection();
			while (ptr != IntPtr.Zero)
			{
				IpAdapterAddress ipAdapterAddress = (IpAdapterAddress)Marshal.PtrToStructure(ptr, typeof(IpAdapterAddress));
				IPAddress ipaddress = ipAdapterAddress.address.MarshalIPAddress();
				ipaddressInformationCollection.InternalAdd(new SystemIPAddressInformation(ipaddress, ipAdapterAddress.flags));
				ptr = ipAdapterAddress.next;
			}
			return ipaddressInformationCollection;
		}

		// Token: 0x04001944 RID: 6468
		internal uint length;

		// Token: 0x04001945 RID: 6469
		internal AdapterAddressFlags flags;

		// Token: 0x04001946 RID: 6470
		internal IntPtr next;

		// Token: 0x04001947 RID: 6471
		internal IpSocketAddress address;
	}
}
