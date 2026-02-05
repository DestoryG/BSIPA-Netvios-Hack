using System;

namespace Mono.Cecil
{
	// Token: 0x02000187 RID: 391
	internal sealed class MethodDefinitionProjection
	{
		// Token: 0x06000C43 RID: 3139 RVA: 0x000284BA File Offset: 0x000266BA
		public MethodDefinitionProjection(MethodDefinition method, MethodDefinitionTreatment treatment)
		{
			this.Attributes = method.Attributes;
			this.ImplAttributes = method.ImplAttributes;
			this.Name = method.Name;
			this.Treatment = treatment;
		}

		// Token: 0x0400055D RID: 1373
		public readonly MethodAttributes Attributes;

		// Token: 0x0400055E RID: 1374
		public readonly MethodImplAttributes ImplAttributes;

		// Token: 0x0400055F RID: 1375
		public readonly string Name;

		// Token: 0x04000560 RID: 1376
		public readonly MethodDefinitionTreatment Treatment;
	}
}
