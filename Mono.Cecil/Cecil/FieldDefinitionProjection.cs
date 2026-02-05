using System;

namespace Mono.Cecil
{
	// Token: 0x020000C7 RID: 199
	internal sealed class FieldDefinitionProjection
	{
		// Token: 0x0600086B RID: 2155 RVA: 0x00019576 File Offset: 0x00017776
		public FieldDefinitionProjection(FieldDefinition field, FieldDefinitionTreatment treatment)
		{
			this.Attributes = field.Attributes;
			this.Treatment = treatment;
		}

		// Token: 0x0400030A RID: 778
		public readonly FieldAttributes Attributes;

		// Token: 0x0400030B RID: 779
		public readonly FieldDefinitionTreatment Treatment;
	}
}
