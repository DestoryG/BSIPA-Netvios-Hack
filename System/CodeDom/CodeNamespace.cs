using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x02000644 RID: 1604
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeNamespace : CodeObject
	{
		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06003A3F RID: 14911 RVA: 0x000F39E8 File Offset: 0x000F1BE8
		// (remove) Token: 0x06003A40 RID: 14912 RVA: 0x000F3A20 File Offset: 0x000F1C20
		public event EventHandler PopulateComments;

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06003A41 RID: 14913 RVA: 0x000F3A58 File Offset: 0x000F1C58
		// (remove) Token: 0x06003A42 RID: 14914 RVA: 0x000F3A90 File Offset: 0x000F1C90
		public event EventHandler PopulateImports;

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06003A43 RID: 14915 RVA: 0x000F3AC8 File Offset: 0x000F1CC8
		// (remove) Token: 0x06003A44 RID: 14916 RVA: 0x000F3B00 File Offset: 0x000F1D00
		public event EventHandler PopulateTypes;

		// Token: 0x06003A45 RID: 14917 RVA: 0x000F3B35 File Offset: 0x000F1D35
		public CodeNamespace()
		{
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x000F3B69 File Offset: 0x000F1D69
		public CodeNamespace(string name)
		{
			this.Name = name;
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x000F3BA4 File Offset: 0x000F1DA4
		private CodeNamespace(SerializationInfo info, StreamingContext context)
		{
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06003A48 RID: 14920 RVA: 0x000F3BD8 File Offset: 0x000F1DD8
		public CodeTypeDeclarationCollection Types
		{
			get
			{
				if ((this.populated & 4) == 0)
				{
					this.populated |= 4;
					if (this.PopulateTypes != null)
					{
						this.PopulateTypes(this, EventArgs.Empty);
					}
				}
				return this.classes;
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06003A49 RID: 14921 RVA: 0x000F3C11 File Offset: 0x000F1E11
		public CodeNamespaceImportCollection Imports
		{
			get
			{
				if ((this.populated & 1) == 0)
				{
					this.populated |= 1;
					if (this.PopulateImports != null)
					{
						this.PopulateImports(this, EventArgs.Empty);
					}
				}
				return this.imports;
			}
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06003A4A RID: 14922 RVA: 0x000F3C4A File Offset: 0x000F1E4A
		// (set) Token: 0x06003A4B RID: 14923 RVA: 0x000F3C60 File Offset: 0x000F1E60
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

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06003A4C RID: 14924 RVA: 0x000F3C69 File Offset: 0x000F1E69
		public CodeCommentStatementCollection Comments
		{
			get
			{
				if ((this.populated & 2) == 0)
				{
					this.populated |= 2;
					if (this.PopulateComments != null)
					{
						this.PopulateComments(this, EventArgs.Empty);
					}
				}
				return this.comments;
			}
		}

		// Token: 0x04002BED RID: 11245
		private string name;

		// Token: 0x04002BEE RID: 11246
		private CodeNamespaceImportCollection imports = new CodeNamespaceImportCollection();

		// Token: 0x04002BEF RID: 11247
		private CodeCommentStatementCollection comments = new CodeCommentStatementCollection();

		// Token: 0x04002BF0 RID: 11248
		private CodeTypeDeclarationCollection classes = new CodeTypeDeclarationCollection();

		// Token: 0x04002BF1 RID: 11249
		private CodeNamespaceCollection namespaces = new CodeNamespaceCollection();

		// Token: 0x04002BF2 RID: 11250
		private int populated;

		// Token: 0x04002BF3 RID: 11251
		private const int ImportsCollection = 1;

		// Token: 0x04002BF4 RID: 11252
		private const int CommentsCollection = 2;

		// Token: 0x04002BF5 RID: 11253
		private const int TypesCollection = 4;
	}
}
