using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x02000642 RID: 1602
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeMethodReferenceExpression : CodeExpression
	{
		// Token: 0x06003A33 RID: 14899 RVA: 0x000F392C File Offset: 0x000F1B2C
		public CodeMethodReferenceExpression()
		{
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x000F3934 File Offset: 0x000F1B34
		public CodeMethodReferenceExpression(CodeExpression targetObject, string methodName)
		{
			this.TargetObject = targetObject;
			this.MethodName = methodName;
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x000F394A File Offset: 0x000F1B4A
		public CodeMethodReferenceExpression(CodeExpression targetObject, string methodName, params CodeTypeReference[] typeParameters)
		{
			this.TargetObject = targetObject;
			this.MethodName = methodName;
			if (typeParameters != null && typeParameters.Length != 0)
			{
				this.TypeArguments.AddRange(typeParameters);
			}
		}

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06003A36 RID: 14902 RVA: 0x000F3973 File Offset: 0x000F1B73
		// (set) Token: 0x06003A37 RID: 14903 RVA: 0x000F397B File Offset: 0x000F1B7B
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

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06003A38 RID: 14904 RVA: 0x000F3984 File Offset: 0x000F1B84
		// (set) Token: 0x06003A39 RID: 14905 RVA: 0x000F399A File Offset: 0x000F1B9A
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

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06003A3A RID: 14906 RVA: 0x000F39A3 File Offset: 0x000F1BA3
		[ComVisible(false)]
		public CodeTypeReferenceCollection TypeArguments
		{
			get
			{
				if (this.typeArguments == null)
				{
					this.typeArguments = new CodeTypeReferenceCollection();
				}
				return this.typeArguments;
			}
		}

		// Token: 0x04002BE9 RID: 11241
		private CodeExpression targetObject;

		// Token: 0x04002BEA RID: 11242
		private string methodName;

		// Token: 0x04002BEB RID: 11243
		[OptionalField]
		private CodeTypeReferenceCollection typeArguments;
	}
}
