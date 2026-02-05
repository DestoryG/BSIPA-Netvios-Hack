using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200002F RID: 47
	internal sealed class BindingContext
	{
		// Token: 0x06000223 RID: 547 RVA: 0x0001104A File Offset: 0x0000F24A
		public BindingContext(CSemanticChecker semanticChecker, ExprFactory exprFactory)
		{
			this.ExprFactory = exprFactory;
			this.SemanticChecker = semanticChecker;
			this.SymbolLoader = semanticChecker.SymbolLoader;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0001106C File Offset: 0x0000F26C
		public BindingContext(BindingContext parent)
		{
			this.ExprFactory = parent.ExprFactory;
			this.ContextForMemberLookup = parent.ContextForMemberLookup;
			this.CheckedNormal = parent.CheckedNormal;
			this.CheckedConstant = parent.CheckedConstant;
			this.SymbolLoader = (this.SemanticChecker = parent.SemanticChecker).SymbolLoader;
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000225 RID: 549 RVA: 0x000110C9 File Offset: 0x0000F2C9
		public SymbolLoader SymbolLoader { get; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000226 RID: 550 RVA: 0x000110D1 File Offset: 0x0000F2D1
		// (set) Token: 0x06000227 RID: 551 RVA: 0x000110D9 File Offset: 0x0000F2D9
		public AggregateDeclaration ContextForMemberLookup { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000228 RID: 552 RVA: 0x000110E2 File Offset: 0x0000F2E2
		public CSemanticChecker SemanticChecker { get; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000110EA File Offset: 0x0000F2EA
		public ExprFactory ExprFactory { get; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600022A RID: 554 RVA: 0x000110F2 File Offset: 0x0000F2F2
		// (set) Token: 0x0600022B RID: 555 RVA: 0x000110FA File Offset: 0x0000F2FA
		public bool CheckedNormal { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00011103 File Offset: 0x0000F303
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0001110B File Offset: 0x0000F30B
		public bool CheckedConstant { get; set; }
	}
}
