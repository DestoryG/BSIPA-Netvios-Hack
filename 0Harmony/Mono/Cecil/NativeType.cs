using System;

namespace Mono.Cecil
{
	// Token: 0x02000157 RID: 343
	internal enum NativeType
	{
		// Token: 0x04000408 RID: 1032
		None = 102,
		// Token: 0x04000409 RID: 1033
		Boolean = 2,
		// Token: 0x0400040A RID: 1034
		I1,
		// Token: 0x0400040B RID: 1035
		U1,
		// Token: 0x0400040C RID: 1036
		I2,
		// Token: 0x0400040D RID: 1037
		U2,
		// Token: 0x0400040E RID: 1038
		I4,
		// Token: 0x0400040F RID: 1039
		U4,
		// Token: 0x04000410 RID: 1040
		I8,
		// Token: 0x04000411 RID: 1041
		U8,
		// Token: 0x04000412 RID: 1042
		R4,
		// Token: 0x04000413 RID: 1043
		R8,
		// Token: 0x04000414 RID: 1044
		LPStr = 20,
		// Token: 0x04000415 RID: 1045
		Int = 31,
		// Token: 0x04000416 RID: 1046
		UInt,
		// Token: 0x04000417 RID: 1047
		Func = 38,
		// Token: 0x04000418 RID: 1048
		Array = 42,
		// Token: 0x04000419 RID: 1049
		Currency = 15,
		// Token: 0x0400041A RID: 1050
		BStr = 19,
		// Token: 0x0400041B RID: 1051
		LPWStr = 21,
		// Token: 0x0400041C RID: 1052
		LPTStr,
		// Token: 0x0400041D RID: 1053
		FixedSysString,
		// Token: 0x0400041E RID: 1054
		IUnknown = 25,
		// Token: 0x0400041F RID: 1055
		IDispatch,
		// Token: 0x04000420 RID: 1056
		Struct,
		// Token: 0x04000421 RID: 1057
		IntF,
		// Token: 0x04000422 RID: 1058
		SafeArray,
		// Token: 0x04000423 RID: 1059
		FixedArray,
		// Token: 0x04000424 RID: 1060
		ByValStr = 34,
		// Token: 0x04000425 RID: 1061
		ANSIBStr,
		// Token: 0x04000426 RID: 1062
		TBStr,
		// Token: 0x04000427 RID: 1063
		VariantBool,
		// Token: 0x04000428 RID: 1064
		ASAny = 40,
		// Token: 0x04000429 RID: 1065
		LPStruct = 43,
		// Token: 0x0400042A RID: 1066
		CustomMarshaler,
		// Token: 0x0400042B RID: 1067
		Error,
		// Token: 0x0400042C RID: 1068
		Max = 80
	}
}
