using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002CD RID: 717
	internal struct MibTcp6RowOwnerPid
	{
		// Token: 0x04001A01 RID: 6657
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		internal byte[] localAddr;

		// Token: 0x04001A02 RID: 6658
		internal uint localScopeId;

		// Token: 0x04001A03 RID: 6659
		internal byte localPort1;

		// Token: 0x04001A04 RID: 6660
		internal byte localPort2;

		// Token: 0x04001A05 RID: 6661
		internal byte ignoreLocalPort3;

		// Token: 0x04001A06 RID: 6662
		internal byte ignoreLocalPort4;

		// Token: 0x04001A07 RID: 6663
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		internal byte[] remoteAddr;

		// Token: 0x04001A08 RID: 6664
		internal uint remoteScopeId;

		// Token: 0x04001A09 RID: 6665
		internal byte remotePort1;

		// Token: 0x04001A0A RID: 6666
		internal byte remotePort2;

		// Token: 0x04001A0B RID: 6667
		internal byte ignoreRemotePort3;

		// Token: 0x04001A0C RID: 6668
		internal byte ignoreRemotePort4;

		// Token: 0x04001A0D RID: 6669
		internal TcpState state;

		// Token: 0x04001A0E RID: 6670
		internal uint owningPid;
	}
}
