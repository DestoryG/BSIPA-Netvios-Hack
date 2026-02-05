using System;

namespace MonoMod.Utils
{
	// Token: 0x02000334 RID: 820
	[Flags]
	internal enum Platform
	{
		// Token: 0x04000F90 RID: 3984
		OS = 1,
		// Token: 0x04000F91 RID: 3985
		Bits64 = 2,
		// Token: 0x04000F92 RID: 3986
		NT = 4,
		// Token: 0x04000F93 RID: 3987
		Unix = 8,
		// Token: 0x04000F94 RID: 3988
		ARM = 65536,
		// Token: 0x04000F95 RID: 3989
		Unknown = 17,
		// Token: 0x04000F96 RID: 3990
		Windows = 37,
		// Token: 0x04000F97 RID: 3991
		MacOS = 73,
		// Token: 0x04000F98 RID: 3992
		Linux = 137,
		// Token: 0x04000F99 RID: 3993
		Android = 393,
		// Token: 0x04000F9A RID: 3994
		iOS = 585
	}
}
