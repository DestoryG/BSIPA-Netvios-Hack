using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000669 RID: 1641
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeVariableReferenceExpression : CodeExpression
	{
		// Token: 0x06003B7A RID: 15226 RVA: 0x000F596E File Offset: 0x000F3B6E
		public CodeVariableReferenceExpression()
		{
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x000F5976 File Offset: 0x000F3B76
		public CodeVariableReferenceExpression(string variableName)
		{
			this.variableName = variableName;
		}

		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x06003B7C RID: 15228 RVA: 0x000F5985 File Offset: 0x000F3B85
		// (set) Token: 0x06003B7D RID: 15229 RVA: 0x000F599B File Offset: 0x000F3B9B
		public string VariableName
		{
			get
			{
				if (this.variableName != null)
				{
					return this.variableName;
				}
				return string.Empty;
			}
			set
			{
				this.variableName = value;
			}
		}

		// Token: 0x04002C44 RID: 11332
		private string variableName;
	}
}
