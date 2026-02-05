using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200064A RID: 1610
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeParameterDeclarationExpression : CodeExpression
	{
		// Token: 0x06003A82 RID: 14978 RVA: 0x000F4101 File Offset: 0x000F2301
		public CodeParameterDeclarationExpression()
		{
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x000F4109 File Offset: 0x000F2309
		public CodeParameterDeclarationExpression(CodeTypeReference type, string name)
		{
			this.Type = type;
			this.Name = name;
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x000F411F File Offset: 0x000F231F
		public CodeParameterDeclarationExpression(string type, string name)
		{
			this.Type = new CodeTypeReference(type);
			this.Name = name;
		}

		// Token: 0x06003A85 RID: 14981 RVA: 0x000F413A File Offset: 0x000F233A
		public CodeParameterDeclarationExpression(Type type, string name)
		{
			this.Type = new CodeTypeReference(type);
			this.Name = name;
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06003A86 RID: 14982 RVA: 0x000F4155 File Offset: 0x000F2355
		// (set) Token: 0x06003A87 RID: 14983 RVA: 0x000F4170 File Offset: 0x000F2370
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
			set
			{
				this.customAttributes = value;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06003A88 RID: 14984 RVA: 0x000F4179 File Offset: 0x000F2379
		// (set) Token: 0x06003A89 RID: 14985 RVA: 0x000F4181 File Offset: 0x000F2381
		public FieldDirection Direction
		{
			get
			{
				return this.dir;
			}
			set
			{
				this.dir = value;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06003A8A RID: 14986 RVA: 0x000F418A File Offset: 0x000F238A
		// (set) Token: 0x06003A8B RID: 14987 RVA: 0x000F41AA File Offset: 0x000F23AA
		public CodeTypeReference Type
		{
			get
			{
				if (this.type == null)
				{
					this.type = new CodeTypeReference("");
				}
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06003A8C RID: 14988 RVA: 0x000F41B3 File Offset: 0x000F23B3
		// (set) Token: 0x06003A8D RID: 14989 RVA: 0x000F41C9 File Offset: 0x000F23C9
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

		// Token: 0x04002C00 RID: 11264
		private CodeTypeReference type;

		// Token: 0x04002C01 RID: 11265
		private string name;

		// Token: 0x04002C02 RID: 11266
		private CodeAttributeDeclarationCollection customAttributes;

		// Token: 0x04002C03 RID: 11267
		private FieldDirection dir;
	}
}
