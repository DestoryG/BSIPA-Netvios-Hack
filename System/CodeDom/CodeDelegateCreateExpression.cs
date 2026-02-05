using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200062D RID: 1581
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeDelegateCreateExpression : CodeExpression
	{
		// Token: 0x060039A2 RID: 14754 RVA: 0x000F2E21 File Offset: 0x000F1021
		public CodeDelegateCreateExpression()
		{
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x000F2E29 File Offset: 0x000F1029
		public CodeDelegateCreateExpression(CodeTypeReference delegateType, CodeExpression targetObject, string methodName)
		{
			this.delegateType = delegateType;
			this.targetObject = targetObject;
			this.methodName = methodName;
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x060039A4 RID: 14756 RVA: 0x000F2E46 File Offset: 0x000F1046
		// (set) Token: 0x060039A5 RID: 14757 RVA: 0x000F2E66 File Offset: 0x000F1066
		public CodeTypeReference DelegateType
		{
			get
			{
				if (this.delegateType == null)
				{
					this.delegateType = new CodeTypeReference("");
				}
				return this.delegateType;
			}
			set
			{
				this.delegateType = value;
			}
		}

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x060039A6 RID: 14758 RVA: 0x000F2E6F File Offset: 0x000F106F
		// (set) Token: 0x060039A7 RID: 14759 RVA: 0x000F2E77 File Offset: 0x000F1077
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

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x060039A8 RID: 14760 RVA: 0x000F2E80 File Offset: 0x000F1080
		// (set) Token: 0x060039A9 RID: 14761 RVA: 0x000F2E96 File Offset: 0x000F1096
		public string MethodName
		{
			get
			{
				if (this.methodName != null)
				{
					return this.methodName;
				}
				return string.Empty;
			}
			set
			{
				this.methodName = value;
			}
		}

		// Token: 0x04002BB5 RID: 11189
		private CodeTypeReference delegateType;

		// Token: 0x04002BB6 RID: 11190
		private CodeExpression targetObject;

		// Token: 0x04002BB7 RID: 11191
		private string methodName;
	}
}
