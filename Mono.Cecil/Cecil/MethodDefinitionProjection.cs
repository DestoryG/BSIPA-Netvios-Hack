using System;

namespace Mono.Cecil
{
	// Token: 0x020000C6 RID: 198
	internal sealed class MethodDefinitionProjection
	{
		// Token: 0x0600086A RID: 2154 RVA: 0x00019543 File Offset: 0x00017743
		public MethodDefinitionProjection(MethodDefinition method, MethodDefinitionTreatment treatment)
		{
			this.Attributes = method.Attributes;
			this.ImplAttributes = method.ImplAttributes;
			this.Name = method.Name;
			this.Treatment = treatment;
		}

		// Token: 0x04000306 RID: 774
		public readonly MethodAttributes Attributes;

		// Token: 0x04000307 RID: 775
		public readonly MethodImplAttributes ImplAttributes;

		// Token: 0x04000308 RID: 776
		public readonly string Name;

		// Token: 0x04000309 RID: 777
		public readonly MethodDefinitionTreatment Treatment;
	}
}
