using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000079 RID: 121
	internal sealed class ExprArrayIndex : ExprWithType
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x00017DEB File Offset: 0x00015FEB
		public ExprArrayIndex(CType type, Expr array, Expr index)
			: base(ExpressionKind.ArrayIndex, type)
		{
			this.Array = array;
			this.Index = index;
			base.Flags = EXPRFLAG.EXF_ASSGOP | EXPRFLAG.EXF_LVALUE;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00017E0E File Offset: 0x0001600E
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x00017E16 File Offset: 0x00016016
		public Expr Array { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x00017E1F File Offset: 0x0001601F
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x00017E27 File Offset: 0x00016027
		public Expr Index { get; set; }
	}
}
