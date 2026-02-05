using System;

namespace System.Net
{
	// Token: 0x02000211 RID: 529
	[Flags]
	internal enum SchProtocols
	{
		// Token: 0x04001573 RID: 5491
		Zero = 0,
		// Token: 0x04001574 RID: 5492
		PctClient = 2,
		// Token: 0x04001575 RID: 5493
		PctServer = 1,
		// Token: 0x04001576 RID: 5494
		Pct = 3,
		// Token: 0x04001577 RID: 5495
		Ssl2Client = 8,
		// Token: 0x04001578 RID: 5496
		Ssl2Server = 4,
		// Token: 0x04001579 RID: 5497
		Ssl2 = 12,
		// Token: 0x0400157A RID: 5498
		Ssl3Client = 32,
		// Token: 0x0400157B RID: 5499
		Ssl3Server = 16,
		// Token: 0x0400157C RID: 5500
		Ssl3 = 48,
		// Token: 0x0400157D RID: 5501
		Tls10Client = 128,
		// Token: 0x0400157E RID: 5502
		Tls10Server = 64,
		// Token: 0x0400157F RID: 5503
		Tls10 = 192,
		// Token: 0x04001580 RID: 5504
		Tls11Client = 512,
		// Token: 0x04001581 RID: 5505
		Tls11Server = 256,
		// Token: 0x04001582 RID: 5506
		Tls11 = 768,
		// Token: 0x04001583 RID: 5507
		Tls12Client = 2048,
		// Token: 0x04001584 RID: 5508
		Tls12Server = 1024,
		// Token: 0x04001585 RID: 5509
		Tls12 = 3072,
		// Token: 0x04001586 RID: 5510
		Tls13Client = 8192,
		// Token: 0x04001587 RID: 5511
		Tls13Server = 4096,
		// Token: 0x04001588 RID: 5512
		Tls13 = 12288,
		// Token: 0x04001589 RID: 5513
		Ssl3Tls = 240,
		// Token: 0x0400158A RID: 5514
		UniClient = -2147483648,
		// Token: 0x0400158B RID: 5515
		UniServer = 1073741824,
		// Token: 0x0400158C RID: 5516
		Unified = -1073741824,
		// Token: 0x0400158D RID: 5517
		ClientMask = -2147472726,
		// Token: 0x0400158E RID: 5518
		ServerMask = 1073747285
	}
}
