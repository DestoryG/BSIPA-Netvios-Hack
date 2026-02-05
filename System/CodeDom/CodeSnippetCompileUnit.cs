using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000652 RID: 1618
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeSnippetCompileUnit : CodeCompileUnit
	{
		// Token: 0x06003AB3 RID: 15027 RVA: 0x000F4431 File Offset: 0x000F2631
		public CodeSnippetCompileUnit()
		{
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x000F4439 File Offset: 0x000F2639
		public CodeSnippetCompileUnit(string value)
		{
			this.Value = value;
		}

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06003AB5 RID: 15029 RVA: 0x000F4448 File Offset: 0x000F2648
		// (set) Token: 0x06003AB6 RID: 15030 RVA: 0x000F445E File Offset: 0x000F265E
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

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06003AB7 RID: 15031 RVA: 0x000F4467 File Offset: 0x000F2667
		// (set) Token: 0x06003AB8 RID: 15032 RVA: 0x000F446F File Offset: 0x000F266F
		public CodeLinePragma LinePragma
		{
			get
			{
				return this.linePragma;
			}
			set
			{
				this.linePragma = value;
			}
		}

		// Token: 0x04002C10 RID: 11280
		private string value;

		// Token: 0x04002C11 RID: 11281
		private CodeLinePragma linePragma;
	}
}
