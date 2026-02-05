using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002CB RID: 715
	internal struct MibTcpRow
	{
		// Token: 0x040019F5 RID: 6645
		internal TcpState state;

		// Token: 0x040019F6 RID: 6646
		internal uint localAddr;

		// Token: 0x040019F7 RID: 6647
		internal byte localPort1;

		// Token: 0x040019F8 RID: 6648
		internal byte localPort2;

		// Token: 0x040019F9 RID: 6649
		internal byte ignoreLocalPort3;

		// Token: 0x040019FA RID: 6650
		internal byte ignoreLocalPort4;

		// Token: 0x040019FB RID: 6651
		internal uint remoteAddr;

		// Token: 0x040019FC RID: 6652
		internal byte remotePort1;

		// Token: 0x040019FD RID: 6653
		internal byte remotePort2;

		// Token: 0x040019FE RID: 6654
		internal byte ignoreRemotePort3;

		// Token: 0x040019FF RID: 6655
		internal byte ignoreRemotePort4;
	}
}
