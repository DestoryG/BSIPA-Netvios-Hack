using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000655 RID: 1621
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeSnippetTypeMember : CodeTypeMember
	{
		// Token: 0x06003AC1 RID: 15041 RVA: 0x000F44E4 File Offset: 0x000F26E4
		public CodeSnippetTypeMember()
		{
		}

		// Token: 0x06003AC2 RID: 15042 RVA: 0x000F44EC File Offset: 0x000F26EC
		public CodeSnippetTypeMember(string text)
		{
			this.Text = text;
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06003AC3 RID: 15043 RVA: 0x000F44FB File Offset: 0x000F26FB
		// (set) Token: 0x06003AC4 RID: 15044 RVA: 0x000F4511 File Offset: 0x000F2711
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

		// Token: 0x04002C14 RID: 11284
		private string text;
	}
}
