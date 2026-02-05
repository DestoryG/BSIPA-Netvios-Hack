using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000075 RID: 117
	[Flags]
	internal enum symbmask_t : long
	{
		// Token: 0x04000506 RID: 1286
		MASK_NamespaceSymbol = 1L,
		// Token: 0x04000507 RID: 1287
		MASK_AssemblyQualifiedNamespaceSymbol = 2L,
		// Token: 0x04000508 RID: 1288
		MASK_AggregateSymbol = 4L,
		// Token: 0x04000509 RID: 1289
		MASK_TypeParameterSymbol = 16L,
		// Token: 0x0400050A RID: 1290
		MASK_FieldSymbol = 32L,
		// Token: 0x0400050B RID: 1291
		MASK_MethodSymbol = 128L,
		// Token: 0x0400050C RID: 1292
		MASK_PropertySymbol = 256L,
		// Token: 0x0400050D RID: 1293
		MASK_EventSymbol = 512L,
		// Token: 0x0400050E RID: 1294
		MASK_ALL = -1L
	}
}
