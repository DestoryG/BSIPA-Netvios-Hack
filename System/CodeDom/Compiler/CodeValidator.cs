using System;
using System.IO;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000674 RID: 1652
	internal class CodeValidator
	{
		// Token: 0x06003C68 RID: 15464 RVA: 0x000F9494 File Offset: 0x000F7694
		internal void ValidateIdentifiers(CodeObject e)
		{
			if (e is CodeCompileUnit)
			{
				this.ValidateCodeCompileUnit((CodeCompileUnit)e);
				return;
			}
			if (e is CodeComment)
			{
				this.ValidateComment((CodeComment)e);
				return;
			}
			if (e is CodeExpression)
			{
				this.ValidateExpression((CodeExpression)e);
				return;
			}
			if (e is CodeNamespace)
			{
				this.ValidateNamespace((CodeNamespace)e);
				return;
			}
			if (e is CodeNamespaceImport)
			{
				CodeValidator.ValidateNamespaceImport((CodeNamespaceImport)e);
				return;
			}
			if (e is CodeStatement)
			{
				this.ValidateStatement((CodeStatement)e);
				return;
			}
			if (e is CodeTypeMember)
			{
				this.ValidateTypeMember((CodeTypeMember)e);
				return;
			}
			if (e is CodeTypeReference)
			{
				CodeValidator.ValidateTypeReference((CodeTypeReference)e);
				return;
			}
			if (e is CodeDirective)
			{
				CodeValidator.ValidateCodeDirective((CodeDirective)e);
				return;
			}
			throw new ArgumentException(SR.GetString("InvalidElementType", new object[] { e.GetType().FullName }), "e");
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x000F9584 File Offset: 0x000F7784
		private void ValidateTypeMember(CodeTypeMember e)
		{
			this.ValidateCommentStatements(e.Comments);
			CodeValidator.ValidateCodeDirectives(e.StartDirectives);
			CodeValidator.ValidateCodeDirectives(e.EndDirectives);
			if (e.LinePragma != null)
			{
				this.ValidateLinePragmaStart(e.LinePragma);
			}
			if (e is CodeMemberEvent)
			{
				this.ValidateEvent((CodeMemberEvent)e);
				return;
			}
			if (e is CodeMemberField)
			{
				this.ValidateField((CodeMemberField)e);
				return;
			}
			if (e is CodeMemberMethod)
			{
				this.ValidateMemberMethod((CodeMemberMethod)e);
				return;
			}
			if (e is CodeMemberProperty)
			{
				this.ValidateProperty((CodeMemberProperty)e);
				return;
			}
			if (e is CodeSnippetTypeMember)
			{
				this.ValidateSnippetMember((CodeSnippetTypeMember)e);
				return;
			}
			if (e is CodeTypeDeclaration)
			{
				this.ValidateTypeDeclaration((CodeTypeDeclaration)e);
				return;
			}
			throw new ArgumentException(SR.GetString("InvalidElementType", new object[] { e.GetType().FullName }), "e");
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x000F9670 File Offset: 0x000F7870
		private void ValidateCodeCompileUnit(CodeCompileUnit e)
		{
			CodeValidator.ValidateCodeDirectives(e.StartDirectives);
			CodeValidator.ValidateCodeDirectives(e.EndDirectives);
			if (e is CodeSnippetCompileUnit)
			{
				this.ValidateSnippetCompileUnit((CodeSnippetCompileUnit)e);
				return;
			}
			this.ValidateCompileUnitStart(e);
			this.ValidateNamespaces(e);
			this.ValidateCompileUnitEnd(e);
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x000F96BD File Offset: 0x000F78BD
		private void ValidateSnippetCompileUnit(CodeSnippetCompileUnit e)
		{
			if (e.LinePragma != null)
			{
				this.ValidateLinePragmaStart(e.LinePragma);
			}
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x000F96D3 File Offset: 0x000F78D3
		private void ValidateCompileUnitStart(CodeCompileUnit e)
		{
			if (e.AssemblyCustomAttributes.Count > 0)
			{
				this.ValidateAttributes(e.AssemblyCustomAttributes);
			}
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x000F96EF File Offset: 0x000F78EF
		private void ValidateCompileUnitEnd(CodeCompileUnit e)
		{
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x000F96F4 File Offset: 0x000F78F4
		private void ValidateNamespaces(CodeCompileUnit e)
		{
			foreach (object obj in e.Namespaces)
			{
				CodeNamespace codeNamespace = (CodeNamespace)obj;
				this.ValidateNamespace(codeNamespace);
			}
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x000F9750 File Offset: 0x000F7950
		private void ValidateNamespace(CodeNamespace e)
		{
			this.ValidateCommentStatements(e.Comments);
			CodeValidator.ValidateNamespaceStart(e);
			this.ValidateNamespaceImports(e);
			this.ValidateTypes(e);
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x000F9772 File Offset: 0x000F7972
		private static void ValidateNamespaceStart(CodeNamespace e)
		{
			if (e.Name != null && e.Name.Length > 0)
			{
				CodeValidator.ValidateTypeName(e, "Name", e.Name);
			}
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x000F979C File Offset: 0x000F799C
		private void ValidateNamespaceImports(CodeNamespace e)
		{
			foreach (object obj in e.Imports)
			{
				CodeNamespaceImport codeNamespaceImport = (CodeNamespaceImport)obj;
				if (codeNamespaceImport.LinePragma != null)
				{
					this.ValidateLinePragmaStart(codeNamespaceImport.LinePragma);
				}
				CodeValidator.ValidateNamespaceImport(codeNamespaceImport);
			}
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x000F97E5 File Offset: 0x000F79E5
		private static void ValidateNamespaceImport(CodeNamespaceImport e)
		{
			CodeValidator.ValidateTypeName(e, "Namespace", e.Namespace);
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x000F97F8 File Offset: 0x000F79F8
		private void ValidateAttributes(CodeAttributeDeclarationCollection attributes)
		{
			if (attributes.Count == 0)
			{
				return;
			}
			foreach (object obj in attributes)
			{
				CodeAttributeDeclaration codeAttributeDeclaration = (CodeAttributeDeclaration)obj;
				CodeValidator.ValidateTypeName(codeAttributeDeclaration, "Name", codeAttributeDeclaration.Name);
				CodeValidator.ValidateTypeReference(codeAttributeDeclaration.AttributeType);
				foreach (object obj2 in codeAttributeDeclaration.Arguments)
				{
					CodeAttributeArgument codeAttributeArgument = (CodeAttributeArgument)obj2;
					this.ValidateAttributeArgument(codeAttributeArgument);
				}
			}
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x000F9898 File Offset: 0x000F7A98
		private void ValidateAttributeArgument(CodeAttributeArgument arg)
		{
			if (arg.Name != null && arg.Name.Length > 0)
			{
				CodeValidator.ValidateIdentifier(arg, "Name", arg.Name);
			}
			this.ValidateExpression(arg.Value);
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x000F98D0 File Offset: 0x000F7AD0
		private void ValidateTypes(CodeNamespace e)
		{
			foreach (object obj in e.Types)
			{
				CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)obj;
				this.ValidateTypeDeclaration(codeTypeDeclaration);
			}
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x000F992C File Offset: 0x000F7B2C
		private void ValidateTypeDeclaration(CodeTypeDeclaration e)
		{
			CodeTypeDeclaration codeTypeDeclaration = this.currentClass;
			this.currentClass = e;
			this.ValidateTypeStart(e);
			this.ValidateTypeParameters(e.TypeParameters);
			this.ValidateTypeMembers(e);
			CodeValidator.ValidateTypeReferences(e.BaseTypes);
			this.currentClass = codeTypeDeclaration;
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x000F9974 File Offset: 0x000F7B74
		private void ValidateTypeMembers(CodeTypeDeclaration e)
		{
			foreach (object obj in e.Members)
			{
				CodeTypeMember codeTypeMember = (CodeTypeMember)obj;
				this.ValidateTypeMember(codeTypeMember);
			}
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x000F99D0 File Offset: 0x000F7BD0
		private void ValidateTypeParameters(CodeTypeParameterCollection parameters)
		{
			for (int i = 0; i < parameters.Count; i++)
			{
				this.ValidateTypeParameter(parameters[i]);
			}
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x000F99FB File Offset: 0x000F7BFB
		private void ValidateTypeParameter(CodeTypeParameter e)
		{
			CodeValidator.ValidateIdentifier(e, "Name", e.Name);
			CodeValidator.ValidateTypeReferences(e.Constraints);
			this.ValidateAttributes(e.CustomAttributes);
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x000F9A28 File Offset: 0x000F7C28
		private void ValidateField(CodeMemberField e)
		{
			if (e.CustomAttributes.Count > 0)
			{
				this.ValidateAttributes(e.CustomAttributes);
			}
			CodeValidator.ValidateIdentifier(e, "Name", e.Name);
			if (!this.IsCurrentEnum)
			{
				CodeValidator.ValidateTypeReference(e.Type);
			}
			if (e.InitExpression != null)
			{
				this.ValidateExpression(e.InitExpression);
			}
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x000F9A88 File Offset: 0x000F7C88
		private void ValidateConstructor(CodeConstructor e)
		{
			if (e.CustomAttributes.Count > 0)
			{
				this.ValidateAttributes(e.CustomAttributes);
			}
			this.ValidateParameters(e.Parameters);
			CodeExpressionCollection baseConstructorArgs = e.BaseConstructorArgs;
			CodeExpressionCollection chainedConstructorArgs = e.ChainedConstructorArgs;
			if (baseConstructorArgs.Count > 0)
			{
				this.ValidateExpressionList(baseConstructorArgs);
			}
			if (chainedConstructorArgs.Count > 0)
			{
				this.ValidateExpressionList(chainedConstructorArgs);
			}
			this.ValidateStatements(e.Statements);
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x000F9AF8 File Offset: 0x000F7CF8
		private void ValidateProperty(CodeMemberProperty e)
		{
			if (e.CustomAttributes.Count > 0)
			{
				this.ValidateAttributes(e.CustomAttributes);
			}
			CodeValidator.ValidateTypeReference(e.Type);
			CodeValidator.ValidateTypeReferences(e.ImplementationTypes);
			if (e.PrivateImplementationType != null && !this.IsCurrentInterface)
			{
				CodeValidator.ValidateTypeReference(e.PrivateImplementationType);
			}
			if (e.Parameters.Count > 0 && string.Compare(e.Name, "Item", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.ValidateParameters(e.Parameters);
			}
			else
			{
				CodeValidator.ValidateIdentifier(e, "Name", e.Name);
			}
			if (e.HasGet && !this.IsCurrentInterface && (e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Abstract)
			{
				this.ValidateStatements(e.GetStatements);
			}
			if (e.HasSet && !this.IsCurrentInterface && (e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Abstract)
			{
				this.ValidateStatements(e.SetStatements);
			}
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x000F9BE0 File Offset: 0x000F7DE0
		private void ValidateMemberMethod(CodeMemberMethod e)
		{
			this.ValidateCommentStatements(e.Comments);
			if (e.LinePragma != null)
			{
				this.ValidateLinePragmaStart(e.LinePragma);
			}
			this.ValidateTypeParameters(e.TypeParameters);
			CodeValidator.ValidateTypeReferences(e.ImplementationTypes);
			if (e is CodeEntryPointMethod)
			{
				this.ValidateStatements(((CodeEntryPointMethod)e).Statements);
				return;
			}
			if (e is CodeConstructor)
			{
				this.ValidateConstructor((CodeConstructor)e);
				return;
			}
			if (e is CodeTypeConstructor)
			{
				this.ValidateTypeConstructor((CodeTypeConstructor)e);
				return;
			}
			this.ValidateMethod(e);
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x000F9C6F File Offset: 0x000F7E6F
		private void ValidateTypeConstructor(CodeTypeConstructor e)
		{
			this.ValidateStatements(e.Statements);
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x000F9C80 File Offset: 0x000F7E80
		private void ValidateMethod(CodeMemberMethod e)
		{
			if (e.CustomAttributes.Count > 0)
			{
				this.ValidateAttributes(e.CustomAttributes);
			}
			if (e.ReturnTypeCustomAttributes.Count > 0)
			{
				this.ValidateAttributes(e.ReturnTypeCustomAttributes);
			}
			CodeValidator.ValidateTypeReference(e.ReturnType);
			if (e.PrivateImplementationType != null)
			{
				CodeValidator.ValidateTypeReference(e.PrivateImplementationType);
			}
			CodeValidator.ValidateIdentifier(e, "Name", e.Name);
			this.ValidateParameters(e.Parameters);
			if (!this.IsCurrentInterface && (e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Abstract)
			{
				this.ValidateStatements(e.Statements);
			}
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x000F9D1C File Offset: 0x000F7F1C
		private void ValidateSnippetMember(CodeSnippetTypeMember e)
		{
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x000F9D20 File Offset: 0x000F7F20
		private void ValidateTypeStart(CodeTypeDeclaration e)
		{
			this.ValidateCommentStatements(e.Comments);
			if (e.CustomAttributes.Count > 0)
			{
				this.ValidateAttributes(e.CustomAttributes);
			}
			CodeValidator.ValidateIdentifier(e, "Name", e.Name);
			if (this.IsCurrentDelegate)
			{
				CodeTypeDelegate codeTypeDelegate = (CodeTypeDelegate)e;
				CodeValidator.ValidateTypeReference(codeTypeDelegate.ReturnType);
				this.ValidateParameters(codeTypeDelegate.Parameters);
				return;
			}
			foreach (object obj in e.BaseTypes)
			{
				CodeTypeReference codeTypeReference = (CodeTypeReference)obj;
				CodeValidator.ValidateTypeReference(codeTypeReference);
			}
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x000F9DD8 File Offset: 0x000F7FD8
		private void ValidateCommentStatements(CodeCommentStatementCollection e)
		{
			foreach (object obj in e)
			{
				CodeCommentStatement codeCommentStatement = (CodeCommentStatement)obj;
				this.ValidateCommentStatement(codeCommentStatement);
			}
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x000F9E2C File Offset: 0x000F802C
		private void ValidateCommentStatement(CodeCommentStatement e)
		{
			this.ValidateComment(e.Comment);
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x000F9E3A File Offset: 0x000F803A
		private void ValidateComment(CodeComment e)
		{
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x000F9E3C File Offset: 0x000F803C
		private void ValidateStatement(CodeStatement e)
		{
			CodeValidator.ValidateCodeDirectives(e.StartDirectives);
			CodeValidator.ValidateCodeDirectives(e.EndDirectives);
			if (e is CodeCommentStatement)
			{
				this.ValidateCommentStatement((CodeCommentStatement)e);
				return;
			}
			if (e is CodeMethodReturnStatement)
			{
				this.ValidateMethodReturnStatement((CodeMethodReturnStatement)e);
				return;
			}
			if (e is CodeConditionStatement)
			{
				this.ValidateConditionStatement((CodeConditionStatement)e);
				return;
			}
			if (e is CodeTryCatchFinallyStatement)
			{
				this.ValidateTryCatchFinallyStatement((CodeTryCatchFinallyStatement)e);
				return;
			}
			if (e is CodeAssignStatement)
			{
				this.ValidateAssignStatement((CodeAssignStatement)e);
				return;
			}
			if (e is CodeExpressionStatement)
			{
				this.ValidateExpressionStatement((CodeExpressionStatement)e);
				return;
			}
			if (e is CodeIterationStatement)
			{
				this.ValidateIterationStatement((CodeIterationStatement)e);
				return;
			}
			if (e is CodeThrowExceptionStatement)
			{
				this.ValidateThrowExceptionStatement((CodeThrowExceptionStatement)e);
				return;
			}
			if (e is CodeSnippetStatement)
			{
				this.ValidateSnippetStatement((CodeSnippetStatement)e);
				return;
			}
			if (e is CodeVariableDeclarationStatement)
			{
				this.ValidateVariableDeclarationStatement((CodeVariableDeclarationStatement)e);
				return;
			}
			if (e is CodeAttachEventStatement)
			{
				this.ValidateAttachEventStatement((CodeAttachEventStatement)e);
				return;
			}
			if (e is CodeRemoveEventStatement)
			{
				this.ValidateRemoveEventStatement((CodeRemoveEventStatement)e);
				return;
			}
			if (e is CodeGotoStatement)
			{
				CodeValidator.ValidateGotoStatement((CodeGotoStatement)e);
				return;
			}
			if (e is CodeLabeledStatement)
			{
				this.ValidateLabeledStatement((CodeLabeledStatement)e);
				return;
			}
			throw new ArgumentException(SR.GetString("InvalidElementType", new object[] { e.GetType().FullName }), "e");
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x000F9FAC File Offset: 0x000F81AC
		private void ValidateStatements(CodeStatementCollection stms)
		{
			foreach (object obj in stms)
			{
				this.ValidateStatement((CodeStatement)obj);
			}
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x000F9FDB File Offset: 0x000F81DB
		private void ValidateExpressionStatement(CodeExpressionStatement e)
		{
			this.ValidateExpression(e.Expression);
		}

		// Token: 0x06003C88 RID: 15496 RVA: 0x000F9FE9 File Offset: 0x000F81E9
		private void ValidateIterationStatement(CodeIterationStatement e)
		{
			this.ValidateStatement(e.InitStatement);
			this.ValidateExpression(e.TestExpression);
			this.ValidateStatement(e.IncrementStatement);
			this.ValidateStatements(e.Statements);
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x000FA01B File Offset: 0x000F821B
		private void ValidateThrowExceptionStatement(CodeThrowExceptionStatement e)
		{
			if (e.ToThrow != null)
			{
				this.ValidateExpression(e.ToThrow);
			}
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x000FA031 File Offset: 0x000F8231
		private void ValidateMethodReturnStatement(CodeMethodReturnStatement e)
		{
			if (e.Expression != null)
			{
				this.ValidateExpression(e.Expression);
			}
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x000FA048 File Offset: 0x000F8248
		private void ValidateConditionStatement(CodeConditionStatement e)
		{
			this.ValidateExpression(e.Condition);
			this.ValidateStatements(e.TrueStatements);
			CodeStatementCollection falseStatements = e.FalseStatements;
			if (falseStatements.Count > 0)
			{
				this.ValidateStatements(e.FalseStatements);
			}
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x000FA08C File Offset: 0x000F828C
		private void ValidateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e)
		{
			this.ValidateStatements(e.TryStatements);
			CodeCatchClauseCollection catchClauses = e.CatchClauses;
			if (catchClauses.Count > 0)
			{
				foreach (object obj in catchClauses)
				{
					CodeCatchClause codeCatchClause = (CodeCatchClause)obj;
					CodeValidator.ValidateTypeReference(codeCatchClause.CatchExceptionType);
					CodeValidator.ValidateIdentifier(codeCatchClause, "LocalName", codeCatchClause.LocalName);
					this.ValidateStatements(codeCatchClause.Statements);
				}
			}
			CodeStatementCollection finallyStatements = e.FinallyStatements;
			if (finallyStatements.Count > 0)
			{
				this.ValidateStatements(finallyStatements);
			}
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x000FA111 File Offset: 0x000F8311
		private void ValidateAssignStatement(CodeAssignStatement e)
		{
			this.ValidateExpression(e.Left);
			this.ValidateExpression(e.Right);
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x000FA12B File Offset: 0x000F832B
		private void ValidateAttachEventStatement(CodeAttachEventStatement e)
		{
			this.ValidateEventReferenceExpression(e.Event);
			this.ValidateExpression(e.Listener);
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x000FA145 File Offset: 0x000F8345
		private void ValidateRemoveEventStatement(CodeRemoveEventStatement e)
		{
			this.ValidateEventReferenceExpression(e.Event);
			this.ValidateExpression(e.Listener);
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x000FA15F File Offset: 0x000F835F
		private static void ValidateGotoStatement(CodeGotoStatement e)
		{
			CodeValidator.ValidateIdentifier(e, "Label", e.Label);
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x000FA172 File Offset: 0x000F8372
		private void ValidateLabeledStatement(CodeLabeledStatement e)
		{
			CodeValidator.ValidateIdentifier(e, "Label", e.Label);
			if (e.Statement != null)
			{
				this.ValidateStatement(e.Statement);
			}
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x000FA199 File Offset: 0x000F8399
		private void ValidateVariableDeclarationStatement(CodeVariableDeclarationStatement e)
		{
			CodeValidator.ValidateTypeReference(e.Type);
			CodeValidator.ValidateIdentifier(e, "Name", e.Name);
			if (e.InitExpression != null)
			{
				this.ValidateExpression(e.InitExpression);
			}
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x000FA1CB File Offset: 0x000F83CB
		private void ValidateLinePragmaStart(CodeLinePragma e)
		{
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x000FA1D0 File Offset: 0x000F83D0
		private void ValidateEvent(CodeMemberEvent e)
		{
			if (e.CustomAttributes.Count > 0)
			{
				this.ValidateAttributes(e.CustomAttributes);
			}
			if (e.PrivateImplementationType != null)
			{
				CodeValidator.ValidateTypeReference(e.Type);
				CodeValidator.ValidateIdentifier(e, "Name", e.Name);
			}
			CodeValidator.ValidateTypeReferences(e.ImplementationTypes);
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x000FA228 File Offset: 0x000F8428
		private void ValidateParameters(CodeParameterDeclarationExpressionCollection parameters)
		{
			foreach (object obj in parameters)
			{
				CodeParameterDeclarationExpression codeParameterDeclarationExpression = (CodeParameterDeclarationExpression)obj;
				this.ValidateParameterDeclarationExpression(codeParameterDeclarationExpression);
			}
		}

		// Token: 0x06003C96 RID: 15510 RVA: 0x000FA259 File Offset: 0x000F8459
		private void ValidateSnippetStatement(CodeSnippetStatement e)
		{
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x000FA25C File Offset: 0x000F845C
		private void ValidateExpressionList(CodeExpressionCollection expressions)
		{
			foreach (object obj in expressions)
			{
				this.ValidateExpression((CodeExpression)obj);
			}
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x000FA28C File Offset: 0x000F848C
		private static void ValidateTypeReference(CodeTypeReference e)
		{
			string baseType = e.BaseType;
			CodeValidator.ValidateTypeName(e, "BaseType", baseType);
			CodeValidator.ValidateArity(e);
			CodeValidator.ValidateTypeReferences(e.TypeArguments);
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x000FA2C0 File Offset: 0x000F84C0
		private static void ValidateTypeReferences(CodeTypeReferenceCollection refs)
		{
			for (int i = 0; i < refs.Count; i++)
			{
				CodeValidator.ValidateTypeReference(refs[i]);
			}
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x000FA2EC File Offset: 0x000F84EC
		private static void ValidateArity(CodeTypeReference e)
		{
			string baseType = e.BaseType;
			int num = 0;
			for (int i = 0; i < baseType.Length; i++)
			{
				if (baseType[i] == '`')
				{
					i++;
					int num2 = 0;
					while (i < baseType.Length && baseType[i] >= '0' && baseType[i] <= '9')
					{
						num2 = num2 * 10 + (int)(baseType[i] - '0');
						i++;
					}
					num += num2;
				}
			}
			if (num != e.TypeArguments.Count && e.TypeArguments.Count != 0)
			{
				throw new ArgumentException(SR.GetString("ArityDoesntMatch", new object[]
				{
					baseType,
					e.TypeArguments.Count
				}));
			}
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x000FA3A8 File Offset: 0x000F85A8
		private static void ValidateTypeName(object e, string propertyName, string typeName)
		{
			if (!CodeGenerator.IsValidLanguageIndependentTypeName(typeName))
			{
				string @string = SR.GetString("InvalidTypeName", new object[]
				{
					typeName,
					propertyName,
					e.GetType().FullName
				});
				throw new ArgumentException(@string, "typeName");
			}
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x000FA3F0 File Offset: 0x000F85F0
		private static void ValidateIdentifier(object e, string propertyName, string identifier)
		{
			if (!CodeGenerator.IsValidLanguageIndependentIdentifier(identifier))
			{
				string @string = SR.GetString("InvalidLanguageIdentifier", new object[]
				{
					identifier,
					propertyName,
					e.GetType().FullName
				});
				throw new ArgumentException(@string, "identifier");
			}
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x000FA438 File Offset: 0x000F8638
		private void ValidateExpression(CodeExpression e)
		{
			if (e is CodeArrayCreateExpression)
			{
				this.ValidateArrayCreateExpression((CodeArrayCreateExpression)e);
				return;
			}
			if (e is CodeBaseReferenceExpression)
			{
				this.ValidateBaseReferenceExpression((CodeBaseReferenceExpression)e);
				return;
			}
			if (e is CodeBinaryOperatorExpression)
			{
				this.ValidateBinaryOperatorExpression((CodeBinaryOperatorExpression)e);
				return;
			}
			if (e is CodeCastExpression)
			{
				this.ValidateCastExpression((CodeCastExpression)e);
				return;
			}
			if (e is CodeDefaultValueExpression)
			{
				CodeValidator.ValidateDefaultValueExpression((CodeDefaultValueExpression)e);
				return;
			}
			if (e is CodeDelegateCreateExpression)
			{
				this.ValidateDelegateCreateExpression((CodeDelegateCreateExpression)e);
				return;
			}
			if (e is CodeFieldReferenceExpression)
			{
				this.ValidateFieldReferenceExpression((CodeFieldReferenceExpression)e);
				return;
			}
			if (e is CodeArgumentReferenceExpression)
			{
				CodeValidator.ValidateArgumentReferenceExpression((CodeArgumentReferenceExpression)e);
				return;
			}
			if (e is CodeVariableReferenceExpression)
			{
				CodeValidator.ValidateVariableReferenceExpression((CodeVariableReferenceExpression)e);
				return;
			}
			if (e is CodeIndexerExpression)
			{
				this.ValidateIndexerExpression((CodeIndexerExpression)e);
				return;
			}
			if (e is CodeArrayIndexerExpression)
			{
				this.ValidateArrayIndexerExpression((CodeArrayIndexerExpression)e);
				return;
			}
			if (e is CodeSnippetExpression)
			{
				this.ValidateSnippetExpression((CodeSnippetExpression)e);
				return;
			}
			if (e is CodeMethodInvokeExpression)
			{
				this.ValidateMethodInvokeExpression((CodeMethodInvokeExpression)e);
				return;
			}
			if (e is CodeMethodReferenceExpression)
			{
				this.ValidateMethodReferenceExpression((CodeMethodReferenceExpression)e);
				return;
			}
			if (e is CodeEventReferenceExpression)
			{
				this.ValidateEventReferenceExpression((CodeEventReferenceExpression)e);
				return;
			}
			if (e is CodeDelegateInvokeExpression)
			{
				this.ValidateDelegateInvokeExpression((CodeDelegateInvokeExpression)e);
				return;
			}
			if (e is CodeObjectCreateExpression)
			{
				this.ValidateObjectCreateExpression((CodeObjectCreateExpression)e);
				return;
			}
			if (e is CodeParameterDeclarationExpression)
			{
				this.ValidateParameterDeclarationExpression((CodeParameterDeclarationExpression)e);
				return;
			}
			if (e is CodeDirectionExpression)
			{
				this.ValidateDirectionExpression((CodeDirectionExpression)e);
				return;
			}
			if (e is CodePrimitiveExpression)
			{
				this.ValidatePrimitiveExpression((CodePrimitiveExpression)e);
				return;
			}
			if (e is CodePropertyReferenceExpression)
			{
				this.ValidatePropertyReferenceExpression((CodePropertyReferenceExpression)e);
				return;
			}
			if (e is CodePropertySetValueReferenceExpression)
			{
				this.ValidatePropertySetValueReferenceExpression((CodePropertySetValueReferenceExpression)e);
				return;
			}
			if (e is CodeThisReferenceExpression)
			{
				this.ValidateThisReferenceExpression((CodeThisReferenceExpression)e);
				return;
			}
			if (e is CodeTypeReferenceExpression)
			{
				CodeValidator.ValidateTypeReference(((CodeTypeReferenceExpression)e).Type);
				return;
			}
			if (e is CodeTypeOfExpression)
			{
				CodeValidator.ValidateTypeOfExpression((CodeTypeOfExpression)e);
				return;
			}
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			throw new ArgumentException(SR.GetString("InvalidElementType", new object[] { e.GetType().FullName }), "e");
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x000FA688 File Offset: 0x000F8888
		private void ValidateArrayCreateExpression(CodeArrayCreateExpression e)
		{
			CodeValidator.ValidateTypeReference(e.CreateType);
			CodeExpressionCollection initializers = e.Initializers;
			if (initializers.Count > 0)
			{
				this.ValidateExpressionList(initializers);
				return;
			}
			if (e.SizeExpression != null)
			{
				this.ValidateExpression(e.SizeExpression);
			}
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x000FA6CC File Offset: 0x000F88CC
		private void ValidateBaseReferenceExpression(CodeBaseReferenceExpression e)
		{
		}

		// Token: 0x06003CA0 RID: 15520 RVA: 0x000FA6CE File Offset: 0x000F88CE
		private void ValidateBinaryOperatorExpression(CodeBinaryOperatorExpression e)
		{
			this.ValidateExpression(e.Left);
			this.ValidateExpression(e.Right);
		}

		// Token: 0x06003CA1 RID: 15521 RVA: 0x000FA6E8 File Offset: 0x000F88E8
		private void ValidateCastExpression(CodeCastExpression e)
		{
			CodeValidator.ValidateTypeReference(e.TargetType);
			this.ValidateExpression(e.Expression);
		}

		// Token: 0x06003CA2 RID: 15522 RVA: 0x000FA701 File Offset: 0x000F8901
		private static void ValidateDefaultValueExpression(CodeDefaultValueExpression e)
		{
			CodeValidator.ValidateTypeReference(e.Type);
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x000FA70E File Offset: 0x000F890E
		private void ValidateDelegateCreateExpression(CodeDelegateCreateExpression e)
		{
			CodeValidator.ValidateTypeReference(e.DelegateType);
			this.ValidateExpression(e.TargetObject);
			CodeValidator.ValidateIdentifier(e, "MethodName", e.MethodName);
		}

		// Token: 0x06003CA4 RID: 15524 RVA: 0x000FA738 File Offset: 0x000F8938
		private void ValidateFieldReferenceExpression(CodeFieldReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				this.ValidateExpression(e.TargetObject);
			}
			CodeValidator.ValidateIdentifier(e, "FieldName", e.FieldName);
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x000FA75F File Offset: 0x000F895F
		private static void ValidateArgumentReferenceExpression(CodeArgumentReferenceExpression e)
		{
			CodeValidator.ValidateIdentifier(e, "ParameterName", e.ParameterName);
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x000FA772 File Offset: 0x000F8972
		private static void ValidateVariableReferenceExpression(CodeVariableReferenceExpression e)
		{
			CodeValidator.ValidateIdentifier(e, "VariableName", e.VariableName);
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x000FA788 File Offset: 0x000F8988
		private void ValidateIndexerExpression(CodeIndexerExpression e)
		{
			this.ValidateExpression(e.TargetObject);
			foreach (object obj in e.Indices)
			{
				CodeExpression codeExpression = (CodeExpression)obj;
				this.ValidateExpression(codeExpression);
			}
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x000FA7F0 File Offset: 0x000F89F0
		private void ValidateArrayIndexerExpression(CodeArrayIndexerExpression e)
		{
			this.ValidateExpression(e.TargetObject);
			foreach (object obj in e.Indices)
			{
				CodeExpression codeExpression = (CodeExpression)obj;
				this.ValidateExpression(codeExpression);
			}
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x000FA858 File Offset: 0x000F8A58
		private void ValidateSnippetExpression(CodeSnippetExpression e)
		{
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x000FA85A File Offset: 0x000F8A5A
		private void ValidateMethodInvokeExpression(CodeMethodInvokeExpression e)
		{
			this.ValidateMethodReferenceExpression(e.Method);
			this.ValidateExpressionList(e.Parameters);
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x000FA874 File Offset: 0x000F8A74
		private void ValidateMethodReferenceExpression(CodeMethodReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				this.ValidateExpression(e.TargetObject);
			}
			CodeValidator.ValidateIdentifier(e, "MethodName", e.MethodName);
			CodeValidator.ValidateTypeReferences(e.TypeArguments);
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x000FA8A6 File Offset: 0x000F8AA6
		private void ValidateEventReferenceExpression(CodeEventReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				this.ValidateExpression(e.TargetObject);
			}
			CodeValidator.ValidateIdentifier(e, "EventName", e.EventName);
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x000FA8CD File Offset: 0x000F8ACD
		private void ValidateDelegateInvokeExpression(CodeDelegateInvokeExpression e)
		{
			if (e.TargetObject != null)
			{
				this.ValidateExpression(e.TargetObject);
			}
			this.ValidateExpressionList(e.Parameters);
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x000FA8EF File Offset: 0x000F8AEF
		private void ValidateObjectCreateExpression(CodeObjectCreateExpression e)
		{
			CodeValidator.ValidateTypeReference(e.CreateType);
			this.ValidateExpressionList(e.Parameters);
		}

		// Token: 0x06003CAF RID: 15535 RVA: 0x000FA908 File Offset: 0x000F8B08
		private void ValidateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
		{
			if (e.CustomAttributes.Count > 0)
			{
				this.ValidateAttributes(e.CustomAttributes);
			}
			CodeValidator.ValidateTypeReference(e.Type);
			CodeValidator.ValidateIdentifier(e, "Name", e.Name);
		}

		// Token: 0x06003CB0 RID: 15536 RVA: 0x000FA940 File Offset: 0x000F8B40
		private void ValidateDirectionExpression(CodeDirectionExpression e)
		{
			this.ValidateExpression(e.Expression);
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x000FA94E File Offset: 0x000F8B4E
		private void ValidatePrimitiveExpression(CodePrimitiveExpression e)
		{
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x000FA950 File Offset: 0x000F8B50
		private void ValidatePropertyReferenceExpression(CodePropertyReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				this.ValidateExpression(e.TargetObject);
			}
			CodeValidator.ValidateIdentifier(e, "PropertyName", e.PropertyName);
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x000FA977 File Offset: 0x000F8B77
		private void ValidatePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e)
		{
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x000FA979 File Offset: 0x000F8B79
		private void ValidateThisReferenceExpression(CodeThisReferenceExpression e)
		{
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x000FA97B File Offset: 0x000F8B7B
		private static void ValidateTypeOfExpression(CodeTypeOfExpression e)
		{
			CodeValidator.ValidateTypeReference(e.Type);
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x000FA988 File Offset: 0x000F8B88
		private static void ValidateCodeDirectives(CodeDirectiveCollection e)
		{
			for (int i = 0; i < e.Count; i++)
			{
				CodeValidator.ValidateCodeDirective(e[i]);
			}
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x000FA9B4 File Offset: 0x000F8BB4
		private static void ValidateCodeDirective(CodeDirective e)
		{
			if (e is CodeChecksumPragma)
			{
				CodeValidator.ValidateChecksumPragma((CodeChecksumPragma)e);
				return;
			}
			if (e is CodeRegionDirective)
			{
				CodeValidator.ValidateRegionDirective((CodeRegionDirective)e);
				return;
			}
			throw new ArgumentException(SR.GetString("InvalidElementType", new object[] { e.GetType().FullName }), "e");
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x000FAA11 File Offset: 0x000F8C11
		private static void ValidateChecksumPragma(CodeChecksumPragma e)
		{
			if (e.FileName.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				throw new ArgumentException(SR.GetString("InvalidPathCharsInChecksum", new object[] { e.FileName }));
			}
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x000FAA45 File Offset: 0x000F8C45
		private static void ValidateRegionDirective(CodeRegionDirective e)
		{
			if (e.RegionText.IndexOfAny(CodeValidator.newLineChars) != -1)
			{
				throw new ArgumentException(SR.GetString("InvalidRegion", new object[] { e.RegionText }));
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06003CBA RID: 15546 RVA: 0x000FAA79 File Offset: 0x000F8C79
		private bool IsCurrentInterface
		{
			get
			{
				return this.currentClass != null && !(this.currentClass is CodeTypeDelegate) && this.currentClass.IsInterface;
			}
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06003CBB RID: 15547 RVA: 0x000FAA9D File Offset: 0x000F8C9D
		private bool IsCurrentEnum
		{
			get
			{
				return this.currentClass != null && !(this.currentClass is CodeTypeDelegate) && this.currentClass.IsEnum;
			}
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06003CBC RID: 15548 RVA: 0x000FAAC1 File Offset: 0x000F8CC1
		private bool IsCurrentDelegate
		{
			get
			{
				return this.currentClass != null && this.currentClass is CodeTypeDelegate;
			}
		}

		// Token: 0x04002C67 RID: 11367
		private static readonly char[] newLineChars = new char[] { '\r', '\n', '\u2028', '\u2029', '\u0085' };

		// Token: 0x04002C68 RID: 11368
		private CodeTypeDeclaration currentClass;
	}
}
