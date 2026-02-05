using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000626 RID: 1574
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeComment : CodeObject
	{
		// Token: 0x06003974 RID: 14708 RVA: 0x000F2AB6 File Offset: 0x000F0CB6
		public CodeComment()
		{
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x000F2ABE File Offset: 0x000F0CBE
		public CodeComment(string text)
		{
			this.Text = text;
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x000F2ACD File Offset: 0x000F0CCD
		public CodeComment(string text, bool docComment)
		{
			this.Text = text;
			this.docComment = docComment;
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06003977 RID: 14711 RVA: 0x000F2AE3 File Offset: 0x000F0CE3
		// (set) Token: 0x06003978 RID: 14712 RVA: 0x000F2AEB File Offset: 0x000F0CEB
		public bool DocComment
		{
			get
			{
				return this.docComment;
			}
			set
			{
				this.docComment = value;
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06003979 RID: 14713 RVA: 0x000F2AF4 File Offset: 0x000F0CF4
		// (set) Token: 0x0600397A RID: 14714 RVA: 0x000F2B0A File Offset: 0x000F0D0A
		public string Text
		{
			get
			{
				if (this.text != null)
				{
					return this.text;
				}
				return string.Empty;
			}
			set
			{
				this.text = value;
			}
		}

		// Token: 0x04002BA7 RID: 11175
		private string text;

		// Token: 0x04002BA8 RID: 11176
		private bool docComment;
	}
}
