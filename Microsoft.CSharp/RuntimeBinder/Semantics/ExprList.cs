using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200008F RID: 143
	internal sealed class ExprList : Expr
	{
		// Token: 0x060004A6 RID: 1190 RVA: 0x0001853B File Offset: 0x0001673B
		public ExprList(Expr optionalElement, Expr optionalNextListNode)
			: base(ExpressionKind.List)
		{
			this.OptionalElement = optionalElement;
			this.OptionalNextListNode = optionalNextListNode;
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00018552 File Offset: 0x00016752
		// (set) Token: 0x060004A8 RID: 1192 RVA: 0x0001855A File Offset: 0x0001675A
		public Expr OptionalElement { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00018563 File Offset: 0x00016763
		// (set) Token: 0x060004AA RID: 1194 RVA: 0x0001856B File Offset: 0x0001676B
		public Expr OptionalNextListNode { get; set; }
	}
}
