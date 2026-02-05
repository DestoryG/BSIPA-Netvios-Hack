using System;

namespace Mono.Cecil
{
	// Token: 0x02000186 RID: 390
	internal sealed class TypeReferenceProjection
	{
		// Token: 0x06000C42 RID: 3138 RVA: 0x00028487 File Offset: 0x00026687
		public TypeReferenceProjection(TypeReference type, TypeReferenceTreatment treatment)
		{
			this.Name = type.Name;
			this.Namespace = type.Namespace;
			this.Scope = type.Scope;
			this.Treatment = treatment;
		}

		// Token: 0x04000559 RID: 1369
		public readonly string Name;

		// Token: 0x0400055A RID: 1370
		public readonly string Namespace;

		// Token: 0x0400055B RID: 1371
		public readonly IMetadataScope Scope;

		// Token: 0x0400055C RID: 1372
		public readonly TypeReferenceTreatment Treatment;
	}
}
