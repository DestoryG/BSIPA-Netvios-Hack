using System;

namespace Mono.Cecil
{
	// Token: 0x020000AC RID: 172
	public enum SecurityAction : ushort
	{
		// Token: 0x04000235 RID: 565
		Request = 1,
		// Token: 0x04000236 RID: 566
		Demand,
		// Token: 0x04000237 RID: 567
		Assert,
		// Token: 0x04000238 RID: 568
		Deny,
		// Token: 0x04000239 RID: 569
		PermitOnly,
		// Token: 0x0400023A RID: 570
		LinkDemand,
		// Token: 0x0400023B RID: 571
		InheritDemand,
		// Token: 0x0400023C RID: 572
		RequestMinimum,
		// Token: 0x0400023D RID: 573
		RequestOptional,
		// Token: 0x0400023E RID: 574
		RequestRefuse,
		// Token: 0x0400023F RID: 575
		PreJitGrant,
		// Token: 0x04000240 RID: 576
		PreJitDeny,
		// Token: 0x04000241 RID: 577
		NonCasDemand,
		// Token: 0x04000242 RID: 578
		NonCasLinkDemand,
		// Token: 0x04000243 RID: 579
		NonCasInheritance
	}
}
