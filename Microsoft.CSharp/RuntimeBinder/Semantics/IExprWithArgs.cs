using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200008D RID: 141
	internal interface IExprWithArgs : IExprWithObject
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060004A1 RID: 1185
		// (set) Token: 0x060004A2 RID: 1186
		Expr OptionalArguments { get; set; }

		// Token: 0x060004A3 RID: 1187
		SymWithType GetSymWithType();
	}
}
