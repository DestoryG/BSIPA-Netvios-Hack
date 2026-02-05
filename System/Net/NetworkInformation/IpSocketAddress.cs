using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002BB RID: 699
	internal struct IpSocketAddress
	{
		// Token: 0x060019B5 RID: 6581 RVA: 0x0007E02C File Offset: 0x0007C22C
		internal IPAddress MarshalIPAddress()
		{
			AddressFamily addressFamily = ((this.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork);
			SocketAddress socketAddress = new SocketAddress(addressFamily, this.addressLength);
			Marshal.Copy(this.address, socketAddress.m_Buffer, 0, this.addressLength);
			return socketAddress.GetIPAddress();
		}

		// Token: 0x04001942 RID: 6466
		internal IntPtr address;

		// Token: 0x04001943 RID: 6467
		internal int addressLength;
	}
}
