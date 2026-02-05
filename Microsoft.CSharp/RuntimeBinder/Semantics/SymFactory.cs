using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200006D RID: 109
	internal sealed class SymFactory
	{
		// Token: 0x060003B5 RID: 949 RVA: 0x00016AD4 File Offset: 0x00014CD4
		public SymFactory(SYMTBL symtable)
		{
			this._symbolTable = symtable;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00016AE4 File Offset: 0x00014CE4
		private Symbol NewBasicSymbol(SYMKIND kind, Name name, ParentSymbol parent)
		{
			Symbol symbol;
			switch (kind)
			{
			case SYMKIND.SK_NamespaceSymbol:
				symbol = new NamespaceSymbol();
				symbol.name = name;
				break;
			case SYMKIND.SK_AssemblyQualifiedNamespaceSymbol:
				symbol = new AssemblyQualifiedNamespaceSymbol();
				symbol.name = name;
				break;
			case SYMKIND.SK_AggregateSymbol:
				symbol = new AggregateSymbol();
				symbol.name = name;
				break;
			case SYMKIND.SK_AggregateDeclaration:
				symbol = new AggregateDeclaration();
				symbol.name = name;
				break;
			case SYMKIND.SK_TypeParameterSymbol:
				symbol = new TypeParameterSymbol();
				symbol.name = name;
				break;
			case SYMKIND.SK_FieldSymbol:
				symbol = new FieldSymbol();
				symbol.name = name;
				break;
			case SYMKIND.SK_LocalVariableSymbol:
				symbol = new LocalVariableSymbol();
				symbol.name = name;
				break;
			case SYMKIND.SK_MethodSymbol:
				symbol = new MethodSymbol();
				symbol.name = name;
				break;
			case SYMKIND.SK_PropertySymbol:
				symbol = new PropertySymbol();
				symbol.name = name;
				break;
			case SYMKIND.SK_EventSymbol:
				symbol = new EventSymbol();
				symbol.name = name;
				break;
			case SYMKIND.SK_Scope:
				symbol = new Scope();
				symbol.name = name;
				break;
			case SYMKIND.SK_IndexerSymbol:
				symbol = new IndexerSymbol();
				symbol.name = name;
				break;
			default:
				throw Error.InternalCompilerError();
			}
			symbol.setKind(kind);
			if (parent != null)
			{
				parent.AddToChildList(symbol);
				this._symbolTable.InsertChild(parent, symbol);
			}
			return symbol;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00016C0E File Offset: 0x00014E0E
		public NamespaceSymbol CreateNamespace(Name name, NamespaceSymbol parent)
		{
			NamespaceSymbol namespaceSymbol = (NamespaceSymbol)this.NewBasicSymbol(SYMKIND.SK_NamespaceSymbol, name, parent);
			namespaceSymbol.SetAccess(ACCESS.ACC_PUBLIC);
			return namespaceSymbol;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00016C25 File Offset: 0x00014E25
		public AssemblyQualifiedNamespaceSymbol CreateNamespaceAid(Name name, ParentSymbol parent)
		{
			return this.NewBasicSymbol(SYMKIND.SK_AssemblyQualifiedNamespaceSymbol, name, parent) as AssemblyQualifiedNamespaceSymbol;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00016C38 File Offset: 0x00014E38
		public AggregateSymbol CreateAggregate(Name name, NamespaceOrAggregateSymbol parent, TypeManager typeManager)
		{
			if (name == null || parent == null || typeManager == null)
			{
				throw Error.InternalCompilerError();
			}
			AggregateSymbol aggregateSymbol = (AggregateSymbol)this.NewBasicSymbol(SYMKIND.SK_AggregateSymbol, name, parent);
			aggregateSymbol.name = name;
			aggregateSymbol.SetTypeManager(typeManager);
			aggregateSymbol.SetSealed(false);
			aggregateSymbol.SetAccess(ACCESS.ACC_UNKNOWN);
			aggregateSymbol.SetIfaces(null);
			aggregateSymbol.SetIfacesAll(null);
			aggregateSymbol.SetTypeVars(null);
			return aggregateSymbol;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00016C94 File Offset: 0x00014E94
		public AggregateDeclaration CreateAggregateDecl(AggregateSymbol agg, AggregateDeclaration declOuter)
		{
			AggregateDeclaration aggregateDeclaration = this.NewBasicSymbol(SYMKIND.SK_AggregateDeclaration, agg.name, null) as AggregateDeclaration;
			if (declOuter != null)
			{
				declOuter.AddToChildList(aggregateDeclaration);
			}
			agg.AddDecl(aggregateDeclaration);
			return aggregateDeclaration;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00016CC7 File Offset: 0x00014EC7
		public FieldSymbol CreateMemberVar(Name name, ParentSymbol parent)
		{
			return this.NewBasicSymbol(SYMKIND.SK_FieldSymbol, name, parent) as FieldSymbol;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00016CD7 File Offset: 0x00014ED7
		public LocalVariableSymbol CreateLocalVar(Name name, ParentSymbol parent, CType type)
		{
			LocalVariableSymbol localVariableSymbol = (LocalVariableSymbol)this.NewBasicSymbol(SYMKIND.SK_LocalVariableSymbol, name, parent);
			localVariableSymbol.SetType(type);
			localVariableSymbol.SetAccess(ACCESS.ACC_UNKNOWN);
			localVariableSymbol.wrap = null;
			return localVariableSymbol;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00016CFC File Offset: 0x00014EFC
		public MethodSymbol CreateMethod(Name name, ParentSymbol parent)
		{
			return this.NewBasicSymbol(SYMKIND.SK_MethodSymbol, name, parent) as MethodSymbol;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00016D0C File Offset: 0x00014F0C
		public PropertySymbol CreateProperty(Name name, ParentSymbol parent)
		{
			return this.NewBasicSymbol(SYMKIND.SK_PropertySymbol, name, parent) as PropertySymbol;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00016D1C File Offset: 0x00014F1C
		public EventSymbol CreateEvent(Name name, ParentSymbol parent)
		{
			return this.NewBasicSymbol(SYMKIND.SK_EventSymbol, name, parent) as EventSymbol;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00016D2D File Offset: 0x00014F2D
		public TypeParameterSymbol CreateMethodTypeParameter(Name pName, MethodSymbol pParent, int index, int indexTotal)
		{
			TypeParameterSymbol typeParameterSymbol = (TypeParameterSymbol)this.NewBasicSymbol(SYMKIND.SK_TypeParameterSymbol, pName, pParent);
			typeParameterSymbol.SetIndexInOwnParameters(index);
			typeParameterSymbol.SetIndexInTotalParameters(indexTotal);
			typeParameterSymbol.SetIsMethodTypeParameter(true);
			typeParameterSymbol.SetAccess(ACCESS.ACC_PRIVATE);
			return typeParameterSymbol;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00016D5A File Offset: 0x00014F5A
		public TypeParameterSymbol CreateClassTypeParameter(Name pName, AggregateSymbol pParent, int index, int indexTotal)
		{
			TypeParameterSymbol typeParameterSymbol = (TypeParameterSymbol)this.NewBasicSymbol(SYMKIND.SK_TypeParameterSymbol, pName, pParent);
			typeParameterSymbol.SetIndexInOwnParameters(index);
			typeParameterSymbol.SetIndexInTotalParameters(indexTotal);
			typeParameterSymbol.SetIsMethodTypeParameter(false);
			typeParameterSymbol.SetAccess(ACCESS.ACC_PRIVATE);
			return typeParameterSymbol;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00016D88 File Offset: 0x00014F88
		public Scope CreateScope(Scope parent)
		{
			Scope scope = (Scope)this.NewBasicSymbol(SYMKIND.SK_Scope, null, parent);
			if (parent != null)
			{
				scope.nestingOrder = parent.nestingOrder + 1U;
			}
			return scope;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00016DB7 File Offset: 0x00014FB7
		public IndexerSymbol CreateIndexer(Name name, ParentSymbol parent, Name realName)
		{
			IndexerSymbol indexerSymbol = (IndexerSymbol)this.NewBasicSymbol(SYMKIND.SK_IndexerSymbol, name, parent);
			indexerSymbol.setKind(SYMKIND.SK_PropertySymbol);
			indexerSymbol.isOperator = true;
			return indexerSymbol;
		}

		// Token: 0x040004D0 RID: 1232
		private readonly SYMTBL _symbolTable;
	}
}
