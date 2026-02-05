using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000092 RID: 146
	internal sealed class ExprMethodInfo : ExprWithType
	{
		// Token: 0x060004B8 RID: 1208 RVA: 0x0001865C File Offset: 0x0001685C
		public ExprMethodInfo(CType type, MethodSymbol method, AggregateType methodType, TypeArray methodParameters)
			: base(ExpressionKind.MethodInfo, type)
		{
			this.Method = new MethWithInst(method, methodType, methodParameters);
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00018676 File Offset: 0x00016876
		public MethWithInst Method { get; }
	}
}
