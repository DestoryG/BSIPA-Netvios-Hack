using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000136 RID: 310
	[StructLayout(LayoutKind.Sequential)]
	internal class SecChannelBindings
	{
		// Token: 0x04001068 RID: 4200
		internal int dwInitiatorAddrType;

		// Token: 0x04001069 RID: 4201
		internal int cbInitiatorLength;

		// Token: 0x0400106A RID: 4202
		internal int dwInitiatorOffset;

		// Token: 0x0400106B RID: 4203
		internal int dwAcceptorAddrType;

		// Token: 0x0400106C RID: 4204
		internal int cbAcceptorLength;

		// Token: 0x0400106D RID: 4205
		internal int dwAcceptorOffset;

		// Token: 0x0400106E RID: 4206
		internal int cbApplicationDataLength;

		// Token: 0x0400106F RID: 4207
		internal int dwApplicationDataOffset;
	}
}
