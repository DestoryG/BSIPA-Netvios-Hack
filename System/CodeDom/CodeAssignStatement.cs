using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000619 RID: 1561
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeAssignStatement : CodeStatement
	{
		// Token: 0x0600390E RID: 14606 RVA: 0x000F23A3 File Offset: 0x000F05A3
		public CodeAssignStatement()
		{
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x000F23AB File Offset: 0x000F05AB
		public CodeAssignStatement(CodeExpression left, CodeExpression right)
		{
			this.Left = left;
			this.Right = right;
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x06003910 RID: 14608 RVA: 0x000F23C1 File Offset: 0x000F05C1
		// (set) Token: 0x06003911 RID: 14609 RVA: 0x000F23C9 File Offset: 0x000F05C9
		public CodeExpression Left
		{
			get
			{
				return this.left;
			}
			set
			{
				this.left = value;
			}
		}

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x06003912 RID: 14610 RVA: 0x000F23D2 File Offset: 0x000F05D2
		// (set) Token: 0x06003913 RID: 14611 RVA: 0x000F23DA File Offset: 0x000F05DA
		public CodeExpression Right
		{
			get
			{
				return this.right;
			}
			set
			{
				this.right = value;
			}
		}

		// Token: 0x04002B81 RID: 11137
		private CodeExpression left;

		// Token: 0x04002B82 RID: 11138
		private CodeExpression right;
	}
}
