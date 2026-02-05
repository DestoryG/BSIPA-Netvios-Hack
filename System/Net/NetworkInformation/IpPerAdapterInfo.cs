using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002C1 RID: 705
	internal struct IpPerAdapterInfo
	{
		// Token: 0x04001985 RID: 6533
		internal bool autoconfigEnabled;

		// Token: 0x04001986 RID: 6534
		internal bool autoconfigActive;

		// Token: 0x04001987 RID: 6535
		internal IntPtr currentDnsServer;

		// Token: 0x04001988 RID: 6536
		internal IpAddrString dnsServerList;
	}
}
