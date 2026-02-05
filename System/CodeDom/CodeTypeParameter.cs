using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000662 RID: 1634
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeParameter : CodeObject
	{
		// Token: 0x06003B2A RID: 15146 RVA: 0x000F4E84 File Offset: 0x000F3084
		public CodeTypeParameter()
		{
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x000F4E8C File Offset: 0x000F308C
		public CodeTypeParameter(string name)
		{
			this.name = name;
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x06003B2C RID: 15148 RVA: 0x000F4E9B File Offset: 0x000F309B
		// (set) Token: 0x06003B2D RID: 15149 RVA: 0x000F4EB1 File Offset: 0x000F30B1
		public string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x06003B2E RID: 15150 RVA: 0x000F4EBA File Offset: 0x000F30BA
		public CodeTypeReferenceCollection Constraints
		{
			get
			{
				if (this.constraints == null)
				{
					this.constraints = new CodeTypeReferenceCollection();
				}
				return this.constraints;
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06003B2F RID: 15151 RVA: 0x000F4ED5 File Offset: 0x000F30D5
		public CodeAttributeDeclarationCollection CustomAttributes
		{
			get
			{
				if (this.customAttributes == null)
				{
					this.customAttributes = new CodeAttributeDeclarationCollection();
				}
				return this.customAttributes;
			}
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06003B30 RID: 15152 RVA: 0x000F4EF0 File Offset: 0x000F30F0
		// (set) Token: 0x06003B31 RID: 15153 RVA: 0x000F4EF8 File Offset: 0x000F30F8
		public bool HasConstructorConstraint
		{
			get
			{
				return this.hasConstructorConstraint;
			}
			set
			{
				this.hasConstructorConstraint = value;
			}
		}

		// Token: 0x04002C32 RID: 11314
		private string name;

		// Token: 0x04002C33 RID: 11315
		private CodeAttributeDeclarationCollection customAttributes;

		// Token: 0x04002C34 RID: 11316
		private CodeTypeReferenceCollection constraints;

		// Token: 0x04002C35 RID: 11317
		private bool hasConstructorConstraint;
	}
}
