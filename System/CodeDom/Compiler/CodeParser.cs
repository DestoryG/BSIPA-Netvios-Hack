using System;
using System.IO;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000673 RID: 1651
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class CodeParser : ICodeParser
	{
		// Token: 0x06003C66 RID: 15462
		public abstract CodeCompileUnit Parse(TextReader codeStream);
	}
}
