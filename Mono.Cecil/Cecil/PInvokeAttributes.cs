using System;

namespace Mono.Cecil
{
	// Token: 0x020000A3 RID: 163
	[Flags]
	public enum PInvokeAttributes : ushort
	{
		// Token: 0x0400020A RID: 522
		NoMangle = 1,
		// Token: 0x0400020B RID: 523
		CharSetMask = 6,
		// Token: 0x0400020C RID: 524
		CharSetNotSpec = 0,
		// Token: 0x0400020D RID: 525
		CharSetAnsi = 2,
		// Token: 0x0400020E RID: 526
		CharSetUnicode = 4,
		// Token: 0x0400020F RID: 527
		CharSetAuto = 6,
		// Token: 0x04000210 RID: 528
		SupportsLastError = 64,
		// Token: 0x04000211 RID: 529
		CallConvMask = 1792,
		// Token: 0x04000212 RID: 530
		CallConvWinapi = 256,
		// Token: 0x04000213 RID: 531
		CallConvCdecl = 512,
		// Token: 0x04000214 RID: 532
		CallConvStdCall = 768,
		// Token: 0x04000215 RID: 533
		CallConvThiscall = 1024,
		// Token: 0x04000216 RID: 534
		CallConvFastcall = 1280,
		// Token: 0x04000217 RID: 535
		BestFitMask = 48,
		// Token: 0x04000218 RID: 536
		BestFitEnabled = 16,
		// Token: 0x04000219 RID: 537
		BestFitDisabled = 32,
		// Token: 0x0400021A RID: 538
		ThrowOnUnmappableCharMask = 12288,
		// Token: 0x0400021B RID: 539
		ThrowOnUnmappableCharEnabled = 4096,
		// Token: 0x0400021C RID: 540
		ThrowOnUnmappableCharDisabled = 8192
	}
}
