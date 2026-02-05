using System;

namespace System.Net
{
	// Token: 0x020001CE RID: 462
	[Flags]
	internal enum ContextFlags
	{
		// Token: 0x0400149C RID: 5276
		Zero = 0,
		// Token: 0x0400149D RID: 5277
		Delegate = 1,
		// Token: 0x0400149E RID: 5278
		MutualAuth = 2,
		// Token: 0x0400149F RID: 5279
		ReplayDetect = 4,
		// Token: 0x040014A0 RID: 5280
		SequenceDetect = 8,
		// Token: 0x040014A1 RID: 5281
		Confidentiality = 16,
		// Token: 0x040014A2 RID: 5282
		UseSessionKey = 32,
		// Token: 0x040014A3 RID: 5283
		AllocateMemory = 256,
		// Token: 0x040014A4 RID: 5284
		Connection = 2048,
		// Token: 0x040014A5 RID: 5285
		InitExtendedError = 16384,
		// Token: 0x040014A6 RID: 5286
		AcceptExtendedError = 32768,
		// Token: 0x040014A7 RID: 5287
		InitStream = 32768,
		// Token: 0x040014A8 RID: 5288
		AcceptStream = 65536,
		// Token: 0x040014A9 RID: 5289
		InitIntegrity = 65536,
		// Token: 0x040014AA RID: 5290
		AcceptIntegrity = 131072,
		// Token: 0x040014AB RID: 5291
		InitManualCredValidation = 524288,
		// Token: 0x040014AC RID: 5292
		InitUseSuppliedCreds = 128,
		// Token: 0x040014AD RID: 5293
		InitIdentify = 131072,
		// Token: 0x040014AE RID: 5294
		AcceptIdentify = 524288,
		// Token: 0x040014AF RID: 5295
		ProxyBindings = 67108864,
		// Token: 0x040014B0 RID: 5296
		AllowMissingBindings = 268435456,
		// Token: 0x040014B1 RID: 5297
		UnverifiedTargetName = 536870912
	}
}
