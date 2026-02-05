using System;

namespace Mono.Cecil
{
	// Token: 0x02000188 RID: 392
	internal sealed class FieldDefinitionProjection
	{
		// Token: 0x06000C44 RID: 3140 RVA: 0x000284ED File Offset: 0x000266ED
		public FieldDefinitionProjection(FieldDefinition field, FieldDefinitionTreatment treatment)
		{
			this.Attributes = field.Attributes;
			this.Treatment = treatment;
		}

		// Token: 0x04000561 RID: 1377
		public readonly FieldAttributes Attributes;

		// Token: 0x04000562 RID: 1378
		public readonly FieldDefinitionTreatment Treatment;
	}
}
