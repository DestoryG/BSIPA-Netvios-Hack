using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security.Permissions;

namespace Microsoft.VisualBasic
{
	// Token: 0x02000007 RID: 7
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class VBCodeProvider : CodeDomProvider
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public VBCodeProvider()
		{
			this.generator = new VBCodeGenerator();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002063 File Offset: 0x00000263
		public VBCodeProvider(IDictionary<string, string> providerOptions)
		{
			if (providerOptions == null)
			{
				throw new ArgumentNullException("providerOptions");
			}
			this.generator = new VBCodeGenerator(providerOptions);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002085 File Offset: 0x00000285
		public override string FileExtension
		{
			get
			{
				return "vb";
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x0000208C File Offset: 0x0000028C
		public override LanguageOptions LanguageOptions
		{
			get
			{
				return LanguageOptions.CaseInsensitive;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000208F File Offset: 0x0000028F
		[Obsolete("Callers should not use the ICodeGenerator interface and should instead use the methods directly on the CodeDomProvider class.")]
		public override ICodeGenerator CreateGenerator()
		{
			return this.generator;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002097 File Offset: 0x00000297
		[Obsolete("Callers should not use the ICodeCompiler interface and should instead use the methods directly on the CodeDomProvider class.")]
		public override ICodeCompiler CreateCompiler()
		{
			return this.generator;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000209F File Offset: 0x0000029F
		public override TypeConverter GetConverter(Type type)
		{
			if (type == typeof(MemberAttributes))
			{
				return VBMemberAttributeConverter.Default;
			}
			if (type == typeof(TypeAttributes))
			{
				return VBTypeAttributeConverter.Default;
			}
			return base.GetConverter(type);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020D8 File Offset: 0x000002D8
		public override void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions options)
		{
			this.generator.GenerateCodeFromMember(member, writer, options);
		}

		// Token: 0x04000059 RID: 89
		private VBCodeGenerator generator;
	}
}
