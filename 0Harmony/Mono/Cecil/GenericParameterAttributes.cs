using System;

namespace Mono.Cecil
{
	// Token: 0x02000119 RID: 281
	[Flags]
	internal enum GenericParameterAttributes : ushort
	{
		// Token: 0x040002EA RID: 746
		VarianceMask = 3,
		// Token: 0x040002EB RID: 747
		NonVariant = 0,
		// Token: 0x040002EC RID: 748
		Covariant = 1,
		// Token: 0x040002ED RID: 749
		Contravariant = 2,
		// Token: 0x040002EE RID: 750
		SpecialConstraintMask = 28,
		// Token: 0x040002EF RID: 751
		ReferenceTypeConstraint = 4,
		// Token: 0x040002F0 RID: 752
		NotNullableValueTypeConstraint = 8,
		// Token: 0x040002F1 RID: 753
		DefaultConstructorConstraint = 16
	}
}
