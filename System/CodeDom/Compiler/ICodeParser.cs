using System;
using System.IO;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000680 RID: 1664
	public interface ICodeParser
	{
		// Token: 0x06003D40 RID: 15680
		CodeCompileUnit Parse(TextReader codeStream);
	}
}
