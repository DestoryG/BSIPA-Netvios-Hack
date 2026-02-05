using System;

namespace Mono.Cecil
{
	// Token: 0x0200008D RID: 141
	[Flags]
	public enum MethodSemanticsAttributes : ushort
	{
		// Token: 0x04000166 RID: 358
		None = 0,
		// Token: 0x04000167 RID: 359
		Setter = 1,
		// Token: 0x04000168 RID: 360
		Getter = 2,
		// Token: 0x04000169 RID: 361
		Other = 4,
		// Token: 0x0400016A RID: 362
		AddOn = 8,
		// Token: 0x0400016B RID: 363
		RemoveOn = 16,
		// Token: 0x0400016C RID: 364
		Fire = 32
	}
}
