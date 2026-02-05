using System;

namespace Mono.Cecil
{
	// Token: 0x020000C5 RID: 197
	internal sealed class TypeReferenceProjection
	{
		// Token: 0x06000869 RID: 2153 RVA: 0x00019510 File Offset: 0x00017710
		public TypeReferenceProjection(TypeReference type, TypeReferenceTreatment treatment)
		{
			this.Name = type.Name;
			this.Namespace = type.Namespace;
			this.Scope = type.Scope;
			this.Treatment = treatment;
		}

		// Token: 0x04000302 RID: 770
		public readonly string Name;

		// Token: 0x04000303 RID: 771
		public readonly string Namespace;

		// Token: 0x04000304 RID: 772
		public readonly IMetadataScope Scope;

		// Token: 0x04000305 RID: 773
		public readonly TypeReferenceTreatment Treatment;
	}
}
