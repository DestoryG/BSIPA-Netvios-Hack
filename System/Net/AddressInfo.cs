using System;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x020001D7 RID: 471
	internal struct AddressInfo
	{
		// Token: 0x040014DF RID: 5343
		internal AddressInfoHints ai_flags;

		// Token: 0x040014E0 RID: 5344
		internal AddressFamily ai_family;

		// Token: 0x040014E1 RID: 5345
		internal SocketType ai_socktype;

		// Token: 0x040014E2 RID: 5346
		internal ProtocolFamily ai_protocol;

		// Token: 0x040014E3 RID: 5347
		internal int ai_addrlen;

		// Token: 0x040014E4 RID: 5348
		internal unsafe sbyte* ai_canonname;

		// Token: 0x040014E5 RID: 5349
		internal unsafe byte* ai_addr;

		// Token: 0x040014E6 RID: 5350
		internal unsafe AddressInfo* ai_next;
	}
}
