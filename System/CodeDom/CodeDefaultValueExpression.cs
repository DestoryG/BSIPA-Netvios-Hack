using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200062C RID: 1580
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeDefaultValueExpression : CodeExpression
	{
		// Token: 0x0600399E RID: 14750 RVA: 0x000F2DE1 File Offset: 0x000F0FE1
		public CodeDefaultValueExpression()
		{
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x000F2DE9 File Offset: 0x000F0FE9
		public CodeDefaultValueExpression(CodeTypeReference type)
		{
			this.type = type;
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x060039A0 RID: 14752 RVA: 0x000F2DF8 File Offset: 0x000F0FF8
		// (set) Token: 0x060039A1 RID: 14753 RVA: 0x000F2E18 File Offset: 0x000F1018
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

		// Token: 0x04002BB4 RID: 11188
		private CodeTypeReference type;
	}
}
