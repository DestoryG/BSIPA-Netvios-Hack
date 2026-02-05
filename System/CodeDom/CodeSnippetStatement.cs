using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000654 RID: 1620
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeSnippetStatement : CodeStatement
	{
		// Token: 0x06003ABD RID: 15037 RVA: 0x000F44AE File Offset: 0x000F26AE
		public CodeSnippetStatement()
		{
		}

		// Token: 0x06003ABE RID: 15038 RVA: 0x000F44B6 File Offset: 0x000F26B6
		public CodeSnippetStatement(string value)
		{
			this.Value = value;
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06003ABF RID: 15039 RVA: 0x000F44C5 File Offset: 0x000F26C5
		// (set) Token: 0x06003AC0 RID: 15040 RVA: 0x000F44DB File Offset: 0x000F26DB
		public string Value
		{
			get
			{
				if (this.value != null)
				{
					return this.value;
				}
				return string.Empty;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x04002C13 RID: 11283
		private string value;
	}
}
