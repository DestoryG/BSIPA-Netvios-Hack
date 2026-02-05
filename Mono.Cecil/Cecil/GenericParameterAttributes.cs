using System;

namespace Mono.Cecil
{
	// Token: 0x02000063 RID: 99
	[Flags]
	public enum GenericParameterAttributes : ushort
	{
		// Token: 0x040000D3 RID: 211
		VarianceMask = 3,
		// Token: 0x040000D4 RID: 212
		NonVariant = 0,
		// Token: 0x040000D5 RID: 213
		Covariant = 1,
		// Token: 0x040000D6 RID: 214
		Contravariant = 2,
		// Token: 0x040000D7 RID: 215
		SpecialConstraintMask = 28,
		// Token: 0x040000D8 RID: 216
		ReferenceTypeConstraint = 4,
		// Token: 0x040000D9 RID: 217
		NotNullableValueTypeConstraint = 8,
		// Token: 0x040000DA RID: 218
		DefaultConstructorConstraint = 16
	}
}
