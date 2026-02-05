using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200007A RID: 122
	internal sealed class ExprArrayInit : ExprWithType
	{
		// Token: 0x0600042B RID: 1067 RVA: 0x00017E30 File Offset: 0x00016030
		public ExprArrayInit(CType type, Expr arguments, Expr argumentDimensions, int[] dimensionSizes, int dimensionSize)
			: base(ExpressionKind.ArrayInit, type)
		{
			this.OptionalArguments = arguments;
			this.OptionalArgumentDimensions = argumentDimensions;
			this.DimensionSizes = dimensionSizes;
			this.DimensionSize = dimensionSize;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00017E59 File Offset: 0x00016059
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x00017E61 File Offset: 0x00016061
		public Expr OptionalArguments { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x00017E6A File Offset: 0x0001606A
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x00017E72 File Offset: 0x00016072
		public Expr OptionalArgumentDimensions { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00017E7B File Offset: 0x0001607B
		public int[] DimensionSizes { get; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x00017E83 File Offset: 0x00016083
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x00017E8B File Offset: 0x0001608B
		public int DimensionSize { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x00017E94 File Offset: 0x00016094
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x00017E9C File Offset: 0x0001609C
		public bool GeneratedForParamArray { get; set; }
	}
}
