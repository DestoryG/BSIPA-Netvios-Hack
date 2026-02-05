using System;

namespace Mono.Cecil
{
	// Token: 0x02000145 RID: 325
	[Flags]
	internal enum MethodSemanticsAttributes : ushort
	{
		// Token: 0x04000386 RID: 902
		None = 0,
		// Token: 0x04000387 RID: 903
		Setter = 1,
		// Token: 0x04000388 RID: 904
		Getter = 2,
		// Token: 0x04000389 RID: 905
		Other = 4,
		// Token: 0x0400038A RID: 906
		AddOn = 8,
		// Token: 0x0400038B RID: 907
		RemoveOn = 16,
		// Token: 0x0400038C RID: 908
		Fire = 32
	}
}
