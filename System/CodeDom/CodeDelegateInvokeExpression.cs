using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200062E RID: 1582
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeDelegateInvokeExpression : CodeExpression
	{
		// Token: 0x060039AA RID: 14762 RVA: 0x000F2E9F File Offset: 0x000F109F
		public CodeDelegateInvokeExpression()
		{
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x000F2EB2 File Offset: 0x000F10B2
		public CodeDelegateInvokeExpression(CodeExpression targetObject)
		{
			this.TargetObject = targetObject;
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x000F2ECC File Offset: 0x000F10CC
		public CodeDelegateInvokeExpression(CodeExpression targetObject, params CodeExpression[] parameters)
		{
			this.TargetObject = targetObject;
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x060039AD RID: 14765 RVA: 0x000F2EF2 File Offset: 0x000F10F2
		// (set) Token: 0x060039AE RID: 14766 RVA: 0x000F2EFA File Offset: 0x000F10FA
		public CodeExpression TargetObject
		{
			get
			{
				return this.targetObject;
			}
			set
			{
				this.targetObject = value;
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x060039AF RID: 14767 RVA: 0x000F2F03 File Offset: 0x000F1103
		public CodeExpressionCollection Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04002BB8 RID: 11192
		private CodeExpression targetObject;

		// Token: 0x04002BB9 RID: 11193
		private CodeExpressionCollection parameters = new CodeExpressionCollection();
	}
}
