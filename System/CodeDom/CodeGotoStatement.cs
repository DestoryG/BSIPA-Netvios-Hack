using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000638 RID: 1592
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeGotoStatement : CodeStatement
	{
		// Token: 0x060039E3 RID: 14819 RVA: 0x000F3246 File Offset: 0x000F1446
		public CodeGotoStatement()
		{
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x000F324E File Offset: 0x000F144E
		public CodeGotoStatement(string label)
		{
			this.Label = label;
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x060039E5 RID: 14821 RVA: 0x000F325D File Offset: 0x000F145D
		// (set) Token: 0x060039E6 RID: 14822 RVA: 0x000F3265 File Offset: 0x000F1465
		public string Label
		{
			get
			{
				return this.label;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("value");
				}
				this.label = value;
			}
		}

		// Token: 0x04002BC1 RID: 11201
		private string label;
	}
}
