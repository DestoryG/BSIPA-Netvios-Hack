using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000637 RID: 1591
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeFieldReferenceExpression : CodeExpression
	{
		// Token: 0x060039DD RID: 14813 RVA: 0x000F31F8 File Offset: 0x000F13F8
		public CodeFieldReferenceExpression()
		{
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x000F3200 File Offset: 0x000F1400
		public CodeFieldReferenceExpression(CodeExpression targetObject, string fieldName)
		{
			this.TargetObject = targetObject;
			this.FieldName = fieldName;
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x060039DF RID: 14815 RVA: 0x000F3216 File Offset: 0x000F1416
		// (set) Token: 0x060039E0 RID: 14816 RVA: 0x000F321E File Offset: 0x000F141E
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

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x060039E1 RID: 14817 RVA: 0x000F3227 File Offset: 0x000F1427
		// (set) Token: 0x060039E2 RID: 14818 RVA: 0x000F323D File Offset: 0x000F143D
		public string FieldName
		{
			get
			{
				if (this.fieldName != null)
				{
					return this.fieldName;
				}
				return string.Empty;
			}
			set
			{
				this.fieldName = value;
			}
		}

		// Token: 0x04002BBF RID: 11199
		private CodeExpression targetObject;

		// Token: 0x04002BC0 RID: 11200
		private string fieldName;
	}
}
