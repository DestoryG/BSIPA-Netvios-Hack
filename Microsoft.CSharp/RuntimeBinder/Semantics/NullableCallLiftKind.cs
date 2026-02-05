using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000054 RID: 84
	internal enum NullableCallLiftKind
	{
		// Token: 0x040003FD RID: 1021
		NotLifted,
		// Token: 0x040003FE RID: 1022
		Operator,
		// Token: 0x040003FF RID: 1023
		EqualityOperator,
		// Token: 0x04000400 RID: 1024
		InequalityOperator,
		// Token: 0x04000401 RID: 1025
		UserDefinedConversion,
		// Token: 0x04000402 RID: 1026
		NullableConversion,
		// Token: 0x04000403 RID: 1027
		NullableConversionConstructor,
		// Token: 0x04000404 RID: 1028
		NullableIntermediateConversion,
		// Token: 0x04000405 RID: 1029
		NotLiftedIntermediateConversion
	}
}
