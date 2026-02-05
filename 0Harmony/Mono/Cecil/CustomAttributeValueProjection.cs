using System;

namespace Mono.Cecil
{
	// Token: 0x02000189 RID: 393
	internal sealed class CustomAttributeValueProjection
	{
		// Token: 0x06000C45 RID: 3141 RVA: 0x00028508 File Offset: 0x00026708
		public CustomAttributeValueProjection(AttributeTargets targets, CustomAttributeValueTreatment treatment)
		{
			this.Targets = targets;
			this.Treatment = treatment;
		}

		// Token: 0x04000563 RID: 1379
		public readonly AttributeTargets Targets;

		// Token: 0x04000564 RID: 1380
		public readonly CustomAttributeValueTreatment Treatment;
	}
}
