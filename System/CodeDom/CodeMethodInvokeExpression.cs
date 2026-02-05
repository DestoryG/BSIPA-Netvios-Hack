using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000641 RID: 1601
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeMethodInvokeExpression : CodeExpression
	{
		// Token: 0x06003A2D RID: 14893 RVA: 0x000F389B File Offset: 0x000F1A9B
		public CodeMethodInvokeExpression()
		{
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x000F38AE File Offset: 0x000F1AAE
		public CodeMethodInvokeExpression(CodeMethodReferenceExpression method, params CodeExpression[] parameters)
		{
			this.method = method;
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x000F38D4 File Offset: 0x000F1AD4
		public CodeMethodInvokeExpression(CodeExpression targetObject, string methodName, params CodeExpression[] parameters)
		{
			this.method = new CodeMethodReferenceExpression(targetObject, methodName);
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06003A30 RID: 14896 RVA: 0x000F3900 File Offset: 0x000F1B00
		// (set) Token: 0x06003A31 RID: 14897 RVA: 0x000F391B File Offset: 0x000F1B1B
		public CodeMethodReferenceExpression Method
		{
			get
			{
				if (this.method == null)
				{
					this.method = new CodeMethodReferenceExpression();
				}
				return this.method;
			}
			set
			{
				this.method = value;
			}
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06003A32 RID: 14898 RVA: 0x000F3924 File Offset: 0x000F1B24
		public CodeExpressionCollection Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04002BE7 RID: 11239
		private CodeMethodReferenceExpression method;

		// Token: 0x04002BE8 RID: 11240
		private CodeExpressionCollection parameters = new CodeExpressionCollection();
	}
}
