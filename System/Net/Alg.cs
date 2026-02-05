using System;

namespace System.Net
{
	// Token: 0x02000212 RID: 530
	[Flags]
	internal enum Alg
	{
		// Token: 0x04001590 RID: 5520
		Any = 0,
		// Token: 0x04001591 RID: 5521
		ClassSignture = 8192,
		// Token: 0x04001592 RID: 5522
		ClassEncrypt = 24576,
		// Token: 0x04001593 RID: 5523
		ClassHash = 32768,
		// Token: 0x04001594 RID: 5524
		ClassKeyXch = 40960,
		// Token: 0x04001595 RID: 5525
		TypeRSA = 1024,
		// Token: 0x04001596 RID: 5526
		TypeBlock = 1536,
		// Token: 0x04001597 RID: 5527
		TypeStream = 2048,
		// Token: 0x04001598 RID: 5528
		TypeDH = 2560,
		// Token: 0x04001599 RID: 5529
		NameDES = 1,
		// Token: 0x0400159A RID: 5530
		NameRC2 = 2,
		// Token: 0x0400159B RID: 5531
		Name3DES = 3,
		// Token: 0x0400159C RID: 5532
		NameAES_128 = 14,
		// Token: 0x0400159D RID: 5533
		NameAES_192 = 15,
		// Token: 0x0400159E RID: 5534
		NameAES_256 = 16,
		// Token: 0x0400159F RID: 5535
		NameAES = 17,
		// Token: 0x040015A0 RID: 5536
		NameRC4 = 1,
		// Token: 0x040015A1 RID: 5537
		NameMD5 = 3,
		// Token: 0x040015A2 RID: 5538
		NameSHA = 4,
		// Token: 0x040015A3 RID: 5539
		NameSHA256 = 12,
		// Token: 0x040015A4 RID: 5540
		NameSHA384 = 13,
		// Token: 0x040015A5 RID: 5541
		NameSHA512 = 14,
		// Token: 0x040015A6 RID: 5542
		NameDH_Ephem = 2
	}
}
