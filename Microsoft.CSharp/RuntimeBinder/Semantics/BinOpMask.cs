using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200002E RID: 46
	[Flags]
	internal enum BinOpMask
	{
		// Token: 0x0400026C RID: 620
		None = 0,
		// Token: 0x0400026D RID: 621
		Add = 1,
		// Token: 0x0400026E RID: 622
		Sub = 2,
		// Token: 0x0400026F RID: 623
		Mul = 4,
		// Token: 0x04000270 RID: 624
		Shift = 8,
		// Token: 0x04000271 RID: 625
		Equal = 16,
		// Token: 0x04000272 RID: 626
		Compare = 32,
		// Token: 0x04000273 RID: 627
		Bitwise = 64,
		// Token: 0x04000274 RID: 628
		BitXor = 128,
		// Token: 0x04000275 RID: 629
		Logical = 256,
		// Token: 0x04000276 RID: 630
		Integer = 247,
		// Token: 0x04000277 RID: 631
		Real = 55,
		// Token: 0x04000278 RID: 632
		BoolNorm = 144,
		// Token: 0x04000279 RID: 633
		Delegate = 19,
		// Token: 0x0400027A RID: 634
		Enum = 242,
		// Token: 0x0400027B RID: 635
		EnumUnder = 3,
		// Token: 0x0400027C RID: 636
		UnderEnum = 1,
		// Token: 0x0400027D RID: 637
		Ptr = 2,
		// Token: 0x0400027E RID: 638
		PtrNum = 3,
		// Token: 0x0400027F RID: 639
		NumPtr = 1,
		// Token: 0x04000280 RID: 640
		VoidPtr = 48
	}
}
