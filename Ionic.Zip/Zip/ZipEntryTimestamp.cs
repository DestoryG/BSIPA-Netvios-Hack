using System;

namespace Ionic.Zip
{
	// Token: 0x02000037 RID: 55
	[Flags]
	public enum ZipEntryTimestamp
	{
		// Token: 0x0400013C RID: 316
		None = 0,
		// Token: 0x0400013D RID: 317
		DOS = 1,
		// Token: 0x0400013E RID: 318
		Windows = 2,
		// Token: 0x0400013F RID: 319
		Unix = 4,
		// Token: 0x04000140 RID: 320
		InfoZip1 = 8
	}
}
