using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002D3 RID: 723
	internal struct MibUdp6RowOwnerPid
	{
		// Token: 0x04001A24 RID: 6692
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		internal byte[] localAddr;

		// Token: 0x04001A25 RID: 6693
		internal uint localScopeId;

		// Token: 0x04001A26 RID: 6694
		internal byte localPort1;

		// Token: 0x04001A27 RID: 6695
		internal byte localPort2;

		// Token: 0x04001A28 RID: 6696
		internal byte ignoreLocalPort3;

		// Token: 0x04001A29 RID: 6697
		internal byte ignoreLocalPort4;

		// Token: 0x04001A2A RID: 6698
		internal uint owningPid;
	}
}
