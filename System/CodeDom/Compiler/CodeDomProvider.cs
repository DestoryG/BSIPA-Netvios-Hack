using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000670 RID: 1648
	[ToolboxItem(false)]
	[ComVisible(true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class CodeDomProvider : Component
	{
		// Token: 0x06003BAA RID: 15274 RVA: 0x000F6BAC File Offset: 0x000F4DAC
		[ComVisible(false)]
		public static CodeDomProvider CreateProvider(string language, IDictionary<string, string> providerOptions)
		{
			CompilerInfo compilerInfo = CodeDomProvider.GetCompilerInfo(language);
			return compilerInfo.CreateProvider(providerOptions);
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x000F6BC8 File Offset: 0x000F4DC8
		[ComVisible(false)]
		public static CodeDomProvider CreateProvider(string language)
		{
			CompilerInfo compilerInfo = CodeDomProvider.GetCompilerInfo(language);
			return compilerInfo.CreateProvider();
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x000F6BE4 File Offset: 0x000F4DE4
		[ComVisible(false)]
		public static string GetLanguageFromExtension(string extension)
		{
			CompilerInfo compilerInfoForExtensionNoThrow = CodeDomProvider.GetCompilerInfoForExtensionNoThrow(extension);
			if (compilerInfoForExtensionNoThrow == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("CodeDomProvider_NotDefined"));
			}
			return compilerInfoForExtensionNoThrow._compilerLanguages[0];
		}

		// Token: 0x06003BAD RID: 15277 RVA: 0x000F6C13 File Offset: 0x000F4E13
		[ComVisible(false)]
		public static bool IsDefinedLanguage(string language)
		{
			return CodeDomProvider.GetCompilerInfoForLanguageNoThrow(language) != null;
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x000F6C1E File Offset: 0x000F4E1E
		[ComVisible(false)]
		public static bool IsDefinedExtension(string extension)
		{
			return CodeDomProvider.GetCompilerInfoForExtensionNoThrow(extension) != null;
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x000F6C2C File Offset: 0x000F4E2C
		[ComVisible(false)]
		public static CompilerInfo GetCompilerInfo(string language)
		{
			CompilerInfo compilerInfoForLanguageNoThrow = CodeDomProvider.GetCompilerInfoForLanguageNoThrow(language);
			if (compilerInfoForLanguageNoThrow == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("CodeDomProvider_NotDefined"));
			}
			return compilerInfoForLanguageNoThrow;
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x000F6C54 File Offset: 0x000F4E54
		private static CompilerInfo GetCompilerInfoForLanguageNoThrow(string language)
		{
			if (language == null)
			{
				throw new ArgumentNullException("language");
			}
			return (CompilerInfo)CodeDomProvider.Config._compilerLanguages[language.Trim()];
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x000F6C8C File Offset: 0x000F4E8C
		private static CompilerInfo GetCompilerInfoForExtensionNoThrow(string extension)
		{
			if (extension == null)
			{
				throw new ArgumentNullException("extension");
			}
			return (CompilerInfo)CodeDomProvider.Config._compilerExtensions[extension.Trim()];
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x000F6CC4 File Offset: 0x000F4EC4
		[ComVisible(false)]
		public static CompilerInfo[] GetAllCompilerInfo()
		{
			ArrayList allCompilerInfo = CodeDomProvider.Config._allCompilerInfo;
			return (CompilerInfo[])allCompilerInfo.ToArray(typeof(CompilerInfo));
		}

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06003BB3 RID: 15283 RVA: 0x000F6CF4 File Offset: 0x000F4EF4
		private static CodeDomCompilationConfiguration Config
		{
			get
			{
				CodeDomCompilationConfiguration codeDomCompilationConfiguration = (CodeDomCompilationConfiguration)PrivilegedConfigurationManager.GetSection("system.codedom");
				if (codeDomCompilationConfiguration == null)
				{
					return CodeDomCompilationConfiguration.Default;
				}
				return codeDomCompilationConfiguration;
			}
		}

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06003BB4 RID: 15284 RVA: 0x000F6D1B File Offset: 0x000F4F1B
		public virtual string FileExtension
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06003BB5 RID: 15285 RVA: 0x000F6D22 File Offset: 0x000F4F22
		public virtual LanguageOptions LanguageOptions
		{
			get
			{
				return LanguageOptions.None;
			}
		}

		// Token: 0x06003BB6 RID: 15286
		[Obsolete("Callers should not use the ICodeGenerator interface and should instead use the methods directly on the CodeDomProvider class. Those inheriting from CodeDomProvider must still implement this interface, and should exclude this warning or also obsolete this method.")]
		public abstract ICodeGenerator CreateGenerator();

		// Token: 0x06003BB7 RID: 15287 RVA: 0x000F6D25 File Offset: 0x000F4F25
		public virtual ICodeGenerator CreateGenerator(TextWriter output)
		{
			return this.CreateGenerator();
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x000F6D2D File Offset: 0x000F4F2D
		public virtual ICodeGenerator CreateGenerator(string fileName)
		{
			return this.CreateGenerator();
		}

		// Token: 0x06003BB9 RID: 15289
		[Obsolete("Callers should not use the ICodeCompiler interface and should instead use the methods directly on the CodeDomProvider class. Those inheriting from CodeDomProvider must still implement this interface, and should exclude this warning or also obsolete this method.")]
		public abstract ICodeCompiler CreateCompiler();

		// Token: 0x06003BBA RID: 15290 RVA: 0x000F6D35 File Offset: 0x000F4F35
		[Obsolete("Callers should not use the ICodeParser interface and should instead use the methods directly on the CodeDomProvider class. Those inheriting from CodeDomProvider must still implement this interface, and should exclude this warning or also obsolete this method.")]
		public virtual ICodeParser CreateParser()
		{
			return null;
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x000F6D38 File Offset: 0x000F4F38
		public virtual TypeConverter GetConverter(Type type)
		{
			return TypeDescriptor.GetConverter(type);
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x000F6D40 File Offset: 0x000F4F40
		public virtual CompilerResults CompileAssemblyFromDom(CompilerParameters options, params CodeCompileUnit[] compilationUnits)
		{
			return this.CreateCompilerHelper().CompileAssemblyFromDomBatch(options, compilationUnits);
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x000F6D4F File Offset: 0x000F4F4F
		public virtual CompilerResults CompileAssemblyFromFile(CompilerParameters options, params string[] fileNames)
		{
			return this.CreateCompilerHelper().CompileAssemblyFromFileBatch(options, fileNames);
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x000F6D5E File Offset: 0x000F4F5E
		public virtual CompilerResults CompileAssemblyFromSource(CompilerParameters options, params string[] sources)
		{
			return this.CreateCompilerHelper().CompileAssemblyFromSourceBatch(options, sources);
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x000F6D6D File Offset: 0x000F4F6D
		public virtual bool IsValidIdentifier(string value)
		{
			return this.CreateGeneratorHelper().IsValidIdentifier(value);
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x000F6D7B File Offset: 0x000F4F7B
		public virtual string CreateEscapedIdentifier(string value)
		{
			return this.CreateGeneratorHelper().CreateEscapedIdentifier(value);
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x000F6D89 File Offset: 0x000F4F89
		public virtual string CreateValidIdentifier(string value)
		{
			return this.CreateGeneratorHelper().CreateValidIdentifier(value);
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x000F6D97 File Offset: 0x000F4F97
		public virtual string GetTypeOutput(CodeTypeReference type)
		{
			return this.CreateGeneratorHelper().GetTypeOutput(type);
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x000F6DA5 File Offset: 0x000F4FA5
		public virtual bool Supports(GeneratorSupport generatorSupport)
		{
			return this.CreateGeneratorHelper().Supports(generatorSupport);
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x000F6DB3 File Offset: 0x000F4FB3
		public virtual void GenerateCodeFromExpression(CodeExpression expression, TextWriter writer, CodeGeneratorOptions options)
		{
			this.CreateGeneratorHelper().GenerateCodeFromExpression(expression, writer, options);
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x000F6DC3 File Offset: 0x000F4FC3
		public virtual void GenerateCodeFromStatement(CodeStatement statement, TextWriter writer, CodeGeneratorOptions options)
		{
			this.CreateGeneratorHelper().GenerateCodeFromStatement(statement, writer, options);
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x000F6DD3 File Offset: 0x000F4FD3
		public virtual void GenerateCodeFromNamespace(CodeNamespace codeNamespace, TextWriter writer, CodeGeneratorOptions options)
		{
			this.CreateGeneratorHelper().GenerateCodeFromNamespace(codeNamespace, writer, options);
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x000F6DE3 File Offset: 0x000F4FE3
		public virtual void GenerateCodeFromCompileUnit(CodeCompileUnit compileUnit, TextWriter writer, CodeGeneratorOptions options)
		{
			this.CreateGeneratorHelper().GenerateCodeFromCompileUnit(compileUnit, writer, options);
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x000F6DF3 File Offset: 0x000F4FF3
		public virtual void GenerateCodeFromType(CodeTypeDeclaration codeType, TextWriter writer, CodeGeneratorOptions options)
		{
			this.CreateGeneratorHelper().GenerateCodeFromType(codeType, writer, options);
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x000F6E03 File Offset: 0x000F5003
		public virtual void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions options)
		{
			throw new NotImplementedException(SR.GetString("NotSupported_CodeDomAPI"));
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x000F6E14 File Offset: 0x000F5014
		public virtual CodeCompileUnit Parse(TextReader codeStream)
		{
			return this.CreateParserHelper().Parse(codeStream);
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x000F6E24 File Offset: 0x000F5024
		private ICodeCompiler CreateCompilerHelper()
		{
			ICodeCompiler codeCompiler = this.CreateCompiler();
			if (codeCompiler == null)
			{
				throw new NotImplementedException(SR.GetString("NotSupported_CodeDomAPI"));
			}
			return codeCompiler;
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x000F6E4C File Offset: 0x000F504C
		private ICodeGenerator CreateGeneratorHelper()
		{
			ICodeGenerator codeGenerator = this.CreateGenerator();
			if (codeGenerator == null)
			{
				throw new NotImplementedException(SR.GetString("NotSupported_CodeDomAPI"));
			}
			return codeGenerator;
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x000F6E74 File Offset: 0x000F5074
		private ICodeParser CreateParserHelper()
		{
			ICodeParser codeParser = this.CreateParser();
			if (codeParser == null)
			{
				throw new NotImplementedException(SR.GetString("NotSupported_CodeDomAPI"));
			}
			return codeParser;
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x000F6E9C File Offset: 0x000F509C
		internal static bool TryGetProbableCoreAssemblyFilePath(CompilerParameters parameters, out string coreAssemblyFilePath)
		{
			string text = null;
			char[] array = new char[] { Path.DirectorySeparatorChar };
			string text2 = Path.Combine("Reference Assemblies", "Microsoft", "Framework");
			foreach (string text3 in parameters.ReferencedAssemblies)
			{
				if (Path.GetFileName(text3).Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase))
				{
					coreAssemblyFilePath = string.Empty;
					return false;
				}
				if (text3.IndexOf(text2, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					string[] array2 = text3.Split(array, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array2.Length - 5; i++)
					{
						if (string.Equals(array2[i], "Reference Assemblies", StringComparison.OrdinalIgnoreCase) && array2[i + 4].StartsWith("v", StringComparison.OrdinalIgnoreCase))
						{
							if (text != null)
							{
								if (!string.Equals(text, Path.GetDirectoryName(text3), StringComparison.OrdinalIgnoreCase))
								{
									coreAssemblyFilePath = string.Empty;
									return false;
								}
							}
							else
							{
								text = Path.GetDirectoryName(text3);
							}
						}
					}
				}
			}
			if (text != null)
			{
				coreAssemblyFilePath = Path.Combine(text, "mscorlib.dll");
				return true;
			}
			coreAssemblyFilePath = string.Empty;
			return false;
		}
	}
}
