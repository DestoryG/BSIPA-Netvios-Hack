using System;

namespace System.IO.Ports
{
	// Token: 0x0200040B RID: 1035
	public enum SerialError
	{
		// Token: 0x040020F4 RID: 8436
		TXFull = 256,
		// Token: 0x040020F5 RID: 8437
		RXOver = 1,
		// Token: 0x040020F6 RID: 8438
		Overrun,
		// Token: 0x040020F7 RID: 8439
		RXParity = 4,
		// Token: 0x040020F8 RID: 8440
		Frame = 8
	}
}
