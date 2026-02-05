using System;

namespace Mono.Cecil
{
	// Token: 0x0200010C RID: 268
	[Flags]
	internal enum FieldAttributes : ushort
	{
		// Token: 0x040002B7 RID: 695
		FieldAccessMask = 7,
		// Token: 0x040002B8 RID: 696
		CompilerControlled = 0,
		// Token: 0x040002B9 RID: 697
		Private = 1,
		// Token: 0x040002BA RID: 698
		FamANDAssem = 2,
		// Token: 0x040002BB RID: 699
		Assembly = 3,
		// Token: 0x040002BC RID: 700
		Family = 4,
		// Token: 0x040002BD RID: 701
		FamORAssem = 5,
		// Token: 0x040002BE RID: 702
		Public = 6,
		// Token: 0x040002BF RID: 703
		Static = 16,
		// Token: 0x040002C0 RID: 704
		InitOnly = 32,
		// Token: 0x040002C1 RID: 705
		Literal = 64,
		// Token: 0x040002C2 RID: 706
		NotSerialized = 128,
		// Token: 0x040002C3 RID: 707
		SpecialName = 512,
		// Token: 0x040002C4 RID: 708
		PInvokeImpl = 8192,
		// Token: 0x040002C5 RID: 709
		RTSpecialName = 1024,
		// Token: 0x040002C6 RID: 710
		HasFieldMarshal = 4096,
		// Token: 0x040002C7 RID: 711
		HasDefault = 32768,
		// Token: 0x040002C8 RID: 712
		HasFieldRVA = 256
	}
}
