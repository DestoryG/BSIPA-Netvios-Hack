using System;

namespace Mono.Cecil
{
	// Token: 0x020000BE RID: 190
	public enum MetadataType : byte
	{
		// Token: 0x040002AE RID: 686
		Void = 1,
		// Token: 0x040002AF RID: 687
		Boolean,
		// Token: 0x040002B0 RID: 688
		Char,
		// Token: 0x040002B1 RID: 689
		SByte,
		// Token: 0x040002B2 RID: 690
		Byte,
		// Token: 0x040002B3 RID: 691
		Int16,
		// Token: 0x040002B4 RID: 692
		UInt16,
		// Token: 0x040002B5 RID: 693
		Int32,
		// Token: 0x040002B6 RID: 694
		UInt32,
		// Token: 0x040002B7 RID: 695
		Int64,
		// Token: 0x040002B8 RID: 696
		UInt64,
		// Token: 0x040002B9 RID: 697
		Single,
		// Token: 0x040002BA RID: 698
		Double,
		// Token: 0x040002BB RID: 699
		String,
		// Token: 0x040002BC RID: 700
		Pointer,
		// Token: 0x040002BD RID: 701
		ByReference,
		// Token: 0x040002BE RID: 702
		ValueType,
		// Token: 0x040002BF RID: 703
		Class,
		// Token: 0x040002C0 RID: 704
		Var,
		// Token: 0x040002C1 RID: 705
		Array,
		// Token: 0x040002C2 RID: 706
		GenericInstance,
		// Token: 0x040002C3 RID: 707
		TypedByReference,
		// Token: 0x040002C4 RID: 708
		IntPtr = 24,
		// Token: 0x040002C5 RID: 709
		UIntPtr,
		// Token: 0x040002C6 RID: 710
		FunctionPointer = 27,
		// Token: 0x040002C7 RID: 711
		Object,
		// Token: 0x040002C8 RID: 712
		MVar = 30,
		// Token: 0x040002C9 RID: 713
		RequiredModifier,
		// Token: 0x040002CA RID: 714
		OptionalModifier,
		// Token: 0x040002CB RID: 715
		Sentinel = 65,
		// Token: 0x040002CC RID: 716
		Pinned = 69
	}
}
