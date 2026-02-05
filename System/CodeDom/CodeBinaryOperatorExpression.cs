using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000620 RID: 1568
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeBinaryOperatorExpression : CodeExpression
	{
		// Token: 0x06003946 RID: 14662 RVA: 0x000F27A4 File Offset: 0x000F09A4
		public CodeBinaryOperatorExpression()
		{
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x000F27AC File Offset: 0x000F09AC
		public CodeBinaryOperatorExpression(CodeExpression left, CodeBinaryOperatorType op, CodeExpression right)
		{
			this.Right = right;
			this.Operator = op;
			this.Left = left;
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06003948 RID: 14664 RVA: 0x000F27C9 File Offset: 0x000F09C9
		// (set) Token: 0x06003949 RID: 14665 RVA: 0x000F27D1 File Offset: 0x000F09D1
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

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x0600394A RID: 14666 RVA: 0x000F27DA File Offset: 0x000F09DA
		// (set) Token: 0x0600394B RID: 14667 RVA: 0x000F27E2 File Offset: 0x000F09E2
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

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x0600394C RID: 14668 RVA: 0x000F27EB File Offset: 0x000F09EB
		// (set) Token: 0x0600394D RID: 14669 RVA: 0x000F27F3 File Offset: 0x000F09F3
		public CodeBinaryOperatorType Operator
		{
			get
			{
				return this.op;
			}
			set
			{
				this.op = value;
			}
		}

		// Token: 0x04002B8A RID: 11146
		private CodeBinaryOperatorType op;

		// Token: 0x04002B8B RID: 11147
		private CodeExpression left;

		// Token: 0x04002B8C RID: 11148
		private CodeExpression right;
	}
}
