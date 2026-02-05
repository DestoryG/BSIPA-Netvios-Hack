using System;

namespace Mono.Cecil
{
	// Token: 0x020000C4 RID: 196
	internal sealed class TypeDefinitionProjection
	{
		// Token: 0x06000868 RID: 2152 RVA: 0x000194E9 File Offset: 0x000176E9
		public TypeDefinitionProjection(TypeDefinition type, TypeDefinitionTreatment treatment)
		{
			this.Attributes = type.Attributes;
			this.Name = type.Name;
			this.Treatment = treatment;
		}

		// Token: 0x040002FF RID: 767
		public readonly TypeAttributes Attributes;

		// Token: 0x04000300 RID: 768
		public readonly string Name;

		// Token: 0x04000301 RID: 769
		public readonly TypeDefinitionTreatment Treatment;
	}
}
