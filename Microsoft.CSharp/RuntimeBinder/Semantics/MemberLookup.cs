using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder.Errors;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200004F RID: 79
	internal sealed class MemberLookup
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x000124D0 File Offset: 0x000106D0
		private void RecordType(AggregateType type, Symbol sym)
		{
			if (!this._prgtype.Contains(type))
			{
				this._prgtype.Add(type);
			}
			this._csym++;
			if (this._swtFirst == null)
			{
				this._swtFirst.Set(sym, type);
				this._fMulti = sym is MethodSymbol || sym is IndexerSymbol;
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0001253C File Offset: 0x0001073C
		private bool SearchSingleType(AggregateType typeCur, out bool pfHideByName)
		{
			bool flag = false;
			pfHideByName = false;
			bool flag2 = !this.GetSemanticChecker().CheckTypeAccess(typeCur, this._symWhere);
			if (flag2 && (this._csym != 0 || this._swtInaccess != null))
			{
				return false;
			}
			Symbol symbol = this.GetSymbolLoader().LookupAggMember(this._name, typeCur.getAggregate(), symbmask_t.MASK_ALL);
			while (symbol != null)
			{
				SYMKIND kind = symbol.getKind();
				if (kind != SYMKIND.SK_AggregateSymbol)
				{
					if (kind == SYMKIND.SK_MethodSymbol)
					{
						if (this._arity <= 0 || ((MethodSymbol)symbol).typeVars.Count == this._arity)
						{
							goto IL_011A;
						}
						if (!this._swtBadArity)
						{
							this._swtBadArity.Set(symbol, typeCur);
						}
					}
					else
					{
						if (this._arity <= 0)
						{
							goto IL_011A;
						}
						if (!this._swtBadArity)
						{
							this._swtBadArity.Set(symbol, typeCur);
						}
					}
				}
				else
				{
					if (((AggregateSymbol)symbol).GetTypeVars().Count == this._arity)
					{
						goto IL_011A;
					}
					if (!this._swtBadArity)
					{
						this._swtBadArity.Set(symbol, typeCur);
					}
				}
				IL_04DB:
				symbol = this.GetSymbolLoader().LookupNextSym(symbol, typeCur.getAggregate(), symbmask_t.MASK_ALL);
				continue;
				IL_011A:
				if (symbol.IsOverride() && !symbol.IsHideByName())
				{
					if (!this._swtOverride)
					{
						this._swtOverride.Set(symbol, typeCur);
						goto IL_04DB;
					}
					goto IL_04DB;
				}
				else
				{
					MethodOrPropertySymbol methodOrPropertySymbol = symbol as MethodOrPropertySymbol;
					MethodSymbol methodSymbol = symbol as MethodSymbol;
					if (methodOrPropertySymbol != null && (this._flags & MemLookFlags.UserCallable) != MemLookFlags.None && !methodOrPropertySymbol.isUserCallable() && methodSymbol != null && methodSymbol.isPropertyAccessor() && ((symbol.name.Text.StartsWith("set_", StringComparison.Ordinal) && methodSymbol.Params.Count > 1) || (symbol.name.Text.StartsWith("get_", StringComparison.Ordinal) && methodSymbol.Params.Count > 0)))
					{
						if (!this._swtInaccess)
						{
							this._swtInaccess.Set(symbol, typeCur);
							goto IL_04DB;
						}
						goto IL_04DB;
					}
					else if (flag2 || !this.GetSemanticChecker().CheckAccess(symbol, typeCur, this._symWhere, this._typeQual))
					{
						if (!this._swtInaccess)
						{
							this._swtInaccess.Set(symbol, typeCur);
						}
						if (flag2)
						{
							return false;
						}
						goto IL_04DB;
					}
					else
					{
						PropertySymbol propertySymbol = symbol as PropertySymbol;
						FieldSymbol fieldSymbol;
						if ((this._flags & MemLookFlags.Ctor) == MemLookFlags.None != (methodSymbol == null || !methodSymbol.IsConstructor()) || (this._flags & MemLookFlags.Operator) == MemLookFlags.None != (methodSymbol == null || !methodSymbol.isOperator) || (this._flags & MemLookFlags.Indexer) == MemLookFlags.None != !(propertySymbol is IndexerSymbol))
						{
							if (!this._swtBad)
							{
								this._swtBad.Set(symbol, typeCur);
								goto IL_04DB;
							}
							goto IL_04DB;
						}
						else if (!(symbol is MethodSymbol) && (this._flags & MemLookFlags.Indexer) == MemLookFlags.None && CSemanticChecker.CheckBogus(symbol))
						{
							if (!this._swtBogus)
							{
								this._swtBogus.Set(symbol, typeCur);
								goto IL_04DB;
							}
							goto IL_04DB;
						}
						else if ((this._flags & MemLookFlags.MustBeInvocable) != MemLookFlags.None && (((fieldSymbol = symbol as FieldSymbol) != null && !this.IsDelegateType(fieldSymbol.GetType(), typeCur) && !this.IsDynamicMember(symbol)) || (propertySymbol != null && !this.IsDelegateType(propertySymbol.RetType, typeCur) && !this.IsDynamicMember(symbol))))
						{
							if (!this._swtBad)
							{
								this._swtBad.Set(symbol, typeCur);
								goto IL_04DB;
							}
							goto IL_04DB;
						}
						else
						{
							if (methodOrPropertySymbol != null)
							{
								MethPropWithType methPropWithType = new MethPropWithType(methodOrPropertySymbol, typeCur);
								this._methPropWithTypeList.Add(methPropWithType);
							}
							flag = true;
							if (this._swtFirst)
							{
								if (!typeCur.isInterfaceType())
								{
									if (!this._fMulti)
									{
										if (this._swtFirst.Sym is FieldSymbol && symbol is EventSymbol && this._swtFirst.Field().isEvent)
										{
											goto IL_04DB;
										}
										if (this._swtFirst.Sym is FieldSymbol && symbol is EventSymbol)
										{
											goto IL_04DB;
										}
									}
									else
									{
										if (this._swtFirst.Sym.getKind() == symbol.getKind())
										{
											goto IL_04C5;
										}
										if (typeCur != this._prgtype[0])
										{
											pfHideByName = true;
											goto IL_04DB;
										}
									}
								}
								else if (!this._fMulti)
								{
									if (symbol is MethodSymbol)
									{
										this._swtAmbigWarn = this._swtFirst;
										this._prgtype = new List<AggregateType>();
										this._csym = 0;
										this._swtFirst.Clear();
										this._swtAmbig.Clear();
										goto IL_04C5;
									}
								}
								else
								{
									if (this._swtFirst.Sym.getKind() != symbol.getKind())
									{
										if (!typeCur.fDiffHidden)
										{
											if (!(this._swtFirst.Sym is MethodSymbol))
											{
												goto IL_04F8;
											}
											if (!this._swtAmbigWarn)
											{
												this._swtAmbigWarn.Set(symbol, typeCur);
											}
										}
										pfHideByName = true;
										goto IL_04DB;
									}
									goto IL_04C5;
								}
								IL_04F8:
								if (!this._swtAmbig)
								{
									this._swtAmbig.Set(symbol, typeCur);
								}
								pfHideByName = true;
								return true;
							}
							IL_04C5:
							this.RecordType(typeCur, symbol);
							if (methodOrPropertySymbol != null && methodOrPropertySymbol.isHideByName)
							{
								pfHideByName = true;
								goto IL_04DB;
							}
							goto IL_04DB;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00012A60 File Offset: 0x00010C60
		private bool IsDynamicMember(Symbol sym)
		{
			DynamicAttribute dynamicAttribute = null;
			FieldSymbol fieldSymbol;
			if ((fieldSymbol = sym as FieldSymbol) != null)
			{
				if (!fieldSymbol.getType().isPredefType(PredefinedType.PT_OBJECT))
				{
					return false;
				}
				object[] array = fieldSymbol.AssociatedFieldInfo.GetCustomAttributes(typeof(DynamicAttribute), false).ToArray<object>();
				if (array.Length == 1)
				{
					dynamicAttribute = array[0] as DynamicAttribute;
				}
			}
			else
			{
				PropertySymbol propertySymbol = (PropertySymbol)sym;
				if (!propertySymbol.getType().isPredefType(PredefinedType.PT_OBJECT))
				{
					return false;
				}
				object[] array2 = propertySymbol.AssociatedPropertyInfo.GetCustomAttributes(typeof(DynamicAttribute), false).ToArray<object>();
				if (array2.Length == 1)
				{
					dynamicAttribute = array2[0] as DynamicAttribute;
				}
			}
			return dynamicAttribute != null && (dynamicAttribute.TransformFlags.Count == 0 || (dynamicAttribute.TransformFlags.Count == 1 && dynamicAttribute.TransformFlags[0]));
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00012B30 File Offset: 0x00010D30
		private bool LookupInClass(AggregateType typeStart, ref AggregateType ptypeEnd)
		{
			AggregateType aggregateType = ptypeEnd;
			AggregateType aggregateType2 = typeStart;
			while (aggregateType2 != aggregateType && aggregateType2 != null)
			{
				bool flag = false;
				this.SearchSingleType(aggregateType2, out flag);
				if (this._swtFirst && !this._fMulti)
				{
					return false;
				}
				if (flag)
				{
					ptypeEnd = null;
					return true;
				}
				if ((this._flags & MemLookFlags.Ctor) != MemLookFlags.None)
				{
					return false;
				}
				aggregateType2 = aggregateType2.GetBaseClass();
			}
			return true;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00012B8C File Offset: 0x00010D8C
		private bool LookupInInterfaces(AggregateType typeStart, TypeArray types)
		{
			if (typeStart != null)
			{
				typeStart.fAllHidden = false;
				typeStart.fDiffHidden = this._swtFirst != null;
			}
			for (int i = 0; i < types.Count; i++)
			{
				AggregateType aggregateType = (AggregateType)types[i];
				aggregateType.fAllHidden = false;
				aggregateType.fDiffHidden = this._swtFirst;
			}
			bool flag = false;
			AggregateType aggregateType2 = typeStart;
			int num = 0;
			if (aggregateType2 == null)
			{
				aggregateType2 = (AggregateType)types[num++];
			}
			for (;;)
			{
				bool flag2 = false;
				if (!aggregateType2.fAllHidden && this.SearchSingleType(aggregateType2, out flag2))
				{
					flag2 |= !this._fMulti;
					TypeArray ifacesAll = aggregateType2.GetIfacesAll();
					for (int j = 0; j < ifacesAll.Count; j++)
					{
						AggregateType aggregateType3 = (AggregateType)ifacesAll[j];
						if (flag2)
						{
							aggregateType3.fAllHidden = true;
						}
						aggregateType3.fDiffHidden = true;
					}
					if (flag2)
					{
						flag = true;
					}
				}
				if (num >= types.Count)
				{
					break;
				}
				aggregateType2 = types[num++] as AggregateType;
			}
			return !flag;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00012C92 File Offset: 0x00010E92
		private SymbolLoader GetSymbolLoader()
		{
			return this._pSymbolLoader;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00012C9A File Offset: 0x00010E9A
		private CSemanticChecker GetSemanticChecker()
		{
			return this._pSemanticChecker;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00012CA2 File Offset: 0x00010EA2
		private ErrorHandling GetErrorContext()
		{
			return this.GetSymbolLoader().GetErrorContext();
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00012CB0 File Offset: 0x00010EB0
		private RuntimeBinderException ReportBogus(SymWithType swt)
		{
			MethodSymbol getterMethod = swt.Prop().GetterMethod;
			MethodSymbol setterMethod = swt.Prop().SetterMethod;
			if (!((getterMethod == null) | (setterMethod == null)))
			{
				return this.GetErrorContext().Error(ErrorCode.ERR_BindToBogusProp2, new ErrArg[]
				{
					swt.Sym.name,
					new SymWithType(getterMethod, swt.GetType()),
					new SymWithType(setterMethod, swt.GetType()),
					new ErrArgRefOnly(swt.Sym)
				});
			}
			return this.GetErrorContext().Error(ErrorCode.ERR_BindToBogusProp1, new ErrArg[]
			{
				swt.Sym.name,
				new SymWithType(getterMethod ?? setterMethod, swt.GetType()),
				new ErrArgRefOnly(swt.Sym)
			});
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00012D90 File Offset: 0x00010F90
		private bool IsDelegateType(CType pSrcType, AggregateType pAggType)
		{
			return this.GetSymbolLoader().GetTypeManager().SubstType(pSrcType, pAggType, pAggType.GetTypeArgsAll())
				.isDelegateType();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00012DB0 File Offset: 0x00010FB0
		public MemberLookup()
		{
			this._methPropWithTypeList = new List<MethPropWithType>();
			this._rgtypeStart = new List<AggregateType>();
			this._swtFirst = new SymWithType();
			this._swtAmbig = new SymWithType();
			this._swtInaccess = new SymWithType();
			this._swtBad = new SymWithType();
			this._swtBogus = new SymWithType();
			this._swtBadArity = new SymWithType();
			this._swtAmbigWarn = new SymWithType();
			this._swtOverride = new SymWithType();
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00012E34 File Offset: 0x00011034
		public bool Lookup(CSemanticChecker checker, CType typeSrc, Expr obj, ParentSymbol symWhere, Name name, int arity, MemLookFlags flags)
		{
			this._prgtype = this._rgtypeStart;
			this._pSemanticChecker = checker;
			this._pSymbolLoader = checker.SymbolLoader;
			this._typeSrc = typeSrc;
			this._obj = ((obj is ExprClass) ? null : obj);
			this._symWhere = symWhere;
			this._name = name;
			this._arity = arity;
			this._flags = flags;
			this._typeQual = (((this._flags & MemLookFlags.Ctor) != MemLookFlags.None) ? this._typeSrc : ((obj != null) ? obj.Type : null));
			AggregateType aggregateType = null;
			AggregateType aggregateType2 = null;
			TypeArray typeArray = BSYMMGR.EmptyTypeArray();
			AggregateType aggregateType3 = null;
			if (!typeSrc.isInterfaceType())
			{
				aggregateType = (AggregateType)typeSrc;
				if (aggregateType.IsWindowsRuntimeType())
				{
					typeArray = aggregateType.GetWinRTCollectionIfacesAll(this.GetSymbolLoader());
				}
			}
			else
			{
				aggregateType2 = (AggregateType)typeSrc;
				typeArray = aggregateType2.GetIfacesAll();
			}
			if (aggregateType2 != null || typeArray.Count > 0)
			{
				aggregateType3 = this.GetSymbolLoader().GetPredefindType(PredefinedType.PT_OBJECT);
			}
			if ((aggregateType == null || this.LookupInClass(aggregateType, ref aggregateType3)) && (aggregateType2 != null || typeArray.Count > 0) && this.LookupInInterfaces(aggregateType2, typeArray) && aggregateType3 != null)
			{
				AggregateType aggregateType4 = null;
				this.LookupInClass(aggregateType3, ref aggregateType4);
			}
			this._results = new CMemberLookupResults(this.GetAllTypes(), this._name);
			return !this.FError();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00012F6B File Offset: 0x0001116B
		public CMemberLookupResults GetResults()
		{
			return this._results;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00012F73 File Offset: 0x00011173
		private bool FError()
		{
			return !this._swtFirst || this._swtAmbig;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00012F8F File Offset: 0x0001118F
		public Symbol SymFirst()
		{
			return this._swtFirst.Sym;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00012F9C File Offset: 0x0001119C
		public SymWithType SwtFirst()
		{
			return this._swtFirst;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00012FA4 File Offset: 0x000111A4
		public Expr GetObject()
		{
			return this._obj;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00012FAC File Offset: 0x000111AC
		public CType GetSourceType()
		{
			return this._typeSrc;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00012FB4 File Offset: 0x000111B4
		public MemLookFlags GetFlags()
		{
			return this._flags;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00012FBC File Offset: 0x000111BC
		private TypeArray GetAllTypes()
		{
			return this.GetSymbolLoader().getBSymmgr().AllocParams(this._prgtype.Count, this._prgtype.ToArray());
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00012FE4 File Offset: 0x000111E4
		public Exception ReportErrors()
		{
			if (this._swtFirst)
			{
				return this.GetErrorContext().Error(ErrorCode.ERR_AmbigMember, new ErrArg[] { this._swtFirst, this._swtAmbig });
			}
			if (this._swtInaccess)
			{
				if (this._swtInaccess.Sym.isUserCallable() || (this._flags & MemLookFlags.UserCallable) == MemLookFlags.None)
				{
					return this.GetSemanticChecker().ReportAccessError(this._swtInaccess, this._symWhere, this._typeQual);
				}
				return this.GetErrorContext().Error(ErrorCode.ERR_CantCallSpecialMethod, new ErrArg[] { this._swtInaccess });
			}
			else if ((this._flags & MemLookFlags.Ctor) != MemLookFlags.None)
			{
				if (this._arity <= 0)
				{
					return this.GetErrorContext().Error(ErrorCode.ERR_NoConstructors, new ErrArg[] { this._typeSrc.getAggregate() });
				}
				return this.GetErrorContext().Error(ErrorCode.ERR_BadCtorArgCount, new ErrArg[]
				{
					this._typeSrc.getAggregate(),
					this._arity
				});
			}
			else
			{
				if ((this._flags & MemLookFlags.Operator) != MemLookFlags.None)
				{
					return this.GetErrorContext().Error(ErrorCode.ERR_NoSuchMember, new ErrArg[] { this._typeSrc, this._name });
				}
				if ((this._flags & MemLookFlags.Indexer) != MemLookFlags.None)
				{
					return this.GetErrorContext().Error(ErrorCode.ERR_BadIndexLHS, new ErrArg[] { this._typeSrc });
				}
				if (this._swtBad)
				{
					return this.GetErrorContext().Error(((this._flags & MemLookFlags.MustBeInvocable) != MemLookFlags.None) ? ErrorCode.ERR_NonInvocableMemberCalled : ErrorCode.ERR_CantCallSpecialMethod, new ErrArg[] { this._swtBad });
				}
				if (this._swtBogus)
				{
					return this.ReportBogus(this._swtBogus);
				}
				if (!this._swtBadArity)
				{
					return this.GetErrorContext().Error(ErrorCode.ERR_NoSuchMember, new ErrArg[] { this._typeSrc, this._name });
				}
				SYMKIND kind = this._swtBadArity.Sym.getKind();
				if (kind == SYMKIND.SK_AggregateSymbol)
				{
					int num = ((AggregateSymbol)this._swtBadArity.Sym).GetTypeVars().Count;
					return this.GetErrorContext().Error((num > 0) ? ErrorCode.ERR_BadArity : ErrorCode.ERR_HasNoTypeVars, new ErrArg[]
					{
						this._swtBadArity,
						new ErrArgSymKind(this._swtBadArity.Sym),
						num
					});
				}
				if (kind == SYMKIND.SK_MethodSymbol)
				{
					int num = ((MethodSymbol)this._swtBadArity.Sym).typeVars.Count;
					return this.GetErrorContext().Error((num > 0) ? ErrorCode.ERR_BadArity : ErrorCode.ERR_HasNoTypeVars, new ErrArg[]
					{
						this._swtBadArity,
						new ErrArgSymKind(this._swtBadArity.Sym),
						num
					});
				}
				return this.GetErrorContext().Error(ErrorCode.ERR_TypeArgsNotAllowed, new ErrArg[]
				{
					this._swtBadArity,
					new ErrArgSymKind(this._swtBadArity.Sym)
				});
			}
		}

		// Token: 0x040003BD RID: 957
		private CSemanticChecker _pSemanticChecker;

		// Token: 0x040003BE RID: 958
		private SymbolLoader _pSymbolLoader;

		// Token: 0x040003BF RID: 959
		private CType _typeSrc;

		// Token: 0x040003C0 RID: 960
		private Expr _obj;

		// Token: 0x040003C1 RID: 961
		private CType _typeQual;

		// Token: 0x040003C2 RID: 962
		private ParentSymbol _symWhere;

		// Token: 0x040003C3 RID: 963
		private Name _name;

		// Token: 0x040003C4 RID: 964
		private int _arity;

		// Token: 0x040003C5 RID: 965
		private MemLookFlags _flags;

		// Token: 0x040003C6 RID: 966
		private CMemberLookupResults _results;

		// Token: 0x040003C7 RID: 967
		private readonly List<AggregateType> _rgtypeStart;

		// Token: 0x040003C8 RID: 968
		private List<AggregateType> _prgtype;

		// Token: 0x040003C9 RID: 969
		private int _csym;

		// Token: 0x040003CA RID: 970
		private readonly SymWithType _swtFirst;

		// Token: 0x040003CB RID: 971
		private readonly List<MethPropWithType> _methPropWithTypeList;

		// Token: 0x040003CC RID: 972
		private readonly SymWithType _swtAmbig;

		// Token: 0x040003CD RID: 973
		private readonly SymWithType _swtInaccess;

		// Token: 0x040003CE RID: 974
		private readonly SymWithType _swtBad;

		// Token: 0x040003CF RID: 975
		private readonly SymWithType _swtBogus;

		// Token: 0x040003D0 RID: 976
		private readonly SymWithType _swtBadArity;

		// Token: 0x040003D1 RID: 977
		private SymWithType _swtAmbigWarn;

		// Token: 0x040003D2 RID: 978
		private readonly SymWithType _swtOverride;

		// Token: 0x040003D3 RID: 979
		private bool _fMulti;
	}
}
