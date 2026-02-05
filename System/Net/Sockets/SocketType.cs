using System;

namespace System.Net.Sockets
{
	// Token: 0x02000385 RID: 901
	public enum SocketType
	{
		// Token: 0x04001F30 RID: 7984
		Stream = 1,
		// Token: 0x04001F31 RID: 7985
		Dgram,
		// Token: 0x04001F32 RID: 7986
		Raw,
		// Token: 0x04001F33 RID: 7987
		Rdm,
		// Token: 0x04001F34 RID: 7988
		Seqpacket,
		// Token: 0x04001F35 RID: 7989
		Unknown = -1
	}
}
