using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000667 RID: 1639
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeReferenceExpression : CodeExpression
	{
		// Token: 0x06003B67 RID: 15207 RVA: 0x000F57F8 File Offset: 0x000F39F8
		public CodeTypeReferenceExpression()
		{
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x000F5800 File Offset: 0x000F3A00
		public CodeTypeReferenceExpression(CodeTypeReference type)
		{
			this.Type = type;
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x000F580F File Offset: 0x000F3A0F
		public CodeTypeReferenceExpression(string type)
		{
			this.Type = new CodeTypeReference(type);
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x000F5823 File Offset: 0x000F3A23
		public CodeTypeReferenceExpression(Type type)
		{
			this.Type = new CodeTypeReference(type);
		}

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x06003B6B RID: 15211 RVA: 0x000F5837 File Offset: 0x000F3A37
		// (set) Token: 0x06003B6C RID: 15212 RVA: 0x000F5857 File Offset: 0x000F3A57
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

		// Token: 0x04002C40 RID: 11328
		private CodeTypeReference type;
	}
}
