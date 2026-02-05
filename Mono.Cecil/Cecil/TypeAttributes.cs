using System;

namespace Mono.Cecil
{
	// Token: 0x020000B8 RID: 184
	[Flags]
	public enum TypeAttributes : uint
	{
		// Token: 0x04000277 RID: 631
		VisibilityMask = 7U,
		// Token: 0x04000278 RID: 632
		NotPublic = 0U,
		// Token: 0x04000279 RID: 633
		Public = 1U,
		// Token: 0x0400027A RID: 634
		NestedPublic = 2U,
		// Token: 0x0400027B RID: 635
		NestedPrivate = 3U,
		// Token: 0x0400027C RID: 636
		NestedFamily = 4U,
		// Token: 0x0400027D RID: 637
		NestedAssembly = 5U,
		// Token: 0x0400027E RID: 638
		NestedFamANDAssem = 6U,
		// Token: 0x0400027F RID: 639
		NestedFamORAssem = 7U,
		// Token: 0x04000280 RID: 640
		LayoutMask = 24U,
		// Token: 0x04000281 RID: 641
		AutoLayout = 0U,
		// Token: 0x04000282 RID: 642
		SequentialLayout = 8U,
		// Token: 0x04000283 RID: 643
		ExplicitLayout = 16U,
		// Token: 0x04000284 RID: 644
		ClassSemanticMask = 32U,
		// Token: 0x04000285 RID: 645
		Class = 0U,
		// Token: 0x04000286 RID: 646
		Interface = 32U,
		// Token: 0x04000287 RID: 647
		Abstract = 128U,
		// Token: 0x04000288 RID: 648
		Sealed = 256U,
		// Token: 0x04000289 RID: 649
		SpecialName = 1024U,
		// Token: 0x0400028A RID: 650
		Import = 4096U,
		// Token: 0x0400028B RID: 651
		Serializable = 8192U,
		// Token: 0x0400028C RID: 652
		WindowsRuntime = 16384U,
		// Token: 0x0400028D RID: 653
		StringFormatMask = 196608U,
		// Token: 0x0400028E RID: 654
		AnsiClass = 0U,
		// Token: 0x0400028F RID: 655
		UnicodeClass = 65536U,
		// Token: 0x04000290 RID: 656
		AutoClass = 131072U,
		// Token: 0x04000291 RID: 657
		BeforeFieldInit = 1048576U,
		// Token: 0x04000292 RID: 658
		RTSpecialName = 2048U,
		// Token: 0x04000293 RID: 659
		HasSecurity = 262144U,
		// Token: 0x04000294 RID: 660
		Forwarder = 2097152U
	}
}
