using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200063B RID: 1595
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeLabeledStatement : CodeStatement
	{
		// Token: 0x060039F5 RID: 14837 RVA: 0x000F335E File Offset: 0x000F155E
		public CodeLabeledStatement()
		{
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x000F3366 File Offset: 0x000F1566
		public CodeLabeledStatement(string label)
		{
			this.label = label;
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x000F3375 File Offset: 0x000F1575
		public CodeLabeledStatement(string label, CodeStatement statement)
		{
			this.label = label;
			this.statement = statement;
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x060039F8 RID: 14840 RVA: 0x000F338B File Offset: 0x000F158B
		// (set) Token: 0x060039F9 RID: 14841 RVA: 0x000F33A1 File Offset: 0x000F15A1
		public string Label
		{
			get
			{
				if (this.label != null)
				{
					return this.label;
				}
				return string.Empty;
			}
			set
			{
				this.label = value;
			}
		}

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x060039FA RID: 14842 RVA: 0x000F33AA File Offset: 0x000F15AA
		// (set) Token: 0x060039FB RID: 14843 RVA: 0x000F33B2 File Offset: 0x000F15B2
		public CodeStatement Statement
		{
			get
			{
				return this.statement;
			}
			set
			{
				this.statement = value;
			}
		}

		// Token: 0x04002BC8 RID: 11208
		private string label;

		// Token: 0x04002BC9 RID: 11209
		private CodeStatement statement;
	}
}
