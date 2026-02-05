using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000636 RID: 1590
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeExpressionStatement : CodeStatement
	{
		// Token: 0x060039D9 RID: 14809 RVA: 0x000F31D0 File Offset: 0x000F13D0
		public CodeExpressionStatement()
		{
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x000F31D8 File Offset: 0x000F13D8
		public CodeExpressionStatement(CodeExpression expression)
		{
			this.expression = expression;
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x060039DB RID: 14811 RVA: 0x000F31E7 File Offset: 0x000F13E7
		// (set) Token: 0x060039DC RID: 14812 RVA: 0x000F31EF File Offset: 0x000F13EF
		public CodeExpression Expression
		{
			get
			{
				return this.expression;
			}
			set
			{
				this.expression = value;
			}
		}

		// Token: 0x04002BBE RID: 11198
		private CodeExpression expression;
	}
}
