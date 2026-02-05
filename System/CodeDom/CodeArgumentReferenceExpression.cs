using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000616 RID: 1558
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeArgumentReferenceExpression : CodeExpression
	{
		// Token: 0x060038F4 RID: 14580 RVA: 0x000F2157 File Offset: 0x000F0357
		public CodeArgumentReferenceExpression()
		{
		}

		// Token: 0x060038F5 RID: 14581 RVA: 0x000F215F File Offset: 0x000F035F
		public CodeArgumentReferenceExpression(string parameterName)
		{
			this.parameterName = parameterName;
		}

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x060038F6 RID: 14582 RVA: 0x000F216E File Offset: 0x000F036E
		// (set) Token: 0x060038F7 RID: 14583 RVA: 0x000F2184 File Offset: 0x000F0384
		public string ParameterName
		{
			get
			{
				if (this.parameterName != null)
				{
					return this.parameterName;
				}
				return string.Empty;
			}
			set
			{
				this.parameterName = value;
			}
		}

		// Token: 0x04002B7A RID: 11130
		private string parameterName;
	}
}
