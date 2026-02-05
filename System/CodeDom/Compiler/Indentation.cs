using System;
using System.Text;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000682 RID: 1666
	internal class Indentation
	{
		// Token: 0x06003D6B RID: 15723 RVA: 0x000FBF13 File Offset: 0x000FA113
		internal Indentation(IndentedTextWriter writer, int indent)
		{
			this.writer = writer;
			this.indent = indent;
			this.s = null;
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06003D6C RID: 15724 RVA: 0x000FBF30 File Offset: 0x000FA130
		internal string IndentationString
		{
			get
			{
				if (this.s == null)
				{
					string tabString = this.writer.TabString;
					StringBuilder stringBuilder = new StringBuilder(this.indent * tabString.Length);
					for (int i = 0; i < this.indent; i++)
					{
						stringBuilder.Append(tabString);
					}
					this.s = stringBuilder.ToString();
				}
				return this.s;
			}
		}

		// Token: 0x04002CB3 RID: 11443
		private IndentedTextWriter writer;

		// Token: 0x04002CB4 RID: 11444
		private int indent;

		// Token: 0x04002CB5 RID: 11445
		private string s;
	}
}
