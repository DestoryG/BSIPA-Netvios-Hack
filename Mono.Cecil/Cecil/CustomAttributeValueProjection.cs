using System;

namespace Mono.Cecil
{
	// Token: 0x020000C8 RID: 200
	internal sealed class CustomAttributeValueProjection
	{
		// Token: 0x0600086C RID: 2156 RVA: 0x00019591 File Offset: 0x00017791
		public CustomAttributeValueProjection(AttributeTargets targets, CustomAttributeValueTreatment treatment)
		{
			this.Targets = targets;
			this.Treatment = treatment;
		}

		// Token: 0x0400030C RID: 780
		public readonly AttributeTargets Targets;

		// Token: 0x0400030D RID: 781
		public readonly CustomAttributeValueTreatment Treatment;
	}
}
