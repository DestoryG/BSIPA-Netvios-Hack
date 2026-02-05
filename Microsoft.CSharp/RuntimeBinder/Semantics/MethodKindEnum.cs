using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000051 RID: 81
	internal enum MethodKindEnum
	{
		// Token: 0x040003D7 RID: 983
		None,
		// Token: 0x040003D8 RID: 984
		Constructor,
		// Token: 0x040003D9 RID: 985
		Destructor,
		// Token: 0x040003DA RID: 986
		PropAccessor,
		// Token: 0x040003DB RID: 987
		EventAccessor,
		// Token: 0x040003DC RID: 988
		ExplicitConv,
		// Token: 0x040003DD RID: 989
		ImplicitConv,
		// Token: 0x040003DE RID: 990
		Anonymous,
		// Token: 0x040003DF RID: 991
		Invoke,
		// Token: 0x040003E0 RID: 992
		BeginInvoke,
		// Token: 0x040003E1 RID: 993
		EndInvoke,
		// Token: 0x040003E2 RID: 994
		AnonymousTypeToString,
		// Token: 0x040003E3 RID: 995
		AnonymousTypeEquals,
		// Token: 0x040003E4 RID: 996
		AnonymousTypeGetHashCode,
		// Token: 0x040003E5 RID: 997
		IteratorDispose,
		// Token: 0x040003E6 RID: 998
		IteratorReset,
		// Token: 0x040003E7 RID: 999
		IteratorGetEnumerator,
		// Token: 0x040003E8 RID: 1000
		IteratorGetEnumeratorDelegating,
		// Token: 0x040003E9 RID: 1001
		IteratorMoveNext,
		// Token: 0x040003EA RID: 1002
		Latent,
		// Token: 0x040003EB RID: 1003
		Actual,
		// Token: 0x040003EC RID: 1004
		IteratorFinally
	}
}
