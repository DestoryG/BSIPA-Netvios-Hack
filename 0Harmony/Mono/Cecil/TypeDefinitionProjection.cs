using System;

namespace Mono.Cecil
{
	// Token: 0x02000185 RID: 389
	internal sealed class TypeDefinitionProjection
	{
		// Token: 0x06000C41 RID: 3137 RVA: 0x00028460 File Offset: 0x00026660
		public TypeDefinitionProjection(TypeDefinition type, TypeDefinitionTreatment treatment)
		{
			this.Attributes = type.Attributes;
			this.Name = type.Name;
			this.Treatment = treatment;
		}

		// Token: 0x04000556 RID: 1366
		public readonly TypeAttributes Attributes;

		// Token: 0x04000557 RID: 1367
		public readonly string Name;

		// Token: 0x04000558 RID: 1368
		public readonly TypeDefinitionTreatment Treatment;
	}
}
