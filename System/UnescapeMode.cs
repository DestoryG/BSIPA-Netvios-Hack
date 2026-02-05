using System;

namespace System
{
	// Token: 0x0200004D RID: 77
	[Flags]
	internal enum UnescapeMode
	{
		// Token: 0x040004A9 RID: 1193
		CopyOnly = 0,
		// Token: 0x040004AA RID: 1194
		Escape = 1,
		// Token: 0x040004AB RID: 1195
		Unescape = 2,
		// Token: 0x040004AC RID: 1196
		EscapeUnescape = 3,
		// Token: 0x040004AD RID: 1197
		V1ToStringFlag = 4,
		// Token: 0x040004AE RID: 1198
		UnescapeAll = 8,
		// Token: 0x040004AF RID: 1199
		UnescapeAllOrThrow = 24
	}
}
