using System;
using System.Globalization;
using System.Text;
using Microsoft.CSharp.RuntimeBinder.Semantics;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Errors
{
	// Token: 0x020000CC RID: 204
	internal sealed class UserStringBuilder
	{
		// Token: 0x06000681 RID: 1665 RVA: 0x0001EC76 File Offset: 0x0001CE76
		public UserStringBuilder(GlobalSymbolContext globalSymbols)
		{
			this.m_buildingInProgress = false;
			this.m_globalSymbols = globalSymbols;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001EC8C File Offset: 0x0001CE8C
		private void BeginString()
		{
			this.m_buildingInProgress = true;
			this.m_strBuilder = new StringBuilder();
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001ECA0 File Offset: 0x0001CEA0
		private void EndString(out string s)
		{
			this.m_buildingInProgress = false;
			s = this.m_strBuilder.ToString();
			this.m_strBuilder = null;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001ECC0 File Offset: 0x0001CEC0
		private void ErrSK(out string psz, SYMKIND sk)
		{
			MessageID messageID;
			switch (sk)
			{
			case SYMKIND.SK_NamespaceSymbol:
				messageID = MessageID.SK_NAMESPACE;
				goto IL_0052;
			case SYMKIND.SK_AggregateSymbol:
				messageID = MessageID.SK_CLASS;
				goto IL_0052;
			case SYMKIND.SK_TypeParameterSymbol:
				messageID = MessageID.SK_TYVAR;
				goto IL_0052;
			case SYMKIND.SK_FieldSymbol:
				messageID = MessageID.SK_FIELD;
				goto IL_0052;
			case SYMKIND.SK_LocalVariableSymbol:
				messageID = MessageID.SK_VARIABLE;
				goto IL_0052;
			case SYMKIND.SK_MethodSymbol:
				messageID = MessageID.SK_METHOD;
				goto IL_0052;
			case SYMKIND.SK_PropertySymbol:
				messageID = MessageID.SK_PROPERTY;
				goto IL_0052;
			case SYMKIND.SK_EventSymbol:
				messageID = MessageID.SK_EVENT;
				goto IL_0052;
			}
			messageID = MessageID.SK_UNKNOWN;
			IL_0052:
			this.ErrId(out psz, messageID);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001ED28 File Offset: 0x0001CF28
		private void ErrAppendParamList(TypeArray @params, bool isVarargs, bool isParamArray)
		{
			if (@params == null)
			{
				return;
			}
			for (int i = 0; i < @params.Count; i++)
			{
				if (i > 0)
				{
					this.ErrAppendString(", ");
				}
				if (isParamArray && i == @params.Count - 1)
				{
					this.ErrAppendString("params ");
				}
				this.ErrAppendType(@params[i], null);
			}
			if (isVarargs)
			{
				if (@params.Count != 0)
				{
					this.ErrAppendString(", ");
				}
				this.ErrAppendString("...");
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001EDA1 File Offset: 0x0001CFA1
		private void ErrAppendString(string str)
		{
			this.m_strBuilder.Append(str);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001EDB0 File Offset: 0x0001CFB0
		private void ErrAppendChar(char ch)
		{
			this.m_strBuilder.Append(ch);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001EDBF File Offset: 0x0001CFBF
		private void ErrAppendPrintf(string format, params object[] args)
		{
			this.ErrAppendString(string.Format(CultureInfo.InvariantCulture, format, args));
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001EDD3 File Offset: 0x0001CFD3
		private void ErrAppendName(Name name)
		{
			if (name == NameManager.GetPredefinedName(PredefinedName.PN_INDEXERINTERNAL))
			{
				this.ErrAppendString("this");
				return;
			}
			this.ErrAppendString(name.Text);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001EDF7 File Offset: 0x0001CFF7
		private void ErrAppendMethodParentSym(MethodSymbol sym, SubstContext pcxt, out TypeArray substMethTyParams)
		{
			substMethTyParams = null;
			this.ErrAppendParentSym(sym, pcxt);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001EE04 File Offset: 0x0001D004
		private void ErrAppendParentSym(Symbol sym, SubstContext pctx)
		{
			this.ErrAppendParentCore(sym.parent, pctx);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001EE14 File Offset: 0x0001D014
		private void ErrAppendParentCore(Symbol parent, SubstContext pctx)
		{
			if (parent == null)
			{
				return;
			}
			if (parent == this.getBSymmgr().GetRootNS())
			{
				return;
			}
			AggregateSymbol aggregateSymbol;
			if (pctx != null && !pctx.FNop() && (aggregateSymbol = parent as AggregateSymbol) != null && aggregateSymbol.GetTypeVarsAll().Count != 0)
			{
				CType ctype = this.GetTypeManager().SubstType(aggregateSymbol.getThisType(), pctx);
				this.ErrAppendType(ctype, null);
			}
			else
			{
				this.ErrAppendSym(parent, null);
			}
			this.ErrAppendChar('.');
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001EE84 File Offset: 0x0001D084
		private void ErrAppendTypeParameters(TypeArray @params, SubstContext pctx, bool forClass)
		{
			if (@params != null && @params.Count != 0)
			{
				this.ErrAppendChar('<');
				this.ErrAppendType(@params[0], pctx);
				for (int i = 1; i < @params.Count; i++)
				{
					this.ErrAppendString(",");
					this.ErrAppendType(@params[i], pctx);
				}
				this.ErrAppendChar('>');
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001EEE4 File Offset: 0x0001D0E4
		private void ErrAppendMethod(MethodSymbol meth, SubstContext pctx, bool fArgs)
		{
			if (meth.IsExpImpl() && meth.swtSlot)
			{
				this.ErrAppendParentSym(meth, pctx);
				SubstContext substContext = new SubstContext(this.GetTypeManager().SubstType(meth.swtSlot.GetType(), pctx) as AggregateType);
				this.ErrAppendSym(meth.swtSlot.Sym, substContext, fArgs);
				return;
			}
			if (meth.isPropertyAccessor())
			{
				PropertySymbol property = meth.getProperty();
				this.ErrAppendSym(property, pctx);
				if (property.GetterMethod == meth)
				{
					this.ErrAppendString(".get");
					return;
				}
				this.ErrAppendString(".set");
				return;
			}
			else
			{
				if (!meth.isEventAccessor())
				{
					TypeArray typeArray = null;
					this.ErrAppendMethodParentSym(meth, pctx, out typeArray);
					if (meth.IsConstructor())
					{
						this.ErrAppendName(meth.getClass().name);
					}
					else if (meth.IsDestructor())
					{
						this.ErrAppendChar('~');
						this.ErrAppendName(meth.getClass().name);
					}
					else if (meth.isConversionOperator())
					{
						this.ErrAppendString(meth.isImplicit() ? "implicit" : "explicit");
						this.ErrAppendString(" operator ");
						this.ErrAppendType(meth.RetType, pctx);
					}
					else if (meth.isOperator)
					{
						this.ErrAppendString("operator ");
						this.ErrAppendString(Operators.OperatorOfMethodName(meth.name));
					}
					else if (meth.IsExpImpl())
					{
						if (meth.errExpImpl != null)
						{
							this.ErrAppendType(meth.errExpImpl, pctx, fArgs);
						}
					}
					else
					{
						this.ErrAppendName(meth.name);
					}
					if (typeArray == null)
					{
						this.ErrAppendTypeParameters(meth.typeVars, pctx, false);
					}
					if (fArgs)
					{
						this.ErrAppendChar('(');
						this.ErrAppendParamList(this.GetTypeManager().SubstTypeArray(meth.Params, pctx), meth.isVarargs, meth.isParamArray);
						this.ErrAppendChar(')');
					}
					return;
				}
				EventSymbol @event = meth.getEvent();
				this.ErrAppendSym(@event, pctx);
				if (@event.methAdd == meth)
				{
					this.ErrAppendString(".add");
					return;
				}
				this.ErrAppendString(".remove");
				return;
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001F0DA File Offset: 0x0001D2DA
		private void ErrAppendIndexer(IndexerSymbol indexer, SubstContext pctx)
		{
			this.ErrAppendString("this[");
			this.ErrAppendParamList(this.GetTypeManager().SubstTypeArray(indexer.Params, pctx), false, indexer.isParamArray);
			this.ErrAppendChar(']');
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001F110 File Offset: 0x0001D310
		private void ErrAppendProperty(PropertySymbol prop, SubstContext pctx)
		{
			this.ErrAppendParentSym(prop, pctx);
			if (prop.IsExpImpl() && prop.swtSlot.Sym != null)
			{
				SubstContext substContext = new SubstContext(this.GetTypeManager().SubstType(prop.swtSlot.GetType(), pctx) as AggregateType);
				this.ErrAppendSym(prop.swtSlot.Sym, substContext);
				return;
			}
			if (prop.IsExpImpl())
			{
				if (prop.errExpImpl != null)
				{
					this.ErrAppendType(prop.errExpImpl, pctx, false);
				}
				IndexerSymbol indexerSymbol;
				if ((indexerSymbol = prop as IndexerSymbol) != null)
				{
					this.ErrAppendChar('.');
					this.ErrAppendIndexer(indexerSymbol, pctx);
					return;
				}
			}
			else
			{
				IndexerSymbol indexerSymbol2;
				if ((indexerSymbol2 = prop as IndexerSymbol) != null)
				{
					this.ErrAppendIndexer(indexerSymbol2, pctx);
					return;
				}
				this.ErrAppendName(prop.name);
			}
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001F1C7 File Offset: 0x0001D3C7
		private void ErrAppendEvent(EventSymbol @event, SubstContext pctx)
		{
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001F1CC File Offset: 0x0001D3CC
		private void ErrAppendId(MessageID id)
		{
			string text;
			this.ErrId(out text, id);
			this.ErrAppendString(text);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001F1E9 File Offset: 0x0001D3E9
		private void ErrAppendSym(Symbol sym, SubstContext pctx)
		{
			this.ErrAppendSym(sym, pctx, true);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001F1F4 File Offset: 0x0001D3F4
		private void ErrAppendSym(Symbol sym, SubstContext pctx, bool fArgs)
		{
			switch (sym.getKind())
			{
			case SYMKIND.SK_NamespaceSymbol:
			case SYMKIND.SK_AssemblyQualifiedNamespaceSymbol:
				if (sym == this.getBSymmgr().GetRootNS())
				{
					this.ErrAppendId(MessageID.GlobalNamespace);
					return;
				}
				this.ErrAppendParentSym(sym, null);
				this.ErrAppendName(sym.name);
				return;
			case SYMKIND.SK_AggregateSymbol:
			{
				string niceName = PredefinedTypes.GetNiceName(sym as AggregateSymbol);
				if (niceName != null)
				{
					this.ErrAppendString(niceName);
					return;
				}
				this.ErrAppendParentSym(sym, pctx);
				this.ErrAppendName(sym.name);
				this.ErrAppendTypeParameters(((AggregateSymbol)sym).GetTypeVars(), pctx, true);
				return;
			}
			case SYMKIND.SK_AggregateDeclaration:
				this.ErrAppendSym(((AggregateDeclaration)sym).Agg(), pctx);
				return;
			case SYMKIND.SK_TypeParameterSymbol:
				if (sym.name == null)
				{
					TypeParameterSymbol typeParameterSymbol = (TypeParameterSymbol)sym;
					if (typeParameterSymbol.IsMethodTypeParameter())
					{
						this.ErrAppendChar('!');
					}
					this.ErrAppendChar('!');
					this.ErrAppendPrintf("{0}", new object[] { typeParameterSymbol.GetIndexInTotalParameters() });
					return;
				}
				this.ErrAppendName(sym.name);
				return;
			case SYMKIND.SK_FieldSymbol:
				this.ErrAppendParentSym(sym, pctx);
				this.ErrAppendName(sym.name);
				return;
			case SYMKIND.SK_LocalVariableSymbol:
				this.ErrAppendName(sym.name);
				return;
			case SYMKIND.SK_MethodSymbol:
				this.ErrAppendMethod((MethodSymbol)sym, pctx, fArgs);
				return;
			case SYMKIND.SK_PropertySymbol:
				this.ErrAppendProperty((PropertySymbol)sym, pctx);
				return;
			case SYMKIND.SK_EventSymbol:
				this.ErrAppendEvent((EventSymbol)sym, pctx);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001F355 File Offset: 0x0001D555
		private void ErrAppendType(CType pType, SubstContext pCtx)
		{
			this.ErrAppendType(pType, pCtx, true);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001F360 File Offset: 0x0001D560
		private void ErrAppendType(CType pType, SubstContext pctx, bool fArgs)
		{
			if (pctx != null)
			{
				if (!pctx.FNop())
				{
					pType = this.GetTypeManager().SubstType(pType, pctx);
				}
				pctx = null;
			}
			switch (pType.GetTypeKind())
			{
			case TypeKind.TK_AggregateType:
			{
				AggregateType aggregateType = (AggregateType)pType;
				string niceName = PredefinedTypes.GetNiceName(aggregateType.getAggregate());
				if (niceName != null)
				{
					this.ErrAppendString(niceName);
				}
				else
				{
					if (aggregateType.outerType != null)
					{
						this.ErrAppendType(aggregateType.outerType, pctx);
						this.ErrAppendChar('.');
					}
					else
					{
						this.ErrAppendParentSym(aggregateType.getAggregate(), pctx);
					}
					this.ErrAppendName(aggregateType.getAggregate().name);
				}
				this.ErrAppendTypeParameters(aggregateType.GetTypeArgsThis(), pctx, true);
				return;
			}
			case TypeKind.TK_VoidType:
				this.ErrAppendName(this.GetNameManager().Lookup(TokenFacts.GetText(TokenKind.Void)));
				return;
			case TypeKind.TK_NullType:
				this.ErrAppendId(MessageID.NULL);
				return;
			case TypeKind.TK_MethodGroupType:
				this.ErrAppendId(MessageID.MethodGroup);
				return;
			case TypeKind.TK_ErrorType:
			{
				ErrorType errorType = (ErrorType)pType;
				if (errorType.HasParent)
				{
					this.ErrAppendName(errorType.nameText);
					this.ErrAppendTypeParameters(errorType.typeArgs, pctx, true);
					return;
				}
				this.ErrAppendId(MessageID.ERRORSYM);
				return;
			}
			case TypeKind.TK_ArgumentListType:
				this.ErrAppendString(TokenFacts.GetText(TokenKind.ArgList));
				return;
			case TypeKind.TK_ArrayType:
			{
				CType ctype = ((ArrayType)pType).GetBaseElementType();
				if (ctype != null)
				{
					this.ErrAppendType(ctype, pctx);
					ctype = pType;
					ArrayType arrayType;
					while ((arrayType = ctype as ArrayType) != null)
					{
						int rank = arrayType.rank;
						this.ErrAppendChar('[');
						if (rank == 1)
						{
							if (!arrayType.IsSZArray)
							{
								this.ErrAppendChar('*');
							}
						}
						else
						{
							for (int i = rank; i > 1; i--)
							{
								this.ErrAppendChar(',');
							}
						}
						this.ErrAppendChar(']');
						ctype = arrayType.GetElementType();
					}
				}
				break;
			}
			case TypeKind.TK_PointerType:
				this.ErrAppendType(((PointerType)pType).GetReferentType(), pctx);
				this.ErrAppendChar('*');
				return;
			case TypeKind.TK_ParameterModifierType:
			{
				ParameterModifierType parameterModifierType = (ParameterModifierType)pType;
				this.ErrAppendString(parameterModifierType.isOut ? "out " : "ref ");
				this.ErrAppendType(parameterModifierType.GetParameterType(), pctx);
				return;
			}
			case TypeKind.TK_NullableType:
				this.ErrAppendType(((NullableType)pType).GetUnderlyingType(), pctx);
				this.ErrAppendChar('?');
				break;
			case TypeKind.TK_TypeParameterType:
				if (pType.GetName() == null)
				{
					TypeParameterType typeParameterType = (TypeParameterType)pType;
					if (typeParameterType.IsMethodTypeParameter())
					{
						this.ErrAppendChar('!');
					}
					this.ErrAppendChar('!');
					this.ErrAppendPrintf("{0}", new object[] { typeParameterType.GetIndexInTotalParameters() });
					return;
				}
				this.ErrAppendName(pType.GetName());
				return;
			default:
				return;
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001F5DC File Offset: 0x0001D7DC
		public bool ErrArgToString(out string psz, ErrArg parg, out bool fUserStrings)
		{
			fUserStrings = false;
			psz = null;
			bool flag = true;
			switch (parg.eak)
			{
			case ErrArgKind.Ids:
				this.ErrId(out psz, parg.ids);
				return flag;
			case ErrArgKind.SymKind:
				this.ErrSK(out psz, parg.sk);
				return flag;
			case ErrArgKind.Sym:
				this.BeginString();
				this.ErrAppendSym(parg.sym, null);
				this.EndString(out psz);
				fUserStrings = true;
				return flag;
			case ErrArgKind.Type:
				this.BeginString();
				this.ErrAppendType(parg.pType, null);
				this.EndString(out psz);
				fUserStrings = true;
				return flag;
			case ErrArgKind.Name:
				if (parg.name == NameManager.GetPredefinedName(PredefinedName.PN_INDEXERINTERNAL))
				{
					psz = "this";
					return flag;
				}
				psz = parg.name.Text;
				return flag;
			case ErrArgKind.Str:
				psz = parg.psz;
				return flag;
			case ErrArgKind.SymWithType:
			{
				SubstContext substContext = new SubstContext(parg.swtMemo.ats, null);
				this.BeginString();
				this.ErrAppendSym(parg.swtMemo.sym, substContext, true);
				this.EndString(out psz);
				fUserStrings = true;
				return flag;
			}
			case ErrArgKind.MethWithInst:
			{
				SubstContext substContext2 = new SubstContext(parg.mpwiMemo.ats, parg.mpwiMemo.typeArgs);
				this.BeginString();
				this.ErrAppendSym(parg.mpwiMemo.sym, substContext2, true);
				this.EndString(out psz);
				fUserStrings = true;
				return flag;
			}
			}
			flag = false;
			return flag;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001F74B File Offset: 0x0001D94B
		private NameManager GetNameManager()
		{
			return this.m_globalSymbols.GetNameManager();
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0001F758 File Offset: 0x0001D958
		private TypeManager GetTypeManager()
		{
			return this.m_globalSymbols.GetTypes();
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0001F765 File Offset: 0x0001D965
		private BSYMMGR getBSymmgr()
		{
			return this.m_globalSymbols.GetGlobalSymbols();
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001F772 File Offset: 0x0001D972
		private int GetTypeID(CType type)
		{
			return 0;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001F775 File Offset: 0x0001D975
		private void ErrId(out string s, MessageID id)
		{
			s = ErrorFacts.GetMessage(id);
		}

		// Token: 0x04000638 RID: 1592
		private bool m_buildingInProgress;

		// Token: 0x04000639 RID: 1593
		private GlobalSymbolContext m_globalSymbols;

		// Token: 0x0400063A RID: 1594
		private StringBuilder m_strBuilder;
	}
}
