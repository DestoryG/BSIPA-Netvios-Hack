using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200062A RID: 1578
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeConditionStatement : CodeStatement
	{
		// Token: 0x06003994 RID: 14740 RVA: 0x000F2CFB File Offset: 0x000F0EFB
		public CodeConditionStatement()
		{
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x000F2D19 File Offset: 0x000F0F19
		public CodeConditionStatement(CodeExpression condition, params CodeStatement[] trueStatements)
		{
			this.Condition = condition;
			this.TrueStatements.AddRange(trueStatements);
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x000F2D4A File Offset: 0x000F0F4A
		public CodeConditionStatement(CodeExpression condition, CodeStatement[] trueStatements, CodeStatement[] falseStatements)
		{
			this.Condition = condition;
			this.TrueStatements.AddRange(trueStatements);
			this.FalseStatements.AddRange(falseStatements);
		}

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06003997 RID: 14743 RVA: 0x000F2D87 File Offset: 0x000F0F87
		// (set) Token: 0x06003998 RID: 14744 RVA: 0x000F2D8F File Offset: 0x000F0F8F
		public CodeExpression Condition
		{
			get
			{
				return this.condition;
			}
			set
			{
				this.condition = value;
			}
		}

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06003999 RID: 14745 RVA: 0x000F2D98 File Offset: 0x000F0F98
		public CodeStatementCollection TrueStatements
		{
			get
			{
				return this.trueStatments;
			}
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x0600399A RID: 14746 RVA: 0x000F2DA0 File Offset: 0x000F0FA0
		public CodeStatementCollection FalseStatements
		{
			get
			{
				return this.falseStatments;
			}
		}

		// Token: 0x04002BAF RID: 11183
		private CodeExpression condition;

		// Token: 0x04002BB0 RID: 11184
		private CodeStatementCollection trueStatments = new CodeStatementCollection();

		// Token: 0x04002BB1 RID: 11185
		private CodeStatementCollection falseStatments = new CodeStatementCollection();
	}
}
