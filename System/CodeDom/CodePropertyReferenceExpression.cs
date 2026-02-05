using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200064D RID: 1613
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodePropertyReferenceExpression : CodeExpression
	{
		// Token: 0x06003A9F RID: 15007 RVA: 0x000F4308 File Offset: 0x000F2508
		public CodePropertyReferenceExpression()
		{
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x000F431B File Offset: 0x000F251B
		public CodePropertyReferenceExpression(CodeExpression targetObject, string propertyName)
		{
			this.TargetObject = targetObject;
			this.PropertyName = propertyName;
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06003AA1 RID: 15009 RVA: 0x000F433C File Offset: 0x000F253C
		// (set) Token: 0x06003AA2 RID: 15010 RVA: 0x000F4344 File Offset: 0x000F2544
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

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06003AA3 RID: 15011 RVA: 0x000F434D File Offset: 0x000F254D
		// (set) Token: 0x06003AA4 RID: 15012 RVA: 0x000F4363 File Offset: 0x000F2563
		public string PropertyName
		{
			get
			{
				if (this.propertyName != null)
				{
					return this.propertyName;
				}
				return string.Empty;
			}
			set
			{
				this.propertyName = value;
			}
		}

		// Token: 0x04002C05 RID: 11269
		private CodeExpression targetObject;

		// Token: 0x04002C06 RID: 11270
		private string propertyName;

		// Token: 0x04002C07 RID: 11271
		private CodeExpressionCollection parameters = new CodeExpressionCollection();
	}
}
