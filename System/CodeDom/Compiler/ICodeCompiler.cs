using System;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x0200067E RID: 1662
	public interface ICodeCompiler
	{
		// Token: 0x06003D2F RID: 15663
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		CompilerResults CompileAssemblyFromDom(CompilerParameters options, CodeCompileUnit compilationUnit);

		// Token: 0x06003D30 RID: 15664
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		CompilerResults CompileAssemblyFromFile(CompilerParameters options, string fileName);

		// Token: 0x06003D31 RID: 15665
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		CompilerResults CompileAssemblyFromSource(CompilerParameters options, string source);

		// Token: 0x06003D32 RID: 15666
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		CompilerResults CompileAssemblyFromDomBatch(CompilerParameters options, CodeCompileUnit[] compilationUnits);

		// Token: 0x06003D33 RID: 15667
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		CompilerResults CompileAssemblyFromFileBatch(CompilerParameters options, string[] fileNames);

		// Token: 0x06003D34 RID: 15668
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		CompilerResults CompileAssemblyFromSourceBatch(CompilerParameters options, string[] sources);
	}
}
