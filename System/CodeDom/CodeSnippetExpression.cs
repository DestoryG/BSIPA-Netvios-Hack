using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000653 RID: 1619
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeSnippetExpression : CodeExpression
	{
		// Token: 0x06003AB9 RID: 15033 RVA: 0x000F4478 File Offset: 0x000F2678
		public CodeSnippetExpression()
		{
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x000F4480 File Offset: 0x000F2680
		public CodeSnippetExpression(string value)
		{
			this.Value = value;
		}

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06003ABB RID: 15035 RVA: 0x000F448F File Offset: 0x000F268F
		// (set) Token: 0x06003ABC RID: 15036 RVA: 0x000F44A5 File Offset: 0x000F26A5
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

		// Token: 0x04002C12 RID: 11282
		private string value;
	}
}
