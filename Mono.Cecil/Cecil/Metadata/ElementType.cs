using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000E3 RID: 227
	internal enum ElementType : byte
	{
		// Token: 0x040003A4 RID: 932
		None,
		// Token: 0x040003A5 RID: 933
		Void,
		// Token: 0x040003A6 RID: 934
		Boolean,
		// Token: 0x040003A7 RID: 935
		Char,
		// Token: 0x040003A8 RID: 936
		I1,
		// Token: 0x040003A9 RID: 937
		U1,
		// Token: 0x040003AA RID: 938
		I2,
		// Token: 0x040003AB RID: 939
		U2,
		// Token: 0x040003AC RID: 940
		I4,
		// Token: 0x040003AD RID: 941
		U4,
		// Token: 0x040003AE RID: 942
		I8,
		// Token: 0x040003AF RID: 943
		U8,
		// Token: 0x040003B0 RID: 944
		R4,
		// Token: 0x040003B1 RID: 945
		R8,
		// Token: 0x040003B2 RID: 946
		String,
		// Token: 0x040003B3 RID: 947
		Ptr,
		// Token: 0x040003B4 RID: 948
		ByRef,
		// Token: 0x040003B5 RID: 949
		ValueType,
		// Token: 0x040003B6 RID: 950
		Class,
		// Token: 0x040003B7 RID: 951
		Var,
		// Token: 0x040003B8 RID: 952
		Array,
		// Token: 0x040003B9 RID: 953
		GenericInst,
		// Token: 0x040003BA RID: 954
		TypedByRef,
		// Token: 0x040003BB RID: 955
		I = 24,
		// Token: 0x040003BC RID: 956
		U,
		// Token: 0x040003BD RID: 957
		FnPtr = 27,
		// Token: 0x040003BE RID: 958
		Object,
		// Token: 0x040003BF RID: 959
		SzArray,
		// Token: 0x040003C0 RID: 960
		MVar,
		// Token: 0x040003C1 RID: 961
		CModReqD,
		// Token: 0x040003C2 RID: 962
		CModOpt,
		// Token: 0x040003C3 RID: 963
		Internal,
		// Token: 0x040003C4 RID: 964
		Modifier = 64,
		// Token: 0x040003C5 RID: 965
		Sentinel,
		// Token: 0x040003C6 RID: 966
		Pinned = 69,
		// Token: 0x040003C7 RID: 967
		Type = 80,
		// Token: 0x040003C8 RID: 968
		Boxed,
		// Token: 0x040003C9 RID: 969
		Enum = 85
	}
}
