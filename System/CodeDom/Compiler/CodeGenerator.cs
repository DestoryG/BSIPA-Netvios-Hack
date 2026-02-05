using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000671 RID: 1649
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class CodeGenerator : ICodeGenerator
	{
		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06003BD0 RID: 15312 RVA: 0x000F6FDC File Offset: 0x000F51DC
		protected CodeTypeDeclaration CurrentClass
		{
			get
			{
				return this.currentClass;
			}
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06003BD1 RID: 15313 RVA: 0x000F6FE4 File Offset: 0x000F51E4
		protected string CurrentTypeName
		{
			get
			{
				if (this.currentClass != null)
				{
					return this.currentClass.Name;
				}
				return "<% unknown %>";
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06003BD2 RID: 15314 RVA: 0x000F6FFF File Offset: 0x000F51FF
		protected CodeTypeMember CurrentMember
		{
			get
			{
				return this.currentMember;
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x06003BD3 RID: 15315 RVA: 0x000F7007 File Offset: 0x000F5207
		protected string CurrentMemberName
		{
			get
			{
				if (this.currentMember != null)
				{
					return this.currentMember.Name;
				}
				return "<% unknown %>";
			}
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06003BD4 RID: 15316 RVA: 0x000F7022 File Offset: 0x000F5222
		protected bool IsCurrentInterface
		{
			get
			{
				return this.currentClass != null && !(this.currentClass is CodeTypeDelegate) && this.currentClass.IsInterface;
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06003BD5 RID: 15317 RVA: 0x000F7046 File Offset: 0x000F5246
		protected bool IsCurrentClass
		{
			get
			{
				return this.currentClass != null && !(this.currentClass is CodeTypeDelegate) && this.currentClass.IsClass;
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06003BD6 RID: 15318 RVA: 0x000F706A File Offset: 0x000F526A
		protected bool IsCurrentStruct
		{
			get
			{
				return this.currentClass != null && !(this.currentClass is CodeTypeDelegate) && this.currentClass.IsStruct;
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06003BD7 RID: 15319 RVA: 0x000F708E File Offset: 0x000F528E
		protected bool IsCurrentEnum
		{
			get
			{
				return this.currentClass != null && !(this.currentClass is CodeTypeDelegate) && this.currentClass.IsEnum;
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06003BD8 RID: 15320 RVA: 0x000F70B2 File Offset: 0x000F52B2
		protected bool IsCurrentDelegate
		{
			get
			{
				return this.currentClass != null && this.currentClass is CodeTypeDelegate;
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06003BD9 RID: 15321 RVA: 0x000F70CC File Offset: 0x000F52CC
		// (set) Token: 0x06003BDA RID: 15322 RVA: 0x000F70D9 File Offset: 0x000F52D9
		protected int Indent
		{
			get
			{
				return this.output.Indent;
			}
			set
			{
				this.output.Indent = value;
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06003BDB RID: 15323
		protected abstract string NullToken { get; }

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06003BDC RID: 15324 RVA: 0x000F70E7 File Offset: 0x000F52E7
		protected TextWriter Output
		{
			get
			{
				return this.output;
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06003BDD RID: 15325 RVA: 0x000F70EF File Offset: 0x000F52EF
		protected CodeGeneratorOptions Options
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x000F70F8 File Offset: 0x000F52F8
		private void GenerateType(CodeTypeDeclaration e)
		{
			this.currentClass = e;
			if (e.StartDirectives.Count > 0)
			{
				this.GenerateDirectives(e.StartDirectives);
			}
			this.GenerateCommentStatements(e.Comments);
			if (e.LinePragma != null)
			{
				this.GenerateLinePragmaStart(e.LinePragma);
			}
			this.GenerateTypeStart(e);
			if (this.Options.VerbatimOrder)
			{
				using (IEnumerator enumerator = e.Members.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						CodeTypeMember codeTypeMember = (CodeTypeMember)obj;
						this.GenerateTypeMember(codeTypeMember, e);
					}
					goto IL_00CA;
				}
			}
			this.GenerateFields(e);
			this.GenerateSnippetMembers(e);
			this.GenerateTypeConstructors(e);
			this.GenerateConstructors(e);
			this.GenerateProperties(e);
			this.GenerateEvents(e);
			this.GenerateMethods(e);
			this.GenerateNestedTypes(e);
			IL_00CA:
			this.currentClass = e;
			this.GenerateTypeEnd(e);
			if (e.LinePragma != null)
			{
				this.GenerateLinePragmaEnd(e.LinePragma);
			}
			if (e.EndDirectives.Count > 0)
			{
				this.GenerateDirectives(e.EndDirectives);
			}
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x000F721C File Offset: 0x000F541C
		protected virtual void GenerateDirectives(CodeDirectiveCollection directives)
		{
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x000F7220 File Offset: 0x000F5420
		private void GenerateTypeMember(CodeTypeMember member, CodeTypeDeclaration declaredType)
		{
			if (this.options.BlankLinesBetweenMembers)
			{
				this.Output.WriteLine();
			}
			if (member is CodeTypeDeclaration)
			{
				((ICodeGenerator)this).GenerateCodeFromType((CodeTypeDeclaration)member, this.output.InnerWriter, this.options);
				this.currentClass = declaredType;
				return;
			}
			if (member.StartDirectives.Count > 0)
			{
				this.GenerateDirectives(member.StartDirectives);
			}
			this.GenerateCommentStatements(member.Comments);
			if (member.LinePragma != null)
			{
				this.GenerateLinePragmaStart(member.LinePragma);
			}
			if (member is CodeMemberField)
			{
				this.GenerateField((CodeMemberField)member);
			}
			else if (member is CodeMemberProperty)
			{
				this.GenerateProperty((CodeMemberProperty)member, declaredType);
			}
			else if (member is CodeMemberMethod)
			{
				if (member is CodeConstructor)
				{
					this.GenerateConstructor((CodeConstructor)member, declaredType);
				}
				else if (member is CodeTypeConstructor)
				{
					this.GenerateTypeConstructor((CodeTypeConstructor)member);
				}
				else if (member is CodeEntryPointMethod)
				{
					this.GenerateEntryPointMethod((CodeEntryPointMethod)member, declaredType);
				}
				else
				{
					this.GenerateMethod((CodeMemberMethod)member, declaredType);
				}
			}
			else if (member is CodeMemberEvent)
			{
				this.GenerateEvent((CodeMemberEvent)member, declaredType);
			}
			else if (member is CodeSnippetTypeMember)
			{
				int indent = this.Indent;
				this.Indent = 0;
				this.GenerateSnippetMember((CodeSnippetTypeMember)member);
				this.Indent = indent;
				this.Output.WriteLine();
			}
			if (member.LinePragma != null)
			{
				this.GenerateLinePragmaEnd(member.LinePragma);
			}
			if (member.EndDirectives.Count > 0)
			{
				this.GenerateDirectives(member.EndDirectives);
			}
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x000F73B8 File Offset: 0x000F55B8
		private void GenerateTypeConstructors(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeTypeConstructor)
				{
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeTypeConstructor codeTypeConstructor = (CodeTypeConstructor)enumerator.Current;
					if (codeTypeConstructor.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeTypeConstructor.LinePragma);
					}
					this.GenerateTypeConstructor(codeTypeConstructor);
					if (codeTypeConstructor.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeTypeConstructor.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x000F74B0 File Offset: 0x000F56B0
		protected void GenerateNamespaces(CodeCompileUnit e)
		{
			foreach (object obj in e.Namespaces)
			{
				CodeNamespace codeNamespace = (CodeNamespace)obj;
				((ICodeGenerator)this).GenerateCodeFromNamespace(codeNamespace, this.output.InnerWriter, this.options);
			}
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x000F751C File Offset: 0x000F571C
		protected void GenerateTypes(CodeNamespace e)
		{
			foreach (object obj in e.Types)
			{
				CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)obj;
				if (this.options.BlankLinesBetweenMembers)
				{
					this.Output.WriteLine();
				}
				((ICodeGenerator)this).GenerateCodeFromType(codeTypeDeclaration, this.output.InnerWriter, this.options);
			}
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x000F75A0 File Offset: 0x000F57A0
		bool ICodeGenerator.Supports(GeneratorSupport support)
		{
			return this.Supports(support);
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x000F75AC File Offset: 0x000F57AC
		void ICodeGenerator.GenerateCodeFromType(CodeTypeDeclaration e, TextWriter w, CodeGeneratorOptions o)
		{
			bool flag = false;
			if (this.output != null && w != this.output.InnerWriter)
			{
				throw new InvalidOperationException(SR.GetString("CodeGenOutputWriter"));
			}
			if (this.output == null)
			{
				flag = true;
				this.options = ((o == null) ? new CodeGeneratorOptions() : o);
				this.output = new IndentedTextWriter(w, this.options.IndentString);
			}
			try
			{
				this.GenerateType(e);
			}
			finally
			{
				if (flag)
				{
					this.output = null;
					this.options = null;
				}
			}
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x000F7640 File Offset: 0x000F5840
		void ICodeGenerator.GenerateCodeFromExpression(CodeExpression e, TextWriter w, CodeGeneratorOptions o)
		{
			bool flag = false;
			if (this.output != null && w != this.output.InnerWriter)
			{
				throw new InvalidOperationException(SR.GetString("CodeGenOutputWriter"));
			}
			if (this.output == null)
			{
				flag = true;
				this.options = ((o == null) ? new CodeGeneratorOptions() : o);
				this.output = new IndentedTextWriter(w, this.options.IndentString);
			}
			try
			{
				this.GenerateExpression(e);
			}
			finally
			{
				if (flag)
				{
					this.output = null;
					this.options = null;
				}
			}
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x000F76D4 File Offset: 0x000F58D4
		void ICodeGenerator.GenerateCodeFromCompileUnit(CodeCompileUnit e, TextWriter w, CodeGeneratorOptions o)
		{
			bool flag = false;
			if (this.output != null && w != this.output.InnerWriter)
			{
				throw new InvalidOperationException(SR.GetString("CodeGenOutputWriter"));
			}
			if (this.output == null)
			{
				flag = true;
				this.options = ((o == null) ? new CodeGeneratorOptions() : o);
				this.output = new IndentedTextWriter(w, this.options.IndentString);
			}
			try
			{
				if (e is CodeSnippetCompileUnit)
				{
					this.GenerateSnippetCompileUnit((CodeSnippetCompileUnit)e);
				}
				else
				{
					this.GenerateCompileUnit(e);
				}
			}
			finally
			{
				if (flag)
				{
					this.output = null;
					this.options = null;
				}
			}
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x000F7780 File Offset: 0x000F5980
		void ICodeGenerator.GenerateCodeFromNamespace(CodeNamespace e, TextWriter w, CodeGeneratorOptions o)
		{
			bool flag = false;
			if (this.output != null && w != this.output.InnerWriter)
			{
				throw new InvalidOperationException(SR.GetString("CodeGenOutputWriter"));
			}
			if (this.output == null)
			{
				flag = true;
				this.options = ((o == null) ? new CodeGeneratorOptions() : o);
				this.output = new IndentedTextWriter(w, this.options.IndentString);
			}
			try
			{
				this.GenerateNamespace(e);
			}
			finally
			{
				if (flag)
				{
					this.output = null;
					this.options = null;
				}
			}
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x000F7814 File Offset: 0x000F5A14
		void ICodeGenerator.GenerateCodeFromStatement(CodeStatement e, TextWriter w, CodeGeneratorOptions o)
		{
			bool flag = false;
			if (this.output != null && w != this.output.InnerWriter)
			{
				throw new InvalidOperationException(SR.GetString("CodeGenOutputWriter"));
			}
			if (this.output == null)
			{
				flag = true;
				this.options = ((o == null) ? new CodeGeneratorOptions() : o);
				this.output = new IndentedTextWriter(w, this.options.IndentString);
			}
			try
			{
				this.GenerateStatement(e);
			}
			finally
			{
				if (flag)
				{
					this.output = null;
					this.options = null;
				}
			}
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x000F78A8 File Offset: 0x000F5AA8
		public virtual void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions options)
		{
			if (this.output != null)
			{
				throw new InvalidOperationException(SR.GetString("CodeGenReentrance"));
			}
			this.options = ((options == null) ? new CodeGeneratorOptions() : options);
			this.output = new IndentedTextWriter(writer, this.options.IndentString);
			try
			{
				CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration();
				this.currentClass = codeTypeDeclaration;
				this.GenerateTypeMember(member, codeTypeDeclaration);
			}
			finally
			{
				this.currentClass = null;
				this.output = null;
				this.options = null;
			}
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x000F7934 File Offset: 0x000F5B34
		bool ICodeGenerator.IsValidIdentifier(string value)
		{
			return this.IsValidIdentifier(value);
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x000F793D File Offset: 0x000F5B3D
		void ICodeGenerator.ValidateIdentifier(string value)
		{
			this.ValidateIdentifier(value);
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x000F7946 File Offset: 0x000F5B46
		string ICodeGenerator.CreateEscapedIdentifier(string value)
		{
			return this.CreateEscapedIdentifier(value);
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x000F794F File Offset: 0x000F5B4F
		string ICodeGenerator.CreateValidIdentifier(string value)
		{
			return this.CreateValidIdentifier(value);
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x000F7958 File Offset: 0x000F5B58
		string ICodeGenerator.GetTypeOutput(CodeTypeReference type)
		{
			return this.GetTypeOutput(type);
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x000F7964 File Offset: 0x000F5B64
		private void GenerateConstructors(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeConstructor)
				{
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeConstructor codeConstructor = (CodeConstructor)enumerator.Current;
					if (codeConstructor.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeConstructor.LinePragma);
					}
					this.GenerateConstructor(codeConstructor, e);
					if (codeConstructor.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeConstructor.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x000F7A5C File Offset: 0x000F5C5C
		private void GenerateEvents(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeMemberEvent)
				{
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeMemberEvent codeMemberEvent = (CodeMemberEvent)enumerator.Current;
					if (codeMemberEvent.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeMemberEvent.LinePragma);
					}
					this.GenerateEvent(codeMemberEvent, e);
					if (codeMemberEvent.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeMemberEvent.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x000F7B54 File Offset: 0x000F5D54
		protected void GenerateExpression(CodeExpression e)
		{
			if (e is CodeArrayCreateExpression)
			{
				this.GenerateArrayCreateExpression((CodeArrayCreateExpression)e);
				return;
			}
			if (e is CodeBaseReferenceExpression)
			{
				this.GenerateBaseReferenceExpression((CodeBaseReferenceExpression)e);
				return;
			}
			if (e is CodeBinaryOperatorExpression)
			{
				this.GenerateBinaryOperatorExpression((CodeBinaryOperatorExpression)e);
				return;
			}
			if (e is CodeCastExpression)
			{
				this.GenerateCastExpression((CodeCastExpression)e);
				return;
			}
			if (e is CodeDelegateCreateExpression)
			{
				this.GenerateDelegateCreateExpression((CodeDelegateCreateExpression)e);
				return;
			}
			if (e is CodeFieldReferenceExpression)
			{
				this.GenerateFieldReferenceExpression((CodeFieldReferenceExpression)e);
				return;
			}
			if (e is CodeArgumentReferenceExpression)
			{
				this.GenerateArgumentReferenceExpression((CodeArgumentReferenceExpression)e);
				return;
			}
			if (e is CodeVariableReferenceExpression)
			{
				this.GenerateVariableReferenceExpression((CodeVariableReferenceExpression)e);
				return;
			}
			if (e is CodeIndexerExpression)
			{
				this.GenerateIndexerExpression((CodeIndexerExpression)e);
				return;
			}
			if (e is CodeArrayIndexerExpression)
			{
				this.GenerateArrayIndexerExpression((CodeArrayIndexerExpression)e);
				return;
			}
			if (e is CodeSnippetExpression)
			{
				this.GenerateSnippetExpression((CodeSnippetExpression)e);
				return;
			}
			if (e is CodeMethodInvokeExpression)
			{
				this.GenerateMethodInvokeExpression((CodeMethodInvokeExpression)e);
				return;
			}
			if (e is CodeMethodReferenceExpression)
			{
				this.GenerateMethodReferenceExpression((CodeMethodReferenceExpression)e);
				return;
			}
			if (e is CodeEventReferenceExpression)
			{
				this.GenerateEventReferenceExpression((CodeEventReferenceExpression)e);
				return;
			}
			if (e is CodeDelegateInvokeExpression)
			{
				this.GenerateDelegateInvokeExpression((CodeDelegateInvokeExpression)e);
				return;
			}
			if (e is CodeObjectCreateExpression)
			{
				this.GenerateObjectCreateExpression((CodeObjectCreateExpression)e);
				return;
			}
			if (e is CodeParameterDeclarationExpression)
			{
				this.GenerateParameterDeclarationExpression((CodeParameterDeclarationExpression)e);
				return;
			}
			if (e is CodeDirectionExpression)
			{
				this.GenerateDirectionExpression((CodeDirectionExpression)e);
				return;
			}
			if (e is CodePrimitiveExpression)
			{
				this.GeneratePrimitiveExpression((CodePrimitiveExpression)e);
				return;
			}
			if (e is CodePropertyReferenceExpression)
			{
				this.GeneratePropertyReferenceExpression((CodePropertyReferenceExpression)e);
				return;
			}
			if (e is CodePropertySetValueReferenceExpression)
			{
				this.GeneratePropertySetValueReferenceExpression((CodePropertySetValueReferenceExpression)e);
				return;
			}
			if (e is CodeThisReferenceExpression)
			{
				this.GenerateThisReferenceExpression((CodeThisReferenceExpression)e);
				return;
			}
			if (e is CodeTypeReferenceExpression)
			{
				this.GenerateTypeReferenceExpression((CodeTypeReferenceExpression)e);
				return;
			}
			if (e is CodeTypeOfExpression)
			{
				this.GenerateTypeOfExpression((CodeTypeOfExpression)e);
				return;
			}
			if (e is CodeDefaultValueExpression)
			{
				this.GenerateDefaultValueExpression((CodeDefaultValueExpression)e);
				return;
			}
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			throw new ArgumentException(SR.GetString("InvalidElementType", new object[] { e.GetType().FullName }), "e");
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x000F7DA4 File Offset: 0x000F5FA4
		private void GenerateFields(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeMemberField)
				{
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeMemberField codeMemberField = (CodeMemberField)enumerator.Current;
					if (codeMemberField.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeMemberField.LinePragma);
					}
					this.GenerateField(codeMemberField);
					if (codeMemberField.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeMemberField.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x000F7E9C File Offset: 0x000F609C
		private void GenerateSnippetMembers(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			bool flag = false;
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeSnippetTypeMember)
				{
					flag = true;
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeSnippetTypeMember codeSnippetTypeMember = (CodeSnippetTypeMember)enumerator.Current;
					if (codeSnippetTypeMember.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeSnippetTypeMember.LinePragma);
					}
					int indent = this.Indent;
					this.Indent = 0;
					this.GenerateSnippetMember(codeSnippetTypeMember);
					this.Indent = indent;
					if (codeSnippetTypeMember.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeSnippetTypeMember.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
			if (flag)
			{
				this.Output.WriteLine();
			}
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x000F7FBC File Offset: 0x000F61BC
		protected virtual void GenerateSnippetCompileUnit(CodeSnippetCompileUnit e)
		{
			this.GenerateDirectives(e.StartDirectives);
			if (e.LinePragma != null)
			{
				this.GenerateLinePragmaStart(e.LinePragma);
			}
			this.Output.WriteLine(e.Value);
			if (e.LinePragma != null)
			{
				this.GenerateLinePragmaEnd(e.LinePragma);
			}
			if (e.EndDirectives.Count > 0)
			{
				this.GenerateDirectives(e.EndDirectives);
			}
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x000F8028 File Offset: 0x000F6228
		private void GenerateMethods(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeMemberMethod && !(enumerator.Current is CodeTypeConstructor) && !(enumerator.Current is CodeConstructor))
				{
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeMemberMethod codeMemberMethod = (CodeMemberMethod)enumerator.Current;
					if (codeMemberMethod.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeMemberMethod.LinePragma);
					}
					if (enumerator.Current is CodeEntryPointMethod)
					{
						this.GenerateEntryPointMethod((CodeEntryPointMethod)enumerator.Current, e);
					}
					else
					{
						this.GenerateMethod(codeMemberMethod, e);
					}
					if (codeMemberMethod.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeMemberMethod.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x000F8160 File Offset: 0x000F6360
		private void GenerateNestedTypes(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeTypeDeclaration)
				{
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)enumerator.Current;
					((ICodeGenerator)this).GenerateCodeFromType(codeTypeDeclaration, this.output.InnerWriter, this.options);
				}
			}
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x000F81CC File Offset: 0x000F63CC
		protected virtual void GenerateCompileUnit(CodeCompileUnit e)
		{
			this.GenerateCompileUnitStart(e);
			this.GenerateNamespaces(e);
			this.GenerateCompileUnitEnd(e);
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x000F81E3 File Offset: 0x000F63E3
		protected virtual void GenerateNamespace(CodeNamespace e)
		{
			this.GenerateCommentStatements(e.Comments);
			this.GenerateNamespaceStart(e);
			this.GenerateNamespaceImports(e);
			this.Output.WriteLine("");
			this.GenerateTypes(e);
			this.GenerateNamespaceEnd(e);
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x000F8220 File Offset: 0x000F6420
		protected void GenerateNamespaceImports(CodeNamespace e)
		{
			foreach (object obj in e.Imports)
			{
				CodeNamespaceImport codeNamespaceImport = (CodeNamespaceImport)obj;
				if (codeNamespaceImport.LinePragma != null)
				{
					this.GenerateLinePragmaStart(codeNamespaceImport.LinePragma);
				}
				this.GenerateNamespaceImport(codeNamespaceImport);
				if (codeNamespaceImport.LinePragma != null)
				{
					this.GenerateLinePragmaEnd(codeNamespaceImport.LinePragma);
				}
			}
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x000F8280 File Offset: 0x000F6480
		private void GenerateProperties(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeMemberProperty)
				{
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeMemberProperty codeMemberProperty = (CodeMemberProperty)enumerator.Current;
					if (codeMemberProperty.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeMemberProperty.LinePragma);
					}
					this.GenerateProperty(codeMemberProperty, e);
					if (codeMemberProperty.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeMemberProperty.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x000F8378 File Offset: 0x000F6578
		protected void GenerateStatement(CodeStatement e)
		{
			if (e.StartDirectives.Count > 0)
			{
				this.GenerateDirectives(e.StartDirectives);
			}
			if (e.LinePragma != null)
			{
				this.GenerateLinePragmaStart(e.LinePragma);
			}
			if (e is CodeCommentStatement)
			{
				this.GenerateCommentStatement((CodeCommentStatement)e);
			}
			else if (e is CodeMethodReturnStatement)
			{
				this.GenerateMethodReturnStatement((CodeMethodReturnStatement)e);
			}
			else if (e is CodeConditionStatement)
			{
				this.GenerateConditionStatement((CodeConditionStatement)e);
			}
			else if (e is CodeTryCatchFinallyStatement)
			{
				this.GenerateTryCatchFinallyStatement((CodeTryCatchFinallyStatement)e);
			}
			else if (e is CodeAssignStatement)
			{
				this.GenerateAssignStatement((CodeAssignStatement)e);
			}
			else if (e is CodeExpressionStatement)
			{
				this.GenerateExpressionStatement((CodeExpressionStatement)e);
			}
			else if (e is CodeIterationStatement)
			{
				this.GenerateIterationStatement((CodeIterationStatement)e);
			}
			else if (e is CodeThrowExceptionStatement)
			{
				this.GenerateThrowExceptionStatement((CodeThrowExceptionStatement)e);
			}
			else if (e is CodeSnippetStatement)
			{
				int indent = this.Indent;
				this.Indent = 0;
				this.GenerateSnippetStatement((CodeSnippetStatement)e);
				this.Indent = indent;
			}
			else if (e is CodeVariableDeclarationStatement)
			{
				this.GenerateVariableDeclarationStatement((CodeVariableDeclarationStatement)e);
			}
			else if (e is CodeAttachEventStatement)
			{
				this.GenerateAttachEventStatement((CodeAttachEventStatement)e);
			}
			else if (e is CodeRemoveEventStatement)
			{
				this.GenerateRemoveEventStatement((CodeRemoveEventStatement)e);
			}
			else if (e is CodeGotoStatement)
			{
				this.GenerateGotoStatement((CodeGotoStatement)e);
			}
			else
			{
				if (!(e is CodeLabeledStatement))
				{
					throw new ArgumentException(SR.GetString("InvalidElementType", new object[] { e.GetType().FullName }), "e");
				}
				this.GenerateLabeledStatement((CodeLabeledStatement)e);
			}
			if (e.LinePragma != null)
			{
				this.GenerateLinePragmaEnd(e.LinePragma);
			}
			if (e.EndDirectives.Count > 0)
			{
				this.GenerateDirectives(e.EndDirectives);
			}
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x000F8574 File Offset: 0x000F6774
		protected void GenerateStatements(CodeStatementCollection stms)
		{
			foreach (object obj in stms)
			{
				((ICodeGenerator)this).GenerateCodeFromStatement((CodeStatement)obj, this.output.InnerWriter, this.options);
			}
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x000F85B4 File Offset: 0x000F67B4
		protected virtual void OutputAttributeDeclarations(CodeAttributeDeclarationCollection attributes)
		{
			if (attributes.Count == 0)
			{
				return;
			}
			this.GenerateAttributeDeclarationsStart(attributes);
			bool flag = true;
			IEnumerator enumerator = attributes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					this.ContinueOnNewLine(", ");
				}
				CodeAttributeDeclaration codeAttributeDeclaration = (CodeAttributeDeclaration)enumerator.Current;
				this.Output.Write(codeAttributeDeclaration.Name);
				this.Output.Write("(");
				bool flag2 = true;
				foreach (object obj in codeAttributeDeclaration.Arguments)
				{
					CodeAttributeArgument codeAttributeArgument = (CodeAttributeArgument)obj;
					if (flag2)
					{
						flag2 = false;
					}
					else
					{
						this.Output.Write(", ");
					}
					this.OutputAttributeArgument(codeAttributeArgument);
				}
				this.Output.Write(")");
			}
			this.GenerateAttributeDeclarationsEnd(attributes);
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x000F86B0 File Offset: 0x000F68B0
		protected virtual void OutputAttributeArgument(CodeAttributeArgument arg)
		{
			if (arg.Name != null && arg.Name.Length > 0)
			{
				this.OutputIdentifier(arg.Name);
				this.Output.Write("=");
			}
			((ICodeGenerator)this).GenerateCodeFromExpression(arg.Value, this.output.InnerWriter, this.options);
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x000F870C File Offset: 0x000F690C
		protected virtual void OutputDirection(FieldDirection dir)
		{
			switch (dir)
			{
			case FieldDirection.In:
				break;
			case FieldDirection.Out:
				this.Output.Write("out ");
				return;
			case FieldDirection.Ref:
				this.Output.Write("ref ");
				break;
			default:
				return;
			}
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x000F8744 File Offset: 0x000F6944
		protected virtual void OutputFieldScopeModifier(MemberAttributes attributes)
		{
			MemberAttributes memberAttributes = attributes & MemberAttributes.VTableMask;
			if (memberAttributes == MemberAttributes.New)
			{
				this.Output.Write("new ");
			}
			switch (attributes & MemberAttributes.ScopeMask)
			{
			case MemberAttributes.Final:
			case MemberAttributes.Override:
				break;
			case MemberAttributes.Static:
				this.Output.Write("static ");
				return;
			case MemberAttributes.Const:
				this.Output.Write("const ");
				break;
			default:
				return;
			}
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x000F87B0 File Offset: 0x000F69B0
		protected virtual void OutputMemberAccessModifier(MemberAttributes attributes)
		{
			MemberAttributes memberAttributes = attributes & MemberAttributes.AccessMask;
			if (memberAttributes <= MemberAttributes.Family)
			{
				if (memberAttributes == MemberAttributes.Assembly)
				{
					this.Output.Write("internal ");
					return;
				}
				if (memberAttributes == MemberAttributes.FamilyAndAssembly)
				{
					this.Output.Write("internal ");
					return;
				}
				if (memberAttributes != MemberAttributes.Family)
				{
					return;
				}
				this.Output.Write("protected ");
				return;
			}
			else
			{
				if (memberAttributes == MemberAttributes.FamilyOrAssembly)
				{
					this.Output.Write("protected internal ");
					return;
				}
				if (memberAttributes == MemberAttributes.Private)
				{
					this.Output.Write("private ");
					return;
				}
				if (memberAttributes != MemberAttributes.Public)
				{
					return;
				}
				this.Output.Write("public ");
				return;
			}
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x000F8864 File Offset: 0x000F6A64
		protected virtual void OutputMemberScopeModifier(MemberAttributes attributes)
		{
			MemberAttributes memberAttributes = attributes & MemberAttributes.VTableMask;
			if (memberAttributes == MemberAttributes.New)
			{
				this.Output.Write("new ");
			}
			switch (attributes & MemberAttributes.ScopeMask)
			{
			case MemberAttributes.Abstract:
				this.Output.Write("abstract ");
				return;
			case MemberAttributes.Final:
				this.Output.Write("");
				return;
			case MemberAttributes.Static:
				this.Output.Write("static ");
				return;
			case MemberAttributes.Override:
				this.Output.Write("override ");
				return;
			default:
			{
				MemberAttributes memberAttributes2 = attributes & MemberAttributes.AccessMask;
				if (memberAttributes2 == MemberAttributes.Family || memberAttributes2 == MemberAttributes.Public)
				{
					this.Output.Write("virtual ");
				}
				return;
			}
			}
		}

		// Token: 0x06003C04 RID: 15364
		protected abstract void OutputType(CodeTypeReference typeRef);

		// Token: 0x06003C05 RID: 15365 RVA: 0x000F891C File Offset: 0x000F6B1C
		protected virtual void OutputTypeAttributes(TypeAttributes attributes, bool isStruct, bool isEnum)
		{
			TypeAttributes typeAttributes = attributes & TypeAttributes.VisibilityMask;
			if (typeAttributes - TypeAttributes.Public > 1)
			{
				if (typeAttributes == TypeAttributes.NestedPrivate)
				{
					this.Output.Write("private ");
				}
			}
			else
			{
				this.Output.Write("public ");
			}
			if (isStruct)
			{
				this.Output.Write("struct ");
				return;
			}
			if (isEnum)
			{
				this.Output.Write("enum ");
				return;
			}
			TypeAttributes typeAttributes2 = attributes & TypeAttributes.ClassSemanticsMask;
			if (typeAttributes2 == TypeAttributes.NotPublic)
			{
				if ((attributes & TypeAttributes.Sealed) == TypeAttributes.Sealed)
				{
					this.Output.Write("sealed ");
				}
				if ((attributes & TypeAttributes.Abstract) == TypeAttributes.Abstract)
				{
					this.Output.Write("abstract ");
				}
				this.Output.Write("class ");
				return;
			}
			if (typeAttributes2 != TypeAttributes.ClassSemanticsMask)
			{
				return;
			}
			this.Output.Write("interface ");
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x000F89EE File Offset: 0x000F6BEE
		protected virtual void OutputTypeNamePair(CodeTypeReference typeRef, string name)
		{
			this.OutputType(typeRef);
			this.Output.Write(" ");
			this.OutputIdentifier(name);
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x000F8A0E File Offset: 0x000F6C0E
		protected virtual void OutputIdentifier(string ident)
		{
			this.Output.Write(ident);
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x000F8A1C File Offset: 0x000F6C1C
		protected virtual void OutputExpressionList(CodeExpressionCollection expressions)
		{
			this.OutputExpressionList(expressions, false);
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x000F8A28 File Offset: 0x000F6C28
		protected virtual void OutputExpressionList(CodeExpressionCollection expressions, bool newlineBetweenItems)
		{
			bool flag = true;
			IEnumerator enumerator = expressions.GetEnumerator();
			int num = this.Indent;
			this.Indent = num + 1;
			while (enumerator.MoveNext())
			{
				if (flag)
				{
					flag = false;
				}
				else if (newlineBetweenItems)
				{
					this.ContinueOnNewLine(",");
				}
				else
				{
					this.Output.Write(", ");
				}
				((ICodeGenerator)this).GenerateCodeFromExpression((CodeExpression)enumerator.Current, this.output.InnerWriter, this.options);
			}
			num = this.Indent;
			this.Indent = num - 1;
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x000F8AB4 File Offset: 0x000F6CB4
		protected virtual void OutputOperator(CodeBinaryOperatorType op)
		{
			switch (op)
			{
			case CodeBinaryOperatorType.Add:
				this.Output.Write("+");
				return;
			case CodeBinaryOperatorType.Subtract:
				this.Output.Write("-");
				return;
			case CodeBinaryOperatorType.Multiply:
				this.Output.Write("*");
				return;
			case CodeBinaryOperatorType.Divide:
				this.Output.Write("/");
				return;
			case CodeBinaryOperatorType.Modulus:
				this.Output.Write("%");
				return;
			case CodeBinaryOperatorType.Assign:
				this.Output.Write("=");
				return;
			case CodeBinaryOperatorType.IdentityInequality:
				this.Output.Write("!=");
				return;
			case CodeBinaryOperatorType.IdentityEquality:
				this.Output.Write("==");
				return;
			case CodeBinaryOperatorType.ValueEquality:
				this.Output.Write("==");
				return;
			case CodeBinaryOperatorType.BitwiseOr:
				this.Output.Write("|");
				return;
			case CodeBinaryOperatorType.BitwiseAnd:
				this.Output.Write("&");
				return;
			case CodeBinaryOperatorType.BooleanOr:
				this.Output.Write("||");
				return;
			case CodeBinaryOperatorType.BooleanAnd:
				this.Output.Write("&&");
				return;
			case CodeBinaryOperatorType.LessThan:
				this.Output.Write("<");
				return;
			case CodeBinaryOperatorType.LessThanOrEqual:
				this.Output.Write("<=");
				return;
			case CodeBinaryOperatorType.GreaterThan:
				this.Output.Write(">");
				return;
			case CodeBinaryOperatorType.GreaterThanOrEqual:
				this.Output.Write(">=");
				return;
			default:
				return;
			}
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x000F8C2C File Offset: 0x000F6E2C
		protected virtual void OutputParameters(CodeParameterDeclarationExpressionCollection parameters)
		{
			bool flag = true;
			bool flag2 = parameters.Count > 15;
			if (flag2)
			{
				this.Indent += 3;
			}
			foreach (object obj in parameters)
			{
				CodeParameterDeclarationExpression codeParameterDeclarationExpression = (CodeParameterDeclarationExpression)obj;
				if (flag)
				{
					flag = false;
				}
				else
				{
					this.Output.Write(", ");
				}
				if (flag2)
				{
					this.ContinueOnNewLine("");
				}
				this.GenerateExpression(codeParameterDeclarationExpression);
			}
			if (flag2)
			{
				this.Indent -= 3;
			}
		}

		// Token: 0x06003C0C RID: 15372
		protected abstract void GenerateArrayCreateExpression(CodeArrayCreateExpression e);

		// Token: 0x06003C0D RID: 15373
		protected abstract void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e);

		// Token: 0x06003C0E RID: 15374 RVA: 0x000F8CB4 File Offset: 0x000F6EB4
		protected virtual void GenerateBinaryOperatorExpression(CodeBinaryOperatorExpression e)
		{
			bool flag = false;
			this.Output.Write("(");
			this.GenerateExpression(e.Left);
			this.Output.Write(" ");
			if (e.Left is CodeBinaryOperatorExpression || e.Right is CodeBinaryOperatorExpression)
			{
				if (!this.inNestedBinary)
				{
					flag = true;
					this.inNestedBinary = true;
					this.Indent += 3;
				}
				this.ContinueOnNewLine("");
			}
			this.OutputOperator(e.Operator);
			this.Output.Write(" ");
			this.GenerateExpression(e.Right);
			this.Output.Write(")");
			if (flag)
			{
				this.Indent -= 3;
				this.inNestedBinary = false;
			}
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x000F8D83 File Offset: 0x000F6F83
		protected virtual void ContinueOnNewLine(string st)
		{
			this.Output.WriteLine(st);
		}

		// Token: 0x06003C10 RID: 15376
		protected abstract void GenerateCastExpression(CodeCastExpression e);

		// Token: 0x06003C11 RID: 15377
		protected abstract void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e);

		// Token: 0x06003C12 RID: 15378
		protected abstract void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e);

		// Token: 0x06003C13 RID: 15379
		protected abstract void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e);

		// Token: 0x06003C14 RID: 15380
		protected abstract void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e);

		// Token: 0x06003C15 RID: 15381
		protected abstract void GenerateIndexerExpression(CodeIndexerExpression e);

		// Token: 0x06003C16 RID: 15382
		protected abstract void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e);

		// Token: 0x06003C17 RID: 15383
		protected abstract void GenerateSnippetExpression(CodeSnippetExpression e);

		// Token: 0x06003C18 RID: 15384
		protected abstract void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e);

		// Token: 0x06003C19 RID: 15385
		protected abstract void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e);

		// Token: 0x06003C1A RID: 15386
		protected abstract void GenerateEventReferenceExpression(CodeEventReferenceExpression e);

		// Token: 0x06003C1B RID: 15387
		protected abstract void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e);

		// Token: 0x06003C1C RID: 15388
		protected abstract void GenerateObjectCreateExpression(CodeObjectCreateExpression e);

		// Token: 0x06003C1D RID: 15389 RVA: 0x000F8D94 File Offset: 0x000F6F94
		protected virtual void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
		{
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributeDeclarations(e.CustomAttributes);
				this.Output.Write(" ");
			}
			this.OutputDirection(e.Direction);
			this.OutputTypeNamePair(e.Type, e.Name);
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x000F8DE9 File Offset: 0x000F6FE9
		protected virtual void GenerateDirectionExpression(CodeDirectionExpression e)
		{
			this.OutputDirection(e.Direction);
			this.GenerateExpression(e.Expression);
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x000F8E04 File Offset: 0x000F7004
		protected virtual void GeneratePrimitiveExpression(CodePrimitiveExpression e)
		{
			if (e.Value == null)
			{
				this.Output.Write(this.NullToken);
				return;
			}
			if (e.Value is string)
			{
				this.Output.Write(this.QuoteSnippetString((string)e.Value));
				return;
			}
			if (e.Value is char)
			{
				this.Output.Write("'" + e.Value.ToString() + "'");
				return;
			}
			if (e.Value is byte)
			{
				this.Output.Write(((byte)e.Value).ToString(CultureInfo.InvariantCulture));
				return;
			}
			if (e.Value is short)
			{
				this.Output.Write(((short)e.Value).ToString(CultureInfo.InvariantCulture));
				return;
			}
			if (e.Value is int)
			{
				this.Output.Write(((int)e.Value).ToString(CultureInfo.InvariantCulture));
				return;
			}
			if (e.Value is long)
			{
				this.Output.Write(((long)e.Value).ToString(CultureInfo.InvariantCulture));
				return;
			}
			if (e.Value is float)
			{
				this.GenerateSingleFloatValue((float)e.Value);
				return;
			}
			if (e.Value is double)
			{
				this.GenerateDoubleValue((double)e.Value);
				return;
			}
			if (e.Value is decimal)
			{
				this.GenerateDecimalValue((decimal)e.Value);
				return;
			}
			if (!(e.Value is bool))
			{
				throw new ArgumentException(SR.GetString("InvalidPrimitiveType", new object[] { e.Value.GetType().ToString() }));
			}
			if ((bool)e.Value)
			{
				this.Output.Write("true");
				return;
			}
			this.Output.Write("false");
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x000F900D File Offset: 0x000F720D
		protected virtual void GenerateSingleFloatValue(float s)
		{
			this.Output.Write(s.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x000F902B File Offset: 0x000F722B
		protected virtual void GenerateDoubleValue(double d)
		{
			this.Output.Write(d.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x000F9049 File Offset: 0x000F7249
		protected virtual void GenerateDecimalValue(decimal d)
		{
			this.Output.Write(d.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x000F9062 File Offset: 0x000F7262
		protected virtual void GenerateDefaultValueExpression(CodeDefaultValueExpression e)
		{
		}

		// Token: 0x06003C24 RID: 15396
		protected abstract void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e);

		// Token: 0x06003C25 RID: 15397
		protected abstract void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e);

		// Token: 0x06003C26 RID: 15398
		protected abstract void GenerateThisReferenceExpression(CodeThisReferenceExpression e);

		// Token: 0x06003C27 RID: 15399 RVA: 0x000F9064 File Offset: 0x000F7264
		protected virtual void GenerateTypeReferenceExpression(CodeTypeReferenceExpression e)
		{
			this.OutputType(e.Type);
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x000F9072 File Offset: 0x000F7272
		protected virtual void GenerateTypeOfExpression(CodeTypeOfExpression e)
		{
			this.Output.Write("typeof(");
			this.OutputType(e.Type);
			this.Output.Write(")");
		}

		// Token: 0x06003C29 RID: 15401
		protected abstract void GenerateExpressionStatement(CodeExpressionStatement e);

		// Token: 0x06003C2A RID: 15402
		protected abstract void GenerateIterationStatement(CodeIterationStatement e);

		// Token: 0x06003C2B RID: 15403
		protected abstract void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e);

		// Token: 0x06003C2C RID: 15404 RVA: 0x000F90A0 File Offset: 0x000F72A0
		protected virtual void GenerateCommentStatement(CodeCommentStatement e)
		{
			if (e.Comment == null)
			{
				throw new ArgumentException(SR.GetString("Argument_NullComment", new object[] { "e" }), "e");
			}
			this.GenerateComment(e.Comment);
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x000F90DC File Offset: 0x000F72DC
		protected virtual void GenerateCommentStatements(CodeCommentStatementCollection e)
		{
			foreach (object obj in e)
			{
				CodeCommentStatement codeCommentStatement = (CodeCommentStatement)obj;
				this.GenerateCommentStatement(codeCommentStatement);
			}
		}

		// Token: 0x06003C2E RID: 15406
		protected abstract void GenerateComment(CodeComment e);

		// Token: 0x06003C2F RID: 15407
		protected abstract void GenerateMethodReturnStatement(CodeMethodReturnStatement e);

		// Token: 0x06003C30 RID: 15408
		protected abstract void GenerateConditionStatement(CodeConditionStatement e);

		// Token: 0x06003C31 RID: 15409
		protected abstract void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e);

		// Token: 0x06003C32 RID: 15410
		protected abstract void GenerateAssignStatement(CodeAssignStatement e);

		// Token: 0x06003C33 RID: 15411
		protected abstract void GenerateAttachEventStatement(CodeAttachEventStatement e);

		// Token: 0x06003C34 RID: 15412
		protected abstract void GenerateRemoveEventStatement(CodeRemoveEventStatement e);

		// Token: 0x06003C35 RID: 15413
		protected abstract void GenerateGotoStatement(CodeGotoStatement e);

		// Token: 0x06003C36 RID: 15414
		protected abstract void GenerateLabeledStatement(CodeLabeledStatement e);

		// Token: 0x06003C37 RID: 15415 RVA: 0x000F9130 File Offset: 0x000F7330
		protected virtual void GenerateSnippetStatement(CodeSnippetStatement e)
		{
			this.Output.WriteLine(e.Value);
		}

		// Token: 0x06003C38 RID: 15416
		protected abstract void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e);

		// Token: 0x06003C39 RID: 15417
		protected abstract void GenerateLinePragmaStart(CodeLinePragma e);

		// Token: 0x06003C3A RID: 15418
		protected abstract void GenerateLinePragmaEnd(CodeLinePragma e);

		// Token: 0x06003C3B RID: 15419
		protected abstract void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c);

		// Token: 0x06003C3C RID: 15420
		protected abstract void GenerateField(CodeMemberField e);

		// Token: 0x06003C3D RID: 15421
		protected abstract void GenerateSnippetMember(CodeSnippetTypeMember e);

		// Token: 0x06003C3E RID: 15422
		protected abstract void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c);

		// Token: 0x06003C3F RID: 15423
		protected abstract void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c);

		// Token: 0x06003C40 RID: 15424
		protected abstract void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c);

		// Token: 0x06003C41 RID: 15425
		protected abstract void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c);

		// Token: 0x06003C42 RID: 15426
		protected abstract void GenerateTypeConstructor(CodeTypeConstructor e);

		// Token: 0x06003C43 RID: 15427
		protected abstract void GenerateTypeStart(CodeTypeDeclaration e);

		// Token: 0x06003C44 RID: 15428
		protected abstract void GenerateTypeEnd(CodeTypeDeclaration e);

		// Token: 0x06003C45 RID: 15429 RVA: 0x000F9143 File Offset: 0x000F7343
		protected virtual void GenerateCompileUnitStart(CodeCompileUnit e)
		{
			if (e.StartDirectives.Count > 0)
			{
				this.GenerateDirectives(e.StartDirectives);
			}
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x000F915F File Offset: 0x000F735F
		protected virtual void GenerateCompileUnitEnd(CodeCompileUnit e)
		{
			if (e.EndDirectives.Count > 0)
			{
				this.GenerateDirectives(e.EndDirectives);
			}
		}

		// Token: 0x06003C47 RID: 15431
		protected abstract void GenerateNamespaceStart(CodeNamespace e);

		// Token: 0x06003C48 RID: 15432
		protected abstract void GenerateNamespaceEnd(CodeNamespace e);

		// Token: 0x06003C49 RID: 15433
		protected abstract void GenerateNamespaceImport(CodeNamespaceImport e);

		// Token: 0x06003C4A RID: 15434
		protected abstract void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes);

		// Token: 0x06003C4B RID: 15435
		protected abstract void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes);

		// Token: 0x06003C4C RID: 15436
		protected abstract bool Supports(GeneratorSupport support);

		// Token: 0x06003C4D RID: 15437
		protected abstract bool IsValidIdentifier(string value);

		// Token: 0x06003C4E RID: 15438 RVA: 0x000F917B File Offset: 0x000F737B
		protected virtual void ValidateIdentifier(string value)
		{
			if (!this.IsValidIdentifier(value))
			{
				throw new ArgumentException(SR.GetString("InvalidIdentifier", new object[] { value }));
			}
		}

		// Token: 0x06003C4F RID: 15439
		protected abstract string CreateEscapedIdentifier(string value);

		// Token: 0x06003C50 RID: 15440
		protected abstract string CreateValidIdentifier(string value);

		// Token: 0x06003C51 RID: 15441
		protected abstract string GetTypeOutput(CodeTypeReference value);

		// Token: 0x06003C52 RID: 15442
		protected abstract string QuoteSnippetString(string value);

		// Token: 0x06003C53 RID: 15443 RVA: 0x000F91A0 File Offset: 0x000F73A0
		public static bool IsValidLanguageIndependentIdentifier(string value)
		{
			return CodeGenerator.IsValidTypeNameOrIdentifier(value, false);
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x000F91A9 File Offset: 0x000F73A9
		internal static bool IsValidLanguageIndependentTypeName(string value)
		{
			return CodeGenerator.IsValidTypeNameOrIdentifier(value, true);
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x000F91B4 File Offset: 0x000F73B4
		private static bool IsValidTypeNameOrIdentifier(string value, bool isTypeName)
		{
			bool flag = true;
			if (value.Length == 0)
			{
				return false;
			}
			int i = 0;
			while (i < value.Length)
			{
				char c = value[i];
				switch (char.GetUnicodeCategory(c))
				{
				case UnicodeCategory.UppercaseLetter:
				case UnicodeCategory.LowercaseLetter:
				case UnicodeCategory.TitlecaseLetter:
				case UnicodeCategory.ModifierLetter:
				case UnicodeCategory.OtherLetter:
				case UnicodeCategory.LetterNumber:
					flag = false;
					break;
				case UnicodeCategory.NonSpacingMark:
				case UnicodeCategory.SpacingCombiningMark:
				case UnicodeCategory.DecimalDigitNumber:
				case UnicodeCategory.ConnectorPunctuation:
					if (flag && c != '_')
					{
						return false;
					}
					flag = false;
					break;
				case UnicodeCategory.EnclosingMark:
				case UnicodeCategory.OtherNumber:
				case UnicodeCategory.SpaceSeparator:
				case UnicodeCategory.LineSeparator:
				case UnicodeCategory.ParagraphSeparator:
				case UnicodeCategory.Control:
				case UnicodeCategory.Format:
				case UnicodeCategory.Surrogate:
				case UnicodeCategory.PrivateUse:
					goto IL_0088;
				default:
					goto IL_0088;
				}
				IL_0097:
				i++;
				continue;
				IL_0088:
				if (!isTypeName || !CodeGenerator.IsSpecialTypeChar(c, ref flag))
				{
					return false;
				}
				goto IL_0097;
			}
			return true;
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x000F926C File Offset: 0x000F746C
		private static bool IsSpecialTypeChar(char ch, ref bool nextMustBeStartChar)
		{
			if (ch <= '>')
			{
				switch (ch)
				{
				case '$':
				case '&':
				case '*':
				case '+':
				case ',':
				case '-':
				case '.':
					break;
				case '%':
				case '\'':
				case '(':
				case ')':
					return false;
				default:
					switch (ch)
					{
					case ':':
					case '<':
					case '>':
						break;
					case ';':
					case '=':
						return false;
					default:
						return false;
					}
					break;
				}
			}
			else if (ch != '[' && ch != ']')
			{
				if (ch != '`')
				{
					return false;
				}
				return true;
			}
			nextMustBeStartChar = true;
			return true;
		}

		// Token: 0x06003C57 RID: 15447 RVA: 0x000F92EC File Offset: 0x000F74EC
		public static void ValidateIdentifiers(CodeObject e)
		{
			CodeValidator codeValidator = new CodeValidator();
			codeValidator.ValidateIdentifiers(e);
		}

		// Token: 0x04002C60 RID: 11360
		private const int ParameterMultilineThreshold = 15;

		// Token: 0x04002C61 RID: 11361
		private IndentedTextWriter output;

		// Token: 0x04002C62 RID: 11362
		private CodeGeneratorOptions options;

		// Token: 0x04002C63 RID: 11363
		private CodeTypeDeclaration currentClass;

		// Token: 0x04002C64 RID: 11364
		private CodeTypeMember currentMember;

		// Token: 0x04002C65 RID: 11365
		private bool inNestedBinary;
	}
}
