using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000643 RID: 1603
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeMethodReturnStatement : CodeStatement
	{
		// Token: 0x06003A3B RID: 14907 RVA: 0x000F39BE File Offset: 0x000F1BBE
		public CodeMethodReturnStatement()
		{
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x000F39C6 File Offset: 0x000F1BC6
		public CodeMethodReturnStatement(CodeExpression expression)
		{
			this.Expression = expression;
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06003A3D RID: 14909 RVA: 0x000F39D5 File Offset: 0x000F1BD5
		// (set) Token: 0x06003A3E RID: 14910 RVA: 0x000F39DD File Offset: 0x000F1BDD
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

		// Token: 0x04002BEC RID: 11244
		private CodeExpression expression;
	}
}
