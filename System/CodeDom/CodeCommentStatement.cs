using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000627 RID: 1575
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeCommentStatement : CodeStatement
	{
		// Token: 0x0600397B RID: 14715 RVA: 0x000F2B13 File Offset: 0x000F0D13
		public CodeCommentStatement()
		{
		}

		// Token: 0x0600397C RID: 14716 RVA: 0x000F2B1B File Offset: 0x000F0D1B
		public CodeCommentStatement(CodeComment comment)
		{
			this.comment = comment;
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x000F2B2A File Offset: 0x000F0D2A
		public CodeCommentStatement(string text)
		{
			this.comment = new CodeComment(text);
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x000F2B3E File Offset: 0x000F0D3E
		public CodeCommentStatement(string text, bool docComment)
		{
			this.comment = new CodeComment(text, docComment);
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x0600397F RID: 14719 RVA: 0x000F2B53 File Offset: 0x000F0D53
		// (set) Token: 0x06003980 RID: 14720 RVA: 0x000F2B5B File Offset: 0x000F0D5B
		public CodeComment Comment
		{
			get
			{
				return this.comment;
			}
			set
			{
				this.comment = value;
			}
		}

		// Token: 0x04002BA9 RID: 11177
		private CodeComment comment;
	}
}
