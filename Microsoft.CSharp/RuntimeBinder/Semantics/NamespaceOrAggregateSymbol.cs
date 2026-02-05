using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000068 RID: 104
	internal abstract class NamespaceOrAggregateSymbol : ParentSymbol
	{
		// Token: 0x060003A6 RID: 934 RVA: 0x000169D8 File Offset: 0x00014BD8
		public void AddDecl(AggregateDeclaration decl)
		{
			if (this._declLast == null)
			{
				this._declLast = decl;
				this._declFirst = decl;
			}
			else
			{
				this._declLast.declNext = decl;
				this._declLast = decl;
			}
			decl.declNext = null;
			decl.bag = this;
		}

		// Token: 0x040004C7 RID: 1223
		private AggregateDeclaration _declFirst;

		// Token: 0x040004C8 RID: 1224
		private AggregateDeclaration _declLast;
	}
}
