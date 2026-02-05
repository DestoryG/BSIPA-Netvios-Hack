using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002BA RID: 698
	internal struct FIXED_INFO
	{
		// Token: 0x04001936 RID: 6454
		internal const int MAX_HOSTNAME_LEN = 128;

		// Token: 0x04001937 RID: 6455
		internal const int MAX_DOMAIN_NAME_LEN = 128;

		// Token: 0x04001938 RID: 6456
		internal const int MAX_SCOPE_ID_LEN = 256;

		// Token: 0x04001939 RID: 6457
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		internal string hostName;

		// Token: 0x0400193A RID: 6458
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		internal string domainName;

		// Token: 0x0400193B RID: 6459
		internal uint currentDnsServer;

		// Token: 0x0400193C RID: 6460
		internal IpAddrString DnsServerList;

		// Token: 0x0400193D RID: 6461
		internal NetBiosNodeType nodeType;

		// Token: 0x0400193E RID: 6462
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		internal string scopeId;

		// Token: 0x0400193F RID: 6463
		internal bool enableRouting;

		// Token: 0x04001940 RID: 6464
		internal bool enableProxy;

		// Token: 0x04001941 RID: 6465
		internal bool enableDns;
	}
}
