using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x0200065F RID: 1631
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeMember : CodeObject
	{
		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x06003B0B RID: 15115 RVA: 0x000F4C4C File Offset: 0x000F2E4C
		// (set) Token: 0x06003B0C RID: 15116 RVA: 0x000F4C62 File Offset: 0x000F2E62
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

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x06003B0D RID: 15117 RVA: 0x000F4C6B File Offset: 0x000F2E6B
		// (set) Token: 0x06003B0E RID: 15118 RVA: 0x000F4C73 File Offset: 0x000F2E73
		public MemberAttributes Attributes
		{
			get
			{
				return this.attributes;
			}
			set
			{
				this.attributes = value;
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06003B0F RID: 15119 RVA: 0x000F4C7C File Offset: 0x000F2E7C
		// (set) Token: 0x06003B10 RID: 15120 RVA: 0x000F4C97 File Offset: 0x000F2E97
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

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06003B11 RID: 15121 RVA: 0x000F4CA0 File Offset: 0x000F2EA0
		// (set) Token: 0x06003B12 RID: 15122 RVA: 0x000F4CA8 File Offset: 0x000F2EA8
		public CodeLinePragma LinePragma
		{
			get
			{
				return this.linePragma;
			}
			set
			{
				this.linePragma = value;
			}
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x06003B13 RID: 15123 RVA: 0x000F4CB1 File Offset: 0x000F2EB1
		public CodeCommentStatementCollection Comments
		{
			get
			{
				return this.comments;
			}
		}

		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x06003B14 RID: 15124 RVA: 0x000F4CB9 File Offset: 0x000F2EB9
		public CodeDirectiveCollection StartDirectives
		{
			get
			{
				if (this.startDirectives == null)
				{
					this.startDirectives = new CodeDirectiveCollection();
				}
				return this.startDirectives;
			}
		}

		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x06003B15 RID: 15125 RVA: 0x000F4CD4 File Offset: 0x000F2ED4
		public CodeDirectiveCollection EndDirectives
		{
			get
			{
				if (this.endDirectives == null)
				{
					this.endDirectives = new CodeDirectiveCollection();
				}
				return this.endDirectives;
			}
		}

		// Token: 0x04002C2A RID: 11306
		private MemberAttributes attributes = (MemberAttributes)20482;

		// Token: 0x04002C2B RID: 11307
		private string name;

		// Token: 0x04002C2C RID: 11308
		private CodeCommentStatementCollection comments = new CodeCommentStatementCollection();

		// Token: 0x04002C2D RID: 11309
		private CodeAttributeDeclarationCollection customAttributes;

		// Token: 0x04002C2E RID: 11310
		private CodeLinePragma linePragma;

		// Token: 0x04002C2F RID: 11311
		[OptionalField]
		private CodeDirectiveCollection startDirectives;

		// Token: 0x04002C30 RID: 11312
		[OptionalField]
		private CodeDirectiveCollection endDirectives;
	}
}
