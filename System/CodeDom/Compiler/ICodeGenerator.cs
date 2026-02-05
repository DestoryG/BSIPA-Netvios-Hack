using System;
using System.IO;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x0200067F RID: 1663
	public interface ICodeGenerator
	{
		// Token: 0x06003D35 RID: 15669
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		bool IsValidIdentifier(string value);

		// Token: 0x06003D36 RID: 15670
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		void ValidateIdentifier(string value);

		// Token: 0x06003D37 RID: 15671
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		string CreateEscapedIdentifier(string value);

		// Token: 0x06003D38 RID: 15672
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		string CreateValidIdentifier(string value);

		// Token: 0x06003D39 RID: 15673
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		string GetTypeOutput(CodeTypeReference type);

		// Token: 0x06003D3A RID: 15674
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		bool Supports(GeneratorSupport supports);

		// Token: 0x06003D3B RID: 15675
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		void GenerateCodeFromExpression(CodeExpression e, TextWriter w, CodeGeneratorOptions o);

		// Token: 0x06003D3C RID: 15676
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		void GenerateCodeFromStatement(CodeStatement e, TextWriter w, CodeGeneratorOptions o);

		// Token: 0x06003D3D RID: 15677
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		void GenerateCodeFromNamespace(CodeNamespace e, TextWriter w, CodeGeneratorOptions o);

		// Token: 0x06003D3E RID: 15678
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		void GenerateCodeFromCompileUnit(CodeCompileUnit e, TextWriter w, CodeGeneratorOptions o);

		// Token: 0x06003D3F RID: 15679
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		void GenerateCodeFromType(CodeTypeDeclaration e, TextWriter w, CodeGeneratorOptions o);
	}
}
