using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security.Permissions;

namespace Microsoft.CSharp
{
	// Token: 0x0200000C RID: 12
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class CSharpCodeProvider : CodeDomProvider
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00006D7F File Offset: 0x00004F7F
		public CSharpCodeProvider()
		{
			this.generator = new CSharpCodeGenerator();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00006D92 File Offset: 0x00004F92
		public CSharpCodeProvider(IDictionary<string, string> providerOptions)
		{
			if (providerOptions == null)
			{
				throw new ArgumentNullException("providerOptions");
			}
			this.generator = new CSharpCodeGenerator(providerOptions);
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00006DB4 File Offset: 0x00004FB4
		public override string FileExtension
		{
			get
			{
				return "cs";
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00006DBB File Offset: 0x00004FBB
		[Obsolete("Callers should not use the ICodeGenerator interface and should instead use the methods directly on the CodeDomProvider class.")]
		public override ICodeGenerator CreateGenerator()
		{
			return this.generator;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00006DC3 File Offset: 0x00004FC3
		[Obsolete("Callers should not use the ICodeCompiler interface and should instead use the methods directly on the CodeDomProvider class.")]
		public override ICodeCompiler CreateCompiler()
		{
			return this.generator;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00006DCB File Offset: 0x00004FCB
		public override TypeConverter GetConverter(Type type)
		{
			if (type == typeof(MemberAttributes))
			{
				return CSharpMemberAttributeConverter.Default;
			}
			if (type == typeof(TypeAttributes))
			{
				return CSharpTypeAttributeConverter.Default;
			}
			return base.GetConverter(type);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00006E04 File Offset: 0x00005004
		public override void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions options)
		{
			this.generator.GenerateCodeFromMember(member, writer, options);
		}

		// Token: 0x04000066 RID: 102
		private CSharpCodeGenerator generator;
	}
}
