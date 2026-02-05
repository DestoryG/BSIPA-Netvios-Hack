using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200065A RID: 1626
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTryCatchFinallyStatement : CodeStatement
	{
		// Token: 0x06003ADD RID: 15069 RVA: 0x000F46B8 File Offset: 0x000F28B8
		public CodeTryCatchFinallyStatement()
		{
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x000F46E4 File Offset: 0x000F28E4
		public CodeTryCatchFinallyStatement(CodeStatement[] tryStatements, CodeCatchClause[] catchClauses)
		{
			this.TryStatements.AddRange(tryStatements);
			this.CatchClauses.AddRange(catchClauses);
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x000F4730 File Offset: 0x000F2930
		public CodeTryCatchFinallyStatement(CodeStatement[] tryStatements, CodeCatchClause[] catchClauses, CodeStatement[] finallyStatements)
		{
			this.TryStatements.AddRange(tryStatements);
			this.CatchClauses.AddRange(catchClauses);
			this.FinallyStatements.AddRange(finallyStatements);
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x000F4788 File Offset: 0x000F2988
		public CodeStatementCollection TryStatements
		{
			get
			{
				return this.tryStatments;
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06003AE1 RID: 15073 RVA: 0x000F4790 File Offset: 0x000F2990
		public CodeCatchClauseCollection CatchClauses
		{
			get
			{
				return this.catchClauses;
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06003AE2 RID: 15074 RVA: 0x000F4798 File Offset: 0x000F2998
		public CodeStatementCollection FinallyStatements
		{
			get
			{
				return this.finallyStatments;
			}
		}

		// Token: 0x04002C19 RID: 11289
		private CodeStatementCollection tryStatments = new CodeStatementCollection();

		// Token: 0x04002C1A RID: 11290
		private CodeStatementCollection finallyStatments = new CodeStatementCollection();

		// Token: 0x04002C1B RID: 11291
		private CodeCatchClauseCollection catchClauses = new CodeCatchClauseCollection();
	}
}
