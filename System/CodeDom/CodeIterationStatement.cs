using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200063A RID: 1594
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeIterationStatement : CodeStatement
	{
		// Token: 0x060039EC RID: 14828 RVA: 0x000F32DB File Offset: 0x000F14DB
		public CodeIterationStatement()
		{
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x000F32EE File Offset: 0x000F14EE
		public CodeIterationStatement(CodeStatement initStatement, CodeExpression testExpression, CodeStatement incrementStatement, params CodeStatement[] statements)
		{
			this.InitStatement = initStatement;
			this.TestExpression = testExpression;
			this.IncrementStatement = incrementStatement;
			this.Statements.AddRange(statements);
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x060039EE RID: 14830 RVA: 0x000F3323 File Offset: 0x000F1523
		// (set) Token: 0x060039EF RID: 14831 RVA: 0x000F332B File Offset: 0x000F152B
		public CodeStatement InitStatement
		{
			get
			{
				return this.initStatement;
			}
			set
			{
				this.initStatement = value;
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x060039F0 RID: 14832 RVA: 0x000F3334 File Offset: 0x000F1534
		// (set) Token: 0x060039F1 RID: 14833 RVA: 0x000F333C File Offset: 0x000F153C
		public CodeExpression TestExpression
		{
			get
			{
				return this.testExpression;
			}
			set
			{
				this.testExpression = value;
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x060039F2 RID: 14834 RVA: 0x000F3345 File Offset: 0x000F1545
		// (set) Token: 0x060039F3 RID: 14835 RVA: 0x000F334D File Offset: 0x000F154D
		public CodeStatement IncrementStatement
		{
			get
			{
				return this.incrementStatement;
			}
			set
			{
				this.incrementStatement = value;
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x060039F4 RID: 14836 RVA: 0x000F3356 File Offset: 0x000F1556
		public CodeStatementCollection Statements
		{
			get
			{
				return this.statements;
			}
		}

		// Token: 0x04002BC4 RID: 11204
		private CodeStatement initStatement;

		// Token: 0x04002BC5 RID: 11205
		private CodeExpression testExpression;

		// Token: 0x04002BC6 RID: 11206
		private CodeStatement incrementStatement;

		// Token: 0x04002BC7 RID: 11207
		private CodeStatementCollection statements = new CodeStatementCollection();
	}
}
