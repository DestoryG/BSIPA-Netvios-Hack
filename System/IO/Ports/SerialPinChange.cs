using System;

namespace System.IO.Ports
{
	// Token: 0x0200040E RID: 1038
	public enum SerialPinChange
	{
		// Token: 0x040020FB RID: 8443
		CtsChanged = 8,
		// Token: 0x040020FC RID: 8444
		DsrChanged = 16,
		// Token: 0x040020FD RID: 8445
		CDChanged = 32,
		// Token: 0x040020FE RID: 8446
		Ring = 256,
		// Token: 0x040020FF RID: 8447
		Break = 64
	}
}
