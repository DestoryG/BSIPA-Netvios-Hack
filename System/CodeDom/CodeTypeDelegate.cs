using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200065E RID: 1630
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeDelegate : CodeTypeDeclaration
	{
		// Token: 0x06003B06 RID: 15110 RVA: 0x000F4BB0 File Offset: 0x000F2DB0
		public CodeTypeDelegate()
		{
			base.TypeAttributes &= ~TypeAttributes.ClassSemanticsMask;
			base.TypeAttributes |= TypeAttributes.NotPublic;
			base.BaseTypes.Clear();
			base.BaseTypes.Add(new CodeTypeReference("System.Delegate"));
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x000F4C0C File Offset: 0x000F2E0C
		public CodeTypeDelegate(string name)
			: this()
		{
			base.Name = name;
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x06003B08 RID: 15112 RVA: 0x000F4C1B File Offset: 0x000F2E1B
		// (set) Token: 0x06003B09 RID: 15113 RVA: 0x000F4C3B File Offset: 0x000F2E3B
		public CodeTypeReference ReturnType
		{
			get
			{
				if (this.returnType == null)
				{
					this.returnType = new CodeTypeReference("");
				}
				return this.returnType;
			}
			set
			{
				this.returnType = value;
			}
		}

		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x06003B0A RID: 15114 RVA: 0x000F4C44 File Offset: 0x000F2E44
		public CodeParameterDeclarationExpressionCollection Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04002C28 RID: 11304
		private CodeParameterDeclarationExpressionCollection parameters = new CodeParameterDeclarationExpressionCollection();

		// Token: 0x04002C29 RID: 11305
		private CodeTypeReference returnType;
	}
}
