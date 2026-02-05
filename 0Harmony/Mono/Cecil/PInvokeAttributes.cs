using System;

namespace Mono.Cecil
{
	// Token: 0x0200015D RID: 349
	[Flags]
	internal enum PInvokeAttributes : ushort
	{
		// Token: 0x04000442 RID: 1090
		NoMangle = 1,
		// Token: 0x04000443 RID: 1091
		CharSetMask = 6,
		// Token: 0x04000444 RID: 1092
		CharSetNotSpec = 0,
		// Token: 0x04000445 RID: 1093
		CharSetAnsi = 2,
		// Token: 0x04000446 RID: 1094
		CharSetUnicode = 4,
		// Token: 0x04000447 RID: 1095
		CharSetAuto = 6,
		// Token: 0x04000448 RID: 1096
		SupportsLastError = 64,
		// Token: 0x04000449 RID: 1097
		CallConvMask = 1792,
		// Token: 0x0400044A RID: 1098
		CallConvWinapi = 256,
		// Token: 0x0400044B RID: 1099
		CallConvCdecl = 512,
		// Token: 0x0400044C RID: 1100
		CallConvStdCall = 768,
		// Token: 0x0400044D RID: 1101
		CallConvThiscall = 1024,
		// Token: 0x0400044E RID: 1102
		CallConvFastcall = 1280,
		// Token: 0x0400044F RID: 1103
		BestFitMask = 48,
		// Token: 0x04000450 RID: 1104
		BestFitEnabled = 16,
		// Token: 0x04000451 RID: 1105
		BestFitDisabled = 32,
		// Token: 0x04000452 RID: 1106
		ThrowOnUnmappableCharMask = 12288,
		// Token: 0x04000453 RID: 1107
		ThrowOnUnmappableCharEnabled = 4096,
		// Token: 0x04000454 RID: 1108
		ThrowOnUnmappableCharDisabled = 8192
	}
}
