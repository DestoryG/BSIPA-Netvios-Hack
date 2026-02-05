using System;

namespace Mono.Cecil
{
	// Token: 0x02000160 RID: 352
	[Flags]
	internal enum PropertyAttributes : ushort
	{
		// Token: 0x04000459 RID: 1113
		None = 0,
		// Token: 0x0400045A RID: 1114
		SpecialName = 512,
		// Token: 0x0400045B RID: 1115
		RTSpecialName = 1024,
		// Token: 0x0400045C RID: 1116
		HasDefault = 4096,
		// Token: 0x0400045D RID: 1117
		Unused = 59903
	}
}
