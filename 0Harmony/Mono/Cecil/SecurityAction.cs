using System;

namespace Mono.Cecil
{
	// Token: 0x02000167 RID: 359
	internal enum SecurityAction : ushort
	{
		// Token: 0x0400046F RID: 1135
		Request = 1,
		// Token: 0x04000470 RID: 1136
		Demand,
		// Token: 0x04000471 RID: 1137
		Assert,
		// Token: 0x04000472 RID: 1138
		Deny,
		// Token: 0x04000473 RID: 1139
		PermitOnly,
		// Token: 0x04000474 RID: 1140
		LinkDemand,
		// Token: 0x04000475 RID: 1141
		InheritDemand,
		// Token: 0x04000476 RID: 1142
		RequestMinimum,
		// Token: 0x04000477 RID: 1143
		RequestOptional,
		// Token: 0x04000478 RID: 1144
		RequestRefuse,
		// Token: 0x04000479 RID: 1145
		PreJitGrant,
		// Token: 0x0400047A RID: 1146
		PreJitDeny,
		// Token: 0x0400047B RID: 1147
		NonCasDemand,
		// Token: 0x0400047C RID: 1148
		NonCasLinkDemand,
		// Token: 0x0400047D RID: 1149
		NonCasInheritance
	}
}
