using System;

namespace System.Net
{
	// Token: 0x0200015D RID: 349
	[Flags]
	public enum SecurityProtocolType
	{
		// Token: 0x04001140 RID: 4416
		SystemDefault = 0,
		// Token: 0x04001141 RID: 4417
		Ssl3 = 48,
		// Token: 0x04001142 RID: 4418
		Tls = 192,
		// Token: 0x04001143 RID: 4419
		Tls11 = 768,
		// Token: 0x04001144 RID: 4420
		Tls12 = 3072,
		// Token: 0x04001145 RID: 4421
		Tls13 = 12288
	}
}
